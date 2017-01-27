using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public EventSystem eventSystem;
    public GameObject happyAnimals;
    public string startSceneName;
    public GameObject mainMenu;
    public GameObject creditsButton, backButton;

    public Image screenFade;
    public SpriteRenderer[] fruitCakeSprites, watermelonSprites;
    private bool fadeToBlack;
    private bool currentlyOnFruitcake;

    private void Start()
    {
        if (PlayerPrefs.GetInt("BeatenOnce") == 1)
        {
            happyAnimals.SetActive(true);
        }
        if (PlayerPrefs.GetInt("JustBeaten") == 1)
        {
            Credits();
            PlayerPrefs.SetInt("JustBeaten", 0);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startSceneName);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        eventSystem.SetSelectedGameObject(backButton);
        StartCoroutine(CreditsScrolling());
    }

    public void ExitCredits()
    {
        mainMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(creditsButton);
        StopAllCoroutines();
    }

    public void Quit()
    {
        PlayerPrefs.SetInt("BeatenOnce", 0);
        PlayerPrefs.SetInt("JustBeaten", 0);
        Application.Quit();
    }

    public void AdjustAudio(AudioSource source)
    {
        source.volume = Random.Range(0.7f, 0.9f);
        source.pitch = Random.Range(0.8f, 1.2f);
    }

    // It would be nice to simply turn these off, but then our paralax/scrolling scripts wouldn't work.
    // We'll need to disable the sprites, and have a black cover fade in between the switch
    IEnumerator CreditsScrolling()
    {
        // Randomizing the background, so it doesn't always have the same one
        if (Random.value >= 0.5)
            currentlyOnFruitcake = true;
        else
            currentlyOnFruitcake = false;

        // Turn the correct starting renderers on and off
        foreach (SpriteRenderer rend in fruitCakeSprites)
        {
            rend.enabled = currentlyOnFruitcake;
        }
        foreach (SpriteRenderer rend in watermelonSprites)
        {
            rend.enabled = !currentlyOnFruitcake;
        }


        while (true)
        {
            if (mainMenu.activeSelf)
            {
                break;
            }

            yield return new WaitForSeconds(Random.Range(5f, 8f));

            float t = 0;


            while (t < 1)
            {
                t += Time.deltaTime;
                screenFade.color = Color.Lerp(Color.clear, Color.black, t);
                yield return null;
            }

            currentlyOnFruitcake = !currentlyOnFruitcake;

            // Turn the correct starting renderers on and off
            foreach (SpriteRenderer rend in fruitCakeSprites)
            {
                rend.enabled = currentlyOnFruitcake;
            }
            foreach (SpriteRenderer rend in watermelonSprites)
            {
                rend.enabled = !currentlyOnFruitcake;
            }

            yield return new WaitForSeconds(0.5f);

            while (t > 0)
            {
                t -= Time.deltaTime;
                screenFade.color = Color.Lerp(Color.clear, Color.black, t);
                yield return null;
            }
        }
    }
}
