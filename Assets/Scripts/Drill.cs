using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    [SerializeField]
    float move = 1f;
    [SerializeField]
    float speed = 1f;

    [SerializeField]
    public Vector3 originPos;

    [SerializeField]
    public bool moveDrill = false;


    void OnEnable()
    {
        //StartCoroutine(SwingSmoothCr());
    }
    
    void Start()
    {
        originPos = transform.position;
        moveDrill = true;
        StartCoroutine(SwingSmoothCr());
    }

    

    void OnDisable()
    {
        //StopAllCoroutines();        
    }

    //IEnumerator SwingSmoothCr(float amount, float speed)
    IEnumerator SwingSmoothCr()
    {        
        float f = 0;
        float offsetX = 0;
        while (true)
        {            
            if (moveDrill)
            {
                f += Time.deltaTime;
                // cos�� x = 0�϶� y = 1�� => x = 90���� �����ؾ� y = 0
                offsetX = Mathf.Cos((90 - f * speed) * Mathf.Deg2Rad) * move;                                
            }

            transform.position = new Vector3(originPos.x + offsetX, originPos.y, originPos.z);

            yield return null;
        }
    }
}
