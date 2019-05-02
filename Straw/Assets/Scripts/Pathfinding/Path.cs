using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private List<Vector3> nodes;

    public int length {
        get {
            return nodes.Count;
        }
    }

    public Path(List<Vector3> nodes) {
        this.nodes = nodes;
    }

    public Vector3 this[int index] {
        get {
            return nodes[index];
        }
    } 

}