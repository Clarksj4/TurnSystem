
namespace TurnSystem
{
    /// <summary>
    /// Interface for controlling turn based pawns
    /// </summary>
    public interface ITurnBasedController
    {
        /// <summary>
        /// One of this controller's pawn's turn is starting
        /// </summary>
        /// <param name="pawn"></param>
        void PawnStart(ITurnBasedPawn pawn);
    }
}
