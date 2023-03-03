using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ui ���� ��ɸ��� �����ϴ� ���� Ŭ����
public class UiManager : MonoBehaviour
{
    //�̱���
    public static UiManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UiManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return instance;
        }
    }
    private static UiManager instance;

    [SerializeField]
    GameObject startUI;

    [SerializeField]
    GameObject puaseUI;

    [SerializeField]
    Text rightBtnTxt;

    [SerializeField]
    GameObject[] inGameUis;

    public void SetStartUi(bool active)
    {
        startUI.SetActive(active);
    }

    public void SetPauseUi(bool active)
    {
        puaseUI.SetActive(active);
    }

    public void SetInGameUIs(bool active)
    {
        foreach (GameObject go in inGameUis) { go.SetActive(active); }
    }

    public void SetRightBtnTxt(string str)
    {
        rightBtnTxt.text = str;
    }
}
