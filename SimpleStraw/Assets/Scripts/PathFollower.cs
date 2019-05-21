using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public enum Status {
        WAITING,
        FOLLOWING,
        BLOCKED,
        DONE,
        STOPPING,
        STOPPED
    }

    public float moveSpeed;
    public int maxChecks;

    private Status _status = Status.WAITING;
    public Status status {
        get {
            return _status;
        }
    }

    private Pathfinding.Path _myPath;
    private int _curPoint;

    private bool _stopping;

    // Update is called once per frame
    void Update()
    {

        if (_myPath != null) {

            if (_curPoint == _myPath.length) {

                _status = _stopping ? Status.STOPPED:Status.DONE;
                _ResetPath();

            } else {

                transform.position = Vector3.MoveTowards(transform.position,
                                                        _myPath[_curPoint],
                                                        moveSpeed * Time.deltaTime);
                if (transform.position.x == _myPath[_curPoint].x && transform.position.y == _myPath[_curPoint].y) {

                    _curPoint++;

                }

            }

        }

    }

    public void Stop() {
        if (_myPath != null && !_stopping) {
            _status = Status.STOPPING;
            _stopping = true;
            List<Vector2> points = new List<Vector2>();
            points.Add(_myPath[_curPoint]);
            _myPath = new Pathfinding.Path(points);
        }
    }

    public void PathTo(Vector2 position) {

        _ResetPath();

        _myPath = Pathfinding.PathFinder.GetPath(transform.position, position, maxChecks);

        _status = _myPath == null ? Status.BLOCKED:Status.FOLLOWING;

    }

    private void _ResetPath() {
        _myPath = null;
        _curPoint = 0; 
        _stopping = false;
    }

}
