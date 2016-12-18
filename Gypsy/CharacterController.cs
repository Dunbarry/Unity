using UnityEngine;
using System.Collections;

public class CharacterController: MonoBehaviour {
//	Movement
	public float runSpeed = 9;
	public float walkSpeed = 0;

	public float turnSmoothTime = 0.2f;
	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;

//	Gravity
	public float distToGrounded = 1.75f;
	public LayerMask ground;
	public Vector3 velocity = Vector3.zero;

	bool Grounded()
	{
		return Physics.Raycast (transform.position, Vector3.down, distToGrounded, ground);
	}

//	private float verticalVelocity;
	private float gravity = 12f;
	private CharacterController controller;

	//	Animation
	public Animator anim;
	Transform cameraT;
	private bool run;
	private bool running;
	Rigidbody rBody;

	//Keys
	public int count;
//	KeyCompletion keyComp;

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
//		keyComp = GetComponent<KeyCompletion> ();
		run = true;

		//Keys
		count = 0;
	}

	void Update()
	{
		velocity.y -= gravity * Time.deltaTime;

		if (Grounded ()) {
			//zero out velocity.y
			velocity.y = 0;
//			Debug.LogError ("Grounded.");
		}

		transform.position += velocity * Time.deltaTime;

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

//		//Blocking
//		if (Input.GetKey (KeyCode.LeftShift)) {
//			anim.Play("ShieldAngle", -1, 0f);
//			anim.SetBool ("Guarding", true);
//
//		}
//		if (Input.GetKeyUp (KeyCode.LeftShift)) {
//			anim.Play("ShieldRelease", -1, 0f);
//			anim.SetBool ("Guarding", false);
//		}

		if(Input.GetKeyDown("1")){
			anim.Play("Visor", -1, 0f);
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Keys")) {
			other.gameObject.SetActive (false);
			count = count + 1;
			Debug.Log ("Count is:" +count);
		}
		if (other.gameObject.CompareTag ("Door")) {
			if (count == 3) { //If all three keys have been collected
				KeyCompletion keyComp = other.GetComponent<KeyCompletion> ();
				keyComp.anim.SetBool ("HasKeys", true);
			}
		}
	}
}
