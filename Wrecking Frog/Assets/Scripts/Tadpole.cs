using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tadpole : MonoBehaviour
{
  
  private Rigidbody2D m_rigidBody;
  private Collider2D m_collider;
  private DistanceJoint2D m_parentJoint;

  void Start()
  {
    m_rigidBody = GetComponent<Rigidbody2D>();
    m_parentJoint = GetComponent<DistanceJoint2D>();
    m_collider = GetComponent<Collider2D>();
  }
  private void OnTriggerEnter2D(Collider2D other) 
  {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
          m_rigidBody.isKinematic = false;
          m_collider.isTrigger = false;
          m_parentJoint.connectedBody = other.gameObject.GetComponent<Rigidbody2D>();
          m_parentJoint.enabled = true;

        } 
  } 
}
