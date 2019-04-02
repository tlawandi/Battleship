using System.Collections.Generic;
using System.Linq;
using Battleship.Common;

namespace Battleship
{
    public class Fleet
    {
        private const string ShipCannotBePlacedOnTopOfEachOtherMessage = "Ship cannot be placed on top of each other.";

        private readonly IList<Battleship> _battleships;

        public Fleet()
        {
            _battleships = new List<Battleship>();
        }

        public TurnResponse IncomingAttack(Coordinate coordinate)
        {
            return _battleships.Any(a => a.IncomingAttack(coordinate) == TurnResponse.Hit)
                ? TurnResponse.Hit
                : TurnResponse.Miss;
        }

        public Result AddShip(Battleship battleship)
        {
            var result = CanAddShip(battleship);
            if (result.IsSuccess)
            {
                _battleships.Add(battleship);
            }

            return result;
        }

        private Result CanAddShip(Battleship battleship)
        {
            var positionIntersects = _battleships.Any(x => x.Position.DoesPositionIntersect(battleship.Position));
            return positionIntersects ? Result.Fail(ShipCannotBePlacedOnTopOfEachOtherMessage) : Result.Ok();
        }

        public FleetStatus GetStatus()
        {
            return _battleships.All(x => x.Status == ShipStatus.Sunk) ? FleetStatus.Destroyed : FleetStatus.Intact;
        }
    }
}