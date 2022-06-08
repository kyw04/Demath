using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Entity collisionEntity = collision.GetComponent<Entity>();
        if (!collision.CompareTag(gameObject.tag))
            move = false;
        if (collision.CompareTag("Result") && gameObject.layer == 8)
        {
            if (result > collisionEntity.result)
            {
                result -= collisionEntity.result;
                transform.GetChild(0).GetComponent<TextMesh>().text = result.ToString();
                Destroy(collision.gameObject);
            }
            else if (result < collisionEntity.result)
            {
                collisionEntity.result -= result;
                collision.transform.GetChild(0).GetComponent<TextMesh>().text = collisionEntity.result.ToString();
                Destroy(gameObject);
            }
            else
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("Castle"))
        {
            if (gameObject.layer == 8)
                GameManager.manager.enemy_hp -= result;
            else
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
