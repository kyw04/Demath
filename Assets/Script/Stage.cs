using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    private StageIndex index;
    private string[] stars;
    private Image[] children_star;
    private int n;
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

        n = 0;
        if (stars.Length > index.value)
            int.TryParse(stars[index.value], out n);
        for (int i = 0; i < n; i++)
            children_star[i].enabled = true;
    }
}
