using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

// Pomocná tøída pro výpoèty (mùže být ve stejném souboru vnì hlavní tøídy)
public class Hrana
{
    public ConnectionPoint cp1, cp2;
    public float vaha;

    public Hrana(ConnectionPoint a, ConnectionPoint b)
    {
        cp1 = a;
        cp2 = b;
        vaha = Vector3.Distance(a.transform.position, b.transform.position);
    }
}