using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tadpole : MonoBehaviour
{
  [Tooltip("The amount of time in seconds before this instance respawns in its original location")]
  public float respawnTime = 10.0f;
  private Vector2 m_spawnPoint;
  private Rigidbody2D m_rigidBody;
  private Rigidbody2D m_animatorRigidbody;
  private Collider2D m_collider;
  private DistanceJoint2D m_parentJoint;
  private float m_respawnTimer;
  private enum ePickupMode
  {
    Idle,
    Collected,
    Dropped
  }
  private ePickupMode m_currentMode;


  void Start()
  {
    m_rigidBody = GetComponent<Rigidbody2D>();
    m_parentJoint = GetComponent<DistanceJoint2D>();
    m_collider = GetComponent<Collider2D>();
    m_animatorRigidbody = GetComponentInChildren<Rigidbody2D>();
    m_currentMode = ePickupMode.Idle;
    m_spawnPoint = transform.position;
  }

  private void OnTriggerEnter2D(Collider2D other) 
  {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
          PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
          player.collectTadpole(this);
          m_currentMode = ePickupMode.Collected;
          m_rigidBody.isKinematic = false;
          m_collider.isTrigger = false;
          m_parentJoint.enabled = true;
        } 
  } 

  public void connectJointToRigidbody(Rigidbody2D other)
  {
    m_parentJoint.connectedBody = other;
    m_parentJoint.enabled = false;
  }

  public void Drop()
  {
    m_currentMode = ePickupMode.Dropped;
    m_rigidBody.velocity = Vector2.zero;
    m_rigidBody.isKinematic = true;
    m_collider.isTrigger = true;
    m_parentJoint.enabled = false;
    m_animatorRigidbody.MoveRotation(0);
    m_respawnTimer = respawnTime;
  }

  void Update() 
  {

    switch(m_currentMode)
    {
      case ePickupMode.Collected:
        Vector2 direction = m_rigidBody.velocity.normalized;
        float angle = Mathf.Atan2(
                  direction.y, 
                  direction.x) * Mathf.Rad2Deg;
        m_animatorRigidbody.MoveRotation(angle - 90);
        break;
      case ePickupMode.Dropped:
        if (m_respawnTimer <= 0)
        {
          //  respawn
          transform.position = m_spawnPoint;
          m_currentMode = ePickupMode.Idle;
        }
        else
        {
          m_respawnTimer -= Time.deltaTime;
        }
        break;
    }
      

    
  }   
  
}
