using System.Collections;
using TMPro;
using UnityEngine;

public class QuestCompletePopup : MonoBehaviour
{
    public Quest quest;
    public TextMeshProUGUI titleText;

    public void initialize(Quest quest)
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
            StartCoroutine(closePopupAfterDelay(5f)); // Start coroutine to close popup after 5 seconds
        }
    }

    private IEnumerator closePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        closeCompleteQuestPopup();
    }

    public void closeCompleteQuestPopup()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
}
