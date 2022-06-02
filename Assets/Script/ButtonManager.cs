using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject back_message;
    public string go_back_scene_name;
    private bool _back;
    private void Start()
    {
        _back = false;
        if (back_message != null)
           back_message.SetActive(_back);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            _back = !_back;

            if (!_back)
                Time.timeScale = 1;

            if (back_message != null)
                back_message.SetActive(_back);
            else
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(go_back_scene_name);
            }
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
        Time.timeScale = 1;
        GameObject button = EventSystem.current.currentSelectedGameObject;
        if (button.name == "Yes_Button")
        {
            if (SceneManager.GetActiveScene().name == "Menu")
                Application.Quit();
            else
                SceneManager.LoadScene(go_back_scene_name);
        }
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
        Time.timeScale = 0;
        _back = !_back;
        if (back_message != null)
            back_message.SetActive(_back);
        else
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(go_back_scene_name);
        }
    }

    public void on_off_click()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        RectTransform button_child_rect = button.transform.GetChild(0).GetComponent<RectTransform>();
        string key_name = button.transform.parent.name;

        button_child_rect.anchoredPosition = -button_child_rect.anchoredPosition;

        if (PlayerPrefs.GetInt(key_name) == 1)
        {
            button.GetComponent<Image>().color = Color.gray;
            PlayerPrefs.SetInt(key_name, 0);
        }
        else
        {
            button.GetComponent<Image>().color = new Color32(0, 225, 0, 255);
            PlayerPrefs.SetInt(key_name, 1);
        }

        Debug.Log(key_name + ' ' + PlayerPrefs.GetInt(key_name));
    }
}
