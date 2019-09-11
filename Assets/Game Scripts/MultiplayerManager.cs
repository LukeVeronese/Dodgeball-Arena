using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//Keeps the score for each player and displays them on a scoreboard
//Activates the scripts for each player (scripts are originally disabled so that players aren't controlling eachother).
//Handles the spawning of the projectiles
//Handles the timer

public class MultiplayerManager : Photon.MonoBehaviour {

    public GameObject newplayer;

    private int playerNumber = 0;

    private Camera camera;
    private AudioListener AL;
    private Playerinfo playerInfo;
    private PlayerMovement playerMovement;
    private CursorMovement cursorMovement;
    private CursorLock cursorLock;
    private RFist1Grab rfist1grab;
    private RFist2Grab rfist2grab;
    private LFist1Grab lfist1grab;
    private LFist2Grab lfist2grab;
    private GrabandLaunch grabAndLaunch;
    private PlayerCollisions[] playerCollisions;
    private Renderer[] renderers;
    private ColorSync colorSync;
    private NameSync nameSync;

    public PlayerScore playerScore;
	
    private List<int> scores = new List<int>();
    private List<string> playernames = new List<string>();

    private struct Scoreinfo
    {
        public int[] orderedscores;
        public string[] orderedplayernames;
    }

    public GameObject playerScore_Frame;
    public TextMesh playerName;

    public GameObject time;
   
    public GameObject continue_ButtonFrame;
	public GameObject menu_ButtonFrame;

    private bool start_Timer = false;

    void Start() {
			
		playerScore_Frame.transform.localPosition = new Vector3(0, 282, 0);
		time.transform.localPosition = new Vector3(400, 275, 0);

		continue_ButtonFrame.transform.localPosition = new Vector3 (0, 50, 0);
        continue_ButtonFrame.SetActive(false);

		menu_ButtonFrame.transform.localPosition = new Vector3 (0, -50, 0);
        menu_ButtonFrame.SetActive(false);

        continue_ButtonFrame.GetComponent<Button>().onClick.AddListener (() => {

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

            continue_ButtonFrame.SetActive(false);
            menu_ButtonFrame.SetActive(false);
        });
						
        menu_ButtonFrame.GetComponent<Button>().onClick.AddListener (() => {

            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(0);
        });
	}

