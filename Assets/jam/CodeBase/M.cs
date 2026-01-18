using System;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Tasks.DonateSystem;
using UnityEngine;

namespace jam.CodeBase
{
    public class M : MonoBehaviour
    {
        public DonateView DonateView;

        private void Awake()
        {
            G.Menu = this;
        }

        private void Start()
        {
            G.Donate.DonateExecuteProcess().Forget();
        }

        private void OnDestroy()
        {
            G.Menu = null;
        }
    }
}