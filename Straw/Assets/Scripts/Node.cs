using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private Vector2 size;
    public readonly Vector2 position;
    public bool isClosed {
        get {
            return !Physics2D.BoxCast(position, size * Camera.main.GetComponent<Grid>().cellSize, 0, Vector2.zero);
        }
    }
}