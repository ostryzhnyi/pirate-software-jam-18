using System;

namespace jam.CodeBase.Core.Tags
{
    [Serializable]
    public class PriorityTag: EntityComponentDefinition
    {
        public int Priority = 0;
    }
}