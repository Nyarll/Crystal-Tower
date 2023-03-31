using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status
{
    // 名前
    [SerializeField]
    public string Name = "Actor";

    // 最大ヒットポイント
    [SerializeField]
    public int MaxHP = 1;

    // ヒットポイント
    [SerializeField]
    public int HP = 1;

    // 最大マジックポイント
    [SerializeField]
    public int MaxMP = 1;

    // マジックポイント
    [SerializeField]
    public int MP = 1;

    // 攻撃力
    [SerializeField]
    public int ATK = 1;

    // 守備力
    [SerializeField]
    public int DEF = 1;

    // 魔法攻撃力
    [SerializeField]
    public int INT = 1;

    // 魔法防御力
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
