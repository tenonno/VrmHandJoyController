using System;
using UnityEngine;
using Vox.Hands;

namespace VrmHandJoyController
{
    [Serializable]
    public struct HandPose
    {
        [SerializeField] private Joycon.Button _button;
        [SerializeField] private bool _enabled;
        [SerializeField] private HandPoseData _handPose;
        [SerializeField] private bool[] _handPoseEnabledArray;

        public Joycon.Button Button => _button;
        public bool Enabled => _enabled;
        public HandPoseData HandPoseData => _handPose;

        public HandPoseData Mix(HandPoseData original)
        {
            for (var i = 0; i < HandPoseData.HumanFingerCount; i++)
            {
                if (_handPoseEnabledArray[i])
                {
                    original[i] = _handPose[i];
                }
            }

            return original;
        }
    }
}
