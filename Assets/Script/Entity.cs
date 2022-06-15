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
        if (this.gameObject.layer == 9)
            move = false;
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
                collision.transform.GetChild(0).GetComponent<TextMesh>().text = "";
                collision.gameObject.layer = 9;
                collision.GetComponent<ParticleSystem>().Play();
                Destroy(collision.gameObject, 1);
            }
            else if (result < collisionEntity.result)
            {
                collisionEntity.result -= result;
                collision.transform.GetChild(0).GetComponent<TextMesh>().text = collisionEntity.result.ToString();
                transform.GetChild(0).GetComponent<TextMesh>().text = "";
                this.gameObject.layer = 9;
                this.GetComponent<ParticleSystem>().Play();
                Destroy(gameObject, 1);
            }
            else
            {
                collision.gameObject.layer = 9;
                this.gameObject.layer = 9;
                transform.GetChild(0).GetComponent<TextMesh>().text = "";
                collision.transform.GetChild(0).GetComponent<TextMesh>().text = "";
                collision.GetComponent<ParticleSystem>().Play();
                this.GetComponent<ParticleSystem>().Play();
                Destroy(collision.gameObject, 1);
                Destroy(gameObject, 1);
                move = true;
            }
        }

        if (collision.CompareTag("Castle"))
        {
            if (gameObject.layer == 8)
                GameManager.manager.enemy_hp -= result;
            else
                GameManager.manager.player_hp -= result;
            gameObject.layer = 9;
            GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).GetComponent<TextMesh>().text = "";
            Destroy(gameObject, 1);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(gameObject.tag))
            move = true;
    }
}
