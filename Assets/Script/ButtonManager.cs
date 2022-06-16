using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private const int LEN = 5;
    private bool _back;
    public GameObject back_message;
    public string go_back_scene_name;
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
                SceneChange.change.StartCoroutine("LoadScene", go_back_scene_name);
            }
        }
    }
    public void start_click()
    {
        SceneChange.change.StartCoroutine("LoadScene", "Stage");
    }
    public void setting_click()
    {
        SceneChange.change.StartCoroutine("LoadScene", "Setting");
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
                SceneChange.change.StartCoroutine("LoadScene", go_back_scene_name);
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
        SceneChange.change.StartCoroutine("LoadScene", "Game");
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
            SceneChange.change.StartCoroutine("LoadScene", go_back_scene_name);
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
    public void summon_click()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        Text txt = button.transform.GetChild(0).GetComponent<Text>();
        float value = button.GetComponent<Result>().value;
        if (txt.text != "")
        {
            Transform expression = GameManager.manager.expression.transform;
            GameManager.manager.player_obj_summon(value);
            for (int i = 0; i < LEN; i++)
                expression.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
            Invoke("change_button", 1);
        }
    }
    private void change_button()
    {
        Transform expression = GameManager.manager.expression.transform;
        string[] data = { };
        if (GameManager.manager.queue.Count > 0)
        {
            data = GameManager.manager.queue.Dequeue().Split(' ');

            for (int i = 0; i < LEN; i++)
            {
                float temp;
                float.TryParse(data[i + 2 + LEN], out temp);
                expression.GetChild(i).GetChild(0).GetComponent<Text>().text = data[i + 2];
                expression.GetChild(i).GetComponent<Result>().value = temp;
                //Debug.Log(i + 2 + LEN + " button : " + temp);
            }
        }
    }

}
