using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private LogSystem logSystem = null;

    private void Start()
    {
        this.logSystem = GameObject.Find("LogSystem").GetComponent<LogSystem>();
    }

    public void AddLogText(string logText, LogSystem.LogType type = LogSystem.LogType.All)
    {
        logSystem.AddLogText(logText, type);
    }
}
