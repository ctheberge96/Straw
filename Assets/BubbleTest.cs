using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")) {
            BubbleMaker.MakeBubble(gameObject, 5, "This is a test of the speech bubble system! This text needs to be long enough for me to see how vertical growth may work. This is a test of the speech bubble system! This text needs to be long enough for me to see how vertical growth may work.");
        }
    }
}
