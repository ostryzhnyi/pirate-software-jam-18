using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ostryzhnyi.EasyViewService.Api.Animation
{
    public abstract class BaseAnimationStrategy : MonoBehaviour
    {
        public abstract UniTask Show();
        public abstract UniTask Hide();
    }
}