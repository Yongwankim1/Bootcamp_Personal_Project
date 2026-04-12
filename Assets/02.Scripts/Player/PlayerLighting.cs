using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLighting : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] PlayerInputReader inputReader;
    [SerializeField] GameObject lighting;

    [SerializeField] bool isLightToggle;
    Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        UpdateRot();
    }

    private void UpdateRot()
    {
        if(inputReader != null)
        {
            isLightToggle = inputReader.IsLightingToggle;
            lighting.SetActive(isLightToggle);
        }
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;

        Vector3 dir = mouseWorldPos - lighting.transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        lighting.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

    }
}
