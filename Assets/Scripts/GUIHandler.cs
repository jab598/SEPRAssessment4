using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// Link to Team Eider's website: https://eldertheduck.wordpress.com/
/// Link to Team Shelduck's website: https://shelduck.wordpress.com/
/// Link to Assessment 3 project version: https://eldertheduck.wordpress.com/assessment-3
/// </summary>

public class GUIHandler : MonoBehaviour {

	//singleton class. referene with GUIHandler.instance. ....
	private static GUIHandler inst = null;
	public static GUIHandler instance {
		get {
			if (inst == null) {
				inst =  FindObjectOfType(typeof (GUIHandler)) as GUIHandler;
			}
			return inst;
		}
	}

	/// <summary>
	/// GUI Element, the points text.
	/// </summary>
	public Text pointsText;

	/// <summary>
	/// A prefab, the object instanced when points or resources are gained. Must have <UpscrollingText> script on it.
	/// </summary>
	public GameObject textUpdateEffect;

	public Text resourceText;

	/// <summary>
	/// GUI Element, the energy bar text
	/// </summary>
	public Text energyText;

	/// <summary>
	/// GUI Element, the actual energy bar panel
	/// </summary>
	public Image energyBar;

	/// <summary>
	/// Max width of the energy bar and healath bar
	/// </summary>
	public int barMaxWidth = 256;

	public Text healthText;
	public Image healthBar;



	/// <summary>
	/// A prefab, the mission text GUI element to be isntanced to build the mission text panel.
	/// </summary>
	public Text missionTextPrefab;

	/// <summary>
	/// The panel behind the mission texts, to be made parent of instanced texts.
	/// </summary>
	public GameObject missionPanel;

	public GameObject pauseMissionPanel;

	public Text timerText;

    //Game Over panel.
    public GameObject gameOverPanel;

	//Private var, list of mission texts for updating.
	List<Text> missionTexts = new List<Text> ();

	//incremented inside the mission constructions and update loops. 
	private int missionTextsPositionOffset;

    float timeStarted;
	
	void OnApplicationQuit() {
		inst = null;
	}

	void Start () {
		//builds the mission texts and updates them.
		buildMissionTexts ();
		updateMissions ();
        timeStarted = Time.time;
	}

	void Update () {
		updateTimer ();
	}

	/// <summary>
	/// Updates the points text.
	/// </summary>
	/// <param name="newPoints">Amount of points to display</param>
	/// <param name="updateText">Text to show sliding up out of the gui.</param>
	/// <param name="mission">If set to <c>true</c>, slides text left instead.</param>
	public void updatePointsText (string newPoints, string updateText, bool mission = false) {
		//update points text, and do different things based on the mission flag
		pointsText.text = "Points: " + newPoints;
		//	instantiate the points update effect, and set its text to the update text
		//	if its a mission, also change the movement vector to scroll right instead.
		if (textUpdateEffect != null && !mission) {
			GameObject p = (GameObject)Instantiate (textUpdateEffect, pointsText.transform.position, Quaternion.identity);
			p.GetComponent<UpscrollingText> ().text = updateText;
		} else if (textUpdateEffect != null && mission) {
			GameObject p = (GameObject)Instantiate (textUpdateEffect, pointsText.transform.position, Quaternion.identity);
			p.GetComponent<UpscrollingText> ().text = updateText;
			p.GetComponent<UpscrollingText>().movementVector = new Vector3(p.GetComponent<UpscrollingText>().movementVector.y,0,0);
		}
	}

	public void updateResourceText(string newResources, string updateText, bool purchased = false) {
		resourceText.text = "Resources: " + newResources;
		if (purchased) {
			GameObject p = (GameObject)Instantiate (textUpdateEffect, resourceText.transform.position, Quaternion.identity);
			p.GetComponent<UpscrollingText>().text = updateText;
			p.GetComponent<Text>().color = Color.red;
		} else {
			GameObject p = (GameObject)Instantiate (textUpdateEffect, resourceText.transform.position, Quaternion.identity);
			p.GetComponent<UpscrollingText>().text = updateText;
		}
	}

