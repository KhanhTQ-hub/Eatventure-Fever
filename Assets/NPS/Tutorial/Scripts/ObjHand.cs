using Spine.Unity;
using UnityEngine;

namespace NPS.Tutorial
{
    public class ObjHand : Hand
    {
        [SerializeField] private SkeletonAnimation sa;

#if UNITY_EDITOR
        private void OnValidate()
        {
            sa = hand.GetComponent<SkeletonAnimation>();
        }
#endif

        public override void Set(HandType type)
        {
            sa.Initialize(false);

            sa.AnimationState.SetAnimation(0, Constant.HandType2Anim[type], true);
        }
    }
}