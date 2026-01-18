using System;
using System.Collections.Generic;
using jam.CodeBase.Core.SavesGeneral;

namespace jam.CodeBase.Character.Data
{
    public class CharactersSaveModel : SaveModel<CharactersSaveData>
    {
        protected override void SetDefault()
        {
            Data = new CharactersSaveData();
            Data.CharactersSaves = new List<CharacterSaveData>();
        }
    }

    [Serializable]
    public class CharactersSaveData 
    {
        public List<CharacterSaveData> CharactersSaves { get; set; }
    }

    [Serializable]
    public class CharacterSaveData
    {
        public string CharacterName { get; set; }
        
        public float Health;
        public float Stress;
        public bool IsDie;
    }
}