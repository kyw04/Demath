using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    private StageIndex index;
    private string[] stars;
    private Image[] children_star;
    void Start()
    {
        index = GetComponent<StageIndex>();
        this.gameObject.SetActive(false);
        if (index.value < PlayerPrefs.GetInt("Clear") + 1)
            this.gameObject.SetActive(true);
        string t = PlayerPrefs.GetString("Star");
        stars = t.Split(',');

        if (index.value == PlayerPrefs.GetInt("Clear"))
            transform.GetChild(1).gameObject.SetActive(false);
        else
            transform.GetChild(1).gameObject.SetActive(true);

        children_star = transform.GetChild(2).GetComponentsInChildren<Image>();

        for (int i = 0; i < children_star.Length; i++)
            children_star[i].enabled = false;
        for (int i = 0; i < int.Parse(stars[index.value]); i++)
            children_star[i].enabled = true;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
