using UnityEngine;

namespace jam.CodeBase.UI
{
    public class PointerBob : MonoBehaviour
    {
        public float amplitude = 0.1f;  
        public float frequency = 2f;   

        Vector3 _startPos;

        void Start()
        {
            _startPos = transform.localPosition;
        }

        void Update()
        {
            float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
            transform.localPosition = _startPos + new Vector3(0f, offsetY, 0f);
        }
    }
}