using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HandAnimationController : MonoBehaviour
{
    //public OVRCustomSkeleton leftHandSkeleton;
    public OVRCustomSkeleton rightHandSkeleton;
    [SerializeField] private RecordingTrackerScriptableObject recordingTrackerScriptableObject;

    private List<string> csvLines;

    void Start()
    {
        StartCoroutine(AnimateHandsCoroutine());
    }

    private IEnumerator AnimateHandsCoroutine()
    {
        // Load CSV data
        Debug.Log(Path.Join(Application.persistentDataPath, "HandTrackingData " + recordingTrackerScriptableObject.recordingNumber + ".csv"));
        csvLines = LoadCSV(Path.Join(Application.persistentDataPath, "HandTrackingData " + recordingTrackerScriptableObject.recordingNumber + ".csv"));

        foreach (var line in csvLines)
        {
            List<float> values = ParseCSVLine(line);
            if (values.Count <= 1)
            {
                continue;
            }

            //Debug.Log(values.Count);
            //AnimateHand(leftHandSkeleton, values.GetRange(1, 168));
            AnimateHand(rightHandSkeleton, values.GetRange(169, 168));

            yield return new WaitForSeconds(0.02f); // Wait for the next frame
        }
    }

    private List<string> LoadCSV(string filePath)
    {
        return new List<string>(File.ReadAllLines(filePath));
    }

    private List<float> ParseCSVLine(string line)
    {
        string[] tokens = line.Split(',');
        List<float> values = new List<float>();
        foreach (string token in tokens)
        {
            if (float.TryParse(token, out float value))
            {
                values.Add(value);
            }
        }
        return values;
    }

    private void AnimateHand(OVRCustomSkeleton handSkeleton, List<float> boneData)
    {
        int dataIndex = 0;
        foreach (Transform bone in handSkeleton.CustomBones)
        {
            if (bone != null)
            {
                Vector3 position = new Vector3(boneData[dataIndex], boneData[dataIndex + 1], boneData[dataIndex + 2]);
                Quaternion rotation = new Quaternion(boneData[dataIndex + 3], boneData[dataIndex + 4], boneData[dataIndex + 5], boneData[dataIndex + 6]);
                bone.position = position;
                bone.rotation = rotation;
                dataIndex += 7;
            }
        }
    }
}
