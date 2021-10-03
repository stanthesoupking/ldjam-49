using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock : MonoBehaviour
{

    public bool IsPlaced = false;

    public static int PhysicsLayer() {
        return LayerMask.GetMask("Block");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsStable()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        float stableMinVelocity = 0.02f;

        return (body.velocity.magnitude < stableMinVelocity) && (body.angularVelocity.magnitude < stableMinVelocity);
    }
}
