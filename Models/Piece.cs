using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gobblety.Models
{
    public class Piece(int team, int size)
    {
        
        public int Team { get; } = team;

        // 1 for small, 2 for medium, 3 for large
        public int Size { get; } = size;

        // helper to get a string representation of the size

        public string GetSizeName()
        {
            return Size switch
            {
                1 => "Small",
                2 => "Medium",
                3 => "Large",
                _ => "Unknown"
            };
        }
        
        // override ToString for easier debugging
        public override string ToString()
        {
            return $"Piece(Team: {Team}, Size: {GetSizeName()})";
        }
        
        // override Equals for proper comparison
        public override bool Equals(object obj)
        {
            if (obj is not Piece other)
                return false;
                
            return Team == other.Team && Size == other.Size;
        }
        
    }
}