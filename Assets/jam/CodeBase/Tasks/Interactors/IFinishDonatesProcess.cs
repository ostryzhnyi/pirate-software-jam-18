using Cysharp.Threading.Tasks;

namespace jam.CodeBase.Tasks.Interactors
{
    public interface IFinishDonatesProcess
    {
        public UniTask OnFinishDonates(TaskDefinition taskDefinition, BaseTask task, float price);
    }
}