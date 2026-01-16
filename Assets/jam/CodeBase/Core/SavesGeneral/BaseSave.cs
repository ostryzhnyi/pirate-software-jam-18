namespace jam.CodeBase.Core.SavesGeneral
{
    public abstract class BaseSave
    {
        public abstract void Initialize();
        public abstract void ForceSave();
        public abstract void Clear();
    }
}