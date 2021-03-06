﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Link to Team Eider's website: https://eldertheduck.wordpress.com/
/// Link to Team Shelduck's website: https://shelduck.wordpress.com/
/// Link to Assessment 3 project version: https://eldertheduck.wordpress.com/assessment-3
/// </summary>

public class PauseMenu : MonoBehaviour {

	/// <summary>
	/// The name of the pause button in the input manager.
	/// </summary>
	public string pauseButtonName;

	/// <summary>
	/// Main pause panel
	/// </summary>
	public GameObject pauseMenuPanel;


	// New for Assessment 3
	public GameObject powerUpPanel;
	public GameObject missionsPanel;

	public Text breadText;
	public Text lazerText;

	bool breadBought = false;
	bool lazerBought = false;
	// end of new for assessment 3

	//new assessment4
	public GameObject cheatsPanel;
	public Toggle infiniteHealthToggle;
	public Toggle infiniteResourceToggle;
	public Toggle superSpeedToggle;
	public Toggle fireRateToggle;

	public bool disableInput;
	//end of new 

	bool paused = false;

	// Use this for initialization
	void Start () {
		//unpause on start
		pauseMenuPanel.SetActive (false);
		cheatsPanel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!disableInput) {
			if (Input.GetButtonDown (pauseButtonName)) {
				if (Tutorial.started == true) {
					if (!paused) {
						pauseGame ();
					} else {
						unpauseGame ();
					}
				}
			}
		}
		if (paused) {
			if (Input.GetButtonDown ("cheatsMenu")) {
				if (Tutorial.started == true) {
					setCheatsPanel(true);
				}
			}
		}
	}

	public void setCheatsPanel(bool value) {
		//pauseMenuPanel.SetActive(!value);
		//powerUpPanel.SetActive (!value);
		//missionsPanel.SetActive (!value);
		cheatsPanel.SetActive (value);
		disableInput = value;
	}

	public void setInfiniteHealth() {
		PlayerStates.inst.infiniteHealth = infiniteHealthToggle.isOn;
	}

	public void setInfiniteResource() {
		PlayerStates.inst.infiniteResource = infiniteResourceToggle.isOn;
	}

	public void GiveFullHealth(){
		PlayerStates.inst.alterHealth (100);
	}

	public void ToggleSuperSpeed() {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().setSuperSpeed(superSpeedToggle.isOn);
	}

	public void ToggleSuperFireRate() {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShooting> ().SetFireRateCheat (fireRateToggle.isOn);
	}


	public void pauseGame() {
		//stop time, show menu and cursor
		Time.timeScale = 0;
		paused = true;
		pauseMenuPanel.SetActive (true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		powerUpPanel.SetActive (false);
		missionsPanel.SetActive (true);
	}

	public void unpauseGame () {
		//resume time, hide menu and cursor
		paused = false;
		Time.timeScale = 1;
		pauseMenuPanel.SetActive (false);
		cheatsPanel.SetActive (false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	/*
	 * 
	 * The below methods handle each button press in the pause menu. 
	 * Theyre self explanetory.
	 * 
	 * */

	public void pressRestart () {
		Time.timeScale = 1;
		Application.LoadLevel (1);
	}

	public void pressQuitToMenu () {
		Time.timeScale = 1;
		Application.LoadLevel (0);
	}

	public void pressExitGame () {
		Application.Quit ();
	}



	//New for Assessment 3

	public void pressPowerUps(){
		if (powerUpPanel.activeSelf) {
			powerUpPanel.SetActive (false);
			missionsPanel.SetActive (true);
		} else {
			powerUpPanel.SetActive (true);
			missionsPanel.SetActive (false);
		}
	}
	public void pressBreadButton(){
		if (PlayerStates.inst.resources >= 50 && !breadBought) {
			breadBought = true;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShooting> ().multipleBreadUnlocked = true;
			PlayerStates.inst.alterResources(-50);
			GUIHandler.instance.updateResourceText(PlayerStates.inst.resources.ToString(), "-50", true);
			breadText.text = "Already Bought";
		}
	}

	public void pressLazerButton(){
		if (PlayerStates.inst.resources >= 60 && !lazerBought) {
			lazerBought = true;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShooting> ().laserUnlocked = true;
			PlayerStates.inst.alterResources(-60);
			GUIHandler.instance.updateResourceText(PlayerStates.inst.resources.ToString(), "-60", true);
			lazerText.text = "Already Bought";
		}
	}

	//end of new for assessment 3
}
