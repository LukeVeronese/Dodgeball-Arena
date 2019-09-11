using UnityEngine;

public class ColorSync : Photon.MonoBehaviour {

    //Each player tells the others what their colors are once upon joining
    //If a new player joins, all players tell the new player what their colors are

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

    private GameObject part;

	public Material red_Color;
	public Material orange_Color;
	public Material yellow_Color;
	public Material green_Color;
	public Material blue_Color;
	public Material purple_Color;
	public Material pink_Color;
	public Material gray_Color;
	public Material white_Color;
	public Material black_Color;

    private int cut;
    private string color;

	//When this client joins
	//it tells all other clients what its colors are for each part
	void Start () {

		GetColors (head);
		GetColors (body);
		GetColors (centerpiece);
		GetColors (bodydetail);
		GetColors (rightshoulder);
		GetColors (leftshoulder);
		GetColors (rightarm);
		GetColors (rightarmcuff);
		GetColors (leftarm);
		GetColors (leftarmcuff);
		GetColors (bottom);
    }

	//When another client joins
	//it tells all other clients what its colors are for each part
	void OnPhotonPlayerConnected () {
			
		GetColors (head);
		GetColors (body);
		GetColors (centerpiece);
		GetColors (bodydetail);
		GetColors (rightshoulder);
		GetColors (leftshoulder);
		GetColors (rightarm);
		GetColors (rightarmcuff);
		GetColors (leftarm);
		GetColors (leftarmcuff);
		GetColors (bottom);
    }

	//getting the material.name returns "Color(Instance)(Instance)"
	//the index of the first paranthesis is found and everything after is cut, leaving the color material
	void GetColors(GameObject part){

		cut = part.GetComponent<Renderer> ().material.name.IndexOf ("(");
		color = part.GetComponent<Renderer> ().material.name.Remove (cut);
		photonView.RPC ("SyncColors", PhotonTargets.All, part.name, color);
	}

    //RPC the colors of each part to other clients
    //Since only primitives can be RPCed, strings for the parts and colors are sent
    [PunRPC]
	void SyncColors(string name, string color, PhotonMessageInfo info){

		switch (name) 
		{
			case "Head":
				part = head;
				break;
			case "Body Outer":
				part = body;
				break;
			case "Center Piece":
				part = centerpiece;
				break;
			case "Body Detail":
				part = bodydetail;
				break;
			case "Right Shoulder Pad":
				part = rightshoulder;
				break;
			case "Left Shoulder Pad":
				part = leftshoulder;
				break;
			case "Right Arm 1":
				part = rightarm;
				break;
			case "Right Cuff":
				part = rightarmcuff;
				break;
			case "Left Arm 1":
				part = leftarm;
				break;
			case "Left Cuff":
				part = leftarmcuff;
				break;
			case "Player(Clone)":
				part = bottom;
				break;
		}

		Renderer[] renderers = part.GetComponentsInChildren<Renderer> ();
		int length;

		if (part == bottom) 
		{
			length = 1;
		} 
		else 
		{
			length = renderers.Length;
		}

		for (int n = 0; n < length; n++) {

			if (color == "Red ") {
				renderers [n].material = red_Color;
			}

			if (color == "Orange ") {
				renderers [n].material = orange_Color;
			}

			if (color == "Yellow ") {
				renderers [n].material = yellow_Color;
			}

			if (color == "Green ") {
				renderers [n].material = green_Color;
			}

			if (color == "Blue ") {
				renderers [n].material = blue_Color;
			}

			if (color == "Purple ") {
				renderers [n].material = purple_Color;
			}

			if (color == "Pink ") {
				renderers [n].material = pink_Color;
			}

			if (color == "Gray ") {
				renderers [n].material = gray_Color;
			}

			if (color == "White ") {
				renderers [n].material = white_Color;
			}

			if (color == "Black ") {
				renderers [n].material = black_Color;
			}
		}
	}
}
