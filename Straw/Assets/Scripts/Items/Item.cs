using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    ///<summary>Drops the item. Enacts the proper actions upon the item.</summary>
    public static void Drop(GameObject item, Vector3 position) {

        //Make sure it's actually an item...
        if (item.GetComponent<Item>() != null) {

            //Move the item to us so it makes sense when it appears.
            item.transform.position = position;

            //It will no longer be held, so it's active to ensure it continues its natural functions.
            item.SetActive(true);

            Manifest.Register(item);

        }

    }

    ///<summary>Picks up the item and returns it for use. Enacts the proper actions upon the item.</summary>
    public static GameObject PickUp(GameObject item) {

        //Make sure it's actually an item...
        if (item.GetComponent<Item>() != null) {

            //It's in our hands, so it's inactive to ensure it does nothing while we hold it
            item.SetActive(false);

            Manifest.DeRegister(item);

            return item;

        }

        return null;

    }

    ///<summary>Deletes the item (and GameObject). Enacts the proper actions upon the item.</summary>
    public static void Delete(GameObject item) {

        //Make sure it's actually an item...
        if (item.GetComponent<Item>() != null) {

            Manifest.DeRegister(item);

            GameObject.Destroy(item);

        }

    }

}
