using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Handles the projectile's collisions with other objects
//Keeps track of who threw this projectile (if thrown)
//Keeps a list of players it hit in order to give points to the thrower if
//a hit player falls off

public class ProjectileStuff : Photon.MonoBehaviour {

    private Vector3 normalDirection;
    private float speed;

    private List<PlayerCollisions> collisionsList = new List<PlayerCollisions>();

    public GameObject playerWhoThrew;

    private SingleplayerManager singlePlayerManager;

    void OnPhotonPlayerConnected() {

        if (PhotonNetwork.isMasterClient == true) {

            photonView.RPC("CurrentPositions", PhotonTargets.All, transform.position);
        }

        if (PhotonNetwork.isMasterClient == true) {

            photonView.RPC("Speeds", PhotonTargets.All, speed);
        }

        if (PhotonNetwork.isMasterClient == true) {

            normalDirection = new Vector3(transform.forward.x, 0, transform.forward.z);
            photonView.RPC("CurrentNormaldirection", PhotonTargets.All, normalDirection);
        }
    }

    [PunRPC]
    void CurrentPositions(Vector3 correctPlayerPos, PhotonMessageInfo info) {

        transform.position = correctPlayerPos;
    }

    [PunRPC]
    void CurrentNormaldirection(Vector3 correctNormalDirection, PhotonMessageInfo info) {

        normalDirection = correctNormalDirection;
    }

    void Start() {

        if (PhotonNetwork.offlineMode == true) {

            singlePlayerManager = (SingleplayerManager)FindObjectOfType(typeof(SingleplayerManager));
        }

        if (PhotonNetwork.isMasterClient == true) {

            photonView.RPC("ProjectileName", PhotonTargets.All, gameObject.name);
        }

        if (gameObject.name == "1") {

            speed = 3;

            if (PhotonNetwork.offlineMode == true) {

                speed = Random.Range(singlePlayerManager.min_Speed, singlePlayerManager.max_Speed);
            }

            normalDirection = Vector3.back;
        }

        if (gameObject.name == "2") {

            speed = 3;

            if (PhotonNetwork.offlineMode == true) {

                speed = Random.Range(singlePlayerManager.min_Speed, singlePlayerManager.max_Speed);
            }

            normalDirection = Vector3.left;
        }

        if (gameObject.name == "3") {

            speed = 3;

            if (PhotonNetwork.offlineMode == true) {

                speed = Random.Range(singlePlayerManager.min_Speed, singlePlayerManager.max_Speed);
            }

            normalDirection = Vector3.forward;
        }

        if (gameObject.name == "4") {

            speed = 3;

            if (PhotonNetwork.offlineMode == true) {

                speed = Random.Range(singlePlayerManager.min_Speed, singlePlayerManager.max_Speed);
            }

            normalDirection = Vector3.right;
        }

        //single player - allows green projectile to grow without needing to be thrown first
        if (PhotonNetwork.offlineMode == true) {

            if (transform.GetComponent<Renderer>().material.name == "Green (Instance)") {

                gameObject.name = "Ungrabbable";
            }
        }

        if (gameObject.CompareTag("launched")) {

            photonView.RPC("LaunchedTag", PhotonTargets.All);

            if (transform.GetComponent<Renderer>().material.name == "Green (Instance)") {

                gameObject.name = "Ungrabbable";
                photonView.RPC("ProjectileName", PhotonTargets.All, gameObject.name);
            }

            speed = 50;
            photonView.RPC("Speeds", PhotonTargets.All, speed);

            normalDirection = new Vector3(transform.forward.x, 0, transform.forward.z);
            photonView.RPC("CurrentNormaldirection", PhotonTargets.All, normalDirection);
        }
    }

    [PunRPC]
    void LaunchedTag(PhotonMessageInfo info) {

        gameObject.tag = "launched";
    }

    public PlayerScore score;
    private float stalltime;

