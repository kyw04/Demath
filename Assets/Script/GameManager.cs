using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player_castle;
    public GameObject enemy_castle;
    public Text test;
    public float camera_speed = 0.25f;
    private float distance;
    private float maxOrthographicSize;
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
        player_castle.transform.position = new Vector3(-enemy_castle.transform.position.x, 1.5f, 0);

        distance = -enemy_castle.transform.position.x + player_castle.transform.position.x;
        maxOrthographicSize = cam.orthographicSize = distance / 3;
        cam.transform.position = new Vector3(0, distance / 3 - 5, -10);
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            cam.transform.position += new Vector3(touch.deltaPosition.x, distance / 3 - 5, -10);
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

            if (dif > 0 && cam.orthographicSize > 5)
            {
                cam.orthographicSize -= dif * camera_speed * Time.deltaTime;
            }
            else if (dif < 0 && cam.orthographicSize < maxOrthographicSize)
            {
                cam.orthographicSize -= dif * camera_speed * Time.deltaTime;
            }
            cam.transform.position = new Vector3(0, distance / 3 - 5, -10);
        }
    }
}
