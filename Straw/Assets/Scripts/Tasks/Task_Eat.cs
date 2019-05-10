using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_Eat : Task
{

    private bool pathFound;

    private GameObject food;

    public Task_Eat(GameObject food) {

        this.food = food;

    }

    private bool eaten;

    public bool IsDone(GameObject unit)
    {
        
        return eaten;

    }

    public bool IsValid()
    {

        return food != null && food.GetComponent<Food>() != null;

    }

    public void OnStart(GameObject unit)
    {
        
        pathFound = unit.GetComponent<PathFollower>().PathTo(food.transform.position);

    }

    public void OnWork(GameObject unit)
    {
        
        if (unit.transform.position == food.transform.position && !eaten) {

            if (unit.GetComponent<Body>().holding == food) {

                unit.GetComponent<Body>().Eat(food);

                eaten = true;

            } else {

                unit.GetComponent<Body>().PickUp(food);

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
            (unit.GetComponent<PathFollower>() == null && (food.transform.position == unit.transform.position && unit.GetComponent<Body>() != null));

    }

}