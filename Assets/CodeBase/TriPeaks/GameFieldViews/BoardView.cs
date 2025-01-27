using CodeBase.TriPeaks.Cards;
using CodeBase.TriPeaks.CardViews;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.TriPeaks.GameFieldViews
{

    public class BoardView : MonoBehaviour, IBoardView
    {
        [SerializeField] private BoardCardView[] _boardCardViews;
        [SerializeField] private int[] _rowSizes;

        private Sprite _backSprite;

        public event Action<IBoardCardView> BoardCardClicked;

        public void Construct(Sprite backSprite)
        {
            _backSprite = backSprite;

            foreach (BoardCardView boardCardView in _boardCardViews)
            {
                boardCardView.BoardClicked += OnBoardCardClicked;
            }
        }

        public void InitStartState(List<CardData> boardCards)
        {
            if (boardCards.Count != _boardCardViews.Length)
                throw new ArgumentException("Wrong boardCards count for start state.");

            for (int i = 0; i < _boardCardViews.Length; i++)
            {
                _boardCardViews[i].SetCardData(boardCards[i], false);
            }

            InitializeCardBlockers();

            SetSprites();
        }

        public void ResetCard(Card card)
        {
            BoardCardView view = _boardCardViews.First(v => v.CardData.Card.Equals(card));
            view.SetImageActive(true);

            foreach (BoardCardView bcView in _boardCardViews.Where(bcView => bcView.IsActive && !bcView.IsOpen && bcView.IsFaceUp))
            {
                bcView.SetSprite(_backSprite);
                bcView.SetFaceUp(false);
            }
        }

        public void DisableViewByCard(Card card)
        {
            BoardCardView cardView = _boardCardViews.First(v => v.CardData.Card.Equals(card));
            cardView.SetImageActive(false);

            foreach (BoardCardView bcView in _boardCardViews.Where(bcView => bcView.IsActive && bcView.IsOpen && !bcView.IsFaceUp))
            {
                bcView.SetDefSprite();
            }
        }

        public void DisableBoardCardView(IBoardCardView cardView)
        {
            cardView.SetImageActive(false);

            foreach (BoardCardView bcView in _boardCardViews.Where(bcView =>bcView.IsActive && bcView.IsOpen && !bcView.IsFaceUp))
            {
                bcView.FlipUp();
            }
        }

        public bool HasAdjacentOpenCardOnBoard(Card card)
        {
            if (_boardCardViews.Any(bcView =>
                bcView.IsActive
                && bcView.IsOpen
                && bcView.CardData.Card.AreCardValuesAdjacent(card)))
            {
                return true;
            }

            return false;
        }

        private void SetSprites()
        {
            for (int i = 0; i < _boardCardViews.Length; i++)
            {
                if (_boardCardViews[i].IsOpen)
                {
                    _boardCardViews[i].SetDefSprite();
                }
                else
                {
                    _boardCardViews[i].SetSprite(_backSprite);
                }
            }
        }

        private void InitializeCardBlockers()
        {

            // row 0
            for (int i = 0; i < _rowSizes[0]; i++)
            {
                int blockingIndex = _rowSizes[0] + i * 2;
                _boardCardViews[i].AddBlocker(_boardCardViews[blockingIndex]);
                _boardCardViews[i].AddBlocker(_boardCardViews[blockingIndex + 1]);
            }

            // row 1
            for (int i = 0; i < _rowSizes[1]; i++)
            {
                int currentIndex = _rowSizes[0] + i;
                int blockingIndex = _rowSizes[0] + _rowSizes[1] + (int)(i * 1.5f);

                _boardCardViews[currentIndex].AddBlocker(_boardCardViews[blockingIndex]);
                _boardCardViews[currentIndex].AddBlocker(_boardCardViews[blockingIndex + 1]);
            }

            // row 2
            for (int i = 0; i < _rowSizes[2]; i++)
            {
                int currentIndex = _rowSizes[0] + _rowSizes[1] + i;
                int blockingIndex = _rowSizes[0] + _rowSizes[1] + _rowSizes[2] + i;

                _boardCardViews[currentIndex].AddBlocker(_boardCardViews[blockingIndex]);
                _boardCardViews[currentIndex].AddBlocker(_boardCardViews[blockingIndex + 1]);
            }
        }

        private void OnBoardCardClicked(IBoardCardView cardView)
        {
            BoardCardClicked?.Invoke(cardView);
        }

        private void OnDestroy()
        {
            foreach (IBoardCardView boardCardView in _boardCardViews)
            {
                boardCardView.BoardClicked -= OnBoardCardClicked;
            }
        }
    }
}