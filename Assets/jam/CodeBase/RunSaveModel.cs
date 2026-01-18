using System;
using System.Collections.Generic;
using jam.CodeBase.Core.SavesGeneral;

namespace jam.CodeBase
{
    public class RunSaveModel : SaveModel<RunSaveData>
    {
        protected override void SetDefault()
        {
            Data = new RunSaveData()
            {
                DayNumber = 1,
                CompletedTask = new List<string>()
            };
        }
    }

    [Serializable]
    public class RunSaveData
    {
        public int DayNumber;
        public List<string> CompletedTask;
        public string CharacterName;
    }
}