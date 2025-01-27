using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.TriPeaks
{
    public interface ICardMover
    {
        void Construct(Sprite backSprite);
        UniTask Move(Sprite sprite, Vector3 from, Vector3 to);
        UniTask MoveWithFlipUp(Sprite sprite, Vector3 from, Vector3 to);
    }
}