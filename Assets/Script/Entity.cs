using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum Type { Player = 1, Enemy = -1, None = 0 }
    public Type type;
    public float speed;
    public float result;
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
        if (!collision.CompareTag(gameObject.tag))
            move = false;
        if (collision.CompareTag("Castle"))
        {
            if (gameObject.CompareTag("Player"))
                GameManager.manager.enemy_hp -= result;
            else if (gameObject.CompareTag("Enemy"))
                GameManager.manager.player_hp -= result;
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(gameObject.tag))
            move = true;
    }
}
