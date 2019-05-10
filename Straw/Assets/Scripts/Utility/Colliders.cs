using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders
{
    
    /*
    This could possibly use some improvements:
    - Many ArrayList additions is costly.
    - Using OverlapBoxAll is extra, but is required based on the overlapping 2D colliders.
    */

    public static Collider2D getCollider(Vector3 position, Vector2 size, int layermask) {

        Collider2D[] colliders = getColliders(position, size, layermask);

        return colliders.Length > 0 ? colliders[0]:null;

    }

    public static bool isFree(Vector3 position, Vector2 size, int layermask) {
        Collider2D collider = getCollider(position, size, layermask);
        return collider == null || collider.GetComponent<Tile>().isStair;
    }

    public static Collider2D[] getColliders(Vector3 position, Vector2 size, int layermask) {

        //COLLIDER EXTENTS, NOT SIZE!
        return Physics2D.OverlapAreaAll(position, new Vector2(position.x + .16f, position.y - .16f), layermask, position.z, position.z);

    } 

}
