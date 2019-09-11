using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GrabandLaunch : Photon.MonoBehaviour {

    //Player can grab and hold a projectile by holding the left mouse click
    //Releasing the left mouse click will release the projectile

    private GameObject newProjectile;
    private GameObject[] projectiles;

	public Transform rightshoulder;
	public Transform leftshoulder;
	public Transform Lfist1;
	public Transform Rfist1;
	public Transform Rfist2;
	public Transform Lfist2;
	public Transform player;

    public string mode;
    public bool holding = false;

    private GameObject projectile_Icon;
    private RawImage raw_Image;
    public string color;
    
    public Texture gray;
    public Texture blue;
    public Texture green;
    public Texture purple;
    public Texture red;

    void Start () {

        projectile_Icon = GameObject.Find("Projectile Icon");
        projectile_Icon.transform.localPosition = new Vector3(430, -275, 0);

        raw_Image = (RawImage)projectile_Icon.GetComponent<RawImage>(); 
    }
	
	void Update () {
				
		if (holding == true)    //player is holding a projectile
        {
			gameObject.tag = "holding";
		}
		else
		{	
			gameObject.tag = "Player";
		}
		
		if(Input.GetButton("Fire1")){ //left mouseclick
				
			mode = "grabbing";
				
			rightshoulder.localRotation = Quaternion.Slerp(rightshoulder.localRotation, Quaternion.Euler(22, -64, -6), Time.deltaTime * 120);
			leftshoulder.localRotation = Quaternion.Slerp(leftshoulder.localRotation, Quaternion.Euler(10, 48, -9), Time.deltaTime * 120);
		}
		
		if(Input.GetButtonUp("Fire1")){ //left mouseclick
				
			mode = "normal";
				
			if (holding == true)
			{
				StartCoroutine(Launched());
			}	
						
			rightshoulder.localRotation = Quaternion.Slerp(rightshoulder.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 320);
			leftshoulder.localRotation = Quaternion.Slerp(leftshoulder.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 320);	
		}


        //changes image to the color of the projectile that the player is holding
        if (holding == true)    //player is holding a projectile
        {
            if (color == "Blue ")
            {
                raw_Image.texture = blue;
            }
            
            if (color == "Green ")
            {
                raw_Image.texture = green;
            }

            if (color == "Purple ")
            {
                raw_Image.texture = purple;
            }

            if (color == "Red ")
            {
                raw_Image.texture = red;
            }
        }
        else
        {
            raw_Image.texture = gray;
        }
    }

    public string projectileColor;
    private ProjectileStuff projectileStuff;

    IEnumerator Launched(){
		
		Lfist1.name = "launching";		
		Rfist1.name = "launching";
		Rfist2.name = "launching";
		Lfist2.name = "launching";

        //projectileColor is defined from the fist grab scripts
        newProjectile = PhotonNetwork.Instantiate(projectileColor, transform.TransformPoint(0, 0.22f, 3), transform.rotation, 0, null);

        //projectile is instantiated from the player so that there is no delay from them releasing click and the projectile being thrown
        //ownnership is then transfered to the scene right after being thrown
        newProjectile.GetComponent<PhotonView>().TransferOwnership(0);

        projectileStuff = newProjectile.GetComponent<ProjectileStuff>();

        projectileStuff.playerWhoThrew = gameObject;
        projectileStuff.score = transform.GetComponent<PlayerScore>();
        newProjectile.tag = "launched";
        
        holding = false;
					
		yield return new WaitForSeconds(.2f);
		
		Lfist1.name = "LFist1";		
		Rfist1.name = "RFist1";
		Rfist2.name = "RFist2";
		Lfist2.name = "LFist2";
	}
}
