using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float verticalSpeed = 6f;
    [SerializeField] float climbingSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(5f, 5f);
    [SerializeField] AudioClip jumpSFX;

    // State
    bool isAlive = true;
    bool isTouchingGround = false;
    bool isClimbing = false;

    // Cached component references
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Animator animator;
    float gravityScaleAtStart;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    // Message then methods

    void Update()
    {
        if (!isAlive) { return; }

        Run();
        Jump();
        ClimbLadder();
        Swimming();
        Die();
        FlipSprite();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");   // value is between -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        animator.SetBool("IsRunning", playerHasHorizontalSpeed());

    }

    private void ClimbLadder()
    {
        isClimbing = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        animator.SetBool("IsClimbing", isClimbing);

        if (!isClimbing) {
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbingVertically = new Vector2(myRigidBody.velocity.x, controlThrow * climbingSpeed);
        myRigidBody.gravityScale = 0f;
        myRigidBody.velocity = climbingVertically;


    }

    private void Jump()
    {
        isTouchingGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (CrossPlatformInputManager.GetButtonDown("Jump") && isTouchingGround) {
            Vector2 jumpVelocityToAdd = new Vector2(0f, verticalSpeed);
            myRigidBody.velocity = jumpVelocityToAdd;
            AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position);
        }
    }
    
    private void FlipSprite()
    { 
        if (playerHasHorizontalSpeed()) {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private bool playerHasHorizontalSpeed()
    {
        return Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
    }

    private void Swimming()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Water"))) {
            GetComponentInChildren<SpriteRenderer>().color = new Color32(255, 255, 255, 90);
            // TODO add swimming ability
        }
        else
            GetComponentInChildren<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"))) {
            animator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            isAlive = false;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
