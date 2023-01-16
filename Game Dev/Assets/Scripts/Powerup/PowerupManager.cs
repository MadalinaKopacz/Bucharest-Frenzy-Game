using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour, IDataManager
{
    private List<GameObject> scenePowerups;
    private List<GameObject> sceneDowngrades;
    private List<string> powerupIds;

    void Awake()
    {
        powerupIds = new List<string>();
        scenePowerups = FindObjects.GetAllObjectsOnlyInScene("Powerup");

        foreach(GameObject go in scenePowerups)
        {
            if (go.GetComponent<Powerup>().id == "")
            {
                go.GetComponent<Powerup>().GenerateID();
            }
            powerupIds.Add(go.GetComponent<Powerup>().id);
        }
    }

    public void LoadData(GameData data)
    {
        scenePowerups = FindObjects.GetAllObjectsOnlyInScene("Powerup");
        foreach(GameObject go in scenePowerups)
        {
            string powerupId = data.PowerupManager.Find(x => x == go.GetComponent<Powerup>().id);
            if (powerupId == null)
            {
                // powerup was used
                go.SetActive(false);
                continue;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        // Save the list of powerup ids available
        List<string> savedpowerups = new List<string>();

        foreach(string id in powerupIds)
        {
            foreach(GameObject go in scenePowerups)
            {
                if (go != null)
                {
                    if (go.GetComponent<Powerup>().id == id)
                    {
                        // The powerup we are looking for
                        savedpowerups.Add(id);
                    }
                }
            }
        }

        data.PowerupManager = savedpowerups;
    }

}
