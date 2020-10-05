using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RingUI : MonoBehaviour {
    [SerializeField]
    private float ringAngleSet;
    [SerializeField]
    private string ringStatusSet;
    [SerializeField]
    private GameObject lightIndicator;
    [SerializeField]
    private GameObject ringIndicator;

    private Renderer lightRenderer;
    private Renderer ringRenderer;
    
    UnityEvent m_RingUI;

    void Start() {
        if (m_RingUI == null)
            m_RingUI = new UnityEvent();

        m_RingUI.AddListener(delegate{RingPosition(ringAngleSet);});
        m_RingUI.AddListener(delegate{RingStatus(ringStatusSet);});

        lightRenderer = lightIndicator.GetComponent<Renderer>();
        ringRenderer = ringIndicator.GetComponent<Renderer>();
    }

    void Update() {
    }

    void RingPosition(float ringAngleSet) {
        // Set current ring angle to assigned angle by Leela
        transform.localRotation = Quaternion.Euler(-ringAngleSet, 0, 90);
    }

    // CURRENTLY, NONE OF THIS WORKS
    void RingStatus(string ringStatusSet) {
        if (ringStatusSet == "Locked") {
            lightRenderer.material.SetColor("_Color", Color.red);
            ringRenderer.material.SetColor("_Color", Color.cyan);
            ringRenderer.material.SetColor("_BaseColor", Color.cyan);
            ringRenderer.material.SetColor("Color_4562A436", Color.cyan);
            ringRenderer.material.SetColor("Color_AC1E9C59", Color.cyan);
        }else if (ringStatusSet == "Lockable") {
            lightRenderer.material.SetColor("_Color", Color.green);
            ringRenderer.material.SetColor("_Color", Color.green);
            ringRenderer.material.SetColor("_BaseColor", Color.green);
            ringRenderer.material.SetColor("Color_4562A436", Color.green);
            ringRenderer.material.SetColor("Color_AC1E9C59", Color.green);
        } else if (ringStatusSet == "Selected") {
            lightRenderer.material.SetColor("_Color", Color.yellow);
            ringRenderer.material.SetColor("_Color", Color.yellow);
            ringRenderer.material.SetColor("_BaseColor", Color.yellow);
            ringRenderer.material.SetColor("Color_4562A436", Color.yellow);
            ringRenderer.material.SetColor("Color_AC1E9C59", Color.yellow);
        } else {
            lightRenderer.material.SetColor("_Color", Color.yellow);
            ringRenderer.material.SetColor("_Color", Color.cyan);
            ringRenderer.material.SetColor("_BaseColor", Color.cyan);
            ringRenderer.material.SetColor("Color_4562A436", Color.cyan);
            ringRenderer.material.SetColor("Color_AC1E9C59", Color.cyan);
        }
    }
}
