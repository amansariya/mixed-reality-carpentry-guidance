using UnityEngine;

public class SmoothFollowFOV : MonoBehaviour
{
    [SerializeField]
    private Transform playerCamera;
    [SerializeField]
    private float smoothTime = 0.3F;
    [SerializeField]
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private Vector3 offset = new Vector3(0, 0, 5);

    void Update()
    {
        //if (!GetComponentInChildren<SkinnedMeshRenderer>().isVisible)
        //    return;
        Vector3 targetPosition = playerCamera.TransformPoint(offset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Optionally, make the object always face the camera.
        //transform.LookAt(playerCamera);
    }
}