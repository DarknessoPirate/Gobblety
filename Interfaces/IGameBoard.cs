using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gobblety.Models;

namespace Gobblety.Interfaces
{
    public interface IGameBoard
    {
        int Size { get; }
        List<Piece> GetPiecesAt(Position position);
        Piece? GetTopPieceAt(Position position);
        bool HasPieces(Position position);
        void AddPiece(Position position, Piece piece);
        bool RemovePiece(Position position, Piece piece);
        bool ContainsPiece(Piece piece, out Position position);
    }
}