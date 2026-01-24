using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Tasks.DonateSystem
{
    public class DonateHUDButton : MonoBehaviour
    {
        [SerializeField] private Image Outline;

        public void SetAmount(float amount, bool instant = false)
        {
            if(Outline != null && Outline.gameObject != null)
                Outline.DOFillAmount(amount, instant ? 0 : 1);
        }
    }
}