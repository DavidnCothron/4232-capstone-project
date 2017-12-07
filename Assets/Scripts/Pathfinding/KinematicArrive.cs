using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicArrive : MonoBehaviour {

	private Vector3 target;
	private KinematicSteering steering;
	[SerializeField] private float maxSpeed;
	[SerializeField] private float turnSpeed;
	[SerializeField] private float radius;
	private const float timeToTarget = 0.1f;
	private bool arrived;
	private PlayerPlatformerController playerControl;

	// Use this for initialization
	void Start () {
		arrived = false;
		playerControl = GameObject.Find("Player").GetComponent<PlayerPlatformerController>();
		if (playerControl != null) maxSpeed = playerControl.maxSpeed;
	}

	public KinematicSteering getSteering() {
		if (steering == null)
			steering = new KinematicSteering ();
		steering.velocity = target - this.transform.position;
		//If obj has arrived at its destination
		if (steering.velocity.magnitude <= radius) {

			arrived = true;
			steering.velocity = Vector3.zero;
			setOrientations (steering);
			return steering;
		}

		steering.velocity /= timeToTarget;

		//if obj exceeds maxspeed, reduce speed to max speed
		if (steering.velocity.magnitude > maxSpeed) {
			steering.velocity.Normalize ();
			steering.velocity *= maxSpeed;
		}
		arrived = false;
		//The following ensures rotation current rotation stays after arrival
		//if (Mathf.Abs ((target - this.transform.position).magnitude) > 1.6f)
			//steering.rotation.SetLookRotation (target - this.transform.position);
		return steering;
	}

	public Vector3 getSteeringVelocity() {
		if (steering != null) return steering.velocity;
		return Vector3.zero;
	}

	public Vector3 getTarget() {
		return target;
	}

	public void setOrientations(KinematicSteering steering){
		this.GetComponent<Rigidbody2D> ().velocity = steering.velocity;
		//Rotation does not need to be used for this game as of now.
		//this.transform.rotation = Quaternion.Slerp (Quaternion.Euler (new Vector3 (0f, 0f, 0f)), Quaternion.Euler (new Vector3 (0f, 0f, 0f)), 1f);
	}

	public void setTarget (Vector3 t){
		target = t;
	}

	public bool getArrived() {
		return arrived;
	}

	//Class to hold steering information
	public class KinematicSteering {
		public Vector3 velocity;
		public Quaternion rotation;
		public KinematicSteering() {
			velocity = new Vector3(0f,0f,0f);
			rotation = new Quaternion();
		}
	}
}
