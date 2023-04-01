using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMST : MonoBehaviour
{
    private bool is_enter = false;

    public void Enter()
    {
        is_enter = true;
    }

    public bool isEnter()
    {
        return is_enter;
    }
}
