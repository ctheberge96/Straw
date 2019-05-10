using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenTestingGround : MonoBehaviour
{
    public GameObject[] blocks; //0 wood 1 glass 2 stair
    public GameObject testFood;
    public GameObject testDrink;

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
        "00 300   0" +
        "00 000   1" +
        "         1" +
        "         1" +
        "         0" +
        "        20" +
        "    4     " +
        "5         ";

        for (int z = 0; z < map.Length; z++) {
            string curMap = map[z];
            for (int i = 0; i < curMap.Length; i++) {
                if (curMap[i] == ' ') { continue; }
                GameObject obj = GameObject.Instantiate(blocks[int.Parse(curMap[i].ToString())], new Vector3((i % 10) * grid.cellSize.x, (i / 10) * grid.cellSize.y, z - 1), Quaternion.identity);

                //If it's a container, fill it.
                if (obj.GetComponent<Container>() != null) {

                    for (int ii = 0; ii < 5; ii++) {

                        GameObject apple = GameObject.Instantiate(testFood, new Vector3((i % 10) * grid.cellSize.x, (i / 10) * grid.cellSize.y, z - 1), Quaternion.identity);
                        apple.SetActive(false);
                        obj.GetComponent<Container>().AddItem(apple);

                    }

                    for (int ii = 0; ii < 5; ii++) {

                        GameObject coffee = GameObject.Instantiate(testDrink, new Vector3((i % 10) * grid.cellSize.x, (i / 10) * grid.cellSize.y, z - 1), Quaternion.identity);
                        coffee.SetActive(false);
                        obj.GetComponent<Container>().AddItem(coffee);

                    }

                }

                Manifest.Register(obj);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
