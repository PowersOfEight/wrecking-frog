using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    public enum eMode
    {
        gIdle,
        gOut,
        gRetract,
        gPull
    }

    [Tooltip("Speed at which Grappler extends (doubled in gRetract mode)")]
    public float grappleSpd = 10;
    [Tooltip("Maximum length that Grappler will reach")]
    public float maxLength = 7.25f;
    [Tooltip("Minimum distance of Grappler from Frog")]
    public float minLength = 0.375f;

    [Header("Dynamic")]
    [SerializeField]
    private eMode _mode = eMode.gIdle;
    public eMode mode
    {
        get { return _mode; }
        private set { _mode = value; }
    }

    private LineRenderer line;
    private Rigidbody2D rigid;
    private Collider2D colld;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        colld = GetComponent<Collider2D>();
    }
}
