using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{

    public interface Task {

        bool IsDone(GameObject worker);

        void OnStart(GameObject worker);

        void OnWork(GameObject worker);

        void OnAbandon(GameObject worker);

        bool CanDo(GameObject worker);

        bool Valid();

    }

    public class TaskManager {
        
        private static List<Task> _tasks = new List<Task>();

        public static Task RequestTask() {
            
            List<int> killList = new List<int>();

            for (int i = 0; i < _tasks.Count; i++) {

                Task possibleTask = _tasks[i];

                if (possibleTask.Valid()) {

                    _CleanTasks(killList);
                    return possibleTask;

                } else {

                    killList.Add(i);

                }

            }

            _CleanTasks(killList);
            return null;

        }

        private static void _CleanTasks(List<int> killList) {

            foreach (int i in killList) {

                _tasks.RemoveAt(i);

            }

        }

        public static bool AddTask(Task task) {

            if (task.Valid()) {

                _tasks.Insert(_tasks.Count, task);
                return true;

            }

            return false;

        }

    }

}
