using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public GameObject player_castle;
    public GameObject enemy_castle;
    public GameObject[] obj;
    public GameObject expression;
    public float camera_speed = 1.5f;
    public float player_hp;
    public float enemy_hp;
    private float distance;
    private float maxOrthographicSize;
    private Camera cam;

    private void Awake()
    {
        if (manager == null) manager = this.GetComponent<GameManager>();
    }
    void Start()
    {
        player_hp = 100;
        enemy_hp = 100;
        cam = Camera.main;
        player_castle.transform.position = new Vector3(-enemy_castle.transform.position.x, 3.5f, 0);

        distance = -enemy_castle.transform.position.x + player_castle.transform.position.x;
        maxOrthographicSize = cam.orthographicSize = distance / 3;
        cam.transform.position = new Vector3(0, 3, -10);
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

        player_castle.transform.GetChild(0).GetComponent<TextMesh>().text = player_hp + "/100";
        enemy_castle.transform.GetChild(0).GetComponent<TextMesh>().text = enemy_hp + "/100";
    }

    public void player_obj_summon(string text, int index)
    {
        GameObject newExpression = Instantiate(obj[index],
                                               new Vector3(player_castle.transform.position.x, 1.5f, player_castle.transform.position.z),
                                               player_castle.transform.rotation);
        newExpression.transform.GetChild(0).GetComponent<TextMesh>().text = text;
    }
}
