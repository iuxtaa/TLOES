using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestCompletePopup : MonoBehaviour
{
    public Quest quest; // the quest that the quest giver will give
    public Player player; // the player that will accept the quest

    public GameObject questWindow;
    public TextMeshProUGUI titleText;

    public void closeCompleteQuestPopupButton()
    {
        questWindow.SetActive(false);
    }
}
