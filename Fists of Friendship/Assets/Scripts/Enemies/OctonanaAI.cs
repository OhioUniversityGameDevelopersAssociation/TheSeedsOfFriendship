using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OctonanaAI : MonoBehaviour
{

    public bool cheered = false;
    public float waitTime = 1.5f;

    bool charging = false;
    public float chargeTime = 2.0f;
    float timeSpentCharging = 0f;

    public SpriteRenderer bod, face, tents;

    public Sprite[] bodSprites, faceSprites, tentsSprites;

    public Color miffedColor;

    public Rigidbody2D banRB, tentaclesRB;

    public float forceAmount;

    bool fired = false;
    bool won = false;

    public EnemyStats myStats;

    public string winScene;

    public GameObject winText;

    public float stopVelocity;
    float startFireVelocity;

    public Rigidbody2D playerRB;

    public Collider2D attackCol;

    public AudioSource source;

    public enum Stages
    {
        Chill,
        Warmup,
        Attack
    }

    public Stages stages = Stages.Chill;

    private void Start()
    {
        StartCoroutine(BeginAI());
    }

    private void Update()
    {
        if (charging && !cheered)
        {
            timeSpentCharging += Time.deltaTime;
            bod.color = Color.Lerp(Color.white, miffedColor, timeSpentCharging / chargeTime);
            tents.color = Color.Lerp(Color.white, miffedColor, timeSpentCharging / chargeTime);
            if (timeSpentCharging > chargeTime)
                charging = false;
        }
        if (fired)
        {
            bod.color = Color.Lerp(Color.white, miffedColor, banRB.velocity.magnitude / startFireVelocity);
            tents.color = Color.Lerp(Color.white, miffedColor, banRB.velocity.magnitude / startFireVelocity);
            attackCol.enabled = banRB.velocity.magnitude > 0.75f;
            tentaclesRB.angularVelocity = banRB.velocity.magnitude * 40;
            tentaclesRB.transform.localPosition = Vector2.zero;
            source.pitch = Mathf.Lerp(0.8f, 1f, startFireVelocity);
            if (banRB.velocity.magnitude < stopVelocity)
                source.Stop();
        }

        tentaclesRB.transform.localPosition = Vector2.zero;


        if (cheered && !won)
        {
            won = true;
            StopAllCoroutines();

            face.sprite = faceSprites[3];
            bod.sprite = bodSprites[3];
            tents.enabled = false;
            winText.SetActive(true);
            Invoke("Win", 5.0f);
        }

    }

    public void UpdateHealth()
    {

        if (myStats.health >= myStats.maxHealth * (7f / 8f)) //80ish
        {
            tents.sprite = tentsSprites[0];
        }
        else if (myStats.health >= myStats.maxHealth * (6f / 8f)) // 75
        {
            tents.sprite = tentsSprites[1];
            bod.sprite = bodSprites[0];
        }
        else if (myStats.health >= myStats.maxHealth * (5f / 8f))
        {
            tents.sprite = tentsSprites[2];
        }
        else if (myStats.health >= myStats.maxHealth * (4f / 8f)) // 50
        {
            tents.sprite = tentsSprites[3];
            bod.sprite = bodSprites[1];
        }
        else if (myStats.health >= myStats.maxHealth * (3f / 8f))
        {
            tents.sprite = tentsSprites[4];
        }
        else if (myStats.health >= myStats.maxHealth * (2f / 8f)) //25
        {
            tents.sprite = tentsSprites[5];
            bod.sprite = bodSprites[2];
        }
        else if (myStats.health >= myStats.maxHealth * (1f / 8f))
        {
            tents.sprite = tentsSprites[6];
        }
        else
        {
            tents.sprite = tentsSprites[7];
        }

    }

    private IEnumerator BeginAI()
    {
        while (!cheered)
        {
            //Chill
            yield return new WaitForSeconds(waitTime);

            charging = true;
            face.sprite = faceSprites[1];

            while (charging)
            {
                yield return null;
            }
            timeSpentCharging = 0f;

            int chance = Random.Range(1, 3);

            Vector2 force = chance == 3 ? new Vector2(Random.Range(-1f, 1f), Random.Range(-.25f, .25f)) : playerRB.position - banRB.position;
            force.Normalize();
            //FIRE
            banRB.AddForce(force * forceAmount, ForceMode2D.Impulse);
            startFireVelocity = banRB.velocity.magnitude;
            fired = true;
            face.sprite = faceSprites[2];
            source.Play();
            while (banRB.velocity.magnitude >= stopVelocity)
            {
                yield return null;
            }

            fired = false;

            face.sprite = faceSprites[0];

        }
    }

    void Win()
    {
        PlayerPrefs.SetInt("BeatenOnce", 1);
        PlayerPrefs.SetInt("JustBeaten", 1);
        SceneManager.LoadScene(winScene);
    }
}
