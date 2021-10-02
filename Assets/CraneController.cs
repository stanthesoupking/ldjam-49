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

    // Hook Anchor
    public float HookPositionSpeed = 10.0f;
    public float HookMaximumPosition = 1.0f;
    public float HookMinimumPosition = 0.0f;

    public BuildingBlock PickedUp;

    private PID TractorBeamControllerPIDX;
    private PID TractorBeamControllerPIDY;
    private PID TractorBeamControllerPIDZ;

    private float PIDThrottle;

    // Force of the tractor beam in newtons
    public float TractorBeamForce = 500.0f;

    void Start()
    {
        TractorBeamControllerPIDX = new PID(4.0f, 0.5f, 5.0f);
        TractorBeamControllerPIDY = new PID(4.0f, 0.5f, 5.0f);
        TractorBeamControllerPIDZ = new PID(4.0f, 0.5f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Control tractor beam
        if (PickedUp) {
            if (Input.GetButtonDown("TractorBeamAttach")) {
                // Drop the picked up block
                Debug.Log("Dropped block");
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
                if (hitBlock && Input.GetButtonDown("TractorBeamAttach")) {
                    Debug.Log("Picked up block");
                    PickedUp = hitBlock;
                }
            }
        }

        // Crane rotation
        float rotateAmount = Input.GetAxis("CraneRotation") * RotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotateAmount);

        // Crane height controls
        float verticalAmount = Input.GetAxis("CraneHeight") * VerticalSpeed * Time.deltaTime;
        Arm.transform.Translate(Vector3.up * verticalAmount);

        // Rope controls
        // float ropeAmount = Input.GetAxis("RopeLength") * RopeSpeed * Time.deltaTime;
        // Hook.transform.Translate(-Vector3.up * ropeAmount);

        // Hook controls
        float hookAnchorAmount = Input.GetAxis("HookPosition") * HookPositionSpeed * Time.deltaTime;
        TractorBeam.transform.Translate(Vector3.forward * hookAnchorAmount);

        Vector3 beamPosition = TractorBeam.transform.localPosition;
        beamPosition.z = Mathf.Clamp(beamPosition.z + hookAnchorAmount, HookMinimumPosition, HookMaximumPosition);

        TractorBeam.transform.localPosition = beamPosition;
    }

    void AntiIntegralCorrection(PID pid, float speed)
    {
        float integralLimit = 1.0f;
        if (speed < -2.0f) // Descending
        {
            TractorBeamControllerPIDY.LimitIntegral(0);
        }
        TractorBeamControllerPIDY.LimitIntegral(integralLimit); // Ascending
    }

    void MoveBlockInsideBeam()
    {
        Rigidbody body = PickedUp.GetComponent<Rigidbody>();
        
        Vector3 desiredPosition = TractorBeam.transform.position + (Vector3.down * 5.0f);
        Vector3 currentPosition = PickedUp.transform.position;

        Vector3 throttle = new Vector3();
        throttle.x = TractorBeamControllerPIDX.Update(desiredPosition.x, currentPosition.x, Time.deltaTime) * 100.0f;
        throttle.y = TractorBeamControllerPIDY.Update(desiredPosition.y, currentPosition.y, Time.deltaTime) * 100.0f;
        throttle.z = TractorBeamControllerPIDZ.Update(desiredPosition.z, currentPosition.z, Time.deltaTime) * 100.0f;

        Vector3 v = body.velocity;
        AntiIntegralCorrection(TractorBeamControllerPIDX, v.x);
        AntiIntegralCorrection(TractorBeamControllerPIDY, v.y);
        AntiIntegralCorrection(TractorBeamControllerPIDZ, v.z);
    
        Vector3 force = throttle;

        Debug.Log("Applying force: " + force);

        body.AddForce(force);
    }
}
