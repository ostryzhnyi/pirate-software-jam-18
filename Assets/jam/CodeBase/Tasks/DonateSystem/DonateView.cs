using System.Collections.Generic;
using jam.CodeBase.Core;
using jam.CodeBase.Tasks.Interactors;
using UnityEngine;

namespace jam.CodeBase.Tasks.DonateSystem
{
    public class DonateView : MonoBehaviour
    {
        [SerializeField] private DonateButton _prefab;
        [SerializeField] private Transform _content;
        [SerializeField] private List<DonateButton> _donateButton;
        
        public void Init(List<BaseTask> tasks)
        {
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var baseTask in tasks)
            {
                Instantiate(_prefab, transform).Init(baseTask, OnClick);
            }    
        }

        private void OnClick(BaseTask baseTask)
        {
            G.Interactors.CallAll<IDonate>((d) => d.Donate(baseTask, baseTask.Price));

            foreach (var VARIABLE in _donateButton)
            {
                
            }
        }
    }
}