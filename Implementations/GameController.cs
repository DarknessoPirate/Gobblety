using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gobblety.Interfaces;
using Gobblety.Models;

namespace Gobblety.Implementations
{
    public class GameController
    {
        private readonly IUserInterface _ui;
        private readonly IGameBoard _board;
        private readonly IGameRules _rules;
        private readonly List<Player> _players;
        private int _currentPlayerIndex;
        
        public GameController(IUserInterface ui, IGameBoard board, IGameRules rules)
        {
            _ui = ui;
            _board = board;
            _rules = rules;
            _players = new List<Player>();
            
            // init players
            _players.Add(new Player(1, _rules.GeneratePlayerPieces(1)));
            _players.Add(new Player(2, _rules.GeneratePlayerPieces(2)));
        }
        
        public void StartGame()
        {
            _currentPlayerIndex = 0;
            bool gameOver = false;
            
            while (!gameOver)
            {
                Player currentPlayer = _players[_currentPlayerIndex];
                
                // display game state
                
                
                // get and process player move
                bool validMove = false;
                while (!validMove)
                {
                    _ui.DisplayBoard(_board);
                    _ui.DisplayPlayerInfo(currentPlayer);
                    Move? move = _ui.GetPlayerMove(currentPlayer, _board);
                    
                    if (move != null)
                    {
                        validMove = ProcessMove(move, currentPlayer);
                        if (validMove)
                        {
                            // check game over condition
                            if (_rules.IsGameOver(_board, out int winnerTeam))
                            {
                                _ui.DisplayWinner(winnerTeam);
                                _ui.DisplayBoard(_board);
                                gameOver = true;
                                break;
                            }
                            
                            // next player
                            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
                        }


                    }
                }
            }
        }
        
        private bool ProcessMove(Move move, Player player)
        {
            switch (move.Type)
            {
                case MoveType.Place:
                    return _rules.PlacePiece(_board, move.TargetPosition, move.Piece, player.Inventory);
                    
                case MoveType.Move:
                    return _rules.MovePiece(_board, move.SourcePosition, move.TargetPosition);
                    
                default:
                    return false;
            }
        }
    }
}