using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSelector : MonoBehaviour
{
    [SerializeField]
    private List<Plac> places = null;

    public Plac GetPlace()
    {
        return places[Random.Range(0, places.Count)];
    }
}
