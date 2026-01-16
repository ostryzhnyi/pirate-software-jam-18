using System.Collections.Generic;
using System.Linq;
using Ostryzhnyi.EasyViewService.Api.Service;
using Ostryzhnyi.EasyViewService.Api.Repository;
using UnityEngine;

namespace Ostryzhnyi.EasyViewService.Impl.Repository
{
    public class ResourceViewRepository : IViewRepository
    {
        private const string Path = "Views";

        public IEnumerable<IView> GetViewPrefabs()
        {
            var views = Resources.LoadAll<GameObject>(Path);
            return views.Select(w => w.GetComponent<IView>());
        }
    }
}