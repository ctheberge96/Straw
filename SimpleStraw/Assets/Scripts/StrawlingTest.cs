using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawlingTest : MonoBehaviour
{
    Pathfinding.Path myPath;
    int curPoint;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            
            ContextMenu.Option opt = new ContextMenu.Option("Test Option",
                                                            () => Debug.Log("Test option clicked!"));

            ContextMenu.ContextMenuBuilder.BuildContextMenu(Vector3.zero, opt);

        }
    }
}
