using jam.CodeBase.Core.SavesGeneral;

namespace jam.CodeBase.Glitches
{
    public class GlitchesSaveModel : SaveModel<GlitchesSaveData>
    {
        protected override void SetDefault()
        {
            Data = new GlitchesSaveData()
            {
                TotalHarm = 0
            };
        }
    }

    public class GlitchesSaveData
    {
        public float TotalHarm;
    }
}