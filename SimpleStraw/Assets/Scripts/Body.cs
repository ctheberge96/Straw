using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>A body is the biological part of the entity. It has needs
///of which need to be filled, although it cannot do this itself.
///It requires a Brain to do the work.</summary>
public class Body : MonoBehaviour
{

    private Text _needsText;

    void Start() {

        _needsText = gameObject.GetComponentInChildren<Text>();

    }

    //HOLDING
    private GameObject _held;

    ///<summary>Picks up the given item, dropping whatever is currently in-hand.</summary>
    public void PickUp(GameObject obj) {

        Drop();
        obj.SetActive(false);
        _held = obj;

    }

    ///<summary>Drops whatever is currently in-hand.</summary>
    public void Drop() {

        if (_held != null) {

            _held.SetActive(true);
            _held.transform.position = transform.position;
            _held = null;

        }

    }

    ///<summary>Checks if this body is holding the given object</summary>
    public bool IsHolding(GameObject obj) {

        return obj == _held;

    }

    //NEEDS
    [Range(0,100), Tooltip("What the hunger must be below to be considered 'hungry'")]
    public float hungerThreshold;
    [Tooltip("The time until the Strawling's hunger reaches 0 (in seconds)")]
    public float timeUntilHungry;

    [Range(0,100), Tooltip("What the thirst must be below to be considered 'thirsty'")]
    public float thirstThreshold;
    [Tooltip("The time until the Strawling's thirst reaches 0 (in seconds)")]
    public float timeUntilThirsty;

    [Range(0,100), Tooltip("What the energy must be below to be considered 'sleepy'")]
    public float energyThreshold;
    [Tooltip("The time until the Strawling's energy reaches 0 (in seconds)")]
    public float timeUntilSleepy;

    private float _hunger = 100;
    private void _Eat(float foodValue) {

        _hunger = Mathf.Clamp(_hunger + foodValue, 0, 100);

    }
    public bool isHungry {
        get {
            return _hunger < hungerThreshold;
        }
    }

    private float _thirst = 100;
    private void _Drink(float drinkValue) {

        _thirst = Mathf.Clamp(_thirst + drinkValue, 0, 100);

    }
    public bool isThirsty {
        get {
            return _thirst < thirstThreshold;
        }
    }

    ///<summary>Eats what's currently held.</summary>
    ///<returns>Returns whether the held object was eaten</returns>
    public bool Consume() {

        Item item = _held.GetComponent<Item>();

        if (item != null) {

            _Drink(item.drinkValue);
            _Eat(item.foodValue);
            Manifest.DeRegister(_held);
            GameObject.Destroy(_held);
            _held = null;
            return true;

        }

        return false;

    }

    private float _energy = 100;
    public void Sleep(float energyChange) {
        _energy = Mathf.Clamp(_energy + energyChange, 0, 100);
    }
    public bool isSleepy {
        get {
            return _energy < energyThreshold;
        }
    }

    public int maxHealth;

    private float _hp;
    public void Hurt(float damage) {

        _hp = Mathf.Clamp(_hp - damage, 0, maxHealth);

    }
    public void Heal(float health) {

        _hp = Mathf.Clamp(_hp + health, 0, maxHealth);

    }

    //Time until the Strawling dies
    private float _deathTimer = -1;

    //Time until the Strawling passes out (goes to sleep)
    private float _passOutTimer = -1;

    // Update is called once per frame
    void Update()
    {

        //Checking if this Strawling should be dead
        if (_deathTimer <= 0 && _deathTimer != -1) {

            //Die
            return;

        }

        //Checking if this Strawling should be asleep
        if (_passOutTimer <= 0 && _passOutTimer != -1) {

            //Passout

        }
        
        //Updating energy
        float prevEnergy = _energy;
        _energy = Mathf.Clamp(_energy - ((100 / timeUntilSleepy) * Time.deltaTime), 0, 100);

        //Checking if the pass out timer needs to be started
        if (prevEnergy != 0 && _energy == 0 && _passOutTimer == -1) {

            _passOutTimer = timeUntilSleepy / 2;

        }

        //Updating thirst
        float prevThirst = _thirst;
        _thirst = Mathf.Clamp(_thirst - ((100 / timeUntilThirsty) * Time.deltaTime), 0, 100);

        //Updating hunger
        float prevHunger = _hunger;
        _hunger = Mathf.Clamp(_hunger - ((100 / timeUntilHungry) * Time.deltaTime), 0, 100);

        //Checking if the death timer needs to be started
        if (prevHunger != 0 && _hunger == 0 && _deathTimer == -1) {

            _deathTimer = timeUntilHungry;

        } else if (prevThirst != 0 && _thirst == 0 && _deathTimer == -1) {

            _deathTimer = timeUntilThirsty;

        }

        //DEBUGGING
        _needsText.text = string.Format("Hunger: {0}\n" +
                            "Thirst: {1}\n" +
                            "Energy: {2}", (int)Mathf.Ceil(_hunger), (int)Mathf.Ceil(_thirst), (int)Mathf.Ceil(_energy));

        //Updating death timer
        if (_deathTimer != -1) {

            _deathTimer -= Time.deltaTime;

            if (_hunger != 0 && _thirst != 0) {

                _deathTimer = -1;

            }

        }

        //Updating pass out timer
        if (_passOutTimer != -1) {

            _passOutTimer -= Time.deltaTime;

            if (_energy != 0) {

                _passOutTimer = -1;

            }

        }

    }
}