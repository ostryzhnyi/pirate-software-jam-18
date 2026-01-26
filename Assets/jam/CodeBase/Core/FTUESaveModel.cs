using jam.CodeBase.Core.SavesGeneral;

namespace jam.CodeBase.Core
{
    public class FTUESaveModel : SaveModel<FTUESaveData>
    {
        protected override void SetDefault()
        {
            Data = new FTUESaveData();
        }
    }

    public class FTUESaveData
    {
        public bool ShowedChatMinigameFTUE;
        public bool ShowedBetFTUE;
        public bool ShowedDonateFTUE;
    }
}