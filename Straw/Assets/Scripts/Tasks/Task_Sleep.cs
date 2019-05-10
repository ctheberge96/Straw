using UnityEngine;

public class Task_Sleep : Task
{

    private GameObject bed;

    public Task_Sleep(GameObject bed) {

        this.bed = bed;

    }

    private bool pathFound;

    private bool done;

    public void Abandon(GameObject unit)
    {
        
        if (unit.GetComponent<PathFollower>() != null) {

            unit.GetComponent<PathFollower>().Stop();

        }

    }

    public bool CanDo(GameObject unit)
    {
        
        return pathFound;

    }

    public bool IsDone(GameObject unit)
    {
        
        return done;

    }

    public bool IsValid()
    {
        
        return bed != null && bed.GetComponent<Bed>() != null;

    }

    public void OnStart(GameObject unit)
    {
        
        pathFound = unit.GetComponent<PathFollower>().PathTo(bed.transform.position);

    }

    public void OnWork(GameObject unit)
    {
        
        if (bed.gameObject.transform.position == unit.transform.position) {

            Body body = unit.GetComponent<Body>();

            body.Sleep(bed, Time.deltaTime * 4);

            if (body.totalEnergy == 100) { done = true; }

        }

    }

}