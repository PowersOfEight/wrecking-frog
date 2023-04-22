using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathway : MonoBehaviour
{
    private Transform[] m_pathway;
    private void Awake()
    {
        m_pathway = new Transform[transform.childCount];
        for (int i = 0; i < m_pathway.Length; ++i) 
        {
            m_pathway[i] = transform.GetChild(i);
        }
        
    }
    
    public Transform[] getPathway()
    {
        return m_pathway;
    }

}
