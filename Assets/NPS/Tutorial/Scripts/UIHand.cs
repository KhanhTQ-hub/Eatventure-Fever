using Spine.Unity;
using UnityEngine;

namespace NPS.Tutorial
{
    public class UIHand : Hand
    {
        [SerializeField] private SkeletonGraphic ga;

#if UNITY_EDITOR
        private void OnValidate()
        {
            ga = hand.GetComponent<SkeletonGraphic>();
        }
#endif

        public override void Set(HandType type)
        {
            ga.Initialize(false);

            ga.AnimationState.SetAnimation(0, Constant.HandType2Anim[type], true);
        }
    }
}