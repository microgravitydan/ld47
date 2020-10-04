using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour {
    [SerializeField]
    private int ringChoice = 1;
    [SerializeField]
    private GameObject ring1, ring2, ring3, ring4, ring5, ring6, ring7, ring8;
    [SerializeField]
    private GameObject ringUI1, ringUI2, ringUI3, ringUI4, ringUI5, ringUI6, ringUI7, ringUI8;
    private GameObject actionTarget;
    [SerializeField]
    private float debounce = 0;
    [SerializeField]
    private float debounceAmount = 0.2f;

    void Start() {
        actionTarget = ring1;
    }

    void UpdateRingChoice() {
        // Brute Force Ring Choice
        if (ringChoice == 1) {
            actionTarget = ring1;
        } else if (ringChoice == 2) {
            actionTarget = ring2;
        } else if (ringChoice == 3) {
            actionTarget = ring3;
        } else if (ringChoice == 4) {
            actionTarget = ring4;
        } else if (ringChoice == 5) {
            actionTarget = ring5;
        } else if (ringChoice == 6) {
            actionTarget = ring6;
        } else if (ringChoice == 7) {
            actionTarget = ring7;
        } else if (ringChoice == 8) {
            actionTarget = ring8;
        } else {
            Debug.Log("Ring choice out of bounds.");
            ringChoice = 1;
            actionTarget = ring1;
            Debug.Log("Reset to Ring 1.");
        }
    }

    void Update() {
        // Ring Choice
        if (debounce <= 0) {
            if (Input.GetAxis("Vertical") > 0.0f) {
                if (ringChoice < 8) {
                    ringChoice += 1;
                    UpdateRingChoice();
                    Debug.Log(ringChoice);
                    debounce += debounceAmount;
                }
            } else if (Input.GetAxis("Vertical") < -0.0f) {
                if (ringChoice > 1) {
                    ringChoice -= 1;
                    UpdateRingChoice();
                    Debug.Log(ringChoice);
                    debounce += debounceAmount;
                }
            } else {};
        }

        // Ring Speed
        if (Input.GetAxis("Horizontal") > 0.02f) {
            actionTarget.SendMessage("RightHandThrust", ringChoice);
            //Debug.Log(Input.GetAxis("Horizontal"));
        } else if (Input.GetAxis("Horizontal") < -0.02f) {
            actionTarget.SendMessage("LeftHandThrust", ringChoice);
            //Debug.Log(Input.GetAxis("Horizontal"));
        } else {};

        // Update debounce
        if (debounce > 0) {
            debounce -= Time.deltaTime;
        }
    }
}
