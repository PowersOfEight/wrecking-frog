using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Main Camera for Mouse Position")]
    public Camera mainCamera;

    [Header("Movement Attributes")]
    [Tooltip("The horizontal speed of player movement")]
    public float speed;
    [Tooltip("The force of the jump")]
    public float jumpForce;

    [Tooltip("The amount of tolerance needed to detect the ground when jumping\nThis should be a small amount")]
    [Range(0.01f,0.1f)]
    public float rayTolerance = 0.1f;

    [Header("Tongue Stuff")]
    [Tooltip("The total length the tongue should extend")]
    public float tongueLength = 7f;
    [Tooltip("The amount of time, in seconds, the tongue should move outward")]
    public float tongueOutDuration = 3.0f;

    [Tooltip("The amount of time, in seconds, that it should take for the tongue to retract")]
    public float tongueRetractionDuration = 0.25f;
    
    private enum eTongueMode {
        Idle,
        Extending,
        Latched,
        Retracting
    }

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D animatorRigidBody;
    private eTongueMode m_tongueMode;
    private float m_tongueMagnitude;
    private float m_tongueOutTimeElapsed;
    //  Normalized vector representing the mouse direction
    private Vector2 m_tongueRelativeDirection;
    private LineRenderer m_line;
    private Rigidbody2D m_rigidBody;
    private BoxCollider2D m_collider;
    private SpringJoint2D m_joint;
    private float m_movementX;
    private float m_movementY;

    void Start()
    {
        m_rigidBody = gameObject.GetComponent<Rigidbody2D>();
        m_collider = gameObject.GetComponent<BoxCollider2D>();
        m_line = gameObject.GetComponent<LineRenderer>();
        m_line.enabled = false;
        m_line.SetPosition(0, transform.position);
        m_joint = gameObject.GetComponent<SpringJoint2D>();
        m_tongueOutTimeElapsed = 0.0f;
        m_tongueMagnitude = 0.0f;
        m_joint.enabled = false;
        m_tongueMode = eTongueMode.Idle;
    }

    private void Update() 
    {
        if (m_movementX != 0) 
        {
            spriteRenderer.flipX = m_movementX < 0;
        }
        if (m_tongueMode != eTongueMode.Idle)
        {
            float angle = Mathf.Atan2(
                m_tongueRelativeDirection.y, 
                m_tongueRelativeDirection.x) * Mathf.Rad2Deg;
            animatorRigidBody.MoveRotation(angle - 90);
            
        }
        animator.SetFloat("horizontalSpeed", Mathf.Abs(m_rigidBody.velocity.x)); 
        animator.SetFloat("verticalVelocity", m_rigidBody.velocity.y);
        m_line.SetPosition(0, transform.position);
        switch(m_tongueMode)
        {
            case eTongueMode.Extending:
                m_tongueOutTimeElapsed += Time.deltaTime;
                if(m_tongueOutTimeElapsed >= tongueOutDuration) 
                {
                    m_tongueOutTimeElapsed = 0.0f;
                    m_tongueMode = eTongueMode.Retracting;
                }
                else
                {
                    m_tongueMagnitude = tongueLength * m_tongueOutTimeElapsed / tongueOutDuration;
                    m_line.SetPosition(1, (Vector2) transform.position + m_tongueMagnitude * m_tongueRelativeDirection);
                    
                }
                break;
            case eTongueMode.Latched:
                break;
            case eTongueMode.Retracting:
                m_tongueMagnitude -= tongueLength * Time.deltaTime / tongueRetractionDuration;
                if (m_tongueMagnitude <= 0.0f)
                {
                    m_tongueMode = eTongueMode.Idle;
                    m_line.enabled = false;
                    animatorRigidBody.MoveRotation(0);
                    animator.SetBool("tongueOut", false);
                }
                else
                {
                    m_line.SetPosition(1, (Vector2) transform.position + m_tongueMagnitude * m_tongueRelativeDirection);
                }
                break;
        }
    }
    

    private void FixedUpdate() 
    {  
        switch(m_tongueMode) 
        {   
            case eTongueMode.Idle:
                break;
            case eTongueMode.Extending:
                RaycastHit2D hit = Physics2D.Raycast(transform.position,
                    m_tongueRelativeDirection,
                    m_tongueMagnitude,
                    ~((1 << 10)|(1 << 11)));
                if(hit.collider != null) 
                {
                    switch(hit.transform.gameObject.layer) 
                    {
                        case 9:
                            m_joint.connectedAnchor = hit.point;
                            m_joint.enabled = true;
                            m_tongueMode = eTongueMode.Latched;
                            break;
                        default:
                            m_tongueMode = eTongueMode.Retracting;
                            break;
                    }
                }
                else
                {
                    updateMouseDirection ();
                }
                break;
            case eTongueMode.Latched:
                updateLatchPointDirection();
                break;
            case eTongueMode.Retracting:
                break;

        }
        m_rigidBody.AddForce(Vector2.right * m_movementX  * speed );
        m_rigidBody.AddForce(Vector2.up * m_movementY, ForceMode2D.Impulse);
        m_movementY = 0;
    
    }

    private void updateLatchPointDirection()
    {
        m_tongueRelativeDirection = m_joint.connectedAnchor - (Vector2) transform.position;
        m_tongueRelativeDirection.Normalize();

    }
    private void updateMouseDirection()
    {
        m_tongueRelativeDirection = (mainCamera.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            ) - transform.position);
        m_tongueRelativeDirection.Normalize();
    }

    void OnJump() 
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            m_collider.bounds.center, 
            m_collider.bounds.size, 
            0.0f, 
            Vector2.down, 
            rayTolerance,
            (1 << 8 | 1));
        if(hit.collider != null ) {
            m_movementY = jumpForce;
        }
    }

    void OnTongue(InputValue value)
    {
        switch(m_tongueMode) 
        {   
            case eTongueMode.Idle:
                if(value.isPressed)
                {
                    animator.SetBool("tongueOut", true);
                    m_tongueOutTimeElapsed = 0.0f;
                    m_tongueMode = eTongueMode.Extending;
                    m_line.enabled = true;
                }
                break;
            case eTongueMode.Extending:
                if(!value.isPressed || m_tongueOutTimeElapsed >= tongueOutDuration)
                {
                    m_tongueMode = eTongueMode.Retracting;
                }
                break;
            case eTongueMode.Latched:
                if(!value.isPressed)
                {
                    m_joint.enabled = false;
                    m_tongueMode = eTongueMode.Retracting;
                }
                break;
            case eTongueMode.Retracting:
                
                break;

        }
        
    }

    void OnMove(InputValue movementValue) 
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        m_movementX = movementVector.x;
        
    }

    public float getImpactEnergy()
    {
        return m_rigidBody.velocity.magnitude;
    }

    public bool isLatched()
    {
        return m_tongueMode == eTongueMode.Latched;
    }
 
}
