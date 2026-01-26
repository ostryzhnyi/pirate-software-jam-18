using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character;
using jam.CodeBase.Core;
using jam.CodeBase.Economy;
using jam.CodeBase.Stream.View;
using ProjectX.CodeBase.Utils;
using UnityEngine;

namespace jam.CodeBase.Stream
{
    public class DaysController
    {
        public event Action<float> OnTimeUpdated;
        public event Action<int> OnDayEnded;

        private const float DAY_TIME = 24; //hours
        private const float DAY_REALTIME = 1f; //minutes
        private const float DAY_SENONDS_PER_REAL_SECOND = (DAY_TIME * 3600) / (DAY_REALTIME * 60); //seconds

        public event Action<int> OnDayUpdated;

        public int CurrentDay { get; private set; } = -1;
        public float CurrentTimeSeconds { get; private set; }

        private CancellationTokenSource _cst;

        public void Dispose()
        {
            _cst?.Cancel();
        }

        public async UniTask SetDay(int dayNumber, bool isNext = false)
        {
            G.Room.TVAnimator.Stop(GetDeyAnim(dayNumber - 1));

            var runSaveModel = G.Saves.Get<RunSaveModel>();
            _cst = new CancellationTokenSource();
            Debug.LogError("SetDay: " + dayNumber);
            G.Room.TVAnimator.Play(GetDeyAnim(dayNumber), int.MaxValue);
            OnDayEnded?.Invoke(CurrentDay);
            CurrentDay = dayNumber;
            OnDayUpdated?.Invoke(CurrentDay);
            runSaveModel.Data.DayNumber = dayNumber;
            StartDayCycle(_cst.Token).Forget();

            var view = G.GlobalViewService.GetView<DaysTransitionPopup>() as DaysTransitionPopup;
            view.Show();

            if (isNext)
            {
                runSaveModel.Data.CurrentDonateNumberInDay = 0;
                
                await view.SetupNextDay(dayNumber);

                var economyTag = GameResources.CMS.BaseEconomy.As<BaseEconomyTag>();
                G.Characters.CurrentCharacter.ChangeHP(economyTag.RestoreHealthByDayRange.GetRandomRange(),
                    StatsChangeMethod.Add).Forget();
                G.Economy.AddMoney(economyTag.RestoreMoneyByDayRange.GetRandomRange());

                foreach (var music in G.Room.Music)
                {
                    music.Stop();
                }

                if (G.Room.Music.All(music => !music.isPlaying))
                {
                    G.Characters.CurrentCharacter.ChangeStress(economyTag.RestoreStressByDayRange.GetRandomRange(),
                        StatsChangeMethod.Remove).Forget();
                }
                else
                {
                    PlaySadAnim().Forget();
                }
                
                await UniTask.WaitForSeconds(2);
                G.Donate.DonateExecuteProcess().Forget();
            }
            else
            {
                await view.SetupCurrent(dayNumber);
            }

            G.Audio.SetLoopAndPlay($"Day{dayNumber}");
            runSaveModel.ForceSave();
        }

        private async UniTask PlaySadAnim()
        {
            G.CharacterAnimator.PlayAnimation(AnimationType.SetSad);
            await UniTask.WaitForSeconds(2);
            G.CharacterAnimator.DisableExpressions();
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

        private TVAnimation GetDeyAnim(int dayNumber)
        {
            switch (dayNumber)
            {
                case 1:
                    return TVAnimation.Day1;
                case 2:
                    return TVAnimation.Day2;
                case 3:
                    return TVAnimation.Day3;
            }

            return TVAnimation.Day1;
        }
    }
}