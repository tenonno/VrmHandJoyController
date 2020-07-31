using System;
using UniRx;
using UnityEngine;
using Vox.Hands;
using VRM;

namespace VrmHandJoyController
{
    public sealed class VrmHandJoyController : MonoBehaviour
    {
        [SerializeField] private VRMMeta _model;
        [SerializeField] HandPosePreset _defaultHandPose;
        [SerializeField] HandPosePreset[] _leftHandPoses;
        [SerializeField] HandPosePreset[] _rightHandPoses;
        [SerializeField] private float _lerpCoefficient = 0.2f;
        [SerializeField] private bool _keepRootBonePosition = true;
        [SerializeField] private HandSourceType _leftHandSource = HandSourceType.Left;
        [SerializeField] private HandSourceType _rightHandSource = HandSourceType.Right;

        private HandController _leftHandController;
        private HandController _rightHandController;

        private HandPoseData _leftHandPoseData;
        private HandPoseData _rightHandPoseData;

        private HandPoseData DefaultHandPose => _defaultHandPose?.HandPoseData ?? new HandPoseData();

        public bool IsConnectedLeftJoyCon { get; private set; }
        public bool IsConnectedRightJoyCon { get; private set; }

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
            var targetLeftHandPoseData = DefaultHandPose;
            var targetRightHandPoseData = DefaultHandPose;

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

                var handPoses = joycon.isLeft ? _leftHandPoses : _rightHandPoses;

                for (var i = 0; i < handPoses.Length; i++)
                {
                    if (!handPoses[i]) continue;
                    if (!joycon.GetButton((Joycon.Button)i)) continue;

                    if (joycon.isLeft)
                    {
                        targetLeftHandPoseData = handPoses[i].Mix(targetLeftHandPoseData);
                    }
                    else
                    {
                        targetRightHandPoseData = handPoses[i].Mix(targetRightHandPoseData);
                    }
                }
            }

            _leftHandPoseData = LerpHandPoseData(_leftHandPoseData, targetLeftHandPoseData, _lerpCoefficient);
            _rightHandPoseData = LerpHandPoseData(_rightHandPoseData, targetRightHandPoseData, _lerpCoefficient);

            switch (_leftHandSource)
            {
                case HandSourceType.None:
                    var defaultHandPose = DefaultHandPose;
                    _leftHandController.SetHandPose(ref defaultHandPose);
                    break;
                case HandSourceType.Left:
                    _leftHandController.SetHandPose(ref _leftHandPoseData);
                    break;
                case HandSourceType.Right:
                    _leftHandController.SetHandPose(ref _rightHandPoseData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (_rightHandSource)
            {
                case HandSourceType.None:
                    var defaultHandPose = DefaultHandPose;
                    _rightHandController.SetHandPose(ref defaultHandPose);
                    break;
                case HandSourceType.Left:
                    _rightHandController.SetHandPose(ref _leftHandPoseData);
                    break;
                case HandSourceType.Right:
                    _rightHandController.SetHandPose(ref _rightHandPoseData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            UpdateJoycons();
        }
    }
}
