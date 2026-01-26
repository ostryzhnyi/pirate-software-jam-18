using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public event Action<float> OnDonateProgressUpdated;
        public Dictionary<BaseTask, float> Donates;

        public TaskDefinition TaskDefinition;
        public List<BaseTask> BaseTasks;
        private float _donateProgress;
        public float DonateProgress
        {
            get => _donateProgress;
            set
            {
                _donateProgress = value;
                OnDonateProgressUpdated?.Invoke(value);
            }
        }

        public async UniTask DonateExecuteProcess()
        {
            if(G.FinishRun)
                return;
            
            var runSaveModel = G.Saves.Get<RunSaveModel>();
            runSaveModel.Data.CurrentDonateNumberInDay++;

            var tasks = GetRandomTaskList();
            runSaveModel.Data.CompletedTask.Add(tasks.Item3);
            runSaveModel.ForceSave();
            G.Interactors.CallAll<ITasksReceive>(t => t.TasksReceive(tasks.Item1, tasks.Item2));

            if (tasks.Item1.Description == "Play in Russian Roulette")
            {
                G.Room.TVAnimator.Play(TVAnimation.LastGame, (tasks.Item1.Duration + 10f));
            }
            TaskDefinition = tasks.Item1;
            BaseTasks = tasks.Item2;
            Donates = new Dictionary<BaseTask, float>();

            G.Menu.HUD.DonateNotification.Play(TaskDefinition.Description).Forget();

            foreach (var baseTask in tasks.Item2)
            {
                Donates.Add(baseTask, 0);
            }

            DonateProgress = 1;
            G.Menu.HUD.DonateHUDButton.SetAmount(1, true);
            float time = tasks.Item1.Duration;
            var economyTag = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();
            var donators = Random.Range(economyTag.DonatorsAmountMinMax.x, economyTag.DonatorsAmountMinMax.y);
            var fullDonate = (economyTag.BaseDonate * economyTag.DonateMultiplier.GetRandomRange()) +
                             G.BetController.MyBet * economyTag.BaseBetMultiplier;
            var oneDonate = fullDonate / donators;

            int totalSeconds = Mathf.CeilToInt(time);
            float duration = tasks.Item1.Duration;

            float remainingDonate = fullDonate;
            int currentSecond = 0;

            while (time > 0)
            {
                if(G.FinishRun)
                    return;

                if (duration / 2 > time)
                {
                    while (!G.Saves.Get<FTUESaveModel>().Data.ShowedDonateFTUE)
                    {
                        await UniTask.WaitForSeconds(1);
                    }
                }
                
                time--;
                G.Menu.HUD.DonateHUDButton.SetAmount(time / duration);
                DonateProgress = time / duration;
                if (currentSecond < totalSeconds && remainingDonate > 0f)
                {
              
                    
                    float donateAmount = oneDonate * economyTag.OneDonateRandMultiplier.GetRandomRange();

                    if (donateAmount > remainingDonate)
                        donateAmount = remainingDonate;

                    remainingDonate -= donateAmount;

                    try
                    {
                        var currentTargetDonate = GetCurrentTarget();

                        var task = BaseTasks.FirstOrDefault(t => t.TaskTarget == currentTargetDonate);
                        if (task != null)
                        {
                            G.Interactors.CallAll<IDonate>(d => d.Donate(task, donateAmount));
                        }
                        else
                        {
                            G.Interactors.CallAll<IDonate>(d => d.Donate(BaseTasks.GetRandom(), donateAmount));
                            UnityEngine.Debug.LogError(
                                $"Not found {currentTargetDonate} task for {TaskDefinition.Description}");
                        }
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError(e);
                        G.Interactors.CallAll<IDonate>(d => d.Donate(BaseTasks.GetRandom(), donateAmount));
                    }

                    currentSecond++;
                }

                await UniTask.WaitForSeconds(1f);
            }

            // if (remainingDonate > 0f)
            // {
            //     try
            //     {
            //         var currentTargetDonate = GetCurrentTarget();
            //         var task = BaseTasks.FirstOrDefault(t => t.TaskTarget == currentTargetDonate);
            //         if (task != null)
            //         {
            //             G.Interactors.CallAll<IDonate>(d => d.Donate(task, remainingDonate));
            //         }
            //         else
            //         {
            //             G.Interactors.CallAll<IDonate>(d => d.Donate(BaseTasks.GetRandom(), remainingDonate));
            //             UnityEngine.Debug.LogError(
            //                 $"Not found final target for {TaskDefinition.Description}");
            //         }
            //     }
            //     catch (Exception e)
            //     {
            //         UnityEngine.Debug.LogError(e);
            //         G.Interactors.CallAll<IDonate>(d => d.Donate(BaseTasks.GetRandom(), remainingDonate));
            //     }
            // }

            var wonTask = Donates.OrderByDescending(p => p.Value).FirstOrDefault();

            G.Interactors.CallAll<IFinishDonatesProcess>(t =>
            t.OnFinishDonates(tasks.Item1, wonTask.Key, wonTask.Value));
            G.Menu.HUD.DonateHUDButton.SetAmount(0);
            DonateProgress = 0;

            G.Menu.HUD.FinishDonateNotification.Play(wonTask.Key.Name).Forget();
            await UniTask.WaitForSeconds(1);
            
            await wonTask.Key.Execute();

            foreach (var statsAfforded in wonTask.Key.StatsAfforded)
            {
                G.Characters.CurrentCharacter.ApplyStatsAfforded(statsAfforded);
            }

            G.Room.TVAnimator.Play(TVAnimation.SmokeTime, 3f);
            
            G.CharacterAnimator.PlayAnimation(AnimationType.Smoking);
            await UniTask.WaitForSeconds(2);
        }


        public (TaskDefinition, List<BaseTask>, string) GetRandomTaskList()
        {
            var runSave = G.Saves.Get<RunSaveModel>().Data;
            CMSEntity randTask = null;
            Debug.LogError("CURRENT DAY: " + runSave.DayNumber + " CURRENT CurrentDonateNumberInDay" +
                           runSave.CurrentDonateNumberInDay);
            if (runSave.DayNumber >= 3 && runSave.CurrentDonateNumberInDay >= 3)
            {
                Debug.LogError("RussianRoulette TASK");

                randTask = GameResources.CMS.Tasks.RussianRoulette.AsEntity();
            }
            else
            {
                List<CMSEntity> tasks;
                if (GameResources.CMS.DebugRun.AsEntity().Is<DebugRunTag>(out var tag) && tag.DebugTask != null 
                    && tag.DebugTask.Any())
                {
                    tasks = tag.DebugTask.Select(d => d.AsEntity()).ToList();
                }
                else
                {
                    tasks = GetTasks(runSave);

                    if (tasks.Count == 0)
                    {
                        Debug.LogError("NOT HAVE TASK");
                        runSave.CompletedTask.Clear();

                        tasks = GetTasks(runSave);

                    }
                }

                randTask = tasks[Random.Range(0, tasks.Count)];
            }


            var taskDefinition = randTask.Get<TaskDefinition>();
            var baseTasks = randTask.components.OfType<BaseTask>().ToList();

            return (taskDefinition, baseTasks, randTask.id);
        }

        private static List<CMSEntity> GetTasks(RunSaveData runSave)
        {
            return CMS.GetAll<CMSEntity>()
                .Where(e => e.Is<TaskDefinition>())
                .Where(e => !e.Is<PlayRussianRoulette>())
                .Where(e => !e.Is<IgnoreTag>())
                .Where(e => !runSave.CompletedTask.Contains(e.id))
                .Where(e => !e.Is<RequireItem>() || runSave.ObtainedItems.Contains(e.Get<RequireItem>().ItemName))
                .Where(e => G.DaysController.CurrentDay != 3 || e.id != "CMS/Tasks/GiftSleepPils")
                .ToList();
        }

        private TaskTarget GetCurrentTarget()
        {
            var economyTag = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();
            var isAlive = G.BetController.MyBetAlive > G.BetController.MyBetDie;
            var oppositeChance = economyTag.DonateСhanceOppositeByPlayer.GetRandomRange();
            var opposite = .5 < oppositeChance;

            Debug.Log("oppositeChance: " + oppositeChance);
            if (isAlive && !opposite)
                return TaskTarget.Live;
            else if (isAlive && opposite)
                return TaskTarget.Die;
            else if (!isAlive && opposite)
                return TaskTarget.Live;
            else
            {
                return TaskTarget.Die;
            }
        }
    }
}