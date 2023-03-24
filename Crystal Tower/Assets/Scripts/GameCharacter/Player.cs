using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    private Tile[,] map;

    private GameObject sequenceManager;
    private Sequence playerPhase;

    public void SetMapData(Tile[,] map)
    {
        this.map = map;
    }

    // Update is called once per frame
    void Update()
    {
        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        Debug.Log("Input: ( " + horizontal + ", " + vertical + " )");

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove(horizontal, vertical);
        }
    }

    protected override void AttemptMove(int xDir, int yDir)
    {
        sequenceManager = GameObject.FindGameObjectWithTag("Observer");
        playerPhase = sequenceManager.GetComponent<SequenceManager>().GetCurrentSequence();

        if (playerPhase == Sequence.StandbyPhase)
        {
            base.AttemptMove(xDir, yDir);
            sequenceManager.GetComponent<SequenceManager>().ChangeCurrentSequence(Sequence.PlayerPhase);
        }
    }

    protected override void OnCantMove(GameObject hitComponent)
    {
        // “Á‚É‚È‚µ
        Debug.Log("PLAYER: OnCantMove");
    }
}
