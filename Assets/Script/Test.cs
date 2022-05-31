using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    public GameObject c;
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
        while (true)
        {
            GameObject newC = Instantiate(c, c.transform.position, c.transform.rotation);
            Destroy(newC, 1);
            t = (t + 0.01f) % 1;
            c.transform.position = (1 - t) * a.transform.position + t * b.transform.position;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
