using TMPro;
using UnityEngine;

public class QuestCompletePopup : MonoBehaviour
{
    public Quest quest;
    public TextMeshProUGUI titleText;

    public void Initialize(Quest quest)
    {
        this.quest = quest;
        this.titleText.text = quest.title;
        if (this.quest != null && this.titleText != null)
        {
            this.titleText.text = this.quest.title;
        }
    }

    public void openCompleteQuestPopup()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
    }

    public void closeCompleteQuestPopupButton()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
}
