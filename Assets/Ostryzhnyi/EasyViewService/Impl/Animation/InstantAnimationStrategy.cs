using Cysharp.Threading.Tasks;
using Ostryzhnyi.EasyViewService.Api.Animation;

namespace Ostryzhnyi.EasyViewService.Impl.Animation
{
    public class InstantAnimationStrategy : BaseAnimationStrategy
    {
        public override UniTask Show()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public override UniTask Hide()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}