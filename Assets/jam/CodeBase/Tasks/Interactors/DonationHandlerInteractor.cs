using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;
using jam.CodeBase.Tasks.DonateSystem;
using UnityEngine;

namespace jam.CodeBase.Tasks.Interactors
{
    public class DonationHandlerInteractor : BaseInteractor, IDonate, IFinishDonatesProcess, ITasksReceive
    {
        public void TasksReceive(TaskDefinition taskDefinition, List<BaseTask> tasks)
        {
            G.Menu.HUD.Donate.interactable = true;
        }
        
        public void Donate(BaseTask task, float cost)
        {
            Debug.Log("Donate: " + cost + " to " + task.Name);
            G.Donate.Donates[task] += cost;

            var sum = G.Donate.Donates.Sum(d => d.Value);
            var donateView = (G.Menu.ViewService.GetView<DonateView>() as DonateView);
            foreach (var item in G.Donate.Donates)
            {
                if(G.Donate.Donates[item.Key] > 0)
                {
                    var donateProportion = G.Donate.Donates[item.Key] / sum;
                    donateView.GetDonateButton(item.Key)?.UpdateProgress(donateProportion);
                }
                else
                {
                    donateView.GetDonateButton(item.Key)?.UpdateProgress(0);
                }
            }
        }

        public async UniTask OnFinishDonates(TaskDefinition taskDefinition, BaseTask task, float price)
        {
            var runSaveData = G.Saves.Get<RunSaveModel>().Data;
            G.Menu.HUD.Donate.interactable = false;
            G.Menu.ViewService.HideView<DonateView>();
            await UniTask.WaitForSeconds(5);

            var donateButtons = (G.Menu.ViewService.GetView<DonateView>() as DonateView)?.DonateButtons;
            if (donateButtons != null)
            {
                foreach (var donateButton in donateButtons)
                {
                    donateButton?.UpdateProgress(0);
                }
            }

            Debug.LogError("CurrentDonateNumberInDay: " +  runSaveData.CurrentDonateNumberInDay);
            if (runSaveData.CurrentDonateNumberInDay >= 3)
            {
                G.DaysController.SetDay(++runSaveData.DayNumber, true);
                await UniTask.WaitForSeconds(5);
                G.Donate.DonateExecuteProcess().Forget();
            }
            else
            {
                G.Donate.DonateExecuteProcess().Forget();
            }
        }
    }
}