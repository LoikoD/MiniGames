using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Common
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        [SerializeField] private RectTransform _curtain;

        private bool _isShown;
        private Tween _activeTween;

        private void Awake()
        {
            _isShown = true;
            DontDestroyOnLoad(this);
        }

        public async UniTask Show()
        {
            if (!_isShown)
            {
                _isShown = true;
                _activeTween?.Kill();

                _activeTween = _curtain
                    .DOMoveY(Screen.height / 2f, 1f)
                    .From(Screen.height * 2)
                    .SetEase(Ease.OutBounce);
                
                await _activeTween;
            }
        }

        public async UniTask Hide()
        {
            _isShown = false;
            _activeTween?.Kill();

            _activeTween = _curtain
                .DOMoveY(-Screen.height, 1f)
                .SetEase(Ease.InQuad);
            
            await _curtain.DOMoveY(-Screen.height, 1f);
        }
    }
}