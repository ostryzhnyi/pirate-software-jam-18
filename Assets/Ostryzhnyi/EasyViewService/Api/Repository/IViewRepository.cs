using System.Collections.Generic;
using Ostryzhnyi.EasyViewService.Api.Service;

namespace Ostryzhnyi.EasyViewService.Api.Repository
{
    public interface IViewRepository
    {
        public IEnumerable<IView> GetViewPrefabs();
    }
}