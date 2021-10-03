using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock : MonoBehaviour
{

    public bool IsPlaced = false;

    private GameController gameController;

    private float TimeSinceLastBang;

    public static int PhysicsLayer() {
        return LayerMask.GetMask("Block");
    }

    // Start is called before the first frame update
    void Start()
    {
        TimeSinceLastBang = float.PositiveInfinity;
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

        TimeSinceLastBang += Time.deltaTime;
    }

    public bool IsStable()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        float stableMinVelocity = 0.02f;

        return (body.velocity.magnitude < stableMinVelocity) && (body.angularVelocity.magnitude < stableMinVelocity);
    }

    public bool IsRoughlyStable()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        float stableMinVelocity = 1.5f;

        return (body.velocity.magnitude < stableMinVelocity) && (body.angularVelocity.magnitude < stableMinVelocity);
    }

    void OnCollisionEnter(Collision c)
    {
        if (TimeSinceLastBang > 4.0f) {
            GetComponent<AudioSource>().Play();
            TimeSinceLastBang = 0.0f;
        }
    }

}
