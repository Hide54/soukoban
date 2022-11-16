using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField,Header("最初に選択されるボタンを設定")]
    private Button button;

    [SerializeField, Header("最小歩数を入れるテキストを設定")]
    private Text text_Step_min;

    private void Awake()
    {
        //ボタンを選択
        button.Select();
        //今までで最小の歩数を表示
        text_Step_min.text = string.Format("{0}", PlayerPrefs.GetInt("minSteps"));
    }

    
    public void Retry()
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
