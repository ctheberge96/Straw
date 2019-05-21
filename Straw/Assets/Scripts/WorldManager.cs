using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public int secondsPerDay;
    public Vector3 cellSize;
    public GameObject cubePrefab;

    public Vector3 ConformToGrid(Vector3 worldPosition) {

        return new Vector3( Mathf.RoundToInt(worldPosition.x / cellSize.x) * cellSize.x,
                            Mathf.RoundToInt(worldPosition.y / cellSize.y) * cellSize.y,
                            Mathf.RoundToInt(worldPosition.z / cellSize.z) * cellSize.z );

    }

    public Vector3 LocalToGrid(Vector3 local) {

        Vector3 even = ConformToGrid(local);
        return new Vector3(even.x / cellSize.x, even.y / cellSize.y, even.z / cellSize.z);

    }

    public Vector3 GridToLocal(Vector3 grid) {

        return new Vector3(grid.x * cellSize.x, grid.y * cellSize.y, grid.z * cellSize.z);

    }

    public void CreateCube(Vector3 position) {

        GameObject cube = GameObject.Instantiate(cubePrefab, ConformToGrid(position), Quaternion.identity);
        cube.transform.localScale = cellSize;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
