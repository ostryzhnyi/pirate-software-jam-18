using Cysharp.Threading.Tasks;

namespace jam.CodeBase.Character
{
   public interface IDieStressCharacter
   {
      public UniTask OnDie(Character character);
   }
   
   public interface IDieHealthCharacter
   {
      public UniTask OnDie(Character character);
   }
   
   public interface IAliveCharacter
   {
      public UniTask OnAlive(Character character);
   }
}