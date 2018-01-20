using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;
    public float highSpeed;


    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var z = 0;//Input.GetAxis("Z");

        var speed = (Input.GetKey(KeyCode.LeftShift)) ? highSpeed : moveSpeed;
        var movement = new Vector3(horizontal * speed, z * speed, vertical * speed) * Time.deltaTime;
        transform.Translate(movement);
    }
}
