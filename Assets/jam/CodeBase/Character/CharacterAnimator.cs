using System;
using System.Collections.Generic;
using System.Linq;
using jam.CodeBase.Core;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.Expression;
using Live2D.Cubism.Rendering;
using Sirenix.OdinInspector;
using UnityEngine;

namespace jam.CodeBase.Character
{
    [RequireComponent(typeof(Animator), typeof(CubismExpressionController))]
    public class CharacterAnimator : SerializedMonoBehaviour
    {
        public CubismExpressionList ExpressionsList;
        public CubismModel CubismModel;

        [SerializeField] private List<CharacterAnimation> _characterAnimationStructures = new  List<CharacterAnimation>();

        private Animator _animator;
        private CubismExpressionController _expressionController;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _expressionController = GetComponent<CubismExpressionController>();
            G.CharacterAnimator = this;
        }

        private void OnDestroy()
        {
            G.CharacterAnimator = null;
        }

        public void ApplyTexture(Texture2D texture)
        {
            var renders = GetComponentsInChildren<CubismRenderer>();
            
            foreach (var render in renders)
                render.MainTexture = texture;
        }

        [Button]
        public void PlayAnimation(AnimationType animationType)
        {
            PlayAnimation(_characterAnimationStructures.First(x => x.AnimationType == animationType).Structure);
        }
        
        
        public void PlayAnimation(CharacterAnimationStructure structure)
        {
            SetTrigger(structure.AnimationTrigger);
            if(structure.Data != null)
                SetParameter(structure.Data);
        }

        public void SetTrigger(string trigger)
        {
            _animator.SetTrigger(trigger);
        }

        [Button]
        public void SetParameter(CubismExpressionData data)
        {
            _expressionController.CurrentExpressionIndex =
                ExpressionsList.CubismExpressionObjects.ToList().IndexOf(data);
        }
    }

    public class CharacterAnimation
    {
        public AnimationType AnimationType;
        public CharacterAnimationStructure Structure;
    }

    [Serializable]
    public struct CharacterAnimationStructure
    {
        public string AnimationTrigger;
        public CubismExpressionData Data;
    }

    public enum AnimationType
    {
        Idle,
        TakeRedPill,
        TakeBluePill,
        Call,
        DrinkWater,
        DrinkLimonade,
        RussianRullete,
        SetHappy,
        BrokeGlass,
        Smoking,
        Eat
    }
}