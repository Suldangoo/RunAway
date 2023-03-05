using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{ 
    ready = 0,
    play,
    pause,
    over,
    clear
}

// ���� ��ü �帧 ����
// 1. ������ ���� (���� ��, �÷��� ��, �Ͻ����� ��) ����
// 2. ������ �ӵ� ����
public class GameManager : MonoBehaviour
{    
    //�̱���
    public static GameManager Instance
    {
        get
        {            
            if (instance == null)
            {                
                instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return instance;
        }
    }
    private static GameManager instance;

    UiManager UiManager => UiManager.Instance;

    public bool IsPlaying => state == GameState.play ? true : false;

    [SerializeField]
    float gameSpeed = 1f;

    [SerializeField]
    GameState state = GameState.ready;
    
    void Start()
    {
        UiManager.SetPauseUi(false);
        UiManager.SetInGameUIs(false);
        UiManager.SetStartUi(true);                

        Time.timeScale = gameSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = gameSpeed;
    }  

    public void StartGame()
    {
        state = GameState.play;

        UiManager.SetInGameUIs(true);
        UiManager.SetStartUi(false);
    }

    public void TogglePause()
    {
        if (state == GameState.play)
        {            
            state = GameState.pause;
            UiManager.SetPauseUi(true);
            Time.timeScale = 0;


        }
        else if (state == GameState.pause)
        {
            state = GameState.play;
            UiManager.SetPauseUi(false);
            Time.timeScale = gameSpeed;
        }
        else
        {
            Debug.Log("cannot toggle pause! state :" + state);
        }

        return;
    }
}
