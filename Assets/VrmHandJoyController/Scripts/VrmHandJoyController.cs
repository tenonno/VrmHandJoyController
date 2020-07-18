﻿using UniRx;
using UnityEngine;
using Vox.Hands;
using VRM;

namespace VrmHandJoyController
{
    public sealed class VrmHandJoyController : MonoBehaviour
    {
        [SerializeField] private VRMMeta _model;
        [SerializeField] private HandPose[] _buttons;
        [SerializeField] HandPoseData _defaultHandPose;
        [SerializeField] private float _lerpCoefficient = 0.2f;
        [SerializeField] private bool _keepRootBonePosition = true;

        private HandController _leftHandController;
        private HandController _rightHandController;

        private HandPoseData _leftHandPoseData;
        private HandPoseData _rightHandPoseData;

        public bool IsConnectedLeftJoyCon { get; private set; } = false;
        public bool IsConnectedRightJoyCon { get; private set; } = false;

        private FingerPoseData LerpFingerPoseData(FingerPoseData a, FingerPoseData b, float t)
        {
            return new FingerPoseData
            {
                spread = Mathf.Lerp(a.spread, b.spread, t),
                muscle1 = Mathf.Lerp(a.muscle1, b.muscle1, t),
                muscle2 = Mathf.Lerp(a.muscle2, b.muscle2, t),
                muscle3 = Mathf.Lerp(a.muscle3, b.muscle3, t),
            };
        }

        private HandPoseData LerpHandPoseData(HandPoseData a, HandPoseData b, float t)
        {
            var result = new HandPoseData();

            for (var i = 0; i < HandPoseData.HumanFingerCount; i++)
            {
                result[i] = LerpFingerPoseData(a[i], b[i], t);
            }

            return result;
        }

        private void Start()
        {
            gameObject.AddComponent<JoyconManager>();

            _leftHandController = _model.gameObject.AddComponent<HandController>();
            _leftHandController.Hand = HandType.LeftHand;
            _rightHandController = _model.gameObject.AddComponent<HandController>();
            _rightHandController.Hand = HandType.RightHand;


            if (_keepRootBonePosition)
            {
                var renderer = _model.GetComponentInChildren<SkinnedMeshRenderer>();
                var initialPosition = renderer.rootBone.position;

                renderer.rootBone.ObserveEveryValueChanged(x => x.position).Skip(1).First().Subscribe(newPosition =>
                {
                    _model.transform.position += initialPosition - newPosition;
                });
            }
        }

        private void UpdateJoycons()
        {
            var targetLeftHandPoseData = _defaultHandPose;
            var targetRightHandPoseData = _defaultHandPose;

            var joycons = JoyconManager.Instance.j;
            if (joycons == null)
            {
                IsConnectedLeftJoyCon = false;
                IsConnectedRightJoyCon = false;
                return;
            }

            foreach (var joycon in joycons)
            {
                if (joycon.isLeft) IsConnectedLeftJoyCon = true;
                else IsConnectedRightJoyCon = true;

                foreach (var button in _buttons)
                {
                    if (!button.Enabled) continue;
                    if (!joycon.GetButton(button.Button)) continue;

                    if (joycon.isLeft)
                    {
                        targetLeftHandPoseData = button.Mix(targetLeftHandPoseData);
                    }
                    else
                    {
                        targetRightHandPoseData = button.Mix(targetRightHandPoseData);
                    }
                }
            }

            _leftHandPoseData = LerpHandPoseData(_leftHandPoseData, targetLeftHandPoseData, _lerpCoefficient);
            _rightHandPoseData = LerpHandPoseData(_rightHandPoseData, targetRightHandPoseData, _lerpCoefficient);

            _leftHandController.SetHandPose(ref _leftHandPoseData);
            _rightHandController.SetHandPose(ref _rightHandPoseData);
        }

        private void Update()
        {
            UpdateJoycons();
        }
    }
}
