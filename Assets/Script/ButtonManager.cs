using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    public GameObject quit_message;
    private bool _quit;
    private void Start()
    {
        _quit = false;
        quit_message.SetActive(_quit);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _quit = !_quit;
            quit_message.SetActive(_quit);
        }
    }
    public void start_click()
    {
        SceneManager.LoadScene("Game");
    }
    public void setting_click()
    {
        SceneManager.LoadScene("Setting");
    }

    public void quit_click()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        if (button.name == "Yes_Button")
            Application.Quit();
        else
        {
            _quit = !_quit;
            quit_message.SetActive(_quit);
        }
    }
}
