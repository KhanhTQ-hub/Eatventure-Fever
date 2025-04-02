using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NPS.Loot
{
    public abstract class UIItem : MonoBehaviour
    {
        [SerializeField] protected Image imgIcon;
        [SerializeField] protected Image imgFrame;
        [SerializeField] protected Image imgIconRandom;
        [SerializeField] protected TextMeshProUGUI txtAmount;
        [SerializeField] protected Animator anim;
        [SerializeField] protected bool xAmount = true;

        public abstract void Set(object data);

        public virtual void SetShaking(bool shaking) { }

        protected void SetFrame(Sprite frame, bool nativeSize = false)
        {
            if (imgFrame != null)
            {
                imgFrame.sprite = frame;
                if (nativeSize)
                {
                    imgFrame.SetNativeSize();
                }
            }
        }

        protected void SetIcon(Sprite icon, bool nativeSize = false)
        {
            if (imgIcon != null)
            {
                imgIcon.sprite = icon;
                if (nativeSize)
                {
                    imgIcon.SetNativeSize();
                }
            }
        }

        public void SetIconRandom(Sprite icon, bool nativeSize = false)
        {
            if (imgIconRandom != null)
            {
                imgIconRandom.sprite = icon;
                if (nativeSize)
                {
                    imgIconRandom.SetNativeSize();
                }
            }
        }

        protected void SetAmount(string amount)
        {
            if (txtAmount != null)
            {
                txtAmount.text = amount;
            }
        }

        protected void IsRandom(bool check = false)
        {
            if(imgIcon) imgIcon.gameObject.SetActive(!check);
            if(imgIconRandom) imgIconRandom.gameObject.SetActive(check);
        }

        public virtual void SetAmountNoX() { }

        public abstract void ShowUIInfor();
    }
}