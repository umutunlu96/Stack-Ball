using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 100;

    void Update()
    {
        Rotate(speed);
    }

    public void Rotate(float speed)
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
