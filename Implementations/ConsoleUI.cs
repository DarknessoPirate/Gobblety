using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gobblety.Interfaces;
using Gobblety.Models;

namespace Gobblety.Implementations
{
    public class ConsoleUI : IUserInterface
    {
        public void DisplayBoard(IGameBoard board)
        {
            for (int row = 0; row < board.Size; row++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                for (int col = 0; col < board.Size; col++)
                {
                    var position = new Position(row, col);
                    Piece? topPiece = board.GetTopPieceAt(position);
                    
                    if (topPiece == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.ForegroundColor = topPiece.Team == 1 ? ConsoleColor.Red : ConsoleColor.Blue;
                        Console.Write(MapSizeToChar(topPiece.Size));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    
                    if (col < board.Size - 1)
                    {
                        Console.Write("|");
                    }
                }
                
                Console.WriteLine();
                
                if (row < board.Size - 1)
                {
                    Console.WriteLine("------");
                }
            }
        }
        
        public void DisplayPlayerInfo(Player player)
        {
            Console.ForegroundColor = player.TeamId == 1 ? ConsoleColor.Red : ConsoleColor.Blue;
            Console.WriteLine($"Player {player.TeamId}'s turn.");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        public void DisplayWinner(int teamId)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Player {teamId} wins!");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        public void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        public Move? GetPlayerMove(Player player, IGameBoard board)
        {
            Console.WriteLine("Choose the move type:");
            Console.WriteLine("1. Place a piece");
            Console.WriteLine("2. Move a piece");
            
            if (!int.TryParse(Console.ReadLine(), out int moveType) || (moveType != 1 && moveType != 2))
            {
                DisplayError("Invalid move type. Please choose 1 or 2.");
                return null;
            }
            
            // place a piece
            if (moveType == 1)
            {
                if (player.Inventory.Count == 0)
                {
                    DisplayError("You have no pieces to place");
                    return null;
                }
                
                Piece? selectedPiece = GetPieceSelection(player);
                if (selectedPiece == null)
                {
                    return null;
                }
                
                Position? targetPosition = GetPositionSelection(board.Size);
                if (targetPosition == null || targetPosition.Value.Row < 0)
                {
                    return null;
                }
                
                return new Move(targetPosition.Value, selectedPiece);
            }
            // move a piece
            else
            {
                Position? sourcePosition = GetSourcePiecePosition(board, player.TeamId);
                if (sourcePosition == null || sourcePosition.Value.Row < 0)
                {
                    return null;
                }
                
                Position? targetPosition = GetPositionSelection(board.Size);
                if (targetPosition == null || targetPosition.Value.Row < 0)
                {
                    return null;
                }
                
                return new Move(sourcePosition.Value, targetPosition.Value);
            }
        }
        
        private Piece? GetPieceSelection(Player player)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Available pieces:");
            Console.ForegroundColor = ConsoleColor.White;
            
            foreach (var piece in player.Inventory)
            {
                Console.Write(MapSizeToChar(piece.Size) + " ");
            }
            Console.WriteLine();
            
            if (!char.TryParse(Console.ReadLine()?.ToLower() ?? "", out char chosenSizeChar))
            {
                DisplayError("Invalid input. Please enter a valid piece size (s, m, b).");
                return null;
            }
            
            try
            {
                int chosenSize = MapCharToSize(chosenSizeChar);
                Piece? chosenPiece = player.Inventory.FirstOrDefault(p => p.Size == chosenSize);
                
                if (chosenPiece == null)
                {
                    DisplayError("No piece of that size available.");
                    return null;
                }
                
                return chosenPiece;
            }
            catch (ArgumentException)
            {
                DisplayError("Invalid size character. Please choose from: s (small), m (medium), b (big).");
                return null;
            }
        }
        
        private Position GetPositionSelection(int boardSize)
        {
            Console.WriteLine("Choose the field to place/move the piece (1-9):");
            
            if (!int.TryParse(Console.ReadLine(), out int square) || square < 1 || square > boardSize * boardSize)
            {
                DisplayError($"Invalid position. Please enter a number between 1 and {boardSize * boardSize}.");
                return new Position(-1, -1); // invalid position
            }
            
            return Position.FromSquareNumber(square, boardSize);
        }
        
        private Position? GetSourcePiecePosition(IGameBoard board, int teamId) 
        {
            Console.WriteLine("Choose the piece to move (1-9):");
            
            string? input = Console.ReadLine(); 
            if (input == null || !int.TryParse(input, out int square) || square < 1 || square > board.Size * board.Size)
            {
                DisplayError($"Invalid position. Please enter a number between 1 and {board.Size * board.Size}.");
                return null;
            }
            
            Position position = Position.FromSquareNumber(square, board.Size);
            
            if (!board.HasPieces(position))
            {
                DisplayError("No piece to move in the selected field.");
                return null;
            }
            
            Piece? topPiece = board.GetTopPieceAt(position); 
            if (topPiece == null || topPiece.Team != teamId)
            {
                DisplayError("You can only move your own pieces.");
                return null;
            }
            
            return position;
        }
        
        private char MapSizeToChar(int size)
        {
            return size switch
            {
                1 => 's',
                2 => 'm',
                3 => 'b',
                _ => throw new ArgumentException("Invalid size")
            };
        }
        
        private int MapCharToSize(char sizeChar)
        {
            return sizeChar switch
            {
                's' => 1,
                'm' => 2,
                'b' => 3,
                _ => throw new ArgumentException("Invalid size character")
            };
        }
    }
}