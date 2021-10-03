using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : MonoBehaviour
{
    // Start is called before the first frame update

    public bool Enabled;

    public ParticleSystem BeamParticles;

    public GameObject TractorBeam;

    public Transform BeamTarget;

    public GameObject Arm;

    public float RotateSpeed = 60.0f;

    public float RopeSpeed = 10.0f;

    public float VerticalSpeed = 10.0f;

    public float TractorBeamTargetSpeed = 10.0f;

    public float TractorBeamMinHeight;
    public float TractorBeamMaxHeight;

    public float CraneMinimumHeight;
    public float CraneMaxHeight;

    public float CraneMinimumRotation;
    public float CraneMaxRotation;

    // Hook Anchor
    public float HookPositionSpeed = 10.0f;
    public float HookMaximumPosition = 1.0f;
    public float HookMinimumPosition = 0.0f;

    public BuildingBlock PickedUp;

    private float PIDThrottle;

    // Force of the tractor beam in newtons
    public float TractorBeamForce = 500.0f;

    public AudioSource BeamSound;

    private Vector3 InitialArmPosition;

    void Start()
    {
        InitialArmPosition = Arm.transform.position;
        Enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(TractorBeam.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000.0f, Physics.AllLayers))
        {
            BeamParticles.transform.position = hit.point;
            BeamParticles.startLifetime = Vector3.Distance(hit.point, BeamTarget.position) / BeamParticles.main.startSpeed.constant;
        }

        if (!Enabled) {
            return;
        }

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
            if (Physics.Raycast(ray, out hit, 1000.0f, BuildingBlock.PhysicsLayer())) {
                BuildingBlock hitBlock = hit.collider.gameObject.GetComponent<BuildingBlock>();
                if (hitBlock && !hitBlock.IsPlaced && Input.GetButtonDown("TractorBeamAttach")) {
                    Debug.Log("Picked up block");
                    PickedUp = hitBlock;
                    BeamSound.Play();
                    PickedUp.GetComponent<Rigidbody>().useGravity = false;
                    PickedUp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                }
            }
        }

        // Crane rotation
        float rotateAmount = -Input.GetAxis("CraneRotation") * RotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotateAmount);

        Vector3 e = transform.rotation.eulerAngles;
        Debug.Log(e);
        //e.y = Mathf.Clamp(e.y, CraneMinimumRotation, CraneMaxRotation);
        if ((e.y < 270) && (e.y > 90)) {
            if ((270 - e.y) > (e.y - 90)) {
                e.y = 90;
            } else {
                e.y = 270;
            }
        }
        transform.rotation = Quaternion.Euler(e);

        // Crane height controls
        float verticalAmount = Input.GetAxis("CraneHeight") * VerticalSpeed * Time.deltaTime;
        Arm.transform.Translate(Vector3.up * verticalAmount);

        Arm.transform.position = new Vector3(Arm.transform.position.x, Mathf.Clamp(Arm.transform.position.y, CraneMinimumHeight, CraneMaxHeight), Arm.transform.position.z);

        // Rope controls
        // float ropeAmount = Input.GetAxis("RopeLength") * RopeSpeed * Time.deltaTime;
        // Hook.transform.Translate(-Vector3.up * ropeAmount);

        // Hook controls
        float hookAnchorAmount = -Input.GetAxis("HookPosition") * HookPositionSpeed * Time.deltaTime;

        Vector3 beamPosition = TractorBeam.transform.localPosition;
        beamPosition.z = Mathf.Clamp(beamPosition.z - hookAnchorAmount, HookMinimumPosition, HookMaximumPosition);

        TractorBeam.transform.localPosition = beamPosition;

        float targetInput = (Input.GetAxis("TractorBeamHeight") + Input.GetAxis("Mouse ScrollWheel") * 100.0f);

        float targetAmount = targetInput * TractorBeamTargetSpeed * Time.deltaTime;
        BeamTarget.Translate(Vector3.forward * targetAmount);

        BeamTarget.position = new Vector3(BeamTarget.position.x, Mathf.Clamp(BeamTarget.position.y, 10, Arm.transform.position.y - 6), BeamTarget.position.z);
    }

    void MoveBlockInsideBeam()
    {

        Rigidbody body = PickedUp.GetComponent<Rigidbody>();
        Vector3 v = body.velocity;

        Vector3 desiredPosition = BeamTarget.position;
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

    bool IsInPlacementZone()
    {
        return (transform.rotation.y > 0.0f);
    }

    public void Reset()
    {
        PickedUp = null;
        transform.rotation = Quaternion.identity;
        Arm.transform.position = InitialArmPosition;
    }
}
