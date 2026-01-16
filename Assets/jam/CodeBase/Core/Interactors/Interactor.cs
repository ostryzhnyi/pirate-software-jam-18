using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;

namespace jam.CodeBase.Core.Interactors
{
    public class Interactor
    {
        public IReadOnlyList<BaseInteractor> Interactors { get; set; }
        
        private List<BaseInteractor> _interactors;

        public Interactor()
        {
            var interactors = ReflectionUtil
                .FindAllSubclasses<BaseInteractor>();
            _interactors = new List<BaseInteractor>();
            
            foreach (var interactor in interactors)
            {
                _interactors.Add(Activator.CreateInstance(interactor) as BaseInteractor);
            }

            _interactors = _interactors.OrderBy(i => i.GetPriority()).ToList();
        }

        public TInteractorInstance[] GetAll<TInteractorInstance>()
        {
            return _interactors
                .Where(i => i is TInteractorInstance)
                .Cast<TInteractorInstance>()
                .ToArray();
        }

        public TInteractorInstance[] CallAll<TInteractorInstance>(Action<TInteractorInstance> action)
        {
            return _interactors
                .Where(i => i is TInteractorInstance)
                .Cast<TInteractorInstance>()
                .ForEach(t => action?.Invoke(t))
                .ToArray();
        }
    }

    public abstract class BaseInteractor
    {
        public virtual int GetPriority()
        {
            return 0;
        }
    }
}