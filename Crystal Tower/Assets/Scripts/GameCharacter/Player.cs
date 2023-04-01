using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    private Sequence nowPhase;

    protected override void Start()
    {
        base.Start();
        StatusImportToJson("/Save/player_status.json");
        //StatusSaveIntoJson("/Save/player_status.json");
    }

    private void OnDisable()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        nowPhase = SequenceManager.instance.GetCurrentSequence();

        if (nowPhase != Sequence.StandbyPhase)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }

        if (status.HP > 0)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                DamageHP(10);
                DamageMP(10);
            }
        }
        Observer observer = GameObject.Find("GameObserver").GetComponent<Observer>();
        observer.Mapping((int)this.transform.position.x, (int)this.transform.position.y);
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
        SequenceManager.instance.ChangeCurrentSequence(Sequence.PlayerPhase);
    }

    protected override void OnCantMove<T>(T hitComponent)
    {
        // 特になし
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Change")
        {
            if (transform.position.x == other.transform.position.x &&
                transform.position.y == other.transform.position.y)
            {
                uiManager.AddLogText("階段を踏んだ", LogSystem.LogType.Event);
                ChangeFloor();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Tag : " + other.tag + "Position : " + other.gameObject.transform.position);
        if (other.tag == "Room" || other.tag == "Pass")
        {
            //Observer observer = GameObject.Find("GameObserver").GetComponent<Observer>();
            //observer.Mapping(other.gameObject);
        }
    }

    private void ChangeFloor()
    {
        StopCoroutine(_moving);
        isMoving = false;
        Observer observer = GameObject.Find("GameObserver").GetComponent<Observer>();
        observer.ChangeFloor();
        StartCoroutine(SmoothMovement(transform.position));
    }

    private void DamageHP(int damage)
    {
        if (base.DamageHP(damage))
        {
            uiManager.AddLogText(status.Name + "は <color=red>" + damage + "</color> ダメージを受けた");
        }
    }

    private void DamageMP(int damage)
    {
        if (base.DamageMP(damage))
        {
            uiManager.AddLogText(status.Name + "は <color=red>" + damage + "</color> MPを失った");
        }
    }
}
