using System.Linq;
using ProjectX.CodeBase.Utils;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    public class Donate
    {
        
        public void DonateExecute()
        {
            var tasks = CMS.GetAll<CMSEntity>()
                .Where(e => e.Is<TaskDefinition>())
                .ToList();

            var randTask = tasks[UnityEngine.Random.Range(0, tasks.Count)];;
            
            var baseTasks = randTask.components.OfType<BaseTask>().ToList();

            Debug.LogError(baseTasks[0].Name);
            Debug.LogError(baseTasks[1].Name);
            
            baseTasks[1].Execute();
        }
    }
}