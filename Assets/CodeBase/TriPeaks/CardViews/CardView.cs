using CodeBase.TriPeaks.Cards;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.TriPeaks.CardViews
{
    public abstract class CardView : MonoBehaviour, ICardView
    {
        [SerializeField] private Image _image;

        private CardData _cardData;

        public CardData CardData => _cardData;
        public Vector3 Position => _image.rectTransform.position;

        public bool IsActive { get; private set; }
        public bool IsFaceUp { get; private set; }

        public void SetCardData(CardData cardData, bool isFaceUp)
        {
            _cardData = cardData;
            if (isFaceUp)
            {
                SetDefSprite();
            }
            else
            {
                SetFaceUp(false);
            }
            SetImageActive(true);
        }

        public void SetDefSprite()
        {
            SetSprite(_cardData.Sprite);
            SetFaceUp(true);
        }

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SetImageActive(bool isActive)
        {
            _image.enabled = isActive;
            IsActive = isActive;
        }

        public void FlipUp()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_image.rectTransform.DORotate(new Vector3(0, 90, 0), 0.1f));
            seq.AppendCallback(() => SetDefSprite());
            seq.Append(_image.rectTransform.DORotate(new Vector3(0, 0, 0), 0.1f));

            seq.ToUniTask().Forget();
        }

        public void SetFaceUp(bool isFaceUp)
        {
            IsFaceUp = isFaceUp;
        }

    }


}