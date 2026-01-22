using System;
using UnityEngine;

namespace jam.CodeBase.Character.Data
{
    [Serializable]
    public class StatsTag : EntityComponentDefinition
    {
        public float Health;
        public float Stress;
        public float MaxStress;
        public float MinHP;
    }
    
    [Serializable]
    public class StatsModifierTag : EntityComponentDefinition
    {
        [Range(0.1f, 1.5f)] public float PainThreshold;
        [Range(0.1f, 1.5f)] public float StressResistance;

        public string GetStringPainThreshold()
        {
            if (PainThreshold >= 1.5f)
                return "Very low";
            else if(PainThreshold < 1.5f && PainThreshold >= 1.1f)
                return "Low";
            else if(PainThreshold < 1.1f && PainThreshold >= 0.9f)
                return "Normal";
            else if(PainThreshold < 0.9f && PainThreshold >= 0.6f)
                return "High";
            else
                return "Very high";
                
        }
        public string GetStringStressResistance()
        {
            if (StressResistance >= 1.5f)
                return "Very low";
            else if(StressResistance < 1.5f && StressResistance >= 1.1f)
                return "Low";
            else if(StressResistance < 1.1f && StressResistance >= 0.9f)
                return "Normal";
            else if(StressResistance < 0.9f && StressResistance >= 0.6f)
                return "High";
            else
                return "Very high";
        }
    }
    
    [Serializable]
    public class CharacterTag : EntityComponentDefinition
    {
        public int Age;
        public Texture2D Texture2D;
    }
}