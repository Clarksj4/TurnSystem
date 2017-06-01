using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TurnBased
{
    /// <summary>
    /// Stores and cycles items in priority order
    /// </summary>
    public class TurnOrder<T> : IEnumerable<ITurnBased<T>> where T : IComparable<T>
    {
        /// <summary>
        /// The item whose turn it currently is. Returns null if the current item was removed from the order
        /// </summary>
        public ITurnBased<T> Current
        {
            get
            {
                if (currentToBeRemoved ||   // If Current was removed from list, don't return its ref
                    currentNode == null)    // Current node not yet set
                    return null;

                // Get item from linked list node
                return currentNode.Value;
            }
        }

        /// <summary>
        /// The number of items in this turn order.
        /// </summary>
        public int Count { get; private set; }

        private LinkedList<ITurnBased<T>> items;
        private LinkedListNode<ITurnBased<T>> currentNode;
        private bool currentToBeRemoved;

        /// <summary>
        /// An empty turn order
        /// </summary>
        public TurnOrder()
        {
            items = new LinkedList<ITurnBased<T>>();
            currentNode = null;
            currentToBeRemoved = false;
            Count = 0;
        }

        /// <summary>
        /// Inserts the item in order based upon its priority
        /// </summary>
        public void Insert(ITurnBased<T> item)
        {
            // Can't insert null item
            if (item == null)
                throw new ArgumentNullException("item cannot be null");

            // Can't have duplicates in order
            if (items.Contains(item))
                throw new ArgumentException("Order already contains item");

            // First item to be inserted
            if (items.Count == 0)
                items.AddFirst(item);

            else
            {
                var walker = items.First;

                // Walk until finding a smaller node
                while (walker != null &&
                       item.Priority.CompareTo(walker.Value.Priority) < 0)
                    walker = walker.Next;

                // Add to end of order
                if (walker == null)
                    items.AddLast(item);

                // Add in front of smaller node
                else
                    items.AddBefore(walker, item);
            }

            Count++;
        }

        /// <summary>
        /// Removes the item from the turn order
        /// </summary>
        public bool Remove(ITurnBased<T> item)
        {
            // Can't remove null item
            if (item == null)
                throw new ArgumentNullException("item cannot be null");

            // Find item's node incase its the current node
            LinkedListNode<ITurnBased<T>> node = items.Find(item);

            if (node == null)
                return false;

            // If current item is being removed, marked it as removed
            if (node == currentNode)
                currentToBeRemoved = true;
            else
                items.Remove(node);

            // Item successfully removed
            Count--;
            return true;
        }

        /// <summary>
        /// Updates the item's position in the turn order based upon its priority
        /// </summary>
        public void UpdatePriority(ITurnBased<T> item)
        {
            // TODO: Proceed to node following current prior to priorty change when end turn is called
            if (item == Current)
                throw new NotImplementedException("Cannot update priority of current node");

            // Can't update item marked for removal
            if (currentToBeRemoved && currentNode.Value == item)
                throw new ArgumentException("Order does not contain item");

            // Remove from order
            bool removed = Remove(item);

            // Can't update item if it doesn't exists in order
            if (removed == false)
                throw new ArgumentException("Order does not contain item");

            // Re-insert into order in correct position
            Insert(item);
        }

        /// <summary>
        /// Move to the next item in the turn order. Notifies the next item of its turn starting.
        /// </summary>
        /// <returns>True if there is another item in the order who has not had its turn yet during this cycle</returns>
        public bool MoveNext()
        {
            // Can't move to next item in order if there is none
            if (items.Count == 0)
                throw new InvalidOperationException("Order is empty");

            // Notify current of turn end
            EndCurrent();

            // Reitem current node so it can be recycled if necessary
            var node = currentNode;

            // Move to next item in order
            bool isMore = Cycle();

            // Remove previous node if it was marked
            DeferredRecycle(node);

            // Notify current of turn start
            StartCurrent();

            // True if there are more items in the order who have not had their turn during this cycle
            return isMore;
        }

        public IEnumerator<ITurnBased<T>> GetEnumerator()
        {
            // Don't return the node that is marked for removal
            if (currentToBeRemoved)
                return items.Where(p => p != currentNode.Value).GetEnumerator();

            else
                return items.GetEnumerator();
        }

        /// <summary>
        /// Informs the current item that its turn has ended
        /// </summary>
        void EndCurrent()
        {
            // If thing has been removed from order, do not notify it of turn end
            if (Current != null)
                Current.TurnEnd();
        }

        void DeferredRecycle(LinkedListNode<ITurnBased<T>> node)
        {
            // Remove current node if its been marked
            if (currentToBeRemoved)
                items.Remove(node);

            // Update mark
            currentToBeRemoved = false;
        }

        /// <summary>
        /// Proceed to next item in order. 
        /// </summary>
        /// <returns>True if there is another item in the order who has not had its turn yet during this cycle</returns>
        bool Cycle()
        {
            // Move to next item in order
            if (currentNode == null)
                currentNode = items.First;

            else
                currentNode = currentNode.Next;

            return currentNode != null;
        }

        /// <summary>
        /// Inform current item that its their turn
        /// </summary>
        void StartCurrent()
        {
            // Activate current item...
            if (Current != null)
                Current.TurnStart();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // Don't return the node that is marked for removal
            if (currentToBeRemoved)
                return items.Where(p => p != currentNode.Value).GetEnumerator();

            else
                return items.GetEnumerator();
        }
    }
}
