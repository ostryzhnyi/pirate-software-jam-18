using System;
using Newtonsoft.Json;
using UnityEngine;

namespace jam.CodeBase.Core.SavesGeneral
{
    public abstract class SaveModel<TData> : BaseSave where TData : new()
    {
        public TData Data { get; set; }
        public event Action<TData> OnUpdate;
        public event Action OnClear;

        private string SaveKey => Saves.SaveKeySuffix + this.GetType().ToString();

        protected abstract void SetDefault();

        public override void Initialize()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                Data = Deserialize(PlayerPrefs.GetString(SaveKey));
            }
            else
            {
                SetDefault();
            }
            
            OnUpdate?.Invoke(Data);
        }
        
        public override void ForceSave()
        {
            var json = Serialize(Data);
            PlayerPrefs.SetString(SaveKey, json);
            OnUpdate?.Invoke(Data);
        }

        public override void Clear()
        {
            SetDefault();
            OnUpdate?.Invoke(Data);
            OnClear?.Invoke();
        }

        protected virtual string Serialize(TData data)
        {
            return JsonConvert.SerializeObject(data);
        }

        protected virtual TData Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<TData>(json);
        }
    }
}