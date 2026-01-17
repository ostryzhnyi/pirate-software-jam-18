using System.Linq;
using jam.CodeBase.Character.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace jam.CodeBase.Stream
{
    public class StreamSceneStarter : MonoBehaviour
    {
        private StreamController _streamController;
        
        public StreamController StreamController => _streamController;

        private void Awake()
        {
            var characters = CMS.GetAll<CMSEntity>().Where(x => x.Is<CharacterTag>());
            var currentCharacter = characters.OrderBy(_ => Random.value).First();
            
            _streamController = new StreamController();
            _streamController.Initialize();
            _streamController.StartStream(currentCharacter);
            //
            // _streamController.OnCharActionExecuted(1);
            //
            //
            // _streamController.OnDonateReceived(100);
            // _streamController.OnDonateReceived(300);
            // _streamController.OnDonateReceived(2000);
            // _streamController.OnDonateReceived(1000);
            // _streamController.OnDonateReceived(100);
        }
    }
}