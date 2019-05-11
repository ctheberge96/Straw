using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LayeredEntity : MonoBehaviour
{
    private SpriteRenderer sprRenderer;

    private Color origColor;

    void Start() {
        sprRenderer = GetComponent<SpriteRenderer>();
        origColor = sprRenderer.color;
    }

    void Update() {

        sprRenderer.sortingOrder = ((int)Mathf.Ceil(transform.position.z) * 10000) - (int)Mathf.Ceil(Mathf.Pow(transform.position.y,2));

        float zDist = Mathf.Abs(Camera.main.transform.position.z - transform.position.z) + 1;
        sprRenderer.color = new Color32((byte)((origColor.r * 255f) / zDist),
                                        (byte)((origColor.g * 255f) / zDist),
                                        (byte)((origColor.b * 255f) / zDist),
                                        (byte)(origColor.a * 255f));
        
    }

}
