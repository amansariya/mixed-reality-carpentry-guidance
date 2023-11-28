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

    // Add all the relevant bone IDs
    private readonly OVRSkeleton.BoneId[] FingertipIDs = new OVRSkeleton.BoneId[]
    {
        OVRSkeleton.BoneId.Hand_ThumbTip,
        OVRSkeleton.BoneId.Hand_IndexTip,
        OVRSkeleton.BoneId.Hand_MiddleTip,
        OVRSkeleton.BoneId.Hand_RingTip,
        OVRSkeleton.BoneId.Hand_PinkyTip
    };

    private void Start()
    {
        StartCoroutine(OnOVRHandInitiallized());
    }
    
    // Assigning the fingertipPrefab to the fingerTips
    private IEnumerator OnOVRHandInitiallized()
    {
        while (hands[0].transform.childCount == 0 || hands[1].transform.childCount == 0)
            yield return new WaitForEndOfFrame();

        skeletonLeft = hands[0].GetComponentInChildren<OVRSkeleton>();
        skeletonRight = hands[1].GetComponentInChildren<OVRSkeleton>();

        foreach (var bone in FingertipIDs)
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
            foreach (var boneID in FingertipIDs)
            {
                OVRBone bone = skeleton.Bones.FirstOrDefault(b => b.Id == boneID);
                if (bone != null)
                {
                    Transform boneTransform = bone.Transform;
                    logMessage += FormatTransform(boneTransform) + ",";
                }
            }
        }
        //Debug.Log("CHECK " + logMessage);
        LogFile.Log("HandTrackingData", logMessage);
    }

    private string FormatTransform(Transform transform)
    {
        return $"{transform.position.x},{transform.position.y},{transform.position.z}," +
               $"{transform.rotation.x},{transform.rotation.y},{transform.rotation.z},{transform.rotation.w}";
    }
}