using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggingPressure : MonoBehaviour
{
    [SerializeField]
    GlobCentreValuesScriptableObject sensorDataObject;
    [SerializeField]
    RecordingTrackerScriptableObject recordingObject;

    // Update is called once per frame
    void Update()
    {
        string logMessage = LogFile.Timestamp() + ",";
        foreach (var sensor in sensorDataObject.sensors)
        {
            logMessage += sensor.userPressure + ",";
        }
        //Debug.Log("CHECK " + logMessage);
        LogFile.Log($"PressureLogged {recordingObject.recordingNumber}", logMessage);
    }

    private void OnDestroy()
    {
        recordingObject.recordingNumber += 1;
    }
}
