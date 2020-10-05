using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

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
    private float winTime = 0;
    private float winGravity;
    private float winThrust;

    // Controls
    private bool gamePaused;
    private float debounce = 0;
    private float debounceAmount = 0.2f;
    private float lockDebounce = 0;
    private float lockDebounceAmount = 1.5f;

    // UI
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private TextMeshProUGUI winScores;
    [SerializeField]
    private GameObject gameOverPanel;

    // Audio
    [SerializeField]
    private AudioClip lockAffirm;
    [SerializeField]
    private AudioClip lockFail;
    [SerializeField]
    private AudioSource audioSource;

    UnityEvent m_Leela;

    void Start() {
        if (m_Leela == null)
            m_Leela = new UnityEvent();

        m_Leela.AddListener(Resume);
        m_Leela.AddListener(Restart);
        m_Leela.AddListener(Quit);

        // Set initial ringVelocity for each ring
        for (int i = 0; i < 8; i++) {
            ringVelocity[i] = 360 / ringStartingPeriod[i];
        }
    }

    void Awake() {

    }

    void Update(){
        // Game Paused
        if (gamePaused) {
            Time.timeScale = 0;
            pausePanel.gameObject.SetActive(true);
            Screen.lockCursor = true;
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

            if (winTime != 0) {
                Screen.lockCursor = false;
                if (Input.GetButtonDown("Restart")) {
                    Restart();
                } else if (Input.GetButtonDown("Quit")) {
                    Quit();
                }
            } else {
                Screen.lockCursor = true;
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
                    winThrust += thrustAmount;
                } else if (Input.GetAxis("Horizontal") < -0.02f) {
                    ringVelocity[ringSelected] -= thrustAmount;
                    winThrust += thrustAmount;
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
                if (ringSelected == 0) {
                    if (Mathf.Abs(ringVelocity[0]) >= ringOneMin & Mathf.Abs(ringVelocity[0]) <= ringOneMax) {
                        ringStatus[0] = "Lockable";
                    } else {
                        ringStatus[0] = "Selected";
                    }
                } else {
                    ringStatus[0] = "Spinning";
                }
            } else {
                if (ringSelected == 0) {
                    ringStatus[0] = "LockSelected";
                } else {
                ringStatus[0] = "Locked";
                }
            }

            // For rings 1-7
            for (int i = 1; i < 8; i++) {
                if (i == ringSelected) {
                    if (ringPosition[i] > ringPosition[i-1] - angleWiggle & ringPosition[i] < ringPosition[i-1] + angleWiggle & ringLocked[i-1] & ringLocked[i] == false) {
                        ringStatus[i] = "Lockable";
                    } else if (ringLocked[i]) {
                        ringStatus[i] = "LockSelected";
                    } else {
                        ringStatus[i] = "Selected";
                    }
                } else {
                    if (ringLocked[i]) {
                        ringStatus[i] = "Locked";
                    } else {
                        ringStatus[i] = "Spinning";
                    }
                }
            }

            // If ring goes beyond ringMax, gameover.
            if (CheckOverSpeed()) {
                gameOverPanel.gameObject.SetActive(true);
                winTime = Time.time;
                Time.timeScale = 0;
                Screen.lockCursor = false;
            }

            // Try to Lock
            if (lockDebounce <= 0) {
                if (Input.GetButtonDown("Jump")){
                    // Ring 0
                    if (ringSelected == 0){
                        if (ringStatus[0] == "Lockable") {
                            ringStatus[0] = "Locked";
                            ringLocked[0] = true;
                            audioSource.PlayOneShot(lockAffirm);
                        } else if (ringStatus[0] == "Locked") {
                            ringStatus[0] = "Spinning";
                            ringLocked[0] = false;
                            audioSource.PlayOneShot(lockFail);
                        }
                    // Rings 1-7
                    } else if (ringStatus[ringSelected] == "Lockable") {
                        if (ringVelocity[ringSelected] > ringVelocity[ringSelected-1] - speedWiggle & ringVelocity[ringSelected] < ringVelocity[ringSelected-1] + speedWiggle) {
                            Debug.Log("Spacebar, Locked");
                            ringStatus[ringSelected] = "Locked";
                            ringLocked[ringSelected] = true;
                            ringVelocity[ringSelected] = ringVelocity[ringSelected-1];
                            Debug.Log("Ring " + ringSelected + " Locked");
                            audioSource.PlayOneShot(lockAffirm);
                        } else {
                            Debug.Log("Spacebar, Too fast");
                            ringStatus[ringSelected-1] = "Spinning";
                            ringLocked[ringSelected-1] = false;
                            ringVelocity[ringSelected-1] = ringVelocity[ringSelected];
                            audioSource.PlayOneShot(lockFail);
                        }
                    } else if (ringStatus[ringSelected] == "Locked") {
                        ringStatus[ringSelected] = "Selected";
                        ringLocked[ringSelected] = false;
                        audioSource.PlayOneShot(lockFail);
                    } else {
                        audioSource.PlayOneShot(lockFail);
                    }
                    lockDebounce = lockDebounceAmount;
                }
            }
        }
        // Break previous ring if Velocity is greater than speedWiggle

        // Check for Win Condition
        if (CheckWinCondition () & winTime == 0) {
            winTime = Time.time;
            winGravity = Mathf.Abs(ringVelocity[0])/45;
            Debug.Log("Win condition set! Final time: " + winTime + " seconds. Final Gravity: " + winGravity + " Martian G " + winGravity * .38 + " Earth G!");

            // Change lights to win condition

            // Display win panel
            winScores.text = "Scores:\nTime to Complete: " + Mathf.Floor(winTime) + " seconds\nMars Equivalent Gravity: " + winGravity + " G\nEarth Equivalent Gravity: " + winGravity * .38 + " G\nThrust expended: " + Mathf.Floor(winThrust) + " Degrees/second";
            winPanel.gameObject.SetActive(true);
            Screen.lockCursor = false;
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
            ringUI[i].SendMessage("RingStatus", ringStatus[i]);
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

    private bool CheckOverSpeed() {
        for (int i = 0; i < ringVelocity.GetLength(0); i++) {
            if (Mathf.Abs(ringVelocity[i]) >= ringMax)
                return true;
        }
        return false;

    }

    void Resume() {
        gamePaused = false;
    }

    void Restart() {
		string currentSceneName = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(currentSceneName);
    }

    void Quit () {
		SceneManager.LoadScene("MainMenu");
	}
}
