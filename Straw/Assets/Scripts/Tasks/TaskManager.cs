using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager
{

    private static LinkedList<Task> tasks = new LinkedList<Task>();
    
    public static void AddTask(Task task) {

        if (task.IsValid()) {

            tasks.AddLast(task);

        }

    }
    
    public static Task RequestTask(GameObject taskAI) {

        foreach (Task task in tasks) {

            if (task.IsValid() && task.CanDo(taskAI)) {

                Task result = task;
                tasks.Remove(task);
                return result;

            } else if (!task.IsValid()) {

                tasks.Remove(task);

            }

        }

        return null;

    }

}
