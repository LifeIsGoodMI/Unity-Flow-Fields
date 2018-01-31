/// <summary>
/// Represents the underlying 2D grid for the implementation. Easily extendable to a 3D grid.
/// </summary>

using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Note:
///     -> Algorithm is currently only for a single group, i.e. all actors will have the same goal !
/// </summary>

[System.Serializable]
public class Grid2D
{
    private int width, length;
    public Cell[] cells;


    private Grid2D () { }

    public Grid2D (int width, int length)
    {
        this.width = width;
        this.length = length;
    }

    public void Generate ()
    {
        cells = new Cell[width * length];

        for (int w = 0; w < width; w++)
        {
            for (int l = 0; l < length; l++)
            {
                int index = w * length + l;
                var cell = new Cell(new Vector2(w, l));

                cells[index] = cell;
            }
        }
    }


    public Cell[] GetNeumannNeighbours(Cell current)
    {
        var result = new Cell[4];

        int x = (int)current.position.x;
        int y = (int)current.position.y;

        var indices = new int[]
        {
            x * length + (y + 1),
            (x + 1) * length + y,
            x * length + (y - 1),
            (x - 1) * length + y
        };


        // North
        if (y < length - 1)
            result[0] = cells[indices[0]];

        // East
        if (x < width - 1)
            result[1] = cells[indices[1]];

        // South
        if (y > 0)
            result[2] = cells[indices[2]];

        // West
        if (x > 0)
            result[3] = cells[indices[3]];

        return result;
    }

    public Cell[] GetMooreNeighbours (Cell current)
    {
        var result = new Cell[8];

        int x = (int) current.position.x;
        int y = (int) current.position.y;

        var indices = new int[]
        {
            x * length + (y + 1),
            (x + 1) * length + y,
            x * length + (y - 1),
            (x - 1) * length + y,
            (x - 1) * length + (y + 1),
            (x + 1) * length + (y + 1),
            (x - 1) * length + (y - 1),
            (x + 1) * length + (y - 1)
        };


        // North
        if (y < length - 1)
            result[0] = cells[indices[0]];

        // East
        if (x < width - 1)
            result[1] = cells[indices[1]];

        // South
        if (y > 0)
            result[2] = cells[indices[2]];

        // West
        if (x > 0)
            result[3] = cells[indices[3]];


        // North-West
        if (y < length - 1 && x > 0)
            result[4] = cells[indices[4]];

        // North-East
        if (y < length - 1 && x < width - 1)
            result[5] = cells[indices[5]];

        // South-West
        if (y > 0 && x > 0)
            result[6] = cells[indices[6]];

        // South-East
        if (y > 0 && x < width - 1)
            result[7] = cells[indices[7]];

        return result;
    }

    public Cell FindCellByPosition(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;

        if (x >= 0 && x < width && y >= 0 && y < length)
        {
            int index = x * length + y;
            var cell = cells[index];
            return cell;
        }

        return null;
    }
    

    #region Debug
    public static void DrawGrid(int width, int length, Grid2D grid, Material mat)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                int index = i * length + j;
                var cell = grid.cells[index];

                GL.Color(Color.cyan);

                var ll = new Vector3(cell.position.x - 0.5f, 0, cell.position.y - 0.5f);
                var ul = new Vector3(cell.position.x - 0.5f, 0, cell.position.y + 0.5f);

                var lr = new Vector3(cell.position.x + 0.5f, 0, cell.position.y - 0.5f);
                var ur = new Vector3(cell.position.x + 0.5f, 0, cell.position.y + 0.5f);

                DrawEdge(mat, ll, ul);
                DrawEdge(mat, ul, ur);
                DrawEdge(mat, ur, lr);
                DrawEdge(mat, lr, ll);
            }
        }
    }

    public static void DrawEdge(Material material, Vector3 start, Vector3 end)
    {
        GL.Begin(GL.LINES);
        //GL.PushMatrix();
        material.SetPass(0);
        //GL.LoadOrtho();
        GL.Color(Color.cyan);
        GL.Vertex(start);
        GL.Vertex(end);
        GL.Color(Color.cyan);

        GL.End();
        //GL.PopMatrix();
    }

    public static List<UnityEngine.UI.Text> SetTexts (int width, int length, GameObject prefab, GameObject parent)
    {
        var texts = new List<UnityEngine.UI.Text>();

        for (int w = 0; w < width; w++)
        {
            for (int l = 0; l < length; l++)
            {
                var go = GameObject.Instantiate(prefab, parent.transform);
                var t = go.GetComponentInChildren<UnityEngine.UI.Text>();
                t.rectTransform.localPosition = new Vector3(w - 10, l - 10, 0);

                texts.Add(t);
            }
        }

        return texts;
    }

    public static List<Transform> SetArrows(int width, int length, GameObject prefab, GameObject parent)
    {
        var arrows = new List<Transform>();

        for (int w = 0; w < width; w++)
        {
            for (int l = 0; l < length; l++)
            {
                var arr = GameObject.Instantiate(prefab).transform;
                arr.localPosition = new Vector3(w, 0, l);

                arrows.Add(arr);
            }
        }

        return arrows;
    }


    public static void UpdateTexts (int width, int length, List<UnityEngine.UI.Text> texts, Grid2D grid, Vector2[] goals)
    {
        for (int i = 0; i < width * length; i++)
        {
            texts[i].text = grid.cells[i].distance.ToString();

            if(grid.cells[i].unpassable)
            {
                texts[i].text = "N/A";
                texts[i].color = Color.red;
            }

            if (goals.Contains(grid.cells[i].position))
                texts[i].color = Color.green;
        }
    }

    public static void UpdateArrows(int width, int length, List<Transform> arrows, Grid2D grid)
    {
        for (int i = 0; i < width * length; i++)
        {
            var cur = grid.cells[i];
            var direction = new Vector3(cur.direction.x, 0, cur.direction.y);

            if (direction != Vector3.zero)
                arrows[i].rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
    #endregion
}
