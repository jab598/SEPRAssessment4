using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Handles cheating
public class CheatHandler : MonoBehaviour {

	//text box that the player enteres the cheat in
	public InputField cheatInput;
	//for giving feedback to the player
	public Text statusText;

	//cheat status'
	bool superSpeedOn = false;
	bool morePickupsOn = false;
	bool infiniteEnergy = false;
	bool doubleFireRate = false;
	bool useDiffExpl = false;

	// Use this for initialization
	void Start () {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShooting> ().projectile.GetComponent<Projectile> ().useDiffExplosion = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Code the player has entered
	/// </summary>
	/// <returns>Code the player has entered</returns>
	public string cheatCode() {
		return cheatInput.text;
	}

	/// <summary>
	/// Changes the status text
	/// </summary>
	public void SetStatus(string status) {
		statusText.text = status;
	}

	/// <summary>
	/// Reads the input box and handles the cheat.
	/// </summary>
	public void GetCheat() {
		string code = cheatCode ();
		if (code.StartsWith ("superspeed")) {
			SuperSpeedEntered ();
		} else if (code.StartsWith ("morepickups")) {
			MorePickupsEntered ();
		} else if (code.StartsWith ("unlockall")) {
			UnlockAllEntered ();
		} else if (code.StartsWith ("infiniteenergy")) {
			InfiniteEnergyEntered ();
		} else if (code.StartsWith ("doublefirerate")) {
			DoubleFireRateEntered ();
		} else if (code.StartsWith ("bigexplosion")) {
			BigExplosionEntered ();
		} else if (code.StartsWith ("giveresource")) {
			GiveResourceEntered ();
		} else if (code.StartsWith ("enterall")) {
			SuperSpeedEntered ();
			MorePickupsEntered ();
			UnlockAllEntered ();
			InfiniteEnergyEntered ();
			DoubleFireRateEntered ();
			BigExplosionEntered ();
			GiveResourceEntered ();
			SetStatus ("Entered all codes");
		}else {
			SetStatus ("Cheat not found");
		}
	}

	//close cheat panel
	public void ClosePanel() {
		GameObject.FindObjectOfType<PauseMenu> ().setCheatsPanel (false);
	}

	//toggles super speed
	public void SuperSpeedEntered() {
		superSpeedOn = !superSpeedOn;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().setSuperSpeed (superSpeedOn);
		SetStatus ("Super speed set to " + superSpeedOn);
	}

	//toggles increased pickups
	public void MorePickupsEntered() {
		morePickupsOn = !morePickupsOn;
		Spawner.inst.maxCollectables = morePickupsOn ? 50 : 20;
		SetStatus ("More pickups set to " + morePickupsOn);
	}

	//Unlocks all weapons
	public void UnlockAllEntered() {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShooting> ().multipleBreadUnlocked = true;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShooting> ().laserUnlocked = true;
		SetStatus ("All weapons unlocked");
	}

	//toggles infinite energy
	public void InfiniteEnergyEntered() {
		infiniteEnergy = !infiniteEnergy;
		PlayerStates.inst.infiniteEnergy = infiniteEnergy;
		SetStatus ("Infinite energy set to " + infiniteEnergy);
	}

	//toggles double fire rate
	public void DoubleFireRateEntered() {
		doubleFireRate = !doubleFireRate;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShooting> ().SetFireRateCheat (doubleFireRate);
		SetStatus ("Double fire rate set to " + doubleFireRate);
	}

	//toggles big explosions
	public void BigExplosionEntered() {
		useDiffExpl = !useDiffExpl;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShooting> ().projectile.GetComponent<Projectile> ().useDiffExplosion = useDiffExpl;
		SetStatus ("Use different explosion set to " + useDiffExpl);
	}

	//gives 100 resources
	public void GiveResourceEntered() {
		PlayerStates.inst.resources += 100;
		GUIHandler.instance.updateResourceText (PlayerStates.inst.resources.ToString(), "+100 CHEAT!");
		SetStatus ("Gave 100 resources");
	}
}
