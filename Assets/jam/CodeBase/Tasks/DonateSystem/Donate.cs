using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Tags;
using jam.CodeBase.Economy;
using jam.CodeBase.Tasks.Interactors;
using ProjectX.CodeBase.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace jam.CodeBase.Tasks
{
    public class Donate
    {
        public Dictionary<BaseTask, float> Donates;

        public TaskDefinition TaskDefinition;
        public List<BaseTask> BaseTasks;
        
        public async UniTask DonateExecuteProcess()
        {
            var runSaveModel = G.Saves.Get<RunSaveModel>();
            runSaveModel.Data.CurrentDonateNumberInDay++;
            
            var tasks = GetRandomTaskList();
            runSaveModel.Data.CompletedTask.Add(tasks.Item3);
            runSaveModel.ForceSave();
            G.Interactors.CallAll<ITasksReceive>(t => t.TasksReceive(tasks.Item1, tasks.Item2));
            TaskDefinition = tasks.Item1;
            BaseTasks = tasks.Item2;
            Donates = new Dictionary<BaseTask, float>();
            
            G.Menu.HUD.DonateNotification.Play(TaskDefinition.Description).Forget();
                
            foreach (var baseTask in tasks.Item2)
            {
                Donates.Add(baseTask, 0);
            }
            
            G.Menu.HUD.DonateHUDButton.SetAmount(1, true);
            float time = tasks.Item1.Duration;
            var economyTag = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();
            var donators = Random.Range(economyTag.DonatorsAmountMinMax.x, economyTag.DonatorsAmountMinMax.y);
            while (time > 0)
            {
                time--;
                G.Menu.HUD.DonateHUDButton.SetAmount( time / tasks.Item1.Duration);
                
                await UniTask.WaitForSeconds(1f);
                if(donators > 0)
                {
                    var donate = G.Economy.CurrentMoney *
                                 Random.Range(economyTag.AdditionalDonatorsMultiplierFromPlayerMoney.x,
                                     economyTag.AdditionalDonatorsMultiplierFromPlayerMoney.y)
                                 + Random.Range(economyTag.MinRandomDonateRange.x, economyTag.MinRandomDonateRange.y);

                    try
                    {
                        var currentTargetDonate = GetCurrentTarget();
                        var task = BaseTasks.FirstOrDefault(t => t.TaskTarget == currentTargetDonate);
                        if(task != null)
                            G.Interactors.CallAll<IDonate>(d => d.Donate(task, donate));
                        else
                        {
                            G.Interactors.CallAll<IDonate>(d => d.Donate(BaseTasks.GetRandom(), donate));
                            UnityEngine.Debug.LogError($"Not found {currentTargetDonate} task for {TaskDefinition.Description}");
                        }
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError(e);
                        G.Interactors.CallAll<IDonate>(d => d.Donate(BaseTasks.GetRandom(), donate));
                    }

                    donators--;
                }
            }

            var wonTask = Donates.OrderByDescending(p => p.Value).FirstOrDefault();
            
            G.Interactors.CallAll<IFinishDonatesProcess>(t => t.OnFinishDonates(tasks.Item1, wonTask.Key, wonTask.Value));
            G.Menu.HUD.DonateHUDButton.SetAmount(0);

            await wonTask.Key.Execute();

            foreach (var statsAfforded in wonTask.Key.StatsAfforded)
            {
                G.Characters.CurrentCharacter.ApplyStatsAfforded(statsAfforded);
            }
            
            G.CharacterAnimator.PlayAnimation(AnimationType.Smoking);
            await UniTask.WaitForSeconds(2);
        }
        
        public (TaskDefinition, List<BaseTask>, string) GetRandomTaskList()
        {
            var runSave = G.Saves.Get<RunSaveModel>().Data;
            CMSEntity randTask = null;
            Debug.LogError("CURRENT DAY: " +runSave.DayNumber + " CURRENT CurrentDonateNumberInDay" + runSave.CurrentDonateNumberInDay);
            if (runSave.DayNumber >= 3 && runSave.CurrentDonateNumberInDay >= 3)
            {
                Debug.LogError("RussianRoulette TASK");
                
                randTask = GameResources.CMS.Tasks.RussianRoulette.AsEntity();
            }
            else
            {
                var tasks = CMS.GetAll<CMSEntity>()
                    .Where(e => e.Is<TaskDefinition>())
                    .Where(e => !e.Is<PlayRussianRoulette>())
                    .Where(e => !e.Is<IgnoreTag>())
                    .Where(e => !runSave.CompletedTask.Contains(e.id))
                    .Where(e => !e.Is<RequireItem>() || runSave.ObtainedItems.Contains(e.Get<RequireItem>().ItemName))
                    .ToList();

                if (tasks.Count == 0)
                {
                    Debug.LogError("NOT HAVE TASK");
                    runSave.CompletedTask.Clear();
                
                    tasks = CMS.GetAll<CMSEntity>()
                        .Where(e => e.Is<TaskDefinition>())
                        .Where(e => !e.Is<PlayRussianRoulette>())
                        .Where(e => !e.Is<IgnoreTag>())
                        .Where(e => !runSave.CompletedTask.Contains(e.id))
                        .Where(e => !e.Is<RequireItem>() || runSave.ObtainedItems.Contains(e.Get<RequireItem>().ItemName))
                        .ToList();
                }

                randTask = tasks[UnityEngine.Random.Range(0, tasks.Count)];;
            }
           

            var taskDefinition = randTask.Get<TaskDefinition>();
            var baseTasks = randTask.components.OfType<BaseTask>().ToList();

            return (taskDefinition, baseTasks, randTask.id);
        }

        private TaskTarget GetCurrentTarget()
        {
            var chanceAlive = (G.BetController.AliveBetCoefficient - 1) * 100;
            var rand = Random.Range(0, 100);
            Debug.Log("chanceAlive: " + chanceAlive);
            Debug.Log("rand: " + rand);
            if (chanceAlive > rand)
                return TaskTarget.Live;
            else
                return TaskTarget.Die;
        }
    }
}