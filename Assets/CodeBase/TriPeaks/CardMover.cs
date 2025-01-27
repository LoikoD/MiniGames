using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.TriPeaks
{
    public class CardMover : MonoBehaviour, ICardMover
    {
        [SerializeField] private Image _image;

        private const float MoveAnimationTime = 0.2f;
        private const float FlipAnimationTime = 0.2f;

        private readonly List<Tween> _activeTweens = new();

        private CancellationTokenSource _cts;

        private Sprite _backSprite;

        public void Construct(Sprite backSprite)
        {
            _backSprite = backSprite;
        }

        public async UniTask Move(Sprite sprite, Vector3 from, Vector3 to)
        {
            CancelPrevious();

            InitState(sprite, from);
            gameObject.SetActive(true);

            _activeTweens.Add(MoveTween(to));

            await PlayTweens();

            gameObject.SetActive(false);
        }

        public async UniTask MoveWithFlipUp(Sprite sprite, Vector3 from, Vector3 to)
        {
            CancelPrevious();

            InitState(_backSprite, from);
            gameObject.SetActive(true);

            _activeTweens.Add(MoveTween(to));
            _activeTweens.Add(FlipUpSequence(sprite));

            await PlayTweens();

            gameObject.SetActive(false);
        }

        private void InitState(Sprite sprite, Vector3 position)
        {
            _image.rectTransform.position = position;
            _image.sprite = sprite;
        }

        private async UniTask PlayTweens()
        {
            await UniTask.WhenAll(_activeTweens.Select(t => t.WithCancellation(_cts.Token)));
        }

        private Tween MoveTween(Vector3 to)
        {
            return _image.rectTransform.DOMove(to, MoveAnimationTime);
        }

        private Sequence FlipUpSequence(Sprite sprite)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_image.rectTransform.DORotate(new Vector3(0, 90, 0), FlipAnimationTime / 2f));
            seq.AppendCallback(() => _image.sprite = sprite);
            seq.Append(_image.rectTransform.DORotate(new Vector3(0, 0, 0), MoveAnimationTime / 2f));
            return seq;
        }

        private void CancelPrevious()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            foreach (Tween t in _activeTweens)
                t?.Kill();
            _activeTweens.Clear();
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}