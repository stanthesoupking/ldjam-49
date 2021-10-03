using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float HeightOffset;

    public CraneController Crane;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float currentHeight = Crane.Arm.transform.position.y;
        
        transform.position = new Vector3(Crane.TractorBeam.transform.position.x, HeightOffset + currentHeight, transform.position.z);
    }
}