    void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        PlayerPrefs.SetString("joinRoomFailed", "true");
        SceneManager.LoadScene(1);
    }

    void OnDisconnectedFromPhoton() {
		
		SceneManager.LoadScene(0);
	}
		
	void OnPhotonPlayerDisconnected (PhotonPlayer otherPlayer){

		//all players that joined after the one that left get their playerNumber subtracted by 1

		Debug.Log ("Disconnected");

		if (PhotonNetwork.player.ID > otherPlayer.ID)
		{
			playerNumber -= 1;	
		}

		photonView.RPC("Disconnected", PhotonTargets.All);
	}

	[PunRPC]
	void Disconnected (PhotonMessageInfo info){

		scores.Clear();
		playernames.Clear();

		for (int n = 0; n < PhotonNetwork.playerList.Length; n++)
		{
			scores.Add(scores[n]);
			playernames.Add(playernames[n]);
		}
	}

	void OnLeftRoom() {

		PhotonNetwork.Destroy(newplayer);
	}

	void ApplyColors(Transform player) {

		renderers = player.GetComponentsInChildren<Renderer>();

		if (playerInfo.colors.Count != 0) 
		{
			for (int i = 0; i < renderers.Length; i++) 
			{
				renderers [i].material = playerInfo.colors [i]; 
			}
		}
	}
					
	void OnJoinedRoom() {

		//There needs to be atleast 2 players for the game to start
		if (PhotonNetwork.isMasterClient == false){
	
			photonView.RPC("TimerStart", PhotonTargets.All);
		}

		playerInfo = (Playerinfo)FindObjectOfType(typeof(Playerinfo));
			
		playerNumber = PhotonNetwork.playerList.Length;

		//Creates list of player names and scores for this player while updating it for the other players
       	photonView.RPC("Addinfo", PhotonTargets.All);

		//object must be named "Player"
		newplayer = PhotonNetwork.Instantiate("Player", new Vector3(27, 15, 16), Quaternion.identity, 0);

		ApplyColors(newplayer.transform);

        //Scripts for each player are originally disabled, so that they aren't controlling eachother
        //Enables scripts for individual player
        GetComponent<Camera>(camera).enabled = true;
		GetComponent<AudioListener> (AL).enabled = true;
		GetComponent<CursorMovement>(cursorMovement).enabled = true;
		GetComponent<CursorLock> (cursorLock).enabled = true;
        GetComponent<GrabandLaunch>(grabAndLaunch).enabled = true;
		GetComponent<RFist1Grab> (rfist1grab).enabled = true;
		GetComponent<RFist2Grab> (rfist2grab).enabled = true;
		GetComponent<LFist1Grab> (lfist1grab).enabled = true;
		GetComponent<LFist2Grab> (lfist2grab).enabled = true;
		GetComponent<PlayerMovement> (playerMovement).enabled = true;
		GetComponent<ColorSync> (colorSync).enabled = true;
        GetComponent<NameSync>(nameSync).enabled = true;

        playerScore = newplayer.GetComponent<PlayerScore>();
        playerScore.enabled = true;

        playerName = newplayer.GetComponentInChildren<TextMesh>();
		playerName.text = playerInfo.playerName;

        playerCollisions = newplayer.GetComponentsInChildren<PlayerCollisions>();
		for (int n = 0; n < playerCollisions.Length; n++){

            playerCollisions[n].enabled = true;
		}
	}
		
	T GetComponent<T>(T item)
	{
		item = newplayer.GetComponentInChildren<T>();
		return item;
	}

    [PunRPC]
	void TimerStart(PhotonMessageInfo info){

		start_Timer = true;
	}
				
	[PunRPC]
	void Addinfo (PhotonMessageInfo info){
		
		if (PhotonNetwork.playerList.Length != 1)
		{
			scores.Clear();
			playernames.Clear();
		}
		
		for (int n = 0; n < PhotonNetwork.playerList.Length; n++)
		{
			scores.Add (0);
			playernames.Add ("");
		}
	}

    int scoreboardx;
    int scoreboardy;
    int scoreboardwidth;
    int scoreboardheight;

    public Text ownScore_Text;

    private int n = 0;

    void OnGUI() {

		if (newplayer != null && playerScore != null)
		{
            ownScore_Text.text = "Score: " + (playerScore.score).ToString();
		}
		
		for (n = 0; n < playernames.Count; n++)
		{					
			//this statement designates the current player
			if (n == playerNumber - 1)
            {									
                scores[n] = playerScore.score;
                playernames [n] = playerInfo.playerName;

				photonView.RPC ("PlayerInformation", PhotonTargets.All, scores [n], playernames [n], n);
			} 
		}

		if (Input.GetKey (KeyCode.Tab)) {
			
			//network array passed through function so the ordered version doesn't
			//reference and tamper with the original

			Scoreinfo scoreinfo = OrderScores (scores.ToArray (), playernames.ToArray ());

			for (int n = 0; n < playernames.Count; n++)
			{						
				scoreboardx = Screen.width/2 - 100;
				scoreboardy = (n + 1) * 40 + 100;
				scoreboardwidth = 200;
				scoreboardheight = 40;

                //this statement designates the current player
                if (scoreinfo.orderedplayernames[n] == playerInfo.playerName) 
                {
					GUI.color = Color.yellow;
				}
				else
				{
					GUI.color = Color.white;
				}

				GUI.Box(new Rect(scoreboardx, scoreboardy, scoreboardwidth, scoreboardheight), (n + 1) + ". " + scoreinfo.orderedplayernames[n] + ": " + scoreinfo.orderedscores[n]);
			}
		}
	}
	
	[PunRPC]
	void PlayerInformation (int correctscores, string correctplayernames, int currentn, PhotonMessageInfo info){
		
		n = currentn;
		scores[n] = correctscores;
		playernames[n] = correctplayernames;
	}

	//updates and orders the scoreboard from highest score to lowest
	
	Scoreinfo OrderScores (int[] scores, string[] playernames)
	{
		int highestindex;
		int highestscore;
		string highestname;

		Scoreinfo scoreinfo;

		for (int n = 0; n < scores.Length; n++)
		{
			highestindex = n;
			highestscore = scores[n];
			highestname = playernames[n];
			
			for (int x = n + 1; x < scores.Length; x++)
			{
				if (scores[x] > highestscore)
				{
					highestscore = scores[x];
					highestname = playernames[x];
					highestindex = x;
				}
			}
			
			scores[highestindex] = scores[n];
			scores[n] = highestscore;			
			playernames[highestindex] = playernames[n];
			playernames[n] = highestname;
		}	

		scoreinfo.orderedplayernames = playernames;
		scoreinfo.orderedscores = scores;
		return scoreinfo;
	}

    private float spawnRate1 = 2.0f;
    private float nextSpawn1 = 0.0f;
    private float spawnRate3 = 2.0f;
    private float nextSpawn3 = 0.0f;

    public string[] projectiles = { "Blue Projectile", "Red Projectile", "Green Projectile", "Purple Projectile" };

    void Update() {

		//when esc is pressed, buttons appear
		if (Input.GetButtonDown ("Cancel")) {

			if (newplayer != null) {

                continue_ButtonFrame.SetActive(true);
                menu_ButtonFrame.SetActive(true);
            }
		} 

		//starts game timer when atleast one other player joins room
		if (PhotonNetwork.isMasterClient && start_Timer == true){
			
			photonView.RPC("Timer", PhotonTargets.All, min, sec);
		}
		else if (PhotonNetwork.isMasterClient && start_Timer == false) //if there is only one player, their score is reset
		{
			playerScore.score = 0;
		}
		 
		//this is where projectiles are spawned
		if (PhotonNetwork.isMasterClient)
		{
			int projectileIndex;

			if (Time.time > nextSpawn1)
			{
				//Wall 1
				projectileIndex = Random.Range(0, 4);
				GameObject projectile1 = PhotonNetwork.InstantiateSceneObject(projectiles[projectileIndex], new Vector3(Random.Range(17.0f, 35.0f), 10, 44), Quaternion.identity, 0, null);
				projectile1.name = "1";				
				spawnRate1 = Random.Range(2.0f, 4.0f);
				nextSpawn1 = Time.time + spawnRate1;
					
			}
					
			if (Time.time > nextSpawn3)
			{
				//Wall 3
				projectileIndex = Random.Range(0, 4);
				GameObject projectile3 = PhotonNetwork.InstantiateSceneObject(projectiles[projectileIndex], new Vector3(Random.Range(17.0f, 35.0f), 10, -12), Quaternion.identity, 0, null);
				projectile3.name = "3";			
				spawnRate3 = Random.Range(2.0f, 4.0f);
				nextSpawn3 = Time.time + spawnRate3;
					
			}
        }
    }

    private int min = 2;
    private int sec = 0;
    private float nextSec;
    private int sec_Rate = 1;
    private string timer = "on";

    public Text time_Text;

    [PunRPC]
    IEnumerator Timer (int currentMin, int currentSec, PhotonMessageInfo info) {

        min = currentMin;
        sec = currentSec;

        if (Time.time > nextSec && min >= -1 && timer == "on") 
		{
			nextSec = Time.time + sec_Rate;
			
			if (sec == 0)
			{
				min -= 1;
				sec = 59;
			}
			else
			{
				sec--;
			}
		}

		if (min <= -1)
		{
			if (PhotonNetwork.playerList.Length < 2)
			{
				start_Timer = false;
				time_Text.text = "Waiting for another player";
			}

			timer = "off";
			min = 2;
			sec = 0;

			GetHighscore();
							
			yield return new WaitForSeconds(5f);

			if (PhotonNetwork.playerList.Length < 2)
			{
				start_Timer = false;
				time_Text.text = "Waiting for another player";
			}

            winner_Frame.SetActive(false);

			timer = "on";

            playerScore.score = 0;
		}
		else if (sec < 10 & timer == "on")
		{
			time_Text.text = min.ToString() + ":0" + sec.ToString();
		}
		else if (timer == "on")
		{
			time_Text.text = min.ToString() + ":" + sec.ToString();
		}
	}

    public GameObject winner_Frame;
    public Text winnerName_Text;
    public Text winnerScore_Text;

    //Display highestscore at the end of the round
    void GetHighscore()
	{
		Scoreinfo scoreinfo = OrderScores(scores.ToArray(), playernames.ToArray());

		winnerName_Text.text = scoreinfo.orderedplayernames[0].ToString();
		winnerScore_Text.text = "with " + scoreinfo.orderedscores[0].ToString() + " points";

        winner_Frame.SetActive(true);

		time_Text.text = "Starting next round";
	}
}