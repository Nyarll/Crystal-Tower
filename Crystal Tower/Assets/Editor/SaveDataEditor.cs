using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveDataEditor : EditorWindow
{
    string text = "データが見つかりませんでした";
    Vector2 scroll;

    [MenuItem("Window/SaveDataEditor")]
    static void Open()
    {
        GetWindow<SaveDataEditor>("セーブデータ編集");
    }

    string key;

    void OnGUI()
    {
        try
        {
            key = EditorGUILayout.TextField(key);
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            {
                if (GUILayout.Button("参照")) text = ReadSaveData(key);
                if (GUILayout.Button("保存")) WriteSaveData(key, text);
            }
            EditorGUILayout.EndHorizontal();

            scroll = EditorGUILayout.BeginScrollView(scroll);
            text = EditorGUILayout.TextArea(text);
            EditorGUILayout.EndScrollView();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    /**
    * セーブデータを文字列で取得
    */
    private string ReadSaveData(string k)
    {
        StreamReader reader = new StreamReader(Application.dataPath + "/" + k);
        string data = reader.ReadToEnd();
        reader.Close();
        if (data == "") return "データが見つかりませんでした";
        data = JsonPrettyPrint(data);
        return data;
    }

    private void WriteSaveData(string k, string data)
    {
        Debug.Log(data);
        StreamWriter writer = new StreamWriter(Application.dataPath + "/" + k);
        writer.Write(data);
        writer.Flush();
        writer.Close();
    }

    private string JsonPrettyPrint(string i_json)
    {
        if (string.IsNullOrEmpty(i_json))
        {
            return string.Empty;
        }

        i_json = i_json.Replace(System.Environment.NewLine, "").Replace("\t", "");
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        bool quote = false;
        bool ignore = false;
        int offset = 0;
        int indentLength = 3;

        foreach (char ch in i_json)
        {
            switch (ch)
            {
                case '"':
                    if (!ignore)
                    {
                        quote = !quote;
                    }
                    break;
                case '\'':
                    if (quote)
                    {
                        ignore = !ignore;
                    }
                    break;
            }

            if (quote)
            {
                sb.Append(ch);
            }
            else
            {
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        sb.Append(System.Environment.NewLine);
                        sb.Append(new string(' ', ++offset * indentLength));
                        break;
                    case '}':
                    case ']':
                        sb.Append(System.Environment.NewLine);
                        sb.Append(new string(' ', --offset * indentLength));
                        sb.Append(ch);
                        break;
                    case ',':
                        sb.Append(ch);
                        sb.Append(System.Environment.NewLine);
                        sb.Append(new string(' ', offset * indentLength));
                        break;
                    case ':':
                        sb.Append(ch);
                        sb.Append(' ');
                        break;
                    default:
                        if (ch != ' ')
                        {
                            sb.Append(ch);
                        }
                        break;
                }
            }
        }

        return sb.ToString().Trim();
    }
}
