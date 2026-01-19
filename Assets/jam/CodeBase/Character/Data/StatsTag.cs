using System;
using UnityEngine;

namespace jam.CodeBase.Character.Data
{
    [Serializable]
    public class StatsTag : EntityComponentDefinition
    {
        public float Health;
        public float Stress;
    }
    
    [Serializable]
    public class StatsModifierTag : EntityComponentDefinition
    {
        [Range(0.1f, 1.5f)] public float PainThreshold;
        [Range(0.1f, 1.5f)] public float StressResistance;

        public string GetStringPainThreshold()
        {
            return "High";
        }
        public string GetStringStressResistance()
        {
            return "High";
        }
    }
    
    [Serializable]
    public class CharacterTag : EntityComponentDefinition
    {
        public int Age;
        public GameObject Prefab;
    }
}