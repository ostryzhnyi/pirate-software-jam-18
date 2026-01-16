using System;
using TMPro;
using UnityEngine;

namespace jam.CodeBase
{
    public class TimeStampBuild : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;
        
        [ContextMenu("Set")]
        public void Set()
        {
            _tmpText.SetText(DateTime.Now.ToShortDateString());
        }
    }
}
