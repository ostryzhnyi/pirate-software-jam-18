using System.Linq;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;
using UnityEngine;

namespace jam.CodeBase.Tasks.Interactors
{
    public class DonationHandlerInteractor : BaseInteractor, IDonate
    {
        public void Donate(BaseTask task, float cost)
        {
            Debug.LogError("Donate: " + cost + " to " + task.Name);
            G.Donate.Donates[task] += cost;

            var sum = G.Donate.Donates.Sum(d => d.Value);
            
            foreach (var item in G.Donate.Donates)
            {
                if(G.Donate.Donates[item.Key] > 0)
                {
                    var donateProportion = G.Donate.Donates[item.Key] / sum;
                    Debug.LogError(item.Key + " " + donateProportion);
                    G.Menu.DonateView.GetDonateButton(item.Key).UpdateProgress(donateProportion);
                }
                else
                    G.Menu.DonateView.GetDonateButton(item.Key).UpdateProgress(0);
            }
        }
    }
}