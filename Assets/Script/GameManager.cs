using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player_castle;
    public GameObject enemy_castle;
    public float camera_speed = 1.5f;
    private float distance;
    private float maxOrthographicSize;
    private Camera cam;
    void Start()
    {
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
    }
}
