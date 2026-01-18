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

        public void StartStream(CMSEntity entity)
        {
            _daysController.SetDay(0);
            _chatController.InitializeData(entity, 0);
            _chatController.StartMessaging();
        }

        public void OnDonateReceived(int value)
        {
            _chatController.ShowDonateMessage(value);
        }

        public void OnCharActionExecuted(int actionType)
        {
            _chatController.ShowReactionMessage(actionType);
        }
    }
}