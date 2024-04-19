using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TrackingHands : MonoBehaviour
{
    [SerializeField] private float scale = .8f;
    [SerializeField] private GameObject fingertipPrefab;

    [SerializeField] private OVRHand[] hands;
    [SerializeField] private OVRSkeleton skeletonLeft;
    [SerializeField] private OVRSkeleton skeletonRight;
    [SerializeField] private RecordingTrackerScriptableObject recordingTrackerScriptableObject;

    // Add all the relevant bone IDs
    private readonly OVRSkeleton.BoneId[] handJoints = new OVRSkeleton.BoneId[]
    {
        //OVRSkeleton.BoneId.Hand_Start,
        OVRSkeleton.BoneId.Hand_WristRoot,
        OVRSkeleton.BoneId.Hand_ForearmStub,
        OVRSkeleton.BoneId.Hand_Thumb0,
        OVRSkeleton.BoneId.Hand_Thumb1,
        OVRSkeleton.BoneId.Hand_Thumb2,
        OVRSkeleton.BoneId.Hand_Thumb3,
        OVRSkeleton.BoneId.Hand_Index1,
        OVRSkeleton.BoneId.Hand_Index2,
        OVRSkeleton.BoneId.Hand_Index3,
        OVRSkeleton.BoneId.Hand_Middle1,
        OVRSkeleton.BoneId.Hand_Middle2,
        OVRSkeleton.BoneId.Hand_Middle3,
        OVRSkeleton.BoneId.Hand_Ring1,
        OVRSkeleton.BoneId.Hand_Ring2,
        OVRSkeleton.BoneId.Hand_Ring3,
        OVRSkeleton.BoneId.Hand_Pinky0,
        OVRSkeleton.BoneId.Hand_Pinky1,
        OVRSkeleton.BoneId.Hand_Pinky2,
        OVRSkeleton.BoneId.Hand_Pinky3,
        OVRSkeleton.BoneId.Hand_ThumbTip,
        OVRSkeleton.BoneId.Hand_IndexTip,
        OVRSkeleton.BoneId.Hand_MiddleTip,
        OVRSkeleton.BoneId.Hand_RingTip,
        OVRSkeleton.BoneId.Hand_PinkyTip
        //OVRSkeleton.BoneId.Hand_End
    };

    private void Start()
    {
        StartCoroutine(OnOVRHandInitialized());
        Debug.Log(Application.persistentDataPath);
        recordingTrackerScriptableObject.recordingNumber += 1;
    }

    // Assigning the fingertipPrefab to the fingerTips
    private IEnumerator OnOVRHandInitialized()
    {
        while (hands[0].transform.childCount == 0 || hands[1].transform.childCount == 0)
            yield return new WaitForEndOfFrame();

        skeletonLeft = hands[0].GetComponentInChildren<OVRSkeleton>();
        skeletonRight = hands[1].GetComponentInChildren<OVRSkeleton>();

        foreach (var bone in handJoints)
        {
            var fingertipLeft = skeletonLeft.Bones.Single(a => a.Id == bone);
            var fingertipRight = skeletonRight.Bones.Single(a => a.Id == bone);
            var interactorLeft = Instantiate(fingertipPrefab);
            var interactorRight = Instantiate(fingertipPrefab);
            interactorLeft.transform.SetParent(fingertipLeft.Transform);
            interactorRight.transform.SetParent(fingertipRight.Transform);
            interactorLeft.transform.localPosition = Vector3.zero;
            interactorRight.transform.localPosition = Vector3.zero;
        }
    }

    private void Update()
    {
        string logMessage = LogFile.Timestamp() + ",";
        foreach (var hand in hands)
        {
            OVRSkeleton skeleton = hand.GetComponentInChildren<OVRSkeleton>();
            foreach (var boneID in handJoints)
            {
                OVRBone bone = skeleton.Bones.FirstOrDefault(b => b.Id == boneID);
                if (bone != null)
                {
                    Transform boneTransform = bone.Transform;
                    logMessage += FormatTransform(boneTransform) + ",";
                }
                else
                {
                    //logMessage += FormatTransform
                    Debug.LogWarning($"Bone not found: {boneID}");
                }
            }
        }
        //Debug.Log("CHECK " + logMessage);
        LogFile.Log("HandTrackingData " + recordingTrackerScriptableObject.recordingNumber, logMessage);
    }

    private string FormatTransform(Transform transform)
    {
        return $"{transform.localPosition.x},{transform.localPosition.y},{transform.localPosition.z}," +
               $"{transform.localRotation.x},{transform.localRotation.y},{transform.localRotation.z},{transform.localRotation.w}";
    }
}