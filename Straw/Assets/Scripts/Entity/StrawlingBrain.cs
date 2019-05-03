using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFollower))]
public class StrawlingBrain : MonoBehaviour
{

    public enum State
    {
        ASLEEP,
        EATING,
        IDLE
    }

    //MISC
    private State state;

    private GameObject held; //What's currently in the hand

    //NEEDS

    public float hungerPerSec;
    public float eatTime;
    private float eatingTimer;
    public float secsToStarvation;
    private float starveTimer = -1;

    public float thirstPerSec;
    public float thirstGainPerSec;
    public float secsToDehydration;
    private float dehydrateTimer = -1;

    public float energyPerSec;
    public float energyGainPerSec;
    public float secsToPassOut;
    private float passOutTimer = -1;

    private float hunger;
    private float thirst;
    private float energy;

    public float maxHp;
    private float hp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //NEEDS

        //Updating decays
        hunger = Mathf.Clamp(hunger - (hungerPerSec / 60f), 0, 100);
        thirst = Mathf.Clamp(thirst - (thirstPerSec / 60f), 0, 100);
        energy = Mathf.Clamp(energy - (energyPerSec / 60f), 0, 100);

        //Reacting to decays
        if (hunger == 0 && starveTimer == -1) {
            starveTimer = secsToStarvation;
        } else if (hunger != 0 && starveTimer != -1) {
            starveTimer = -1;
        }

        if (starveTimer == 0 && hunger == 0) {
            //die
        }

        if (thirst == 0 && dehydrateTimer == -1) {
            dehydrateTimer = secsToDehydration;
        } else if (thirst != 0 && dehydrateTimer != -1) {
            dehydrateTimer = -1;
        }

        if (dehydrateTimer == 0 && thirst == 0) {
            //die
        }

        if (energy == 0 && passOutTimer == -1) {
            passOutTimer = secsToPassOut;
        } else if (energy != 0 && passOutTimer != -1) {
            passOutTimer = -1;
        }

        if (passOutTimer == 0 && energy == 0) {
            //pass out
        }

        //MAIN STATE UPDATES
        switch (state) {

            case State.ASLEEP:
            
                energy = Mathf.Clamp(energy + (energyGainPerSec / 60f), 0, 100);
                break;

            case State.EATING:

                //Eat what's in hand if you can
                if (eatingTimer > 0) {

                    eatingTimer -= Time.deltaTime;

                } else {

                    Food heldFood = held.GetComponent<Food>();
                    hunger = Mathf.Clamp(hunger + heldFood.hungerValue, 0, 100);
                    thirst = Mathf.Clamp(thirst + heldFood.thirstValue, 0, 100);
                    held = null;
                    ChangeState(State.IDLE);

                }

                break;

        }

    }

    private bool ChangeState(State targetState) {
        
        //Commit any actions that need to happen based on state before change
        switch (targetState) {

            case State.ASLEEP:

                energy = Mathf.Clamp(energy + (energyGainPerSec / Time.deltaTime), 0, 100);

                break;

            case State.EATING:

                if (held == null || held.GetComponent<Food>() == null) {
                    return false;
                }

                eatingTimer = eatTime;

                break;

        }

        //Actually change it
        state = targetState;
        return true;

    }

}
