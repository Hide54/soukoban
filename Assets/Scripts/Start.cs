using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    [SerializeField,Header("最初に選択されるボタンを設定")]
    private  Button button;


    /* ゴールまでの最小歩数を初期化
     * ボタンを選択
     */
    private void Awake()
    {
        button.Select();
        PlayerPrefs.SetInt("minSteps", 99);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt("minSteps"));
    }
    //ゲームを開始する処理
    public void Play()
    {
        SceneManager.LoadScene("Stage1");
    }

    //ゲームを終了する処理
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
