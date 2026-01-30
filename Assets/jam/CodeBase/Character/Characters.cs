using System.Collections.Generic;
using System.Linq;
using jam.CodeBase.Character.Data;
using jam.CodeBase.Core;
using jam.CodeBase.Core.Tags;

namespace jam.CodeBase.Character
{
    public class Characters
    {
        public List<Character> CharactersList = new List<Character>();
        public Character CurrentCharacter;

        public Characters()
        {
            var entities = CMS.GetAll<CMSEntity>()
                .Where(e => e.Is<CharacterTag>());
            var save = G.Saves.Get<CharactersSaveModel>().Data;
            
            foreach (var entity in entities)
            {
                var name = entity.Get<NameTag>();
                var charSave = save.CharactersSaves.FirstOrDefault(s => s.CharacterName == name.Name);
                var character = new Character(entity, charSave);
                CharactersList.Add(character);
            }
        }

        public void ClearSaveForAliveCharacters()
        {
            var saveModel = G.Saves.Get<CharactersSaveModel>();
            var saveData = saveModel.Data;
            
            if (CharactersList.All(c => c.IsDie))
            {
                saveModel.Clear();
            }

            foreach (var character in CharactersList)
            {
                if (!character.IsDie)
                {

                    var currentCharacterSave = saveData.CharactersSaves
                        .FirstOrDefault(s => s.CharacterName == character.Name);

                    if (currentCharacterSave == null)
                    {
                        currentCharacterSave = new CharacterSaveData
                        {
                            CharacterName = character.Name,
                        };
                        if (saveModel.Data.CharactersSaves == null)
                        {
                            saveModel.Data.CharactersSaves = new List<CharacterSaveData>();
                        }

                        saveModel.Data.CharactersSaves.Add(currentCharacterSave);
                    }

                    currentCharacterSave.Health = character.BaseHP;
                    currentCharacterSave.Stress = character.BaseStress;

                    saveModel.ForceSave();
                }
            }
        }
    }
}