using UnityEngine;
using Unity;

public class EntityBillboard : MonoBehaviour {

    public bool omnidirectional;

    void Update() {

        //Always look at the camera
        transform.LookAt(Camera.main.transform.position);
        
        if (omnidirectional) {

            //Change your sprite based on where the camera is looking at you from based on forward vector
            float entityAngle = Vector2.SignedAngle(new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z),
                                            new Vector2(transform.parent.forward.x, transform.parent.forward.z)); //Angle between camera & entity vectors

            Animator animator = GetComponent<Animator>();

            animator.SetFloat("Angle", entityAngle);

        }

    }

}