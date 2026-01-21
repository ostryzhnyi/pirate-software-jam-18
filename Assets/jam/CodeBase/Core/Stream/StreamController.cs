using Cysharp.Threading.Tasks;

namespace jam.CodeBase.Stream
{
    public class StreamController
    {
        private DaysController _daysController;
        private ChatController _chatController;

        // todo: Add
        // private StreamRewards _streamRewards; // accumulative donates, Players bid and coefficient
        private CMSEntityPfb _currentEntity;
        public DaysController DaysController => _daysController;
        public ChatController ChatController => _chatController;

        public StreamController()
        {
            _daysController = new DaysController();
            _chatController = new ChatController();
        }

        public void Dispose()
        {
            _daysController.Dispose();
            _chatController.Dispose();
        }

        public void StartStream(CMSEntity entity)
        {
            _chatController.InitializeData(entity, 0);
            _chatController.StartMessaging().Forget();
        }

        public void OnDonateReceived(int value, string goal)
        {
            _chatController.ShowDonateMessage(value, goal);
        }

        public void OnCharActionExecuted(int actionType)
        {
            _chatController.ShowReactionMessage(actionType);
        }
    }
}