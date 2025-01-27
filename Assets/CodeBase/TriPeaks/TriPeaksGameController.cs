using CodeBase.TriPeaks.Cards;
using CodeBase.TriPeaks.CardViews;
using CodeBase.TriPeaks.GameFieldViews;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.TriPeaks
{
    public class TriPeaksGameController : ITriPeaksGameController
    {
        private const int BoardCardsCount = 28;
        private const int DeckSize = 52;

        private readonly List<CardData> _deck;
        private readonly IBoardView _boardView;
        private readonly IStockView _stockView;
        private readonly IWasteCardView _wasteCardView;
        private readonly ICardMover _cardMover;

        private ITriPeaksSaveData _gameData;

        private Stack<Card> _stockCards;
        private Stack<Card> _wastedCards;
        private List<Card> _boardCards;

        public event Action OnTurn;
        public event Action OnVictory;
        public event Action OnDefeat;


        public TriPeaksGameController(List<CardData> fullDeck, IBoardView boardView, IStockView stockView, IWasteCardView wasteCardView, ICardMover cardMover)
        {
            _deck = fullDeck;
            _boardView = boardView;
            _stockView = stockView;
            _wasteCardView = wasteCardView;
            _cardMover = cardMover;

            _boardView.BoardCardClicked += OnBoardCardClicked;
            _stockView.StockCardClicked += OnStockCardClicked;
        }

        public void LoadGame(ITriPeaksSaveData gameData)
        {
            _gameData = gameData;

            if (_gameData.StartStack.Count == 0)
            {
                SetupGame(newGame: true);
                return;
            }

            SetupGame(newGame: false);

            Queue<Turn> turnsQueue = new(_gameData.Turns.Reverse());

            foreach (Turn turn in turnsQueue)
            {
                SimulateTurn(turn);
            }

            CheckEndGame();
        }

        public void SetupGame(bool newGame)
        {
            if (newGame)
            {
                Stack<Card> startStack = CreateShuffledStack(_deck);
                _gameData.SetStartStack(startStack);
                _gameData.Turns.Clear();
            }

            if (_gameData.StartStack.Count != DeckSize)
                throw new Exception("Wrong start deck cards count.");

            _stockCards = new(_gameData.StartStack);

            TakeBoardCards();

            List<CardData> boardCardsData = new();
            foreach (Card card in _boardCards)
            {
                boardCardsData.Add(_deck.Find(c => c.Card.Equals(card)));
            }

            _boardView.InitStartState(boardCardsData);

            _wasteCardView.SetImageActive(false);
            _wastedCards = new Stack<Card>();

            _stockView.InitStartState(_deck.Find(c => c.Card.Equals(_stockCards.Peek())));

        }

        public void UndoTurn()
        {
            Turn lastTurn = _gameData.Turns.Pop();

            _wastedCards.Pop();
            if (_wastedCards.TryPeek(out Card wastedCard))
            {
                _wasteCardView.SetCardData(_deck.Find(c => c.Card.Equals(wastedCard)), true);
            }
            else
            {
                _wasteCardView.SetImageActive(false);
            }

            switch (lastTurn.Type)
            {
                case TurnType.Board:
                    UndoBoardTurn(lastTurn.Card);
                    break;

                case TurnType.Stock:
                    UndoStockTurn(lastTurn.Card);
                    break;

                default:
                    throw new Exception("Uknown turn type");
            }
        }

        private void UndoBoardTurn(Card card)
        {
            _boardCards.Add(card);
            _boardView.ResetCard(card);
        }

        private void UndoStockTurn(Card card)
        {
            _stockCards.Push(card);
            _stockView.SetTopCard(_deck.Find(c => c.Card.Equals(card)));
        }

        private void TakeBoardCards()
        {
            _boardCards = new();
            for (int i = 0; i < BoardCardsCount; i++)
            {
                _boardCards.Add(_stockCards.Pop());
            }
        }

        private void OnBoardCardClicked(IBoardCardView boardCardView)
        {
            if (_wastedCards.TryPeek(out Card wastedCard) && !boardCardView.CardData.Card.AreCardValuesAdjacent(wastedCard))
                return;

            AddTurn(TurnType.Board, boardCardView.CardData.Card);
            DoBoardTurn(boardCardView.CardData.Card);

            MoveBoardCardToWasteAnimation(boardCardView.CardData, boardCardView.Position).Forget();
            _boardView.DisableBoardCardView(boardCardView);

            CheckEndGame();
        }

        private void OnStockCardClicked(IStockCardView stockCardView)
        {
            AddTurn(TurnType.Stock, stockCardView.CardData.Card);

            DoStockTurn(stockCardView.CardData.Card);

            MoveStockCardToWasteAnimation(stockCardView.CardData, stockCardView.Position).Forget();

            SetStockTopCard();

            CheckLose();
        }

        #region Simalate Turn
        private void SimulateTurn(Turn turn)
        {
            switch (turn.Type)
            {
                case TurnType.Board:
                    SimulateBoardTurn(turn.Card);
                    break;

                case TurnType.Stock:
                    SimulateStockTurn(turn.Card);
                    break;

                default:
                    throw new Exception("Uknown turn type");
            }
        }

        private void SimulateBoardTurn(Card card)
        {
            DoBoardTurn(card);

            _wasteCardView.SetCardData(_deck.Find(c => c.Card.Equals(card)), true);
            _boardView.DisableViewByCard(card);
        }

        private void SimulateStockTurn(Card card)
        {
            DoStockTurn(card);

            _wasteCardView.SetCardData(_deck.Find(c => c.Card.Equals(card)), true);

            SetStockTopCard();
        }
        #endregion

        #region Do Turn
        private void AddTurn(TurnType turnType, Card card)
        {
            _gameData.Turns.Push(new(turnType, card));
            OnTurn?.Invoke();
        }

        private void DoBoardTurn(Card card)
        {
            _boardCards = _boardCards.Where(c => !c.Equals(card)).ToList();
            _wastedCards.Push(card);
        }

        private void DoStockTurn(Card card)
        {
            _stockCards.Pop();
            _wastedCards.Push(card);
        }
        private void SetStockTopCard()
        {
            if (_stockCards.TryPeek(out Card topStockCard))
            {
                _stockView.SetTopCard(_deck.Find(c => c.Card.Equals(topStockCard)));
            }
            else
            {
                _stockView.SetEmpty();
            }
        }
        #endregion

        #region End Game
        private void CheckEndGame()
        {
            if (CheckWin())
                return;
            CheckLose();
        }

        private bool CheckLose()
        {
            if (_stockCards.Count > 0)
                return false;

            Card wastedTopCard = _wastedCards.Peek();

            if (_boardView.HasAdjacentOpenCardOnBoard(wastedTopCard))
                return false;

            EndGame(isVictory: false);
            return true;
        }

        private bool CheckWin()
        {
            if (_boardCards.Count == 0)
            {
                EndGame(isVictory: true);
                return true;
            }
            return false;
        }

        private void EndGame(bool isVictory)
        {
            if (isVictory)
            {
                OnVictory?.Invoke();
            }
            else
            {
                OnDefeat?.Invoke();
            }
        }
        #endregion

        #region Move To Waste Animations
        private async UniTaskVoid MoveBoardCardToWasteAnimation(CardData cardData, Vector3 fromPosition)
        {
            await _cardMover.Move(cardData.Sprite, fromPosition, _wasteCardView.Position);

            _wasteCardView.SetCardData(cardData, true);

        }
        private async UniTaskVoid MoveStockCardToWasteAnimation(CardData cardData, Vector3 fromPosition)
        {
            await _cardMover.MoveWithFlipUp(cardData.Sprite, fromPosition, _wasteCardView.Position);

            _wasteCardView.SetCardData(cardData, true);

        }
        #endregion

        #region Shuffle cards
        private Stack<Card> CreateShuffledStack(List<CardData> cards)
        {
            List<Card> shuffled = cards.Select(c => c.Card).ToList();
            ShuffleList(shuffled);
            return new Stack<Card>(shuffled);
        }

        private void ShuffleList(List<Card> list, int shuffleTimes = 3)
        {
            for (int shuffleCounter = 0; shuffleCounter < shuffleTimes; shuffleCounter++)
            {
                for (int i = list.Count - 1; i > 0; i--)
                {
                    int randomIndex = UnityEngine.Random.Range(0, i + 1);
                    (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
                }
            }
        }
        #endregion

        ~TriPeaksGameController()
        {
            _boardView.BoardCardClicked -= OnBoardCardClicked;
            _stockView.StockCardClicked -= OnStockCardClicked;
        }
    }
}
