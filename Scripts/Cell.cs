using Godot;
using System;

// Author : Sophia Solignac
namespace Com.IsartDigital.ProjectName.Game
{
    public struct Coordinates
    {
        public int x, y;
        public Coordinates(int pX, int pY)
        {
            x = pX;
            y = pY;
        }
        public Coordinates(Vector2 pVector)
        {
            x = (int)pVector.x;
            y = (int)pVector.y;
        }
        public Coordinates(Coordinates pCoordinates)
        {
            x = pCoordinates.x;
            y = pCoordinates.y;
        }

        public override string ToString() => $"({x};{y})";

        public override bool Equals(object obj)
        {
            if (obj is Coordinates)
            {
                return this == (Coordinates)obj;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return (x.GetHashCode() + y.GetHashCode()) * x * y.GetHashCode();
        }
        public Vector2 ToVector() => new Vector2(x, y);

        public static Coordinates operator +(Coordinates pCoordinatesA, Coordinates pCoordinatesB)
            => new Coordinates(pCoordinatesA.x + pCoordinatesB.x, pCoordinatesA.y + pCoordinatesB.y);

        public static Coordinates operator +(Coordinates pCoordinatesA, Vector2 pCoordinatesB)
           => new Coordinates(pCoordinatesA.x + (int)pCoordinatesB.x, pCoordinatesA.y + (int)pCoordinatesB.y);
        public static Coordinates operator -(Coordinates pCoordinates) => new Coordinates(-pCoordinates.x, -pCoordinates.y);

        public static Coordinates operator -(Coordinates pCoordinatesA, Coordinates pCoordinatesB)
            => pCoordinatesA + -pCoordinatesB;
        public static Coordinates operator -(Coordinates pCoordinatesA, Vector2 pCoordinatesB)
           => new Coordinates(pCoordinatesA.x - (int)pCoordinatesB.x, pCoordinatesA.y - (int)pCoordinatesB.y);
        public static bool operator ==(Coordinates pCoordinatesA, Coordinates pCoordinatesB)
            => (pCoordinatesA.x == pCoordinatesB.x && pCoordinatesA.y == pCoordinatesB.y);
        public static bool operator !=(Coordinates pCoordinatesA, Coordinates pCoordinatesB)
            => (pCoordinatesA.x != pCoordinatesB.x || pCoordinatesA.y != pCoordinatesB.y);

    }
    public class Cell : Node2D
    {
        public enum CellType { Empty, Void, House, IronSpot, FoodSpot}
        public CellType cellType = CellType.Empty;
        public Coordinates gridCoordinates;

        public override void _Ready()
        {
            
        }

    }
}