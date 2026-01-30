using jam.CodeBase.Core.SavesGeneral;

namespace jam.CodeBase.Glitches
{
    public class GlitchesSaveModel : SaveModel<GlitchesSaveData>
    {
        protected override void SetDefault()
        {
            Data = new GlitchesSaveData
            {
                TotalHarm = 0,
                HandDrawnShown = false,
                ChromaticShown = false,
                GlitchShown = false,
                FlickerShown = false,
                FadeFirstShown = false,
                FadeLastShown = false
            };
        }
    }

    public class GlitchesSaveData
    {
        public float TotalHarm;

        public bool HandDrawnShown;
        public bool ChromaticShown;
        public bool GlitchShown;
        public bool FlickerShown;

        public bool FadeFirstShown;
        public bool FadeLastShown;
    }
}