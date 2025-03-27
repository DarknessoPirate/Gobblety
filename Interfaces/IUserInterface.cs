using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gobblety.Models;

namespace Gobblety.Interfaces
{
    public interface IUserInterface
    {
        void DisplayBoard(IGameBoard board);
        void DisplayPlayerInfo(Player player);
        void DisplayWinner(int teamId);
        Move? GetPlayerMove(Player player, IGameBoard board);
        void DisplayError(string message);
    }
}