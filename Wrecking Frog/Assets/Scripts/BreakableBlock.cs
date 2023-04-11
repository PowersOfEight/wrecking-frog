using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableBlock : MonoBehaviour
{


    [Tooltip("The minimum amount of energy necessary to break this block")]
    [Range(1f,8f)]
    public float impactEnergyThreshold = 5.76f;
    
    [Header("Particle System")]
    [Tooltip("Add the particle system prefab for the break animation here")]
    public GameObject particleSystemPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }



    private void OnCollisionEnter2D(Collision2D other) 
    {
        PlayerMovement player = other.gameObject.GetComponentInParent<PlayerMovement>();
        if (player != null && player.isLatched()
            && other.GetContact(0).normal.x != 0 
            && other.relativeVelocity.magnitude >= impactEnergyThreshold)
        {
            // Debug.Log($"The amount of force: {other.relativeVelocity.magnitude} and the threshold is {impactEnergyThreshold}");
            Instantiate<GameObject>(particleSystemPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameObject.active)
        {
            Destroy(gameObject);
        }
    }
}
