using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffButton : MonoBehaviour
{
    private void Start()
    {
        string key_name = transform.parent.name;

        if (PlayerPrefs.GetInt(key_name) == 0)
        {
            GetComponent<Image>().color = Color.gray;
            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 55);
        }
        else
        {
            GetComponent<Image>().color = new Color32(0, 225, 0, 255);
            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -55);
        }
    }
}
