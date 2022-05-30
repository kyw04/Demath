using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    public GameObject back_message;
    public string go_back_scene_name;
    private bool _back;
    public int stage_len;
    private void Start()
    {
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
        Debug.Log(PlayerPrefs.GetString("Star"));
        _back = false;
        if (back_message != null)
           back_message.SetActive(_back);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _back = !_back;
            if (back_message != null)
                back_message.SetActive(_back);
            else
                SceneManager.LoadScene(go_back_scene_name);
        }
    }
    public void start_click()
    {
        SceneManager.LoadScene("Stage");
    }
    public void setting_click()
    {
        SceneManager.LoadScene("Setting");
    }

    public void quit_click()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        if (button.name == "Yes_Button") Application.Quit();
        else
        {
            _back = !_back;
            back_message.SetActive(_back);
        }
    }
    public void stage_click()
    {
        GameObject stage_button = EventSystem.current.currentSelectedGameObject;
        StageIndex index = stage_button.GetComponent<StageIndex>();
        PlayerPrefs.SetInt("Stage", index.value);
        SceneManager.LoadScene("Game");
    }

    public void back_click()
    {
        _back = !_back;
        if (back_message != null)
            back_message.SetActive(_back);
        else
            SceneManager.LoadScene(go_back_scene_name);
    }
}
