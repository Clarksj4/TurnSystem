using System;

namespace TurnBased
{
    /// <summary>
    /// Turn order member object that operates in a turn based manner. 
    /// </summary>
    public interface ITurnBased<T> where T : IComparable<T>
    {
        /// <summary>
        /// The object's priority in the turn order
        /// </summary>
        T Priority { get; }

        /// <summary>
        /// The object's turn is starting
        /// </summary>
        void TurnStart();

        /// <summary>
        /// The object's turn is ending
        /// </summary>
        void TurnEnd();


    }
}
