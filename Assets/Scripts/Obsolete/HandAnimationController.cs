using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HandAnimationController : MonoBehaviour
{
    //public OVRCustomSkeleton leftHandSkeleton;
    //public OVRCustomSkeleton rightHandSkeleton;
    [SerializeField]
    public GameObject rightHand;

    [SerializeField]
    private RecordingTrackerScriptableObject recordingTrackerScriptableObject;

    private OVRCustomSkeleton rightHandSkeleton;
    private SkinnedMeshRenderer skinnedMeshRendererRightHand;
    private Material rightHandMaterial;
    private Color originalColor;

    private List<string> csvLines;

    void Start()
    {
        skinnedMeshRendererRightHand = rightHand.GetComponentInChildren<SkinnedMeshRenderer>();
        rightHandSkeleton = rightHand.GetComponent<OVRCustomSkeleton>();
        rightHandMaterial = skinnedMeshRendererRightHand.material;
        originalColor = rightHandMaterial.color;
        StartCoroutine(AnimateHandsCoroutine());
    }

    private IEnumerator AnimateHandsCoroutine()
    {
        // Load CSV data
        //Debug.Log("Application.persistentDataPath" + Application.persistentDataPath);
        //Debug.Log(Path.Join(Application.persistentDataPath, "HandTrackingData " + recordingTrackerScriptableObject.recordingNumber + ".csv"));
        Debug.Log("DataPathScript:" + Path.Join(Application.persistentDataPath, "HandTrackingData.csv"));
        //csvLines = LoadCSV(Path.Join(Application.persistentDataPath, "HandTrackingData " + recordingTrackerScriptableObject.recordingNumber + ".csv"));
        csvLines = LoadCSV(Path.Join(Application.persistentDataPath, "HandTrackingData.csv"));
        while (true)
        {
            foreach (var line in csvLines)
            {
                //Debug.Log("linelineline: " + line);
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
            StartCoroutine(FlashGreenAndFade());
        }
    }

    IEnumerator FlashGreenAndFade()
    {
        // Change to green color instantly
        rightHandMaterial.color = Color.green;

        // Fade out the color and alpha
        float duration = 1.0f; // Duration in seconds over which the fade will occur
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // Normalize time to [0, 1]
            rightHandMaterial.color = Color.Lerp(Color.green, originalColor, t);
            rightHandMaterial.color = new Color(rightHandMaterial.color.r, rightHandMaterial.color.g, rightHandMaterial.color.b, 1 - t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restore original color with full transparency
        rightHandMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
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
                bone.localPosition = position;
                bone.localRotation = rotation;
                dataIndex += 7;
            }
        }
    }
}
