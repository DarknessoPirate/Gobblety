using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gobblety
{
    public class Piece
    {

        public readonly int Team;
        public readonly int Size;
        public Piece(int team, int size)
        {
            this.Team = team;
            this.Size = size;
        }
    }
}