using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders3D
{
    
    /*
    This could possibly use some improvements:
    - Many ArrayList additions is costly.
    - Using OverlapBoxAll is extra, but is required based on the overlapping 2D colliders.
    */

    public static Collider getCollider(Vector3 position, Vector3 size, int layermask) {

        RaycastHit[] colliders = getColliders(position, size, layermask);

        return colliders.Length > 0 ? colliders[0].collider:null;

    }

    public static bool isFree(Vector3 position, Vector3 size, int layermask) {
        Collider collider = getCollider(position, size, layermask);
        return collider == null;
    }

    public static RaycastHit[] getColliders(Vector3 position, Vector3 size, int layermask) {

        //COLLIDER EXTENTS, NOT SIZE!
        return Physics.BoxCastAll(position, size * .45f, Vector3.one, Quaternion.identity, .01f, LayerMask.GetMask("solid"));

    } 

}
