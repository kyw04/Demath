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
    public float camera_speed = 1.5f;
    public float player_hp;
    public float enemy_hp;
    private string[] file;
    private float distance;
    private float maxOrthographicSize;
    private Camera cam;
    private const int LEN = 5;
    private void Awake()
    {
        if (manager == null) manager = this.GetComponent<GameManager>();
    }
    void Start()
    {
        for (int i = 0; i < expression.transform.childCount; i++)
        {
            expression.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
        }

        Debug.Log(PlayerPrefs.GetInt("Stage"));
        string file_path = "Assets/Text/Stage" + PlayerPrefs.GetInt("Stage") + ".txt";
        file = File.ReadAllLines(file_path);

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

        if (player_hp <= 0 || enemy_hp <= 0)
        {
            Time.timeScale = 0;
            StopCoroutine("enemy_obj_summon");
            if (player_hp <= 0)
            { 
                
            }
            else
            {

            }
        }

        player_castle.transform.GetChild(0).GetComponent<TextMesh>().text = player_hp + "/100";
        enemy_castle.transform.GetChild(0).GetComponent<TextMesh>().text = enemy_hp + "/100";
    }

    public void player_obj_summon(float result)
    {
        GameObject newExpression = Instantiate(obj[0],
                                               new Vector3(player_castle.transform.position.x, 1.5f, 1),
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
                                                  new Vector3(enemy_castle.transform.position.x, 1.5f, 1),
                                                  player_castle.transform.rotation);
            newExpression.transform.GetChild(0).GetComponent<TextMesh>().text = data[1];
            newExpression.GetComponent<Entity>().result = result;

            if (expression.transform.GetChild(0).GetChild(0).GetComponent<Text>().text == "" && queue.Count > 0)
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
    }
}
