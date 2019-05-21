using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenWorld : MonoBehaviour
{

    public GameObject floor;
    public GameObject wall;
    public GameObject food;

    // Start is called before the first frame update
    void Start()
    {
        
        string map =
        "11102" +
        "10100" +
        "10100" +
        "00010" +
        "00000";

        for (int i = 0; i < map.Length; i++) {

            Vector3Int pos = new Vector3Int(i % 5, i / 5, 0);

            if (map[i] == '0') {

                GameObject.Instantiate(floor,
                                   WorldManager.grid.CellToLocal(pos),
                                   Quaternion.identity);

            } else if (map[i] == '1') {

                GameObject.Instantiate(wall,
                                   WorldManager.grid.CellToLocal(pos),
                                   Quaternion.identity);
            
            } else if (map[i] == '2') {

                GameObject.Instantiate(floor,
                                   WorldManager.grid.CellToLocal(pos),
                                   Quaternion.identity);
                GameObject foodObj = GameObject.Instantiate(food,
                                                           WorldManager.grid.CellToLocal(pos),
                                                            Quaternion.identity);
                Manifest.Register(foodObj);

            }

        }

    }

}
