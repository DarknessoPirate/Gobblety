using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gobblety.Models
{
     public class Move
    {
        public MoveType Type { get; }
        public Position SourcePosition { get; }
        public Position TargetPosition { get; }
        public Piece Piece { get; }
        

        public Move(Position targetPosition, Piece piece)
        {
            Type = MoveType.Place;
            TargetPosition = targetPosition;
            Piece = piece;
        }
        
        public Move(Position sourcePosition, Position targetPosition)
        {
            Type = MoveType.Move;
            SourcePosition = sourcePosition;
            TargetPosition = targetPosition;
        }
    }
}