using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 配列管理用のスクリプト
/// </summary>
public class FieldArrayData : MonoBehaviour
{
    // タグリストの名前に紐づく番号
    private const int NONE = 0;
    private const int STATIC_BLOCK = 1;
    private const int MOVE_BLOCK = 2;
    private const int PLAYER = 3;
    private const int GOAL = 4;

    [SerializeField,Header("配置するオブジェクトの親オブジェクトを設定")]
    private GameObject _fieldRootObject;

    private string[] _fieldObjectTagList = {
        "","StaticBlock","MoveBlock","Player","Goal"
    };

    [SerializeField, Header("動かないオブジェクトを設定(Tagを識別する)")]
    private GameObject _staticBlock;

    [SerializeField, Header("動くオブジェクトを設定(Tagを識別する)")]
    private GameObject _moveBlock;

    [SerializeField, Header("プレイヤーオブジェクトを設定(Tagを識別する)")]
    private GameObject _player;

    [SerializeField, Header("ゴールオブジェクトを設定(Tagを識別する)")]
    private GameObject _goal;

    [SerializeField, Header("歩数を表示するオブジェクトを設定")]
    private Text _textStep;

    [SerializeField, Header("最小歩数を表示するオブジェクトを設定")]
    private Text _textStepMin;

    /// <summary>
    /// フィールドデータ用の変数を定義
    /// </summary>
    private int[,] _fieldData = {
    { 0, 0, 0, 0, 0, },
    { 0, 0, 0, 0, 0, },
    { 0, 0, 0, 0, 0, },
    { 0, 0, 0, 0, 0, },
    { 0, 0, 0, 0, 0, },
    };

    // 縦横の最大数
    private int _horizontalMaxCount = 0;
    private int _verticalMaxCount = 0;

    public Vector2 PlayerPosition { get; set; }

    private int[,] _goalData = {
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
        { 0, 0, 0, 0, 0, },
    };


    // ブロックがターゲットに入った数
    private int _goalClearCount = 0;
    // ターゲットの最大数
    private int _goalMaxCount = 0;
    //歩数
    private int _stepCount = 0;
    //ゴールまでの最小歩数
    private int _stepCountMin = default;

    public void ImageToArray()
    {
        foreach (Transform fieldObject in _fieldRootObject.transform)
        {
            int col = Mathf.FloorToInt(fieldObject.position.x);
            int row = Mathf.FloorToInt(-fieldObject.position.y);

            if (_fieldObjectTagList[STATIC_BLOCK].Equals(fieldObject.tag))
            {
                _fieldData[row, col] = STATIC_BLOCK;
            }
            else if (_fieldObjectTagList[MOVE_BLOCK].Equals(fieldObject.tag))
            {
                _fieldData[row, col] = MOVE_BLOCK;
            }
            else if (_fieldObjectTagList[PLAYER].Equals(fieldObject.tag))
            {
                _fieldData[row, col] = PLAYER;
                PlayerPosition = new Vector2(row, col);
            }
            else if (_fieldObjectTagList[GOAL].Equals(fieldObject.tag))
            {
                _fieldData[row, col] = GOAL;

                // ターゲットの最大カウント
                _goalMaxCount++;
            }
            // フィールドデータをターゲット用のデータにコピーする
            _goalData = (int[,])_fieldData.Clone();


        }
    }

