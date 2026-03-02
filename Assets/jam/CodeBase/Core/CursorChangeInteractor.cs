using System.Threading;
using Cysharp.Threading.Tasks;
using jam.CodeBase.Core.Interactors;
using UnityEngine;

namespace jam.CodeBase.Core
{
    public class CursorChangeInteractor : BaseInteractor, IOnAwake
    {
        private Texture2D _defaultCursor;
        private Texture2D _clickCursor;
        
        public UniTask OnAwake()
        {
            _defaultCursor = Resources.Load<Texture2D>("Art/Cursors/CursorDefaultOutline");
            _clickCursor = Resources.Load<Texture2D>("Art/Cursors/CursorClickOutline");
            
            CheckCursorState(G.GameAliveCancellationToken).AttachExternalCancellation(G.GameAliveCancellationToken).Forget();
            
            return UniTask.CompletedTask;
        }

        private async UniTask CheckCursorState(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if(Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2))
                {
                    Cursor.SetCursor(_clickCursor, Vector2.zero,  CursorMode.Auto);
                }
                else
                {
                    Cursor.SetCursor(_defaultCursor, Vector2.zero, CursorMode.Auto);
                }

                await UniTask.WaitForEndOfFrame(token);
            }
        }
    }
}