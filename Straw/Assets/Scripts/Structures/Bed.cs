using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    [Range(0, 1), Tooltip("How good the bed is. Lower value, the longer Strawlings need to sleep.")]
    public float sleepQuality;

    private GameObject occupant;
    
    public bool Occupy(GameObject occupant) {

        this.occupant = (this.occupant == null ? occupant:this.occupant);
        return this.occupant == occupant;

    }

    public void DeOccupy(GameObject currentOccupant) {

        this.occupant = (this.occupant == currentOccupant ? null:this.occupant);

    }

    public GameObject GetOccupant() {

        return occupant;

    }

    public bool IsEmpty() {

        return occupant == null;

    }

}
