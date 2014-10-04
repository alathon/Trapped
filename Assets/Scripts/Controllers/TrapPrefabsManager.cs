using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrapPrefabsManager : MonoBehaviour {
    [SerializeField]
    public List<GameObject> prefabs;

    private Dictionary<string, GameObject> trapByName = new Dictionary<string, GameObject>();

	void Awake () {
        foreach (GameObject gObj in prefabs)
        {
            trapByName.Add(gObj.GetComponent<TrapMetadata>().trapName, gObj);
        }
	}

    public string GetTrapNameByObj(GameObject obj)
    {
        foreach (KeyValuePair<string, GameObject> entry in trapByName)
        {
            if (entry.Value == obj) return entry.Key;
        }
        return null;
    }

    public GameObject GetTrapObjByName(string name)
    {
        GameObject retVal = null;
        trapByName.TryGetValue(name, out retVal);
        return retVal;
    }
}
