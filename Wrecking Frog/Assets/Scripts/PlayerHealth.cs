using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [Tooltip("This changes how long the blink lasts.  For example, if the number 4 is chosen, the blink will be 4 frames on, 4 frames off")]
    [Range(1,20)]
    public int blinkFrames = 4;
    [Tooltip("Determines how long the player will remain invincible after taking damage")]
    public float invincibilityTime = 5; 
    public float lowerDeathBoundary = -20f;
    private bool m_invincible;
    private float m_invincibleTimeElapsed;
    private int m_invincibleFrameIndex;
    private Rigidbody2D m_rigidBody;
    private Stack<Tadpole> m_tadpoleStack;
    private SpriteRenderer m_renderer;
    private Color m_originalColor;
    private Color m_zeroAlphaColor;

    private GameState m_gameState;

    void Start() 
    {
        m_invincible = false;
        m_invincibleTimeElapsed = 0.0f;
        m_invincibleFrameIndex = 0;
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_tadpoleStack = new Stack<Tadpole>();
        m_renderer = GetComponentInChildren<SpriteRenderer>();
        m_originalColor = m_renderer.color;
        m_zeroAlphaColor =  new Color(m_originalColor.r, m_originalColor.g, m_originalColor.b, 0.0f);
        m_gameState = GameObject.Find("GameState").GetComponent<GameState>();
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
        if(m_gameState.allTadpolesCollected(m_tadpoleStack))
        {
            m_gameState.PlayerWin(this);
        }
    }

    

    public void TakeDamage()
    {
        if(m_tadpoleStack.Count > 0)
        {
            m_invincible = true;
            Tadpole tadpole = m_tadpoleStack.Pop();
            tadpole.Drop();
            m_invincibleTimeElapsed = 0.0f;
        }
        else
        {
            Die();
        }
    }

    void Update() 
    {
        if(transform.position.y < lowerDeathBoundary) 
        {
            Die();
        }
        else
        {
            if(m_invincible)
            {
                if(m_invincibleTimeElapsed >= invincibilityTime)
                {
                    m_invincible = false;
                    m_renderer.color = m_originalColor;
                }
                else
                {
                    m_invincibleFrameIndex = (m_invincibleFrameIndex + 1) % blinkFrames;
                    if(m_invincibleFrameIndex == 0)
                    {
                        m_renderer.color = m_renderer.color == m_zeroAlphaColor ? m_originalColor : m_zeroAlphaColor;
                    }
                    m_invincibleTimeElapsed += Time.deltaTime;
                }
            }

        }
    }

    public bool isInvincible()
    {
        return m_invincible;
    }

    private void Die()
    {
        m_gameState.PlayerLose(this);
    }
}
