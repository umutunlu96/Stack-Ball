using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPartController : MonoBehaviour
{
    private Rigidbody rigidBody;
    private MeshRenderer meshRenderer;
    private Collider coll;
    private StackController stackController;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        coll = GetComponent<Collider>();
        stackController = transform.parent.GetComponent<StackController>();
    }

    public void Shatter()
    {
        rigidBody.isKinematic = false;  //to be effected by gravity
        coll.enabled = false;   //doesnt collide with others

        Vector3 forcePoint = transform.parent.position; //  pivot point of parent
        float parentXpos = transform.parent.position.x;
        float xPos = meshRenderer.bounds.center.x;  // mid point of mesh renderer

        /*
         * if(parentXpos - xPos <0){
         * subDir = Vector3.right;
         * }
         * else{
         * subDir = Vector3.left;
         * }
         */
        
        Vector3 subDir = (parentXpos - xPos < 0) ? Vector3.right : Vector3.left;
        Vector3 direction = (Vector3.up * 1.5f + subDir).normalized;

        float force = Random.Range(20, 35);
        float torque = Random.Range(110, 180);

        rigidBody.AddForceAtPosition(direction * force, forcePoint, ForceMode.Impulse);
        rigidBody.AddTorque(Vector3.left * torque);
        rigidBody.velocity = Vector3.down;
    }

    public void RemoveAllChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetParent(null);
            i--;
        }
    }
}
