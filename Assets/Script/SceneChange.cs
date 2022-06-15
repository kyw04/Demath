using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    public static SceneChange change;
    private Image chageImage;
    private void Awake()
    {
        chageImage = transform.GetChild(0).GetComponent<Image>();

        var obj = FindObjectsOfType<SceneChange>();
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
        chageImage.fillAmount = 0;
        while (chageImage.fillAmount < 1)
        {
            chageImage.fillAmount += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public IEnumerator OpenScene()
    {
        chageImage.fillAmount = 1;
        while (chageImage.fillAmount > 0)
        {
            chageImage.fillAmount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator LoadScene(string sceneName)
    {
        while (chageImage.fillAmount < 1)
        {
            chageImage.fillAmount += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(0.25f);
        
        while (chageImage.fillAmount > 0)
        {
            chageImage.fillAmount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
