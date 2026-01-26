using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using jam.CodeBase.Core;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Framework.Expression;
using Live2D.Cubism.Rendering;
using Sirenix.OdinInspector;
using UnityEngine;

namespace jam.CodeBase.Character
{
    [RequireComponent(typeof(Animator), typeof(CubismExpressionController))]
    public class BoxAnimator: MonoBehaviour
    {
        public CubismExpressionList ExpressionsList;
        public CubismModel CubismModel;

        [SerializeField]
        private List<BoxAnimation> _characterAnimationStructures = new List<BoxAnimation>();
        
        [SerializeField] private CubismExpressionController _defaultExpressionController;
        [SerializeField] private CubismRenderController _cubismRenderController;
        [SerializeField] private Transform _praticleParent;

        private Animator _animator;

        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            G.BoxAnimator = this;
            ResetAsync().Forget();
        }

        private void OnDestroy()
        {
            G.BoxAnimator = null;
        }

        private void ResetState()
        {
            for (var i = 0; i < CubismModel.Parameters.Length; i++)
            {
                var parameter = CubismModel.Parameters[i];
                parameter.OverrideValue(parameter.DefaultValue);
            }

            CubismModel.ForceUpdateNow();
        }
        
        [Button]
        public void PlayAnimation(BoxAnimationType animationType)
        {
            var structure = _characterAnimationStructures.FirstOrDefault(x => x.AnimationType == animationType);
           
            PlayAnimation(structure.Structure);
        }
        
        public void PlayAnimation(CharacterAnimationStructure structure)
        {
            transform.DOScale(Vector3.one * 4.36f, .5f);
            PlayParticle().Forget();
            if (structure.AnimationTrigger != null)
                SetTrigger(structure.AnimationTrigger);
            if (structure.Data != null)
                SetExpression(structure.Data);
            
            ResetAsync(7).Forget();
        }

        private async UniTask ResetAsync(float delay = .1f)
        {
            await UniTask.WaitForSeconds(delay);
            transform.DOScale(Vector3.zero, .5f);
        }

        private async UniTask PlayParticle()
        {
            await UniTask.WaitForSeconds(4.8f);

            Instantiate(GameResources.VFX.BoxParticle, _praticleParent).transform.localScale = Vector3.one * 0.21f;
        }
        
        public void SetParameter(string id, float value)
        {
            //var par = CubismModel.Parameters.First(p => p.Id == parameter.Id);
            CubismModel.Parameters.First(p => p.Id == id).Value = value;
            Debug.LogError("SET PARAMETER: " + id + " to " + value);
            //DOTween.To(() => par.Value, (v) =>  par.Value = v, par.Value, .4f);
        }

        
        public void SetTrigger(string trigger)
        {
            _animator.SetTrigger(trigger);
        }

        [Button]
        public void SetExpression(CubismExpressionData data)
        {
            _defaultExpressionController.CurrentExpressionIndex =
                ExpressionsList.CubismExpressionObjects.ToList().IndexOf(data);
        }
    }
    
    [Serializable]
    public class BoxAnimation
    {
        public BoxAnimationType AnimationType;
        public CharacterAnimationStructure Structure;
    }
    
    public enum BoxAnimationType
    {
       Pils,
       Phone,
       Bandage,
       Bottle,
       Gun
    }
}