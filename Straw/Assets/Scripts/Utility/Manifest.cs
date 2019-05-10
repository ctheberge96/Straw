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
    private static Dictionary<Category, List<GameObject>> manifest = new Dictionary<Category, List<GameObject>>(); //For objects on their own
    private static Dictionary<Category, List<GameObject>> manifestContainers = new Dictionary<Category, List<GameObject>>(); //For objects in containers (containers which contain category)

    ///<summary>Registers an object in the manifest, auto detecting categories based on components.</summary>
    public static void Register(GameObject obj) {

        //Register food
        if (obj.GetComponent<Food>() != null) {

            Add(Category.FOOD, obj);
            
        }

        //Register drinks
        if (obj.GetComponent<Drink>() != null) {

            Add(Category.DRINK, obj);
            
        }

        //Register containers
        if (obj.GetComponent<Container>()) {

            Add(Category.CONTAINER, obj);

            if (obj.GetComponent<Container>().FindItem(typeof(Food)) != null) {

                AddContainer(Category.FOOD, obj);

            }

            if (obj.GetComponent<Container>().FindItem(typeof(Drink)) != null) {

                AddContainer(Category.DRINK, obj);

            }

        }
        
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

    private static void AddContainer(Category category, GameObject container) {

        if (container.GetComponent<Container>() == null) { return; }

        if (!manifestContainers.ContainsKey(category)) {

            manifestContainers[category] = new List<GameObject>();
            
        }

        List<GameObject> list = manifestContainers[category];

        if (!list.Contains(container)) {

            list.Add(container);

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

    ///<summary>Gets a list of all GameObjects within a category.</summary>
    public static List<GameObject> SeeContainerList(Category category) {

        return manifestContainers.ContainsKey(category) ? manifestContainers[category]:null;

    }

    public static GameObject FindClosest(Vector3 point, Category category) {

        List<GameObject> catList = Manifest.SeeList(category);

        if (catList == null) { return null; }

        //Finds the closest container & lone food.
        GameObject bestObj = null;

        foreach (GameObject obj in catList) {

            if (bestObj == null ||
                Vector3.Distance(point, obj.transform.position) < Vector3.Distance(point, bestObj.transform.position)) {

                bestObj = obj;

            }

        }

        return bestObj;

    }

    public static GameObject FindClosestContainer(Vector3 point, Category category) {

        List<GameObject> catList = Manifest.SeeContainerList(category);

        if (catList == null) { return null; }

        //Finds the closest container & lone food.
        GameObject bestObj = null;

        foreach (GameObject obj in catList) {

            if (bestObj == null ||
                Vector3.Distance(point, obj.transform.position) < Vector3.Distance(point, bestObj.transform.position)) {

                bestObj = obj;

            }

        }

        return bestObj;

    }

}