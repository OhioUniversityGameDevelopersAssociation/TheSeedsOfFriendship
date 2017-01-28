using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{

    // Movement
    public float movementSpeed = 1f;

    private Rigidbody2D playerRB;
    private SpriteRenderer rend;
    private bool facingRight = true;
    Vector2 move;
    public Animator feetAnim;
    public Animator bodyAnim;
    public SpriteRenderer feetRend;

    // Attacking

    public GameObject smiles;
    public float smileCooldown = 0.5f;
    float timeSinceLastSmile = 100f;
    public float smileSpeed = 5f;

    public Transform emoticonCenter;
    public Animator emoticonAnim;
    public float emoticonCooldown = 0.5f;
    float timeSinceLastEmoticon = 100f;

    public AudioClip slice, lemon;
    public AudioSource source;




    // Use this for initialization
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        AccountForFlip();

        Attack();

        if (Input.GetButtonDown("Exit"))
            SceneManager.LoadScene("MainMenu");
    }

    void Move()
    {
        move.x = (Input.GetAxis("Horizontal") * movementSpeed * Time.fixedDeltaTime) + playerRB.position.x;
        move.y = (Input.GetAxis("Vertical") * movementSpeed * Time.fixedDeltaTime) + playerRB.position.y;

        playerRB.MovePosition(move);

        if (feetAnim.GetBool("Walking") == false && (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f))
            feetAnim.SetBool("Walking", true);
        else if (feetAnim.GetBool("Walking") == true && (Input.GetAxis("Horizontal") == 0f && Input.GetAxis("Vertical") == 0f))
            feetAnim.SetBool("Walking", false);
    }

    void AccountForFlip()
    {
        //TODO: Account for animated hand??
        if ((Input.GetAxis("Horizontal") > 0f && facingRight == false) || (Input.GetAxis("Horizontal") < 0f && facingRight == true)) // If we are facing left and started going right
        {
            // Flip
            rend.flipX = !rend.flipX;
            facingRight = !facingRight;
            float yRot = facingRight ? 0f : 180f;
            emoticonCenter.rotation = Quaternion.Euler(0f, yRot, 0f);
            feetRend.flipX = !feetRend.flipX;
        }
    }

    void Attack()
    {
        if (!bodyAnim.GetCurrentAnimatorStateInfo(0).IsName("Orangutan - Wave"))
        {
            if (timeSinceLastSmile <= smileCooldown)
            {
                timeSinceLastSmile += Time.fixedDeltaTime;
            }
            if (Input.GetButton("Smiley") && timeSinceLastSmile > smileCooldown)
            {
                float relativeX = facingRight ? gameObject.transform.position.x : gameObject.transform.position.x;
                float smileyForce = facingRight ? smileSpeed : -smileSpeed;
                Rigidbody2D smileRB = Instantiate(smiles, new Vector3(relativeX, transform.position.y + 0.6f, 0f), Quaternion.identity).GetComponent<Rigidbody2D>();
                smileRB.GetComponent<SpriteRenderer>().flipX = smileyForce > 0;
                smileRB.velocity = new Vector2(smileyForce, 0f);
                timeSinceLastSmile = 0f;
                bodyAnim.SetTrigger("Attack");
                source.PlayOneShot(lemon);
            }

            if (timeSinceLastEmoticon <= emoticonCooldown)
            {
                timeSinceLastEmoticon += Time.fixedDeltaTime;
            }
            if (Input.GetButton("Emoticon") && timeSinceLastEmoticon > emoticonCooldown)
            {
                emoticonAnim.SetTrigger("Attack");
                timeSinceLastEmoticon = 0f;
                bodyAnim.SetTrigger("Slice");
                source.PlayOneShot(slice);
            }
        }

    }
}
