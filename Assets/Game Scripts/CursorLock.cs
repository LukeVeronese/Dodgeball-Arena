using UnityEngine;

public class CursorLock : MonoBehaviour {

    //Cursor is invisible and locked at the center of the screen
    //Character will follow mouse movement
    //Pressing esc key unlocks the mouse and makes it visible again

    public Transform body;

	void Start () {

        Vector3 Pos = Input.mousePosition;

        Vector3 point = GetComponentInChildren<Camera>().ScreenToWorldPoint(new Vector3(Pos.x, Pos.y, 20));

        body.LookAt(point);

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }
	
	void Update () {

		//pressing "esc" unlocks the mouse from the middle of the screen and makes it visible
		if (Input.GetButtonDown ("Cancel")) { //esc key
						
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
    }
}
