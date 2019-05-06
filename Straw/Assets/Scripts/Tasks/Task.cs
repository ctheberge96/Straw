using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Task {

    public enum Status { 

        OKAY,
        DONE,
        IMPOSSIBLE,
        CANTDO,
        NOTASSIGNED

    }

    public Task(params Step[] steps) {

        this.steps.AddRange(steps);

    }

    private GameObject assigned;
    public void Assign(GameObject entity) {
        assigned = entity;
    }
    public void UnAssign() {
        assigned = null;
    }

    private List<Step> steps;

    private int curStepInd;

    private Step curStep {

        get {

            return curStepInd != steps.Count ? steps[curStepInd]:null;

        }

    }

    private void NextStep() {

        curStepInd = Mathf.Clamp(curStepInd + 1, 0, steps.Count);

    }

    public bool done {

        get {

            foreach (Step step in steps) {

                if (!step.Validate()) {

                    return false;

                }

            }

            return curStep == null;

        }

    }

    public Status WorkOn() {

        if (assigned == null) { return Status.NOTASSIGNED; }

        if (done) { return Status.DONE; } //task is completed

        if (curStep.Completed(assigned)) {

            NextStep();

            return WorkOn(); //do the next step

        } else {

            //If we can do it,
            if (curStep.Validate() && curStep.CanDo(assigned)) {

                //Do it.
                curStep.WorkOn(assigned);

                return Status.OKAY;

            //If it's impossible,
            } else if (!curStep.Validate()) {

                //Say so.
                return Status.IMPOSSIBLE;

            //If we alone cannot do it,
            } else {

                //Say so.
                return Status.CANTDO;

            }

        }

    }

    public void Stop() {

        if (!done) { curStep.Stop(assigned); }
    
    }

}