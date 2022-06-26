using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public Queue<string> queue = new Queue<string>();
    public GameObject player_castle;
    public GameObject enemy_castle;
    public GameObject[] obj;
    public GameObject expression;
    public GameObject gameOverMessage;
    public Text timer;
    public float camera_speed = 1.5f;
    public float player_hp;
    public float enemy_hp;
    public bool buttonChange;
    private string[] file;
    private float distance;
    private float maxOrthographicSize;
    private float playTime;
    private bool check;
    private Camera cam;
    private const int LEN = 5;
    private void Awake()
    {
        if (manager == null) manager = this.GetComponent<GameManager>();
    }
    void Start()
    {
        buttonChange = false;
        check = false;
        playTime = 0;
        for (int i = 0; i < expression.transform.childCount; i++)
        {
            expression.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
        }

        Debug.Log(PlayerPrefs.GetInt("Stage"));
        TextAsset asset = Resources.Load <TextAsset>("Text/Stage" + PlayerPrefs.GetInt("Stage"));
        Debug.Log(asset);
        if (asset != null)
        {
            string str = asset.text;
            file = str.Split('\n');
        }
        else
            SceneManager.LoadScene("Stage");

        float enemy_pos_x;
        float.TryParse(file[0], out enemy_pos_x);
        enemy_castle.transform.position = new Vector3(enemy_pos_x, 3.3f, 0);

        for (int i = 1; i < file.Length; i++) // 배열을 랜덤하게 배치
        {
            int index1 = Random.Range(1, file.Length);
            int index2 = Random.Range(1, file.Length);

            string temp = file[index1];
            file[index1] = file[index2];
            file[index2] = temp;
        }
        //for (int i = 1; i < file.Length; i++)
        //    Debug.Log(file[i]);

        player_hp = 100;
        enemy_hp = 100;

        cam = Camera.main;
        player_castle.transform.position = new Vector3(-enemy_castle.transform.position.x, 3.5f, 0);

        distance = -enemy_castle.transform.position.x + player_castle.transform.position.x;
        Screen.SetResolution(1920, 1080, true);
        maxOrthographicSize = cam.orthographicSize = distance / 3;
        cam.transform.position = new Vector3(0, 3, -10);

        if (file.Length > 0)
            StartCoroutine("enemy_obj_summon");
        else
        {
            SceneManager.LoadScene("Stage");
        }
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("Zoom") == 1)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                float dif = touch.deltaPosition.x;
                if (dif > 200) dif = 200;
                else if (dif < -200) dif = -200;

                if (dif > 0 && enemy_castle.transform.position.x + cam.orthographicSize < cam.transform.position.x ||
                    dif < 0 && player_castle.transform.position.x - cam.orthographicSize > cam.transform.position.x)
                    cam.transform.position -= new Vector3(dif, 0, 0) * camera_speed * Time.deltaTime;
            }
            else if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float dif = prevTouchDeltaMag - TouchDeltaMag;
                if (dif > 100) dif = 100;
                else if (dif < -100) dif = -100;

                if (dif > 0 && cam.orthographicSize < maxOrthographicSize)
                {
                    cam.orthographicSize += dif * camera_speed * Time.deltaTime;
                    if (cam.transform.position.x >= 1)
                    {
                        cam.transform.position -= new Vector3(camera_speed, 0, 0) * dif * cam.transform.position.x * 0.5f * Time.deltaTime;
                    }
                    else if (cam.transform.position.x <= -1)
                    {
                        cam.transform.position -= new Vector3(camera_speed, 0, 0) * dif * cam.transform.position.x * 0.5f * Time.deltaTime;
                    }
                }
                else if (dif < 0 && cam.orthographicSize > 10)
                {
                    cam.orthographicSize += dif * camera_speed * Time.deltaTime;
                }
                cam.transform.position = new Vector3(cam.transform.position.x, 3, -10);
            }
            if (cam.orthographicSize >= maxOrthographicSize - 1 && cam.orthographicSize <= maxOrthographicSize + 1)
                cam.transform.position = new Vector3(0, 3, -10);
        }

        if ((player_hp <= 0 || enemy_hp <= 0) && !check)
        {
            check = true;
            Time.timeScale = 0;
            StopCoroutine("enemy_obj_summon");
            for (int i = 0; i < 3; i++)
                gameOverMessage.transform.GetChild(3).GetChild(i).gameObject.SetActive(false);
            gameOverMessage.SetActive(true);
            if (player_hp <= 0)
            {
                gameOverMessage.transform.GetChild(1).GetComponent<Text>().text = "패배";
            }
            else
            {
                Debug.Log(player_hp);
                Debug.Log(playTime);
                gameOverMessage.transform.GetChild(1).GetComponent<Text>().text = "승리";
                string[] starArray = PlayerPrefs.GetString("Star").Split(',');
                int starMax = 0;
                int.TryParse(starArray[PlayerPrefs.GetInt("Stage")], out starMax);
                int star = 1;

                if (player_hp >= 75)
                    star++;
                if (playTime <= 100)
                    star++;

                Debug.Log(star);
                for (int i = 0; i < star; i++)
                    gameOverMessage.transform.GetChild(3).GetChild(i).gameObject.SetActive(true);

                if (starMax < star)
                    starArray[PlayerPrefs.GetInt("Stage")] = star.ToString();

                if (PlayerPrefs.GetInt("Stage") + 1 > PlayerPrefs.GetInt("Clear"))
                    PlayerPrefs.SetInt("Clear", PlayerPrefs.GetInt("Stage") + 1);
                string t = "";
                for (int i = 0; i < starArray.Length; i++)
                {
                    t += starArray[i];
                    if (i != starArray.Length - 1)
                        t += ",";
                }
                PlayerPrefs.SetString("Star", t);
                Debug.Log(PlayerPrefs.GetString("Star"));
            }
        }
        else
        {
            player_hp = player_hp < 0 ? 0 : player_hp;
            enemy_hp = enemy_hp < 0 ? 0 : enemy_hp;
            player_castle.transform.GetChild(0).GetComponent<TextMesh>().text = player_hp + "/100";
            enemy_castle.transform.GetChild(0).GetComponent<TextMesh>().text = enemy_hp + "/100";
            playTime += Time.deltaTime;
            timer.text = "시간 : " + (int)playTime / 60 + "분 " + (int)playTime % 60 + "초";
        }
    }

    public void player_obj_summon(float result)
    {
        GameObject newExpression = Instantiate(obj[0],
                                               new Vector3(player_castle.transform.position.x - 0.75f, 1.5f, 1),
                                               player_castle.transform.rotation);
        newExpression.transform.GetChild(0).GetComponent<TextMesh>().text = result.ToString();
        newExpression.GetComponent<Entity>().result = result;
        //Debug.Log("summon : " + result);
    }

    IEnumerator enemy_obj_summon()
    {
        for (int i = 1; i < file.Length; i++)
        {
            queue.Enqueue(file[i]);
            string[] data = file[i].Split(' ');
            float time, result;
            float.TryParse(data[0], out time);
            float.TryParse(data[1], out result);
            //Debug.Log(time);
            yield return new WaitForSeconds(time);
            GameObject newExpression = Instantiate(obj[1],
                                                  new Vector3(enemy_castle.transform.position.x + 0.75f, 1.5f, 1),
                                                  player_castle.transform.rotation);
            newExpression.transform.GetChild(0).GetComponent<TextMesh>().text = data[1];
            newExpression.GetComponent<Entity>().result = result;

            if (expression.transform.GetChild(0).GetChild(0).GetComponent<Text>().text == "" && queue.Count > 0 && buttonChange == false)
            {
                string[] str = queue.Dequeue().Split(' ');
                for (int j = 0; j < LEN; j++)
                {
                    //Debug.Log(str[j + 2 + LEN]);
                    float temp;
                    float.TryParse(str[j + 2 + LEN], out temp);
                    expression.transform.GetChild(j).GetChild(0).GetComponent<Text>().text = str[j + 2];
                    expression.transform.GetChild(j).GetComponent<Result>().value = temp;
                }
            }
        }
        GameObject lastExpression = Instantiate(obj[1],
                                               new Vector3(enemy_castle.transform.position.x, 1.5f, 1),
                                               player_castle.transform.rotation);
        lastExpression.transform.GetChild(0).GetComponent<TextMesh>().text = "999999";
        lastExpression.GetComponent<Entity>().result = 999999;
    }
}
