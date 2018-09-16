using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class LocalisationData
{
    public LocalisationItem[] items;
}

[System.Serializable]
public class LocalisationItem
{
    public string key;
    public string value;
}

public class LocalisationManager : MonoBehaviour {

    private List<string> keys = new List<string>();

	// Use this for initialization
	void Start () {
		
	}

    public static LocalisationManager instance;

    private Dictionary<string, string> localisedText;
    private bool isReady = false;
    private string missingTextString = "Localised text not found";

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadLocalisedText(string fileName)
    {
        localisedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalisationData loadedData = JsonUtility.FromJson<LocalisationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localisedText.Add(loadedData.items[i].key, loadedData.items[i].value);
                keys.Add(loadedData.items[i].key);
            }


            Debug.Log("Data loaded, dictionary contains: " + localisedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        isReady = true;
    }

    public string GetLocalisedValue(string key)
    {
        string result = missingTextString;
        if (localisedText.ContainsKey(key))
        {
            result = localisedText[key];
        }

        return result;

    }

    public string GetRandomValue()
    {
        string result = missingTextString;
        Random rand = new Random();
        result = localisedText[keys[Random.Range(0, keys.Count)]];
        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }
}
