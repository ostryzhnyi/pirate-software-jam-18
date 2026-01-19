using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character.Data;
using jam.CodeBase.Core;
using jam.CodeBase.Stream.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace jam.CodeBase.Stream
{
    public class StreamSceneStarter : MonoBehaviour
    {
        private StreamController _streamController;

        public StreamController StreamController => _streamController;

        private async void Start()
        {
            G.StreamController.DaysController.OnDayEnded += OnDayChangedTransition;

            var characters = CMS.GetAll<CMSEntity>().Where(x => x.Is<CharacterTag>());
            var currentCharacter = characters.OrderBy(_ => Random.value).First();

            await UniTask.DelayFrame(2);
            G.StreamController.StartStream(currentCharacter);
            //
            await UniTask.Delay(2000);
            G.StreamController.OnCharActionExecuted(1);
            await UniTask.Delay(2000);
            //
            // await UniTask.Delay(5000);
            // G.StreamController.OnDonateReceived(100);
            // G.StreamController.OnDonateReceived(300);
            // G.StreamController.OnDonateReceived(2000);
            // G.StreamController.OnDonateReceived(1000);
            // G.StreamController.OnDonateReceived(100);
        }

        private void OnDestroy()
        {
            if(G.StreamController != null && G.StreamController.DaysController != null)
                G.StreamController.DaysController.OnDayEnded -= OnDayChangedTransition;
        }

        private void OnDayChangedTransition(int obj)
        {
            Debug.LogError($"Day {obj} ended");
            if (obj < 0)
                return;
            G.GlobalViewService.ShowView<DaysTransitionPopup>();
        }
    }
}