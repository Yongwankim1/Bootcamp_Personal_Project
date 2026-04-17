using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLighting : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] PlayerInputReader inputReader;
    [SerializeField] GameObject[] lighting;

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
            lighting[0].SetActive(isLightToggle);
            lighting[1].SetActive(isLightToggle);
        }
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;

        Vector3 dir = mouseWorldPos - lighting[0].transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        lighting[0].transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

    }
}
