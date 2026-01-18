using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Tasks.DonateSystem;
using jam.CodeBase.Tasks.Interactors;
using UnityEngine;

namespace jam.CodeBase.Tasks
{
    public class Donate
    {
        public Dictionary<BaseTask, float> Donates;

        public TaskDefinition TaskDefinition;
        public List<BaseTask> BaseTasks;
        
        
        public async UniTask DonateExecuteProcess()
        {
            var tasks = GetRandomTaskList();
            G.Interactors.CallAll<ITasksReceive>(t => t.TasksReceive(tasks.Item1, tasks.Item2));
            TaskDefinition = tasks.Item1;
            BaseTasks = tasks.Item2;

            Donates = new Dictionary<BaseTask, float>();

            foreach (var baseTask in tasks.Item2)
            {
                Donates.Add(baseTask, 0);
            }
            
            float time = tasks.Item1.Duration;
            while (time > 0)
            {
                time--;
                await UniTask.WaitForSeconds(1f);
            }

            var wonTask = Donates.OrderByDescending(p => p.Value).FirstOrDefault();
            G.Interactors.CallAll<IFinishDonatesProcess>(t => t.OnFinishDonates(tasks.Item1, wonTask.Key, wonTask.Value));

            await wonTask.Key.Execute();

            foreach (var statsAfforded in wonTask.Key.StatsAfforded)
            {
                G.Characters.CurrentCharacter.ApplyStatsAfforded(statsAfforded);
            }
        }
        
        public (TaskDefinition, List<BaseTask>) GetRandomTaskList()
        {
            Debug.LogError("G.Saves:" + G.Saves);
            Debug.LogError("G.Saves.Get<RunSaveModel>():" + G.Saves.Get<RunSaveModel>());
            var runSave = G.Saves.Get<RunSaveModel>().Data;
            
            var tasks = CMS.GetAll<CMSEntity>()
                .Where(e => e.Is<TaskDefinition>())
                .Where(e => !runSave.CompletedTask.Contains(e.id))
                .ToList();

            if (tasks.Count == 0)
            {
                Debug.LogError("NOT HAVE TASK");
            }

            var randTask = tasks[UnityEngine.Random.Range(0, tasks.Count)];;

            var taskDefinition = randTask.Get<TaskDefinition>();
            var baseTasks = randTask.components.OfType<BaseTask>().ToList();

            return (taskDefinition, baseTasks);
        }
    }
}