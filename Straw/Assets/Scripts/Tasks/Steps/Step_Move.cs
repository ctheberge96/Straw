using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Step_Move : Step
{

    public Step_Move(Vector3 location) {
        
        this.location = location;

    }

    private bool pathFound = false;

    private Vector3 location;
    
    public bool CanDo(GameObject assigned)
    {
        
        return pathFound;

    }

    public bool Completed(GameObject assigned)
    {
        
        return assigned.GetComponent<PathFollower>().Done()
                && assigned.transform.position == location;

    }

    public void Start(GameObject assigned)
    {
        
        PathFollower follower = assigned.GetComponent<PathFollower>();

        pathFound = follower != null && follower.PathTo(location);

    }

    public void Stop(GameObject assigned)
    {
        
        PathFollower follower = assigned.GetComponent<PathFollower>();

        if (follower != null) {

            follower.Stop();

        }

    }

    public bool Validate()
    {
        
        return location != null;

    }

    public void WorkOn(GameObject assigned)
    {

        //No work needs to be done, as the pathing begins upon Start().

    }

}