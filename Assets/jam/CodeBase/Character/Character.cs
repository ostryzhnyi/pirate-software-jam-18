using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Character.Data;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Tags;
using jam.CodeBase.Tasks;
using ProjectX.CodeBase.Utils;
using Runtime;
using UnityEngine;

namespace jam.CodeBase.Character
{
    public class Character
    {
        public event Action<float> OnStressUpdated;
        public event Action<float> OnHealthUpdated;
        public event Action<float> OnHealthChanges;
        public event Action<float> OnStressChanges;

        public string Name => Entity.Get<NameTag>().Name;
        public string Description => Entity.Get<DescribeTag>().Description;
        public int Age => Entity.Get<CharacterTag>().Age;
        public Sprite Preview => Entity.Get<TagSprite>().sprite;
        public Texture2D Texture => Entity.Get<CharacterTag>().Texture2D;
        public bool IsDie => IsDieStress() || IsDieHP();

        public float CurrentHealth;
        public float CurrentStress;
        public float BaseStress { private set; get; }
        public float BaseHP { private set; get; }
        public float MaxStress => Entity.Get<StatsTag>().MaxStress;

        public CMSEntity Entity;

        public StatsModifierTag ModifierTag { private set; get; }

        public Character(CMSEntity entity, CharacterSaveData characterSaveData)
        {
            Entity = entity;
            
            if (characterSaveData != null)
            {
                CurrentHealth = characterSaveData.Health;
                CurrentStress = characterSaveData.Stress;
            }
            else
            {
                CurrentHealth = Entity.Get<StatsTag>().Health;
                CurrentStress = Entity.Get<StatsTag>().Stress;

                Save();
            }

            BaseHP = Entity.Get<StatsTag>().Health;
            BaseStress = Entity.Get<StatsTag>().Stress;

            ModifierTag = entity.Get<StatsModifierTag>();
        }

        public void ApplyStatsAfforded(StatsAfforded statsAfforded)
        {
            if (statsAfforded.StatsType == StatsType.Health)
            {
                ChangeHP(statsAfforded.ValueRange.GetRandomRange(), statsAfforded.Method).Forget();
            }
            else
            {
                ChangeStress(statsAfforded.ValueRange.GetRandomRange(), statsAfforded.Method).Forget();
            }
        }

        public async UniTask ChangeHP(float amount, StatsChangeMethod method)
        {
            if (method == StatsChangeMethod.Add)
            {
                if (CurrentHealth + amount >= BaseHP)
                    CurrentHealth = BaseHP;
                else
                    CurrentHealth += amount;
            }
            else if (method == StatsChangeMethod.Remove)
            {
                CurrentHealth -= amount * ModifierTag.PainThreshold;
            }

            OnHealthUpdated?.Invoke(CurrentHealth);
            OnStressChanges?.Invoke(method == StatsChangeMethod.Remove ? amount : -amount);

            if (IsDieHP())
            {
                var dies = G.Interactors.GetAll<IDieHealthCharacter>();

                await UniTask.WaitForSeconds(1f);
                foreach (var dy in dies)
                {
                    await dy.OnDie(this);
                }
            }
            Debug.LogError("Update HP to: " + CurrentHealth);
            Save();
        }

        public async UniTask ChangeStress(float amount, StatsChangeMethod method)
        {
            if (method == StatsChangeMethod.Add)
            {
                CurrentStress += amount * ModifierTag.StressResistance;
            }
            else if (method == StatsChangeMethod.Remove)
            {
                if (CurrentStress - amount >= BaseStress)
                    CurrentStress -= amount;
                else
                    CurrentStress = BaseStress;
            }

            OnStressUpdated?.Invoke(CurrentStress);
            OnStressChanges?.Invoke(method == StatsChangeMethod.Add ? amount : -amount);
            
            if (IsDieStress())
            {
                await UniTask.WaitForSeconds(1f);
                    
                var dies = G.Interactors.GetAll<IDieStressCharacter>();
                foreach (var dy in dies)
                {
                    await dy.OnDie(this);
                }
            }
            Debug.LogError("Update stress to: " + CurrentStress);
            Save();
        }

        private bool IsDieHP()
        {
            return CurrentHealth <= Entity.Get<StatsTag>().MinHP;
        }

        private bool IsDieStress()
        {
            return CurrentStress >= Entity.Get<StatsTag>().MaxStress;
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

            currentCharacterSave.Health = CurrentHealth;
            currentCharacterSave.Stress = CurrentStress;

            saveModel.ForceSave();
        }
    }
}