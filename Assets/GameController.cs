using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // タイルの種類
    private enum TileType
    {
        NONE, // 何も無い
        GROUND, // 地面
        TARGET, // 目的地
        PLAYER, // プレイヤー
        BLOCK, // ブロック

        PLAYER_ON_TARGET, // プレイヤー（目的地の上）
        BLOCK_ON_TARGET, // ブロック（目的地の上）
    }

    [SerializeField, Header("マップの構造が書かれたテキストを設定")]
    private TextAsset mapFile;

    private int rows;
    private int columns;
    private TileType[,] tileList;

    //タイル情報を読み込む
    private void LoadTileData()
    {
        //タイルの情報を一行ごとに分割
        string[] lines = mapFile.text.Split
            (
                new[] { '\r', '\n' },
                System.StringSplitOptions.RemoveEmptyEntries
            );

        //タイルの列数を計算
        string[] nums = lines[0].Split(new[] { ',' });

        //タイルの列数と行数を保持
        rows = lines.Length;
        columns = nums.Length;

        tileList = new TileType[columns, rows];
        for (int y = 0; y < rows; y++)
        {
            string st = lines[y];
            nums = st.Split(new[] { ',' });
            for (int x = 0; x < columns; x++)
            {
                tileList[x, y] = (TileType)int.Parse(nums[x]);
            }
        }
    }



    [SerializeField, Header("タイルのサイズ")]
    private float tileSize;
    [SerializeField, Header("スプライト")]
    private Sprite allSprite;
    //プレイヤーのゲームオブジェクト
    private GameObject player;
    //中心位置
    private Vector2 middleOffset;
    //ブロックの数
    private int blockCount;
    //各位置に存在するゲームオブジェクトを管理する連想配列
    private Dictionary<GameObject, Vector2Int> gameObjectPosTable = new Dictionary<GameObject, Vector2Int>();
    private float tempNum = 0.5f;

    private void CreateStage()
    {
        //ステージの中心位置を計算
        middleOffset.x = columns * tileSize * tempNum - tileSize * tempNum;
        middleOffset.y = rows * tileSize * tempNum - tileSize * tempNum;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                TileType val = tileList[x, y];

                
                //壁の場合
                if (val == TileType.NONE) continue;
                //{
                //    GameObject wall = new GameObject("Wall");
                //    sr = wall.AddComponent<SpriteRenderer>();
                //    sr.sprite = allSprite;
                //    sr.color = new Color(255, 95, 0);
                //    sr.sortingOrder = 2;
                //    wall.transform.position = GetDisplayPosition(x, y);
                //}

                string name = "tile" + y + "_" + x;
                GameObject tile = new GameObject(name);
                SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
                sr.sprite = allSprite;
                sr.color = new Color(120, 120, 120);
                tile.transform.position = GetDisplayPosition(x, y);

                //目的地の場合
                if (val == TileType.TARGET)
                {
                    GameObject destination = new GameObject("Goal");
                    sr.sprite = allSprite;
                    sr.color = new Color(0, 75, 255);
                    sr.sortingOrder = 1;
                    destination.transform.position = GetDisplayPosition(x, y);
                }
                // プレイヤーの場合
                if (val == TileType.PLAYER)
                {
                    player = new GameObject("player");
                    sr = player.AddComponent<SpriteRenderer>();
                    sr.sprite = allSprite;
                    sr.color = new Color(255, 255, 255);
                    sr.sortingOrder = 2;
                    player.transform.position = GetDisplayPosition(x, y);
                    gameObjectPosTable.Add(player, new Vector2Int(x, y));
                }
                // ブロックの場合
                else if (val == TileType.BLOCK)
                {
                    blockCount++;
                    GameObject block = new GameObject("block" + blockCount);
                    sr = block.AddComponent<SpriteRenderer>();
                    sr.sprite = allSprite;
                    sr.color = new Color(255, 175, 0);
                    sr.sortingOrder = 2;
                    block.transform.position = GetDisplayPosition(x, y);
                    gameObjectPosTable.Add(block, new Vector2Int(x, y));
                }
            }
        }
    }

    // 指定された行番号と列番号からスプライトの表示位置を計算して返す
    private Vector2 GetDisplayPosition(int x, int y)
    {
        return new Vector2
        (
            x * tileSize - middleOffset.x,
            y * -tileSize + middleOffset.y
        );
    }

    private void Awake()
    {
        LoadTileData();
        CreateStage();
    }
}
