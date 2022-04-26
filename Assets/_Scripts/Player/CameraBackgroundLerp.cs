using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackgroundLerp : MonoBehaviour
{
    public Color colorUp;
    public Color colorDown;

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
        cam.backgroundColor = Color.Lerp(colorUp, colorDown, rate);
    }

    public void SetColors(Color colorUp)
    {
        this.colorUp = colorUp;
    }
}
