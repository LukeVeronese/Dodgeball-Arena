using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : Photon.MonoBehaviour {

    //Character moves with arrows keys or "wasd"
    //Spacebar is for jumping, if it is held down, the character jumps higher

    private PlayerCollisions playerCollisions;
	
	void Start () {
		
        playerCollisions = transform.GetComponent<PlayerCollisions>();
	}

    private int speed = 10;
    private int fallingSpeed = 5;
    private string position = "airborne";
    private string button;
    private float jumpRate = 0.5f;
    private float nextJump = 0.0f;

    void Update () {

        //code for moving
		if (playerCollisions.status != "hit")
		{
			float a = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
			float c = Input.GetAxis("Vertical") * Time.deltaTime * speed;

			transform.Translate(a, 0, c);
		}
	
		//Code for jumping
		if (position == "grounded")
		{
			if(Input.GetButtonDown("Jump"))
			{
				nextJump = Time.time + jumpRate;
			}
			
			if(Input.GetButton("Jump"))
			{
				transform.Translate(Vector3.up * Time.deltaTime * 5);
				button = "pressed";
				
				if (Time.time > nextJump)
				{
					position = "airborne";
				}
			}
		}
		
		if(Input.GetButtonUp("Jump"))
		{
			position = "airborne";
			button = "released";
		}
		
		if (position == "airborne")
		{
			transform.Translate(Vector3.down * Time.deltaTime * fallingSpeed);
		}
	}
	
	void OnCollisionStay(Collision other){
		
		if (other.gameObject.CompareTag("object"))
		{
			position = "grounded";
		}
		
		if (button == "pressed" && other.gameObject.CompareTag("object"))
		{
			nextJump = Time.time + jumpRate;	
		}
	}
	
	void OnCollisionExit(Collision other){
		
		if (button != "pressed" && other.gameObject.name == "Platform")
		{
			position = "airborne";
		}
	}
}
