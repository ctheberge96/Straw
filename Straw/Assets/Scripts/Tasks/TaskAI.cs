using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAI : MonoBehaviour
{

    public enum Status {
        WAITING,
        WORKING
    }

    private Status status;

    private float timer = 0;

    private Queue<Task> personalQueue = new Queue<Task>();
    private bool personalTask;

    private Task task;

    void Update() {

        if (status == Status.WAITING) {

            //get new task
            timer -= Time.deltaTime;

            if (timer <= 0) {

                if (personalQueue.Count == 0) {

                    task = TaskManager.RequestTask(gameObject);

                } else {
                    
                    task = personalQueue.Dequeue();
                    personalTask = true;

                }

                timer = 1;

                if (task != null) { status = Status.WORKING; task.OnStart(gameObject); }

            }

        } else {

            if (!personalTask && personalQueue.Count > 0) {

                AbandonTask();
                ResetTask();
                task = personalQueue.Dequeue();
                personalTask = true;
                return;

            }

            //Work on the current one

            if (!task.IsValid()) {
                
                //Throw it away
                ResetTask();

            } else {

                if (!task.CanDo(gameObject)) {
                    
                    if (!personalTask) {

                        TaskManager.AddTask(task);

                    }

                    ResetTask();
                    
                } else {

                    task.OnWork(gameObject);

                    if (task.IsDone(gameObject)) {

                        ResetTask();

                    }

                }

            }

        }

    }

    private void ResetTask() {

        task = null;
        status = Status.WAITING;
        personalTask = false;

    }

    public void AbandonTask() {
        
        if (task != null) {

            task.Abandon(gameObject);
            ResetTask();

        }

    }

    public void TakeTask(Task task) {

        if (task != null) {

            AbandonTask();
            this.task = task;
            status = Status.WORKING;
            this.task.OnStart(gameObject);

        }

    }

    public void TakePersonalTask(Task task) {

        if (!personalQueue.Contains(task)) {

            personalQueue.Enqueue(task);

        }

    }

}