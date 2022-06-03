using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum Type { Player = 1, Enemy = -1, None = 0 }
    public Type type;
    public float speed;
    private bool move = true;
    void Start()
    {
        
    }

    void Update()
    {
        if (move)
            transform.position -= transform.right * speed * (int)type * Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != gameObject.tag)
            move = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != gameObject.tag)
            move = true;
    }
}
