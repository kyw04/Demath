using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour
{
    public int stage_len;

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);

        if (!PlayerPrefs.HasKey("Clear"))
            PlayerPrefs.SetInt("Clear", 0);

        if (!PlayerPrefs.HasKey("Star"))
        {
            string t = "";
            for (int i = 0; i < stage_len; i++)
            {
                t += "0";
                if (i != stage_len - 1)
                    t += ",";
            }
            PlayerPrefs.SetString("Star", t);
        }
        else
        {
            string[] arr = PlayerPrefs.GetString("Star").Split(',');
            if (arr.Length > stage_len)
            {
                string t = "";
                for (int i = 0; i < stage_len; i++)
                {
                    if (arr[i] != null)
                        t += arr[i];
                    else
                        t += "0";

                    if (i != stage_len - 1)
                        t += ",";
                }
            }
        }
        Debug.Log(PlayerPrefs.GetString("Star"));

        if (!PlayerPrefs.HasKey("Zoom"))
            PlayerPrefs.SetInt("Zoom", 1);
        if (!PlayerPrefs.HasKey("BackgroundSound"))
            PlayerPrefs.SetInt("BackgroundSound", 1);
        if (!PlayerPrefs.HasKey("EffectSound"))
            PlayerPrefs.SetInt("EffectSound", 1);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
            Awake();
        }
    }
}
