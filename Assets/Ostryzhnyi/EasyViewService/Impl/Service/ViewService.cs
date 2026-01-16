using System;
using System.Collections.Generic;
using System.Linq;
using Ostryzhnyi.EasyViewService.Impl.Repository;
using Ostryzhnyi.EasyViewService.Api.Repository;
using Ostryzhnyi.EasyViewService.Api.Service;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ostryzhnyi.EasyViewService.Impl.Service
{
    /// <summary>
    /// Represents a service that manages the lifecycle and display of UI View within the application.
    /// Implements the <see cref="IViewService"/> interface and is based on Unity's MonoBehaviour.
    /// </summary>
    [DefaultExecutionOrder(-999)]
    public class ViewService : MonoBehaviour, IViewService
    {
        /// <summary>
        /// Singleton instance of the <see cref="IViewService"/>.
        /// </summary>
        public static IViewService Instance { get; private set; }
        
        /// <summary>
        /// Event triggered when a View is opened.
        /// </summary>
        public event Action<IView> OnViewOpen;

        /// <summary>
        /// Event triggered when a View is hidden.
        /// </summary>
        public event Action<IView> OnViewHide;
        
        /// <summary>
        /// Checks if any View is currently open.
        /// </summary>
        public bool AnyViewOpen => _spawnedView.Any(win => win.IsOpened);
        
        private readonly List<IView> _spawnedView = new List<IView>();
        private IView[] _viewPrefabs;

        public ViewServiceCanvas _canvas;
        
        private IViewRepository _repository;

        #region Initialize
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void StartGame()
        {
            Instance = null;
        }

        /// <summary>
        /// Unity's Awake method, used to initialize the service and its dependencies.
        /// </summary>
        public void Awake()
        {
            RepositoryInitialize();
            Initialize();
        }

        /// <summary>
        /// Initializes the repository for managing View prefabs.
        /// </summary>
        private void RepositoryInitialize()
        {
            _repository = new ResourceViewRepository();
        }
        
        /// <summary>
        /// Initializes the ViewService instance and loads View prefabs.
        /// </summary>
        private void Initialize()
        {
            Instance = this;
            
            _viewPrefabs = _repository.GetViewPrefabs().ToArray();
        }

        #endregion
        
        /// <summary>
        /// Shows the specified View type.
        /// </summary>
        /// <typeparam name="TType">The type of View to show, must implement <see cref="IView"/>.</typeparam>
        /// <param name="option">Optional parameters for View display.</param>
        public void ShowView<TType>(ViewOption option = null) where TType : IView
        { 
            var view = GetView<TType>();
            ShowViewInternal(view, option);
        }
        
        /// <summary>
        /// Shows a View by its type.
        /// </summary>
        /// <param name="type">The type of View to show.</param>
        /// <param name="option">Optional parameters for View display.</param>
        public void ShowView(Type type, ViewOption option = null)
        {
            var view = GetView(type);
            ShowViewInternal(view, option);
        }

        /// <summary>
        /// Hides the View of the specified type.
        /// </summary>
        /// <param name="type">The type of View to hide.</param>
        public void HideView(Type type)
        {
            var view = GetView(type);
            HideViewInternal(view);
        }

        /// <summary>
        /// Hides the specified View type.
        /// </summary>
        /// <typeparam name="TType">The type of View to hide, must implement <see cref="IView"/>.</typeparam>
        public void HideView<TType>() where TType : IView
        {
            var view = GetView<TType>();
            HideViewInternal(view);
        }

        /// <summary>
        /// Hides all currently spawned View.
        /// </summary>
        public void HideAllViews()
        {
            foreach (var view in _spawnedView)
            {
                view.Hide();
                OnViewHide?.Invoke(view);
            }
        }
        
        /// <summary>
        /// Retrieves the View instance of the specified type.
        /// </summary>
        /// <typeparam name="TType">The type of View to retrieve, must implement <see cref="IView"/>.</typeparam>
        /// <returns>The instance of the View.</returns>
        public IView GetView<TType>() where TType : IView
        {
            var view = _spawnedView.FirstOrDefault(v => v.GetType() == typeof(TType));
            if (view == default)
            {
                return SpawnView<TType>();
            }

            return view;
        }
        
        /// <summary>
        /// Retrieves the View instance by its type.
        /// </summary>
        /// <param name="type">The type of View to retrieve.</param>
        /// <returns>The instance of the View.</returns>
        public IView GetView(Type type)
        {
            var view = _spawnedView.FirstOrDefault(view => view.GetType() == type);
            
            if (view == default)
            {
                return SpawnView(type);
            }
            
            return view;
        }

        private void ShowViewInternal(IView view, ViewOption option = null) 
        {
            if (view != default)
            {
                foreach (var otherView in _spawnedView.Where(w => w.Layer == view.Layer))
                {
                    if(otherView != view)
                        otherView.Hide();
                }
                
                //_canvas.Layers[view.Layer].gameObject.SetActive(true);
                view.Show(option);
                OnViewOpen?.Invoke(view);
            }
        }
        
        private void HideViewInternal(IView view) 
        {
            if (view != default)
            {
                //_canvas.Layers[view.Layer].gameObject.SetActive(false);
                view.Hide();
                OnViewHide?.Invoke(view);
            }
        }

        private BaseView SpawnView(Type type)
        {
            var prefab = _viewPrefabs.FirstOrDefault(view => view.GetType() == type);

            if (prefab == default)
                throw new NullReferenceException($"{type} is missing. Add {type} prefab to Resources/View.");
            
            if(_canvas == default)
                throw new NullReferenceException($"{_canvas} is missing. Add {nameof(ViewServiceCanvas)} component to View canvas.");
            
            var instance = InstantiateView(prefab);
            _spawnedView.Add(instance);

            return instance;
        }

        private BaseView SpawnView<TType>() where TType : IView
        {
            var prefab = _viewPrefabs.FirstOrDefault(view => view.GetType() == typeof(TType));
            
            if (prefab == default)
                throw new NullReferenceException($"{typeof(TType)} is missing. Add {typeof(TType)} prefab to Resources/View.");
            
            if(_canvas == default)
                throw new NullReferenceException($"{_canvas} is missing. Add {nameof(ViewServiceCanvas)} component to View canvas.");
            
            var instance = InstantiateView(prefab);
            _spawnedView.Add(instance);

            return instance;
        }
        
        /// <summary>
        /// Instantiates a new View from the given prefab. Can be overridden to support dependency injection.
        /// </summary>
        /// <param name="prefab">The View prefab to instantiate.</param>
        /// <returns>A new instance of the View.</returns>
        protected virtual BaseView InstantiateView(IView prefab)
        {
            return Instantiate((prefab as BaseView), _canvas.Layers[prefab.Layer].transform);
        }
    }

}