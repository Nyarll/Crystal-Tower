using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject nextFloor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        changeFloor();
    }

    private void changeFloor()
    {
        if(player.transform.position == nextFloor.transform.position)
        {
            this.GetComponent<SequenceManager>().ChangeCurrentSequence(Sequence.EndPhase);
            this.GetComponent<MapCreator>().Generate();
        }
    }
}
