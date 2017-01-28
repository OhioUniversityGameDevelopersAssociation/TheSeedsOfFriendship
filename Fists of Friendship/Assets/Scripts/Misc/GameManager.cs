using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [System.Serializable]
    public struct Encounters
    {
        public float encounterTrigger;
        public float leftBound;
        public float rightBound;

        [System.Serializable]
        public struct Wave
        {
            public int numberOfStrawBearrys;
            public int numberOfTomatogators;
            public float waveWait;
        }
        public Wave[] waves;
    }


    public CameraMovement cam;
    private Rigidbody2D camRB;
    public GameObject Strawbearry, Tomatogator;
    public Transform LeftBound, RightBound;

    public int currentEncounter = 0;
    public Encounters[] encounters;
    public int currentEnemyCount;
    bool currentlyInEncounter = false;
    bool finishedEncounters = false;

    public Image arrow;
    public AudioSource source;

    private void Update()
    {
        if (cam.transform.position.x > encounters[currentEncounter].encounterTrigger && !currentlyInEncounter && !finishedEncounters)
        {
            cam.EnterEncounter(encounters[currentEncounter].leftBound, encounters[currentEncounter].rightBound);
            StartCoroutine(EnterEncounter());
        }
        //These controls are only meant for Debugging, so commenting them out for now 
        /*if(Input.GetKeyDown(KeyCode.K))
        {
            GameObject[] crocs = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in crocs)
            {
                Destroy(enemy);
            }
            currentEnemyCount = 0;
        }*/
    }

    IEnumerator EnterEncounter()
    {
        currentlyInEncounter = true;
        float xPositioner, yPositioner;
        foreach (Encounters.Wave wave in encounters[currentEncounter].waves)
        {
            for (int i = 0; i < wave.numberOfStrawBearrys; i++)
            {
                xPositioner = Random.Range(0, 1) == 1 ? LeftBound.position.x - 1f : RightBound.position.x + 1f;
                yPositioner = Random.Range(-3f, 0f);
                StrawBearryAI b = Instantiate(Strawbearry, new Vector3(xPositioner, yPositioner, 0f), Quaternion.identity).GetComponentInChildren<StrawBearryAI>();
                b.cheered = false;
                currentEnemyCount++;

                yield return new WaitForSeconds(0.5f);
            }
            for (int i = 0; i < wave.numberOfTomatogators; i++)
            {
                xPositioner = Random.Range(0, 1) == 0 ? LeftBound.position.x - 1f : RightBound.position.x + 1f;
                yPositioner = Random.Range(-3f, 0f);
                Instantiate(Tomatogator, new Vector3(xPositioner, yPositioner, 0f), Quaternion.identity);
                currentEnemyCount++;
                yield return new WaitForSeconds(0.5f);
            }

            while (currentEnemyCount > 0)
            {
                yield return null;
            }
            yield return new WaitForSeconds(wave.waveWait);
        }
        cam.ExitEncounter();
        StartCoroutine(FlashArrow());
        if (currentEncounter != 2)
            currentEncounter++;
        else
        {
            finishedEncounters = true;
            FindObjectOfType<CameraMovement>().EnterEncounter(10f, 107.5f);
        }
        currentlyInEncounter = false;
    }

    IEnumerator FlashArrow()
    {
        WaitForSeconds wait = new WaitForSeconds(0.25f);
        for (int i = 0; i < 4; i++)
        {
            arrow.enabled = true;
            source.Play();
            yield return wait;
            arrow.enabled = false;
            yield return wait;
        }
    }
}
