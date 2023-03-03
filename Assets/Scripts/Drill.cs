using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    [SerializeField]
    float move = 1f;
    [SerializeField]
    float speed = 1f;

    float startX;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    void OnEnable()
    {
        StartCoroutine(SwingSmoothCr());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    //IEnumerator SwingSmoothCr(float amount, float speed)
    IEnumerator SwingSmoothCr()
    {
        Vector3 originPos = transform.position;
        float f = 0;

        while (true)
        {
            f += Time.deltaTime;
            // cos�� x = 0�϶� y = 1�� => x = 90���� �����ؾ� y = 0
            float posX = originPos.x + Mathf.Cos((90 - f * speed) * Mathf.Deg2Rad) * move;
            transform.position = new Vector3(posX, originPos.y, originPos.z);
            yield return new WaitForEndOfFrame();
        }
    }
}
