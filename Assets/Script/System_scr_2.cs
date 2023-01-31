
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_scr_2 : MonoBehaviour
{
    // --------------------------------------------------------
    //
    //Sokoban_script.cs
    //
    //�쐬��: 2022/10/29
    //�쐬��: Okada Haruki
    //
    // --------------------------------------------------------


    #region �ϐ�

    //��������I�u�W�F�N�g�̈ꗗ
    [SerializeField] private GameObject _ground;
    [SerializeField] private GameObject _wall;
    [SerializeField] private GameObject _block;
    [SerializeField] private GameObject _goal;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _theWall;

    //�X�e�[�W���N���A�������ɕ\�������UI
    [SerializeField] private GameObject _goalUi;
    //�Q�[�����X�^�[�g�̎��ɕ\�������UI
    [SerializeField] private GameObject _startUi;

    //�X�e�[�W�̓�Փx��ݒ肷�邽�߂̕ϐ�
    //�X�e�[�W�𐶐�����ۂ̃��[�v�̉񐔁B�l���傫���قǓ�Փx���オ��B
    [SerializeField] private int _gameLevel = 10;

    private int _playerPos_x = 0;
    private int _playerPos_y = 0;
    private int _goalPos_x = 0;
    private int _goalPos_y = 0;

    private bool GOAL = false;

    //10*10�̓񎟌��z���p��
    static int[,] _maps = new int[10, 10];

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        /* 
         * �K�v�ȏ���
         * �@�}�b�v�̐���
         * �A���̔z�u
         * �B�ړ������鏈��
         */

        /*
         * �񎟌��z��Ɋi�[����镶���̈Ӗ�
         * 0 ...�n��(�v���C���[�ړ��\)
         * 1 ...��
         * 2 ...�u���b�N
         * 3 ...�u���b�N�S�[��
         * 4 ...�v���C���[
         * 5 ...��(��Ώ��)
         */


        /** �ȉ����C������ **/
        /*-- �ǂƐ�Ώ�ǂ��}�b�v���ɔz�u���� --*/
        MapSet_Wall();

        /*-- �v���C���[�ƃu���b�N���}�b�v���ɔz�u���� --*/
        MapSet_PlayerAndBlock();

        /*-- �v���C���[�̋t�Đ� --*/
        //�w��񐔕��v���C���[�𓮂���

        //��������X�^�[�g�n�_�����߁A�}�b�v�𐶐�����B
        //�v���C���[�̃u���b�N�����������̋t���s��(�����o��)
        //�v���C���[�̍s���͂R�p�^�[��
        //�@n�}�X�u���b�N�������A��荞�އB������̃u���b�N�̏ꏊ�܂ňړ�����
        //�R�̍s���̂����ǂ��I�����邩�̓����_��

        //���[�v��n��s���B���[�v�̉�(n�̒l)�������قǓ�Փx�������Ȃ�B
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();//�Q�[���v���C�I��
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _startUi.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            MapSet_Wall();
            MapSet_PlayerAndBlock();

            //�X�e�[�W�𐶐�����ۃv���C���[�Ɣ��ƕǂ̈ʒu����ǂ̊����߂��������ł��邩��I������Ƃ��Ɏg���B
            int _select = 0;
            _goalUi.gameObject.SetActive(false);
            GOAL = false;
            SearchBoxPos();
            int y = 0;
            for (int i = 0; i < _gameLevel; i++)
            {
                //���݂Ƃ��s�����m�F
                //�܂��̓v���C���[�̎��͂̏󋵂�c��
                //�v���C���[�̈ʒu��{��
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
        //�z��̓����̗v�f�ɂP(��)���i�[
        //�O�@��͂T(��Ώ��)���i�[

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
        //���[�v�Ŏg���t���O
        bool loopend = false;
        //�v�f���̏�������������ɍs����܂Ŗ������[�v
        while (!loopend)
        {
            //�͈͓����烉���_���łӂ��l��I�o
            int generationmap_rnd_A = Random.Range(1, 8);
            int generationmap_rnd_B = Random.Range(1, 8);
            //�z��̒��g���P(��)�ł���Ώ������s���B�Ⴆ�΃��[�v���J��Ԃ��ēx�l�̑I�o���s���B
            //����if���͂Q�x�ڂ̃��[�v�ȍ~�Ńv���C���[���u���b�N�ɏ㏑�����Ă��܂��̂�h�~���邽�߂ł���B
            if (_maps[generationmap_rnd_A, generationmap_rnd_B] == 1)
            {
                //�ǂ���u���b�N�ɏ�������
                _maps[generationmap_rnd_A, generationmap_rnd_B] = 2;
                //�v���C���[��t�߂ɔz�u
                //�u���b�N�̍��E�㉺�ǂ����Ƀv���C���[��z�u�������B
                //�z�u�ł���܂Ń��[�v������B
                //���[�v��Flag�ŊǗ�����B
                bool loopfrag = false;
                while (!loopfrag)
                {
                    //�z�u����ۂɉ������ނ��߂̈ړ��X�y�[�X���m�ۂł��邩���m�F����B
                    //�����_���ŕ��������߂�
                    //�S��������P�����������_���őI�o
                    int rnd_direction = Random.Range(1, 4);
                    //�ϐ� GM_tes_A �� GM_tes_B �ɂ����
                    int GM_tes_A1 = generationmap_rnd_A;
                    int GM_tes_A2 = generationmap_rnd_A;
                    int GM_tes_B1 = generationmap_rnd_B;
                    int GM_tes_B2 = generationmap_rnd_B;
                    //�X�C�b�`��������
                    switch (rnd_direction)
                    {
                        //��
                        case 1:
                            GM_tes_A1 -= 1;
                            GM_tes_A2 -= 2;
                            break;
                        //�E
                        case 2:
                            GM_tes_B1 += 1;
                            GM_tes_B2 += 2;
                            break;
                        //��
                        case 3:
                            GM_tes_A1 += 1;
                            GM_tes_A2 += 2;
                            break;
                        //��
                        case 4:
                            GM_tes_B1 += 1;
                            GM_tes_B2 += 2;
                            break;
                    }
                    if ((_maps[GM_tes_A1, GM_tes_B1] == 1) && (_maps[GM_tes_A2, GM_tes_B2] == 1))
                    {
                        _maps[GM_tes_A1, GM_tes_B1] = 4;
                        //���[�v���I��点��
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
        //�P�}�X�������鏈��
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
        //�v���C���[�̌��ݒn�𒆐S�ɔ��a�P�}�X�͈̔͂ɂ���u���b�N��T��
        //�I�����ꂽ�v�f���Q(�u���b�N)�ł����
        if (_maps[_playerPos_x + j, _playerPos_y + l] == 2)
        {
            //��͂ǂ���
            switch (j)
            {
                case -1:
                    //��(�G��)�͍s
                    //�v���C���[�̐^��ɂ���u���b�N�𓮂���
                    /*
                     020
                     040
                     000
                     */

                    if (_maps[_playerPos_x - 1, _playerPos_y] == 2)
                    {

                        //���Ɉ����o���邩
                        /*
                         020      000
                         040  ->  020
                         000      040
                         */
                        if (((_maps[_playerPos_x + 1, _playerPos_y] == 1) || (_maps[_playerPos_x + 1, _playerPos_y] == 0)) && (select == 1))
                        {
                            //changePos_PlayerAndBox(������̔���x���W,������̔���y���W,������̃v���C���[��x���W,������̃v���C���[��y���W)
                            ChangePos_PlayerAndBox(_playerPos_x, _playerPos_y, _playerPos_x + 1, _playerPos_y);

                            //�v���C���[�Ɣ����ʂ��������O(�n��)�ɂ���
                            _maps[_playerPos_x - 1, _playerPos_y] = 0;

                            return true;
                        }


                        //�E�ɉ�荞��ŉE�Ɉ����o���邩
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
                            //changePos_PlayerAndBox(������̔���x���W,������̔���y���W,������̃v���C���[��x���W,������̃v���C���[��y���W)
                            ChangePos_PlayerAndBox(_playerPos_x - 1, _playerPos_y + 1, _playerPos_x - 1, _playerPos_y + 2);

                            //�v���C���[�Ɣ����ʂ��������O(�n��)�ɂ���
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x - 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y + 1] = 0;

                            return true;
                        }


                        //���ɉ�荞��ō��Ɉ����o���邩
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
                            //changePos_PlayerAndBox(������̔���x���W,������̔���y���W,������̃v���C���[��x���W,������̃v���C���[��y���W)
                            ChangePos_PlayerAndBox(_playerPos_x - 1, _playerPos_y - 1, _playerPos_x - 1, _playerPos_y - 2);

                            //�v���C���[�Ɣ����ʂ��������O(�n��)�ɂ���
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x - 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y - 1] = 0;

                            return true;
                        }

                    }
                    break;

                case 0:
                    //��(�G��)�͍s
                    //�v���C���[�̐^��ɂ���u���b�N�𓮂���

                    /*
                     000 
                     042
                     000  
                     */
                    if (_maps[_playerPos_x, _playerPos_y + 1] == 2)
                    {

                        //���Ɉ����o���邩
                        /*
                         000      000
                         042  ->  420
                         000      000
                         */
                        if ((_maps[_playerPos_x, _playerPos_y - 1] == 1) || (_maps[_playerPos_x, _playerPos_y - 1] == 0) && (select == 6))
                        {
                            //changePos_PlayerAndBox(������̔���x���W,������̔���y���W,������̃v���C���[��x���W,������̃v���C���[��y���W)
                            ChangePos_PlayerAndBox(_playerPos_x, _playerPos_y, _playerPos_x, _playerPos_y - 1);

                            //�v���C���[�Ɣ����ʂ��������O(�n��)�ɂ���
                            _maps[_playerPos_x, _playerPos_y + 1] = 0;

                            return true;

                        }


                        //��ɉ�荞��ŏ�Ɉ����o���邩
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
                            //changePos_PlayerAndBox(������̔���x���W,������̔���y���W,������̃v���C���[��x���W,������̃v���C���[��y���W)
                            ChangePos_PlayerAndBox(_playerPos_x - 1, _playerPos_y + 1, _playerPos_x - 2, _playerPos_y + 1);

                            //�v���C���[�Ɣ����ʂ��������O(�n��)�ɂ���
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x - 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y + 1] = 0;

                            return true;

                        }


                        //���ɉ�荞��ŉ��Ɉ����o���邩
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
                            Debug.Log("��ɉ�荞��ŏ�Ɉ����o��");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(������̔���x���W,������̔���y���W,������̃v���C���[��x���W,������̃v���C���[��y���W)
                            ChangePos_PlayerAndBox(_playerPos_x + 1, _playerPos_y + 1, _playerPos_x + 2, _playerPos_y + 1);

                            //�v���C���[�Ɣ����ʂ��������O(�n��)�ɂ���
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
                        //�E�Ɉ����o���邩
                        /*
                         000      000
                         240  ->  024
                         000      000
                         */
                        if ((_maps[_playerPos_x, _playerPos_y + 1] == 1) || (_maps[_playerPos_x, _playerPos_y + 1] == 0)
                        && (select == 11))
                        {
                            /*---- DEBUGLOG ----*/
                            Debug.Log("���Ɉ����o��");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(������̔���x���W,������̔���y���W,������̃v���C���[��x���W,������̃v���C���[��y���W)
                            ChangePos_PlayerAndBox(_playerPos_x, _playerPos_y, _playerPos_x, _playerPos_y + 1);

                            //�v���C���[�Ɣ����ʂ��������O(�n��)�ɂ���
                            _maps[_playerPos_x, _playerPos_y - 1] = 0;

                            return true;

                        }


                        //��ɉ�荞��ŏ�Ɉ����o���邩
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
                            Debug.Log("��ɉ�荞��ŏ�Ɉ����o��");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(������̔���x���W,������̔���y���W,������̃v���C���[��x���W,������̃v���C���[��y���W)
                            ChangePos_PlayerAndBox(_playerPos_x - 1, _playerPos_y - 1, _playerPos_x - 2, _playerPos_y - 1);

                            //�v���C���[�Ɣ����ʂ��������O(�n��)�ɂ���
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x - 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y - 1] = 0;

                            return true;
                        }


                        //���ɉ�荞��ŉ��Ɉ����o���邩
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
                            Debug.Log("���ɉ�荞��ŉ��Ɉ����o��");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(������̔���x���W,������̔���y���W,������̃v���C���[��x���W,������̃v���C���[��y���W)
                            ChangePos_PlayerAndBox(_playerPos_x + 1, _playerPos_y - 1, _playerPos_x + 2, _playerPos_y - 1);

                            //�v���C���[�Ɣ����ʂ��������O(�n��)�ɂ���
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x + 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y - 1] = 0;

                            return true;
                        }


                    }
                    break;

                case 1:
                    //��(�G��)�͍s
                    //�v���C���[�̐^��ɂ���u���b�N�𓮂���
                    /*
                     000
                     040
                     020
                     */
                    if (_maps[_playerPos_x + 1, _playerPos_y] == 2)
                    {

                        //��Ɉ����o���邩
                        /*
                         000      040
                         040  ->  020
                         020      000
                         */
                        if ((_maps[_playerPos_x - 1, _playerPos_y] == 1) || (_maps[_playerPos_x - 1, _playerPos_y] == 0)
                        && (select == 16))
                        {
                            /*---- DEBUGLOG ----*/
                            Debug.Log("��Ɉ����o��");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(������̔���x���W,������̔���y���W,������̃v���C���[��x���W,������̃v���C���[��y���W)
                            ChangePos_PlayerAndBox(_playerPos_x, _playerPos_y, _playerPos_x - 1, _playerPos_y);

                            //�v���C���[�Ɣ����ʂ��������O(�n��)�ɂ���
                            _maps[_playerPos_x + 1, _playerPos_y] = 0;

                            return true;

                        }


                        //�E�ɉ�荞��ŉE�Ɉ����o���邩
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
                            Debug.Log("�E�ɉ�荞��ŉE�Ɉ����o��");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(������̔���x���W,������̔���y���W,������̃v���C���[��x���W,������̃v���C���[��y���W)
                            ChangePos_PlayerAndBox(_playerPos_x + 1, _playerPos_y + 1, _playerPos_x + 1, _playerPos_y + 2);

                            //�v���C���[�Ɣ����ʂ��������O(�n��)�ɂ���
                            _maps[_playerPos_x, _playerPos_y] = 0;
                            _maps[_playerPos_x + 1, _playerPos_y] = 0;
                            _maps[_playerPos_x, _playerPos_y + 1] = 0;

                            return true;


                        }


                        //���ɉ�荞��ō��Ɉ����o���邩
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
                            Debug.Log("���ɉ�荞��ň����o��");
                            /*---- DEBUGLOG ----*/
                            //changePos_PlayerAndBox(������̔���x���W,������̔���y���W,������̃v���C���[��x���W,������̃v���C���[��y���W)
                            ChangePos_PlayerAndBox(_playerPos_x + 1, _playerPos_y - 1, _playerPos_x + 1, _playerPos_y - 2);

                            //�v���C���[�Ɣ����ʂ��������O(�n��)�ɂ���
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
                     * �񎟌��z��Ɋi�[����镶���̈Ӗ�
                     * 0 ...�n��(�v���C���[�ړ��\)
                     * 1 ...��
                     * 2 ...�u���b�N
                     * 3 ...�u���b�N�S�[��
                     * 4 ...�v���C���[
                     * 5 ...��(��Ώ��)
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