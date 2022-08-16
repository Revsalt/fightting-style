using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    
    public Animator anim;
    [SerializeField] private Transform model;
    [SerializeField] private LayerMask jumpableGround;

    public float moveSpeed = 7f;
    public float jumpForce = 14f;
    public int MaxjumpCount = 2;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashCoolDownDuration = 5;
    [SerializeField] private PhysicMaterial[] physicMaterials;

    private enum MovementState { idle, running, jumping, falling }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        moveSpeed *= Time.fixedDeltaTime;
        jumpForce *= Time.fixedDeltaTime;
        dashDistance *= Time.fixedDeltaTime;
    }

    bool canMove = true;
    bool canDash = true;
    int jumpCount = 0;

    private void Update()
    {
        if (!canMove)
            return;

        if (Input.GetButtonDown("Jump") && jumpCount < MaxjumpCount)
        {
            jumpCount++;
            rb.AddForce(Vector3.up * jumpForce , ForceMode.Impulse);
        }

        if (IsGrounded())
        {
            jumpCount = 0;
            canDash = true;
        }

        if (Input.GetButtonDown("Fire2") && canDash)
        {
            StartCoroutine(startDash(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized));
            canMove = false;
        }

        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;

        rb.AddForce(Input.GetAxisRaw("Horizontal") * moveSpeed * Vector3.right , ForceMode.Acceleration);

        if (Input.GetAxisRaw("Vertical") < 0 && !IsGrounded())
        {
            rb.AddForce(Vector3.up * -(jumpForce / 10) , ForceMode.Impulse);
        }

        rb.AddForce(Vector3.up * -(18) * Time.deltaTime, ForceMode.Impulse);
    }

    IEnumerator startDash(Vector2 direction)
    {
        canDash = false;
        gameObject.layer = 9;
        rb.velocity = direction * dashDistance;

        anim.SetTrigger("isDash");

        yield return new WaitForSeconds(.1f);

        rb.velocity = Vector2.zero;
        gameObject.layer = 10;
        canMove = true;

        //delay
        yield return new WaitForSeconds(dashCoolDownDuration);
        canDash = true;
    }

    public void AddKockBack(float force, Vector3 direction)
    {
        float health_ = GetComponent<Health>().health;

        StopCoroutine(KnockBackNoMovement());
        StartCoroutine(KnockBackNoMovement());

        IEnumerator KnockBackNoMovement()
        {
            rb.AddForce(direction * (force * health_) * Time.deltaTime, ForceMode.Impulse);
            GetComponent<CapsuleCollider>().material = physicMaterials[1];
            yield return new WaitForSeconds(health_ * .01f);
            GetComponent<CapsuleCollider>().material = physicMaterials[0];
            rb.velocity = Vector3.zero;
        }
    }

    private void UpdateAnimationState()
    {
        if (anim.enabled == false)
            return;

        MovementState state;

        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            state = MovementState.running;
            model.localScale = new Vector3(0.74929f, model.localScale.y, model.localScale.z);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            state = MovementState.running;
            model.localScale = new Vector3(-0.74929f, model.localScale.y, model.localScale.z);
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        CapsuleCollider coll = GetComponent<CapsuleCollider>();
        return Physics.Raycast(coll.bounds.center, Vector3.down, coll.bounds.extents.y + .2f, jumpableGround);
    }

}