using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSpawn : MonoBehaviour
{

    public int Length = 100;

    public float PartHeight = 1.0f;

    public GameObject prefab;

    public GameObject Hook;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        GameObject previousSegment = gameObject;

        for(int i = 0; i < Length; ++i)
        {
            Vector3 offset0 = new Vector3(0, -i * PartHeight, 0);
            GameObject part = Instantiate(prefab, transform.position + offset0, Quaternion.identity, null);

            part.GetComponent<CharacterJoint>().connectedBody = previousSegment.GetComponent<Rigidbody>();
            previousSegment = part;
        }

        Vector3 offset = new Vector3(0, -Length * PartHeight, 0);
        GameObject hook = Instantiate(Hook, transform.position + offset, Quaternion.identity, null);
        hook.GetComponent<FixedJoint>().connectedBody = previousSegment.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
