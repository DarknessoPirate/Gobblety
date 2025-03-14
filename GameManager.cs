using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Gobblety
{
    public class GameManager
    {
        private int currentPlayer;
        private int winnerTeam;
        private int boardSize = 3;
        private List<Piece>[,] board = new List<Piece>[3, 3];
        private List<Piece> player1Pieces = new();
        private List<Piece> player2Pieces = new();

        public GameManager()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = new List<Piece>();
                }
            }
        }


        public void StartGame()
        {
            currentPlayer = 1;
            GeneratePlayingPieces(player1Pieces, 1);
            GeneratePlayingPieces(player2Pieces, 2);
            GameLoop();
            return;
        }

        private void GameLoop()
        {
            bool gameOver = false;
            

            while (!gameOver)
            {
                bool validMove = false;
                // player can move until he makes a valid move
                while (!validMove)
                {
                    PrintBoard();
                    PrintInfo();
                    var playerPieces = currentPlayer == 1 ? player1Pieces : player2Pieces;
                    var move = GetPlayerMove(playerPieces);

                    if (move != null)
                    {
                        if (move.Item4 == 1)
                        {
                            validMove = PlacePiece(move.Item1, move.Item2, move.Item3, playerPieces);
                        }
                        else if (move.Item4 == 2)
                        {
                            validMove = MovePiece(move.Item1, move.Item2, move.Item3);
                        }

                        if (validMove)
                        {
                            // gameover check
                            if (IsGameOver())
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Player {winnerTeam} wins!");
                                Console.ForegroundColor = ConsoleColor.White;
                                gameOver = true;
                                PrintBoard();
                                break;
                            }

                            // next player
                            currentPlayer = currentPlayer == 1 ? 2 : 1;
                        }
                    }
                }
            }
        }

        private Tuple<int, int, Piece, int> GetPlayerMove(List<Piece> playerPieces)
        {
            Console.WriteLine("Choose the move type:");
            Console.WriteLine("1. Place a piece");
            Console.WriteLine("2. Move a piece");
            int moveType = int.Parse(Console.ReadLine());

            // if inventory empty
            if (moveType == 1 && playerPieces.Count == 0)
            {
                Console.WriteLine("You have no pieces to place");
                return null;
            }

            Piece chosenPiece = null;
            if (moveType == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Available pieces:");
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var piece in playerPieces)
                {
                    Console.Write(MapSizeToChar(piece.Size) + " ");
                }
                Console.WriteLine();

                char chosenSizeChar = char.Parse(Console.ReadLine().ToLower());
                int chosenSize = MapCharToSize(chosenSizeChar);
                chosenPiece = playerPieces.First(p => p.Size == chosenSize);
            }
            else if (moveType == 2)
            {
                Console.WriteLine("Choose the piece to move (1-9):");
                int square = int.Parse(Console.ReadLine());
                int row = (square - 1) / 3;
                int col = (square - 1) % 3;

                // if selected empty square
                if (board[row, col].Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("No piece to move in the selected field.");
                    Console.ForegroundColor = ConsoleColor.White;
                    return null;
                }

                chosenPiece = board[row, col].Last();

                // check if piece belongs to the current player
                if (chosenPiece.Team != currentPlayer)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You can only move your own pieces.");
                    Console.ForegroundColor = ConsoleColor.White;
                    return null;
                }
            }

            Console.WriteLine("Choose the field to place/move the piece (1-9):");
            int targetSquare = int.Parse(Console.ReadLine());
            int targetRow = (targetSquare - 1) / 3;
            int targetCol = (targetSquare - 1) % 3;

            return Tuple.Create(targetRow, targetCol, chosenPiece, moveType);
        }

        private void PrintBoard()
        {
            for (int row = 0; row < boardSize; row++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                for (int col = 0; col < boardSize; col++)
                {
                    var topPiece = board[row, col].LastOrDefault();
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

                    if (col < boardSize - 1)
                    {
                        Console.Write("|");
                    }
                }
                Console.WriteLine();
                if (row < boardSize - 1)
                {
                    Console.WriteLine("------");
                }
            }
        }

        // place the piece logic
        private bool PlacePiece(int row, int col, Piece piece, List<Piece> playerPieces)
        {
            if (board[row, col].Count > 0)
            {
                var topPiece = board[row, col].Last();

                // chck if piece can be placed on top of a piece
                if ((piece.Size == 1 && topPiece.Size == 1) || // small on small
                    (piece.Size == 1 && topPiece.Size == 2) || // small onmedium
                    (piece.Size == 2 && topPiece.Size == 2) || // medium on medium
                    (piece.Size == 2 && topPiece.Size == 3) || // medium on large
                    (piece.Size == 3 && topPiece.Size == 3))   // large on large
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Cannot place a smaller or same size piece on top of another piece.");
                    Console.ForegroundColor = ConsoleColor.White;
                    return false;
                }
            }

            board[row, col].Add(piece);
            playerPieces.Remove(piece);
            return true;
        }

        // list check and piece moving logic, returns true if move succeeded, false otherwise
        private bool MovePiece(int row, int col, Piece piece)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (board[i, j].Contains(piece))
                    {
                        board[i, j].Remove(piece);
                        if (board[row, col].Count > 0)
                        {
                            var topPiece = board[row, col].Last();

                            // Check if the piece can be placed on top of the existing piece
                            if ((piece.Size == 1 && topPiece.Size == 1) || // small on small
                                (piece.Size == 1 && topPiece.Size == 2) || // small on medium
                                (piece.Size == 2 && topPiece.Size == 2) || // medium onmedium
                                (piece.Size == 2 && topPiece.Size == 3) || // medium on large
                                (piece.Size == 3 && topPiece.Size == 3))   // large on large
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Cannot place a smaller or same size piece on top of another piece.");
                                Console.ForegroundColor = ConsoleColor.White;
                                board[i, j].Add(piece);// put piece back
                                return false;
                            }
                        }
                        board[row, col].Add(piece);
                        return true;
                    }
                }
            }
            return false;
        }

        private void PrintInfo()
        {
            if (currentPlayer == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Player {currentPlayer}'s turn.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Player {currentPlayer}'s turn.");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        // this could be optimized to only check on relevant fields on each move
        private bool IsGameOver()
        {
            // rows
            for (int row = 0; row < boardSize; row++)
            {
                if (board[row, 0].Count > 0 && board[row, 1].Count > 0 && board[row, 2].Count > 0)
                {
                    var team = board[row, 0].Last().Team;
                    if (board[row, 1].Last().Team == team && board[row, 2].Last().Team == team)
                    {
                        winnerTeam = team;
                        return true;
                    }
                }
            }

            // columns
            for (int col = 0; col < boardSize; col++)
            {
                if (board[0, col].Count > 0 && board[1, col].Count > 0 && board[2, col].Count > 0)
                {
                    var team = board[0, col].Last().Team;
                    if (board[1, col].Last().Team == team && board[2, col].Last().Team == team)
                    {
                        winnerTeam = team;
                        return true;
                    }
                }
            }

            // diagonals
            if (board[0, 0].Count > 0 && board[1, 1].Count > 0 && board[2, 2].Count > 0)
            {
                var team = board[0, 0].Last().Team;
                if (board[1, 1].Last().Team == team && board[2, 2].Last().Team == team)
                {
                    winnerTeam = team;
                    return true;
                }
            }

            if (board[0, 2].Count > 0 && board[1, 1].Count > 0 && board[2, 0].Count > 0)
            {
                var team = board[0, 2].Last().Team;
                if (board[1, 1].Last().Team == team && board[2, 0].Last().Team == team)
                {
                    winnerTeam = team;
                    return true;
                }
            }

            return false;
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

        private void GeneratePlayingPieces(List<Piece> playerPieces, int teamNumber)
        {
            playerPieces.Add(new Piece(teamNumber, 1));
            playerPieces.Add(new Piece(teamNumber, 1));
            playerPieces.Add(new Piece(teamNumber, 2));
            playerPieces.Add(new Piece(teamNumber, 2));
            playerPieces.Add(new Piece(teamNumber, 3));
            playerPieces.Add(new Piece(teamNumber, 3));
        }

    }
}