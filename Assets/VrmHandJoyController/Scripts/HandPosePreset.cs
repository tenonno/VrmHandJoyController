using UnityEngine;
using Vox.Hands;

namespace VrmHandJoyController
{
    [CreateAssetMenu(menuName = "VrmHandJoyController/HandPosePreset")]
    public sealed class HandPosePreset : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private HandPoseData _handPose;
        [SerializeField] private bool[] _fingerEnabledArray;

        public HandPoseData HandPoseData => _handPose;

        public HandPoseData Mix(HandPoseData original)
        {
            for (var i = 0; i < HandPoseData.HumanFingerCount; i++)
            {
                if (_fingerEnabledArray[i])
                {
                    original[i] = _handPose[i];
                }
            }

            return original;
        }
    }
}
