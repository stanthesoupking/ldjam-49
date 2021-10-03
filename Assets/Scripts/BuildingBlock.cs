using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock : MonoBehaviour
{

    public bool IsPlaced = false;

    private GameController gameController;

    public static int PhysicsLayer() {
        return LayerMask.GetMask("Block");
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -100)
        {
            transform.position = gameController.SpawnLocation.transform.position;
            Rigidbody body = GetComponent<Rigidbody>();
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }
    }

    public bool IsStable()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        float stableMinVelocity = 0.02f;

        return (body.velocity.magnitude < stableMinVelocity) && (body.angularVelocity.magnitude < stableMinVelocity);
    }
}
