using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonRandomDisplayChance : MonoBehaviour
{
    [Range(0f, 1f)]
    public float chance = 1f;

    [SerializeField] private float fadeDuration = 0.5f;

    private Image image;
    private Button button;

    void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    void OnEnable()
    {
        if (image != null)
        {
            Color c = image.color;
            c.a = 0f;
            image.color = c;
        }

        // Roll chance
        if (Random.value <= chance)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = t / fadeDuration;

            if (image != null)
            {
                Color c = image.color;
                c.a = alpha;
                image.color = c;
            }

            yield return null;
        }

        if (image != null)
        {
            Color c = image.color;
            c.a = 1f;
            image.color = c;
        }
    }

    public void OpenScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneName);
    }

    public void RestartScene()
    {
        Time.timeScale = 1.0f;
        _ = Game.Utilities.SceneLoader.ReloadSceneAsync();
    }
}
