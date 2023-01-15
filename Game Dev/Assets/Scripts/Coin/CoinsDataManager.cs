using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsDataManager : MonoBehaviour, IDataManager
{
    void Awake()
    {
        List<GameObject> loadedSceneCoins = FindObjects.GetAllObjectsOnlyInScene("Coin");
        foreach(GameObject go in loadedSceneCoins)
        {
            if (go.GetComponent<Coin>().id == "")
            {
                // Coin was found in coin manager => picked up
                go.GetComponent<Coin>().GenerateID();
                continue;
            }
        }
    }

    public void LoadData(GameData data)
    {
        List<GameObject> loadedSceneCoins = FindObjects.GetAllObjectsOnlyInScene("Coin");

        // See what coins were consumed so we won't put them in the scene
        foreach(GameObject go in loadedSceneCoins)
        {
            string coinId = data.CoinManager.Find(x => x == go.GetComponent<Coin>().id);
            if (coinId == null)
            {
                // Coin was found in coin manager => picked up
                go.SetActive(false);
                continue;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        List<GameObject> loadedSceneCoins = FindObjects.GetAllObjectsOnlyInScene("Coin");
        List<string> coinIds = new List<string>();

        // See what coins were consumed so we won't save them
        foreach(GameObject go in loadedSceneCoins)
        {
            if (go.activeInHierarchy)
            {
                // Coin is active, add to list
                coinIds.Add(go.GetComponent<Coin>().id);
            }
        }

        Debug.Log("called");
        data.CoinManager = coinIds;
    }
}
