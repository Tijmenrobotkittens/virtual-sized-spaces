using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatter
{
    public Scatter(string prefab, GameObject parent, int nr, float ypos, float xrange, float zrange) {
        for (int i = 0; i < nr; i++) {
            GameObject ob = HelperFunctions.GetPrefab(prefab,parent);
            ob.transform.localPosition = new Vector3(Random.Range(-xrange, xrange), ypos, Random.Range(-zrange, zrange));
        }
    }
}
