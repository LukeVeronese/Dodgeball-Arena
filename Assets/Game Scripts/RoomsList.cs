using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RoomsList : Photon.MonoBehaviour {

    //A new room is added to the list everytime a game is created
    //Rooms are ordered from least players to most players

    private string roomName = "";
    private RoomInfo[] roomsList;

    private GameObject canvas;

    public GameObject roomname_ButtonFrame;
    public GameObject createroom_ButtonFrame;
    public GameObject refresh_ButtonFrame;
    public GameObject menu_ButtonFrame;
    public GameObject right_ButtonFrame;
    public GameObject left_ButtonFrame;
    public GameObject joinRoomFail_Frame;

    public GameObject joinroom_Button;
    private List<GameObject> joinroom_ButtonFrame = new List<GameObject>();

    public InputField input;

    IEnumerator JoinRoomFailed()
    {
        joinRoomFail_Frame.SetActive(true);
        yield return new WaitForSeconds(2f);
        joinRoomFail_Frame.SetActive(false);
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
        Application.runInBackground = true;

        if (PlayerPrefs.HasKey("joinRoomFailed"))
        { 
            if (PlayerPrefs.GetString("joinRoomFailed") == "true")
            {
                StartCoroutine(JoinRoomFailed());
                PlayerPrefs.SetString("joinRoomFailed", "false");
            }
        }

        canvas = GameObject.Find("Canvas");

        roomname_ButtonFrame.transform.localPosition = new Vector3(-300, 270, 0);
        createroom_ButtonFrame.transform.localPosition = new Vector3(0, 190, 0);
		refresh_ButtonFrame.transform.localPosition = new Vector3(-200, -275, 0);
        menu_ButtonFrame.transform.localPosition = new Vector3(200, -275, 0);
        input.transform.localPosition = new Vector3(0, 270, 0);
        joinRoomFail_Frame.transform.localPosition = new Vector3(0, 0, 0);

		refresh_ButtonFrame.GetComponent<Button>().onClick.AddListener(() =>
		{
            Refresh();
		});

		menu_ButtonFrame.GetComponent<Button>().onClick.AddListener(() =>
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(0);
        });

        right_ButtonFrame = GameObject.Find("Right Arrow");
        right_ButtonFrame.transform.localPosition = new Vector3(400, -275, 0);

		right_ButtonFrame.GetComponent<Button>().onClick.AddListener(() =>
        {
       		center_Position += 1;

			for (int i = 0; i < joinroom_ButtonFrame.ToArray().Length; i++)
			{
				joinroom_ButtonFrame[i].transform.localPosition = new Vector3(joinroom_ButtonFrame[i].transform.localPosition.x - 1200, joinroom_ButtonFrame[i].transform.localPosition.y, 0);
			}

			if (center_Position >= limit - 1)
			{
				right_ButtonFrame.GetComponent<RawImage>().enabled = false;
				right_ButtonFrame.GetComponentInChildren<Text>().enabled = false;
			}

			if (left_ButtonFrame.GetComponent<RawImage>().enabled == false)
			{
				left_ButtonFrame.GetComponent<RawImage>().enabled = true;
				left_ButtonFrame.GetComponentInChildren<Text>().enabled = true;
			}
        });

        left_ButtonFrame = GameObject.Find("Left Arrow");
        left_ButtonFrame.transform.localPosition = new Vector3(-400, -275, 0);

		left_ButtonFrame.GetComponent<Button>().onClick.AddListener(() =>
        {
            center_Position -= 1;

			for (int i = 0; i < joinroom_ButtonFrame.ToArray().Length; i++)
			{
				joinroom_ButtonFrame[i].transform.localPosition = new Vector3(joinroom_ButtonFrame[i].transform.localPosition.x + 1200, joinroom_ButtonFrame[i].transform.localPosition.y, 0);
			}

            if (center_Position == 0)
            {
				left_ButtonFrame.GetComponent<RawImage>().enabled = false;
				left_ButtonFrame.GetComponentInChildren<Text>().enabled = false;
            }

			if (right_ButtonFrame.GetComponent<RawImage>().enabled == false)
			{
				right_ButtonFrame.GetComponent<RawImage>().enabled = true;
				right_ButtonFrame.GetComponentInChildren<Text>().enabled = true;
			}
        });

        input.onEndEdit.AddListener(SubmitName);
    }

    private void Refresh()
    {
        for (int i = 0; i < joinroom_ButtonFrame.ToArray().Length; i++)
        {
            Destroy(joinroom_ButtonFrame[i]);
            joinroom_ButtonFrame.Clear();
        }

        limit = 1;
        loop = "started";
        center_Position = 0;
        xDistance = 0;
        yDistance = 0;
    }

    private void SubmitName(string line)
    {
        roomName = line;
    }
			
	void OnConnectedToMaster()
	{
        createroom_ButtonFrame.GetComponent<Button>().onClick.AddListener(() =>
		{
			PhotonNetwork.CreateRoom(roomName);
			SceneManager.LoadScene(2);
		});
	}

    private List<int> players = new List<int>();
    private List<string> roomNames = new List<string>();
    private List<string> room = new List<string>();

    private string loop = "started";

    void Update()
    {
		if (roomsList != null)
        {
            //makes sure to run the loop only once so that only one listener is added to each button to prevent crashing
        	if (loop == "started")
            {
				for (int i = 0; i < roomsList.Length; i++)
                {
					joinroom_ButtonFrame.Add(joinroom_Button);
					joinroom_ButtonFrame[i] = Instantiate(joinroom_Button, new Vector3(0, 0, 0), Quaternion.identity);
	
					joinroom_ButtonFrame[i].name = "Join Room(Clone) " + i.ToString();						                     

                    joinroom_ButtonFrame[i].transform.SetParent(canvas.transform);

					players.Add(roomsList[i].PlayerCount);
                    roomNames.Add(roomsList[i].Name);

					joinroom_ButtonFrame[i].GetComponentInChildren<Text>().text = roomNames[i] + " (" + players[i] + " Player(s))";

                    room.Add(roomNames[i]);
                    room[i] = roomNames[i];
                }

                CreateRoom();

				if (joinroom_ButtonFrame.ToArray().Length != 0)
                {
                    ActivateButtons();
                }
				
				loop = "completed";
            }
        }
    }

    private string cloneName;
    private int cloneCount = 2;
    private bool samename = false;

    void CreateRoom()
    {
		createroom_ButtonFrame.GetComponent<Button>().onClick.AddListener(() =>
        {
			//default room name if nothing is typed
            if (roomName.Length == 0)
            {
                roomName = "Room";
            }

			//checks for other rooms that have the same name
			for (int i = 0; i < roomsList.Length; i++)
            {
                if (roomName == room[i])
                {
                    samename = true;
                    cloneName = roomName;
                }
            }

			//changes the name by adding a number or incrementing it
            while (samename == true)
            {
                roomName = cloneName + "(" + cloneCount.ToString() + ")";
                samename = false;

				for (int i = 0; i < roomsList.Length; i++)
                {
                    if (roomName == room[i])
                    {
                        cloneCount++;
                        samename = true;
                    }
                }
            }

            if (samename == false)
            {
                PhotonNetwork.CreateRoom(roomName);
                SceneManager.LoadScene(2);
            }
        });
    }

    private int lowestIndex;
    private int least_Players;
    private GameObject least_Playersroom;

    private int limit = 1;
    private int xDistance = 0;
    private int yDistance = 0;
    private int center_Position = 0;

    void ActivateButtons()
    {
		for (int i = 0; i < roomsList.Length; i++)
        {
			int currenti = i;

			joinroom_ButtonFrame[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                PhotonNetwork.JoinRoom(room[currenti]);
                SceneManager.LoadScene(2);
            });
        }
				
		//orders rooms from least players to most players
		for (int i = 0; i < roomsList.Length; i++)
        {
            lowestIndex = i;
            least_Players = players[i];
            least_Playersroom = joinroom_ButtonFrame[i];

			for (int n = i + 1; n < roomsList.Length; n++)
            {
                if (players[n] < least_Players)
                {
                    least_Players = players[n];
                    least_Playersroom = joinroom_ButtonFrame[n];
                    lowestIndex = n;
                }
            }

            players[lowestIndex] = players[i];
            players[i] = least_Players;
            joinroom_ButtonFrame[lowestIndex] = joinroom_ButtonFrame[i];
            joinroom_ButtonFrame[i] = least_Playersroom;
        }

		//creates positions for buttons
		//limit of 6 rooms per "page"
		//once 6 rooms is reached, new rooms are made offscreen to the right
		//and are accessed via a directionional button
		for (int i = 0; i < roomsList.Length; i++)
        {
            if (i == 6 * limit)
            {
                limit++;
                xDistance += 1200;
                yDistance = 0;

                if (center_Position < limit - 1)
                {
					right_ButtonFrame.GetComponent<RawImage>().enabled = true;
					right_ButtonFrame.GetComponentInChildren<Text>().enabled = true;
                }
            }

            joinroom_ButtonFrame[i].transform.localPosition = new Vector3(xDistance, 100 - (50 * yDistance), 0);
            yDistance++;
        }
    }

    void OnReceivedRoomListUpdate()
    {
        roomsList = PhotonNetwork.GetRoomList();
        Refresh();
    }
}
