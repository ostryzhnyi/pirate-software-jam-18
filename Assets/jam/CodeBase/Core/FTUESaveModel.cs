using jam.CodeBase.Core.SavesGeneral;

namespace jam.CodeBase.Core
{
    public class FTUESaveModel : SaveModel<FTUESaveData>
    {
        protected override void SetDefault()
        {
           
        }
    }

    public class FTUESaveData
    {
        public bool ShowedChatMinigameFTUE;
    }
}