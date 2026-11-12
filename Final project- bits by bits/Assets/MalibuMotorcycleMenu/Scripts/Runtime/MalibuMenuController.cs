using UnityEngine;
using UnityEngine.SceneManagement;

public class MalibuMenuController : MonoBehaviour
{
    [Header("Set this to the exact name of your gameplay scene")]
    public string gameSceneName = "GameScene";

    [Header("Optional instructions panel")]
    public GameObject instructionsPanel;

    public void PlayGame()
    {
        if (string.IsNullOrWhiteSpace(gameSceneName))
        {
            Debug.LogError("Malibu Menu: Enter your gameplay scene name in the Inspector.");
            return;
        }

        SceneManager.LoadScene(gameSceneName);
    }

    public void ShowInstructions()
    {
        if (instructionsPanel != null) instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        if (instructionsPanel != null) instructionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Malibu Menu: Quit requested.");
        Application.Quit();
    }
}
