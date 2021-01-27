using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float TimeAlive = 10;

    // Update is called once per frame
    void Update()
    {
        TimeAlive -= Time.deltaTime;
        if (TimeAlive <= 0) Destroy(transform.gameObject);
    }
}
