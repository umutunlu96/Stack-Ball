using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(bool shake, float magnitude)
    {

        Quaternion localRot = transform.rotation;


        if (shake)
        {
            float rotate = Random.Range(-1f, 1f) * magnitude;

            transform.localRotation =  new Quaternion(localRot.x, rotate, localRot.z,localRot.w);

            shake = false;
            
            yield return null;

        }

        transform.localRotation = localRot;
    }
}
