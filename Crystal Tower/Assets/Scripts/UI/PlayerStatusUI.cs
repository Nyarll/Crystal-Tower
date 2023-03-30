using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class PlayerStatusUI : MonoBehaviour
{
    // �v���C���[
    [SerializeField]
    private Player player;

    // �ꊇHP�\���pHP�o�[
    [SerializeField]
    private Slider hp_bar;

    // ���X�Ɍ��炷�pHP�o�[
    [SerializeField]
    private Slider backhp_bar;

    // ���݂̗͕̑\���p�e�L�X�g
    [SerializeField]
    private Text hp_text;

    // �ꊇMP�\���pMP�o�[
    [SerializeField]
    private Slider mp_bar;

    // ���X�Ɍ��炷�pMP�o�[
    [SerializeField]
    private Slider backmp_bar;

    // ���݂̗͕̑\���p�e�L�X�g
    [SerializeField]
    private Text mp_text;

    // �������鑁��
    [SerializeField]
    private float declining_speed = 8f;

    // �ő�̗�
    private int max_hp;
    // ���݂̗̑�
    private int now_hp;
    // ���O�̗̑�
    private int beforeHP;
    // �������̗̑�
    private float decliningHP;

    // �ő�MP
    private int max_mp;
    // ����MP
    private int now_mp;
    // ���OMP
    private int beforeMP;
    // ������MP
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
