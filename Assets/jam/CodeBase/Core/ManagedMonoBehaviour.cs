using UnityEngine;

namespace jam.CodeBase.Core
{
    public abstract class ManagedMonoBehaviour : MonoBehaviour
    {
        public void Update()
        {
            if (!G.IsPaused)
            {
                ManagedUpdate();
            }
        }

        protected virtual void ManagedUpdate(){}
    }
}