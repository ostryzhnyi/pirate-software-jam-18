using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using jam.CodeBase.Core;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.Expression;
using Live2D.Cubism.Rendering;
using Sirenix.OdinInspector;
using UnityEngine;

namespace jam.CodeBase.Character
{
    [RequireComponent(typeof(Animator), typeof(CubismExpressionController))]
    public class CharacterAnimator : MonoBehaviour
    {
        public CubismExpressionList ExpressionsList;
        public CubismModel CubismModel;

        [SerializeField]
        private List<CharacterAnimation> _characterAnimationStructures = new List<CharacterAnimation>();

        [SerializeField] private CubismExpressionController _defaultExpressionController;
        [SerializeField] private CubismRenderController _cubismRenderController;
        [SerializeField] private SpriteRenderer _move;

        private Animator _animator;

        private void Awake()
        {
            // CubismModel.Parameters.First(p => p.Id == "Param6").Value = -30;
            //PlayAnimation(AnimationType.BadHP);
            _animator = GetComponent<Animator>();
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
            var structure = _characterAnimationStructures.FirstOrDefault(x => x.AnimationType == animationType);

            // if (animationType is AnimationType.BadHP or AnimationType.FineHP or AnimationType.NormalHP or
            //     AnimationType.NormalStress or AnimationType.BadStress or AnimationType.FineStress)
            // {
            //     Debug.LogError("UPDATE STATEL: " + animationType + " " + structure.Structure.Data.Parameters[0].Id);
            //     SetParameter("Param5", 30);
            //     SetParameter("Param6", 30);
            //     SetParameter("ParamEyeROpen", 0);
            //
            //     if (animationType is AnimationType.NormalHP)
            //     {
            //         SetParameter("Param5", -30);
            //     }
            //     else if (animationType is AnimationType.BadHP)
            //     {
            //         SetParameter("Param6", -30);
            //     }
            //     else
            //     {
            //         SetParameter(structure.Structure.Data.Parameters[0].Id,
            //             structure.Structure.Data.Parameters[0].Value);
            //     }
            //
            //     return;
            // }
            // else if (animationType == AnimationType.Move)
            // {
            //     PlayMoveAnim().Forget();
            //     return;
            // }

            PlayAnimation(structure.Structure);
        }


        public void PlayAnimation(CharacterAnimationStructure structure)
        {
            if (structure.AnimationTrigger != null)
                SetTrigger(structure.AnimationTrigger);
            // if (structure.Data != null)
            //     SetExpression(structure.Data);
        }

        public void SetTrigger(string trigger)
        {
            _animator.SetTrigger(trigger);
        }

        public void DisableExpressions()
        {
            _defaultExpressionController.CurrentExpressionIndex = -1;
        }

        [Button]
        public void SetExpression(CubismExpressionData data)
        {
            _defaultExpressionController.CurrentExpressionIndex =
                ExpressionsList.CubismExpressionObjects.ToList().IndexOf(data);
        }

        public void SetParameter(string id, float value)
        {
            //var par = CubismModel.Parameters.First(p => p.Id == parameter.Id);
            CubismModel.Parameters.First(p => p.Id == id).Value = value;
            Debug.LogError("SET PARAMETER: " + id + " to " + value);
            //DOTween.To(() => par.Value, (v) =>  par.Value = v, par.Value, .4f);
        }

        public async UniTask PlayMoveAnim(float delay = .8f)
        {
            _move.gameObject.SetActive(true);
            DOTween.To(() => _cubismRenderController.Opacity, (o) => _cubismRenderController.Opacity = o, 0, .5f);
            await _move.DOColor(new Color(1, 1, 1, 1), 0.5f).ToUniTask();

            await UniTask.WaitForSeconds(delay);

            DOTween.To(() => _cubismRenderController.Opacity, (o) => _cubismRenderController.Opacity = o, 1, .5f);
            await _move.DOColor(new Color(1, 1, 1, 0), 0.5f).ToUniTask();
            _move.gameObject.SetActive(false);
        }
    }

    [Serializable]
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
        SetSad,
        BrokeGlass,
        Smoking,
        Eat,
        NormalStress,
        FineStress,
        BadStress,
        NormalHP,
        FineHP,
        BadHP,
        Move,
        WetHair
    }
}