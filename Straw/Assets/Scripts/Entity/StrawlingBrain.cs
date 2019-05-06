using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFollower), typeof(AudioSource))]
public class StrawlingBrain : MonoBehaviour
{

    ///<summary>Represents a Strawling's state of being</summary>
    public enum State
    {
        ASLEEP, //Sleeping
        EATING, //Eating what is in hand
        IDLE, //Idle, where they decide what state to be in
        PICKUP //Working to pick something up. Moving to it is included.
    }

    [Tooltip("The sound played when they are eating. Stops abruptly when eating is done.")]
    public AudioClip eatingSound;

    //STATES
    public State state = State.IDLE; //Current state
    private State prevState; //The last state they were in

    private GameObject held; //What is in their hands
    private GameObject target; //What they are currently focused on
    private System.Type targetComp; //The component they are currently focused on

    //NEEDS

    //Hunger
    public float hunger = 100; //curent hunger level

    [Tooltip("How much hunger is lost per second. Hunger has a max of 100.")]
    public float hungerPerSec;

    [Tooltip("How many seconds is needed before a piece of food is eaten")]
    public float eatTime;

    private float eatingTimer; //The current countdown to eat completion

    [Tooltip("How many seconds until the Strawling starves (dies)")]
    public float secsToStarvation;

    private float starveTimer = -1; //The current countdown to starvation

    //Thirst
    public float thirst = 100;

    [Tooltip("How much thirst is lost per second. Thirst has a max of 100.")]
    public float thirstPerSec;

    [Tooltip("How many seconds until the Strawling dehydrates (dies)")]
    public float secsToDehydration;

    private float dehydrateTimer = -1; //The current countdown to starvation

    //Energy/Sleep
    public float energy = 100;

    [Tooltip("Energy lost per second. Energy has a max of 100.")]
    public float energyPerSec;

    [Tooltip("How much energy is gained per second of sleep.")]
    public float energyGainPerSec;

    [Tooltip("How many seconds until the Strawling passes out (falls asleep)")]
    public float secsToPassOut;

    private float passOutTimer = -1; //The current countdown to passout

    //Health
    [Tooltip("The max amount of health the Strawling can have")]
    public float maxHp;

    private float hp; //current HP

