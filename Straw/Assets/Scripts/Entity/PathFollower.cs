using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{

    public float moveSpeed;
    public int maxChecks;
    private Path myPath;
    private int curNode = 0;
    private bool stopping = false;

    // Update is called once per frame
    void Update()
    {

        if (myPath != null & !stopping) {

            if (transform.position.x == myPath[curNode].x && transform.position.y == myPath[curNode].y && transform.position.z == myPath[curNode].z) {

                curNode++;

                if (!ValidateNextCell()) {
                    Stop();
                }

                if (curNode == myPath.length) { ResetPath(); }

            } else {

                MoveToCurNode();

            }

        } else if (stopping) {

            if (transform.position.x == myPath[curNode].x && transform.position.y == myPath[curNode].y && transform.position.z == myPath[curNode].z) {

                ResetPath();

            } else {

                MoveToCurNode();

            }

        }

    }

    ///<summary>
    ///Paths to a given position moving around obstacles on the grid.
    ///</summary>
    ///<returns>
    ///Returns whether a path was able to be found.
    ///</returns>
    public bool PathTo(Vector3 point) {

        myPath = Pathfinder.getPath(transform.position, point, maxChecks);
        curNode = 0;

        return myPath == null;

    }

    ///<summary>
    ///Stops the pathing agent, letting it move to be aligned with the grid.
    ///</summary>
    public void Stop() {
        stopping = true;
    }

    ///<summary>
    ///Stops the pathing agent exactly where it is now.
    ///</summary>
    public void ImmediateStop() {
        ResetPath();
    }

    private void ResetPath() {
        myPath = null;
        stopping = false;
        curNode = 0;
    }

    private void MoveToCurNode() {
        transform.position = Vector3.MoveTowards(transform.position, myPath[curNode], moveSpeed * Time.deltaTime);
    }

    private bool ValidateNextCell() {
        return Colliders.isFree(myPath[curNode],
                                Camera.main.GetComponent<Grid>().cellSize,
                                LayerMask.GetMask("solid"));
    }

}
