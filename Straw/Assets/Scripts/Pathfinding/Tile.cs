using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Tile : MonoBehaviour
{
    public bool isStair;
    public int moveCost;
    private SpriteRenderer sprRenderer;
    private Color origColor;

    void Start() {
        sprRenderer = GetComponent<SpriteRenderer>();
        origColor = sprRenderer.color;
    }

    void Update() {

        float zDist = Mathf.Abs(Camera.main.transform.position.z - transform.position.z) + 1;
        sprRenderer.color = new Color32((byte)((origColor.r * 255f) / zDist),
                                        (byte)((origColor.r * 255f) / zDist),
                                        (byte)((origColor.r * 255f) / zDist),
                                        (byte)(origColor.a * 255f));

        sprRenderer.sortingOrder = (int)Mathf.Floor(transform.position.z);
        
    }
}
