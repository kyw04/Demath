using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    static public GameObject[] stages;
    string[] stars;
    void Start()
    {
        for (int i = 0; i < stages.Length; i++)
            stages[i].SetActive(false);
        for (int i = 0; i < stages.Length && i < PlayerPrefs.GetInt("Clear") + 1; i++)
            stages[i].SetActive(true);
        string t = PlayerPrefs.GetString("Star");
        stars = t.Split(',');

    }
}
