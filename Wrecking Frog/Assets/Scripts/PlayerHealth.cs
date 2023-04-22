using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Rigidbody2D m_rigidBody;
    private Stack<Tadpole> m_tadpoleStack;

    void Start() 
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_tadpoleStack = new Stack<Tadpole>();
    }

    public void collectTadpole(Tadpole tadpole)
    {
        if(m_tadpoleStack.Count > 0) 
        {
            tadpole.connectJointToRigidbody(m_tadpoleStack.Peek().GetComponent<Rigidbody2D>());
        }
        else
        {
            tadpole.connectJointToRigidbody(m_rigidBody);
        }
        m_tadpoleStack.Push(tadpole);
    }

    public void TakeDamage()
    {
        if(m_tadpoleStack.Count > 0)
        {
            Tadpole tadpole = m_tadpoleStack.Pop();
            tadpole.Drop();
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("You totally died just now");
        //  TODO: add death logic here
    }
}
