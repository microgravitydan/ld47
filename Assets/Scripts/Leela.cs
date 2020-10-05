using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private float ringOneMin = 30;
    [SerializeField]
    private float ringOneMax = 180;
    [SerializeField]
    private float ringMax = 360;

    // Score Tracking


    // Controls
    private bool gamePaused;
    private float debounce = 0;
    private float debounceAmount = 0.2f;
    private float lockDebounce = 0;
    private float lockDebounceAmount = 1.0f;

    // UI
    [SerializeField]
    private GameObject pausePanel;

    void Start() {
        // Set initial ringVelocity for each ring
        for (int i = 0; i < 8; i++) {
            ringVelocity[i] = 360 / ringStartingPeriod[i];
        }

    }

    void Awake() {

    }

    void Update(){
        // Game Pause
        if (gamePaused) {
            Time.timeScale = 0;
            pausePanel.gameObject.SetActive(true);
            if (Input.GetButtonDown("Cancel")){
				gamePaused = false;
			} else if (Input.GetButtonDown("Restart")) {
                Restart();
            } else if (Input.GetButtonDown("Quit")) {
                Quit();
            }
        } else {
            // Manage Pause Screen
            Time.timeScale = 1;
            pausePanel.gameObject.SetActive(false);
            if (Input.GetButtonDown("Cancel")){
				gamePaused = true;
			}
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
            if (ringLocked[ringSelected] == false) {
                if (Input.GetAxis("Horizontal") > 0.02f) {
                    ringVelocity[ringSelected] += thrustAmount;
                } else if (Input.GetAxis("Horizontal") < -0.02f) {
                    ringVelocity[ringSelected] -= thrustAmount;
                } else {};
            }

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
            // For ring 0
            if (ringLocked[0] == false) {
                if (Mathf.Abs(ringVelocity[0]) >= ringOneMin & Mathf.Abs(ringVelocity[0]) <= ringOneMax) {
                    ringStatus[0] = "Lockable";
                } else if (ringSelected == 0) {
                    ringStatus[0] = "Selected";
                } else {
                    ringStatus[0] = "Spinning";
                }
            }
            // For rings 1-7
            for (int i = 1; i < 8; i++) {
                if (ringLocked[i-1] & ringLocked[i] == false) {
                    if (ringPosition[i] > ringPosition[i-1] - angleWiggle & ringPosition[i] < ringPosition[i-1] + angleWiggle) {
                        ringStatus[i] = "Lockable";
                    } else if (i == ringSelected) {
                        ringStatus[i] = "Selected";
                    } else {
                        ringStatus[i] = "Spinning";
                    }
                } else if (ringLocked[i] == true) {
                    ringStatus[i] = "Locked";
                }
            }

            // TODO: OVERSPEED
                // If ring goes beyond ringMax, gameover.

            // Try to Lock
            if (lockDebounce <= 0) {
                if (Input.GetButtonDown("Jump")){
                    // Ring 0
                    if (ringSelected == 0){
                        if (ringStatus[0] == "Lockable") {
                            ringStatus[0] = "Locked";
                            ringLocked[0] = true;
                        } else if (ringStatus[0] == "Locked") {
                            ringStatus[0] = "Spinning";
                            ringLocked[0] = false;
                        }
                    // Rings 1-7
                    } else if (ringStatus[ringSelected] == "Lockable") {
                        if (ringVelocity[ringSelected] > ringVelocity[ringSelected-1] - speedWiggle & ringVelocity[ringSelected] < ringVelocity[ringSelected-1] + speedWiggle) {
                            Debug.Log("Spacebar, Locked");
                            ringStatus[ringSelected] = "Locked";
                            ringLocked[ringSelected] = true;
                            ringVelocity[ringSelected] = ringVelocity[ringSelected-1];
                            Debug.Log("Ring " + ringSelected + " Locked");
                        } else {
                            Debug.Log("Spacebar, Too fast");
                            ringStatus[ringSelected-1] = "Spinning";
                        }
                    } else if (ringStatus[ringSelected] == "Locked") {
                        ringStatus[ringSelected] = "Selected";
                        ringLocked[ringSelected] = false;
                    } else {
                    }
                    lockDebounce = lockDebounceAmount;
                }
            }
        }
        // Break previous ring if Velocity is greater than speedWiggle

        // Check for Win Condition
        if (CheckWinCondition ()) {
            Debug.Log("Win condition set");
        }

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
        if (lockDebounce > 0) {
            lockDebounce -= Time.deltaTime;
        }
    }

    private bool CheckWinCondition() {
        for (int i = 0; i < ringLocked.GetLength(0); i++) {
            if (ringLocked[i] == false)
                return false;
        }
        return true;
    }

    void Restart() {
		string currentSceneName = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(currentSceneName);
    }

    void Quit () {
		Application.Quit();
	}
}
