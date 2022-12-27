using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;

    public float moveSpeed;
    public float jumpForce;

    // For ground check, avoid flying with Jump
    public Transform groundPoint;
    private bool isOnGround;
    public LayerMask whatIsGround;

    public Animator animator;

    public BulletController shotToFire;
    public Transform shotPoint;

    private bool canDounleJump;

    public float dashSpeed;
    public float dashTime;
    private float dashCounter;

    public SpriteRenderer theSR;
    public SpriteRenderer afterImage;
    public float afterImageLifetime;
    public float timeBetweenAfterImages;
    private float afterImageCounter;
    public Color afterImageColor;

    public float waitAfterDashing;
    private float dashRechargeCounter;

    public GameObject standing;
    public GameObject ball;
    public float waitToBall;
    private float ballCounter;
    public Animator ballAnimator;

    public Transform bombPoint;
    public GameObject bomb;

    private PlayerAbilityTracker abilities;

    // Start is called before the first frame update
    void Start()
    {
        abilities = GetComponent<PlayerAbilityTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dashRechargeCounter > 0)
        {
            dashRechargeCounter -= Time.deltaTime;
        }
        else
        {
            if (Input.GetButtonDown("Fire2") && standing.activeSelf && abilities.canDash)
            {
                dashCounter = dashTime;

                ShowAfterImage();
;            }
        }

        if (dashCounter > 0)
        {
            // delta = 1/60 (60 fps) of a second
            dashCounter = dashCounter - Time.deltaTime;

            theRB.velocity = new Vector2(dashSpeed * transform.localScale.x, theRB.velocity.y);

            afterImageCounter -= Time.deltaTime;
            if (afterImageCounter <= 0)
            {
                ShowAfterImage();
            }

            dashRechargeCounter = waitAfterDashing;
        }
        else
        {
            // only move if not dashing...

            // move sideways
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, theRB.velocity.y);

            // handle direction
            if (theRB.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (theRB.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
            }
        }


        // check if on the ground
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);

        // jumping
        if (Input.GetButtonDown("Jump") && (isOnGround || (canDounleJump && abilities.canDoubleJump)))
        {
            if (isOnGround)
            {
                canDounleJump = true;
            }
            else
            {
                canDounleJump = false;

                animator.SetTrigger("doubleJump");
            }

            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }

        // shooting
        if (Input.GetButtonDown("Fire1"))
        {
            if (standing.activeSelf)
            {
                Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0);

                animator.SetTrigger("shotFired");
            }
            else if (ball.activeSelf && abilities.canDropBomb)
            {
                Instantiate(bomb, bombPoint.position, bombPoint.rotation);
            }
        }

        // ball mode
        if (!ball.activeSelf)
        {
            if (Input.GetAxisRaw("Vertical") < -.9f && abilities.canBecomeBall)
            {
                ballCounter -= Time.deltaTime;
                if (ballCounter <= 0)
                {
                    ball.SetActive(true);
                    standing.SetActive(false);
                }
            }
            else
            {
                ballCounter = waitToBall;
            }
        }
        else
        {
            if (Input.GetAxisRaw("Vertical") > .9f)
            {
                ballCounter -= Time.deltaTime;
                if (ballCounter <= 0)
                {
                    ball.SetActive(false);
                    standing.SetActive(true);
                }
            }
            else
            {
                ballCounter = waitToBall;
            }
        }

        if (standing.activeSelf)
        {

            animator.SetBool("isOnGround", isOnGround);
            animator.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        }

        if (ball.activeSelf)
        {
            ballAnimator.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        }
    }

    public void ShowAfterImage()
    {
        var image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = theSR.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;

        Destroy(image.gameObject, afterImageLifetime);

        afterImageCounter = timeBetweenAfterImages;
    }
}
