using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawBearryAI : MonoBehaviour {

    public EnemyStats myStats;
    
    
    public float wanderRadius = 5.0f;

    Transform target;
    Vector2 wanderTarget;
    public float wanderHeightMax, wanderHeightMin;
    Transform LeftBound, RightBound;
    
    public CircleCollider2D searchCollider;
    public CircleCollider2D feetCollider;
    public Rigidbody2D bearRB;
    public Animator bearAnim, feetAnim;
    public SpriteRenderer bearRend, feetRend;
    public Transform attackCenter;
    public bool knockedBack;
    public float knockBackTime;
    public Sprite happySprite;
    public CameraMovement cam;

    // Tracking variables
    Vector2 dir;
    Rigidbody2D playerRB;

    // Wandering variables
    public float waitMin, waitMax, walkMin, walkMax;
    float timeSpentWaiting, currentWait, TimeSpentWalking, currentWalk;
    bool waiting = false, attacking = false, startedCheer = false, startAI = false;
    public bool cheered = false;

    public AudioSource source;
    public AudioClip clip;

    private void Start()
    {
        Transform camT = Camera.main.transform;
        LeftBound = camT.FindChild("LeftBound");
        RightBound = camT.FindChild("RightBound");
        cam = camT.GetComponent<CameraMovement>();

        //Vector2 distFromRight = (Vector2)RightBound.position - bearRB.position;
        //Vector2 distFromLeft = (Vector2)LeftBound.position - bearRB.position;
        float x = RightBound.position.x - 3;
        wanderTarget.Set(x, bearRB.position.y);
        currentWait = 10;
        TimeSpentWalking = 0;
    }

    private void FixedUpdate()
    {
        if (!knockedBack)
        {
            if (startAI)
            {
                if (!cheered)
                {
                    if (!waiting)
                    {
                        if (target)
                        {
                            dir.Set(playerRB.position.x - bearRB.position.x, playerRB.position.y - bearRB.position.y);

                            if ((dir.x > 0 && bearRend.flipX) || (dir.x < 0 && !bearRend.flipX))
                            {
                                bearRend.flipX = !bearRend.flipX;
                                feetRend.flipX = !feetRend.flipX;

                                float yRot = bearRend.flipX ? 180f : 0f;
                                attackCenter.rotation = Quaternion.Euler(0f, yRot, 0f);
                            }
                            if (dir.magnitude < 1.5f)
                            {
                                feetAnim.SetBool("Walking", false);
                                waiting = true;
                                timeSpentWaiting = 0f;
                                currentWait = 10f;
                                if(!attacking)
                                    StartCoroutine(Attack());
                            }
                            else
                            {
                                dir.Normalize();
                                dir.Set(bearRB.position.x + (dir.x * myStats.movementSpeed * Time.fixedDeltaTime), bearRB.position.y + (dir.y * myStats.movementSpeed * Time.fixedDeltaTime));

                                bearRB.MovePosition(dir);
                            }
                        }
                        else
                        {
                            if (TimeSpentWalking == 0)
                            {
                                do
                                {
                                    wanderTarget.Set(Random.Range(-wanderRadius, wanderRadius), Random.Range(-wanderRadius, wanderRadius));
                                } while ((wanderTarget.y > wanderHeightMax || wanderTarget.y < wanderHeightMin) && (wanderTarget.x < LeftBound.position.x + 1f || wanderTarget.x > RightBound.position.x - 1f));
                            }

                            dir.Set(wanderTarget.x - bearRB.position.x, wanderTarget.y - bearRB.position.y);

                            if ((dir.x > 0 && bearRend.flipX) || (dir.x < 0 && !bearRend.flipX))
                            {
                                bearRend.flipX = !bearRend.flipX;
                                feetRend.flipX = !feetRend.flipX;

                                float yRot = bearRend.flipX ? 180f : 0f;
                                attackCenter.rotation = Quaternion.Euler(0f, yRot, 0f);
                            }
                            if (dir.magnitude < 0.1f)
                            {
                                feetAnim.SetBool("Walking", false);
                                waiting = true;
                                TimeSpentWalking = 0;
                                timeSpentWaiting = 0;
                                currentWait = Random.Range(waitMin, waitMax);
                            }
                            dir.Normalize();
                            dir.Set(bearRB.position.x + (dir.x * myStats.movementSpeed * Time.fixedDeltaTime), bearRB.position.y + (dir.y * myStats.movementSpeed * Time.fixedDeltaTime));

                            bearRB.MovePosition(dir);
                        }
                    }
                    if (waiting && !attacking)
                    {
                        timeSpentWaiting += Time.fixedDeltaTime;
                        if (timeSpentWaiting > currentWait)
                        {
                            feetAnim.SetBool("Walking", true);
                            waiting = false;
                            currentWalk = Random.Range(walkMin, walkMax);
                        }
                    }
                    else if (!waiting && !attacking)
                    {
                        TimeSpentWalking += Time.fixedDeltaTime;
                        if (TimeSpentWalking > currentWalk)
                        {
                            if (target)
                                StartCoroutine(Attack());
                            else
                            {
                                feetAnim.SetBool("Walking", false);
                                waiting = true;
                                timeSpentWaiting = 0f;
                                TimeSpentWalking = 0f;
                                currentWait = Random.Range(waitMin, waitMax);
                            }
                        }
                    }
                }
                else
                {

                    if (startedCheer == false)
                        LeaveCheerfully();
                    startedCheer = true;

                    dir.Set(wanderTarget.x - bearRB.position.x, wanderTarget.y - bearRB.position.y);

                    if ((dir.x > 0 && bearRend.flipX) || (dir.x < 0 && !bearRend.flipX))
                    {
                        bearRend.flipX = !bearRend.flipX;
                        feetRend.flipX = !feetRend.flipX;

                        float yRot = bearRend.flipX ? 180f : 0f;
                        attackCenter.rotation = Quaternion.Euler(0f, yRot, 0f);
                    }


                    dir.Normalize();
                    dir.Set(bearRB.position.x + (dir.x * myStats.movementSpeed * Time.fixedDeltaTime), bearRB.position.y + (dir.y * myStats.movementSpeed * Time.fixedDeltaTime));
                    bearRB.MovePosition(dir);

                    if (bearRB.position.x > RightBound.position.x + 0.5f || bearRB.position.x < LeftBound.position.x - 0.5f)
                    {
                        Destroy(bearRB.gameObject);
                    }
                }
            }
            else
            {

                dir.Set(wanderTarget.x - bearRB.position.x, wanderTarget.y - bearRB.position.y);

                if ((dir.x > 0 && bearRend.flipX) || (dir.x < 0 && !bearRend.flipX))
                {
                    bearRend.flipX = !bearRend.flipX;
                    feetRend.flipX = !feetRend.flipX;

                    float yRot = bearRend.flipX ? 180f : 0f;
                    attackCenter.rotation = Quaternion.Euler(0f, yRot, 0f);
                }

                if (dir.magnitude < 0.1f)
                {
                    feetAnim.SetBool("Walking", false);
                    waiting = true;
                    TimeSpentWalking = 0;
                    timeSpentWaiting = 0;
                    currentWait = Random.Range(waitMin, waitMax);
                    startAI = true;
                }

                dir.Normalize();
                dir.Set(bearRB.position.x + (dir.x * myStats.movementSpeed * Time.fixedDeltaTime), bearRB.position.y + (dir.y * myStats.movementSpeed * Time.fixedDeltaTime));
                bearRB.MovePosition(dir);
            }
        }
        else
        {
            knockBackTime += Time.fixedDeltaTime;
            if (knockBackTime >= 0.5f)
                knockedBack = false;
        }
        if(bearRB.transform.position.y >= 1.5f ||
            bearRB.transform.position.y <= -4.5f ||
            bearRB.transform.position.x <= cam.currentLeftBound - 10 ||
            bearRB.transform.position.x >= cam.currentRightBound + 10)
        {
            if(!cheered)
                FindObjectOfType<GameManager>().currentEnemyCount--;
            Destroy(bearRB.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !cheered)
        {
            startAI = true;
            target = collision.transform;
            searchCollider.radius += 1;
            playerRB = target.GetComponent<Rigidbody2D>();
            waiting = false;
            feetAnim.SetBool("Walking", true);
            TimeSpentWalking = 0;
            currentWalk = Random.Range(walkMin, walkMax);
            timeSpentWaiting = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !cheered)
        {
            target = null;
            searchCollider.radius -= 1;
            waiting = true;
            feetAnim.SetBool("Walking", true);
            timeSpentWaiting = 0;
            TimeSpentWalking = 0;
            currentWait = Random.Range(waitMin, waitMax);
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        bearAnim.SetTrigger("Attack");
        if(!source.isPlaying)
            source.PlayOneShot(clip);
        while (bearAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            yield return null;
        }

        attacking = false;
        waiting = false;
        timeSpentWaiting = 0f;
        currentWalk = Random.Range(walkMin, walkMax);

        TimeSpentWalking = 0f;
    }

    void LeaveCheerfully()
    {
        // Set sprite to happy Animation?
        bearAnim.SetTrigger("Cheer");
        FindObjectOfType<GameManager>().currentEnemyCount--;
        // Find Game Manager and decrease current enemy count
        // Turn target to leave
        feetAnim.SetBool("Walking", true);
        //Vector2 distFromRight = (Vector2)RightBound.position - bearRB.position;
        //Vector2 distFromLeft = (Vector2)LeftBound.position - bearRB.position;
        float x = LeftBound.position.x - 10;
        wanderTarget.Set(x, bearRB.position.y);
        // set super huge walk time
        currentWalk = 100f;
        // turn physical collider off
        feetCollider.isTrigger = true;
    }
}
