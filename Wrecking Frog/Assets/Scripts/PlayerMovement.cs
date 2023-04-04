using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Main Camera for Mouse Position")]
    public Camera mainCamera;

    [Header("Movement Attributes")]
    public float speed;
    public float jumpForce;

    [Tooltip("The amount of tolerance needed for ray collisions")]
    public float RayTolerance = 0.1f;

    [Header("Tongue Stuff")]
    public float tongueLength = 7f;
    public float tongueSpeed = 3f;
    public float tongueOutDuration = 3.0f;
    

    [Header("Debugging Information")]
    public float rayCastDurationInSeconds = 3;


    private float m_remainingRayCastDuration;
    private Vector2 m_currentTongueOrigin;
    private Vector3 m_tongueEnd;
    private Vector3 m_relativeTarget;

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
        m_joint.enabled = false;
        m_tongueActive = false;
    }

    private void Update() 
    {
        
        if(m_remainingRayCastDuration > 0)
        {
            m_remainingRayCastDuration -= Time.deltaTime;
            //  Ok, so the tongue will move at tongue speed, to a maximum extension
            //  We know we need the relative target on click to get the direction of the ray cast
            //  However, we need to find a normalized way of doing this, so that the tongue moves out at a certain speed
            //  and achieves a percentage of its maximum extension based on the percentage of the duration achieved
            //  In other words, the ratio should look like this:
            //  DrawLine(m_collider.transform.position, m_collider.transform.position + (m_relativeTarget.normalized * m_tongueOutTimeElapsed/tongueOutMaxDuration))
            Debug.DrawLine(m_collider.transform.position,m_collider.transform.position + m_tongueEnd,Color.cyan);
            m_tongueEnd += tongueSpeed * Time.deltaTime * m_tongueEnd.normalized;
        
        }
    }
    

    private void FixedUpdate() 
    {  
        m_rigidBody.AddForce(Vector2.right * m_movementX  * speed );
        m_rigidBody.AddForce(Vector2.up * m_movementY, ForceMode2D.Impulse);
        m_movementY = 0;
        m_line.SetPosition(0, transform.position);
        if(m_tongueActive) 
        {
            // m_relativeTarget = (mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - m_collider.transform.position).normalized;
        }
        
        
    }

    void OnJump() 
    {
        // Debug.DrawRay(transform.position, Vector3.down, Color.red, 2.0f);
        RaycastHit2D hit = Physics2D.Raycast(m_collider.transform.position, Vector2.down, (m_collider.size.y + 0.1f) / 2 , 1 << 8);
        Debug.DrawLine(m_collider.transform.position, m_collider.transform.position + Vector3.down * (m_collider.size.y + 0.1f)/2, Color.black, 3.0f);
        if(hit.collider != null ) {
            m_movementY = jumpForce;
        }
    }

    void OnLook()
    {

    }

    void doRayCast(Vector2 relativePosition) 
    {
        m_remainingRayCastDuration = rayCastDurationInSeconds;
        m_tongueEnd = relativePosition.normalized;
    }

    void OnTongue(InputValue value)
    {
        
        if(value.isPressed)
        {
            m_tongueActive = true;
            
            // //  TODO: logic to extend tongue when the mouse is pressed down
            // Debug.Log($"tongue actuated...");
            // Vector3 playerPos = gameObject.transform.position;
            // Vector2 playerPosition = new Vector2(playerPos.x, playerPos.y);
            Vector2 pos = Mouse.current.position.ReadValue();
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 currentPosition = m_collider.transform.position;
            doRayCast(mousePosition - currentPosition);
            // m_joint.enabled = true;
            // // Debug.DrawLine(playerPos, mousePosition,Color.green, 2.0f);
            // m_line.SetPosition(1, mousePosition);
            // m_line.enabled = true;
        } 
        else
        {
            m_tongueActive = false;
            m_joint.enabled = false;
            m_line.enabled = false;
            Debug.Log($"Mouse has been released");
            //  TODO: logic to retract tongue when the mouse is not pressed
        }
        
    }

    void OnMove(InputValue movementValue) 
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        m_movementX = movementVector.x;
        
    }
 
}
