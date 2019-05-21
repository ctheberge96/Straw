using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Dictates all things the Strawling does. Keeping itself alive, performing tasks, etc.</summary>
[RequireComponent(typeof(Body))]
public class Brain : MonoBehaviour
{

    private Body _body;

    public enum State {

        WAITING,
        WORKING,
        PERSONAL

    }

    private State _state;

    private Tasks.Task _curTask;

    private List<Tasks.Task> _personalTasks = new List<Tasks.Task>();
    private bool _AddPersonalTask(Tasks.Task task) {

        foreach (Tasks.Task queuedTask in _personalTasks) {

            if (queuedTask.GetType() == task.GetType()) { return false; }

        }

        if (task.Valid()) {
            _personalTasks.Add(task);
            return true;
        } else {
            return false;
        }
    }
    private Tasks.Task _GetPersonalTask() {

        if (_personalTasks.Count == 0) { return null; }

        List<Tasks.Task> killList = new List<Tasks.Task>();

        Tasks.Task chosenTask = null;

        for (int i = 0; i < _personalTasks.Count; i++) {

            if (_personalTasks[i].Valid()) {

                chosenTask = _personalTasks[i];
                killList.Add(chosenTask);

                break;

            } else {

                killList.Add(_personalTasks[i]);

            }

        }

        foreach (Tasks.Task task in killList) {

            _personalTasks.Remove(task);

        }

        return chosenTask;

    }

    // Start is called before the first frame update
    void Start()
    {
        
        _body = GetComponent<Body>();

    }

    private float _requestTimer;

    // Update is called once per frame
    void Update()
    {
        
        //Check needs
        if (_body.isHungry || _body.isThirsty || _body.isSleepy) {

            if (_state == State.WORKING) {

                _curTask.OnAbandon(gameObject);
                _state = State.WAITING;

            }

            if (_body.isHungry) {
                _AddPersonalTask( new Tasks.Eat( Manifest.SearchForClosest(gameObject, Manifest.Category.FOOD) ) );
            }
            if (_body.isThirsty) {}
            if (_body.isSleepy) {}

        }

        switch (_state) {

            case State.WAITING:

                //Will either get a personal task or request one
                if (_requestTimer <= 0) {

                    if (_personalTasks.Count != 0) {

                        _curTask = _GetPersonalTask();

                        if (_curTask != null && _curTask.Valid()) {

                            _curTask.OnStart(gameObject);
                            _state = State.PERSONAL;

                        } else if (_curTask != null && !_curTask.Valid()) {

                            _curTask = null;

                        }

                    } else {

                        _curTask = Tasks.TaskManager.RequestTask();

                        if (_curTask != null && _curTask.Valid()) {

                            _curTask.OnStart(gameObject);
                            _state = State.WORKING;

                        } else if (_curTask != null && !_curTask.Valid()) {

                            _curTask = null;

                        }

                    }

                    _requestTimer = 1;

                } else {

                    _requestTimer -= Time.deltaTime;

                }

                break;

            case State.PERSONAL:
            case State.WORKING:

                if (_curTask.Valid()) {

                    if (_curTask.CanDo(gameObject)) {

                        if (_curTask.IsDone(gameObject)) {

                            _curTask = null;
                            _state = State.WAITING;

                        } else {

                            _curTask.OnWork(gameObject);

                        }

                    } else {

                        if (_state == State.PERSONAL) {

                            _curTask = null;
                            _state = State.WAITING;

                        } else {

                            Tasks.TaskManager.AddTask(_curTask);
                            _curTask = null;
                            _state = State.WAITING;

                        }

                    }

                } else {

                    _curTask = null;
                    _state = State.WAITING;

                }

                break;

        }

    }

}
