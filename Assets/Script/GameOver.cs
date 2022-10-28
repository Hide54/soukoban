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


    private void Awake()
    {
        
    }


    private void Update()
    {
        float VerticalKey = Input.GetAxisRaw("Vertical");
        button[number].Select();
        if (VerticalKey > 0 && !g_inputState)
        {
            if (number == number_max)
            {
                number = 0;
            }
            number++;
            Debug.Log(number);
            g_inputState = true;
            Select();
        }
        else if (VerticalKey < 0 && !g_inputState)
        {
            if (number == 0)
            {
                number = number_max;
            }
            number--;
            Debug.Log(number);
            g_inputState = true;
            Select();
        }
    }
}
