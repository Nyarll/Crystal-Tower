using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    GameObject camera;

    [SerializeField]
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        this.MoveCamera();
    }

    private void MoveCamera()
    {
        camera.transform.position =
            new Vector3(target.transform.position.x,
            target.transform.position.y,
            camera.transform.position.z);
    }
}
