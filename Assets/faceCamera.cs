using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceCamera : MonoBehaviour
{
    private Transform localTrans;

    private void Start()
    {
        localTrans = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Camera.main) {
            localTrans.LookAt(2 * localTrans.position - Camera.main.transform.position);
        }
    }
}
