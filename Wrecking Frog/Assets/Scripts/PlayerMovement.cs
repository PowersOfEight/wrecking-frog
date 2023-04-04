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


    private LineRenderer m_line;
    private Rigidbody2D m_rigidBody;
    private BoxCollider2D m_collider;
    private float m_movementX;
    private float m_movementY;


    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = gameObject.GetComponent<Rigidbody2D>();
        m_collider = gameObject.GetComponent<BoxCollider2D>();
        m_line = gameObject.GetComponent<LineRenderer>();
    }

    private void FixedUpdate() 
    {  
        m_rigidBody.AddForce(Vector2.right * m_movementX  * speed );
        m_rigidBody.AddForce(Vector2.up * m_movementY, ForceMode2D.Impulse);
        m_movementY = 0;
        m_line.SetPosition(0, transform.position);
        
    }

    void OnJump() 
    {
        // Debug.DrawRay(transform.position, Vector3.down, Color.red, 2.0f);
        RaycastHit2D hit = Physics2D.Raycast(m_collider.transform.position, Vector2.down, (m_collider.size.y + 0.1f) / 2 , 1 << 8);
        if(hit.collider != null ) {
            m_movementY = jumpForce;
        }
    }

    void OnLook()
    {

    }
    void OnTongue(InputValue value)
    {
        
        if(value.isPressed)
        {
            //  TODO: logic to extend tongue when the mouse is pressed down
            Debug.Log($"tongue actuated...");
            Vector3 playerPos = gameObject.transform.position;
            Vector2 playerPosition = new Vector2(playerPos.x, playerPos.y);
            Vector2 pos = Mouse.current.position.ReadValue();
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(pos.x, pos.y));
            Debug.DrawLine(playerPos, mousePosition,Color.green, 2.0f);
            m_line.SetPosition(1, mousePosition);
            m_line.enabled = true;

        } 
        else
        {
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
