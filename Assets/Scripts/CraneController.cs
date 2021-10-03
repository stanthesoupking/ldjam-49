using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject TractorBeam;

    public GameObject Arm;

    public float RotateSpeed = 60.0f;

    public float RopeSpeed = 10.0f;

    public float VerticalSpeed = 10.0f;

    public float CraneMinimumHeight;

    // Hook Anchor
    public float HookPositionSpeed = 10.0f;
    public float HookMaximumPosition = 1.0f;
    public float HookMinimumPosition = 0.0f;

    public BuildingBlock PickedUp;

    private float PIDThrottle;

    // Force of the tractor beam in newtons
    public float TractorBeamForce = 500.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Control tractor beam
        if (PickedUp) {
            if (Input.GetButtonDown("TractorBeamAttach")) {
                // Drop the picked up block
                Debug.Log("Dropped block");
                PickedUp.IsPlaced = IsInPlacementZone();
                PickedUp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                PickedUp.GetComponent<Rigidbody>().useGravity = true;
                PickedUp = null;
            } else {
                // Move picked up object to be inside beam
                MoveBlockInsideBeam();
            }
        } else {
            Ray ray = new Ray(TractorBeam.transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000.0f, BuildingBlock.PhysicsLayer())) {
                BuildingBlock hitBlock = hit.collider.gameObject.GetComponent<BuildingBlock>();
                if (hitBlock && !hitBlock.IsPlaced && Input.GetButtonDown("TractorBeamAttach")) {
                    Debug.Log("Picked up block");
                    PickedUp = hitBlock;
                    PickedUp.GetComponent<Rigidbody>().useGravity = false;
                    PickedUp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                }
            }
        }

        // Crane rotation
        float rotateAmount = -Input.GetAxis("CraneRotation") * RotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotateAmount);

        // Crane height controls
        float verticalAmount = Input.GetAxis("CraneHeight") * VerticalSpeed * Time.deltaTime;
        Arm.transform.Translate(Vector3.up * verticalAmount);

        if (Arm.transform.position.y < CraneMinimumHeight) {
            Arm.transform.position = new Vector3(Arm.transform.position.x, CraneMinimumHeight, Arm.transform.position.z);
        }

        // Rope controls
        // float ropeAmount = Input.GetAxis("RopeLength") * RopeSpeed * Time.deltaTime;
        // Hook.transform.Translate(-Vector3.up * ropeAmount);

        // Hook controls
        float hookAnchorAmount = Input.GetAxis("HookPosition") * HookPositionSpeed * Time.deltaTime;

        Vector3 beamPosition = TractorBeam.transform.localPosition;
        beamPosition.z = Mathf.Clamp(beamPosition.z - hookAnchorAmount, HookMinimumPosition, HookMaximumPosition);

        TractorBeam.transform.localPosition = beamPosition;
    }

    void MoveBlockInsideBeam()
    {

        Rigidbody body = PickedUp.GetComponent<Rigidbody>();
        Vector3 v = body.velocity;

        Vector3 desiredPosition = TractorBeam.transform.position + (Vector3.down * 20.0f);
        Vector3 currentPosition = PickedUp.transform.position;

        PickedUp.transform.position = new Vector3(desiredPosition.x, currentPosition.y, desiredPosition.z);

        float distance = Vector3.Distance(currentPosition, desiredPosition);

        float desiredVelocityMag = distance * 2.0f;

        float acceleration = (desiredVelocityMag - v.magnitude);

        Vector3 direction = (desiredPosition - currentPosition).normalized;
        Debug.Log(direction);

        float f = acceleration * body.mass;

        Vector3 cancelForce = -v * body.mass;

        body.AddForce(direction * f + cancelForce);
    }

    bool IsInPlacementZone() {
        return (transform.rotation.y > 0.0f);
    }
}
