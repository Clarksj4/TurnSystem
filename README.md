# Turn System

## Overview

A turn order is a priority ordered collection of items that allows enumeration. A turn order notifies members when they are iterated via the _MoveNext_ method.

Opertation     | Description                                                         |
---------------|---------------------------------------------------------------------|
Current        | Get the object whose turn it is                                     |
Insert         | Inserts an object in priority order                                 |
Remove         | Removes an object from the order                                    |
UpdatePriority | Updates an object's position in the order based on its new priority |
MoveNext       | Progress current to the next object in the order                    |

## Usage

### ITurnBased

Each object in the turn order must implement the _ITurnBased_ interface. This allows the object to be notified of its turn beginning and ending.

    using TurnBased;
    
    public class TurnBasedExample : ITurnBased<int>
    {
        ...
        public int Priority
        {
            get 
            {
                // Get or calculate priority in turn order...
                int priority = ...
                return priority;
            }
        }        
        
        // This object's turn is beginning
        public void TurnStart()
        {
            // ... Do something ...
            // ... Refresh time units, allow for input, draw cards, etc...
            
            ...
        }
        
        // This object's turn is ending
        public void TurnEnd()
        {
            // ... Do something ...
            // ... Resolve effects, disable input control, discard cards, etc...
            
            ...
        }
    }   
    
### Creating a turn order

Objects that implement the _ITurnBased_ interface can be added to a turn order.

    TurnBasedExample examplePawn1;
    TurnBasedExample examplePawn2;

    public void TurnOrderExample()
    {
        // An empty order
        TurnOrder<int> order = new TurnOrder<int>();
    
        // Add the example pawns to the order
        order.Insert(examplePawn1);
        order.Insert(examplePawn2);
    }
    
### Cycling the turn order    

Calls to _MoveNext_ cycle through the items in order. The current item's _TurnEnd_ method is called, and the next item's _TurnStart_ method is called. After a cycle completes, a further call to _MoveNext_ will restart the cycle. This allows for action at the end of a complete cycle.
    
    TurnOrder<int> order;
    
    public void MoveNextExample()
    {
        // Game over when order no longer contains any items
        while (order.Count > 0)
        {
            // Returns true if there is another item in the order who is yet to have a turn
            bool remaining = order.MoveNext();
            while (remaining)
            {
                // ... Do something ...
                // ... Update turn order interface, etc ...
            
                // Proceed to next item in order
                remaining = order.MoveNext();
            }
        
            // Complete cycle of turn order complete!
        
            // ... Do something ...
            // ... Discard cards, resolve effects, replenish health, etc ...
        }
    }
    
### Checking whose turn it is

The _Current_ property provides a reference to the item whose turn it currently is. This is useful should other objects need to know if it is a particular items turn.

    TurnOrder<int> order;
    TurnBasedExample examplePawn1;
    
    public void CurrentExample()
    {
        // Check if it is examplePawn1's turn
        bool isTurn = order.Current == examplePawn1;
        if (isTurn)
        {
            // ... Do something ....
            // ... Allow input, display stats, etc
        }
    }
    
### Removing items from the order

Items can be removed from the order at any time. If the currently acting item is removed, the _Current_ property will return null until _MoveNext_ is called to proceed to the next item in the order. _Remove_ will return true if the item was present in the order and has been removed.

    TurnOrder<int> order;
    TurnBasedExample examplePawn1;
    
    public void RemoveExample()
    {
        // Check if it is examplePawn1's turn
        bool isTurn = order.Current == examplePawn1;
    
        // Remove examplePawn1 from the order
        bool removed = order.Remove(examplePawn1);
        
        // If examplePawn1 was present in the order
        if (removed)
        {
            // ... Do something ...
            // ... Update interface, check for game over, etc
        
            // Was it the removed item's turn?
            if (isTurn)
            {
                // Move on to next item in the order
                order.MoveNext();
            }
        }
    }
    
### Priority changes

Should an item's priority change, calling _UpdatePriority_ will reposition the given item into the correct place in the turn order.

    TurnOrder<int> order;
    TurnBasedExample examplePawn1;
    TurnBasedExample examplePawn2;
    
    public void UpdatePriorityExample()
    {
        // An empty order
        TurnOrder<int> order = new TurnOrder<int>();
    
        // Set priority of items
        examplePawn1.Priority = 12;
        exameplPawn2.Priority = 32;
    
        // Add the example pawns to the order. Pawn2 is first in order
        order.Insert(examplePawn1);
        order.Insert(examplePawn2);
    
        // Change item's priority. Order needs to update position of pawns 
        examplePawn1.Priority = 42;
        
        // Item positioned correctly in order. Pawn1 is first in order
        order.UpdatePriority(examplePawn1);
    }

### Multiple turn orders

The following is an example of multiple turn orders being used to simulate a Team-Unit scenario. The game consists of multiple Teams which take turns to act. During a Teams turn their Units act in a specific order.

    TurnOrder<int> teamOrder;
    
    public void TeamOrderExample()
    {
        while (!gameOver)
        {
            // Loop through all teams
            bool teamsRemaining = teamOrder.MoveNext();
            while (teamsRemaining)
            {
                // Proceed to next team in order
                teamsRemaining = order.MoveNext();
            }
        }
    }
    
    
    public class Team : ITurnBased
    {
        TurnOrder<int> unitOrder;
    
        ...
    
        public void TurnStart()
        {
            // Loop through all units on team
            bool unitsRemaining = unitOrder.MoveNext();
            while (unitsRemaining)
            {
                // Proceed to next unit in order
                unitsRemaining = order.MoveNext();
            }
        }
    
        ...
    }
    
