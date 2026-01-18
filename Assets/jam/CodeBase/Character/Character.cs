using System;
using System.Collections.Generic;
using System.Linq;
using jam.CodeBase.Character.Data;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Tags;
using jam.CodeBase.Tasks;
using UnityEngine;

namespace jam.CodeBase.Character
{
    public class Character
    {
        public event Action OnStressDie;
        public event Action OnHealthDie;
        
        public string Name => _entity.Get<NameTag>().Name;
        public string Description => _entity.Get<DescribeTag>().Description;
        public int Age => _entity.Get<CharacterTag>().Age;
        public bool IsDie => _saveData is { IsDie: true };

        public float CurrentHealth;
        public float CurrentStress;
        
        private CMSEntity _entity;
        private CharacterSaveData _saveData;

        private StatsModifierTag _modifierTag;
        
        public Character(CMSEntity entity, CharacterSaveData characterSaveData)
        {
            _entity = entity;
            _saveData = characterSaveData;

            if (_saveData != null)
            {
                CurrentHealth = _saveData.Health;
                CurrentStress = _saveData.Stress;
            }
            else
            {
                CurrentHealth = _entity.Get<StatsTag>().Health;
                CurrentStress = _entity.Get<StatsTag>().Stress;
            }

            _modifierTag = entity.Get<StatsModifierTag>();
        }

        public void ApplyStatsAfforded(StatsAfforded statsAfforded)
        {
            if (statsAfforded.StatsType == StatsType.Health)
            {
                ChangeHP(statsAfforded.Value, statsAfforded.Method);
            }
            else
            {
                ChangeStress(statsAfforded.Value, statsAfforded.Method);
            }
        }

        public void ChangeHP(float amount, StatsChangeMethod method)
        {
            if (method == StatsChangeMethod.Add)
            {
                CurrentHealth += amount;
            }
            else if (method == StatsChangeMethod.Remove)
            {
                CurrentHealth -= amount * _modifierTag.PainThreshold;

                if (CurrentStress <= 0)
                {
                    OnHealthDie?.Invoke();
                    _saveData.IsDie = true;
                }
            }

            Debug.LogError("Update HP to: " +  CurrentHealth);
            Save();
        }   
        
        public void ChangeStress(float amount, StatsChangeMethod method)
        {
            if (method == StatsChangeMethod.Add)
            {
                CurrentStress += amount;
            }
            else if (method == StatsChangeMethod.Remove)
            {
                CurrentStress -= amount * _modifierTag.StressResistance;
                
                if (CurrentStress <= 0)
                {
                    OnStressDie?.Invoke();
                    _saveData.IsDie = true;
                }
            }
            
            Debug.LogError("Update stress to: " +  CurrentHealth);
            Save();
        }

        private void Save()
        {
            var saveModel = G.Saves.Get<CharactersSaveModel>();
            var saveData = saveModel.Data;
            
            var currentCharacterSave = saveData.CharactersSaves
                .FirstOrDefault(s => s.CharacterName == Name);

            if (currentCharacterSave == null)
            {
                currentCharacterSave = new CharacterSaveData
                {
                    CharacterName = Name,
                };
                if (saveModel.Data.CharactersSaves == null)
                {
                    saveModel.Data.CharactersSaves = new List<CharacterSaveData>();
                }
                saveModel.Data.CharactersSaves.Add(currentCharacterSave);
            }

            currentCharacterSave.Health   = CurrentHealth;
            currentCharacterSave.Stress   = CurrentStress;

            saveModel.ForceSave();
        }

    }
}