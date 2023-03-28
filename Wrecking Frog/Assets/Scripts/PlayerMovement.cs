using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D m_rigidBody;
    private float m_movementX;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {  
        m_rigidBody.AddForce(Vector2.right * m_movementX  * speed );
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
