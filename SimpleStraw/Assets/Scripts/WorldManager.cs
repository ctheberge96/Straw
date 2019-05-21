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
}
