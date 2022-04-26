using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackgroundLerp : MonoBehaviour
{
    public Color color1;
    public Color color2;

    private Camera cam;
    private Player player;

    public float rate;

    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        //cam.clearFlags = CameraClearFlags.SolidColor;
        player = FindObjectOfType<Player>();

    }

    void Update()
    {
        rate = (float)player.currentBrokenStacks / (float)player.totalStacks;
        cam.backgroundColor = Color.Lerp(color1, color2, rate);
    }
}
