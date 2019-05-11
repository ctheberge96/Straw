using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public GameObject lookTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(0,0,-Input.mouseScrollDelta.y);
        transform.RotateAround(lookTarget.transform.position, Vector3.up, .5f);
    }
}
