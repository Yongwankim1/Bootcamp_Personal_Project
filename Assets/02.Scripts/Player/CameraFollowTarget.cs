using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float followDistance;
    [SerializeField] float projectionSize;
    public bool IsFollow {  get; private set; } = true;
    public void SetIsFollow(bool value) => IsFollow = value;
    private void Awake()
    {
        if(mainCamera == null)
            mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        FollowCamera();
    }

    void FollowCamera()
    {
        if (!IsFollow) return;
        Vector3 position = transform.position;
        position.z = -1 * followDistance;
        mainCamera.transform.position = position;
        mainCamera.orthographicSize = projectionSize;
    }

}
