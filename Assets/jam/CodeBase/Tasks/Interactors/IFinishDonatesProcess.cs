namespace jam.CodeBase.Tasks.Interactors
{
    public interface IFinishDonatesProcess
    {
        public void OnFinishDonates(TaskDefinition taskDefinition, BaseTask task, float price);
    }
}