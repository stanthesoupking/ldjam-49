using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject HookAnchor;

    public GameObject Arm;

    public GameObject Hook;

    public GameObject RopeRoot;

    public float RotateSpeed = 60.0f;

    public float RopeSpeed = 10.0f;

    public float VerticalSpeed = 10.0f;

    // Hook Anchor
    public float HookPositionSpeed = 10.0f;
    public float HookMaximumPosition = 1.0f;
    public float HookMinimumPosition = 0.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Crane rotation
        float rotateAmount = Input.GetAxis("CraneRotation") * RotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotateAmount);

        // Crane height controls
        float verticalAmount = Input.GetAxis("CraneHeight") * VerticalSpeed * Time.deltaTime;
        Arm.transform.Translate(Vector3.up * verticalAmount);

        // Rope controls
        float ropeAmount = Input.GetAxis("RopeLength") * RopeSpeed * Time.deltaTime;
        Hook.transform.Translate(-Vector3.up * ropeAmount);

        // Hook controls
        float hookAnchorAmount = Input.GetAxis("HookPosition") * HookPositionSpeed * Time.deltaTime;
        HookAnchor.transform.Translate(Vector3.forward * hookAnchorAmount);

        Vector3 hookPosition = HookAnchor.transform.localPosition;
        hookPosition.z = Mathf.Clamp(hookPosition.z + hookAnchorAmount, HookMinimumPosition, HookMaximumPosition);

        HookAnchor.transform.localPosition = hookPosition;
    }
}
