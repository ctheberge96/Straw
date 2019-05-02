using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class ReportCreator : MonoBehaviour
{
    public GameObject strawling;
    public GameObject destination;
    // Start is called before the first frame update

    private GameObject[] MakeStrawling(int number) {
        for (int i = 0; i < number; i++) {
            GameObject.Instantiate(strawling, new Vector3(1,-1,0), Quaternion.identity);
        }
        return GameObject.FindGameObjectsWithTag("straw");
    }
}
