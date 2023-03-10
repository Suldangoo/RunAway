using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{ 
    ready = 0,
    playWait,
    play,
    pause,
    over,
    preClearWait, // 프로그래스 바 100% -> 다음 그라운드 패턴 = 피니시 그라운드 출현
    clearWait, // 클리어 포인트 도달
    clear
}

// 게임 전체 흐름(진행 상태) 제어
public class GameManager : MonoBehaviour
{    
    // 싱글톤
    public static GameManager Instance
    {
        get
        {            
            if (instance == null)
            {                
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    private static GameManager instance;

    RepeatGround repeatGround;
    ProgressBar progressBar;

    Drill drill;
    float drillMove = 7f;    

    [SerializeField]
    float gameSpeed = 1f; // 기본값 1
    float maxGameSpeed = 5f;
    public float GameSpeed => gameSpeed;

    [SerializeField]
    GameState state = GameState.ready;

    UiManager UiManager => UiManager.Instance;

    public bool IsPlaying => state == GameState.play ? true : false;
    public bool IsInCutScene => state == GameState.playWait ? true : false;
    public bool IsInPreClearWait => state == GameState.preClearWait ? true : false;

    void Start()
    {
        UiManager.SetPauseUi(false);
        UiManager.SetInGameUIs(false);
        UiManager.SetStartUi(true);                

        Time.timeScale = gameSpeed;

        drill = FindObjectOfType<Drill>();
        progressBar = FindObjectOfType<ProgressBar>();
        repeatGround = FindObjectOfType<RepeatGround>();
    }

    void Update()
    {
        if (state == GameState.play)
        {            
            if(gameSpeed < maxGameSpeed) gameSpeed += Time.deltaTime * 0.01f;
            else if(gameSpeed > maxGameSpeed)gameSpeed = maxGameSpeed;    
            
            if(progressBar.isFullProgress)
            {
                Debug.Log("state : preClearWait");                      
                state = GameState.preClearWait;
            }            
        }                
    }

    public void StartGame()
    {
        state = GameState.play;              
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

    public void HoldPlayerProgress(float duration)
    {
        progressBar.SetHoldTime(duration);
    }

    public void GameOver()
    {
        state = GameState.over;
        SetGameSpeed(0);
        UiManager.SetOverUi(true);
    }

    void GameClear()
    {
        //Debug.Log("GameClear");
        SetGameSpeed(0);
        UiManager.SetClearUi(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetGameSpeed(float f)
    {
        gameSpeed = f;
    }

    #region 컷신연출

    public void PlayStartCutScene()
    {
        if (state != GameState.ready)
        {
            Debug.Log("can play StartCutScene On ready state!");
            return;
        }

        UiManager.SetInGameUIs(true);
        UiManager.SetStartUi(false);

        float duration = 3f;
        state = GameState.playWait;
        StartCoroutine(StartCutSceneCr(duration));
    }

    IEnumerator StartCutSceneCr(float duration)
    {
        SetGameSpeed(2);        

        float t = 0;
        drill.moveDrill = false;
        while (true)
        {
            t += Time.fixedDeltaTime / duration;
            if (t > 1) t = 1;
            if (t == 1) break;

            drill.originPos -= Vector3.right * Time.fixedDeltaTime / duration * drillMove;            

            yield return new WaitForFixedUpdate();
        }

        //yield return new WaitForSeconds(duration);
        SetGameSpeed(1);

        StartGame();
    }

    public void PlayOverCutScene()
    {

    }

    public void PlayClearCutScene()
    {
        SetGameSpeed(0);

        float clearWait = 5f;
        StartCoroutine(ClearCutSceneCr(clearWait));
        Invoke(nameof(GameClear), clearWait);        
    }

    IEnumerator ClearCutSceneCr(float duaration)
    {
        float move = 0.25f;

        while (true)
        {
            repeatGround.Elevator.Translate(0, move * Time.deltaTime / duaration, 0);
            yield return null;
        }                
    }

    #endregion 컷신연출
}
