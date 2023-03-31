using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status
{
    // ���O
    [SerializeField]
    public string Name = "Actor";

    // �ő�q�b�g�|�C���g
    [SerializeField]
    public int MaxHP = 1;

    // �q�b�g�|�C���g
    [SerializeField]
    public int HP = 1;

    // �ő�}�W�b�N�|�C���g
    [SerializeField]
    public int MaxMP = 1;

    // �}�W�b�N�|�C���g
    [SerializeField]
    public int MP = 1;

    // �U����
    [SerializeField]
    public int ATK = 1;

    // �����
    [SerializeField]
    public int DEF = 1;

    // ���@�U����
    [SerializeField]
    public int INT = 1;

    // ���@�h���
    [SerializeField]
    public int MIND = 1;

    public Status()
    {
        Name = "Actor";
        HP = 1;
        MP = 1;
        ATK = 1;
        DEF = 1;
        INT = 1;
        MIND = 1;
    }
}
