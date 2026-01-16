using System;
using System.Collections.Generic;
using Ostryzhnyi.EasyViewService.Api.Service;
using UnityEngine;

namespace Ostryzhnyi.EasyViewService.Impl.Service
{
    public class ViewServiceCanvas : MonoBehaviour, IViewServiceCanvas
    {
        public IReadOnlyDictionary<ViewLayers.ViewLayers, Transform> Layers => _layers;

        private readonly Dictionary<ViewLayers.ViewLayers, Transform> _layers = new Dictionary<ViewLayers.ViewLayers, Transform>();
        
        [SerializeField] private Transform _emptyLayer = default;

        private void Awake()
        {
            var layers = (ViewLayers.ViewLayers[])Enum.GetValues(typeof(ViewLayers.ViewLayers));
            foreach (var layer in layers)
            {
                if (!_layers.ContainsKey(layer))
                {
                    var layerInstance = Instantiate(_emptyLayer, transform);
                    layerInstance.name = layer.ToString();
                    //layerInstance.gameObject.SetActive(false);
                    
                    _layers.Add(layer, layerInstance);
                }
            }
        }

    }
}