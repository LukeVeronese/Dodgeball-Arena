using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SingleplayerManager: Photon.MonoBehaviour {

	public GameObject newplayer;

	public GameObject singlePlayer;

	public Transform newplayerTransform;
	public Transform bodyTransform;

	public string status;

	public Playerinfo playerInfo;
	public PlayerMovement playerMovement;
	public CursorMovement cursorMovement;
	public PlayerCollisions playerCollisions;
	public Renderer[] renderers;

    public GameObject playAgain_ButtonFrame;
	public GameObject continue_ButtonFrame;
	public GameObject menu_ButtonFrame;

    public GameObject time;
    public Text time_text;

    public TextMesh playerName;

	void Start() {
				
		PhotonNetwork.offlineMode = true;

		playAgain_ButtonFrame.transform.localPosition = new Vector3 (0, 50, 0);
        playAgain_ButtonFrame.SetActive(false);

        playAgain_ButtonFrame.GetComponent<Button>().onClick.AddListener (() => {

			SceneManager.LoadScene(4);
		});

		continue_ButtonFrame.transform.localPosition = new Vector3 (0, 50, 0);
        continue_ButtonFrame.SetActive(false);

        continue_ButtonFrame.GetComponent<Button>().onClick.AddListener (() => {

            continue_ButtonFrame.SetActive(false);
            menu_ButtonFrame.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		});

		menu_ButtonFrame.transform.localPosition = new Vector3 (0, -50, 0);
        menu_ButtonFrame.SetActive(false);

        menu_ButtonFrame.GetComponent<Button>().onClick.AddListener (() => {

			PhotonNetwork.Disconnect();
			SceneManager.LoadScene(0);
		});
	
		time.transform.localPosition = new Vector3(400, 275, 0);

		min = 0;
		sec = 0;

		playerInfo = (Playerinfo)FindObjectOfType(typeof(Playerinfo));

		newplayer = (GameObject) Instantiate (singlePlayer, new Vector3 (27, 12, 16), Quaternion.identity);

		newplayerTransform = newplayer.GetComponent<Transform>();
		bodyTransform = newplayerTransform.Find("Body");

		GetColors(newplayer.transform);
			
		playerName = newplayer.GetComponentInChildren<TextMesh>();
		playerName.text = playerInfo.playerName;

        playerMovement = newplayer.GetComponent<PlayerMovement>();
        cursorMovement = newplayer.GetComponent<CursorMovement>();
        playerCollisions = newplayer.GetComponent<PlayerCollisions>();

		status = "spawned";	
	}

	void GetColors(Transform player) {

		renderers = player.GetComponentsInChildren<Renderer>();

		if (playerInfo.colors.Count != 0) 
		{
			for (int i = 0; i < renderers.Length; i++) 
			{
				renderers [i].material = playerInfo.colors [i]; 
			}
		}
	}
				
	IEnumerator Respawn() {

        playerCollisions.alive = true;

        continue_ButtonFrame.SetActive(false);

        time_text.enabled = false;
					
		yield return new WaitForSeconds(.5f);
        playerMovement.enabled = false;
        cursorMovement.enabled = false;	

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

        playAgain_ButtonFrame.SetActive(true);
        menu_ButtonFrame.SetActive(true);
    }

    private float nextSpawn = 5.0f;
    private int Spawnrate = 5;
    private float min_Spawn = 2.0f;
    private float max_Spawn = 4.0f;

    public int min_Speed = 10;
    public int max_Speed = 30;
    private float nextChange = 5.0f;
    private int Changerate = 5;

    private float spawnRate1 = 2.0f;
    private float nextSpawn1 = 0.0f;
    private float spawnRate2 = 2.0f;
    private float nextSpawn2 = 0.0f;
    private float spawnRate3 = 2.0f;
    private float nextSpawn3 = 0.0f;
    private float spawnRate4 = 2.0f;
    private float nextSpawn4 = 0.0f;

    public string[] projectiles = { "Blue Projectile", "Red Projectile", "Green Projectile", "Purple Projectile" };

    void Update() {

		if (Input.GetButtonDown ("Cancel")) {

			if (newplayer != null) {
								
                continue_ButtonFrame.SetActive(true);
            }
						
            menu_ButtonFrame.SetActive(true);
        } 

		Timer();

		if (status == "spawned") {

			//makes sure all parts are facing the right direction
			if (bodyTransform.localEulerAngles != new Vector3 (bodyTransform.eulerAngles.x, 0, bodyTransform.eulerAngles.z)) {
				
				bodyTransform.localEulerAngles = new Vector3 (bodyTransform.eulerAngles.x, 0, bodyTransform.eulerAngles.z);
			}

			if (playerCollisions.alive == false) {
				
				StartCoroutine (Respawn ());
			}
		}
			
		if (Time.time > nextSpawn) {

			nextSpawn = Time.time + Spawnrate;

			min_Spawn *= .5f;
			max_Spawn *= .5f;
		}

		if (Time.time > nextChange) {

			nextChange = Time.time + Changerate;

			min_Speed += 5;
			max_Speed += 5;
		}
	
		if (PhotonNetwork.isMasterClient == true)
		{
			int projectileIndex;

			if (Time.time > nextSpawn1)
			{
				//Wall 1
				projectileIndex = Random.Range(0, 4);
                GameObject projectile1 = PhotonNetwork.InstantiateSceneObject(projectiles[projectileIndex], new Vector3(Random.Range(17.0f, 35.0f), 10, 44), Quaternion.identity, 0, null);
				projectile1.name = "1";				
				spawnRate1 = Random.Range(min_Spawn, max_Spawn);
				nextSpawn1 = Time.time + spawnRate1;

			}

			if (Time.time > nextSpawn2)
			{                                            
				//Wall 2
				projectileIndex = Random.Range(0, 4);
                GameObject projectile2 = PhotonNetwork.InstantiateSceneObject(projectiles[projectileIndex], new Vector3(54, 10, Random.Range(7.0f, 25.0f)), Quaternion.identity, 0, null);
				projectile2.name = "2";
				spawnRate2 = Random.Range(min_Spawn, max_Spawn);
				nextSpawn2 = Time.time + spawnRate2;	
			}

			if (Time.time > nextSpawn3)
			{
				//Wall 3
				projectileIndex = Random.Range(0, 4);
				GameObject projectile3 = PhotonNetwork.InstantiateSceneObject(projectiles[projectileIndex], new Vector3(Random.Range(17.0f, 35.0f), 10, -12), Quaternion.identity, 0, null);
				projectile3.name = "3";			
				spawnRate3 = Random.Range(min_Spawn, max_Spawn);
				nextSpawn3 = Time.time + spawnRate3;

			}

			if (Time.time > nextSpawn4)
			{                                            
				//Wall 4
				projectileIndex = Random.Range(0, 4);
                GameObject projectile4 = PhotonNetwork.InstantiateSceneObject(projectiles[projectileIndex], new Vector3(-2, 10, Random.Range(7.0f, 25.0f)), Quaternion.identity, 0, null);
				projectile4.name = "4";
				spawnRate4 = Random.Range(min_Spawn, max_Spawn);
				nextSpawn4 = Time.time + spawnRate4;	
			}
		}
	}

    private int min = 0;
    private int sec = 0;
    private float nextSec;
    private int sec_Rate = 1;
    private string timer = "on";

    void Timer()
	{
		if (Time.time > nextSec && min >= -1 && timer == "on") 
		{
			nextSec = Time.time + sec_Rate;

			if (sec == 59)
			{
				min += 1;
				sec = 0;
			}
			else
			{
				sec++;
			}
		}

		if (sec < 10 & timer == "on")
		{
			time_text.text = min.ToString() + ":0" + sec.ToString();
		}
		else if (timer == "on")
		{
			time_text.text = min.ToString() + ":" + sec.ToString();
		}
	}
}
