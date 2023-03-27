using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sequence
{
    StandbyPhase,
    PlayerPhase,
    EnemyBegin,
    EnemyPhase,

    EndPhase
}

public class SequenceManager : MonoBehaviour
{
    public static SequenceManager instance = null;

    private Sequence currentSequence;
    float turnDelay = 0.02f;

    private void Awake()
    {
        ChangeCurrentSequence(Sequence.StandbyPhase);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public Sequence GetCurrentSequence()
    {
        return this.currentSequence;
    }

    public void ChangeCurrentSequence(Sequence sequence)
    {
        this.currentSequence = sequence;
        this.OnSequenceChanged(this.currentSequence);
    }

    private void OnSequenceChanged(Sequence sequence)
    {
        switch (sequence)
        {
            case Sequence.StandbyPhase:
                break;

            case Sequence.PlayerPhase:
                StartCoroutine("PlayerPhase");
                break;

            case Sequence.EnemyBegin:
                ChangeCurrentSequence(Sequence.EnemyPhase);
                break;

            case Sequence.EnemyPhase:
                StartCoroutine("EnemyPhase");
                break;

            case Sequence.EndPhase:
                ChangeCurrentSequence(Sequence.StandbyPhase);
                break;
        }
    }

    private IEnumerator PlayerPhase()
    {
        yield return new WaitForSeconds(turnDelay);
        ChangeCurrentSequence(Sequence.EnemyBegin);
    }

    private IEnumerator EnemyPhase()
    {
        yield return new WaitForSeconds(turnDelay);
        ChangeCurrentSequence(Sequence.EndPhase);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
