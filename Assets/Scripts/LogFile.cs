using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LogFile
{
    private const string EXTENSION = ".csv";
    private const string TIMESTAMP_FORMAT = "yyyyMMddHHmmssffff";

    private static readonly string TIMESTAMP = Timestamp();
    private static readonly Dictionary<string, StreamWriter> repository = new();
    private static LogFileSchedueler scheduler;

    public static void Log(string name, string message)
    {
        if (!repository.ContainsKey(name))
            Create(name);
        repository[name].WriteLine(message);
        repository[name].Flush();
        Debug.Log($"[{name}] {message}");
    }

    private static void Create(string name)
    {
        Debug.Log($"[{name}]");
        string path = Path.Join(Application.persistentDataPath, name + EXTENSION);
        Debug.Log($"[{name}] {path}");
        var dir = Directory.GetParent(path);
        Directory.CreateDirectory(dir.FullName);
        File.Create(path).Dispose();
        var writer = new StreamWriter(path);
        repository[name] = writer;
        Debug.Log($"[{name}] {path}");
    }

    public static Coroutine ScheduleLog(string name, Func<string> logFunction, float periodSeconds)
    {
        if (scheduler == null)
        {
            var gobj = new GameObject("LogFile");
            scheduler = gobj.AddComponent<LogFileSchedueler>();
        }
        return scheduler.StartCoroutine(PeriodicLog_Coroutine(name, logFunction, periodSeconds));
    }

    private static IEnumerator PeriodicLog_Coroutine(string name, Func<string> logFunction, float periodSeconds)
    {
        while (true)
        {
            Log(name, logFunction());
            yield return new WaitForSeconds(periodSeconds);
        }
    }

    public static string Timestamp()
    {
        return DateTime.UtcNow.ToString(TIMESTAMP_FORMAT);
    }

    private class LogFileSchedueler : MonoBehaviour { }
}