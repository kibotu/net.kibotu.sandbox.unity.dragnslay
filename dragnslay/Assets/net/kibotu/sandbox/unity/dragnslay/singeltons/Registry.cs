
using System.Collections.Generic;
using UnityEngine;

class Registry : MonoBehaviour
{
    public Dictionary<int, Orb> Orbs;

    private static Registry _instance;

    public static Registry Instance
    {
        get { return _instance ?? (_instance = new GameObject("Registry").AddComponent<Registry>()); }
    }
}