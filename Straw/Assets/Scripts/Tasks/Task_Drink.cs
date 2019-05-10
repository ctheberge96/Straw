using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Drink : Task
{

    private bool pathFound;

    private GameObject drink;

    public Task_Drink(GameObject drink) {

        this.drink = drink;

    }

    private bool drank;

    public bool IsDone(GameObject unit)
    {
        
        return drank;

    }

    public bool IsValid()
    {

        return drink != null && drink.GetComponent<Drink>() != null;

    }

    public void OnStart(GameObject unit)
    {
        
        pathFound = unit.GetComponent<PathFollower>().PathTo(drink.transform.position);

    }

    public void OnWork(GameObject unit)
    {
        
        if (unit.transform.position == drink.transform.position && !drank) {

            if (unit.GetComponent<Body>().holding == drink) {

                unit.GetComponent<Body>().Drink(drink);

                drank = true;

            } else {

                unit.GetComponent<Body>().PickUp(drink);

            }

        }

    }

    public void Abandon(GameObject unit)
    {
         
        unit.GetComponent<Body>().DropHeld();
        if (unit.GetComponent<PathFollower>() != null) {

            unit.GetComponent<PathFollower>().Stop();

        }

    }

    public bool CanDo(GameObject unit)
    {

        return (unit.GetComponent<PathFollower>() != null && (pathFound && unit.GetComponent<Body>() != null)) ||
            (unit.GetComponent<PathFollower>() == null && (drink.transform.position == unit.transform.position && unit.GetComponent<Body>() != null));

    }

}