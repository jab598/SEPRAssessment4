using UnityEngine;
using System.Collections;

/// <summary>
/// Link to Team Eider's website: https://eldertheduck.wordpress.com/
/// Link to Team Shelduck's website: https://shelduck.wordpress.com/
/// Link to Assessment 3 project version: https://eldertheduck.wordpress.com/assessment-3
/// </summary>

public class AI : MonoBehaviour {

	/// <summary>
	/// Speed at which the entitiy moves when not near a player
	/// </summary>
	public float patrolSpeed;

	//rotation that the goose wants to move to
	protected Vector3 wantedRot;

	/// <summary>
	/// Distance from the player the entity is until it chases
	/// </summary>
	public int distanceToChasePlayer;

	/// <summary>
	/// Speed at which the entity moves when chasing the player
	/// </summary>
	public float chasingPlayerMoveSpeed;

	/// <summary>
	/// The entitys animator. This should be attached to the same object as this script
	/// </summary>
	public Animator anim;

	//reference to the player objetc, assigned in the sub-class constructor.
	protected GameObject player;

	//reference to the attached rigidbody. Assigned in the sub class constructor.
	protected Rigidbody rigid;

	public enum State {
		Walking, Chasing
	}

	//state variable to be read by sub class.
	protected State state = State.Walking;

	/// <summary>
	/// Current health of the entity
	/// </summary>
	public int health;

	float rotatedTime;
	Vector3 rotVector = new Vector3 (0, 1, 0);

	/// <summary>
	/// Is this object demented
	/// </summary>
	public bool isDemented;

	bool randomCharge = false;

	/// <summary>
	/// Distances to player.
	/// </summary>
	/// <returns>The to player.</returns>
	float distanceToPlayer () {
		return Mathf.Abs((player.transform.position - this.transform.position).magnitude);
	}

	float dist2DtoPlayer() {
		Vector3 dist = player.transform.position - this.transform.position;
		dist.y = 0;
		return Mathf.Abs (dist.magnitude);
	}

	void StopCharging() {
		randomCharge = false;
		this.state = State.Walking;

	}

	void StartCharge() {
		randomCharge = true;
		this.state = State.Chasing;
		transform.Rotate (new Vector3 (0, Random.Range (0, 360), 0));
		Invoke ("StopCharging", 1);
	}


	protected void Move () {
		if (distanceToPlayer () >= distanceToChasePlayer && !PlayerStates.inst.IsFoggy) {
			if (!randomCharge) {
				rigid.MovePosition (transform.position + (transform.forward * patrolSpeed * Time.deltaTime));
				transform.Rotate (Time.deltaTime * rotVector * 20);
				if (Time.time >= rotatedTime) {
					rotatedTime += Random.Range (5, 10);
					rotVector.y = -rotVector.y;
				}
				RaycastHit r;
				if (Physics.Raycast (transform.position, transform.forward, out r)) {
					if (r.distance <= 2) {
						transform.Rotate (new Vector3 (0, 180, 0));
					}
				}
				if (isDemented) {
					if (Random.Range (0, 300) == 1) {
						StartCharge ();
					}
				}
				state = State.Walking;
			} else {
				rigid.MovePosition (transform.position + (transform.forward * patrolSpeed * Time.deltaTime * 5));
				state = State.Chasing;
			}
		} else {
			Vector3 lpos = player.transform.position;
			lpos.y = transform.position.y;
			transform.LookAt(lpos);
			if(dist2DtoPlayer() >= 1.4f) {
				rigid.MovePosition(transform.position + (transform.forward * Time.deltaTime * chasingPlayerMoveSpeed));
			}
			state = State.Chasing;
		}
	}
}