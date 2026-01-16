using System;
using System.Collections.Generic;
using System.Linq;

namespace jam.CodeBase.Core.SavesGeneral
{
    public class Saves
    {
        public static string SaveKeySuffix => "sv_1_";
        
        private List<BaseSave> _savables = new List<BaseSave>();
        
        public Saves()
        {
            var savesModels = ReflectionUtil.FindAllSubclasses<BaseSave>();
            
            foreach (var savesModel in savesModels)
            {
                var savable = Activator.CreateInstance(savesModel) as BaseSave;
                savable.Initialize();
                
                _savables.Add(savable);
            }
        }

        public TSave Get<TSave>() where TSave : BaseSave
        {
            var save = _savables.FirstOrDefault(s => s is TSave);
            return save as TSave;
        }

        public void ClearAll()
        {
            foreach (var save in _savables)
            {
                save.Clear();
            }
        }
    }
}