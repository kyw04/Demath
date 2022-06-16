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
    private void Awake()
    {
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

    public IEnumerator CloseScene()
    {
        changeImage.fillAmount = 0;
        while (changeImage.fillAmount < 1)
        {
            changeImage.fillAmount += speed;
            yield return new WaitForSeconds(0.0001f);
        }
        changeImage.gameObject.SetActive(false);
    }
    public IEnumerator OpenScene()
    {
        changeImage.gameObject.SetActive(true);
        changeImage.fillAmount = 1;
        while (changeImage.fillAmount > 0)
        {
            changeImage.fillAmount -= speed;
            yield return new WaitForSeconds(0.0001f);
        }
    }

    public IEnumerator LoadScene(string sceneName)
    {
        changeImage.gameObject.SetActive(true);
        while (changeImage.fillAmount < 1)
        {
            changeImage.fillAmount += speed;
            yield return new WaitForSeconds(0.0001f);
        }

        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(0.5f);
        
        while (changeImage.fillAmount > 0)
        {
            changeImage.fillAmount -= speed;
            yield return new WaitForSeconds(0.0001f);
        }
        changeImage.gameObject.SetActive(false);
    }
}
