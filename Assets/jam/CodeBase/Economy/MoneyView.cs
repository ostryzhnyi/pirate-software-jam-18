using System;
using DG.Tweening;
using jam.CodeBase.Core;
using jam.CodeBase.Utils;
using TMPro;
using UnityEngine;

namespace jam.CodeBase.Economy
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _money;

        private void OnEnable()
        {
            _money.SetText(((int)(G.Economy.CurrentMoney)).ToString());
            G.Economy.OnMoneyChanged += OnChange;
        }

        private void OnDisable()
        {
            G.Economy.OnMoneyChanged -= OnChange;
        }

        private void OnChange(float current)
        {
            _money.DOFloatNumber(G.Economy.CurrentMoney, .7f);
            _money.transform.DOPunchScale(Vector3.one * 0.1f, .2f);
        }
    }
}