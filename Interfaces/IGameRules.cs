using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gobblety;
using Gobblety.Interfaces;
using Gobblety.Models;

namespace Gobblety.Interfaces
{
    public interface IGameRules
    {
        List<Piece> GeneratePlayerPieces(int teamId);
        bool PlacePiece(IGameBoard board, Position position, Piece piece, List<Piece> inventory);
        bool MovePiece(IGameBoard board, Position source, Position target);
        bool IsGameOver(IGameBoard board, out int winnerTeam);
        bool CanPlaceOnTop(Piece pieceToPlace, Piece existingPiece);
    }
}

