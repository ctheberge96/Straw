using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Task
{

    ///<summary>What happens when this task is received by a TaskAI.</summary>   
    void OnStart(GameObject unit);

    ///<summary>What happens when this task is worked on by a TaskAI. Called once per frame.</summary>  
    void OnWork(GameObject unit);

    ///<summary>Checks if this task is currently valid for all TaskAI units.</summary>  
    bool IsValid();

    ///<summary>Checks if this task is currently doable for this TaskAI unit.</summary>  
    bool CanDo(GameObject unit);

    ///<summary>Checks if this task is completed.</summary>
    bool IsDone(GameObject unit);

    ///<summary>Resets this task</summary>  
    void Abandon(GameObject unit);

}
