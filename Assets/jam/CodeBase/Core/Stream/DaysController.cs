using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core;
using jam.CodeBase.Stream.View;
using UnityEngine;

namespace jam.CodeBase.Stream
{
    public class DaysController
    {
        public event Action<float> OnTimeUpdated;
        public event Action<int> OnDayEnded;

        private const float DAY_TIME = 24;//hours
        private const float DAY_REALTIME = 1f;//minutes
        private const float DAY_SENONDS_PER_REAL_SECOND = (DAY_TIME * 3600) / (DAY_REALTIME * 60);//seconds

        public event Action<int> OnDayUpdated;

        public int CurrentDay { get; private set; } = -1;
        public float CurrentTimeSeconds { get; private set; }

        private CancellationTokenSource _cst;

        public void Dispose()
        {
            _cst?.Cancel();
        }

        public async void SetDay(int dayNumber, bool isNext = false)
        {
            var runSaveModel = G.Saves.Get<RunSaveModel>();
            _cst = new CancellationTokenSource(); 
            Debug.LogError("SetDay: " +  dayNumber);

            OnDayEnded?.Invoke(CurrentDay);
            CurrentDay = dayNumber;
            OnDayUpdated?.Invoke(CurrentDay);
            runSaveModel.Data.CurrentDonateNumberInDay = 0;
            runSaveModel.Data.DayNumber = dayNumber;
            StartDayCycle(_cst.Token).Forget();
            
            var view =  G.GlobalViewService.GetView<DaysTransitionPopup>() as DaysTransitionPopup;
            view.Show();
            
            if(isNext)
                await view.SetupNextDay(dayNumber);
            else
            {
                await view.SetupCurrent(dayNumber);
            }
            G.Audio.SetLoopAndPlay($"Day{dayNumber}");
            runSaveModel.ForceSave();
        }

        private async UniTask StartDayCycle(CancellationToken token)
        {
            CurrentTimeSeconds = 0f;

            while (CurrentTimeSeconds < DAY_TIME * 3600f)
            {
                await UniTask.Delay(1000, cancellationToken: token);
                CurrentTimeSeconds += DAY_SENONDS_PER_REAL_SECOND;
                OnTimeUpdated?.Invoke(CurrentTimeSeconds);
            }

            OnDayEnded?.Invoke(CurrentDay);
            // SetDay(CurrentDay + 1);
        }
    }
}