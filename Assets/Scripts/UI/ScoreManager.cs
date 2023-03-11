using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // ?���??��
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreManager>();
            }
            return instance;
        }
    }
    private static ScoreManager instance;

    [SerializeField]
    int score = 0;

    [SerializeField]
    Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        SetScoreText(score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetScoreText(int i)
    {
        // ���ڸ� ���ڿ��� ��ȯ
        string str = i.ToString();

        // ���� �ڸ����� 1~3�ڸ��� �տ� '0,00' ~ '0,' �� �ٿ��ֱ�
        if (i < 1000)
        {
            str = "0," + str.PadLeft(3, '0');
        }
        // ���� �ڸ����� 4�ڸ��� ',' ������ ����
        else if (i < 10000)
        {
            str = string.Format("{0:#,}", i);
        }
        // ���� �ڸ����� 5�ڸ� �̻��̸� '9,999+' �� ���
        else
        {
            str = "9,999+";
        }

        // ���� ǥ�� ������Ʈ
        scoreText.text = str;
    }

    public void AddScore(int amount)
    {
        score += amount;
        SetScoreText(score);
    }
}
