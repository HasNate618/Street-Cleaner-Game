using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject startPanel, gamePanel, wasteGuidePanel, howToPlayPanel;

    public void StartGame()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        GameManager.instance.StartGame();
    }

    public void OpenWasteGuide()
    {
        startPanel.SetActive(false);
        wasteGuidePanel.SetActive(true);
    }

    public void OpenHowToPlay()
    {
        startPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void ReturnToMenu()
    {
        wasteGuidePanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
