using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

namespace CodeBase.TriPeaks.CardViews
{
    public class BoardCardView : CardView, IBoardCardView, IPointerClickHandler
    {
        private readonly List<IBoardCardView> _blockedBy = new();

        public event Action<IBoardCardView> BoardClicked;

        public bool IsOpen => !_blockedBy.Any(x => x.IsActive);

        public void AddBlocker(IBoardCardView card)
        {
            _blockedBy.Add(card);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsOpen)
            {
                BoardClicked?.Invoke(this);
            }
        }
    }
}