using UnityEngine;
using System.Collections;

public class CharacterController: MonoBehaviour {

	public float runSpeed = 9;
	public float walkSpeed = 0;

	public float turnSmoothTime = 0.2f;
	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;

	//	Animation
	public Animator anim;
	Transform cameraT;
	private bool run;
	private bool running;
	Rigidbody rBody;

	void Start()
	{
		if (GetComponent<Rigidbody> ())
			rBody = GetComponent<Rigidbody> ();
		else
			Debug.LogError ("The character needs a rigid body.");
		cameraT = Camera.main.transform;

		//		Animation
		anim = GetComponent<Animator> ();
		rBody = GetComponent<Rigidbody> ();
		run = true;
	}

	void Update()
	{
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector2 inputDir = input.normalized;

		if (inputDir != Vector2.zero) {
			float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
		}

		run = running = true;
		float targetSpeed = ((running) ? runSpeed : 0) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

		transform.Translate (transform.forward * currentSpeed * Time.deltaTime, Space.World);

		float charSpeed = ((running) ? 1 : .5f) * inputDir.magnitude;
		anim.SetFloat ("charSpeed", charSpeed, speedSmoothTime, Time.deltaTime);
	}
}
