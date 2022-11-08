using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // フィールド操作クラスの定義
    FieldArrayData g_fieldArrayData;




    // ゲームの状態管理用構造体
    // START      : ゲーム開始前処理
    // STOP       : ゲーム停止状態
    // BLOCK_MOVE : ブロック移動処理
    // PLAYER     : プレイヤー操作処理
    // END        : ゲームオーバー処理a
    public enum GameState
    {
        START,
        STOP,
        BLOCK_MOVE,
        PLAYER,
        END,
    }



    // 現在のゲーム状態
    public GameState g_gameState = GameState.START;


    
    // ゲームの状態設定を行うメソッド
    // ゲームの状態は以下参照
    // START      : ゲーム開始前処理
    // STOP       : ゲーム停止状態
    // BLOCK_MOVE : ブロック移動処理
    // PLAYER     : プレイヤー操作処理
    // END        : ゲームオーバー処理
    void SetGameState(GameState gameState)
    {
        this.g_gameState = gameState;
    }



    // START      : ゲーム開始前処理
    // STOP       : ゲーム停止状態
    // BLOCK_MOVE : ブロック移動処理
    // PLAYER     : プレイヤー操作処理
    // END        : ゲームオーバー処理
    GameState GetGameState()
    {
        return this.g_gameState;
    }
    // キーパットの入力状態
    bool g_inputState = false;


    



    private void Awake()
    {
        g_fieldArrayData = this.GetComponent<FieldArrayData>();
    }

    private void Update()
    {
        // ゲーム状態によって処理を分ける
        switch (g_gameState)
        {
            case GameState.START:
                SetGameState(GameState.PLAYER);
                break;
            case GameState.STOP:
                break;
            case GameState.PLAYER:
                float horizontalInput = Input.GetAxisRaw("Horizontal");
                float verticalInput = Input.GetAxisRaw("Vertical");
                // 横入力が0より大きい場合は右に移動
                if (horizontalInput > 0 && !g_inputState)
                {
                    g_fieldArrayData.PlayerMove(
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.x),
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.y),
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.x),
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.y + 1));
                    g_inputState = true;
                }
                // 横入力が0より小さい場合は左に移動
                else if (horizontalInput < 0 && !g_inputState)
                {
                    g_fieldArrayData.PlayerMove(
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.x),
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.y),
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.x),
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.y - 1));
                    g_inputState = true;
                }
                // 縦入力が0より大きい場合は上に移動
                if (verticalInput > 0 && !g_inputState)
                {
                    g_fieldArrayData.PlayerMove(
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.x),
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.y),
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.x - 1),
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.y));
                    g_inputState = true;
                }
                // 縦入力が0より小さい場合は下に移動
                else if (verticalInput < 0 && !g_inputState)
                {
                    g_fieldArrayData.PlayerMove(
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.x),
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.y),
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.x + 1),
                    Mathf.FloorToInt(g_fieldArrayData.PlayerPosition.y));
                    g_inputState = true;
                }
                // 入力状態が解除されるまで再入力できないようにする
                if ((horizontalInput + verticalInput) == 0)
                {
                    g_inputState = false;
                }
                //初期化
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    string sceneIndex = SceneManager.GetActiveScene().path;
                    SceneManager.LoadScene(sceneIndex);
                }
                // クリア判定
                if (g_fieldArrayData.GetGameClearJudgment())
                {
                    SetGameState(GameState.END);
                }
                break;
            case GameState.BLOCK_MOVE:
                break;

            case GameState.END:
                //ゴールまでの最小歩数を保存
                PlayerPrefs.SetInt("minSteps", g_fieldArrayData.stepCountMin);
                PlayerPrefs.Save();
                SceneManager.LoadScene("Clear");
                break;
        }
    }
}
