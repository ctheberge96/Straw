using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manifest
{

    ///<summary>Categories for categorizing certain objects in the Manifest.</summary>
    public enum Category {
        FOOD, //Food / food containers
        DRINK, //Drinks / drink containers
        BED, //Where to sleep
        CONTAINER //Containers
    }

    //The internal dictionary running the manifest
    private static Dictionary<Category, List<GameObject>> manifest = new Dictionary<Category, List<GameObject>>();

    ///<summary>Registers an object in the manifest, auto detecting categories based on components.</summary>
    public static void Register(GameObject obj) {

        //Register food
        if (obj.GetComponent<Food>()) {

            Food food = obj.GetComponent<Food>();

            if (food.hungerValue > 0 && food.hungerValue > food.thirstValue) {

                Add(Category.FOOD, obj); //If it can be eaten to replenish hunger

            } else if (food.thirstValue > 0 && food.thirstValue > food.hungerValue) {

                Add(Category.DRINK, obj); //If it can be drank to replenish thirst

            } else if (food.thirstValue > 0 && food.hungerValue > 0) {

                //Can be considered both
                Add(Category.FOOD, obj);

                Add(Category.DRINK, obj);

            }
            
        }

        //Register containers
        if (obj.GetComponent<Container>()) {

            Add(Category.CONTAINER, obj);

            //Register it as food if it has food in it.
            List<GameObject> foods = obj.GetComponent<Container>().FindAllItems(typeof(Food));

            bool hunger = false; bool thirst = false;

            foreach (GameObject food in foods) {
                
                Food foodComp = food.GetComponent<Food>();

                if (foodComp) {
                    
                    //If you can eat it
                    if (!hunger && foodComp.hungerValue > 0) { Add(Category.FOOD, obj); hunger = true; }

                    //If you can drink it
                    if (!thirst && foodComp.thirstValue > 0) { Add(Category.DRINK, obj); thirst = true; }

                    if (hunger && thirst) { break; }

                }

            }

        }

        if (obj.GetComponent<Food>()) { Add(Category.FOOD, obj); }

        if (obj.GetComponent<Bed>()) { Add(Category.BED, obj); }

    }

    ///<summary>Adds an object into the manifest with the category.
    ///Ensures there's a list to put it into.</summary>
    private static void Add(Category category, GameObject obj) {

        if (!manifest.ContainsKey(category)) {

            manifest[category] = new List<GameObject>();
            
        }

        List<GameObject> list = manifest[category];

        if (!list.Contains(obj)) {

            list.Add(obj);

        }

    }

    ///<summary>Deregisters an object in the manifest, removing it from all categories.</summary>
    ///<returns>Returns whether the deregister was sucessful.</returns>
    public static bool DeRegister(GameObject obj) {

        Dictionary<Category, List<GameObject>>.ValueCollection coll = manifest.Values;

        bool result = false;

        foreach(List<GameObject> objList in coll) {

            if (objList.Remove(obj)) { result = true; }

        }

        return result;

    }

    ///<summary>Gets a list of all GameObjects within a category.</summary>
    public static List<GameObject> SeeList(Category category) {

        return manifest.ContainsKey(category) ? manifest[category]:null;

    }

}