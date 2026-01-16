using System.Collections.Generic;
using UnityEngine;

namespace Ostryzhnyi.EasyViewService.Api.Service
{
    public interface IViewServiceCanvas
    {
        public IReadOnlyDictionary<ViewLayers.ViewLayers, Transform> Layers { get; }
    }
}