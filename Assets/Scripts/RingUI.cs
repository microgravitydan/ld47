using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RingUI : MonoBehaviour {
    [SerializeField]
    private bool xRotAxis, yRotAxis, zRotAxis;
    [SerializeField]
    private float ringAngle;
    private float ringAngleSet;
    private Vector4 ringColor;
    private Vector4 ringColorSet;
    
    UnityEvent m_RingUI;

    void Start() {
        if (m_RingUI == null)
            m_RingUI = new UnityEvent();

        m_RingUI.AddListener(delegate{RingSpeed(ringAngleSet);});
        m_RingUI.AddListener(delegate{RingColor(ringColorSet);});

    }

    void Update() {
    }

    void RingSpeed(float ringAngleSet) {
        // Set current ring angle to assigned angle by Axis boolean
        //transform.rotation = Quaternion.identity;
        //transform.Rotate(0, ringAngleSet, 0, Space.Self);
        transform.localRotation = Quaternion.Euler(90, ringAngleSet, 90);
        //transform.localEulerAngles.y = ringAngleSet;
    }

    void RingColor(Vector4 ringColorSet) {
        // Set ring colors
    }
}
