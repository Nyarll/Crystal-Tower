using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status
{
    // ヒットポイント
    [SerializeField]
    private int Hp = 1;
    public int HP => Hp;

    // マジックポイント
    [SerializeField]
    private int Mp = 1;
    public int MP => Mp;

    // 攻撃力
    [SerializeField]
    private int Atk = 1;
    public int ATK => Atk;

    // 守備力
    [SerializeField]
    private int Def = 1;
    public int DEF => Def;

    // 魔法攻撃力
    [SerializeField]
    private int Int = 1;
    public int INT => Int;

    // 魔法防御力
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