	int lastAddedMissionPointer = 0;
	/// <summary>
	/// Builds the mission GUI
	/// </summary>
	public void buildMissionTexts () {
		//reset missiontexts flag, and start constructing the mission GUI
		missionTextsPositionOffset = 0;
		Debug.Log (lastAddedMissionPointer.ToString () + " is last added index, the count is : " + MissionManager.instance.missions.Count.ToString());
		while (lastAddedMissionPointer < MissionManager.instance.missions.Count) {
			Mission m = MissionManager.instance.missionsDict.Values.ToArray()[lastAddedMissionPointer];
			Text t = (Text)Instantiate (missionTextPrefab,Vector2.zero, Quaternion.identity);
			t.gameObject.GetComponent<Text>().text = m.missionText + ": " + m.progress.ToString()+"/"+m.completeProgress.ToString();
			t.gameObject.transform.SetParent(missionPanel.transform);
			t.rectTransform.localPosition = new Vector2(-110, 100-20*(1+lastAddedMissionPointer));
			
			//build pause menu
			Text pauseText = (Text)Instantiate (missionTextPrefab,Vector2.zero, Quaternion.identity);
			pauseText.gameObject.GetComponent<Text>().text = m.missionText + ": " + m.missionDescription;
			pauseText.gameObject.transform.SetParent(pauseMissionPanel.transform);
			pauseText.rectTransform.sizeDelta = new Vector2(500,30);
			pauseText.rectTransform.localPosition = new Vector2(-245, 130-20*(1+lastAddedMissionPointer));

			missionTexts.Add(t);
			lastAddedMissionPointer++;
		}
		/*
		foreach(Mission m in MissionManager.instance.missionsDict.Values) {
			//build mission top left GUI
			//instantiate a text gui element, set the text to the mission text, set its parent to the mission panel,
			//and then position it based on the flag. Add the mission to a list of current missions, and incriment the flag.
			Text t = (Text)Instantiate (missionTextPrefab,Vector2.zero, Quaternion.identity);
			t.gameObject.GetComponent<Text>().text = m.missionText + ": " + m.progress.ToString()+"/"+m.completeProgress.ToString();
			t.gameObject.transform.SetParent(missionPanel.transform);
			t.rectTransform.localPosition = new Vector2(-110, 100-20*(1+missionTextsPositionOffset));

			//build pause menu
			Text pauseText = (Text)Instantiate (missionTextPrefab,Vector2.zero, Quaternion.identity);
			pauseText.gameObject.GetComponent<Text>().text = m.missionText + ": " + m.missionDescription;
			pauseText.gameObject.transform.SetParent(pauseMissionPanel.transform);
			pauseText.rectTransform.sizeDelta = new Vector2(500,30);
			pauseText.rectTransform.localPosition = new Vector2(-245, 130-20*(1+missionTextsPositionOffset));

			missionTextsPositionOffset++;
			missionTexts.Add(pauseText);
			//missionTexts.Add(t);
			Debug.Log ("Rebuilt");
		}*/
	}

	/// <summary>
	/// Updates the missions GUI panel with the current mission states.
	/// </summary>
	public void updateMissions () {
		//set the pointer to zero, and then update each mission text.
		missionTextsPositionOffset = 0;
		foreach (Mission m in MissionManager.instance.missionsDict.Values) {
			Debug.Log ("Updating mission : " + m.missionTag);
			missionTexts[missionTextsPositionOffset].text = m.missionText + ": " + m.progress.ToString() + "/" + m.completeProgress.ToString();
			missionTextsPositionOffset++;
		}
	}

	/// <summary>
	/// Updates the energy bar.
	/// </summary>
	/// <param name="value">Value to set the energy bar to (0-100).</param>
	public void updateEnergyBar (int value) {
		//clamps energy between 0 and 100,
		//then updates the text, and calculates the size of the bar.
		int clampVal = Mathf.Clamp (value, 0, 100);
		energyText.text = "Energy: " + clampVal.ToString() + "%";
		Vector3 newSize = energyBar.rectTransform.sizeDelta;
		newSize.x = clampVal*barMaxWidth/100;
		energyBar.rectTransform.sizeDelta = newSize;
	}

	public void updateHealthBar(int value) {
		int clampVal = Mathf.Clamp (value, 0, 100);
		healthText.text = "Health: " + clampVal.ToString() + "%";
		Vector3 newSize = healthBar.rectTransform.sizeDelta;
		newSize.x = clampVal*barMaxWidth/100;
		healthBar.rectTransform.sizeDelta = newSize;
	}

	public void updateTimer () {
		timerText.text = Mathf.FloorToInt((MissionManager.instance.gameplayLength - Time.time + timeStarted)).ToString();
		if (timerText.text == "0"){
			updateGameOver ();
			Invoke("loadMain",5f);
		}
	
	}

	public void updateGameOver () {
        gameOverPanel.SetActive(true);
	}

	public void loadMain(){
		Application.LoadLevel ("mainmenu");
	}

}