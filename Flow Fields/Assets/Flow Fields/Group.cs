/// <summary>
/// Can be used to combine multiple actors with the same goal.
/// </summary>

using UnityEngine;

[System.Serializable]
public class Group
{
    public int size;
    public Actor[] actors;

    private GameObject prefab;


    private Group () { }

    public Group (int size, GameObject prefab)
    {
        this.size = size;
        this.prefab = prefab;

        GeneratePopulation();
    }


    public void GeneratePopulation ()
    {
        actors = new Actor[size];
        for (int i = 0; i < size; i++)
        {
            var go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            actors[i] = go.GetComponent<Actor>();
        }
    }


    public void MoveActors ()
    {
        foreach (var act in actors)
            act.Move();
    }



    // A little hack to set a predefined position for our single actor.
    public void SetStartPositions()
    {
        actors[0].position = new Vector2(16, 16);
    }
}
