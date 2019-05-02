using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenTestingGround : MonoBehaviour
{
    public GameObject[] blocks; //0 wood 1 glass 2 stair

    private string[] map = new string[2];

    // Start is called before the first frame update
    void Start() {
    
        Grid grid = Camera.main.GetComponent<Grid>();

        map[0] = 
        "0000000000" +
        "0000000000" +
        "0000000000" +
        "0000000000" +
        "0000000000" +
        "0000000000" +
        "          " +
        "0000000000" +
        "0000000000" +
        "0000000000";

        map[1] = 
        "000000    " +
        "000000  20" +
        "00  00   0" +
        "00 000   1" +
        "         1" +
        "         1" +
        "         0" +
        "        20" +
        "          " +
        "          ";

        for (int z = 0; z < map.Length; z++) {
            string curMap = map[z];
            for (int i = 0; i < curMap.Length; i++) {
                if (curMap[i] == ' ') { continue; }
                GameObject.Instantiate(blocks[int.Parse(curMap[i].ToString())], new Vector3((i % 10) * grid.cellSize.x, (i / 10) * grid.cellSize.y, z - 1), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
