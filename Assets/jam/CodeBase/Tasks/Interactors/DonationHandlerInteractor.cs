using System.Collections.Generic;
using System.Linq;
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
            Debug.LogError("Donate: " + cost + " to " + task.Name);
            G.Donate.Donates[task] += cost;

            var sum = G.Donate.Donates.Sum(d => d.Value);
            var donateView = (G.Menu.ViewService.GetView<DonateView>() as DonateView);
            
            foreach (var item in G.Donate.Donates)
            {
                if(G.Donate.Donates[item.Key] > 0)
                {
                    var donateProportion = G.Donate.Donates[item.Key] / sum;
                    Debug.LogError(item.Key + " " + donateProportion);
                    donateView.GetDonateButton(item.Key).UpdateProgress(donateProportion);
                }
                else
                    donateView.GetDonateButton(item.Key).UpdateProgress(0);
            }
        }

        public void OnFinishDonates(TaskDefinition taskDefinition, BaseTask task, float price)
        {
            G.Menu.HUD.Donate.interactable = false;
            G.Menu.ViewService.HideView<DonateView>();
        }
    }
}