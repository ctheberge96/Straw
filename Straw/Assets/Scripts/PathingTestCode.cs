using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class PathingTestCode : MonoBehaviour
{
    public float speed;
    public GameObject[] destinations;
    private int curDest = -1;
    private Path myPath;
    private int curNode;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("path_debug");
            foreach(GameObject point in objs) {
                GameObject.Destroy(point);
            }
            curDest++;
            if (curDest >= destinations.Length) { curDest = 0; }
            PathTo(destinations[curDest].transform.position);
        }

        if (myPath != null) {

            if (transform.position.x == myPath[curNode].x && transform.position.y == myPath[curNode].y && transform.position.z == myPath[curNode].z) {

                curNode++;

                if (curNode == myPath.length) { curNode = 0; myPath = null; }

            } else {

                float step =  speed * Time.deltaTime; // calculate distance to move

                //sprite
                float moveX = Mathf.MoveTowards(transform.position.x, myPath[curNode].x, step) - transform.position.x;
                float moveY= Mathf.MoveTowards(transform.position.y, myPath[curNode].y, step) - transform.position.y;
                animator.SetFloat("FaceX",moveX == 0 ? 0:Mathf.Sign(moveX));
                animator.SetFloat("FaceY",moveY == 0 ? 0:Mathf.Sign(moveY));

                //actual movement
                transform.position = Vector3.MoveTowards(transform.position, myPath[curNode], step);;

            }

        }
    }

    public void PathTo(Vector3 point) {
        myPath = Pathfinder.getPath(transform.position, point, 100);
        curNode = 0;
    }

}
