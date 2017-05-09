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
    
    public class TurnBasedPawnExample : ITurnBasedPawn
    {
        ...
        
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
        TurnOrder order = new TurnOrder();
    
        // Add the example pawns to the order
        order.Insert(examplePawn);
        order.Insert(examplePawn);
    }
    