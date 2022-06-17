using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    public static SceneChange change;
    public float speed;
    private Image changeImage;
    private IEnumerator coroutine;
    private void Awake()
    {
        coroutine = null;
        changeImage = transform.GetChild(0).GetComponent<Image>();

        var obj = FindObjectsOfType<SceneChange>();
        changeImage.gameObject.SetActive(false);
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            change = gameObject.GetComponent<SceneChange>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (coroutine != null)
            Time.timeScale = 1;
    }

    public IEnumerator CloseScene()
    {
        changeImage.gameObject.SetActive(true);
        changeImage.fillAmount = 0;
        while (changeImage.fillAmount < 1)
        {
            changeImage.fillAmount += speed;
            yield return new WaitForSeconds(0.0001f);
        }
    }
    public IEnumerator OpenScene()
    {
        changeImage.fillAmount = 1;
        while (changeImage.fillAmount > 0)
        {
            changeImage.fillAmount -= speed;
            yield return new WaitForSeconds(0.0001f);
        }
        changeImage.gameObject.SetActive(false);
    }

    public IEnumerator LoadScene(string sceneName)
    {
        yield return StartCoroutine("CloseScene");

        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(0.5f);
        
        yield return StartCoroutine("OpenScene");
        coroutine = null;
    }

    public void StartCoroutineOne(IEnumerator inputCoroutine)
    {
        if (coroutine == null)
        {
            Debug.Log("Start Coroutine");
            coroutine = inputCoroutine;
            StartCoroutine(inputCoroutine);
        }
        else Debug.Log("Error");
    }
}
