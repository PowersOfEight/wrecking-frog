using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private Rigidbody2D m_rigidBody;
    private BoxCollider2D m_collider;
    private float m_movementX;
    private float m_movementY;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = gameObject.GetComponent<Rigidbody2D>();
        m_collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() 
    {  
        m_rigidBody.AddForce(Vector2.right * m_movementX  * speed );
        m_rigidBody.AddForce(Vector2.up * m_movementY, ForceMode2D.Impulse);
        m_movementY = 0;
    }

    void OnJump() 
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(m_collider.transform.position, Vector2.down, (m_collider.size.y + 0.1f) / 2 , 1 << 8);
        if(hit.collider != null ) {
            m_movementY = jumpForce;
        }
    }

    void OnTongue(InputValue value)
    {
        if(Mouse.current.leftButton.IsPressed())
        {
            //  TODO: logic to extend tongue when the mouse is pressed down
            Debug.Log($"tongue actuated...");
        } 
        else
        {
            Debug.Log($"Mouse has been released");
            //  TODO: logic to retract tongue when the mouse is not pressed
        }
        Debug.Log($"Current mouse position: {Mouse.current.position.ReadValue()}");
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
