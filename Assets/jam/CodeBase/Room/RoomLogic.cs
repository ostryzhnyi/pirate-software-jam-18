using System;
using jam.CodeBase.Core;
using UnityEngine;

namespace jam.CodeBase.Room
{
    public class RoomLogic : MonoBehaviour
    {
        public FanAnimator FanAnimator;
        public TVAnimator TVAnimator;

        private void Awake()
        {
            G.Room = this;
        }

        private void OnDestroy()
        {
            G.Room = null;
        }
    }
}