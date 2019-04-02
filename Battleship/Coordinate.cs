using System.Collections.Generic;
using Battleship.Common;

namespace Battleship
{
    public class Coordinate : ValueObject<Coordinate>   
    {
        public int XCoordinate { get;  }
        public int YCoordinate { get;  }

        public Coordinate(int x, int y)
        {
            XCoordinate = x;
            YCoordinate = y;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return XCoordinate;
            yield return YCoordinate;
        }
    }
}