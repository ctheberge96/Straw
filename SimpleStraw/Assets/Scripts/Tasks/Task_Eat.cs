using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks {

    public class Eat : Task
    {

        private GameObject _target;
        private bool _ate = false;

        public Eat(GameObject food) {

            this._target = food;

        }

        public bool CanDo(GameObject worker)
        {
            return worker.GetComponent<PathFollower>().status != PathFollower.Status.BLOCKED;
        }

        public bool IsDone(GameObject worker)
        {
            
            return _ate;

        }

        public void OnAbandon(GameObject worker)
        {

            worker.GetComponent<PathFollower>().Stop();
            
            if (worker.GetComponent<Body>().IsHolding(_target)) {

                worker.GetComponent<Body>().Drop();

            }

        }

        public void OnStart(GameObject worker)
        {
            
            worker.GetComponent<PathFollower>().PathTo(_target.transform.position);

        }

        public void OnWork(GameObject worker)
        {
            
            if ( worker.GetComponent<PathFollower>().status == PathFollower.Status.DONE && !worker.GetComponent<Body>().IsHolding(_target)) {

                worker.GetComponent<Body>().PickUp(_target);

            } else if (worker.GetComponent<PathFollower>().status == PathFollower.Status.DONE) {

                worker.GetComponent<Body>().Consume();
                _ate = true;

            }

        }

        public bool Valid()
        {
            
            return !_ate && _target != null;

        }

    }

}