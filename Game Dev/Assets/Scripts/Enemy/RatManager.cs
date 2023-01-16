using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatManager : MonoBehaviour, IDataManager
{
    void Awake()
    {
        List<GameObject> loadedRats = FindObjects.GetAllObjectsOnlyInScene("Rat");
        foreach(GameObject go in loadedRats)
        {
            if (go.GetComponent<Rat>() != null && go.GetComponent<Rat>().id == "")
            {
                // Coin was found in coin manager => picked up
                go.GetComponent<Rat>().GenerateID();
                continue;
            }
        }
    }

    public void LoadData(GameData data)
    {
        Vector3 helperV = new Vector3(-1000, -1000, -1000);
        if (Vector3.Distance(data.playerData.position, helperV) > 0)
        {
            List<GameObject> loadedRats = FindObjects.GetAllObjectsOnlyInScene("Rat");
            foreach(GameObject go in loadedRats)
            {
                if (go.GetComponent<Rat>() != null)
                {
                    string id = data.RatIds.Find(x => x == go.GetComponent<Rat>().id);
                    if (id == null)
                    {
                        // Coin was found in coin manager => picked up
                        go.SetActive(false);
                        continue;
                    } else {
                        go.transform.position = data.RatPositions[data.RatIds.FindIndex(x => x == go.GetComponent<Rat>().id)];
                    }
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        List<GameObject> loadedRats = FindObjects.GetAllObjectsOnlyInScene("Rat");
        List<string> RatIds = new List<string>();
        List<Vector3> RatPositions = new List<Vector3>();

        // See what coins were consumed so we won't save them
        foreach(GameObject go in loadedRats)
        {
            if (go.GetComponent<Rat>() != null && go.activeInHierarchy)
            {
                // Coin is active, add to list
                RatIds.Add(go.GetComponent<Rat>().id);
                RatPositions.Add(go.transform.position);
            }
        }

        data.RatIds = RatIds;
        data.RatPositions = RatPositions;
    }
}
