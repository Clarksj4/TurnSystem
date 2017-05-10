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
    
Subsequent calls to _MoveNext_ cycle through the pawns in the order. The current pawn's _TurnEnd_ method is called, and the next pawn's _TurnStart_ method is called. After a cycle completes, a further call to _MoveNext_ will restart the cycle. This allows for action at the end of a complete cycle.
    
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
                order.MoveNext();
            }
        
            // Complete cycle of turn order complete!
        
            // ... Do something ...
            // ... Discard cards, resolve effects, replenish health, etc ...
        }
    }
    
