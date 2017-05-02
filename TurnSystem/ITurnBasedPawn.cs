using System;

namespace TurnSystem
{
    /// <summary>
    /// Interface for object that operates in a turn based manner
    /// </summary>
    public interface ITurnBasedPawn : IComparable<ITurnBasedPawn>
    {
        /// <summary>
        /// The object that controls this pawn
        /// </summary>
        ITurnBasedController Controller { get; }

        /// <summary>
        /// The pawn's turn is starting
        /// </summary>
        void TurnStart();

        /// <summary>
        /// The pawn's turn is ending
        /// </summary>
        void TurnEnd();
    }
}
