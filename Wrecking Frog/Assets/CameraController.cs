using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject m_frog;//    Reference to the frog in the scene
    [Header("Camera Bounds")]
    [Tooltip("The leftmost part of the camera's range in the scene")]
    public float minimumXValue;
    [Tooltip("The farthest the camera will follow the player to the left")]
    public float maximumXValue;
    [Tooltip("The lowest the camera will follow the player")]
    public float minimumYValue;
    [Tooltip("The highest the camera will follow the player")]
    public float maximumYValue;

    

    void Start()
    {
       m_frog = GameObject.Find("Frog");    
    }

    void LateUpdate()
    {
        float x = Mathf.Clamp(m_frog.transform.position.x, minimumXValue, maximumXValue);
        float y = Mathf.Clamp(m_frog.transform.position.y, minimumYValue, maximumYValue);
        gameObject.transform.position = new Vector3(x, y, transform.position.z);
    }
}
