using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public float moveTime = 2;
    public Vector3 scaleChange = new Vector3(0, 0, 0.5f);
    float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = moveTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.localScale += scaleChange;

        currentTime -= Time.deltaTime;
        if(currentTime <= 0)
        {
            scaleChange *= -1;
            currentTime = moveTime;
        }
    }
}
