using System;
using System.Linq;
using jam.CodeBase.Room;
using UnityEngine;

public enum TVAnimation
{
    WorldIsWatching,
    MusicTime,
    ChooseRedOrBlue,
    NewDonation,
    FoodTime,
    BrakeIt,
    SmokeTime,
    Day1,
    Day2,
    Day3,
    LastGame,
    BonusGame,
    ShowerTime,
    GameOver,
    CallFriend,
    ShootTime
}

[System.Serializable]
public struct TVType
{
    public TVAnimation anim;
    public Sprite sprite;
}

public class TVAnimator : MonoBehaviour
{
    [SerializeField] TVType[] tvTypes;
    [SerializeField] TV[] tvs;

    void Awake()
    {
        if (tvs == null || tvs.Length == 0)
            tvs = FindObjectsOfType<TV>();
    }

    public void Play(TVAnimation anim, float duration)
    {
        if (duration <= 0f) return;

        var typeIndex = Array.FindIndex(tvTypes, t => t.anim == anim);
        if (typeIndex < 0) return;

        var sprite = tvTypes[typeIndex].sprite;
        if (sprite == null) return;

        var existing = Array.Find(tvs, t => t != null && t.currentAnim == anim);
        if (existing != null)
        {
            existing.remainingTime = duration;
            return;
        }

        var free = Array.Find(tvs, t => t != null && t.currentAnim == null);
        if (free != null)
        {
            Assign(free, anim, sprite, duration);
            return;
        }

        TV minTime = null;
        var min = float.MaxValue;
        for (int i = 0; i < tvs.Length; i++)
        {
            var tv = tvs[i];
            if (tv == null) continue;
            if (tv.remainingTime < min)
            {
                min = tv.remainingTime;
                minTime = tv;
            }
        }

        if (minTime != null)
            Assign(minTime, anim, sprite, duration);
    }

    public void Stop(TVAnimation anim)
    {
        var tv = Array.Find(tvs, t => t != null && t.currentAnim == anim);
        if (tv == null) return;

        tv.currentAnim = null;
        tv.remainingTime = 0f;
        if (tv.renderer != null)
            tv.renderer.sprite = null;
    }

    void Assign(TV tv, TVAnimation anim, Sprite sprite, float duration)
    {
        tv.currentAnim = anim;
        tv.remainingTime = duration;
        if (tv.renderer != null)
            tv.renderer.sprite = sprite;
    }
}
