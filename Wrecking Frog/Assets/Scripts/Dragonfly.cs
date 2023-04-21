using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragonfly : MonoBehaviour
{

    public bool drawPath = false;
    public float speed = 3;
    public Transform[] waypoints;


    private float[] m_colliderXOffsets;
    private int m_waypointIndex;
    private SpriteRenderer m_renderer;
    private CompositeCollider2D m_collider;
    private BoxCollider2D[] m_colliders;
    private Rigidbody2D m_rigidBody;

    void Start()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_collider = GetComponent<CompositeCollider2D>();
        m_colliders = GetComponents<BoxCollider2D>();
        m_colliderXOffsets = new float[m_colliders.Length];
        for(int i = 0; i < m_colliders.Length; ++i)
        {
            m_colliderXOffsets[i] = m_colliders[i].offset.x;
        }
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_waypointIndex = 0;
    }

    void flipComponentsX(bool alter)
    {
        int multiplier = alter ? -1 : 1;
        for(int i = 0; i < m_colliders.Length; ++i)
        {
            m_colliders[i].offset = new Vector2(
                multiplier * m_colliderXOffsets[i],
                m_colliders[i].offset.y);
        }
    }

    void Update()
    {
        
        Vector3 direction = waypoints[m_waypointIndex].position - transform.position;
        if(direction.x > 0 && !m_renderer.flipX)
        {
            m_renderer.flipX = true;
            flipComponentsX(true);
            
        }
        else if(direction.x < 0 && m_renderer.flipX)
        {
            m_renderer.flipX = false;
            flipComponentsX(false);
        }
    
        Vector3 movement = direction.normalized * speed * Time.deltaTime;
        if(movement.magnitude > direction.magnitude) 
        {
            m_waypointIndex = (m_waypointIndex + 1) % waypoints.Length;
        }
        transform.Translate(movement);
        
    }
}
