using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Customization : MonoBehaviour {

    //Here the person chooses the colors they want for their character
    //The information is saved in the Playerinfo script

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

	public GameObject menu_ButtonFrame;
	public GameObject all_ButtonFrame;
	public GameObject head_ButtonFrame;
	public GameObject body_ButtonFrame;
	public GameObject centerpiece_ButtonFrame;
	public GameObject bodydetail_ButtonFrame;
	public GameObject rightshoulder_ButtonFrame;
	public GameObject leftshoulder_ButtonFrame;
	public GameObject rightarm_ButtonFrame;
	public GameObject leftarm_ButtonFrame;
	public GameObject rightarmcuff_ButtonFrame;
	public GameObject leftarmcuff_ButtonFrame;
	public GameObject bottom_ButtonFrame;

	public GameObject red_ButtonFrame;
	public Material red_Color;

	public GameObject orange_ButtonFrame;
	public Material orange_Color;

	public GameObject yellow_ButtonFrame;
	public Material yellow_Color;

	public GameObject green_ButtonFrame;
	public Material green_Color;

	public GameObject blue_ButtonFrame;
	public Material blue_Color;

	public GameObject purple_ButtonFrame;
	public Material purple_Color;

	public GameObject pink_ButtonFrame;
	public Material pink_Color;

	public GameObject gray_ButtonFrame;
	public Material gray_Color;

	public GameObject white_ButtonFrame;
	public Material white_Color;

	public GameObject black_ButtonFrame;
	public Material black_Color;

	public Renderer[] renderers;

    private Playerinfo playerInfo;

    void Awake()
	{
		playerInfo = (Playerinfo)FindObjectOfType(typeof(Playerinfo));

		playerInfo.bottom = GameObject.Find("Player (Customization)");
	}
		
	void Start () {
				
		//This takes saved colors from the playerinfo script
        //The player may have customized before, left, and then came back

        //goes from the root
		GetColors (bottom.transform);

		menu_ButtonFrame.transform.localPosition = new Vector3(0, -275, 0);
		
		menu_ButtonFrame.GetComponent<Button>().onClick.AddListener(() => {
			
            renderers = bottom.GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                playerInfo.colors[i] = renderers[i].material;
            }

            SceneManager.LoadScene(0);
		});

		//initiate buttons for body parts
		PartButtons ();

		//initiate buttons for colors
		ColorButtons ();
	}

	void GetColors(Transform player) {

		renderers = player.GetComponentsInChildren<Renderer>();

		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].material = playerInfo.colors[i]; 
		}
	}

	void PartButtons()
	{
		GetButton (bottom, all_ButtonFrame);
		GetButton (head, head_ButtonFrame);
		GetButton (body, body_ButtonFrame);
		GetButton (centerpiece, centerpiece_ButtonFrame);
		GetButton (bodydetail, bodydetail_ButtonFrame);
		GetButton (rightshoulder, rightshoulder_ButtonFrame);
		GetButton (leftshoulder, leftshoulder_ButtonFrame);
		GetButton (rightarm, rightarm_ButtonFrame);
		GetButton (leftarm, leftarm_ButtonFrame);
		GetButton (rightarmcuff, rightarmcuff_ButtonFrame);
		GetButton (leftarmcuff, leftarmcuff_ButtonFrame);
		GetButton (bottom, bottom_ButtonFrame);
	}

    private int yCoordinatePart = 300;

    void GetButton(GameObject part, GameObject buttonFrame)
	{
		if (buttonFrame == all_ButtonFrame) 
		{
			buttonFrame.transform.localPosition = new Vector3 (0, 275, 0);

			buttonFrame.GetComponent<Button>().onClick.AddListener(() => {           

				renderers = part.GetComponentsInChildren<Renderer>();
			});
		}
		else 
		{
			buttonFrame.transform.localPosition = new Vector3 (-370, yCoordinatePart, 0);

			if (buttonFrame == bottom_ButtonFrame) 
			{
				buttonFrame.GetComponent<Button> ().onClick.AddListener (() => {           

					renderers = new Renderer[1];
					renderers[0] = part.GetComponent<Renderer> ();
				});
			} 
			else 
			{
				buttonFrame.GetComponent<Button> ().onClick.AddListener (() => {           

					renderers = part.GetComponentsInChildren<Renderer> ();
				});
			}
				
			yCoordinatePart -= 60;
		}
	}

	void ColorButtons()
	{
		GetMaterial (red_Color, red_ButtonFrame);
		GetMaterial (orange_Color, orange_ButtonFrame);
		GetMaterial (yellow_Color, yellow_ButtonFrame);
		GetMaterial (green_Color, green_ButtonFrame);
		GetMaterial (blue_Color, blue_ButtonFrame);
		GetMaterial (purple_Color, purple_ButtonFrame);
		GetMaterial (pink_Color, pink_ButtonFrame);
		GetMaterial (gray_Color, gray_ButtonFrame);
		GetMaterial (white_Color, white_ButtonFrame);
		GetMaterial (black_Color, black_ButtonFrame);
	}

    private int count = 0;
    private int xCoordinateMaterial = 300;
    private int yCoordinateMaterial = 280;

    void GetMaterial(Material color, GameObject buttonframe)
	{
		buttonframe.transform.localPosition = new Vector3(xCoordinateMaterial, yCoordinateMaterial, 0);

		buttonframe.GetComponent<Button>().onClick.AddListener(() => {           

			if (renderers != null)
			{
				for (int n = 0; n < renderers.Length; n++){

                    if (renderers[n].name != "Name")
                    {
                        renderers[n].material = color;
                    }
				}
			}
		});

		if (count % 2 == 0) 
		{
			xCoordinateMaterial += 120;
		} 
		else 
		{
			xCoordinateMaterial -= 120;
			yCoordinateMaterial -= 120;
		}

		count++;
	}
	
    //allows user to rotate the character
	void Update () {
		
		float a = Input.GetAxis("Horizontal") * Time.deltaTime * 300;
		transform.Rotate(0, -a, 0);

		if(Input.GetButton("Fire1")){

			var rotationX = Input.GetAxis("Mouse X") * 5;
			transform.Rotate(0, -rotationX, 0);
		}
	}
}