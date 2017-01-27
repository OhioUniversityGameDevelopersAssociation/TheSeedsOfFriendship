using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {

    public int health, maxHealth;
    public SpriteRenderer body, feet;
    public Color damageColor, maxHealthColor, minHealthColor;

    public Image healthBar;

    bool invulnrabilityFrames = false;

    public AudioSource source;
    public AudioClip clip;

    private void Start()
    {
        UpdateUI();
    }

    public void Damage(int damageTaken)
    {
        if(!invulnrabilityFrames)
        {
            health -= damageTaken;
            source.PlayOneShot(clip);
            if(health <= 0)
            {
                SceneManager.LoadScene("Level 1");
            }
            UpdateUI();
            StopAllCoroutines();
            StartCoroutine(InvulnrabilityFrames());
        }
    }

    void UpdateUI()
    {
        float healthPercentage = (float)health / (float)maxHealth;
        healthBar.fillAmount = healthPercentage;
        healthBar.color = Color.Lerp(minHealthColor, maxHealthColor, healthPercentage);

    }

    IEnumerator InvulnrabilityFrames()
    {
        invulnrabilityFrames = true;
        WaitForSeconds flashTime = new WaitForSeconds(0.05f);
        body.color = damageColor;
        feet.color = damageColor;
        for (int i = 0; i < 6; i++)
        {
            body.enabled = false;
            feet.enabled = false;
            yield return flashTime;
            body.enabled = true;
            feet.enabled = true;
            yield return flashTime;
            body.color = Color.white;
            feet.color = Color.white;
        }
        invulnrabilityFrames = false;
    }
}
