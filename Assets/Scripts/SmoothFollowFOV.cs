using UnityEngine;

public class SmoothFollowFOV : MonoBehaviour
{
    [SerializeField]
    private Transform playerCamera;
    //public OVRSkeleton rightHandSkeleton;
    [SerializeField]
    private Transform rightHandSkeleton;
    [SerializeField]
    private float smoothTime = 0.3F;
    [SerializeField]
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private Vector3 offsetAnimatedHand;
    [SerializeField]
    private Vector3 offsetPressureHand;
    [SerializeField]
    private Transform AnimatedHand;
    [SerializeField]
    private Transform PressureHand;

    //void Update()
    //{

    //    //if (!GetComponentInChildren<SkinnedMeshRenderer>().isVisible)
    //    //    return;
    //    Vector3 targetPosition = rightHandSkeleton.TransformPoint(offsetPressureHand);
    //    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

    //    // make the object always face the camera
    //    transform.LookAt(playerCamera);
    //}



    void Update()
    {
        Vector3 pressureHandTargetPosition = rightHandSkeleton.TransformPoint(offsetPressureHand);
        Vector3 animatedHandTargetPosition = rightHandSkeleton.TransformPoint(offsetAnimatedHand);

        PressureHand.transform.position = Vector3.SmoothDamp(PressureHand.transform.position, pressureHandTargetPosition, ref velocity, smoothTime);
        AnimatedHand.transform.position = Vector3.SmoothDamp(AnimatedHand.transform.position, animatedHandTargetPosition, ref velocity, smoothTime);

        // make the object always face the camera
        PressureHand.transform.LookAt(playerCamera);
        AnimatedHand.transform.LookAt(playerCamera);
    }
}