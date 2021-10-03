using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightTracker : MonoBehaviour
{

    public GameObject Visual;

    private float TimeSinceLastMove;

    // Start is called before the first frame update
    void Start()
    {
        TimeSinceLastMove = float.PositiveInfinity;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeSinceLastMove < 0.2f) {
            TimeSinceLastMove += Time.deltaTime;
            Visual.SetActive(true);
        } else {
            Visual.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        BuildingBlock block = other.GetComponent<BuildingBlock>();
        if (block && block.IsPlaced && block.IsStable()) {
            transform.Translate(0, 0.1f, 0);
            TimeSinceLastMove = 0.0f;
        }
    }

    public bool IsMoving()
    {
        return TimeSinceLastMove < 0.2f;
    }    
}
