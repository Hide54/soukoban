using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldArrayData : MonoBehaviour
{
    // タグリストの名前に紐づく番号
    private const int NONE = 0;
    private const int STATIC_BLOCK = 1;
    private const int MOVE_BLOCK = 2;
    private const int PLAYER = 3;
    private const int GOAL = 4;

    [SerializeField,Header("配置するオブジェクトの親オブジェクトを設定")]
    private GameObject g_fieldRootObject;

    private string[] g_fieldObjectTagList = {
        "","StaticBlock","MoveBlock","Player","Goal"
    };

    [SerializeField, Header("動かないオブジェクトを設定(Tagを識別する)")]
    private GameObject g_staticBlock;

    [SerializeField, Header("動くオブジェクトを設定(Tagを識別する)")]
    private GameObject g_moveBlock;

    [SerializeField, Header("プレイヤーオブジェクトを設定(Tagを識別する)")]
    private GameObject g_player;

    [SerializeField, Header("ゴールオブジェクトを設定(Tagを識別する)")]
    private GameObject g_goal;

    [SerializeField, Header("歩数を表示するオブジェクトを設定")]
    private Text text_Step;

    [SerializeField, Header("最小歩数を表示するオブジェクトを設定")]
    private Text text_Step_min;

    /// <summary>
    /// フィールドデータ用の変数を定義
    /// </summary>

    private int[,] g_fieldData = {
    { 0, 0, 0, 0, 0, },
    { 0, 0, 0, 0, 0, },
    { 0, 0, 0, 0, 0, },
    { 0, 0, 0, 0, 0, },
    { 0, 0, 0, 0, 0, },
    };

    // 縦横の最大数
    private int g_horizontalMaxCount = 0;
    private int g_verticalMaxCount = 0;

    public Vector2 PlayerPosition { get; set; }

    private int[,] g_goalData = {
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
    };


    // ブロックがターゲットに入った数
    private int g_goalClearCount = 0;
    // ターゲットの最大数
    private int g_goalMaxCount = 0;
    //歩数
    private int stepCount = 0;
    //ゴールまでの最小歩数
    public int stepCountMin = 99;

    public void ImageToArray()
    {
        foreach (Transform fieldObject in g_fieldRootObject.transform)
        {
            int col = Mathf.FloorToInt(fieldObject.position.x);
            int row = Mathf.FloorToInt(-fieldObject.position.y);

            if (g_fieldObjectTagList[STATIC_BLOCK].Equals(fieldObject.tag))
            {
                g_fieldData[row, col] = STATIC_BLOCK;
            }
            else if (g_fieldObjectTagList[MOVE_BLOCK].Equals(fieldObject.tag))
            {
                g_fieldData[row, col] = MOVE_BLOCK;
            }
            else if (g_fieldObjectTagList[PLAYER].Equals(fieldObject.tag))
            {
                g_fieldData[row, col] = PLAYER;
                PlayerPosition = new Vector2(row, col);
            }
            else if (g_fieldObjectTagList[GOAL].Equals(fieldObject.tag))
            {
                g_fieldData[row, col] = GOAL;

                // ターゲットの最大カウント
                g_goalMaxCount++;
            }
            // フィールドデータをターゲット用のデータにコピーする
            g_goalData = (int[,])g_fieldData.Clone();


        }
    }

    public void SetFieldMaxSize()
    {
        // フィールドの縦と横の最大数を取得(フィールドの大きさを取得)
        foreach (Transform fieldObject in g_fieldRootObject.transform)
        {
            /*
            * 縦方向に関しては座標の兼ね合い上
            * 下に行くほどyは減っていくので-をつけることで
            * yの位置を逆転させている
            */
            int positionX = Mathf.FloorToInt(fieldObject.position.x);
            int positionY = Mathf.FloorToInt(-fieldObject.position.y);
            // 横の最大数を設定する
            if (g_horizontalMaxCount < positionX)
            {
                g_horizontalMaxCount = positionX;
            }
            // 縦の最大数を設定する
            if (g_verticalMaxCount < positionY)
            {
                g_verticalMaxCount = positionY;
            }
        }
        // フィールド配列の初期化
        g_fieldData = new int[g_verticalMaxCount + 1, g_horizontalMaxCount + 1];
    }





    // フィールドオブジェクトから指定した座標のオブジェクトを取得する
    // tagIdが-1の場合すべてのタグを対象に検索する
    // 検索にヒットしない場合Nullを返す
    /// <param name="tagId">検索対象のタグを指定</param>
    /// <param name="row">縦位置</param>
    /// <param name="col">横位置</param>
    /// <returns></returns>
    public GameObject GetFieldObject(int tagId, int row, int col)
    {
        foreach (Transform fieldObject in g_fieldRootObject.transform)
        {
            if (tagId != -1 && fieldObject.tag != g_fieldObjectTagList[tagId])
            {
                continue;
            }
            /*
            * 縦方向に関しては座標の兼ね合い上
            * 下に行くほどyは減っていくので-をつけることで
            * yの位置を逆転させている
            */
            if (fieldObject.transform.position.x == col &&
            fieldObject.transform.position.y == -row)
            {
                return fieldObject.gameObject;
            }
        }
        return null;
    }
    // オブジェクトを移動する
    // データを上書きするので移動できるかどうか検査して
    // 移動可能な場合処理を実行してください
    /// <param name="preRow">移動前縦情報</param>
    /// <param name="preCol">移動前横情報</param>
    /// <param name="nextRow">移動後縦情報</param>
    /// <param name="nextCol">移動後横情報</param>
    public void MoveData(int preRow, int preCol, int nextRow, int nextCol)
    {
        // オブジェクトを移動する
        GameObject moveObject =
        GetFieldObject(g_fieldData[preRow, preCol], preRow, preCol);
        if (moveObject != null)
        {
            /*
            * 縦方向に関しては座標の兼ね合い上
            * 下に行くほどyは減っていくので-をつけることで
            * yの位置を逆転させている
            */
            // 座標情報なので最初の引数はX
            moveObject.transform.position = new Vector2(nextCol, -nextRow);
        }


        // 上書きするので要注意
        g_fieldData[nextRow, nextCol] = g_fieldData[preRow, preCol];
        // 移動したら元のデータは削除する
        g_fieldData[preRow, preCol] = NONE;
    }
    // ブロックを移動することが可能かチェックする
    // trueの場合移動可能 flaseの場合移動不可能
    /// <param name="y">移動先Y座標</param>
    /// <param name="x">移動先X座標</param>
    /// <returns>ブロック移動の可否</returns>
    public bool BlockMoveCheck(int y, int x)
    {
        // ターゲットブロックだったら
        if (g_goalData[y, x] == GOAL)
        {
            // ターゲットクリアカウントを上げる
            g_goalClearCount++;
            return true;
        }

        return g_fieldData[y, x] == NONE;
    }
    // ブロックを移動する(ブロック移動チェックも実施)
    /// <param name="preRow">移動前縦情報</param>
    /// <param name="preCol">移動前横情報</param>
    /// <param name="nextRow">移動後縦情報</param>
    /// <param name="nextCol">移動後横情報</param>
    public bool BlockMove(int preRow, int preCol, int nextRow, int nextCol)
    {
        // 境界線外エラー
        if (nextRow < 0 || nextCol < 0 ||
        nextRow > g_verticalMaxCount || nextCol > g_horizontalMaxCount)
        {
            return false;
        }
        bool moveFlag = BlockMoveCheck(nextRow, nextCol);
        // 移動可能かチェックする
        if (moveFlag)
        {
            // 移動が可能な場合移動する
            MoveData(preRow, preCol, nextRow, nextCol);
        }
        return moveFlag;
    }






    // プレイヤーを移動することが可能かチェックする
    // trueの場合移動可能 flaseの場合移動不可能
    /// <param name="preRow">移動前縦情報</param>
    /// <param name="preCol">移動前横情報</param>
    /// <param name="nextRow">移動後縦情報</param>
    /// <param name="nextCol">移動後横情報</param>
    /// <returns>プレイヤー移動の可否</returns>
    public bool PlayerMoveCheck(int preRow, int preCol, int nextRow, int nextCol)
    {
        /* プレイヤーの移動先が動くブロックの時
        * ブロックを移動する処理を実施する
        */
        if (g_fieldData[nextRow, nextCol] == MOVE_BLOCK)
        {
            bool blockMoveFlag = BlockMove(nextRow, nextCol,
            nextRow + (nextRow - preRow),
            nextCol + (nextCol - preCol));

            // ターゲットブロックかつ移動できる移動ブロックだったら
            if (g_goalData[nextRow, nextCol] == GOAL && blockMoveFlag)
            {
                // ターゲットクリアカウントを下げる
                g_goalClearCount--;
            }
            return blockMoveFlag;
        }
        // プレイヤーの移動先が空の時移動する
        // プレイヤーの移動先がターゲットの時移動する
        if (g_fieldData[nextRow, nextCol] == NONE ||
            g_fieldData[nextRow, nextCol] == GOAL)
        {
            return true;
        }
        return false;
    }
    // プレイヤーを移動する(プレイヤー移動チェックも実施)
    /// <param name="preRow">移動前縦情報</param>
    /// <param name="preCol">移動前横情報</param>
    /// <param name="nextRow">移動後縦情報</param>
    /// <param name="nextCol">移動後横情報</param>
    public void PlayerMove(int preRow, int preCol, int nextRow, int nextCol)
    {
        // 移動可能かチェックする
        if (PlayerMoveCheck(preRow, preCol, nextRow, nextCol))
        {
            //歩数をカウント
            stepCount++;

            // 移動が可能な場合移動する
            MoveData(preRow, preCol, nextRow, nextCol);
            // プレイヤーの位置を更新する
            // 座標情報なので最初の引数はX
            PlayerPosition = new Vector2(nextRow, nextCol);
        }
    }






    // ゲームクリアの判定
    public bool GetGameClearJudgment()
    {
        // ターゲットクリア数とターゲットの最大数が一致したらゲームクリア
        if (g_goalClearCount == g_goalMaxCount)
        {
            //ゴールまでの最小歩数を更新
            if (stepCount < stepCountMin)
            {
                stepCountMin = stepCount;
                PlayerPrefs.SetInt("minSteps", stepCountMin);
                PlayerPrefs.Save();
            }
            return true;
        }
        return false;
    }






    // 初回起動時
    // シーンに配置されたオブジェクトを元に配列データを生成する
    private void Awake()
    {
        SetFieldMaxSize();
        ImageToArray();

        if (PlayerPrefs.GetInt("minSteps") < stepCountMin)
        {
            stepCountMin = PlayerPrefs.GetInt("minSteps");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            // 配列を出力するテスト
            print("Field------------------------------------------");
            for (int y = 0; y <= g_verticalMaxCount; y++)
            {
                string outPutString = "";
                for (int x = 0; x <= g_horizontalMaxCount; x++)
                {
                    outPutString += g_fieldData[y, x];
                }
                print(outPutString);
            }
            print("Field------------------------------------------");
            print("プレイヤーポジション:" + PlayerPosition);
        }

        //現在の歩数を表示
        text_Step.text = string.Format("{0}",stepCount);
        //今までで一番小さい歩数を表示
        text_Step_min.text = string.Format("{0}", stepCountMin);
    }
}
