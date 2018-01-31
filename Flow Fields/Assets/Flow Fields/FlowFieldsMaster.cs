/// <summary>
/// Sets up the environment and runs the algorithm. 
/// Sets actor's movement.
/// </summary>

using UnityEngine;

public class FlowFieldsMaster : MonoBehaviour
{
    public int width = 20, length = 20;

    private Grid2D grid;
    private Group group;
    private Algorithm algorithm;
    public GameObject actorPrefab;

    public Vector2[] goals = new Vector2[]
    {
        new Vector2(2, 3),
        new Vector2(3, 4),
        new Vector2(4, 5),
        new Vector2(5, 6)
    };

    public DrawGrid debug;


    private void Awake()
    {
        grid = new Grid2D(width, length);
        algorithm = new Algorithm(grid, goals);
    }


    private void Start()
    {
        grid.Generate();
        SetUnpassables();

        algorithm.GenerateDistanceField();
        algorithm.GenerateVectorFields();

        // Spawns just a single actor for testing.
        group = new Group(1, actorPrefab);
        group.SetStartPositions();

        #if UNITY_EDITOR
        debug.InitCam(width, length, grid, goals);
        #endif
    }


    private void Update()
    {
        AssignVelocities();
        group.MoveActors();
    }


    private void AssignVelocities()
    {
        for (int i = 0; i < group.size; i++)
        {
            var actor = group.actors[i];
            var pos = actor.position;

            var cell = grid.FindCellByPosition(actor.position);
            actor.direction = (cell != null) ? cell.direction : Vector2.zero;
        }
    }



    /// <summary>
    /// A little hack to mark some cells as unpassable.
    /// </summary>
    private void SetUnpassables()
    {
        for (int x = 4; x < 9; x++)
        {
            int index = x * length + 10;
            grid.cells[index].unpassable = true;
        }

        for (int y = 4; y < 10; y++)
        {
            int index = 8 * length + y;
            grid.cells[index].unpassable = true;
        }
    }
}
