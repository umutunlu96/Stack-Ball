using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    private float targetX, targetY;
    private float orthoSize;

    private void Awake()
    {
        targetX = 1080; targetY = 1920;
    }



    private void Update()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = targetX / targetY;

        if (screenRatio > targetRatio)
            Camera.main.orthographicSize = 4;

        else if (screenRatio == targetRatio)
            Camera.main.orthographicSize = 4.25f;

        else
            Camera.main.orthographicSize = 4.6f;
    }
}