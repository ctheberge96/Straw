using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{

    public Body body {
        get {
            return gameObject.GetComponent<Body>();
        }
    }

    private bool tryingToEat = false;
    private bool tryingToDrink = false;
    private bool tryingToSleep = false;

    // Update is called once per frame
    void Update()
    {

        if (body.isHungry && !tryingToEat) {

            GameObject container = Manifest.FindClosestContainer(transform.position, Manifest.Category.FOOD);
            GameObject food = Manifest.FindClosest(transform.position, Manifest.Category.FOOD);

            //Prefers containers.
            if (container != null) {

                tryingToEat = true;

                GetComponent<TaskAI>().TakePersonalTask(new Task_EatFromContainer(container));

            } else if (food != null) {

                tryingToEat = true;

                GetComponent<TaskAI>().TakePersonalTask(new Task_Eat(food));

            }

        }

        if (body.isThirsty && !tryingToDrink) {

            GameObject container = Manifest.FindClosestContainer(transform.position, Manifest.Category.DRINK);
            GameObject drink = Manifest.FindClosest(transform.position, Manifest.Category.DRINK);

            //Prefers containers.
            if (container != null) {

                tryingToDrink = true;

                GetComponent<TaskAI>().TakePersonalTask(new Task_DrinkFromContainer(container));

            } else if (drink != null) {

                tryingToDrink = true;

                GetComponent<TaskAI>().TakePersonalTask(new Task_Drink(drink));

            }

        }

        if (body.isSleepy && !tryingToSleep) {

            GameObject bed = Manifest.FindClosest(transform.position, Manifest.Category.BED);

            //Prefers containers.
            if (bed != null) {

                tryingToSleep = true;

                GetComponent<TaskAI>().TakePersonalTask(new Task_Sleep(bed));

            }

        }

    }

}
