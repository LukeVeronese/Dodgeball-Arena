using UnityEngine;

public class CursorMovement : MonoBehaviour {

    //Script for mouse movement and how the character follows the mouse when the user moves with it

    public Transform body;
    private string position;

    void Update () {

		float rotationX = Input.GetAxis ("Mouse X") * 1.5f;
        float rotationY = Input.GetAxis("Mouse Y") * 1.5f;

        transform.Rotate(0, rotationX, 0);

        //limits for how far the play can tilt forward or backward
        if (body.eulerAngles.x >= 0 && body.eulerAngles.x < 50)
        {
            position = "tiltedforward";
        }

        if (body.eulerAngles.x <= 359.99 && body.eulerAngles.x > 310)
        {
            position = "tiltedbackward";
        }

		//prevents player from rotating too far forward
        if (position == "tiltedforward" && body.eulerAngles.x > 50)
        {
            if (rotationY < 0)
            {
                rotationY = 0;
            }

            body.Rotate(-rotationY, 0, 0);
        }
        else //prevents player from tilting too far backward
        if (position == "tiltedbackward" && body.eulerAngles.x < 310)
        {
            if (rotationY > 0)
            {
                rotationY = 0;
            }

			body.Rotate (-rotationY, 0, 0);
        }
        else 
		{
            body.Rotate(-rotationY, 0, 0);
        }
    }
}
