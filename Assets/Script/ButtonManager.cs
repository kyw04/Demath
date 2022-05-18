using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    public GameObject quit_message;
    private bool _quit;
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
        _quit = false;
        if (quit_message != null)
           quit_message.SetActive(_quit);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _quit = !_quit;
            if (quit_message != null)
                quit_message.SetActive(_quit);
            else
                SceneManager.LoadScene(0);
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
        if (button.name == "Yes_Button")
        {
            Application.Quit();
        }
        else
        {
            _quit = !_quit;
            quit_message.SetActive(_quit);
        }
    }
    public void stage_click()
    {
        GameObject stage_button = EventSystem.current.currentSelectedGameObject;
        ButtonIndex index = stage_button.GetComponent<ButtonIndex>();
        SceneManager.LoadScene(stage_button.name);
    }
}
