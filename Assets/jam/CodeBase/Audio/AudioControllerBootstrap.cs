using System.Collections;
using UnityEngine;

namespace jam.CodeBase.Audio
{
    public class AudioControllerBootstrap : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(DelayedInit());
        }

        private IEnumerator DelayedInit()
        {
            yield return new WaitForSeconds(0.01f);
            var prefab = GameResources.AudioController;
            Instantiate(prefab);
            Destroy(gameObject);
        }
    }
}