    public void SetFieldMaxSize()
    {
        // フィールドの縦と横の最大数を取得(フィールドの大きさを取得)
        foreach (Transform fieldObject in _fieldRootObject.transform)
        {
            /*
             * 縦方向に関しては座標の兼ね合い上
             * 下に行くほどyは減っていくので-をつけることで
             * yの位置を逆転させている
             */
            int positionX = Mathf.FloorToInt(fieldObject.position.x);
            int positionY = Mathf.FloorToInt(-fieldObject.position.y);
            // 横の最大数を設定する
            if (_horizontalMaxCount < positionX)
            {
                _horizontalMaxCount = positionX;
            }
            // 縦の最大数を設定する
            if (_verticalMaxCount < positionY)
            {
                _verticalMaxCount = positionY;
            }
        }
        // フィールド配列の初期化
        _fieldData = new int[_verticalMaxCount + 1, _horizontalMaxCount + 1];
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
        foreach (Transform fieldObject in _fieldRootObject.transform)
        {
            if (tagId != -1 && fieldObject.tag != _fieldObjectTagList[tagId])
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
        GetFieldObject(_fieldData[preRow, preCol], preRow, preCol);
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
        _fieldData[nextRow, nextCol] = _fieldData[preRow, preCol];
        // 移動したら元のデータは削除する
        _fieldData[preRow, preCol] = NONE;
    }






    // ブロックを移動することが可能かチェックする
    // trueの場合移動可能 flaseの場合移動不可能
    /// <param name="y">移動先Y座標</param>
    /// <param name="x">移動先X座標</param>
    /// <returns>ブロック移動の可否</returns>
    public bool BlockMoveCheck(int y, int x)
    {
        // ターゲットブロックだったら
        if (_goalData[y, x] == GOAL)
        {
            // ターゲットクリアカウントを上げる
            _goalClearCount++;
            return true;
        }

        return _fieldData[y, x] == NONE;
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
        nextRow > _verticalMaxCount || nextCol > _horizontalMaxCount)
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
        if (_fieldData[nextRow, nextCol] == MOVE_BLOCK)
        {
            bool blockMoveFlag = BlockMove(nextRow, nextCol,
            nextRow + (nextRow - preRow),
            nextCol + (nextCol - preCol));

            // ターゲットブロックかつ移動できる移動ブロックだったら
            if (_goalData[nextRow, nextCol] == GOAL && blockMoveFlag)
            {
                // ターゲットクリアカウントを下げる
                _goalClearCount--;
            }
            return blockMoveFlag;
        }
        // プレイヤーの移動先が空の時移動する
        // プレイヤーの移動先がターゲットの時移動する
        if (_fieldData[nextRow, nextCol] == NONE ||
            _fieldData[nextRow, nextCol] == GOAL)
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
            _stepCount++;

            // 移動が可能な場合移動する
            MoveData(preRow, preCol, nextRow, nextCol);
            // プレイヤーの位置を更新する
            // 座標情報なので最初の引数はX
            PlayerPosition = new Vector2(nextRow, nextCol);

            //現在の歩数を表示
            _textStep.text = _stepCount.ToString();
        }
    }






    // ゲームクリアの判定
    public bool GetGameClearJudgment()
    {
        // ターゲットクリア数とターゲットの最大数が一致したらゲームクリア
        if (_goalClearCount == _goalMaxCount)
        {
            //ゴールまでの最小歩数を更新
            if (_stepCount < _stepCountMin)
            {
                _stepCountMin = _stepCount;
            }
            //ゴールまでの最小歩数を保存
            PlayerPrefs.SetInt("minSteps", _stepCountMin);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }






    // 初回起動時
    // シーンに配置されたオブジェクトを元に配列データを生成する
    // 最小歩数を設定
    private void Awake()
    {
        SetFieldMaxSize();
        ImageToArray();

        _stepCountMin = PlayerPrefs.GetInt("minSteps");

        //今までで一番小さい歩数を表示
        _textStepMin.text = _stepCountMin.ToString();
        //現在の歩数を表示
        _textStep.text = _stepCount.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            // 配列を出力するテスト
            print("Field------------------------------------------");
            for (int y = 0; y <= _verticalMaxCount; y++)
            {
                string outPutString = "";
                for (int x = 0; x <= _horizontalMaxCount; x++)
                {
                    outPutString += _fieldData[y, x];
                }
                print(outPutString);
            }
            print("Field------------------------------------------");
            print("プレイヤーポジション:" + PlayerPosition);
        }
    }
}
