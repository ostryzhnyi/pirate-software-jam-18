using System.Collections.Generic;

namespace jam.CodeBase.Tasks.Interactors
{
    public interface ITasksReceive
    {
        public void TasksReceive(List<BaseTask> tasks);
    }
}