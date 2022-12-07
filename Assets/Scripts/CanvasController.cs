using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    [SerializeField, Header("ボタンにするテキストを設定")]
    private Text[] text = new Text[0];

    [SerializeField, Header("ボタンの配列の最大値（0スタート）")]
    private int _buttonNumMax = default;
    private int _buttonNum = default;
    private const int _buttonNum1 = 1;
    private const int _buttonNum2 = 2;

    [SerializeField, Header("現在選択しているボタン")]
    private Text textButton = default;

    // キーパットの入力状態
   　private bool _inputState = false;

    // ボタンを識別するタグリスト
    private string[] _fieldObjectTagList = {"Start", "Continue", "End" };

    // ボタンを一つ選択する
    // 選択中のボタンの色を変える
    private void Awake()
    {
        textButton = text[_buttonNum];
        textButton.color = Color.green;
    }

    private void Update()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");

        /* ボタンの上を押したら一つ上のボタンを選択
         * ボタンの下を押したら一つ下のボタンを選択
         * 一番上を選択中にボタンの上を押したら一番下を選択
         * 一番下を選択中にボタンの下を押したら一番上を選択
         * 選択中のボタンの色を変える
         * 選択されていないボタンの色をもとに戻す
         * 入力状態を設定
         */
        if (verticalInput > 0 && !_inputState)
        {
            textButton.color = new Color(0f, 0f, 0f, 1f);

            if (_buttonNum > 0)
            {
                --_buttonNum;
                textButton = text[_buttonNum];
                Debug.Log(_buttonNum);
            }
            else if (_buttonNum == 0)
            {
                _buttonNum = _buttonNumMax;
                textButton = text[_buttonNum];
            }

            if(textButton.color != Color.green)
            {
                textButton.color = Color.green;
            }
            _inputState = true;
        }
        else if (verticalInput < 0 && !_inputState)
        {
            textButton.color = new Color(0f, 0f, 0f, 1f);

            if (_buttonNum < _buttonNumMax)
            {
                ++_buttonNum;
                textButton = text[_buttonNum];
                Debug.Log(_buttonNum);
            }
            else if (_buttonNum == _buttonNumMax)
            {
                _buttonNum = 0;
                textButton = text[_buttonNum];
            }

            if (textButton.color != Color.green)
            {
                textButton.color = Color.green;
            }
            _inputState = true;
        }

        // ボタンを押した処理
        /* ゲームを開始するボタンを選択している場合はゲームスタート
         * ゲームを終了するボタンを選択している場合はゲームを終わる
         */
        if (Input.GetButtonDown("Jump"))
        {
            string _textTag = textButton.tag;
            if (_textTag == _fieldObjectTagList[0])
            {
                PlayerPrefs.SetInt("minSteps", 99);
                PlayerPrefs.Save();
                SceneManager.LoadScene("Stage1");
            }
            else if (_textTag == _fieldObjectTagList[_buttonNum1])
            {
                SceneManager.LoadScene("Stage1");
            }
            else if (_textTag == _fieldObjectTagList[_buttonNum2])
            {
                End();
            }

        }

        // 何も入力していなければ入力状態を解除
        if (verticalInput == 0)
        {
            _inputState = false;
        }
    }

    // ゲームを終了する処理
    public void End()
    {
#if UNITY_EDITOR
        //デバッグモードを終了
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        //アプリを終了
        Application.Quit();
#endif
    }
}
