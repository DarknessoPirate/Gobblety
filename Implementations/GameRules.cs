using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gobblety.Interfaces;
using Gobblety.Models;

namespace Gobblety.Implementations
{
    public class GameRules : IGameRules
    {
        public List<Piece> GeneratePlayerPieces(int teamId)
        {
            var pieces = new List<Piece>();
            
            // 2 small, 2 medium, 2 large pieces
            for (int i = 0; i < 2; i++)
            {
                pieces.Add(new Piece(teamId, 1)); // Small
                pieces.Add(new Piece(teamId, 2)); // Medium
                pieces.Add(new Piece(teamId, 3)); // Large
            }
            
            return pieces;
        }
        
        public bool PlacePiece(IGameBoard board, Position position, Piece piece, List<Piece> inventory)
        {
            if (board.HasPieces(position))
            {
                Piece? topPiece = board.GetTopPieceAt(position); 
                if (topPiece != null && !CanPlaceOnTop(piece, topPiece)) 
                {
                    return false;
                }
            }
            
            board.AddPiece(position, piece);
            inventory.Remove(piece);
            return true;
        }
        
        public bool MovePiece(IGameBoard board, Position source, Position target)
        {
            Piece? pieceToMove = board.GetTopPieceAt(source); 
            if (pieceToMove == null) 
            {
                return false;
            }
            
            if (board.HasPieces(target))
            {
                Piece? topPiece = board.GetTopPieceAt(target);
                if (topPiece != null && !CanPlaceOnTop(pieceToMove, topPiece)) 
                {
                    return false;
                }
            }
            
            board.RemovePiece(source, pieceToMove);
            board.AddPiece(target, pieceToMove);
            return true;
        }
        
        public bool CanPlaceOnTop(Piece pieceToPlace, Piece existingPiece)
        {
            // can only place larger pieces on top of smaller ones
            return pieceToPlace.Size > existingPiece.Size;
        }
        
        public bool IsGameOver(IGameBoard board, out int winnerTeam)
        {
            winnerTeam = 0;
            
            // check rows
            for (int row = 0; row < board.Size; row++)
            {
                if (CheckLine(board, row, 0, 0, 1, out int rowWinner))
                {
                    winnerTeam = rowWinner;
                    return true;
                }
            }
            
            // check columns
            for (int col = 0; col < board.Size; col++)
            {
                if (CheckLine(board, 0, col, 1, 0, out int colWinner))
                {
                    winnerTeam = colWinner;
                    return true;
                }
            }
            
            // check diagonals
            if (CheckLine(board, 0, 0, 1, 1, out int diag1Winner))
            {
                winnerTeam = diag1Winner;
                return true;
            }
            
            if (CheckLine(board, 0, board.Size - 1, 1, -1, out int diag2Winner))
            {
                winnerTeam = diag2Winner;
                return true;
            }
            
            return false;
        }
        
        private bool CheckLine(IGameBoard board, int startRow, int startCol, int rowInc, int colInc, out int winner)
        {
            winner = 0;
            
            // ensure starting pos has a piece
            var startPos = new Position(startRow, startCol);
            if (!board.HasPieces(startPos))
            {
                return false;
            }
            
            Piece? startPiece = board.GetTopPieceAt(startPos); 
            if (startPiece == null) 
            {
                return false;
            }
            
            int team = startPiece.Team;
            
            for (int i = 1; i < board.Size; i++)
            {
                var pos = new Position(startRow + i * rowInc, startCol + i * colInc);
                
                Piece? currentPiece = board.GetTopPieceAt(pos); 
                if (!board.HasPieces(pos) || currentPiece == null || currentPiece.Team != team) 
                {
                    return false;
                }
            }
            
            winner = team;
            return true;
        }
    }
}