using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    [SerializeField,Header("最初に選択されるボタンを設定")]
    private  Button button;

    //ゲームを開始する処理
    public void Play()
    {
        SceneManager.LoadScene("Stage1");
    }

    //ゲームを終了する処理
    public void End()
    {
        //デバッグモードを終了
        UnityEditor.EditorApplication.isPlaying = false;
        //アプリを終了
        Application.Quit();
    }

    /* ゴールまでの最小歩数を初期化
     * ボタンを選択
     */
    private void Awake()
    {
        button.Select();
        PlayerPrefs.SetInt("minSteps", 99);
        PlayerPrefs.Save();
    }
}
