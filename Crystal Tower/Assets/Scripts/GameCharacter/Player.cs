using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    private Sequence nowPhase;

    protected override void Start()
    {
        base.Start();
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
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
        SequenceManager.instance.ChangeCurrentSequence(Sequence.PlayerPhase);
    }

    protected override void OnCantMove<T>(T hitComponent)
    {
        // ���ɂȂ�
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Change")
        {
            ChangeFloor();
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
