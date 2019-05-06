using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    //How much this container can hold.
    [Range(1, 500), Tooltip("How many items this container can hold")]
    public int capacity;

    //The actual list representing the contents of the container
    private List<GameObject> contents = new List<GameObject>();

    ///<summary>Adds an item to the barrel.</summary>
    ///<returns>Returns whether the item was successfully added.</returns>
    public bool AddItem(GameObject item) {

        //If we can't put it in, report that.
        if (IsFull() || Contains(item) || GetComponent<Item>() == null) { return false; }

        contents.Add(item);

        //Re-register the container to update it.
        Manifest.Register(gameObject);

        return true;

    }

    ///<summary>Removes an item from the barrel.</summary>
    ///<returns>Returns whether the item was successfully removed.</returns>
    public bool RemoveItem(GameObject item) {

        return contents.Remove(item);

    }

    ///<summary>Finds and returns the object of the given Component type.</summary>
    ///<returns>The found object, or null if it doesn't exist.</returns>
    public GameObject FindItem(System.Type componentType) {

        for (int i = 0; i < count; i++) {

            //Try and get the component to see if it's in this GameObject
            if (this[i].GetComponent(componentType) != null) {

                return this[i];

            }

        }

        return null;

    }

    ///<summary>Finds and returns all objects of the given Component type.</summary>
    ///<returns>Returns a list of the objects. It can be empty.</returns>
    public List<GameObject> FindAllItems(System.Type componentType) {

        List<GameObject> objList = new List<GameObject>();

        for (int i = 0; i < count; i++) {

            //Try and get the component to see if it's in this GameObject
            if (this[i].GetComponent(componentType) != null) {

                objList.Add(this[i]);

            }

        }

        return objList;

    }

    ///<summary>Gets the item at this index.</summary>
    public GameObject this[int index] {

        get {

            return contents[index];

        }

    }

    ///<summary>Checks whether an item is in the container.</summary>
    public bool Contains(GameObject obj) {

        return contents.Contains(obj);

    }

    ///<summary>Finds how many items are currently in the container. Can't be more than its count.</summary>
    public int count {

        get {

            return contents.Count;

        }

    }

    ///<summary>Checks if the container is full.</summary>
    public bool IsFull() {

        return count >= capacity;
        
    }

}