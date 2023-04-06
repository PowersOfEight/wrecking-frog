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

    private eTongueMode m_tongueMode;
    private float m_tongueOutTimeElapsed;
    //  Normalized vector representing the mouse direction
    private Vector2 m_mouseRelativeDirection;
    private bool m_mouseIsActive;
    private LineRenderer m_line;
    private Rigidbody2D m_rigidBody;
    private BoxCollider2D m_collider;
    private SpringJoint2D m_joint;
    private bool m_tongueActive;
    private float m_movementX;
    private float m_movementY;


    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = gameObject.GetComponent<Rigidbody2D>();
        m_collider = gameObject.GetComponent<BoxCollider2D>();
        m_line = gameObject.GetComponent<LineRenderer>();
        m_line.enabled = false;
        m_joint = gameObject.GetComponent<SpringJoint2D>();
        m_tongueOutTimeElapsed = 0.0f;
        m_joint.enabled = false;
        m_mouseIsActive = false;
        m_tongueMode = eTongueMode.Idle;
    }

    private void Update() 
    {
        //  TODO: Use the relative state of the tongue to render
        if(m_mouseIsActive && m_tongueOutTimeElapsed <= tongueOutDuration)
        {
            m_tongueOutTimeElapsed += Time.deltaTime;
            float tongueRelativeMagnitude = tongueLength * m_tongueOutTimeElapsed / tongueOutDuration;
            Debug.DrawRay(transform.position, tongueRelativeMagnitude * m_mouseRelativeDirection, Color.magenta);
        }
    }
    

    private void FixedUpdate() 
    {  
        switch(m_tongueMode) 
        {   
            case eTongueMode.Idle:
                break;
            case eTongueMode.Extending:
                break;
            case eTongueMode.Latched:
                break;
            case eTongueMode.Retracting:
                break;

        }
        if( m_mouseIsActive)
        {
            updateMouseDirection();
        }
        m_rigidBody.AddForce(Vector2.right * m_movementX  * speed );
        m_rigidBody.AddForce(Vector2.up * m_movementY, ForceMode2D.Impulse);
        m_movementY = 0;
    
    }

    private void updateMouseDirection()
    {
        m_mouseRelativeDirection = (mainCamera.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            ) - transform.position);
        m_mouseRelativeDirection.Normalize();
    }

    void OnJump() 
    {
        RaycastHit2D hit = 
            Physics2D.Raycast(
                m_collider.transform.position, 
                Vector2.down, 
                (m_collider.size.y + rayTolerance) / 2 , 
                1 << 8);
        if(hit.collider != null ) {
            m_movementY = jumpForce;
        }
    }

    void OnLook()
    {

    }

 

    void OnTongue(InputValue value)
    {
        switch(m_tongueMode) 
        {   
            case eTongueMode.Idle:
                // m_tongueOutTimeElapsed = 0.0f;
                break;
            case eTongueMode.Extending:
                break;
            case eTongueMode.Latched:
                break;
            case eTongueMode.Retracting:
                break;

        }
        m_mouseIsActive = value.isPressed;
        m_tongueOutTimeElapsed = 0.0f;
        
    }

    void OnMove(InputValue movementValue) 
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        m_movementX = movementVector.x;
        
    }
 
}
