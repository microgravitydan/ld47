using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ring : MonoBehaviour{
    [SerializeField]
    private float ringAngleSet;
    [SerializeField]
    private string ringStatusSet;
    [SerializeField]
    private Light lightIndicator;

    UnityEvent m_Ring;

    void Start() {
        if (m_Ring == null)
            m_Ring = new UnityEvent();

        m_Ring.AddListener(delegate{RingPosition(ringAngleSet);});
        m_Ring.AddListener(delegate{RingStatus(ringStatusSet);});
    }

    void Update(){
    }

    void RingPosition(float ringAngleSet) {
        // Set current ring angle to assigned angle by Leela
        transform.rotation = Quaternion.Euler(ringAngleSet, -90, 0);
    }

    void RingStatus(string ringStatusSet) {
        // Set ring colors
        if (ringStatusSet =="Locked") {
            lightIndicator.color = Color.red;
            } else if (ringStatusSet == "Lockable") {
            // Set Light to Green
            lightIndicator.color = Color.green;
        } else {
            lightIndicator.color = Color.yellow;
        }
    }

}
