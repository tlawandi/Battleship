using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Common;

namespace Battleship
{
    public class Position  : ValueObject<Position>
    {
        private const string InvalidPositionCoordinates = "Coordinate do not form a straight line";
        public Coordinate StartCoordinate { get; }
        public Coordinate EndCoordinate { get; }

        public IList<Coordinate> OccupiedCoordinates { get; }

        public Position(Coordinate startCoordinate, Coordinate endCoordinate)
        {
            StartCoordinate = startCoordinate;
            EndCoordinate = endCoordinate;

            OccupiedCoordinates = PopulateOccupiedCoordinates(startCoordinate, endCoordinate);
        }

        private IList<Coordinate> PopulateOccupiedCoordinates(Coordinate startCoordinate, Coordinate endCoordinate)
        {
            var occupiedCoordinates = new List<Coordinate>();

            var coordinateToAdd = startCoordinate;
            if (startCoordinate.XCoordinate == endCoordinate.XCoordinate)
            {
                //Vertical orientation
                while (coordinateToAdd.YCoordinate != endCoordinate.YCoordinate)
                {
                    occupiedCoordinates.Add(coordinateToAdd);
                    coordinateToAdd = new Coordinate(coordinateToAdd.XCoordinate, coordinateToAdd.YCoordinate + 1);
                }
            }
            else if (startCoordinate.YCoordinate == endCoordinate.YCoordinate)
            {
                //Even though there is some duplicate code Hear that Violates DRY. I felt that it was more readable then using an alternative solution.
                //Horizontal orientation
                while (coordinateToAdd.XCoordinate != endCoordinate.XCoordinate)
                {
                    occupiedCoordinates.Add(coordinateToAdd);
                    coordinateToAdd = new Coordinate(coordinateToAdd.XCoordinate + 1, coordinateToAdd.YCoordinate);
                }
            }
            else
            {
                throw new ArgumentException(InvalidPositionCoordinates);
            }

            return occupiedCoordinates;
        }

        //Extracted from https://stackoverflow.com/questions/4543506/algorithm-for-intersection-of-2-lines
        //This solution failed in the following test Add2BattleshipToTheBoardThatOverlapVerticallyFails. This is why i love tests.
        //above is an inefficient solution but solves the requirement. I would not normally leave in commented code. But just wanted to show progress.

        //public bool DoesPositionIntersect(Position position)
        //{
        //    //Line1
        //    float A1 = EndCoordinate.YCoordinate - StartCoordinate.YCoordinate;
        //    float B1 = EndCoordinate.XCoordinate - StartCoordinate.XCoordinate;
        //    float C1 = A1 * StartCoordinate.XCoordinate + B1 * StartCoordinate.YCoordinate;

        //    //Line2
        //    float A2 = position.EndCoordinate.YCoordinate - position.StartCoordinate.YCoordinate;
        //    float B2 = position.EndCoordinate.XCoordinate - position.StartCoordinate.XCoordinate;
        //    float C2 = A2 * position.StartCoordinate.XCoordinate + B2 * position.StartCoordinate.YCoordinate;

        //    float delta = A1 * B2 - A2 * B1;

        //    if (delta == 0)
        //        return false;
        //    else
        //        return true;

        //    //Position of intersection
        //    //float x = (B2 * C1 - B1 * C2) / delta;
        //    //float y = (A1 * C2 - A2 * C1) / delta;
        //}

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartCoordinate;
            yield return EndCoordinate;
        }

        public bool DoesCoordinateIntersect(Coordinate coordinate)
        {
            return this.OccupiedCoordinates.Contains(coordinate);
        }

        public bool DoesPositionIntersect(Position position)
        {
            return this.OccupiedCoordinates.Any(x => position.OccupiedCoordinates.Contains(x));
        }

       
        
    }
}