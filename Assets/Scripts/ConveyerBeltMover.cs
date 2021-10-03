using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBeltMover : MonoBehaviour
{

    public float MovementForce = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        BuildingBlock block = other.GetComponent<BuildingBlock>();
        if (block) {
            Rigidbody body = block.GetComponent<Rigidbody>();
            body.AddForce(Vector3.left * MovementForce);
        }
    }
}
