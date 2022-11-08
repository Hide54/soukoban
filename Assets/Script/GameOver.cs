using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField,Header("最初に選択されるボタンを設定")]
    private Button b_button;

    [SerializeField, Header("最小歩数を入れるテキストを設定")]
    Text text_Step_min;

    int minSteps;

    public void Retry()
    {
        SceneManager.LoadScene("Stage1");
    }
    public void End()
    {
        PlayerPrefs.SetInt("min_Steps", 99);
        PlayerPrefs.Save();
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    private void Awake()
    {
        b_button.Select();
        minSteps = PlayerPrefs.GetInt("minSteps");
        text_Step_min.text = string.Format("{0}", minSteps);
    }
}
