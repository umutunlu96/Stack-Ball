using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 camFollow;
    public Transform player, win;

    void Awake()
    {
        player = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        if (win == null)
            win = GameObject.Find("win(Clone)").GetComponent<Transform>();
        if (transform.position.y > player.transform.position.y && transform.position.y > win.position.y + 4.5f)
            camFollow = new Vector3(transform.position.x,player.transform.position.y,transform.position.z);
        transform.position = new Vector3(transform.position.x,camFollow.y,-5);
    }

}
