using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMaker
{
    
    public static void MakeBubble(GameObject parent, float lifetime, string text) {

        //Instantiate the bubble itself
        GameObject bubble = (GameObject)GameObject.Instantiate(Resources.Load("Speech Bubble"));
        bubble.transform.SetParent(parent.transform);

        BubbleScript bScript = bubble.GetComponent<BubbleScript>();
        bScript.parent = parent;
        bScript.life = lifetime;
        bScript.fullText = text;

    }

}
