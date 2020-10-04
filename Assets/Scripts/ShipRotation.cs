using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShipRotation : MonoBehaviour {
    [SerializeField]
    public float rotationPeriod; //Rotation Period (8seconds/rotation for mars)
    [SerializeField]
    private bool xRotAxis, yRotAxis, zRotAxis;
    [SerializeField]
    private int ringNumber;
    private int ringChoice;
    [SerializeField]
    private float ringAngle;

    [SerializeField]
    private GameObject ringUI;

    UnityEvent m_Ring;

    void Start() {
        if (m_Ring == null)
            m_Ring = new UnityEvent();

        m_Ring.AddListener(delegate{RightHandThrust(ringChoice);});
        m_Ring.AddListener(delegate{LeftHandThrust(ringChoice);});

        rotationPeriod = 4/rotationPeriod;
    }

    void Awake() {
    }

    void Update() {
        //Rotate
        if (xRotAxis == true ) {
            transform.Rotate(rotationPeriod, 0, 0, Space.Self);
        } else if (yRotAxis == true) {
            transform.Rotate(0, rotationPeriod, 0, Space.Self);
        } else if (zRotAxis == true) {
            transform.Rotate(0, 0, rotationPeriod, Space.Self);
        } else {
        }
        
        // Get ringAngle and send to UI
        ringAngle = transform.eulerAngles.x;
        if (ringAngle < 0) {
            ringAngle += 360;
        }
        //Debug.Log(ringNumber + " " +transform.eulerAngles);
        ringUI.SendMessage("RingSpeed", ringAngle);
    }

    void RightHandThrust(int ringChoice) {
        Debug.Log(ringNumber + "Right Hand Thrust");
        if (ringChoice == ringNumber) {
            rotationPeriod += 0.001f;
        }
    }

    void LeftHandThrust (int ringChoice) {
        Debug.Log(ringNumber + "Left Hand Thrust");
        if (ringChoice == ringNumber) {
            rotationPeriod -= 0.001f;
        }
    }
}
