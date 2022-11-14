using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    [SerializeField,Header("最初に選択されるボタンを設定")]
    private Button button;

    //ゲームを開始する処理
    public void Play()
    {
        SceneManager.LoadScene("Stage1");
    }

    //ゲームを終了する処理
    public void End()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    /* ゴールまでの最小歩数を初期化
     * 
     */
    private void Awake()
    {
        PlayerPrefs.SetInt("min_Steps", 99);
        PlayerPrefs.Save();
        button.Select();
    }
}
