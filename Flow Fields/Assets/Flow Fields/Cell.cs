/// <summary>
/// A single node of the underlying 2D grid. Easily extendable to a 3D grid.
/// </summary>

using UnityEngine;

[System.Serializable]
public class Cell
{
    public Vector2 position, direction;
    public float distance;

    // I've used a bool as a simple hack to mark cells unpassable. 
    // You might just want to use a cost factor for something like this.
    public bool unpassable;

    private Cell () { }

    public Cell (Vector2 position)
    {
        this.position = position;
    }
}
