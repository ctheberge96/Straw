using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Step {

    ///<summary>When this step has begun. Can be called multiple times if the step must be revisited.</summary>
    void Start(GameObject assigned);

    ///<summary>Checks if the step can be completed.</summary>
    ///<returns>Whether this step is physically possible.</returns>
    bool Validate();

    ///<summary>Checks if the step can be completed.</summary>
    ///<returns>Whether this step is possible for the assigned.</returns>
    bool CanDo(GameObject assigned);

    ///<summary>Checks if this step has been completed by the assigned.</summary>
    bool Completed(GameObject assigned);

    ///<summary>Makes the assigned work on the step. Can be called every frame.</summary>
    void WorkOn(GameObject assigned);

    ///<summary>Makes the assigned stop working, ensuring all things are returned to normal. May be called just once.</summary>
    void Stop(GameObject assigned);

}