using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Battleship;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace BattleShip.Test
{

    public class TestHelper
    {
        public static Board CreateBoard(int? horizontalSize=null, int? verticalSize=null)
        {
            return new Board(horizontalSize ?? 10, verticalSize ?? 10);
        }
    }

    public class BoardSpec
    {

        [Fact]
        public void CreateABoardSuccessfully()
        {

            Action boardCreation = () => TestHelper.CreateBoard();

            boardCreation.Should().NotThrow<Exception>();
        }

        [Fact]
        public void AddBattleShip_AddsAShipToBoardSuccessfully()
        {
            var shipSize = 4;
            var orientation = Orientation.Vertical;
            var xCoordinate = 1;
            var yCoordinate = 1;

            var board = TestHelper.CreateBoard();

            var result = board.AddShip(shipSize, orientation, xCoordinate, yCoordinate);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void AddBattleshipToTheBoardThatIsOutsideTheBoardDimensionsFails()
        {
            var errorMessage =
                "Ship start position is outside the board's dimensions. Please place the ship on the board";
            var shipSize = 4;
            var orientation = Orientation.Vertical;
            var xCoordinate = 10;
            var yCoordinate = 1;

            var board = TestHelper.CreateBoard();

            var result = board.AddShip(shipSize, orientation, xCoordinate, yCoordinate);
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(errorMessage);
        }

        [Fact]
        public void AddBattleshipToTheBoardThatHangsOutsideTheBoardFails()
        {
            var shipSize = 4;
            var orientation = Orientation.Vertical;
            var xCoordinate = 1;
            var yCoordinate = 8;

            var board = TestHelper.CreateBoard();
            var result = board.AddShip(shipSize, orientation, xCoordinate, yCoordinate);
            result.IsSuccess.Should().BeFalse();
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        [InlineData(5, true)]
        public void Add2BattleshipToTheBoardThatOverlapFails(int ship2YCoordinate, bool successResult)
        {            
            var shipSize = 4;
            var ship1Orientation = Orientation.Vertical;
            var ship2Orientation = Orientation.Horizontal;
            var ship1XCoordinate = 3;
            var ship1YCoordinate = 1;
            var ship2XCoordinate = 2;

            var board = TestHelper.CreateBoard();
            var initialAddShip = board.AddShip(shipSize, ship1Orientation, ship1XCoordinate, ship1YCoordinate);
            var result = board.AddShip(shipSize, ship2Orientation, ship2XCoordinate, ship2YCoordinate);
            initialAddShip.IsSuccess.Should().BeTrue();
            result.IsSuccess.Should().Be(successResult);
        }

        [Fact]
        public void Add2BattleshipToTheBoardThatOverlapAtMoreThanOnePointFails()
        {
            var shipSize = 4;
            var ship1Orientation = Orientation.Vertical;
            var ship2Orientation = Orientation.Vertical;
            var ship1XCoordinate = 3;
            var ship1YCoordinate = 1;
            var ship2XCoordinate = 3;
            var ship2YCoordinate = 2;

            var board = TestHelper.CreateBoard();
            var initialAddShip = board.AddShip(shipSize, ship1Orientation, ship1XCoordinate, ship1YCoordinate);
            var result = board.AddShip(shipSize, ship2Orientation, ship2XCoordinate, ship2YCoordinate);
            initialAddShip.IsSuccess.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Add2BattleshipToTheBoardThatDoNotOverlapSucceeds()
        {
            var shipSize = 4;
            var ship1Orientation = Orientation.Vertical;
            var ship2Orientation = Orientation.Vertical;
            var ship1XCoordinate = 3;
            var ship1YCoordinate = 1;
            var ship2XCoordinate = 4;
            var ship2YCoordinate = 2;

            var board = TestHelper.CreateBoard();
            var initialAddShip = board.AddShip(shipSize, ship1Orientation, ship1XCoordinate, ship1YCoordinate);
            var result = board.AddShip(shipSize, ship2Orientation, ship2XCoordinate, ship2YCoordinate);
            initialAddShip.IsSuccess.Should().BeTrue();
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void IncomingAttackOnEmptyBoardPosition_ResultsInAMiss()
        {
            var shipSize = 4;
            var shipOrientation = Orientation.Vertical;
            var shipXCoordinate = 3;
            var shipYCoordinate = 1;


            var board = TestHelper.CreateBoard();
            var addShipResult = board.AddShip(shipSize, shipOrientation, shipXCoordinate, shipYCoordinate);

            var attackResponse = board.IncomingAttack(4, 1);

            attackResponse.Should().Be(TurnResponse.Miss);
            board.IsGameLost().Should().BeFalse();
        }

        [Theory]
        [InlineData(3,1)]
        [InlineData(3,2)]
        [InlineData(3,3)]
        [InlineData(3,4)]

        public void IncomingAttackOnOccupiedBoardPosition_ResultsInAHit(int attackXCoordinate, int attackYCoordinate)
        {
            
            var shipSize = 4;
            var shipOrientation = Orientation.Vertical;
            var shipXCoordinate = 3;
            var shipYCoordinate = 1;


            var board = TestHelper.CreateBoard();
            var addShipResult = board.AddShip(shipSize, shipOrientation, shipXCoordinate, shipYCoordinate);

            var attackResponse = board.IncomingAttack(attackXCoordinate, attackYCoordinate);

            addShipResult.IsSuccess.Should().BeTrue();
            attackResponse.Should().Be(TurnResponse.Hit);
            board.IsGameLost().Should().BeFalse();
        }

        [Fact]
        public void WhenAllShipsAreSunk_ThenGameIsLost()
        {            
            var shipSize = 4;
            var shipOrientation = Orientation.Vertical;
            var shipXCoordinate = 3;
            var shipYCoordinate = 1;

            var ship2Size = 1;
            var ship2Orientation = Orientation.Horizontal;
            var ship2XCoordinate = 8;
            var ship2YCoordinate = 8;


            var board = TestHelper.CreateBoard();
            var ship1Result = board.AddShip(shipSize, shipOrientation, shipXCoordinate, shipYCoordinate);

            ship1Result.IsSuccess.Should().BeTrue();

            var ship2Result = board.AddShip(ship2Size, ship2Orientation, ship2XCoordinate, ship2YCoordinate);
            ship2Result.IsSuccess.Should().BeTrue();


            var attackResponse = board.IncomingAttack(3, 3);
            attackResponse.Should().Be(TurnResponse.Hit);
            board.IsGameLost().Should().BeFalse();

            attackResponse = board.IncomingAttack(3, 1);
            attackResponse.Should().Be(TurnResponse.Hit);
            board.IsGameLost().Should().BeFalse();

            attackResponse = board.IncomingAttack(3, 2);
            attackResponse.Should().Be(TurnResponse.Hit);
            board.IsGameLost().Should().BeFalse();

            attackResponse = board.IncomingAttack(3, 4);
            attackResponse.Should().Be(TurnResponse.Hit);
            board.IsGameLost().Should().BeFalse();

            attackResponse = board.IncomingAttack(8, 8);
            attackResponse.Should().Be(TurnResponse.Hit);
            board.IsGameLost().Should().BeTrue();
        }

        [Fact]
        public void AttackingTheSamePositionTwiceResultsInException()
        {
            var shipSize = 4;
            var shipOrientation = Orientation.Vertical;
            var shipXCoordinate = 3;
            var shipYCoordinate = 1;


            var board = TestHelper.CreateBoard();
            var addShipResult = board.AddShip(shipSize, shipOrientation, shipXCoordinate, shipYCoordinate);

            addShipResult.IsSuccess.Should().BeTrue();

            var attackResponse = board.IncomingAttack(3, 3);
            attackResponse.Should().Be(TurnResponse.Hit);

            Action attackSamePosition = () => board.IncomingAttack(3, 3);

            attackSamePosition.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void WhenGameIsAlreadyLost_ExceptionIsThrownOnIncomingAttack()
        {
            const string errorMessage = "Game has already been lost";
            
            var shipSize = 1;
            var shipOrientation = Orientation.Vertical;
            var shipXCoordinate = 3;
            var shipYCoordinate = 1;


            var board = TestHelper.CreateBoard();
            var addShipResult = board.AddShip(shipSize, shipOrientation, shipXCoordinate, shipYCoordinate);

            addShipResult.IsSuccess.Should().BeTrue();

            var attackResponse = board.IncomingAttack(3, 1);
            attackResponse.Should().Be(TurnResponse.Hit);

            Action incomingAttack = () => board.IncomingAttack(3, 3);

            incomingAttack.Should().Throw<ApplicationException>().WithMessage(errorMessage);
        }

        [Fact]
        public void WhenAShipIsSunkSubsequentHitsOnShipResultInException()
        {
            var errorMessage = "Ship is already sunk";
            
            var shipSize = 1;
            var shipOrientation = Orientation.Vertical;
            var shipXCoordinate = 3;
            var shipYCoordinate = 1;


            var board = TestHelper.CreateBoard();
            var addShipResult = board.AddShip(shipSize, shipOrientation, shipXCoordinate, shipYCoordinate);

            board.AddShip(1, Orientation.Vertical, 5, 5);

            addShipResult.IsSuccess.Should().BeTrue();

            var attackResponse = board.IncomingAttack(3, 1);
            attackResponse.Should().Be(TurnResponse.Hit);
            

            Action incomingAttack = () => board.IncomingAttack(3, 1);

            incomingAttack.Should().Throw<InvalidOperationException>().WithMessage(errorMessage);
        }
    }
}
