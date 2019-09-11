using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Playerinfo : Photon.MonoBehaviour {

    //Saves the player's name and the colors they chose for their character

    private GameMenu gameMenu;
	public string playerName;

	public GameObject head;
	public GameObject body;
	public GameObject centerpiece;
	public GameObject bodydetail;
	public GameObject rightshoulder;
	public GameObject leftshoulder;
	public GameObject rightarm;
	public GameObject rightarmcuff;
	public GameObject leftarm;
	public GameObject leftarmcuff;
	public GameObject bottom;

	public Material head_Color;
	public Material body_Color;
	public Material centerpiece_Color;
	public Material bodydetail_Color;
	public Material rightshoulder_Color;
	public Material leftshoulder_Color;
	public Material rightarm_Color;
	public Material rightarmcuff_Color;
	public Material leftarm_Color;
	public Material leftarmcuff_Color;
	public Material bottom_Color;

	public List<Material> colors = new List<Material>();
	Renderer[] renderers;

	//Keeps only one of this script
	void Awake(){
		
		DontDestroyOnLoad(transform.gameObject);

		if (FindObjectsOfType (GetType ()).Length > 1) 
		{
			Destroy (gameObject);
		}
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode){

		if (bottom != null) 
		{
			renderers = bottom.GetComponentsInChildren<Renderer> ();

			for (int i = 0; i < renderers.Length; i++)
			{
				colors.Add(renderers[i].material);
			}
		}
	}

    void Update () {

		if (gameMenu != null) 
		{
			playerName = gameMenu.playerNameInput;
		} 
		else 
		{
			gameMenu = (GameMenu)FindObjectOfType(typeof(GameMenu));
		}
	}
}
