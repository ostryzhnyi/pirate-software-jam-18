using System;
using UnityEngine;

namespace jam.CodeBase.Glitches
{
    [Serializable]
    public class GlitchesTag : EntityComponentDefinition
    {
        public HandDrawn HandDrawnProportion;
        public ChromaticAberration ChromaticAberrationProportion;
        public Glitch GlitchProportion;
        public Flicker FlickerProportion;
        public Fade FadeProportion;
    }


    [Serializable]
    public class HandDrawn
    {
        public const string Param = "_HandDrawnAmount";
        
        public Vector2 TotalHarm = new Vector2(0, 200) ;
        public Vector2 HandDrawnAmount = new  Vector2(0, 4) ;
    }
    
    [Serializable]
    public class ChromaticAberration
    {
        public const string Param = "_ChromAberrAmount";
        
        public Vector2 TotalHarm = new Vector2(150, 250) ;
        public Vector2 Amount = new  Vector2(0, .2f) ;
    }
    
    [Serializable]
    public class Glitch
    {
        public const string Param = "_GlitchAmount";
        
        public Vector2 TotalHarm = new Vector2(200, 350) ;
        public Vector2 Amount = new Vector2(0, 15f) ;
    }
    
    [Serializable]
    public class Flicker
    {
        public const string Param = "_FlickerPercent";
        
        public Vector2 TotalHarm = new Vector2(200, 350) ;
        public Vector2 Percent = new Vector2(0, 15f) ;
    }
    
    [Serializable]
    public class Fade
    {
        public const string Param = "_FadeAmount";
        
        public Vector2 TotalHarm = new Vector2(500, 1500) ;
        public Vector2 Amount = new Vector2(0, 0.7f) ;
    }
}