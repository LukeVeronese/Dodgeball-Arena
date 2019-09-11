using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : Photon.MonoBehaviour {

    //Initiates the buttons for multiplayer, singleplayer, and customization
    //This is where user enters their name

    public string playerNameInput;

	public GameObject singleplayer_ButtonFrame;
	public GameObject multiplayer_ButtonFrame;
	public GameObject customization_ButtonFrame;

	public InputField input;

	void Start() {

		Playerinfo playerInfo = (Playerinfo)FindObjectOfType(typeof(Playerinfo));

		if (playerInfo.playerName != null) 
		{
			playerNameInput = playerInfo.playerName;

			input.text = playerInfo.playerName;
		}
			
		singleplayer_ButtonFrame.transform.localPosition = new Vector3(360, -200, 0);

		singleplayer_ButtonFrame.GetComponent<Button>().onClick.AddListener(() => {
						 
			PhotonNetwork.offlineMode = true;
			PhotonNetwork.CreateRoom("room");
			SceneManager.LoadScene(4);
		});

        multiplayer_ButtonFrame.transform.localPosition = new Vector3(-360, -200, 0);

        multiplayer_ButtonFrame.GetComponent<Button>().onClick.AddListener(() => {

            SceneManager.LoadScene(1); 
		});
			
		customization_ButtonFrame.transform.localPosition = new Vector3(0, -200, 0);

		customization_ButtonFrame.GetComponent<Button>().onClick.AddListener(() => {

            SceneManager.LoadScene(3); 
		});

		input.transform.localPosition = new Vector3(0, 75, 0);

		input.onEndEdit.AddListener (SubmitName);
	}

	private void SubmitName(string line)
	{
		playerNameInput = line;
	}
}