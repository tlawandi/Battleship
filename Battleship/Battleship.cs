using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Battleship
{
    public class Battleship 
    {
        private const string InvalidShipSizeMessage = "Ship size should be at minimum one";
        private const string CoordinateAlreadyHitMessage = "Coordinate has already been hit";
        private const string ShipAlreadySunkMessage = "Ship is already sunk";

        private readonly int _shipSize;
        private readonly IList<Coordinate> _damageCoordinates;

        public ShipStatus Status { get; private set; }
        public Position Position { get; }


        public Battleship(Coordinate startCoordinate, Orientation orientation, int shipSize) 
        {
            if(shipSize < 1)
                throw new InvalidOperationException(InvalidShipSizeMessage);
            _shipSize = shipSize;

            Position = CalculatePosition(startCoordinate, orientation, shipSize);
            _damageCoordinates = new List<Coordinate>();
            Status = ShipStatus.Alive;
        }

        private Position CalculatePosition(Coordinate startCoordinate, Orientation orientation, int shipSize)
        {
            Coordinate endCoordinate;
            if (orientation == Orientation.Vertical)
            {
                endCoordinate = new Coordinate(startCoordinate.XCoordinate, startCoordinate.YCoordinate + shipSize); 
            }
            else
            {
                endCoordinate = new Coordinate(startCoordinate.XCoordinate + shipSize, startCoordinate.YCoordinate);
            }

            return new Position(startCoordinate, endCoordinate);
        }                      

        public TurnResponse IncomingAttack(Coordinate coordinate)   
        {
            var doesCoordinateIntersect = Position.DoesCoordinateIntersect(coordinate);
            if (doesCoordinateIntersect)
            {
                if (Status == ShipStatus.Sunk)
                    throw new InvalidOperationException(ShipAlreadySunkMessage);

                SufferedDamage(coordinate);
                return TurnResponse.Hit;
            }

            return TurnResponse.Miss;
        }

        private void SufferedDamage(Coordinate coordinate)
        {
            if (_damageCoordinates.Contains(coordinate))
                throw new InvalidOperationException(CoordinateAlreadyHitMessage);

            _damageCoordinates.Add(coordinate);

            if (_damageCoordinates.Count == _shipSize)
            {
                Status = ShipStatus.Sunk;
            }
        }
    }
}
