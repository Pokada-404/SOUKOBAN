
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_scr_2 : MonoBehaviour
{
    // --------------------------------------------------------
    //
    //Sokoban_script.cs
    //
    //作成日: 2022/10/29
    //作成者: Okada Haruki
    //
    // --------------------------------------------------------


    #region 変数

    //生成するオブジェクトの一覧
    [SerializeField] private GameObject _ground;
    [SerializeField] private GameObject _wall;
    [SerializeField] private GameObject _block;
    [SerializeField] private GameObject _goal;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _theWall;

    //ステージをクリアした時に表示されるUI
    [SerializeField] private GameObject _goalUi;
    //ゲームがスタートの時に表示されるUI
    [SerializeField] private GameObject _startUi;

    //ステージの難易度を設定するための変数
    //ステージを生成する際のループの回数。値が大きいほど難易度が上がる。
    [SerializeField] private int _gameLevel = 10;

    private int _playerPos_x = 0;
    private int _playerPos_y = 0;
    private int _goalPos_x = 0;
    private int _goalPos_y = 0;

    private bool GOAL = false;

    //10*10の二次元配列を用意
    static int[,] _maps = new int[10, 10];

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        /* 
         * 必要な処理
         * ①マップの生成
         * ②箱の配置
         * ③移動させる処理
         */

        /*
         * 二次元配列に格納される文字の意味
         * 0 ...地面(プレイヤー移動可能)
         * 1 ...壁
         * 2 ...ブロック
         * 3 ...ブロックゴール
         * 4 ...プレイヤー
         * 5 ...壁(絶対障壁)
         */


        /** 以下メイン処理 **/
        /*-- 壁と絶対障壁をマップ内に配置する --*/
        MapSet_Wall();

        /*-- プレイヤーとブロックをマップ内に配置する --*/
        MapSet_PlayerAndBlock();

        /*-- プレイヤーの逆再生 --*/
        //指定回数分プレイヤーを動かす

        //ここからスタート地点を決め、マップを生成する。
        //プレイヤーのブロックを押す動きの逆を行う(引き出す)
        //プレイヤーの行動は３パターン
        //①nマスブロックを引く②回り込む③もう一つのブロックの場所まで移動する
        //３つの行動のうちどれを選択するかはランダム

        //ループをn回行う。ループの回数(nの値)が多いほど難易度が高くなる。
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();//ゲームプレイ終了
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _startUi.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MapSet_Wall();
            MapSet_PlayerAndBlock();

            //ステージを生成する際プレイヤーと箱と壁の位置からどの巻き戻し処理ができるかを選択するときに使う。
            int _select = 0;
            _goalUi.gameObject.SetActive(false);
            GOAL = false;
            SearchBoxPos();
            int y = 0;
            for (int i = 0; i < _gameLevel; i++)
            {
                //現在とれる行動を確認
                //まずはプレイヤーの周囲の状況を把握
                //プレイヤーの位置を捜査
                SearchPlayerPos();
                bool success = false;
                while (success == false)
                {
                    _select = Random.Range(1, 20);
                    success = ReversePlayer_PullBox(_select);
                    y++;
                    if (y >= 200)
                    {
                        success = true;
                    }

                }
            }

            Arrangement();
        }
        if (!GOAL)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SearchPlayerPos();
                if (_maps[_playerPos_x, _playerPos_y + 1] == 2)
                {
                    if (_maps[_playerPos_x, _playerPos_y + 2] == 0)
                    {
                        _maps[_playerPos_x, _playerPos_y] = 0;
                        _maps[_playerPos_x, _playerPos_y + 1] = 4;
                        _maps[_playerPos_x, _playerPos_y + 2] = 2;
                    }
                    else if (_maps[_playerPos_x, _playerPos_y + 2] == 3)
                    {
                        _maps[_playerPos_x, _playerPos_y] = 0;
                        _maps[_playerPos_x, _playerPos_y + 1] = 4;
                        _maps[_playerPos_x, _playerPos_y + 2] = 2;
                        GOAL = true;
                        _goalUi.gameObject.SetActive(true);
                    }
                }
                else if (_maps[_playerPos_x, _playerPos_y + 1] == 0)
                {
                    _maps[_playerPos_x, _playerPos_y] = 0;
                    _maps[_playerPos_x, _playerPos_y + 1] = 4;
                }
                else
                {

                }
                Arrangement();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SearchPlayerPos();
                if (_maps[_playerPos_x, _playerPos_y - 1] == 2)
                {
                    if (_maps[_playerPos_x, _playerPos_y - 2] == 0)
                    {
                        _maps[_playerPos_x, _playerPos_y] = 0;
                        _maps[_playerPos_x, _playerPos_y - 1] = 4;
                        _maps[_playerPos_x, _playerPos_y - 2] = 2;
                    }
                    else if (_maps[_playerPos_x, _playerPos_y - 2] == 3)
                    {
                        _maps[_playerPos_x, _playerPos_y] = 0;
                        _maps[_playerPos_x, _playerPos_y - 1] = 4;
                        _maps[_playerPos_x, _playerPos_y - 2] = 2;
                        GOAL = true;
                        _goalUi.gameObject.SetActive(true);
                    }
                }
                else if (_maps[_playerPos_x, _playerPos_y - 1] == 0)
                {
                    _maps[_playerPos_x, _playerPos_y] = 0;
                    _maps[_playerPos_x, _playerPos_y - 1] = 4;
                }
                else
                {

                }
                Arrangement();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SearchPlayerPos();
                if (_maps[_playerPos_x - 1, _playerPos_y] == 2)
                {
                    if (_maps[_playerPos_x - 2, _playerPos_y] == 0)
                    {
                        _maps[_playerPos_x, _playerPos_y] = 0;
                        _maps[_playerPos_x - 1, _playerPos_y] = 4;
                        _maps[_playerPos_x - 2, _playerPos_y] = 2;
                    }
                    else if (_maps[_playerPos_x - 2, _playerPos_y] == 3)
                    {
                        _maps[_playerPos_x, _playerPos_y] = 0;
                        _maps[_playerPos_x - 1, _playerPos_y] = 4;
                        _maps[_playerPos_x - 2, _playerPos_y] = 2;
                        GOAL = true;
                        _goalUi.gameObject.SetActive(true);
                    }
                }
                else if (_maps[_playerPos_x - 1, _playerPos_y] == 0)
                {
                    _maps[_playerPos_x, _playerPos_y] = 0;
                    _maps[_playerPos_x - 1, _playerPos_y] = 4;
                }
                else
                {

                }
                Arrangement();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SearchPlayerPos();
                if (_maps[_playerPos_x + 1, _playerPos_y] == 2)
                {
                    if (_maps[_playerPos_x + 2, _playerPos_y] == 0)
                    {
                        _maps[_playerPos_x, _playerPos_y] = 0;
                        _maps[_playerPos_x + 1, _playerPos_y] = 4;
                        _maps[_playerPos_x + 2, _playerPos_y] = 2;
                    }
                    else if (_maps[_playerPos_x + 2, _playerPos_y] == 3)
                    {
                        _maps[_playerPos_x, _playerPos_y] = 0;
                        _maps[_playerPos_x + 1, _playerPos_y] = 4;
                        _maps[_playerPos_x + 2, _playerPos_y] = 2;
                        GOAL = true;
                        _goalUi.gameObject.SetActive(true);
                    }
                }
                else if (_maps[_playerPos_x + 1, _playerPos_y] == 0)
                {
                    _maps[_playerPos_x, _playerPos_y] = 0;
                    _maps[_playerPos_x + 1, _playerPos_y] = 4;
                }
                else
                {

                }
                Arrangement();
            }
        }
    }






    void MapSet_Wall()
    {
        //配列の内側の要素に１(壁)を格納
        //外掘りは５(絶対障壁)を格納

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if ((i == 0) || (i == 9) || (j == 0) || (j == 9))
                {
                    _maps[i, j] = 5;
                }
                else
                {
                    _maps[i, j] = 1;
                }
            }
        }
    }
    void MapSet_PlayerAndBlock()
    {
        //ループで使うフラグ
        bool loopend = false;
        //要素内の書き換えが正常に行われるまで無限ループ
        while (!loopend)
        {
            //範囲内からランダムでふたつ値を選出
            int generationmap_rnd_A = Random.Range(1, 8);
            int generationmap_rnd_B = Random.Range(1, 8);
            //配列の中身が１(壁)であれば処理を行う。違えばループを繰り返し再度値の選出を行う。
            //このif文は２度目のループ以降でプレイヤーをブロックに上書きしてしまうのを防止するためである。
            if (_maps[generationmap_rnd_A, generationmap_rnd_B] == 1)
            {
                //壁からブロックに書き換え
                _maps[generationmap_rnd_A, generationmap_rnd_B] = 2;
                //プレイヤーを付近に配置
                //ブロックの左右上下どこかにプレイヤーを配置したい。
                //配置できるまでループさせる。
                //ループはFlagで管理する。
                bool loopfrag = false;
                while (!loopfrag)
                {
                    //配置する際に押し込むための移動スペースが確保できるかを確認する。
                    //ランダムで方向を決める
                    //４方向から１方向をランダムで選出
                    int rnd_direction = Random.Range(1, 4);
                    //変数 GM_tes_A と GM_tes_B にいれる
                    int GM_tes_A1 = generationmap_rnd_A;
                    int GM_tes_A2 = generationmap_rnd_A;
                    int GM_tes_B1 = generationmap_rnd_B;
                    int GM_tes_B2 = generationmap_rnd_B;
                    //スイッチ文をつかう
                    switch (rnd_direction)
                    {
                        //上
                        case 1:
                            GM_tes_A1 -= 1;
                            GM_tes_A2 -= 2;
                            break;
                        //右
                        case 2:
                            GM_tes_B1 += 1;
                            GM_tes_B2 += 2;
                            break;
                        //下
                        case 3:
                            GM_tes_A1 += 1;
                            GM_tes_A2 += 2;
                            break;
                        //左
                        case 4:
                            GM_tes_B1 += 1;
                            GM_tes_B2 += 2;
                            break;
                    }
                    if ((_maps[GM_tes_A1, GM_tes_B1] == 1) && (_maps[GM_tes_A2, GM_tes_B2] == 1))
                    {
                        _maps[GM_tes_A1, GM_tes_B1] = 4;
                        //ループを終わらせる
                        loopfrag = true;
                    }
                }

                loopend = true;
            }

        }
    }

    void SearchPlayerPos()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (_maps[i, j] == 4)
                {
                    _playerPos_x = i;
                    _playerPos_y = j;
                }
            }
        }
    }
    void SearchBoxPos()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (_maps[i, j] == 2)
                {
                    _goalPos_x = i;
                    _goalPos_y = j;
                    return;
                }
            }
        }
    }

    bool ReversePlayer_PullBox(int select)
    {
        //１マス引っ張る処理
        for (int j = -1; j < 2; j++)
        {
            for (int l = -1; l < 2; l++)
            {
                if (Main(l, j, select))
                {
                    return true;
                }
            }
        }
        return false;
    }
    bool Main(int l, int j, int select)
    {
        //プレイヤーの現在地を中心に半径１マスの範囲にあるブロックを探す
        //選択された要素が２(ブロック)であれば
        if (_maps[_playerPos_x + j, _playerPos_y + l] == 2)
        {
            //列はどこか
            switch (j)
            {
                case -1:
                    //ｌ(エル)は行
                    //プレイヤーの真上にあるブロックを動かす
                    /*
                     020
                     040
                     000
                     */

                    if (_maps[_playerPos_x - 1, _playerPos_y] == 2)
                    {

                        //下に引き出せるか
                        /*
                         020      000
                         040  ->  020
                         000      040
                         */
                        if (((_maps[_playerPos_x + 1, _playerPos_y] == 1) || (_maps[_playerPos_x + 1, _playerPos_y] == 0)) && (select == 1))
                        {
                            //changePos_PlayerAndBox(処理後の箱のx座標,処理後の箱のy座標,処理後のプレイヤーのx座標,処理後のプレイヤーのy座標)
                            ChangePos_PlayerAndBox(_playerPos_x, _playerPos_y, _playerPos_x + 1, _playerPos_y);

                            //プレイヤーと箱が通った道を０(地面)にする
                            _maps[_playerPos_x - 1, _playerPos_y] = 0;

                            return true;
                        }


                        //右に回り込んで右に引き出せるか
                        /*
                         020      024      0024
                         040  ->  000  ->  0000
                         000      000      0000
                         */
                        if (((_maps[_playerPos_x - 1, _playerPos_y + 1] == 1) || (_maps[_playerPos_x - 1, _playerPos_y + 1] == 0))
                        && ((_maps[_playerPos_x, _playerPos_y + 1] == 1) || (_maps[_playerPos_x, _playerPos_y + 1] == 0))
                        && ((_maps[_playerPos_x - 1, _playerPos_y + 2] == 1) || (_maps[_playerPos_x - 1, _playerPos_y + 2] == 0))
                        && (select == 2))
                        {
                            //changePos_PlayerAndBox(処理後の箱のx座標,処理後の箱のy座標,処理後のプレイヤーのx座標,処理後のプレイヤーのy座標)
                            ChangePos_PlayerAndBox(_playerPos_x - 1, _playerPos_y + 1, _playerPos_x - 1, _playerPos_y + 2);

                            //プレイヤーと箱が通った道を０(地面)にする
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x - 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y + 1] = 0;

                            return true;
                        }


                        //左に回り込んで左に引き出せるか
                        /*
                         020      420      4200
                         040  ->  000  ->  0000
                         000      000      0000
                         */
                        if (((_maps[_playerPos_x - 1, _playerPos_y - 1] == 1) || (_maps[_playerPos_x - 1, _playerPos_y - 1] == 0))
                        && ((_maps[_playerPos_x, _playerPos_y - 1] == 1) || (_maps[_playerPos_x, _playerPos_y - 1] == 0))
                        && ((_maps[_playerPos_x - 1, _playerPos_y - 2] == 1) || (_maps[_playerPos_x - 1, _playerPos_y - 2] == 0))
                        && (select == 4))
                        {
                            //changePos_PlayerAndBox(処理後の箱のx座標,処理後の箱のy座標,処理後のプレイヤーのx座標,処理後のプレイヤーのy座標)
                            ChangePos_PlayerAndBox(_playerPos_x - 1, _playerPos_y - 1, _playerPos_x - 1, _playerPos_y - 2);

                            //プレイヤーと箱が通った道を０(地面)にする
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x - 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y - 1] = 0;

                            return true;
                        }

                    }
                    break;

                case 0:
                    //ｌ(エル)は行
                    //プレイヤーの真上にあるブロックを動かす

                    /*
                     000 
                     042
                     000  
                     */
                    if (_maps[_playerPos_x, _playerPos_y + 1] == 2)
                    {

                        //左に引き出せるか
                        /*
                         000      000
                         042  ->  420
                         000      000
                         */
                        if ((_maps[_playerPos_x, _playerPos_y - 1] == 1) || (_maps[_playerPos_x, _playerPos_y - 1] == 0) && (select == 6))
                        {
                            //changePos_PlayerAndBox(処理後の箱のx座標,処理後の箱のy座標,処理後のプレイヤーのx座標,処理後のプレイヤーのy座標)
                            ChangePos_PlayerAndBox(_playerPos_x, _playerPos_y, _playerPos_x, _playerPos_y - 1);

                            //プレイヤーと箱が通った道を０(地面)にする
                            _maps[_playerPos_x, _playerPos_y + 1] = 0;

                            return true;

                        }


                        //上に回り込んで上に引き出せるか
                        /*
                                           004   
                         000      004      002
                         042  ->  002  ->  000
                         000      000      000
                         */
                        if (((_maps[_playerPos_x - 1, _playerPos_y] == 1) || (_maps[_playerPos_x - 1, _playerPos_y] == 0))
                        && ((_maps[_playerPos_x - 1, _playerPos_y + 1] == 1) || (_maps[_playerPos_x - 1, _playerPos_y + 1] == 0))
                        && ((_maps[_playerPos_x - 2, _playerPos_y + 1] == 1) || (_maps[_playerPos_x - 2, _playerPos_y + 1] == 0))
                        && (select == 7))
                        {
                            //changePos_PlayerAndBox(処理後の箱のx座標,処理後の箱のy座標,処理後のプレイヤーのx座標,処理後のプレイヤーのy座標)
                            ChangePos_PlayerAndBox(_playerPos_x - 1, _playerPos_y + 1, _playerPos_x - 2, _playerPos_y + 1);

                            //プレイヤーと箱が通った道を０(地面)にする
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x - 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y + 1] = 0;

                            return true;

                        }


                        //下に回り込んで下に引き出せるか
                        /*
                         000      000      000
                         042  ->  002  ->  000
                         000      004      002
                                           004
                         */
                        if (((_maps[_playerPos_x + 1, _playerPos_y] == 1) || (_maps[_playerPos_x + 1, _playerPos_y] == 0))
                        && ((_maps[_playerPos_x + 1, _playerPos_y + 1] == 1) || (_maps[_playerPos_x + 1, _playerPos_y + 1] == 0))
                        && ((_maps[_playerPos_x + 2, _playerPos_y + 1] == 1) || (_maps[_playerPos_x + 2, _playerPos_y + 1] == 0))
                        && (select == 9))
                        {
                            /*---- DEBUGLOG ----*/
                            Debug.Log("上に回り込んで上に引き出し");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(処理後の箱のx座標,処理後の箱のy座標,処理後のプレイヤーのx座標,処理後のプレイヤーのy座標)
                            ChangePos_PlayerAndBox(_playerPos_x + 1, _playerPos_y + 1, _playerPos_x + 2, _playerPos_y + 1);

                            //プレイヤーと箱が通った道を０(地面)にする
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x + 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y + 1] = 0;

                            return true;

                        }


                    }

                    /*
                     000
                     240
                     000
                     */
                    if (_maps[_playerPos_x, _playerPos_y - 1] == 2)
                    {
                        //右に引き出せるか
                        /*
                         000      000
                         240  ->  024
                         000      000
                         */
                        if ((_maps[_playerPos_x, _playerPos_y + 1] == 1) || (_maps[_playerPos_x, _playerPos_y + 1] == 0)
                        && (select == 11))
                        {
                            /*---- DEBUGLOG ----*/
                            Debug.Log("左に引き出し");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(処理後の箱のx座標,処理後の箱のy座標,処理後のプレイヤーのx座標,処理後のプレイヤーのy座標)
                            ChangePos_PlayerAndBox(_playerPos_x, _playerPos_y, _playerPos_x, _playerPos_y + 1);

                            //プレイヤーと箱が通った道を０(地面)にする
                            _maps[_playerPos_x, _playerPos_y - 1] = 0;

                            return true;

                        }


                        //上に回り込んで上に引き出せるか
                        /*
                                           400   
                         000      400      200
                         240  ->  200  ->  000
                         000      000      000
                         */
                        if (((_maps[_playerPos_x - 1, _playerPos_y] == 1) || (_maps[_playerPos_x - 1, _playerPos_y] == 0))
                        && ((_maps[_playerPos_x - 1, _playerPos_y - 1] == 1) || (_maps[_playerPos_x - 1, _playerPos_y - 1] == 0))
                        && ((_maps[_playerPos_x - 2, _playerPos_y - 1] == 1) || (_maps[_playerPos_x - 2, _playerPos_y - 1] == 0))
                        && (select == 12))
                        {
                            /*---- DEBUGLOG ----*/
                            Debug.Log("上に回り込んで上に引き出し");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(処理後の箱のx座標,処理後の箱のy座標,処理後のプレイヤーのx座標,処理後のプレイヤーのy座標)
                            ChangePos_PlayerAndBox(_playerPos_x - 1, _playerPos_y - 1, _playerPos_x - 2, _playerPos_y - 1);

                            //プレイヤーと箱が通った道を０(地面)にする
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x - 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y - 1] = 0;

                            return true;
                        }


                        //下に回り込んで下に引き出せるか
                        /*
                         000      000      000
                         240  ->  200  ->  000
                         000      400      200
                                           400
                         */
                        if (((_maps[_playerPos_x + 1, _playerPos_y] == 1) || (_maps[_playerPos_x + 1, _playerPos_y] == 0))
                        && ((_maps[_playerPos_x + 1, _playerPos_y - 1] == 1) || (_maps[_playerPos_x + 1, _playerPos_y - 1] == 0))
                        && ((_maps[_playerPos_x + 2, _playerPos_y - 1] == 1) || (_maps[_playerPos_x + 2, _playerPos_y - 1] == 0))
                        && (select == 14))
                        {
                            /*---- DEBUGLOG ----*/
                            Debug.Log("下に回り込んで下に引き出し");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(処理後の箱のx座標,処理後の箱のy座標,処理後のプレイヤーのx座標,処理後のプレイヤーのy座標)
                            ChangePos_PlayerAndBox(_playerPos_x + 1, _playerPos_y - 1, _playerPos_x + 2, _playerPos_y - 1);

                            //プレイヤーと箱が通った道を０(地面)にする
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x + 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y - 1] = 0;

                            return true;
                        }


                    }
                    break;

                case 1:
                    //ｌ(エル)は行
                    //プレイヤーの真上にあるブロックを動かす
                    /*
                     000
                     040
                     020
                     */
                    if (_maps[_playerPos_x + 1, _playerPos_y] == 2)
                    {

                        //上に引き出せるか
                        /*
                         000      040
                         040  ->  020
                         020      000
                         */
                        if ((_maps[_playerPos_x - 1, _playerPos_y] == 1) || (_maps[_playerPos_x - 1, _playerPos_y] == 0)
                        && (select == 16))
                        {
                            /*---- DEBUGLOG ----*/
                            Debug.Log("上に引き出し");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(処理後の箱のx座標,処理後の箱のy座標,処理後のプレイヤーのx座標,処理後のプレイヤーのy座標)
                            ChangePos_PlayerAndBox(_playerPos_x, _playerPos_y, _playerPos_x - 1, _playerPos_y);

                            //プレイヤーと箱が通った道を０(地面)にする
                            _maps[_playerPos_x + 1, _playerPos_y] = 0;

                            return true;

                        }


                        //右に回り込んで右に引き出せるか
                        /*
                         000      000      0000
                         040  ->  000  ->  0000
                         020      024      0024
                         */
                        if (((_maps[_playerPos_x + 1, _playerPos_y + 1] == 1) || (_maps[_playerPos_x + 1, _playerPos_y + 1] == 0))
                        && ((_maps[_playerPos_x, _playerPos_y + 1] == 1) || (_maps[_playerPos_x, _playerPos_y + 1] == 0))
                        && ((_maps[_playerPos_x + 1, _playerPos_y + 2] == 1) || (_maps[_playerPos_x + 1, _playerPos_y + 2] == 0))
                        && (select == 17))
                        {
                            /*---- DEBUGLOG ----*/
                            Debug.Log("右に回り込んで右に引き出し");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(処理後の箱のx座標,処理後の箱のy座標,処理後のプレイヤーのx座標,処理後のプレイヤーのy座標)
                            ChangePos_PlayerAndBox(_playerPos_x + 1, _playerPos_y + 1, _playerPos_x + 1, _playerPos_y + 2);

                            //プレイヤーと箱が通った道を０(地面)にする
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x + 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y + 1] = 0;

                            return true;


                        }


                        //左に回り込んで左に引き出せるか
                        /*
                         000      000      0000
                         040  ->  000  ->  0000
                         020      420      4200
                         */
                        if (((_maps[_playerPos_x + 1, _playerPos_y - 1] == 1) || (_maps[_playerPos_x + 1, _playerPos_y - 1] == 0))
                        && ((_maps[_playerPos_x, _playerPos_y - 1] == 1) || (_maps[_playerPos_x, _playerPos_y - 1] == 0))
                        && ((_maps[_playerPos_x + 1, _playerPos_y - 2] == 1) || (_maps[_playerPos_x + 1, _playerPos_y - 2] == 0))
                        && (select == 19))
                        {
                            /*---- DEBUGLOG ----*/
                            Debug.Log("左に回り込んで引き出し");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(処理後の箱のx座標,処理後の箱のy座標,処理後のプレイヤーのx座標,処理後のプレイヤーのy座標)
                            ChangePos_PlayerAndBox(_playerPos_x + 1, _playerPos_y - 1, _playerPos_x + 1, _playerPos_y - 2);

                            //プレイヤーと箱が通った道を０(地面)にする
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x + 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y - 1] = 0;

                            return true;
                        }

                    }
                    break;

            }

        }
        return false;
    }

    void ChangePos_PlayerAndBox(int PP_x, int PP_y, int BP_x, int BP_y)
    {
        _maps[PP_x, PP_y] = 2;
        _maps[BP_x, BP_y] = 4;
    }
    void Arrangement()
    {
        if (_maps[_goalPos_x, _goalPos_y] == 0)
        {
            _maps[_goalPos_x, _goalPos_y] = 3;
        }
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                switch (_maps[i, j])
                {
                    /*
                     * 二次元配列に格納される文字の意味
                     * 0 ...地面(プレイヤー移動可能)
                     * 1 ...壁
                     * 2 ...ブロック
                     * 3 ...ブロックゴール
                     * 4 ...プレイヤー
                     * 5 ...壁(絶対障壁)
                     */

                    case 0:
                        Instantiate(_ground, new Vector3(i, this.transform.position.x - 2.6f, j), Quaternion.identity);
                        break;
                    case 1:
                        Instantiate(_wall, new Vector3(i, this.transform.position.x - 2.6f, j), Quaternion.identity);
                        break;

                    case 2:
                        Instantiate(_block, new Vector3(i, this.transform.position.x - 2.6f, j), Quaternion.identity);
                        break;

                    case 3:
                        Instantiate(_goal, new Vector3(i, this.transform.position.x - 2.6f, j), Quaternion.identity);
                        break;

                    case 4:
                        Instantiate(_player, new Vector3(i, this.transform.position.x - 2.6f, j), Quaternion.identity);
                        break;

                    case 5:
                        Instantiate(_theWall, new Vector3(i, this.transform.position.x - 2.6f, j), Quaternion.identity);
                        break;

                }

            }
        }

        Vector3 cam_pos = this.transform.position;
        cam_pos.y += 2;
        this.transform.position = cam_pos;
    }
}