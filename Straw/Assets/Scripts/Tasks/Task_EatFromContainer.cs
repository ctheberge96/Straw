using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_EatFromContainer : Task
{

    private bool pathFound;

    private GameObject container;
    private GameObject taken;

    public Task_EatFromContainer(GameObject container) {

        this.container = container;

    }

    private bool eaten;

    public bool CanDo(GameObject unit)
    {

        if (unit.GetComponent<PathFollower>() != null) {

            return pathFound && unit.GetComponent<Body>() != null;
            
        } else {

            return container.transform.position == unit.transform.position && unit.GetComponent<Body>() != null;

        }

    }

    public bool IsDone(GameObject unit)
    {
        
        return eaten;

    }

    public bool IsValid()
    {
        
        return container != null &&
                container.GetComponent<Container>() != null &&
                container.GetComponent<Container>().FindItem(typeof(Food)) != null;

    }

    public void OnStart(GameObject unit)
    {
        
        pathFound = unit.GetComponent<PathFollower>().PathTo(container.transform.position);

    }

    public void OnWork(GameObject unit)
    {
        
        if (unit.transform.position == container.transform.position && !eaten && taken != null) {

            if (unit.GetComponent<Body>().holding == taken) {

                unit.GetComponent<Body>().Eat(taken);

                eaten = true;

            }

        } else if (unit.transform.position == container.transform.position && taken == null) {

            taken = container.GetComponent<Container>().TakeItem(typeof(Food));

            unit.GetComponent<Body>().PickUp(taken);

        }

    }

    public void Abandon(GameObject unit)
    {
         
        unit.GetComponent<Body>().DropHeld();
        if (unit.GetComponent<PathFollower>() != null) {

            unit.GetComponent<PathFollower>().Stop();

        }

    }

}