using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [Header("配置するボタンを設定")]
    public Button[] button;

    Sokoban sokoban;

    const int number_max = 1;
    int number = 0;

    // キーパットの入力状態
    bool g_inputState = false;

    public void Retry()
    {
        SceneManager.LoadScene("Sokoban");
    }
    public void End()
    {
        Application.Quit();
    }

    private void Select()
    {
        button[number].Select();
        return;
    }
}
