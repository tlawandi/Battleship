using System;
using System.IO;
using System.Text;
using Battleship.Common;

namespace Battleship
{
    public class Board
    {
        private const string ShipStartPositionViolationMessage = "Ship start position is outside the board's dimensions. Please place the ship on the board";
        private const string ShipDoesNotFitViolationMessage = "Ship does not fit on the board. Please choose a smaller ship or a better starting coordinate.";
        private const string GameAlreadyLostMessage = "Game has already been lost";

        private readonly int _horizontalSize;
        private readonly int _verticalSize; 

        // My gut feel is that player can be separated into its own bounded context or micro service.
        // I don't want to make the decision too early. As more requirements come in we can decide then.
        private readonly Player _player;
        private readonly Fleet _fleet;

        public Board(int horizontalSize, int verticalSize)
        {
            _horizontalSize = horizontalSize;
            _verticalSize = verticalSize;
            _fleet = new Fleet();
            _player = new Player(_fleet);
        }

        public bool IsGameLost()
        {
            return _player.GetStatus() == PlayerStatus.Lost;
        }

        public TurnResponse IncomingAttack(int x, int y)
        {
            if (_player.GetStatus() == PlayerStatus.Lost)
            {
                throw new ApplicationException(GameAlreadyLostMessage);
            }

            //TODO: possibly need to store hit register. throw exception if coordinate hit twice.
            var coordinate = new Coordinate(x, y);
            return _fleet.IncomingAttack(coordinate);
        }

        public Result AddShip(int shipSize, Orientation orientation, int horizontalCoordinate, int verticalCoordinate)
        {
            var coordinate = new Coordinate(horizontalCoordinate, verticalCoordinate);
            var battleship = new Battleship(coordinate, orientation, shipSize);
            var result = IsPositionValid(battleship);
            if (result.IsSuccess)
            {
                result = _fleet.AddShip(battleship);
            }

            return result;
        }

        private Result IsPositionValid(Battleship newlyAddedBattleship)
        {
            var shipStartingPositionOutsideBoard = IsShipStartingPositionOutsideBoard(newlyAddedBattleship);
            if(shipStartingPositionOutsideBoard)
                return Result.Fail(ShipStartPositionViolationMessage);

            var shipDoesNotFitOnBoard = DoesShipFitOnBoard(newlyAddedBattleship);
            if(shipDoesNotFitOnBoard)
                return Result.Fail(ShipDoesNotFitViolationMessage);

            return Result.Ok();
            
        }

        private bool IsShipStartingPositionOutsideBoard(Battleship newlyAddedBattleship)
        {
            return newlyAddedBattleship.Position.StartCoordinate.XCoordinate >= _horizontalSize ||
                   newlyAddedBattleship.Position.StartCoordinate.YCoordinate >= _verticalSize;
        }

        private bool DoesShipFitOnBoard(Battleship newlyAddedBattleship)
        {
            return newlyAddedBattleship.Position.EndCoordinate.XCoordinate >= _horizontalSize ||
                newlyAddedBattleship.Position.EndCoordinate.YCoordinate >= _verticalSize;
        }
    }
}
