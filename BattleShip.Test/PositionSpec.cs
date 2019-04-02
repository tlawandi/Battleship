using System;
using Battleship;
using FluentAssertions;
using Xunit;

namespace BattleShip.Test
{
    public class PositionSpec
    {
        [Fact]
        public void CreationOfStraightLinePositionSucceeds()
        {
            var startCoordinate = new Coordinate(1, 1);
            var endCoordinate = new Coordinate(1, 4);

            Action boardCreation = () => new Position(startCoordinate, endCoordinate);

            boardCreation.Should().NotThrow<ArgumentException>();
        }

        [Fact]
        public void CreationOfNonStraightPositionFails()
        {
            var startCoordinate = new Coordinate(1, 1);
            var endCoordinate = new Coordinate(4, 4);

            Action boardCreation = () => new Position(startCoordinate, endCoordinate);

            boardCreation.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void TestingEqualityOfPosition()
        {
            var startCoordinate1 = new Coordinate(1, 1);
            var endCoordinate1 = new Coordinate(1, 4);

            var position1 = new Position(startCoordinate1, endCoordinate1);
            var startCoordinate2 = new Coordinate(1, 1);
            var endCoordinate2 = new Coordinate(1, 4);

            var position2 = new Position(startCoordinate2, endCoordinate2);

            position1.Should().Equals(position2);

            position1.Equals(position2).Should().BeTrue();
            (position1 == position2).Should().BeTrue();
        }

        [Fact]
        public void WhenCreatingNewPosition_OccupiedCoordinatesIsValid()
        {

            var startCoordinate = new Coordinate(1, 1);
            var endCoordinate = new Coordinate(1, 4);

            var position = new Position(startCoordinate, endCoordinate);

            position.OccupiedCoordinates.Contains(startCoordinate).Should().BeTrue();
            position.OccupiedCoordinates.Contains(new Coordinate(1, 2)).Should().BeTrue();
            position.OccupiedCoordinates.Contains(new Coordinate(1, 3)).Should().BeTrue();
            position.OccupiedCoordinates.Contains(endCoordinate).Should().BeFalse();
        }

        //Does Position Intersect and coordinate intersect are better tested through aggregate root which is the Board.
        //Its more of an integration style tests that I believe yields best coverage for this scenario
    }
}