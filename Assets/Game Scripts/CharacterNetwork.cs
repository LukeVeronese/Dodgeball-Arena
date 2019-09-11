using UnityEngine;

public class CharacterNetwork : Photon.MonoBehaviour{

    //This script updates network information for all clients
    //Constantly updates all of thier positions and rotations 
    //Names of players are constantly rotated to always face client
	
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private string correctPlayerText;

	private PlayerCollisions playerCollisions;
    private TextMesh textMesh;

    private Transform player;
    private Transform playerBody;

    void Start()
    {
        if (gameObject.name == "Player(Clone)")
        {
            playerCollisions = transform.GetComponent<PlayerCollisions>();
        }

        if (gameObject.name == "Name")
        {
            player = transform.root;
            playerBody = player.GetChild(1);
            textMesh = gameObject.GetComponent<TextMesh>();
        }
    }

    //client updates information that other clients write to it
    void Update() {
		
        if (!photonView.isMine)
        {
			if (gameObject.name == "Player(Clone)") {

				if (playerCollisions.status == "launched_hit") { 

					//if player is hit by a launched projectile, their starting position is moved to make up for ping delay
					transform.position = Vector3.Slerp (transform.TransformPoint(playerCollisions.normalDirection * 1f), this.correctPlayerPos, Time.deltaTime * 5);
				}
				else 
				{
					transform.position = Vector3.Slerp (transform.position, this.correctPlayerPos, Time.deltaTime * 5);
				}
			}

            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);

            //The names of players are constatly rotated to face the client
			if (gameObject.name == "Name")
			{	
				textMesh.transform.eulerAngles = new Vector3(playerBody.eulerAngles.x, player.eulerAngles.y, 0);
	        }						
		}
	}

    //Only the positions of the root object for each player needs to be updated
    //the rotations for other parts do need to be updated however (like when they grab or throw)

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
			
		if (stream.isWriting) //client writing
		{	
			if (gameObject.name == "Player(Clone)") {
								
				stream.SendNext (transform.position);
			}

			stream.SendNext(transform.rotation);
		}
		else //client reading
		{
			if (gameObject.name == "Player(Clone)") {								
				this.correctPlayerPos = (Vector3)stream.ReceiveNext ();
			}

			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();			
		}
	}
}




