using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gobblety.Interfaces;
using Gobblety.Models;

namespace Gobblety.Implementations
{
     public class GameBoard : IGameBoard
    {
        private readonly List<Piece>[,] _grid;
        
        public int Size { get; }
        
        public GameBoard(int size)
        {
            Size = size;
            _grid = new List<Piece>[size, size];
            
            // init grid
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _grid[i, j] = new List<Piece>();
                }
            }
        }
        
        public List<Piece> GetPiecesAt(Position position) => _grid[position.Row, position.Col];
        
        public Piece? GetTopPieceAt(Position position)
        {
            var pieces = _grid[position.Row, position.Col];
            return pieces.Count > 0 ? pieces[pieces.Count - 1] : null;
        }
        public bool HasPieces(Position position) => _grid[position.Row, position.Col].Count > 0;
        
        public void AddPiece(Position position, Piece piece)
        {
            _grid[position.Row, position.Col].Add(piece);
        }
        
        public bool RemovePiece(Position position, Piece piece)
        {
            return _grid[position.Row, position.Col].Remove(piece);
        }
        
        public bool ContainsPiece(Piece piece, out Position position)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (_grid[i, j].Contains(piece))
                    {
                        position = new Position(i, j);
                        return true;
                    }
                }
            }
            
            position = default;
            return false;
        }
    }
}