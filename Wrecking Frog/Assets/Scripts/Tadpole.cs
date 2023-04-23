using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tadpole : MonoBehaviour
{
  [Tooltip("The amount of time in seconds before this instance respawns in its original location")]
  public float respawnTime = 10.0f;

  [Tooltip("The percentage of the respawn time that the tadpole should start blinking")]
  [Range(0.05f, 0.5f)]
  public float blinkPercentage = 0.25f;
  [Tooltip("This changes how long the blink lasts.  For example, if the number 4 is chosen, the blink will be 4 frames on, 4 frames off")]
  [Range(1,20)]
  public int blinkFrames = 6;
  private Vector2 m_spawnPoint;
  private Rigidbody2D m_rigidBody;
  private Rigidbody2D m_animatorRigidbody;
  private SpriteRenderer m_renderer;
  private Color m_originalColor;
  private Color m_zeroAlphaColor;

  private Collider2D m_collider;
  private DistanceJoint2D m_parentJoint;
  private float m_respawnTimer;
  private int m_blinkIndex;
  private enum ePickupMode
  {
    Idle,
    Collected,
    Dropped,
    Blinking
  }
  private ePickupMode m_currentMode;


  void Start()
  {
    m_rigidBody = GetComponent<Rigidbody2D>();
    m_parentJoint = GetComponent<DistanceJoint2D>();
    m_collider = GetComponent<Collider2D>();
    m_animatorRigidbody = GetComponentInChildren<Rigidbody2D>();
    m_renderer = GetComponentInChildren<SpriteRenderer>();
    m_originalColor = m_renderer.color;
    m_zeroAlphaColor = new Color(m_originalColor.r, m_originalColor.g, m_originalColor.b, 0.0f);
    m_currentMode = ePickupMode.Idle;
    m_spawnPoint = transform.position;
    m_blinkIndex = 0;
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
    m_rigidBody.angularVelocity = 0;
    m_rigidBody.isKinematic = true;
    m_collider.isTrigger = true;
    m_parentJoint.enabled = false;
    m_animatorRigidbody.MoveRotation(0);
    m_respawnTimer = respawnTime * (1 - blinkPercentage);
    
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
          m_currentMode = ePickupMode.Blinking;
          m_respawnTimer = respawnTime * blinkPercentage;
        }
        else
        {
          m_respawnTimer -= Time.deltaTime;
        }
        break;
      case ePickupMode.Blinking:
        if (m_respawnTimer <= 0)
        {
          if(transform.position.x == m_spawnPoint.x && 
            transform.position.y == m_spawnPoint.y)
          {
            m_currentMode = ePickupMode.Idle;
            m_renderer.color = m_originalColor;
          }
          else
          {
            transform.position = m_spawnPoint;
            m_respawnTimer = respawnTime * blinkPercentage;
          }
        }
        else
        {
          m_blinkIndex = (m_blinkIndex + 1) % blinkFrames;
          if(m_blinkIndex == 0)
          {
            m_renderer.color = m_renderer.color == m_originalColor ? m_zeroAlphaColor : m_originalColor;
          }
          m_respawnTimer -= Time.deltaTime;
        }
        break;
    }
      

    
  }   
  
}