    void Update() {

        if (Time.time >= stalltime + 3f && stalltime != 0)
        {
            photonView.RPC("ProjectileName", PhotonTargets.MasterClient, "destroyed");
        }

        if (gameObject.name == "destroyed" && PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        //collisionsList keeps list of players this projectile hit when thrown
        //collisionsList is an array in case the projectile hits more than one player
        if (collisionsList.ToArray().Length > 0 && playerWhoThrew != null) {

            for (int count = 0; count < collisionsList.ToArray().Length; count++) {

                if (collisionsList[count] != null && collisionsList.ToArray().Length > 0) {

                    Debug.Log(collisionsList.ToArray().Length);

                    if (collisionsList[count].alive == false) {

                        score.score += 3;
                        collisionsList.Remove(collisionsList[count]);
                    }
                }
            }
        }

        transform.Translate(normalDirection * Time.deltaTime * speed, Space.World);
    }

    IEnumerator PostExplosion() {

        yield return new WaitForSeconds(.5f);

        if (PhotonNetwork.offlineMode == true)
        {
            Destroy(gameObject);
        }

        if (collisionsList.ToArray().Length == 0 && PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            stalltime = Time.time;
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().detectCollisions = false;
        }
    }

    IEnumerator ColliderScaleRevert() {

        yield return new WaitForSeconds(.5f);
        transform.GetComponent<SphereCollider>().radius = 0.6f;
        transform.GetComponent<SphereCollider>().center = Vector3.zero;
    }

    private PlayerCollisions playerCollisions;
    public List<Transform> lastHit = new List<Transform>();

    //This is to make sure the sameplayer isn't added to the lastHit list more than once
    private bool samePlayer;

    void OnCollisionEnter(Collision other) {

        //collisions with other projectiles
        if (other.gameObject.CompareTag("launched") || other.gameObject.CompareTag("projectile")) {

            if (other.gameObject.layer == 8 && other.transform.localScale.x == 7f) //exploded red projectile
            { 
                speed += 10;

                if (PhotonNetwork.offlineMode == true) {

                    normalDirection = new Vector3(other.contacts[0].normal.x, 0, other.contacts[0].normal.z).normalized;
                }
                else
                {
                    normalDirection = new Vector3(other.contacts[0].normal.x, 0, other.contacts[0].normal.z).normalized;
                    photonView.RPC("CurrentNormaldirection", PhotonTargets.All, normalDirection);
                    photonView.RPC("Speeds", PhotonTargets.All, speed);
                }
            }
            else
            {      
                if (PhotonNetwork.offlineMode == true) {

                    normalDirection = new Vector3(other.contacts[0].normal.x, 0, other.contacts[0].normal.z).normalized;
                }
                else
                {
                    normalDirection = new Vector3(other.contacts[0].normal.x, 0, other.contacts[0].normal.z).normalized;
                    photonView.RPC("CurrentNormaldirection", PhotonTargets.All, normalDirection);
                }
            }
        }

        if (gameObject.name == "Ungrabbable" && other.gameObject.name != "Platform" && transform.localScale.x < 4f)
        {
            if (PhotonNetwork.offlineMode == true) {
                transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            }
            else
            {
                photonView.RPC("ScaleIncrease", PhotonTargets.All);
            }
        }

        //ball is grabbed if the player is grabbing and isn't holding a projectile
        if (other.gameObject.name == "grabbing" && other.gameObject.transform.root.tag != "holding")
        {
            if (gameObject.name != "Ungrabbable")
            {
                photonView.RPC("ProjectileName", PhotonTargets.All, gameObject.name);
                photonView.RPC("Invisible", PhotonTargets.All);
            }

            photonView.RPC("ProjectileName", PhotonTargets.All, gameObject.name);
        }

        //collisions with players
        if (other.gameObject.name == "Body" || other.gameObject.name == "Player(Clone)" || other.gameObject.name == "RFist1" || other.gameObject.name == "RFist2" || other.gameObject.name == "LFist1" || other.gameObject.name == "LFist2") {

            playerCollisions = other.transform.root.GetComponent<PlayerCollisions>();

            if (gameObject.layer == 8) //red projectiles
            {
                if (other.gameObject.name != "grabbing" || other.transform.root.tag == "holding")
                {
                    if (PhotonNetwork.offlineMode == true)
                    {
                        transform.localScale = new Vector3(7f, 7f, 7f);
                        normalDirection = Vector3.zero;
                        speed = 0;
                        StartCoroutine(PostExplosion());
                    }
                    else
                    {
                        photonView.RPC("Explosion", PhotonTargets.All);
                    }
                }
            }
            else
            {    
                if (PhotonNetwork.offlineMode == true) {

                    normalDirection = new Vector3(other.contacts[0].normal.x, 0, other.contacts[0].normal.z).normalized;
                }
                else
                {
                    if (other.gameObject.name != "grabbing" || other.transform.root.tag == "holding")
                    {
                        //This is to ensure that the player detects the collision, not just the projectile
                       
                        normalDirection = new Vector3(other.contacts[0].normal.x, 0, other.contacts[0].normal.z).normalized;

                        photonView.RPC("ColliderScaleIncrease", PhotonTargets.All, normalDirection);
                        photonView.RPC("CurrentNormaldirection", PhotonTargets.All, normalDirection);
                    }
                }
            }

            if (playerWhoThrew != null && other.transform.root.gameObject != playerWhoThrew) {

                if (lastHit.ToArray().Length > 0) {

                    for (int count = 0; count < lastHit.ToArray().Length; count++) {

                        if (other.transform.root.gameObject.GetPhotonView() == lastHit[count].gameObject.GetPhotonView()) {

                            samePlayer = true;
                        }
                    }
                }
                else
                {
                    samePlayer = false;
                }

                if (samePlayer == false)
                {
                    lastHit.Add(other.transform.root);           
                    collisionsList.Add(other.transform.root.GetComponentInChildren<PlayerCollisions>());
                }
            }
        }

        if (other.gameObject.CompareTag("boundary"))
        {
            if (collisionsList.ToArray().Length == 0)
            {
                photonView.RPC("ProjectileName", PhotonTargets.MasterClient, "destroyed");
            }
            else
            {
                photonView.RPC("Invisible", PhotonTargets.All);
                photonView.RPC("Speeds", PhotonTargets.All, speed);
                photonView.RPC("CurrentNormaldirection", PhotonTargets.All, normalDirection);
            }
        }
    }
			
	[PunRPC]
	void ProjectileName (string correctProjectileName, PhotonMessageInfo info) {
		
		gameObject.name = correctProjectileName;
	}
		
	[PunRPC]
	void Speeds (float correctSpeed, PhotonMessageInfo info) {
		
		speed = correctSpeed;
	}

	[PunRPC]
	void Explosion (PhotonMessageInfo info) {

		transform.localScale = new Vector3(7f, 7f, 7f);
		normalDirection = Vector3.zero;
		speed = 0;
        gameObject.name = "Ungrabbable";
        StartCoroutine(PostExplosion());
    }

    [PunRPC]
    void Invisible(PhotonMessageInfo info)
    {
        stalltime = Time.time;
        speed = 0;
        normalDirection = Vector3.zero;

        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().detectCollisions = false;
    }
		
	[PunRPC]
	void ScaleIncrease (PhotonMessageInfo info) {

		transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
		speed = 6;	
	}

    [PunRPC]
    void ColliderScaleIncrease(Vector3 normalDirection, PhotonMessageInfo info)
    {
        if (gameObject.name != "Ungrabbable")
        {
            transform.GetComponent<SphereCollider>().radius = 3.5f;
            transform.GetComponent<SphereCollider>().center = normalDirection;
            StartCoroutine(ColliderScaleRevert());
        }
    }
}	