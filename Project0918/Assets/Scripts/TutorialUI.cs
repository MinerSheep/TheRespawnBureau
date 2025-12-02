using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;

    private void Start()
    {
        StartCoroutine(TutorialSequence());
    }

    private System.Collections.IEnumerator TutorialSequence()
    {
        tutorialText.text = "Press SPACE to jump";
        yield return new WaitForSeconds(5f);

        tutorialText.text = "Please collect more coins";
        yield return new WaitForSeconds(5f);

        tutorialText.text = "Avoid the obstacles!";
        yield return new WaitForSeconds(5f);
    }
}