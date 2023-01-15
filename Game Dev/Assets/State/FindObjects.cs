using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class FindObjects
{
    public static List<GameObject> FindGameObjectsInScene(string objectTag)
    {
        List<GameObject> found = new List<GameObject>();
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.tag == objectTag)
            {
                found.Add(go);
            }
        }

        return found;
    }

    public static List<GameObject> GetAllObjectsOnlyInScene(string objectTag)
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (!(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
            {
                if (go.tag == objectTag)
                {
                    objectsInScene.Add(go);
                }
            }
        }

        return objectsInScene;
    }
}
