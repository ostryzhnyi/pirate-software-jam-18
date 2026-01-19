using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.ViewLayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace jam.CodeBase.Character
{
    public class CharacterCardView : BaseOptionView<CharacterCardViewOptions>
    {
        public override ViewLayers Layer => ViewLayers.Popup;
        
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _desc;
        [SerializeField] private TMP_Text _age;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _stress;
        [SerializeField] private TMP_Text _stressResistance;
        [SerializeField] private TMP_Text _painThreashold;
        [SerializeField] private Image _image;

        protected override void Showed(ViewOption option = null)
        {
            base.Showed(option);

            var character = CastedOption.Character;
            
            _name.SetText(character.Name);
            _desc.SetText(character.Description);
            _age.SetText("Age: " +  character.Age);
            _health.SetText("Health: " + character.BaseHP);
            _stress.SetText("Stress: " +  character.Age);
            _painThreashold.SetText("Pain threshold: " +  character.ModifierTag.GetStringPainThreshold());
            _stressResistance.SetText("Stress resistance: " +  character.ModifierTag.GetStringStressResistance());
            _image.sprite = character.Preview;
        }
    }
    
    public class CharacterCardViewOptions : ViewOption
    {
        public Character Character;

        public CharacterCardViewOptions(Character character)
        {
            Character = character;
        }
    }
}