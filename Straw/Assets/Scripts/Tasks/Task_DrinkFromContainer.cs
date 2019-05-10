using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_DrinkFromContainer : Task
{

    private bool pathFound;

    private GameObject container;
    private GameObject taken;

    public Task_DrinkFromContainer(GameObject container) {

        this.container = container;

    }

    private bool drank;

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
        
        return drank;

    }

    public bool IsValid()
    {
        
        return container != null &&
                container.GetComponent<Container>() != null &&
                container.GetComponent<Container>().FindItem(typeof(Drink)) != null;

    }

    public void OnStart(GameObject unit)
    {
        
        pathFound = unit.GetComponent<PathFollower>().PathTo(container.transform.position);

    }

    public void OnWork(GameObject unit)
    {
        
        if (unit.transform.position == container.transform.position && !drank && taken != null) {

            if (unit.GetComponent<Body>().holding == taken) {

                unit.GetComponent<Body>().Drink(taken);

                drank = true;

            }

        } else if (unit.transform.position == container.transform.position && taken == null) {

            taken = container.GetComponent<Container>().TakeItem(typeof(Drink));

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