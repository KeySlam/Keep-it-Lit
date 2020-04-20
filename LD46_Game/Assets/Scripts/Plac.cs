using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plac : MonoBehaviour
{
    [SerializeField]
    public float radius = 0;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
