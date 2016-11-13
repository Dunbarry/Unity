// A Depricated script that integrates motion control with animation control. For purposes of abstraction these are now two seperate scripts.

using UnityEngine;
using System.Collections;

public class CharacterController: MonoBehaviour {

	public float inputDelay = 0.1f;
	public float forwardVel = 3;
	public float rotateVel = 100;
	private float walkVel = 1;

//	Animation
	public Animator anim;
	private float inputH;
	private float inputV;
	private bool run;

	Quaternion targetRotation;
	Rigidbody rBody;
	float forwardInput, turnInput;

	public Quaternion TargetRotation
	{
		get {return targetRotation;}
	}

	void Start()
	{
		targetRotation = transform.rotation;
		if (GetComponent<Rigidbody> ())
			rBody = GetComponent<Rigidbody> ();
		else
			Debug.LogError ("The character needs a rigid body.");

		forwardInput = turnInput = 0;

//		Animation
		anim = GetComponent<Animator> ();
		rBody = GetComponent<Rigidbody> ();
		run = false;
	}

	void GetInput()
	{
		forwardInput = Input.GetAxis("Vertical");
		turnInput = Input.GetAxis("Horizontal");
	}

	void AnimationCues()
	{
		if (Input.GetKeyDown ("1")) {
			anim.Play ("WAIT01", -1, 0f);
		}
		if (Input.GetKeyDown ("2")) {
			anim.Play ("WAIT02", -1, 0f);
		}
		if (Input.GetKeyDown ("3")) {
			anim.Play ("WAIT03", -1, 0f);
		}
		if (Input.GetKeyDown ("4")) {
			anim.Play ("WAIT04", -1, 0.1f);
		}
		if (Input.GetMouseButtonDown (0)) {
			int n = Random.Range (0, 2);
			anim.Play ("DAMAGED0" + n, -1, 0.1f);
		}

		if(Input.GetKey(KeyCode.LeftShift)){
			run = true;
		} else {
			run = false;
		}

		if (Input.GetKey (KeyCode.Space)) {
			anim.SetBool ("jump", true);
		} else {
			anim.SetBool ("jump", false);
		}

		anim.SetBool ("run", run);

		inputH = Input.GetAxis ("Horizontal");
		inputV = Input.GetAxis ("Vertical");

		anim.SetFloat ("inputH", inputH);
		anim.SetFloat ("inputV", inputV);

		float moveX = inputH * 20f * Time.deltaTime;
		float moveZ = inputV * 50f * Time.deltaTime;

		if (moveZ <= 0f) {
			moveX = 0f;
		} else if (run){
			moveX *= 3f;
			moveZ *= 3f;
		}

		rBody.velocity = new Vector3 (moveX, 0f, moveZ);
	}

	void Update()
	{
		GetInput();
		Turn();
		AnimationCues ();
	}

	void FixedUpdate()
	{
		Run();
	}

	void Run()
	{
		if (Mathf.Abs(forwardInput) > inputDelay)
		{
			// move
			if (run) {
				rBody.velocity = transform.forward * forwardInput * forwardVel;
			} else {
				rBody.velocity = transform.forward * forwardInput * walkVel;
			}
		}
		else
			// zero velocity
			rBody.velocity = Vector3.zero;
	}

	void Turn()
	{
		if (Mathf.Abs (turnInput) > inputDelay)
		{
			targetRotation *= Quaternion.AngleAxis (rotateVel * turnInput * Time.deltaTime, Vector3.up);
		}
		transform.rotation = targetRotation;
	}
}
