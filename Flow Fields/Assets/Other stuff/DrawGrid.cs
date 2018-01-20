using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DrawGrid : MonoBehaviour
{
    public bool drawGird;
    public Material debugMat;

    int w, l;
    private Grid2D grid;
    private Vector2[] goals;
    public List<Text> texts;
    public List<Transform> arrows;
    public GameObject txPrefab, arrPrefab;
    public GameObject canvas, arrowParent;


    public void InitCam(int w, int l, Grid2D grid, Vector2[] goals)
    {
        this.w = w;
        this.l = l;
        this.grid = grid;
        this.goals = goals;

        texts = Grid2D.SetTexts(w, l, txPrefab, canvas);
        arrows = Grid2D.SetArrows(w, l, arrPrefab, canvas);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Grid2D.UpdateTexts(w, l, texts, grid, goals);
            Grid2D.UpdateArrows(w, l, arrows, grid);
        }
    }

    private void OnPostRender()
    {
        if (drawGird)
            Grid2D.DrawGrid(w, l, grid, debugMat);
    }
}
