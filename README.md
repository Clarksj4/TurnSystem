# Turn System

This page is a WIP

## Overview

A turn order allows for insertion of pawns whilst maintaining their order and tracking which pawn's turn it currently is.

Opertation     | Description                                                  |
---------------|--------------------------------------------------------------|
Current        | Get the pawn whose turn it is                                |
Insert         | Inserts a pawn in priority order                             |
Remove         | Removes a pawn from the order                                |
UpdatePriority | Updates a pawn's position in order based on its new priority |
MoveNext       | Progress current to the next pawn in the order               |

## Usage

### IPawn

Each object in the turn order must implement the IPawn interface. This allows the object to be notified of its turn beginning and ending.

    using TurnBased;
    
    public class TurnBasedPawnExample : ITurnBasedPawn<int>
    {
        ...
        public int Priority
        {
            get 
            {
                // Get or calculate pawn priority in turn order...
                int pawnPriority = ...
                return pawnPriority;
            }
        }        
        
        // This pawn's turn is beginning
        public void TurnStart()
        {
            // ... Do something ...
            // ... Refresh time units, allow for input, draw cards, etc...
            
            ...
        }
        
        // This pawn's turn is ending
        public void TurnEnd()
        {
            // ... Do something ...
            // ... Resolve effects, disable input control, discard cards, etc...
            
            ...
        }
    }   
    
### Creating a turn order

Objects that implement the IPawn interface can be added to a turn order.

    TurnBasedPawnExample examplePawn1;
    TurnBasedPawnExample examplePawn2;

    public void TurnOrderExample()
    {
        // An empty order
        TurnOrder<int> order = new TurnOrder<int>();
    
        // Add the example pawns to the order
        order.Insert(examplePawn1);
        order.Insert(examplePawn2);
    }
    
### Cycling the turn order    

Calls to _MoveNext_ cycle through the pawns in the order. The current pawn's _TurnEnd_ method is called, and the next pawn's _TurnStart_ method is called. After a cycle completes, a further call to _MoveNext_ will restart the cycle. This allows for action at the end of a complete cycle.
    
    TurnOrder<int> order;
    
    public void MoveNextExample()
    {
        while (!gameOver)
        {
            // Returns true if there is another pawn in the order who is yet to have a turn
            bool pawnsRemaining = order.MoveNext();
            while (pawnsRemaining)
            {
                // ... Do something ...
                // ... Update turn order interface, etc ...
            
                // Proceed to next pawn in order
                pawnsRemaining = order.MoveNext();
            }
        
            // Complete cycle of turn order complete!
        
            // ... Do something ...
            // ... Discard cards, resolve effects, replenish health, etc ...
        }
    }
    
### Checking whose turn it is

The _Current_ property provides a reference to the pawn whose turn it currently is. This is useful should other objects need to know if it is a particular pawns turn.

    TurnOrder<int> order;
    TurnBasedPawnExample examplePawn1;
    
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
    
### Removing pawns from the order

Pawns can be removed from the order at any time. If the currently acting pawn is removed, the _Current_ property will return null until _MoveNext_ is called to proceed to the next pawn in the order. _Remove_ will return true if the pawn was present in the order and has been removed.

    TurnOrder<int> order;
    TurnBasedPawnExample examplePawn1;
    
    public void RemovePawnExample()
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
        
            // Was it the removed pawn's turn?
            if (isTurn)
            {
                // Move on to next pawn in the order
                order.MoveNext();
            }
        }
    }
    
### Priority changes

Should a pawn's priority change, calling _UpdatePriority_ will reposition the given pawn into the correct place in the turn order.

    TurnOrder<int> order;
    TurnBasedPawnExample examplePawn1;
    TurnBasedPawnExample examplePawn2;
    
    public void UpdatePriorityExample()
    {
        // An empty order
        TurnOrder<int> order = new TurnOrder<int>();
    
        // Set priority of pawns
        examplePawn1.Priority = 12;
        exameplPawn2.Priority = 32;
    
        // Add the example pawns to the order. Pawn2 is first in order
        order.Insert(examplePawn1);
        order.Insert(examplePawn2);
    
        // Change pawn's priority. Order needs to update position of pawns 
        examplePawn1.Priority = 42;
        
        // Pawn positioned correctly in order. Pawn1 is first in order
        order.UpdatePriority(examplePawn1);
    }

### Multiple turn orders

The following is an example of multiple turn orders being used to simulate a team and unit scenario. The game consists of multiple teams which take turns to act. During a teams turn their units act in a specific order.

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
    
    
    public class Team : IPawn
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
    
