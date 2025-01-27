using CodeBase.TriPeaks.Cards;
using CodeBase.TriPeaks.CardViews;
using System;
using UnityEngine;

namespace CodeBase.TriPeaks.GameFieldViews
{
    public class StockView : MonoBehaviour, IStockView
    {
        [SerializeField] private StockCardView _topCardView;

        private Sprite _backSprite;

        public event Action<IStockCardView> StockCardClicked;

        public void Construct(Sprite backSprite)
        {
            _backSprite = backSprite;

            _topCardView.StockClicked += OnStockCardClicked;
        }

        public void InitStartState(CardData topCard)
        {
            _topCardView.SetSprite(_backSprite);
            SetTopCard(topCard);
        }

        public void OnStockCardClicked(IStockCardView stockCardView)
        {
            StockCardClicked?.Invoke(stockCardView);
        }

        public void SetTopCard(CardData cardData)
        {
            _topCardView.SetCardData(cardData, false);
        }

        public void SetEmpty()
        {
            _topCardView.SetImageActive(false);
        }

        private void OnDestroy()
        {
            _topCardView.StockClicked -= OnStockCardClicked;
        }
    }
}
