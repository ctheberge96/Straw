using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    
    public Vector3 destination;
    private Path myPath;
    private int curNode = 0;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) {

            myPath = PathFinding.PathFinder.GetPath(transform.position, destination);

            if (myPath == null) { Debug.Log("NO PATH FOUND FOR " + destination); }

        }

        if (myPath != null) {

            if (transform.position == myPath[curNode]) {

                curNode++;

                if (curNode == myPath.length) {
                
                    curNode = 0;
                    myPath = null;
                
                }

            } else {

               transform.position = Vector3.MoveTowards(transform.position, myPath[curNode], 2 * Time.deltaTime); 

            }

        }

    }

}
