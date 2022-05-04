using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 100;
    private Player player;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    void Update()
    {
        if (player.playerState != Player.PlayerState.Died)
            Rotate(speed);
    }

    public void Rotate(float speed)
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