    // Update is called once per frame
    void Update()
    {
        
        //NEEDS

        //Updating decays
        hunger = Mathf.Clamp(hunger - (hungerPerSec * Time.deltaTime), 0, 100);
        thirst = Mathf.Clamp(thirst - (thirstPerSec * Time.deltaTime), 0, 100);
        energy = Mathf.Clamp(energy - (energyPerSec * Time.deltaTime), 0, 100);

        //Reacting to decays
        if (hunger == 0 && starveTimer == -1) {

            //Set the timer if the hunger is 0 & we haven't started the timer yet
            starveTimer = secsToStarvation;

        } else if (hunger != 0 && starveTimer != -1) {

            //If the timer is going, but we're no longer hungry, reset timer.
            starveTimer = -1;

        }

        if (starveTimer == 0 && hunger == 0) {

            //die

        }

        if (thirst == 0 && dehydrateTimer == -1) {

            //Set the timer if the thirst is 0 & we haven't started the timer yet
            dehydrateTimer = secsToDehydration;

        } else if (thirst != 0 && dehydrateTimer != -1) {

            //If the timer is going, but we're no longer thirsty, reset timer.
            dehydrateTimer = -1;

        }

        if (dehydrateTimer == 0 && thirst == 0) {

            //die

        }

        if (energy == 0 && passOutTimer == -1) {

            //Set the timer if the energy is 0 & we haven't started the timer yet
            passOutTimer = secsToPassOut;

        } else if (energy != 0 && passOutTimer != -1) {

            //If the timer is going, but we're no longer tired, reset timer.
            passOutTimer = -1;

        }

        if (passOutTimer == 0 && energy == 0) {

            //pass out

        }

        //MAIN STATE UPDATES
        switch (state) {

            case State.IDLE:

                //When idle, figure out what to do

                if (hunger < 20) {

                    //I'm hungry!

                    //If I'm holding something edible
                    if (held != null && held.GetComponent<Food>() != null) {

                        ChangeState(State.EATING);

                    } else {

                        //Not holding anything, need to find some food!

                        List<GameObject> foodList = Manifest.SeeList(Manifest.Category.FOOD);

                        if (foodList == null) { break; } //No food!

                        //Finds the closest container & lone food.
                        GameObject bestContainer = null;
                        GameObject bestFood = null;

                        foreach (GameObject obj in foodList) {
                            
                            Container container = obj.GetComponent<Container>();
                            Food food = obj.GetComponent<Food>();

                            if (container != null) {

                                if (bestContainer == null ||
                                    Vector3.Distance(transform.position, obj.transform.position) < Vector3.Distance(transform.position, bestContainer.transform.position)) {

                                    bestContainer = obj;

                                }

                            } else if (food != null) {

                                if (bestFood == null ||
                                    Vector3.Distance(transform.position, obj.transform.position) < Vector3.Distance(transform.position, bestFood.transform.position)) {

                                    bestFood = obj;

                                }

                            }

                        }

                        //Prefers containers.
                        if (bestContainer != null) {

                            target = bestContainer;
                            targetComp = typeof(Food);
                            ChangeState(State.PICKUP);

                        } else if (bestFood != null) {
                            
                            target = bestFood;
                            ChangeState(State.PICKUP);

                        }

                    }

                }

                if (thirst < 20) {

                    //drriiinnnkkkk

                    //If I'm holding something edible
                    if (held != null && held.GetComponent<Food>() != null) {

                        ChangeState(State.EATING);

                    } else {

                        //Not holding anything, need to find some food!

                        List<GameObject> drinkList = Manifest.SeeList(Manifest.Category.DRINK);

                        if (drinkList == null) { break; } //No drink!

                        //Finds the closest container & lone drink.
                        GameObject bestContainer = null;
                        GameObject bestFood = null;

                        foreach (GameObject obj in drinkList) {
                            
                            Container container = obj.GetComponent<Container>();
                            Food food = obj.GetComponent<Food>();

                            if (container != null) {

                                if (bestContainer == null ||
                                    Vector3.Distance(transform.position, obj.transform.position) < Vector3.Distance(transform.position, bestContainer.transform.position)) {

                                    bestContainer = obj;

                                }

                            } else if (food != null) {

                                if (bestFood == null ||
                                    Vector3.Distance(transform.position, obj.transform.position) < Vector3.Distance(transform.position, bestFood.transform.position)) {

                                    bestFood = obj;

                                }

                            }

                        }

                        //Prefers containers.
                        if (bestContainer != null) {

                            target = bestContainer;
                            targetComp = typeof(Food);
                            ChangeState(State.PICKUP);

                        } else if (bestFood != null) {
                            
                            target = bestFood;
                            ChangeState(State.PICKUP);

                        }

                    }

                }

                if (energy < 20) {

                    //sleeeep

                    //Not holding anything, need to find some food!

                    List<GameObject> bedList = Manifest.SeeList(Manifest.Category.BED);

                    if (bedList == null) { break; } //No drink!

                    //Finds the closest bed
                    GameObject bestBed = null;

                    foreach (GameObject bedObj in bedList) {
                        
                        Bed bedComp = bedObj.GetComponent<Bed>();

                        if (bedComp.IsEmpty()) {

                            if (bestBed == null ||
                                Vector3.Distance(transform.position, bedObj.transform.position) < Vector3.Distance(transform.position, bedObj.transform.position)) {

                                bestBed = bedObj;

                            }

                        }

                    }

                    if (bestBed != null) {
                        
                        bestBed.GetComponent<Bed>().Occupy(gameObject);
                        target = bestBed;
                        ChangeState(State.ASLEEP);

                    }

                }

                break;

            case State.ASLEEP:

                if (target == null) {

                    GetComponent<PathFollower>().Stop();
                    ChangeState(State.IDLE);

                } else {

                    if (GetComponent<PathFollower>().Done() && transform.position == target.transform.position) {

                        //sleep
                        energy = Mathf.Clamp(energy + ((energyGainPerSec / Time.deltaTime) * target.GetComponent<Bed>().sleepQuality), 0, 100);

                        //INTERRUPTS HERE!

                        if (energy == 100) {

                            target = null;
                            ChangeState(State.IDLE);

                        }

                    } else if (GetComponent<PathFollower>().Done() && transform.position != target.transform.position) {

                        //We're not there, but done. Couldn't get there!
                        target = null;
                        ChangeState(State.IDLE);

                    }

                }

                break;

            case State.EATING:

                //Eat what's in hand if you can
                if (eatingTimer > 0) {

                    eatingTimer -= Time.deltaTime;

                } else {

                    Food heldFood = held.GetComponent<Food>();
                    hunger = Mathf.Clamp(hunger + heldFood.hungerValue, 0, 100);
                    thirst = Mathf.Clamp(thirst + heldFood.thirstValue, 0, 100);
                    Item.Delete(held);
                    held = null;
                    AudioSource audio = GetComponent<AudioSource>();

                    if (audio.clip == eatingSound) {
                        audio.Stop();
                    }

                    ChangeState(State.IDLE);

                }

                break;

            case State.PICKUP:

                if (target == null && targetComp == null) {
                    GetComponent<PathFollower>().Stop();
                }

                if (GetComponent<PathFollower>().Done() && transform.position == target.transform.position) {

                    //put in hand

                    if (targetComp != null) {

                        Container container = target.GetComponent<Container>();

                        GameObject obj = container.FindItem(targetComp);

                        PickUp(obj);

                    } else {

                        PickUp(target);

                    }

                    target = null;
                    targetComp = null;
                    ChangeState(prevState);

                } else if (GetComponent<PathFollower>().Done() && transform.position != target.transform.position) {

                    //We're not there, but done. Couldn't get there!
                    target = null;
                    targetComp = null;
                    ChangeState(prevState);

                }

                break;

        }

    }

    private bool ChangeState(State targetState) {
        
        //Commit any actions that need to happen based on state before change
        switch (targetState) {

            case State.ASLEEP:

                PathFollower follower = GetComponent<PathFollower>();

                follower.PathTo(target.transform.position);

                break;

            case State.EATING:

                if (held == null || held.GetComponent<Food>() == null) {
                    return false;
                }

                eatingTimer = eatTime;

                AudioSource audio = GetComponent<AudioSource>();
                audio.clip = eatingSound;
                audio.Play();

                break;

            case State.PICKUP:

                if ((target == null && targetComp == null) || (target.GetComponent<Item>() == null && target.GetComponent<Container>() == null)) {
                    return false;
                }

                PathFollower pufollower = GetComponent<PathFollower>();

                pufollower.PathTo(target.transform.position);

                break;

        }

        //Actually change it
        prevState = state;
        state = targetState;
        return true;

    }

    private void PickUp(GameObject item) {

        Item itemComponent = item.GetComponent<Item>();

        if (held != null) {
            Item.Drop(held, transform.position);
        }

        held = Item.PickUp(item);

        Manifest.DeRegister(item);

    }

}
