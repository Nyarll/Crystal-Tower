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
                uiManager.AddLogText("<color=red>" + 10 + "</color> ダメージを受けた", LogSystem.LogType.Event);
                this.status.HP -= 10;
                this.status.MP -= 10;
            }
        }
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

    private void ChangeFloor()
    {
        StopCoroutine(_moving);
        isMoving = false;
        Observer observer = GameObject.Find("GameObserver").GetComponent<Observer>();
        observer.ChangeFloor();
        StartCoroutine(SmoothMovement(transform.position));
    }
}
