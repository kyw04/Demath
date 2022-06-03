using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    public GameObject c;
    private float t;

    private Queue<int[]> q = new Queue<int[]>();
    void Start()
    {
        int[] arr = { 1, 2, 3, 4, 5 };
        q.Enqueue(arr);
        Debug.Log(q.Peek());
        int[] arr2 = q.Peek();
        Debug.Log(arr2[0]);
        int[] arr3 = q.Dequeue();
        Debug.Log(arr3[0]);

        if (a != null && b != null && c != null)
        {
            t = 0;
            c.transform.position = (1 - t) * a.transform.position + t * b.transform.position;
            StartCoroutine(start());
        }
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
