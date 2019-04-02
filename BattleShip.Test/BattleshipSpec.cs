using System;
using System.Security.Cryptography.X509Certificates;
using Battleship;
using FluentAssertions;
using Xunit;

namespace BattleShip.Test
{
    public class BattleshipSpec
    {

        [Fact]
        public void InvalidBattleshipSizeThrowsException()
        {
            var startCoordinate = new Coordinate(0,0);
            Action battleshipCreation = () => new Battleship.Battleship(startCoordinate, Orientation.Vertical, 0);

            battleshipCreation.Should().Throw<InvalidOperationException>(); //I didn't test for message as I feel this makes the test more brittle

        }

        [Fact]
        public void BattleshipPositionIsCorrectForHorizontal()
        {
            var battleshipSpaces = 4;
            var startCoordinate = new Coordinate(1, 1);

            var battleship = new Battleship.Battleship(startCoordinate, Orientation.Vertical, battleshipSpaces);

            var expectedEndCoordinate = new Coordinate(startCoordinate.XCoordinate, startCoordinate.YCoordinate + battleshipSpaces);

            battleship.Position.StartCoordinate.Should().Be(startCoordinate);
            battleship.Position.EndCoordinate.Should().Be(expectedEndCoordinate);
        }

        [Fact]
        public void BattleshipPositionIsCorrectForVerticalOrientation()
        {
            var battleshipSpaces = 4;
            var startCoordinate = new Coordinate(1, 1);

            var battleship = new Battleship.Battleship( startCoordinate, Orientation.Horizontal, battleshipSpaces);

            var expectedEndCoordinate = new Coordinate(startCoordinate.XCoordinate + battleshipSpaces, startCoordinate.YCoordinate );

            battleship.Position.StartCoordinate.Should().Be(startCoordinate);
            battleship.Position.EndCoordinate.Should().Be(expectedEndCoordinate);
        }

        [Fact]
        public void WhenAllOccupiedPositionOnTheShipAreDamaged_ShipIsSunk()
        {
            var battleshipSpaces = 2;
            var orientation = Orientation.Horizontal;
            var xCoordinate = 1;
            var yCoordinate = 1;

            var startCoordinate = new Coordinate(xCoordinate, yCoordinate);
            var battleship = new Battleship.Battleship(startCoordinate, orientation, battleshipSpaces);

            var response = battleship.IncomingAttack(startCoordinate);
            response.Should().Be(TurnResponse.Hit);
            battleship.Status.Should().Be(ShipStatus.Alive);


            response = battleship.IncomingAttack(new Coordinate(xCoordinate + 1, yCoordinate));
            response.Should().Be(TurnResponse.Hit);
            battleship.Status.Should().Be(ShipStatus.Sunk);

        }
    }
}
