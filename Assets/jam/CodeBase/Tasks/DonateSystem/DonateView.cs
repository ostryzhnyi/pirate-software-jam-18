using System.Collections.Generic;
using System.Linq;
using jam.CodeBase.Core;
using jam.CodeBase.Tasks.Interactors;
using UnityEngine;

namespace jam.CodeBase.Tasks.DonateSystem
{
    public class DonateView : MonoBehaviour
    {
        public List<DonateButton> DonateButtons = new  List<DonateButton>();
        
        [SerializeField] private DonateButton _prefab;
        [SerializeField] private Transform _content;
        
        public void Init(List<BaseTask> tasks)
        {
            DonateButtons.Clear();
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var baseTask in tasks)
            {
                var button = Instantiate(_prefab, _content);
                button.Init(baseTask, OnClick);
                DonateButtons.Add(button);
            }    
        }

        private void OnClick(DonateButton button)
        {
            Debug.LogError("Donate: ");
            
            G.Interactors.CallAll<IDonate>((d) => d.Donate(button.Task, button.Price));
        }

        public void LockButtons()
        {
            foreach (var donateButton in DonateButtons)
            {
                donateButton.Button.interactable = false;
            }
        }

        public DonateButton GetDonateButton(BaseTask task)
        {
            return DonateButtons.FirstOrDefault(d => d.Task == task);
        }
    }
}