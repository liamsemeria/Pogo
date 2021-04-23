using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SLASH 19

public class slash : MonoBehaviour
{
    // script for all player movement, slash move, animations, and death logic
    public Animator animator;
    private Animator playerA;
    public float moveSpeed; // was 5
    public int slashPower;
    public Transform slashpt;
    public Transform hitbox;
    // inputs for all moves, (so they can be changed later in setting)
    public static KeyCode Left, Right, Up, Down;
    public static KeyCode MoveLeft, MoveRight;
    public static KeyCode Slash;

    // vectors for player aim
    Vector2 netpt;
    Vector2 frozenpt;
    Vector2 playerpt;
    // constants for left and right inputs
    Vector2 slightRight;
    Vector2 slightLeft;
    // players components rigidbodym collider
    Rigidbody2D rb;
    Collider2D colhead;
    Collider2D colfeet;

    // sounds
    public AudioSource swipe;
    public AudioSource hit;
    public AudioSource death;

    // slashing
    bool slashing;
    bool buffering;
    bool slashmoving;
    int frame;
    float stime;
    int slashmoveframe;
    float smoveTime;

    bool canTranslate = true;
    bool movingLeft, movingRight;
    // inputs
    public static bool inputAim;
    // dying
    public static bool dying = false;
    float dyingTime;
    public static Vector3 startpt;

    // conigure slash location based on arrow keys and handle slash hitbox and animation
    void Start()
    {

        animator.SetBool("slashing", false);
        slightRight = new Vector2(.75f, 0);// .5 0
        slightLeft = new Vector2(-.75f, 0);// -.5 0
        netpt = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();
        playerA = GetComponent<Animator>();
        colhead = GetComponent<Collider2D>();
        colfeet = GetComponents<Collider2D>()[1];
        Left = KeyCode.RightArrow;
        Right = KeyCode.LeftArrow;
        Up = KeyCode.DownArrow;
        Down = KeyCode.UpArrow;
        MoveLeft = KeyCode.LeftArrow;
        MoveRight = KeyCode.RightArrow;
        Slash = KeyCode.C;

        moveSpeed = 3.95f;
        slashPower = 549;

        frame = 0;
        stime = 0;
        slashmoveframe = 0;
        dyingTime = 0;
        slashing = false;
        slashmoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        // TEST
        if (slashmoving) canTranslate = false;
        else if (!dying) canTranslate = true;


        //if (Input.GetAxis("HorizontalAim") > 0) Debug.Log("right");
        //if (Input.GetAxis("HorizontalAim") < 0) Debug.Log("left");
        // toggle death mode
        // change idle animation based on facing
        if (Input.GetKey(Down)) playerA.SetBool("facingup",true);
        else playerA.SetBool("facingup", false);
        if (Input.GetKey(Up)) playerA.SetBool("facingdown", true);
        else playerA.SetBool("facingdown", false);

        //dying logic

        if (dying)
        {
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            canTranslate = false;
            dyingTime += Time.deltaTime;
            if (dyingTime > .3f)
            {
                transform.position = startpt;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                canTranslate = true;
                dying = false;
                dyingTime = 0;
            }
        }
        Camera cam = Camera.main;


        // update inputAim based on inputMode
        inputAim = Input.GetKey(Up) || Input.GetKey(Down) ||
        Input.GetKey(Left) || Input.GetKey(Right);

        // move left and right
        if (canTranslate)
        {
            // TODO try to adjust the translate using the rigidnody
            if (Input.GetKey(MoveRight)) transform.Translate(Time.deltaTime * moveSpeed, 0f, 0f);
            if (Input.GetKey(MoveLeft)) transform.Translate(Time.deltaTime * -moveSpeed, 0f, 0f);
        }
        netpt = Vector2.zero;
        // find the orientation for the slash point by adding up the directional inputs
        if (Input.GetKey(Up))
        {
            netpt += Vector2.up;
        }
        if (Input.GetKey(Left))
        {
            netpt += slightLeft;
        }
        if (Input.GetKey(Down))
        {
            netpt += Vector2.down;
        }
        if (Input.GetKey(Right))
        {
            netpt += slightRight;
        }

        // reseting values for slash point
        if (!inputAim)
        {
            netpt = Vector2.zero;
        }
        slashpt.position = Vector2.zero;
        netpt.Normalize();
        playerpt = transform.position;
        slashpt.position = playerpt + netpt;
        // hitbox for slash animation, set position and rotation
        // note: animation and hitbox is seperate so that the animation freezes when slashing but the hitbox doesnt
        animator.gameObject.transform.position = playerpt + frozenpt; 
        hitbox.position = playerpt + netpt;

        // set rotation for animation and its hitbox
        hitbox.gameObject.transform.up = transform.position - hitbox.gameObject.transform.position;
        animator.gameObject.transform.up = transform.position - animator.gameObject.transform.position;

        // flip sprite based on movement
        playerA.SetFloat("velocity", rb.velocity.x);
        if (rb.velocity.x == 0)
        {
            if (Input.GetKey(MoveRight))
            {
                //playerA.SetFloat("velocity", 1);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            if (Input.GetKey(MoveLeft))
            {
                //playerA.SetFloat("velocity", -1);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        // slash input
        if (Input.GetKeyDown(Slash) && inputAim && !buffering && !slashing && !slashmoving)
        {
            frozenpt = netpt;
            buffering = true;
            animator.Play("slashPlayer");
        }

        // a hit
        if ((slashReader.canJump) && (!slashing) && buffering && !slashmoving)
        {
            buffering = false;
            //slashCam.shake(3);
            //animator.Play("slashPlayer");
            // randomize the sound affect
            hit.pitch = 1 + Random.Range(.15f,.65f);
            hit.Play();
            slashing = true;
            slashmoving = true;
            rb.velocity *= Vector2.right * .77f;
            slashReader.canJump = false;
            // was 450
            rb.AddForce(netpt * -slashPower);
            netpt = Vector2.zero; // ADDED
        }

        if (buffering)
        {
            frame++;
            stime += Time.deltaTime;
        }
        // a miss
        if ((stime > .085)&& buffering) // 10
        {
            frame = 0;
            stime = 0;
            buffering = false;
        }
        if (slashing)
        {
            frame++;
            stime += Time.deltaTime;
        }
        // lag for slashing a successful hit
        if (stime > .17) // 40
        {
            slashing = false;
            frame = 0;
            stime = 0;
        }
        if (slashmoving)
            smoveTime += Time.deltaTime;//
            //slashmoveframe++;
            

        if (smoveTime > .188f) // 45
        {
            
            slashmoving = false;
            slashmoveframe = 0;
            smoveTime = 0;
            rb.velocity *= .4f;
        }


        // fall faster
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = 3.2f;
        }
        else if (slashmoving)
        {
            rb.gravityScale = 0;
        }
        else rb.gravityScale = 1.8f;
    }

    // kill the player on hitting a killing object
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // die from collisions with kill colliders
        if ((collision.gameObject.tag == "kill") || (collision.gameObject.tag == "unslash"))
        {
                dying = true;
                playerA.Play("die");
                death.Play();
            sceneLoader.counterincr(false);
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // collect trinket
        if (collision.gameObject.tag == "trink")
        {
            death.Play();
            Destroy(collision.gameObject);
            sceneLoader.counterincr(true);
        }
    }

}
