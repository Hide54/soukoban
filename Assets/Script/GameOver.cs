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

    int min_Steps;

    public void Retry()
    {
        SceneManager.LoadScene("Sokoban");
    }
    public void End()
    {
        PlayerPrefs.SetInt("min_Steps", 99);
        PlayerPrefs.Save();
        Application.Quit();
    }

    private void Awake()
    {
        b_button.Select();
        min_Steps = PlayerPrefs.GetInt("min_Steps");
        text_Step_min.text = string.Format("{0}", min_Steps);
    }
}
