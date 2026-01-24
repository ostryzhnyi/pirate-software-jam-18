using UnityEngine;

namespace jam.CodeBase.Room
{
    public class TV : MonoBehaviour
    {
        public SpriteRenderer renderer;
        public TVAnimation? currentAnim;
        public float remainingTime;
    
        void Update()
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                if (remainingTime <= 0)
                {
                    currentAnim = null;
                    renderer.sprite = null;
                }
            }
        }
    }
}