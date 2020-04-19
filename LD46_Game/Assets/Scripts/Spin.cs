using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    private float speed = 0;

    void FixedUpdate()
    {
        transform.localEulerAngles += new Vector3(0, 0, speed * Time.fixedDeltaTime);
    }
}
