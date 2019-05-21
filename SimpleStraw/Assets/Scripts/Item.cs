using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool twoHanded;

    [Tooltip("If this item is considered food. If so, this will be considered during a food search.")]
    public bool isFood;
    [Tooltip("How much hunger this relieves the eater. Item does not need to be food to relieve hunger.")]
    public float foodValue;

    [Tooltip("If this item is considered a drink. If so, this will be considered during a drink search.")]
    public bool isDrink;
    [Tooltip("How much thirst this relieves the eater. Item does not need to be a drink to relieve thirst.")]
    public float drinkValue;

}
