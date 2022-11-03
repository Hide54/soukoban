using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField, Header("配置するボタンを設定")]
    private GameObject[] g_button;

    [SerializeField,Header("最初に選択されるボタンを設定")]
    private Button b_button;

    FieldArrayData fieldarrayData;

    public void Retry()
    {
        SceneManager.LoadScene("Sokoban");
    }
    public void End()
    {
        Application.Quit();
    }

    private void Awake()
    {
        fieldarrayData = GetComponent<FieldArrayData>();
        g_button[0].SetActive(false);
        g_button[1].SetActive(false);
        b_button.Select();
    }
    private void Update()
    {
        if (fieldarrayData.g_Gameover)
        {
            g_button[0].SetActive(true);
            g_button[1].SetActive(true);
        }
    }
}
