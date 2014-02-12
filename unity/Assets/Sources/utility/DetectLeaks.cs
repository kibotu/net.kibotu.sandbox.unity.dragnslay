using System.Linq;
using UnityEngine;
using System.Collections.Generic;

// http://wiki.unity3d.com/index.php/DetectLeaks
public class DetectLeaks : MonoBehaviour
{
    void OnGUI()
    {
        var objects = FindObjectsOfType(typeof(Object));

        var dictionary = new Dictionary<string, int>();

        foreach (var key in objects.Select(obj => obj.GetType().ToString()))
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key]++;
            }
            else
            {
                dictionary[key] = 1;
            }
        }

        var myList = new List<KeyValuePair<string, int>>(dictionary);
        myList.Sort((firstPair, nextPair) => nextPair.Value.CompareTo((firstPair.Value)));

        foreach (var entry in myList)
        {
            GUILayout.Label(entry.Key + ": " + entry.Value);
        }
    }
}