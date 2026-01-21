using jam.CodeBase.Core;
using jam.CodeBase.Core.Interactors;
using jam.CodeBase.Tasks;
using jam.CodeBase.Tasks.Interactors;

namespace jam.CodeBase.Stream
{
    public class ChatDonationHandlerInteractor : BaseInteractor, IDonate
    {
        public void Donate(BaseTask task, float cost)
        {
            G.StreamController.OnDonateReceived((int)cost, task.Name);
        }
    }
}