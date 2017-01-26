using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject happyAnimals;
    public string startSceneName;
    public GameObject credits;
    public GameObject creditsButton, backButton;

    private void Start()
    {
        if(PlayerPrefs.GetInt("BeatenOnce") == 1)
        {
            happyAnimals.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startSceneName);
    }

    public void Credits()
    {
        credits.SetActive(true);
        eventSystem.SetSelectedGameObject(backButton);
    }

    public void ExitCredits()
    {
        credits.SetActive(false);
        eventSystem.SetSelectedGameObject(creditsButton);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void AdjustAudio(AudioSource source)
    {
        source.volume = Random.Range(0.7f, 0.9f);
        source.pitch = Random.Range(0.8f, 1.2f);
    }
}
