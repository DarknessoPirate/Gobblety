using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gobblety.Models
{
    public readonly struct Position
    {
        public readonly int Row;
        public readonly int Col;
        
        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }
        
        public static Position FromSquareNumber(int square, int boardSize)
        {
            int row = (square - 1) / boardSize;
            int col = (square - 1) % boardSize;
            return new Position(row, col);
        }
        
        public int ToSquareNumber(int boardSize)
        {
            return Row * boardSize + Col + 1;
        }
    }
}