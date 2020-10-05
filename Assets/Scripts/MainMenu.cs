using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    UnityEvent m_MainMenu;

    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private GameObject storyPanel;
    [SerializeField]
    private GameObject creditsPanel;

    void Start() {
        if (m_MainMenu == null)
            m_MainMenu = new UnityEvent();

        m_MainMenu.AddListener(NewGame);
        m_MainMenu.AddListener(Back);
        m_MainMenu.AddListener(Story);
        m_MainMenu.AddListener(Credits);
        m_MainMenu.AddListener(Quit);

    }

    void Update() {
        if (Input.GetButtonDown("Cancel")){
			Back();
		} else if (Input.GetButtonDown("Quit")) {
            Quit();
        } else if (Input.GetButtonDown("Story")) {
            Story();
        } else if (Input.GetButtonDown("Credits")) {
            Credits();
        } else if (Input.GetButtonDown("NewGame")) {
            NewGame();
        } else if (Input.GetButtonDown("Submit")) {
            NewGame();
        } else if (Input.GetButtonDown("Back")) {
            Back();
        }
    }

    void NewGame() {
        SceneManager.LoadScene("Sagittarius240");
    }

    void Back() {
        mainPanel.gameObject.SetActive(true);
        storyPanel.gameObject.SetActive(false);
        creditsPanel.gameObject.SetActive(false);
    }

    void Story() {
        mainPanel.gameObject.SetActive(false);
        storyPanel.gameObject.SetActive(true);
        creditsPanel.gameObject.SetActive(false);
    }

    void Credits() {
        mainPanel.gameObject.SetActive(false);
        storyPanel.gameObject.SetActive(false);
        creditsPanel.gameObject.SetActive(true);
    }

    void Quit() {
		Application.Quit();
    }
}
