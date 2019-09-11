using UnityEngine;
using System.Collections;

//Handles the player's collisions with other objects

public class PlayerCollisions : Photon.MonoBehaviour {

    private Transform playerBody;

	public int speed = 6;
	
	public Vector3 normalDirection;
	public string status;

	public PlayerCollisions playerCollisions;

	public bool alive = true;

    public PlayerScore playerScore;

    void Start()
    {
        playerCollisions = transform.root.GetComponent<PlayerCollisions>();
       
        if (transform == transform.root)
        {
            playerBody = transform.GetChild(1);
            //makes sure all parts are facing the right direction
            playerBody.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    void Update () {	

		if (playerCollisions.status == "hit" && gameObject.name == transform.root.name)
        {
            StartCoroutine(Hit());	
		}								
	}

	//player is knocked back when hit by projectile
	public IEnumerator Hit() {
		
		transform.root.Translate (playerCollisions.normalDirection * Time.deltaTime * playerCollisions.speed, Space.World);
		yield return new WaitForSeconds(1f);

        playerCollisions.status = "idle";
    }
	
	void OnCollisionEnter(Collision other){

        playerCollisions = transform.root.GetComponent<PlayerCollisions>();
        
        //collisions with projectiles
        if (other.gameObject.CompareTag("projectile") || other.gameObject.CompareTag("launched")) {

            //red projectiles explode when collided with
            if (other.gameObject.layer == 8){
					
				if (gameObject.name != "grabbing" || transform.root.tag == "holding") {

                    playerCollisions.normalDirection = new Vector3(other.contacts[0].normal.x, 0, other.contacts[0].normal.z).normalized;
                    playerCollisions.speed = 40;
                    playerCollisions.status = "hit";
                }
			}
			else //if projectile was launched, player is knocked back at a faster speed
			if (other.gameObject.CompareTag("launched"))
			{
				if (gameObject.name != "grabbing" || transform.root.tag == "holding") {

                    playerCollisions.normalDirection = new Vector3(other.contacts[0].normal.x, 0, other.contacts[0].normal.z).normalized;
                    playerCollisions.speed = 30;
                    playerCollisions.status = "hit";
                }	
			}
			else //normal collision with another projectile
			if (gameObject.name != "grabbing" || transform.root.tag == "holding") {
               
                playerCollisions.normalDirection = new Vector3(other.contacts[0].normal.x, 0, other.contacts[0].normal.z).normalized;
                playerCollisions.speed = 20;
                playerCollisions.status = "hit";
            }
		}

		//collisions with boundary walls
		if (other.gameObject.CompareTag("boundary"))
		{
            if (transform == transform.root)
            {
                if (PhotonNetwork.offlineMode)
                {
                    playerCollisions.alive = false;
                }
                else
                { 
                    playerScore = transform.GetComponent<PlayerScore>();
                    photonView.RPC("Dead", PhotonTargets.All);
                    playerScore.score -= 1;
                    StartCoroutine(Respawn());
                }
            }
		}	
	}

    [PunRPC]
    void Dead(PhotonMessageInfo info)
    {
        playerCollisions.alive = false;
    }

    IEnumerator Respawn()
    {
        GrabandLaunch grabAndLaunch = transform.GetComponent<GrabandLaunch>();
        grabAndLaunch.holding = false;
        yield return new WaitForSeconds(1f);
        transform.position = new Vector3(27, 15, 16);
        photonView.RPC("Alive", PhotonTargets.All);
    }

    [PunRPC]
    void Alive(PhotonMessageInfo info)
    {
        playerCollisions.alive = true;
    }

    //collision with other players or boundaries
    void OnCollisionStay(Collision other) {
		
		if (other.gameObject.name == "Body" || other.gameObject.name == "Player(Clone)" || other.gameObject.name == "RFist1" || other.gameObject.name == "RFist2" || other.gameObject.name == "LFist1" || other.gameObject.name == "LFist2"){

            normalDirection = new Vector3(other.contacts[0].normal.x, 0, other.contacts[0].normal.z);
            transform.root.Translate(normalDirection * Time.deltaTime * 3, Space.World);
        }
		
		if (other.gameObject.name == "Boundary"){

            normalDirection = new Vector3(other.contacts[0].normal.x, 0, other.contacts[0].normal.z);
            transform.root.Translate(normalDirection * Time.deltaTime * 20, Space.World);
        }
	}
}
