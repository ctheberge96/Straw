using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredEntity : MonoBehaviour
{
    private SpriteRenderer sprRenderer;

    void Start() {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {

        sprRenderer.sortingOrder = (int)Mathf.Ceil(transform.position.z);
        
    }
}
