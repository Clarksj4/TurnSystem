using System;

namespace TurnSystem
{
    public interface ITurnBasedPawn : IComparable<ITurnBasedPawn>
    {
        ITurnBasedController Controller { get; }

        void TurnStart();
        void TurnEnd();
    }
}
