using Cysharp.Threading.Tasks;

namespace jam.CodeBase.Core
{
    public interface IGameplayLoaded
    {
        public UniTask OnLoaded(RunSaveModel runSaveModel);
    }
    
    public interface IGameplayUnloaded
    {
        public UniTask OnUnloaded();
    }
}