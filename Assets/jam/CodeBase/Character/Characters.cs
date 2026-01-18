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
    }
}