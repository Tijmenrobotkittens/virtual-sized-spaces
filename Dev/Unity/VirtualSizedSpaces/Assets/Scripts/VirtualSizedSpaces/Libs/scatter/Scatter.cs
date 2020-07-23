using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatter:MonoBehaviour
{
    List<GameObject> gameObjects = new List<GameObject>();

    private void ClearGameObjects() {
        foreach (GameObject game in gameObjects) {
            Destroy(game);
        }
        gameObjects.Clear();
    }

    public void Make(string prefab, GameObject parent, int nr, float ypos, float xrange, float zrange) {
        ClearGameObjects();


        for (int i = 0; i < nr; i++) {
            GameObject ob = HelperFunctions.GetPrefab(prefab,parent);
            gameObjects.Add(ob);
            ob.transform.localPosition = new Vector3(Random.Range(-xrange, xrange), ypos, Random.Range(-zrange, zrange));
        }
    }
}
