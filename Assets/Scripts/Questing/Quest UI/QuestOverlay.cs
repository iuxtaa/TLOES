using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestOverlay : MonoBehaviour
{
    public static QuestOverlay Instance { get; private set; }

    public Player player; // the player that will accept the quest

    public GameObject questWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI objectiveText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy the new object if one already exists
        }
    }

    public void Update()
    {
        if (player != null && Player.currentQuest != null)
        {
            if(Player.currentQuest.isActive && !Player.currentQuest.completionStatus)
            {
                openQuestOverlay();
            }
        }
    }
    private void openQuestOverlay()
    {
        questWindow.SetActive(true);
        titleText.text = Player.currentQuest.title;
        descriptionText.text = Player.currentQuest.description;
        objectiveText.text = Player.currentQuest.objectivesToString();
    }
}
