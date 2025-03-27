using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gobblety.Models
{
    public class Player(int teamId, List<Piece> inventory)
    {
        public int TeamId { get; } = teamId;
        public List<Piece> Inventory { get; } = inventory;
    }
}