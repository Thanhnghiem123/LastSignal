using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementScript : MonoBehaviour {
	Rigidbody rb;

	[Tooltip("Current players speed")]
	public float currentSpeed;

	[Tooltip("Assign players camera here")]
	[HideInInspector]public Transform cameraMain, secondCamera;
	[Tooltip("Force that moves player into jump")]
	public float jumpForce = 15000;

	[Tooltip("Position of the camera inside the player")]
	[HideInInspector]
	public Vector3 cameraPosition;

    [Tooltip("The maximum speed you want to achieve")]
    public int maxSpeed = 5;

    [Tooltip("The higher the number the faster it will stop")]
    public float deaccelerationSpeed = 15.0f;

    [Tooltip("Force that is applied when moving forward or backward")]
    public float accelerationSpeed = 500000.0f;

    [Tooltip("Tells us weather the player is grounded or not.")]
    public bool grounded;

    RaycastHit hitInfo;
    private float meleeAttack_cooldown;
    private string currentWeapo;

    [Tooltip("Put 'Player' layer here")]
    [Header("Shooting Properties")]
    private LayerMask ignoreLayer;//to ignore player layer

    Ray ray1, ray2, ray3, ray4, ray5, ray6, ray7, ray8, ray9;
    private float rayDetectorMeeleSpace = 0.15f;
    private float offsetStart = 0.05f;

    [Tooltip("Put BulletSpawn gameobject here, palce from where bullets are created.")]
    [HideInInspector]
    public Transform bulletSpawn; //from here we shoot a ray to check where we hit him;

    public bool been_to_meele_anim = false;

    [Header("BloodForMelleAttaacks")]
    RaycastHit hit;//stores info of hit;

    [Tooltip("Put your particle blood effect here.")]
    public GameObject bloodEffect;//blod effect prefab;




    private Vector3 slowdownV;
    private Vector2 horizontalMovement;
    private GameObject myBloodEffect;

    public float cameraSmoothSpeed; // Tốc độ mượt của camera
    public float shakeAmount; // Cường độ dao động khi di chuyển
    public float shakeAmountSprint ; // Cường độ dao động khi di chuyển


    /*
	 * Getting the Players rigidbody component.
	 * And grabbing the mainCamera from Players child transform.
	 */
    void Awake(){
        //transform.rotation = Quaternion.identity;
        rb = GetComponent<Rigidbody>();
		cameraMain = transform.Find("Main Camera").transform;
		secondCamera = cameraMain.Find("Camera").transform;
		bulletSpawn = cameraMain.Find ("BulletSpawn").transform;
		ignoreLayer = 1 << LayerMask.NameToLayer ("Player");
    }


    /*
	* Update loop calling other stuff
	*/
    void Update()
    {

        Jumping();

        //Crouching();

        WalkingSound();

        // Di chuyển camera bodycam
        if (secondCamera != null)
        {
            // Lấy góc quay hiện tại và tính toán targetRotation
            Quaternion targetRotation = secondCamera.localRotation;
			float shakeTemp = shakeAmount;

            // Thêm hiệu ứng dao động nhẹ (khi player di chuyển)
            if (Input.GetAxis("Vertical") > 0)
            {

				if (Input.GetKey(KeyCode.LeftShift))
				{
                    shakeTemp = shakeAmountSprint;
				}
				else
				{
                    shakeTemp = shakeAmount;
				}
                // Tạo hiệu ứng dao động ngẫu nhiên cho các góc xoay
                float shakeX = Random.Range(-shakeTemp, shakeTemp);
                float shakeY = Random.Range(-shakeTemp, shakeTemp);
                float shakeZ = Random.Range(-shakeTemp, shakeTemp);

                // Tạo một quaternion xoay nhẹ và áp dụng vào targetRotation
                Quaternion shakeRotation = Quaternion.Euler(shakeX, shakeY, 0);

                // Áp dụng dao động vào targetRotation
                targetRotation = targetRotation * shakeRotation;

                // Làm mượt quá trình xoay
                secondCamera.localRotation = Quaternion.Slerp(secondCamera.localRotation, targetRotation, cameraSmoothSpeed);
            }
            else
            {
                

                // Làm mượt chuyển động về góc (0, 0, 0) khi không di chuyển
                secondCamera.localRotation = Quaternion.Slerp(secondCamera.localRotation, Quaternion.Euler(0f, 0f, 0f), 0.1f);
            }



        }
    }

    /*
	* Raycasting for meele attacks and input movement handling here.
	*/
    void FixedUpdate(){
		RaycastForMeleeAttacks ();

		PlayerMovementLogic ();
	}

    



    /*
	* Accordingly to input adds force and if magnitude is bigger it will clamp it.
	* If player leaves keys it will deaccelerate
	*/
    void PlayerMovementLogic()
	{
		currentSpeed = rb.velocity.magnitude;
		horizontalMovement = new Vector2(rb.velocity.x, rb.velocity.z);
		if (horizontalMovement.magnitude > maxSpeed)
		{
			horizontalMovement = horizontalMovement.normalized;
			horizontalMovement *= maxSpeed;
		}
		rb.velocity = new Vector3(
			horizontalMovement.x,
			rb.velocity.y,
			horizontalMovement.y
		);
		if (grounded)
		{
			rb.velocity = Vector3.SmoothDamp(rb.velocity,
				new Vector3(0, rb.velocity.y, 0),
				ref slowdownV,
				deaccelerationSpeed);
		}

		if (grounded)
		{
			rb.AddRelativeForce(Input.GetAxis("Horizontal") * accelerationSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * accelerationSpeed * Time.deltaTime);
        }
		else
		{
			rb.AddRelativeForce(Input.GetAxis("Horizontal") * accelerationSpeed / 2 * Time.deltaTime, 0, Input.GetAxis("Vertical") * accelerationSpeed / 2 * Time.deltaTime);

        }
		/*
		 * Slippery issues fixed here
		 */
		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
		{
			deaccelerationSpeed = 0.5f;
		}
		else
		{
			deaccelerationSpeed = 0.1f;
		}
	}
    /*
	* Handles jumping and ads the force and sounds.
	*/
    void Jumping(){
		if (Input.GetKeyDown (KeyCode.Space) && grounded) {
			rb.AddRelativeForce (Vector3.up * jumpForce);
			if (_jumpSound)
				_jumpSound.Play ();
			else
				print ("Missig jump sound.");
			_walkSound.Stop ();
			_runSound.Stop ();
		}
	}
	//end update

	/*
	* Checks if player is grounded and plays the sound accorindlgy to his speed
	*/
	void WalkingSound(){
		if (_walkSound && _runSound) {
			if (RayCastGrounded ()) { //for walk sounsd using this because suraface is not straigh			
				if (currentSpeed > 1) {
					//				print ("unutra sam");
					if (maxSpeed == 3) {
						//	print ("tu sem");
						if (!_walkSound.isPlaying) {
							//	print ("playam hod");
							_walkSound.Play ();
							_runSound.Stop ();
						}					
					} else if (maxSpeed == 5) {
						//	print ("NE tu sem");

						if (!_runSound.isPlaying) {
							_walkSound.Stop ();
							_runSound.Play ();
						}
					}
				} else {
					_walkSound.Stop ();
					_runSound.Stop ();
				}
			} else {
				_walkSound.Stop ();
				_runSound.Stop ();
			}
		} else {
			print ("Missing walk and running sounds.");
		}

	}
	/*
	* Raycasts down to check if we are grounded along the gorunded method() because if the
	* floor is curvy it will go ON/OFF constatly this assures us if we are really grounded
	*/
	private bool RayCastGrounded(){
		RaycastHit groundedInfo;
		if(Physics.Raycast(transform.position, transform.up *-1f, out groundedInfo, 1, ~ignoreLayer)){
			Debug.DrawRay (transform.position, transform.up * -1f, Color.red, 0.0f);
			if(groundedInfo.transform != null){
				//print ("vracam true");
				return true;
			}
			else{
				//print ("vracam false");
				return false;
			}
		}
		//print ("nisam if dosao");

		return false;
	}

	/*
	* If player toggle the crouch it will scale the player to appear that is crouching
	*/
	//void Crouching(){
	//	if(Input.GetKey(KeyCode.C)){
	//		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1,0.6f,1), Time.deltaTime * 15);
	//	}
	//	else{
	//		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1,1,1), Time.deltaTime * 15);

	//	}
	//}


	
	/*
	* checks if our player is contacting the ground in the angle less than 60 degrees
	*	if it is, set groudede to true
	*/
	void OnCollisionStay(Collision other){
		foreach(ContactPoint contact in other.contacts){
			if(Vector2.Angle(contact.normal,Vector3.up) < 60){
				grounded = true;
			}
		}
	}
	/*
	* On collision exit set grounded to false
	*/
	void OnCollisionExit ()
	{
		grounded = false;
	}


	
	/*
	* This method casts 9 rays in different directions. ( SEE scene tab and you will see 9 rays differently coloured).
	* Used to widley detect enemy infront and increase meele hit detectivity.
	* Checks for cooldown after last preformed meele attack.
	*/


	
	private void RaycastForMeleeAttacks()
	{




		if (meleeAttack_cooldown > -5)
		{
			meleeAttack_cooldown -= 1 * Time.deltaTime;
		}


		if (GetComponent<GunInventory>().currentGun)
		{
			if (GetComponent<GunInventory>().currentGun.GetComponent<GunScript>())
				currentWeapo = "gun";
		}

		//middle row
		ray1 = new Ray(bulletSpawn.position + (bulletSpawn.right * offsetStart), bulletSpawn.forward + (bulletSpawn.right * rayDetectorMeeleSpace));
		ray2 = new Ray(bulletSpawn.position - (bulletSpawn.right * offsetStart), bulletSpawn.forward - (bulletSpawn.right * rayDetectorMeeleSpace));
		ray3 = new Ray(bulletSpawn.position, bulletSpawn.forward);
		//upper row
		ray4 = new Ray(bulletSpawn.position + (bulletSpawn.right * offsetStart) + (bulletSpawn.up * offsetStart), bulletSpawn.forward + (bulletSpawn.right * rayDetectorMeeleSpace) + (bulletSpawn.up * rayDetectorMeeleSpace));
		ray5 = new Ray(bulletSpawn.position - (bulletSpawn.right * offsetStart) + (bulletSpawn.up * offsetStart), bulletSpawn.forward - (bulletSpawn.right * rayDetectorMeeleSpace) + (bulletSpawn.up * rayDetectorMeeleSpace));
		ray6 = new Ray(bulletSpawn.position + (bulletSpawn.up * offsetStart), bulletSpawn.forward + (bulletSpawn.up * rayDetectorMeeleSpace));
		//bottom row
		ray7 = new Ray(bulletSpawn.position + (bulletSpawn.right * offsetStart) - (bulletSpawn.up * offsetStart), bulletSpawn.forward + (bulletSpawn.right * rayDetectorMeeleSpace) - (bulletSpawn.up * rayDetectorMeeleSpace));
		ray8 = new Ray(bulletSpawn.position - (bulletSpawn.right * offsetStart) - (bulletSpawn.up * offsetStart), bulletSpawn.forward - (bulletSpawn.right * rayDetectorMeeleSpace) - (bulletSpawn.up * rayDetectorMeeleSpace));
		ray9 = new Ray(bulletSpawn.position - (bulletSpawn.up * offsetStart), bulletSpawn.forward - (bulletSpawn.up * rayDetectorMeeleSpace));

		Debug.DrawRay(ray1.origin, ray1.direction, Color.cyan);
		Debug.DrawRay(ray2.origin, ray2.direction, Color.cyan);
		Debug.DrawRay(ray3.origin, ray3.direction, Color.cyan);
		Debug.DrawRay(ray4.origin, ray4.direction, Color.red);
		Debug.DrawRay(ray5.origin, ray5.direction, Color.red);
		Debug.DrawRay(ray6.origin, ray6.direction, Color.red);
		Debug.DrawRay(ray7.origin, ray7.direction, Color.yellow);
		Debug.DrawRay(ray8.origin, ray8.direction, Color.yellow);
		Debug.DrawRay(ray9.origin, ray9.direction, Color.yellow);

		//if (GetComponent<GunInventory> ().currentGun) {
		//	if (GetComponent<GunInventory> ().currentGun.GetComponent<GunScript> ().meeleAttack == false) {
		//		been_to_meele_anim = false;
		//	}
		//	if (GetComponent<GunInventory> ().currentGun.GetComponent<GunScript> ().meeleAttack == true && been_to_meele_anim == false) {
		//		been_to_meele_anim = true;
		//		//	if (isRunning == false) {
		//		StartCoroutine ("MeeleAttackWeaponHit");
		//		//	}
		//	}
		//}
		if (meleeAttack_cooldown > -5)
		{
			meleeAttack_cooldown -= 1 * Time.deltaTime;
		}

		// Kiểm tra xem vũ khí hiện tại là súng hay kiếm và xử lý tương ứng
		if (GetComponent<GunInventory>().currentGun != null)
		{
			if (GetComponent<GunInventory>().currentGun.GetComponent<GunScript>() != null)
			{
				currentWeapo = "gun"; // Nếu là súng, sử dụng GunScript
			}
			else if (GetComponent<GunInventory>().currentGun.GetComponent<SwordScript>() != null)
			{
				currentWeapo = "sword"; // Nếu là kiếm, sử dụng SwordScript
			}
		}


		// Nếu vũ khí hiện tại là kiếm
		if (currentWeapo == "sword")
		{
            // Kiểm tra trạng thái tấn công cận chiến của súng
            if (GetComponent<GunInventory>().currentGun.GetComponent<SwordScript>().isAttacking == false)
            {
                been_to_meele_anim = false;
                Debug.Log("Su dung kiemmmmmmmmmmmmmmmmmmmmmm11111111111111111111111");
            }
            if (GetComponent<GunInventory>().currentGun.GetComponent<SwordScript>().isAttacking == true && been_to_meele_anim == false)
            {
                been_to_meele_anim = true;
                StartCoroutine("MeeleAttackWeaponHit");
                
            }
            
		}

		// Nếu vũ khí hiện tại là súng
		if (currentWeapo == "gun")
		{
			// Kiểm tra trạng thái tấn công cận chiến của súng
			if (GetComponent<GunInventory>().currentGun.GetComponent<GunScript>().meeleAttack == false)
			{
				been_to_meele_anim = false;
			}
			if (GetComponent<GunInventory>().currentGun.GetComponent<GunScript>().meeleAttack == true && been_to_meele_anim == false)
			{
                Debug.Log("Su dung sunggggggggggggggggggggggggggggggg");
                been_to_meele_anim = true;
				StartCoroutine("MeeleAttackWeaponHit");
			}
		}

	}

	/*
	 *Method that is called if the waepon hit animation has been triggered the first time via Q input
	 *and if is, it will search for target and make damage
	 */
	IEnumerator MeeleAttackWeaponHit(){
		if (Physics.Raycast (ray1, out hitInfo, 2f, ~ignoreLayer) || Physics.Raycast (ray2, out hitInfo, 2f, ~ignoreLayer) || Physics.Raycast (ray3, out hitInfo, 2f, ~ignoreLayer)
			|| Physics.Raycast (ray4, out hitInfo, 2f, ~ignoreLayer) || Physics.Raycast (ray5, out hitInfo, 2f, ~ignoreLayer) || Physics.Raycast (ray6, out hitInfo, 2f, ~ignoreLayer)
			|| Physics.Raycast (ray7, out hitInfo, 2f, ~ignoreLayer) || Physics.Raycast (ray8, out hitInfo, 2f, ~ignoreLayer) || Physics.Raycast (ray9, out hitInfo, 2f, ~ignoreLayer)) {
			//Debug.DrawRay (bulletSpawn.position, bulletSpawn.forward + (bulletSpawn.right*0.2f), Color.green, 0.0f);
			if (hitInfo.transform.tag=="Dummie") {
                hitInfo.collider.GetComponent<Enemy>().GetHit();
                hitInfo.collider.GetComponent<EnemyHealth>().TakeDamage(10);
                Transform _other = hitInfo.transform.root.transform;
				if (_other.transform.tag == "Dummie") {
					print ("hit a dummie");
                    
                }
                InstantiateBlood(hitInfo,false);
			}
		}
		yield return new WaitForEndOfFrame ();
	}

    
	/*
	* Upon hitting enemy it calls this method, gives it raycast hit info 
	* and at that position it creates our blood prefab.
	*/
	void InstantiateBlood (RaycastHit _hitPos,bool swordHitWithGunOrNot) {		

		if (currentWeapo == "gun") {
			GunScript.HitMarkerSound ();

			if (_hitSound)
				_hitSound.Play ();
			else
				print ("Missing hit sound");
			
			if (!swordHitWithGunOrNot) {
				if (bloodEffect)
					Instantiate (bloodEffect, _hitPos.point, Quaternion.identity);
				else
					print ("Missing blood effect prefab in the inspector.");
			}
		} 
		if(currentWeapo == "sword")
		{
			if(_hitSound)
			{
				_hitSound.Play();
			}
            if (!swordHitWithGunOrNot)
            {
                if (bloodEffect)
                    Instantiate(bloodEffect, _hitPos.point, Quaternion.identity);
                else
                    print("Missing blood effect prefab in the inspector.");
            }
        }
	}
	


	[Header("Player SOUNDS")]
	[Tooltip("Jump sound when player jumps.")]
	public AudioSource _jumpSound;
	[Tooltip("Sound while player makes when successfully reloads weapon.")]
	public AudioSource _freakingZombiesSound;
	[Tooltip("Sound Bullet makes when hits target.")]
	public AudioSource _hitSound;
	[Tooltip("Walk sound player makes.")]
	public AudioSource _walkSound;
	[Tooltip("Run Sound player makes.")]
	public AudioSource _runSound;
}

