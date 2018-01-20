using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm
{
    private Grid2D grid;

    private Vector2[] goals;


    private Algorithm () { }

    public Algorithm (Grid2D grid, Vector2[] goals)
    {
        this.grid = grid;
        this.goals = goals;
    }


    /// <summary>
    /// Wavefront algorithm to create a distance field.
    /// </summary>
    public void GenerateDistanceField()
    {
        var marked = new List<Cell>();
        var cells = grid.cells;

        for (int i = 0; i < goals.Length; i++)
        {
            var goalCell = cells.First(c => c.position == goals[i]);
            goalCell.distance = 0;

            marked.Add(goalCell);
        }

        if (goals == null || goals.Length < 1)
        {
            Debug.LogError("No goals set !");
            return;
        }


        while (marked.Count < cells.Length)
        {
            for (int i = 0; i < marked.Count; i++)
            {
                if (marked[i].unpassable)
                    continue;

                var neighbours = grid.GetMooreNeighbours(marked[i]);
                for (int j = 0; j < 8; j++)
                {
                    var cur = neighbours[j];
                    if (cur == null || marked.Contains(cur))
                        continue;

                    cur.distance = marked[i].distance;
                    cur.distance += (cur.position - marked[i].position).magnitude;

                    marked.Add(cur);
                }
            }
        }
    }


    public void GenerateVectorFields()
    {
        for (int i = 0; i < grid.cells.Length; i++)
        {
            var cur = grid.cells[i];
            var neighbours = grid.GetNeumannNeighbours(cur);

            float left, right, up, down;
            left = right = up = down = cur.distance;

            if (neighbours[0] != null && !neighbours[0].unpassable) up = neighbours[0].distance;
            if (neighbours[1] != null && !neighbours[1].unpassable) right = neighbours[1].distance;
            if (neighbours[2] != null && !neighbours[2].unpassable) down = neighbours[2].distance;
            if (neighbours[3] != null && !neighbours[3].unpassable) left = neighbours[3].distance;


            float x = left - right;
            float y = down - up;

            cur.direction = new Vector2(x, y);
            cur.direction.Normalize();
        }
    }
}
