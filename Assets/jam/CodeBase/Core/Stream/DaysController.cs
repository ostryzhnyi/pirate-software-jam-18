namespace jam.CodeBase.Stream
{
    public class DaysController
    {
        public int CurrentDay {get; private set;}

        public void SetDay(int dayNumber)
        {
            CurrentDay = dayNumber;
        }
    }
}