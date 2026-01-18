using System;

namespace jam.CodeBase.Stream
{
    public class DaysController
    {
        public event Action<int> OnDayUpdated;

        public int CurrentDay { get; private set; }

        public void SetDay(int dayNumber)
        {
            CurrentDay = dayNumber;
            OnDayUpdated?.Invoke(CurrentDay);
        }
    }
}