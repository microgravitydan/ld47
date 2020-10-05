using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Leela : MonoBehaviour {
    // Rings
    [SerializeField]
    private GameObject[] ring;
    [SerializeField]
    private GameObject[] ringUI;
    [SerializeField]
    private float[] ringVelocity; // Degrees per second
    [SerializeField]
    private float[] ringStartingPeriod; // Seconds to complete revolution
    [SerializeField]
    private float[] ringPosition;
    [SerializeField]
    private string[] ringStatus;
    [SerializeField]
    private bool[] ringLocked;
    
    //Gameplay
    [SerializeField]
    private float angleWiggle = 5.0f;
    [SerializeField]
    private float speedWiggle = 0.2f;
    [SerializeField]
    private int ringSelected = 0;
    [SerializeField]
    private float thrustAmount = 0.1f; // Degrees per second per second

    //Controls
    private float debounce = 0;
    private float debounceAmount = 0.2f;


    void Start() {
        // Set initial ringVelocity for each ring
        for (int i = 0; i < 8; i++) {
            ringVelocity[i] = 360 / ringStartingPeriod[i];
        }

    }

    void Update(){
        // Ring Choice
        if (debounce <= 0) {
            if (Input.GetAxis("Vertical") > 0.0f) {
                if (ringSelected < 7) {
                    ringSelected += 1;
                    Debug.Log(ringSelected);
                    debounce += debounceAmount;
                }
            } else if (Input.GetAxis("Vertical") < -0.0f) {
                if (ringSelected > 0) {
                    ringSelected -= 1;
                    Debug.Log(ringSelected);
                    debounce += debounceAmount;
                }
            } else {};
        }

        // Ring Velocity
        if (Input.GetAxis("Horizontal") > 0.02f) {
            ringVelocity[ringSelected] += thrustAmount;
        } else if (Input.GetAxis("Horizontal") < -0.02f) {
            ringVelocity[ringSelected] -= thrustAmount;
        } else {};

        // Ring Position
        for (int i = 0; i < 8; i++) {
            ringPosition[i] += Time.deltaTime * ringVelocity[i];
            if (ringPosition[i] > 360) {
                ringPosition[i] -= 360;
            } else if (ringPosition[i] < 0) {
                ringPosition[i] += 360;
            }
        }

        // Determine State
        for (int i = 1; i < 8; i++) {
            if (ringLocked[i-1] & ringLocked[i] == false) {
                if (ringPosition[i] > ringPosition[i-1] - angleWiggle & ringPosition[i] < ringPosition[i-1] + angleWiggle) {
                    ringStatus[i] = "Lockable";
                } else {
                    ringStatus[i] = "Spinning";
                }
            }
        }

        // Try to Lock

        // Break previous ring if Velocity is greater than speedWiggle

        // Update Rings
        for (int i = 0; i < 8; i++) {
            // Send ring position
            ring[i].SendMessage("RingPosition", ringPosition[i]);
            // Send ring status
            ring[i].SendMessage("RingStatus", ringStatus[i]);
        }

        // Update UI
        for (int i = 0; i < 8; i++) {
            ringUI[i].SendMessage("RingPosition", ringPosition[i]);
            // Send ring status colors
        }

        // Update debounce
        if (debounce > 0) {
            debounce -= Time.deltaTime;
        }
    }
}
