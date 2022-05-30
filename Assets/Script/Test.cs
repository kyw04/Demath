using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    public GameObject c;
    private Vector3 vector;
    private float t;
    void Start()
    {
        t = 0;
        c.transform.position = (1 - t) * a.transform.position + t * b.transform.position;
        StartCoroutine(start());
    }

    void Update()
    {
        
    }
    IEnumerator start()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(c, c.transform.position, c.transform.rotation);
            t += 0.1f;
            c.transform.position = (1 - t) * a.transform.position + t * b.transform.position;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
