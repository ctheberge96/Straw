using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager
{
    public static Grid grid {
        get {
            return GameObject.Find("WorldManager").GetComponent<Grid>();
        }
    }

    public static GameObject CreateManifestedObject(GameObject source, Vector3 position) {

        if (source == null || position == null) { return null; }

        GameObject obj = GameObject.Instantiate(source, position, Quaternion.identity);

        Manifest.Register(obj);

        return obj;

    }

    public static void DestroyManifestedObject(GameObject obj) {

        Manifest.DeRegister(obj);
        GameObject.Destroy(obj);

    }

}
