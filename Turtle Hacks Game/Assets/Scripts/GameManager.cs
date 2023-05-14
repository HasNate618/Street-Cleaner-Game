using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] ItemSpawner itemSpawner;

    public int recyclingScore, garbageScore, organicsScore, mistakes, time;
    [SerializeField] Text recyclingText, garbageText, organicsText, mistakesText, timer, report, accuracyText;
    [SerializeField] GameObject endPanel, iconsPanel;
    [SerializeField] Image itemIcon, binIcon;

    private Dictionary<string, int> mistakeCount = new Dictionary<string, int>();

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 0;
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        Audio.PlaySound("DigitalLove", null);
        timer.text = (time + 1).ToString();
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        int timeRemaining = int.Parse(timer.text);
        timeRemaining--;
        timer.text = timeRemaining.ToString();
        if (timeRemaining > 0)
        {
            Invoke("UpdateTimer", 1);
        }
        else
        {
            // Run out of time
            OnGameEnd();
        }
    }

    public void OnMistakeMade(string itemName)
    {
        if (mistakeCount.ContainsKey(itemName))
        {
            mistakeCount[itemName]++;
        }
        else
        {
            mistakeCount.Add(itemName, 1);
        }
        print(itemName + ", " + mistakeCount[itemName].ToString());
        UpdateScore("Missed");
    }

    public void UpdateScore(string wasteType)
    {
        switch (wasteType)
        {
            case "Recycling":
                recyclingScore++;
                recyclingText.text = "Recycling: " + recyclingScore.ToString();
                Audio.PlaySound("PointGain", null);
                break;
            case "Garbage":
                garbageScore++;
                garbageText.text = "Garbage: " + garbageScore.ToString();
                Audio.PlaySound("PointGain", null);
                break;
            case "Organics":
                organicsScore++;
                organicsText.text = "Organics: " + organicsScore.ToString();
                Audio.PlaySound("PointGain", null);
                break;
            case "Missed":
                mistakes++;
                mistakesText.text = "Missed: " + mistakes.ToString();
                Audio.PlaySound("PointLoss", null);
                break;
        }
        int accuracy = (int)(100 - (mistakes * 100f / (recyclingScore + garbageScore + organicsScore + mistakes)));
        accuracyText.text = "Accuracy: " + accuracy.ToString() + "%";
    }

    public void OnGameEnd()
    {
        Time.timeScale = 0;
        endPanel.SetActive(true);
        timer.gameObject.SetActive(false);
        itemSpawner.ClearItems();

        string largestMistakesItem = null;
        int highestMistakes = 0;
        foreach(string key in mistakeCount.Keys)
        {
            if (mistakeCount[key] > highestMistakes)
            {
                largestMistakesItem = key;
                highestMistakes = mistakeCount[key];
            }
        }

        if (largestMistakesItem != null)
        {
            report.text = $"The most mistaken item was {largestMistakesItem} with {highestMistakes} mistakes. " +
                $"This item belongs in the {itemSpawner.GetItemWasteType(largestMistakesItem)}";
            itemIcon.sprite = itemSpawner.GetIconFromItem(largestMistakesItem);
            binIcon.sprite = itemSpawner.GetIconFromWasteType(itemSpawner.GetItemWasteType(largestMistakesItem));
        }
        else
        {
            report.text = "Good job! You made no mistakes!";
            iconsPanel.SetActive(false);
        }
    }
}
