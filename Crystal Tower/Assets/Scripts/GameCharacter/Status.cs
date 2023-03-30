using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status
{
    // �q�b�g�|�C���g
    [SerializeField]
    private int Hp = 1;
    public int HP => Hp;

    // �}�W�b�N�|�C���g
    [SerializeField]
    private int Mp = 1;
    public int MP => Mp;

    // �U����
    [SerializeField]
    private int Atk = 1;
    public int ATK => Atk;

    // �����
    [SerializeField]
    private int Def = 1;
    public int DEF => Def;

    // ���@�U����
    [SerializeField]
    private int Int = 1;
    public int INT => Int;

    // ���@�h���
    [SerializeField]
    private int Mind = 1;
    public int MIND => Mind;

    public Status()
    {
        Hp = 1;
        Mp = 1;
        Atk = 1;
        Def = 1;
        Int = 1;
        Mind = 1;
    }
}
