using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alt_Tile
{
    public Alt_Bullet Content = null;
    public Alt_Bullet ContentNextTurn = null;
    public string name;

    [field: SerializeField] private TileType myType;

    [field:SerializeField]private Alt_Tile nextStaticTile;
    public Alt_TileRepresentation myRepresentation;
    private int _x, _y; // arrayPosition
    static private Alt_Functions.MaipulationFunction _manipulation;
    static private Alt_Functions.MovementFunction _movement;

    public bool moveSecondBool = false;




    private void Awake()
    {

    }

    public void useManipulationFunction()
    {
        _manipulation = Alt_Functions.GetManipulation(myType);
        _manipulation(this);
    }
    public void useMovementFunction()
    {
        _movement = Alt_Functions.GetMovement();
        _movement(this);
    }















    public Alt_Tile GetNextTile()
    {
        return this.nextStaticTile;
    }
    public void SetNextTile(Alt_Tile yourChild)
    {
        this.nextStaticTile = yourChild;
    }
    public void setCoordinates(int x, int y)
    {
        this._x = x;
        this._y = y;
    }
    public Alt_TileRepresentation GetRepresentation()
    {
        return myRepresentation;
    }
    public void SetRepresentation(Alt_TileRepresentation representation)
    {
        this.myRepresentation = representation;
    }

    public void SetTileType(TileType newType)
    {
        myType = newType;
    }
    public TileType GetTileType()
    {
        return myType;
    }

    public int getXValue()
    {
        return _x;
    }
    public int getYValue()
    {
        return _y;
    }
}
