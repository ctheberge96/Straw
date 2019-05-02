using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TileNoComp
{
    public TileNoComp(int moveCost, bool isStair) {
        this.moveCost = moveCost;
        this.isStair = isStair;
    }

    public bool isStair;
    public int moveCost;
}
