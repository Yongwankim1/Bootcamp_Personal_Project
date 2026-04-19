using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseSceneBtnManager : MonoBehaviour
{
    [SerializeField] GameObject baseInventoryPanel;
    [SerializeField] GameObject selectPlayPanel;

    public void OnSetCurrentStage(int value)
    {
        if (StageManager.Instance == null) return;

        StageManager.Instance.CurrentStage = value;
    }
    public void OnBaseInventory()
    {
        baseInventoryPanel.SetActive(true);
    }

    public void OnSelectPlayPanel()
    {
        selectPlayPanel.SetActive(true);
    }

    public void OnLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnGameExit()
    {
        Application.Quit();
    }
}
