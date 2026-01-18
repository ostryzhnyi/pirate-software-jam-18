using UnityEngine;
using TMPro;
using jam.CodeBase.Core;

public class TimeView: MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    
    private void Start()
    {
        G.StreamController.DaysController.OnTimeUpdated += UpdateTime;
    }

    private void UpdateTime(float time)
    {
        _text.text = $"{(int)(time/3600):00}:{(int)(time/60) % 60:00}";
    }
}
