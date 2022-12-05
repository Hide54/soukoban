using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField, Header("最小歩数を入れるテキストを設定")]
    private Text _textStepMin;

    private void Awake()
    {
        //今までで最小の歩数を表示
        _textStepMin.text = PlayerPrefs.GetInt("minSteps").ToString();
    }
}
