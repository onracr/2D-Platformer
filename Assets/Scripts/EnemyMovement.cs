using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    Rigidbody2D myRigidbody2D;
    BoxCollider2D enemyCollider;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
        myRigidbody2D.velocity = new Vector2(moveSpeed, myRigidbody2D.velocity.y);
    }

    void Update()
    {
        Debug.Log(myRigidbody2D.velocity.x);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody2D.velocity.x)), 1f);
        moveSpeed = -moveSpeed;
        myRigidbody2D.velocity = new Vector2(moveSpeed, myRigidbody2D.velocity.y);
    }
}
