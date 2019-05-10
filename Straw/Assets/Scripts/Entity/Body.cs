using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{

    //NEEDS
    public float hunger = 100;
    public bool isHungry {
        get { return hunger <= 20; }
    }
    public bool Eat(GameObject food) {

        if (food != null && food.GetComponent<Food>() != null) {

            hunger = Mathf.Clamp(hunger + food.GetComponent<Food>().hungerValue, 0, 100);
            
            return true;

        }

        return false;

    }

    public float thirst = 100;
    public bool isThirsty {
        get { return thirst <= 20; }
    }
    public bool Drink(GameObject drink) {

        if (drink != null && drink.GetComponent<Drink>() != null) {

            thirst = Mathf.Clamp(thirst + drink.GetComponent<Drink>().thirstValue, 0, 100);

            return true;

        }

        return false;

    }

    public float energy = 100;
    public float totalEnergy {
        get { return energy; }
    }
    public bool isSleepy {
        get { return energy <= 20; }
    }
    public bool Sleep(GameObject bed, float sleepValue) {

        if (bed != null && bed.GetComponent<Bed>() != null) {

            energy = Mathf.Clamp(energy + (bed.GetComponent<Bed>().sleepQuality * sleepValue), 0, 100);

            return true;

        }

        return false;

    }

    private float hp;
    [Tooltip("The maximum amount of health this body could ever have.")]
    public float maxHp;

    //NEED RATES
    [Tooltip("How many seconds it takes before the belly is entirely empty.")]
    public float hungryTime;
    private float starveTimer;

    [Tooltip("How many seconds it takes before the body is entirely out of water.")]
    public float thirstyTime;
    private float dehydrateTimer;

    [Tooltip("How many seconds it takes before the body is entirely out of energy.")]
    public float sleepyTime;
    private float passOutTimer;

    private void UpdateNeeds() {

        float hungerPerSec = 100f / hungryTime;
        hunger = Mathf.Clamp(hunger - (hungerPerSec * Time.deltaTime), 0, 100);

        float thirstPerSec = 100f / thirstyTime;
        thirst = Mathf.Clamp(thirst - (thirstPerSec * Time.deltaTime), 0, 100);

        float energyPerSec = 100f / sleepyTime;
        energy = Mathf.Clamp(energy - (energyPerSec * Time.deltaTime), 0, 100);

    }

    //CONSTRAINTS
    private float moveSpeed;
    public float speed {
        get {
            return moveSpeed * (isHungry ? .75f:1) * (isThirsty ? .75f:1) * (isSleepy ? .75f:1);
        }
    }

    private GameObject held;
    public void DropHeld() {
        Item.Drop(held, transform.position);
        held = null;
    }
    public void PickUp(GameObject item) {
        held = Item.PickUp(item);
    }

    public GameObject holding {
        get {
            return held;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateNeeds();

    }

}
