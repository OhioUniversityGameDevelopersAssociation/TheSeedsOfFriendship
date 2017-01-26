using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour {

    public int maxHealth;
    public int health;
    public float movementSpeed;
    public int damage;

    public GameObject healthUI;
    public Image healthBar;

    public StrawBearryAI bear;
    public TomatogatorAI gator;
    public OctonanaAI octo;

    public SpriteRenderer body, feet;
    public Color damageColor;
    public bool invulnrabilityFrames;

    public AudioSource source;
    public AudioClip clip;

    public bool isBanana = false;

    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("Cheered");
            CheerUp();
        }
    }

    void CheerUp()
    {
        //if(!source.isPlaying)
        //    source.PlayOneShot(clip);
        // Grab the movement controller and walk off screen
        if (bear)
            bear.cheered = true;
        if (gator)
            gator.cheered = true;
        if (octo)
            octo.cheered = true;
    }

    public void Damage(int damageTaken)
    {
        if (!invulnrabilityFrames)
        {
            health -= damageTaken;
            if (isBanana)
                octo.UpdateHealth();
            StopAllCoroutines();
            StartCoroutine(InvulnrabilityFrames());
        }
    }

    IEnumerator InvulnrabilityFrames()
    {
        invulnrabilityFrames = true;
        WaitForSeconds flashTime = new WaitForSeconds(0.05f);
        body.color = damageColor;
        feet.color = damageColor;
        for (int i = 0; i < 2; i++)
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

    IEnumerator UpdateHealthUI()
    {
        healthUI.SetActive(true);
        healthBar.fillAmount = 1f - ((float)health / (float)maxHealth);
        healthBar.color = Color.Lerp(Color.red, Color.green, healthBar.fillAmount);
        yield return new WaitForSeconds(2.0f);
        healthUI.SetActive(false);
        
    }
}
