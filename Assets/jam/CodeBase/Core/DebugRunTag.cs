using System;
using System.Collections.Generic;

namespace jam.CodeBase.Core
{
    [Serializable]
    public class DebugRunTag : EntityComponentDefinition
    {
        public List<CMSEntityPfb> DebugTask;

        public int OverrideDay = -1;
        public int OverrideEconomy = -1;
        public int OverrideCurentDonate = -1;
        public int OverrideHelath = -1;
        public int OverrideTotalHarm = -1;
        public bool OnlyChat = false;
    }
}