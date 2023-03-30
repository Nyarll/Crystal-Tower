using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LogSystem : MonoBehaviour
{
    public enum LogType
    {
        All,
        Event
    }

    // ログ出力先テキスト
    [SerializeField]
    private Text logText;

    // 全ログデータ
    private List<string> allLogs;
    // イベントログデータ
    private List<string> eventLogs;

    // 現在表示するログの種類
    [SerializeField]
    private LogType logTypeToDisplay = LogType.All;

    // ログを保存する数
    [SerializeField]
    private int logDataNum = 256;

    // スクロールバー
    [SerializeField]
    private Scrollbar verticalScrollber;
    private StringBuilder logTextStringBuilder;

    // Start is called before the first frame update
    void Start()
    {
        allLogs = new List<string>();
        eventLogs = new List<string>();

        logTextStringBuilder = new StringBuilder();
    }

    public void AddLogText(string logText, LogType logType)
    {
        logText = " " + logText;
        allLogs.Add(logText);
        switch (logType)
        {
            case LogType.Event:
                eventLogs.Add(logText);
                break;
        }

        LogDataSort(this.allLogs);
        LogDataSort(this.eventLogs);

        if (logTypeToDisplay == LogType.All || logTypeToDisplay == logType)
        {
            ViewLogText();
        }
    }

    private void LogDataSort(List<string> list)
    {
        if (list.Count > logDataNum)
        {
            list.RemoveAt(0);
        }
    }

    /// <summary>
    /// ログテキスト表示
    /// </summary>
    public void ViewLogText()
    {
        logTextStringBuilder.Clear();
        List<string> selectedLogs = new List<string>();
        switch (logTypeToDisplay)
        {
            case LogType.All:
                selectedLogs = allLogs;
                break;

            case LogType.Event:
                selectedLogs = eventLogs;
                break;
        }
        foreach (string log in selectedLogs)
        {
            logTextStringBuilder.Insert(logTextStringBuilder.Length, log + Environment.NewLine);
        }
        logText.text = logTextStringBuilder.ToString().TrimEnd();
        UpdateScrollBar();
    }

    public void UpdateScrollBar()
    {
        verticalScrollber.value = 0f;
    }
}
