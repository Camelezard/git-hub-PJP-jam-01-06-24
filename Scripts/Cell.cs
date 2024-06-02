using Godot;
using System;
using System.Collections.Generic;

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
        public enum CellType { Void, Empty, House, IronSpot, FoodSpot, BlackHole }
        public CellType cellType = CellType.Empty;
        public Coordinates gridCoordinates;

        public Sprite CellSprite;

        [Export] private NodePath CellSpritePath;
        [Export] private NodePath LabelPath;

        [Export] private List<Texture> CellTexturList = new List<Texture>();

        [Export] private PackedScene ConstructorBoxFactory;

        [Export] PackedScene settlersFactory;

        [Export] PackedScene HomeAreaFactory;

        public int settlersin;
        public Label lLabel;

        public override void _Ready()
        {
            CellSprite = GetNode<Sprite>(CellSpritePath);
            lLabel = GetNode<Label>(LabelPath);
            CellSprite.Texture = CellTexturList[(int)cellType];
        }

        public void AdoptTheCellTexture()
        {
            CellSprite.Texture = CellTexturList[(int)cellType];

            if (cellType == CellType.House)
            {
                CellSprite.Offset = new Vector2(0, -15);
                lLabel.Text = settlersin.ToString();
                lLabel.Show();
            }
            else if (cellType == CellType.IronSpot) CellSprite.Offset = new Vector2(0, -1);
            else CellSprite.Offset = Vector2.Zero;
        }

        public void creatConstructorBox()
        {
            Control lConstructorBox = (Control)ConstructorBoxFactory.Instance();
            AddChild(lConstructorBox);
        }

        public void CreateTile(CellType pcellType = CellType.Empty)
        {
            cellType = pcellType;
            
            AdoptTheCellTexture();
        }

        public void SpawnSettler()
        {
            if (settlersin > 0)
            {
                Node2D lsettler = (Node2D)settlersFactory.Instance();
                AddChild(lsettler);

                settlersin--;
                ShowTheNumberOfSettlerIn();
            }
        }

        public void ShowTheNumberOfSettlerIn()
        {
            lLabel.Show();
            lLabel.Text = settlersin.ToString();
        }

        public void SpawnAHomeArea()
        {
            Node2D lsettler = (Node2D)settlersFactory.Instance();
            AddChild(lsettler);
        }
    }
}