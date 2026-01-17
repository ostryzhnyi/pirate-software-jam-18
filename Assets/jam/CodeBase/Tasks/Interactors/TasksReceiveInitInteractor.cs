using System.Collections.Generic;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;

namespace jam.CodeBase.Tasks.Interactors
{
    public class TasksReceiveInitInteractor : BaseInteractor, ITasksReceive
    {
        public void TasksReceive(List<BaseTask> tasks)
        {
            G.Menu.DonateView.Init(tasks);
        }
    }
}