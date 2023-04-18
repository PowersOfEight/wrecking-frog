using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum eType { none, Tadpole }

    [Header("Inscribed")]
    public eType itemType;

    private Collider2D  colld;
    private const float colliderEnableDelay = 0.5f;
    
    void Awake() {
        colld = GetComponent<Collider2D>();
        colld.enabled = false;
        Invoke( nameof(EnableCollider), colliderEnableDelay );
    }

    void EnableCollider() {
        colld.enabled = true;
    }
}
