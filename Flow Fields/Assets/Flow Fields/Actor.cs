/// <summary>
/// This class represents a single pathfinder.
/// </summary>

using UnityEngine;

public class Actor : MonoBehaviour
{
    public const float speed = 1.0f;

    public Vector2 position
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.z);
        }

        set
        {
            transform.position = new Vector3(value.x, 0, value.y);
        }
    }

    public Vector2 direction;


    public void Move()
    {
        position += direction * speed * Time.deltaTime;
    }
}
