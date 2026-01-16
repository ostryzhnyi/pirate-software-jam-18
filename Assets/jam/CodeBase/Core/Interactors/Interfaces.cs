using Cysharp.Threading.Tasks;

namespace jam.CodeBase.Core.Interactors
{
    public interface IOnAwake
    {
        UniTask OnAwake();
    }
    
    public interface IOnStart
    {
        UniTask OnStart();
    }
    
    public interface IOnUpdate
    {
        UniTask OnUpdate();
    }
    
    public interface IOnDestroy
    {
        UniTask OnDestroy();
    }
}