using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class PlayerStatusUI : MonoBehaviour
{
    // プレイヤー
    [SerializeField]
    private Player player;

    // 一括HP表示用HPバー
    [SerializeField]
    private Slider hp_bar;

    // 徐々に減らす用HPバー
    [SerializeField]
    private Slider backhp_bar;

    // 現在の体力表示用テキスト
    [SerializeField]
    private Text hp_text;

    // 一括MP表示用MPバー
    [SerializeField]
    private Slider mp_bar;

    // 徐々に減らす用MPバー
    [SerializeField]
    private Slider backmp_bar;

    // 現在の体力表示用テキスト
    [SerializeField]
    private Text mp_text;

    // 減少する早さ
    [SerializeField]
    private float declining_speed = 8f;

    // 最大体力
    private int max_hp;
    // 現在の体力
    private int now_hp;
    // 直前の体力
    private int beforeHP;
    // 減少中の体力
    private float decliningHP;

    // 最大MP
    private int max_mp;
    // 現在MP
    private int now_mp;
    // 直前MP
    private int beforeMP;
    // 減少中MP
    private float decliningMP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        max_hp = player.GetStatus().MaxHP;
        now_hp = player.GetStatus().HP;

        max_mp = player.GetStatus().MaxMP;
        now_mp = player.GetStatus().MP;

        if (hp_bar.value != now_hp)
        {
            beforeHP = (int)hp_bar.value;
            decliningHP = hp_bar.value;
        }
        if (mp_bar.value != now_mp)
        {
            beforeMP = (int)mp_bar.value;
            decliningMP = mp_bar.value;
        }

        hp_bar.maxValue = max_hp;
        backhp_bar.maxValue = max_hp;

        mp_bar.maxValue = max_mp;
        backmp_bar.maxValue = max_mp;

        hp_bar.value = now_hp;
        mp_bar.value = now_mp;

        hp_text.text = "<b><i>" + now_hp + "</i> / <i>" + max_hp + "</i></b>";
        mp_text.text = "<b><i>" + now_mp + "</i> / <i>" + max_mp + "</i></b>";

        if (beforeHP != now_hp)
        {
            DecreaseHP();
        }
        if (beforeMP != now_mp)
        {
            DecreaseMP();
        }
    }

    private void DecreaseHP()
    {
        decliningHP -= declining_speed * Time.deltaTime;
        backhp_bar.value = decliningHP;
        if (backhp_bar.value == hp_bar.value)
        {
            beforeHP = now_hp;
        }
    }

    private void DecreaseMP()
    {
        decliningMP -= declining_speed * Time.deltaTime;
        backmp_bar.value = decliningMP;
        if (backmp_bar.value == mp_bar.value)
        {
            beforeMP = now_mp;
        }
    }
}
