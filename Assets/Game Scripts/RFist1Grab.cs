using UnityEngine;

//Seperate scripts for each fist part for collision events

public class RFist1Grab : Photon.MonoBehaviour {

	public GrabandLaunch grabAndLaunch;

	public Transform Rfist2;
	public Transform Lfist1;
	public Transform Lfist2;
	public Transform player;

    private string color;
    private int cut;
	
	void Start () {

        grabAndLaunch = player.GetComponent<GrabandLaunch>();
	}
	
	void Update () {		
		
		if(Input.GetButton("Fire1")){
					
			if (grabAndLaunch.holding == false && gameObject.name != "launching" && Rfist2.name != "launching" && Lfist1.name != "launching" && Lfist2.name != "launching")		
			{
				gameObject.name = "grabbing";  
			}
		}
		
		if(Input.GetButtonUp("Fire1")){
												
			if (grabAndLaunch.holding == false)
			{
				gameObject.name = "RFist1";   
			}
		}		
	}

    //name of color is extracted from the material so that the GrabandLaunch script knows which color to instantiate
    void OnCollisionEnter(Collision other){

		if (gameObject.name == "grabbing")
		{
			if (other.gameObject.CompareTag("projectile") || other.gameObject.name == "destroyed" && other.gameObject.name != "Ungrabbable")
			{
				if (grabAndLaunch.holding == false)
				{	
					cut = other.gameObject.GetComponent<Renderer> ().material.name.IndexOf ("(");
					color = other.gameObject.GetComponent<Renderer> ().material.name.Remove (cut);

                    grabAndLaunch.color = color;
					
					grabAndLaunch.projectileColor = string.Concat(color, "Projectile");

                    grabAndLaunch.holding = true;					
				}
			}	
		}
	}
}
