using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlStarsJoystick : MonoBehaviour
{
	public float speed;
	public Transform joystickFG, joystickBG, aa, bb;
	
	private Transform player;
	private bool touchStart, ajustPos, keepMoving;
	private Vector2 pointA, pointB, cameraStartPos, offset;
	private Vector3 joystickBG_backup, pointA_screen, scaleBackup, positionBackup, cameraStartPosBackup;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Transform>();
        scaleBackup = player.transform.localScale;
        positionBackup = player.transform.position;
        Debug.Log("DEBUG HERE: " + Camera.main.WorldToScreenPoint(joystickBG.position));
        cameraStartPosBackup = Camera.main.transform.position;
        // joystickBG_backup = Camera.main.ScreenToWorldPoint(joystickBG.position);
        joystickBG_backup = Camera.main.WorldToScreenPoint(joystickBG.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
	    	cameraStartPos = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        	pointA_screen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
        	pointA = Camera.main.ScreenToWorldPoint(pointA_screen);
	    	Debug.Log(pointA);
    		joystickBG.position = Camera.main.ScreenToWorldPoint(new Vector3(pointA_screen.x, pointA_screen.y, joystickBG.position.z));
    		// aa.position = new Vector3(pointA.x, pointA.y, aa.position.z);
        }
        if(Input.GetMouseButton(0))
        {
        	touchStart = true;
        	// pointB = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        	pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
    		// bb.position = new Vector3(pointB.x, pointB.y, bb.position.z);
        }
        else
        {
        	touchStart = false;
        	float x = 100;
        	float y = 400;
        	float z = 0;
        	// joystickBG.position = Camera.main.ScreenToWorldPoint(joystickBG_backup);
        	joystickBG.position = Camera.main.ScreenToWorldPoint(joystickBG_backup);
        	joystickFG.localPosition = Vector3.zero;
        	// joystickBG.position = new Vector3(Camera.main.ScreenToWorldPoint(joystickBG_backup).x, Camera.main.ScreenToWorldPoint(joystickBG_backup).y, joystickBG.position.z);
        }
    	if(touchStart || keepMoving)
    	{
    		if(!keepMoving)
    		{
	    		offset =  pointB - new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) - pointA + cameraStartPos;
    		}
			Vector2 dis = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) - cameraStartPos;
    			
    		Vector2 direction = Vector2.ClampMagnitude(offset, 1);
    		
    		float areaDead = 0.3f;

    		if(offset.magnitude > areaDead)
    		{
	    		moveCharacter(direction);
    		}
    		// Debug.Log("||||  "+(pointB-pointA));
    		// Debug.Log("Point A: " + pointA);
    		if(offset.magnitude > 1.2f)
    		{
    			// ajustPos = true;
    			float speed2 = 6f;
    			// pointA = Vector2.Lerp(pointA, pointB, speed2 * Time.deltaTime);

    			pointA = Vector2.MoveTowards(pointA, pointB-dis, speed2 * Time.deltaTime);
    		// 	// joystickBG.position = new Vector3(pointA.x+Camera.main.transform.position.x, pointA.y+Camera.main.transform.position.y, joystickBG.position.z);
    		// 	// joystickBG.position = Vector3.MoveTowards(joystickBG.position, new Vector3(pointB.x, pointB.y, joystickBG.position.z), speed2 * Time.deltaTime);
    		// 	Debug.Log("MOVE! " + pointA);
	    	// 	offset =  pointB - new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) - pointA + cameraStartPos;
    		}

    		// if(ajustPos)
    		// {
    		// 	Debug.Log("DEBUG: "+(-(1.2f - (pointB-pointA).magnitude)));
    		// 	pointA = Vector2.Lerp(pointA, pointB, -(1.2f - (pointB-pointA).magnitude));
    		// 	ajustPos = false;

    		// }
    		// aa.position = new Vector3(pointA.x+dis.x, pointA.y+dis.y, aa.position.z);
			joystickBG.position = new Vector3(pointA.x+dis.x, pointA.y+dis.y, aa.position.z);
    		// aa.position = new Vector3(pointA.x+Camera.main.transform.position.x, pointA.y+Camera.main.transform.position.y, aa.position.z);
    		// bb.position = new Vector3(pointB.x, pointB.y, bb.position.z);
			joystickFG.position = new Vector3(pointB.x, pointB.y, bb.position.z);
    		// else
    		// {
	    	// 	joystickBG.position = Camera.main.ScreenToWorldPoint(new Vector3(pointA_screen.x, pointA_screen.y, joystickBG.position.z));
    		// }
    	}
		// else
		// {
  //   		joystickBG.position = Camera.main.ScreenToWorldPoint(new Vector3(pointA_screen.x, pointA_screen.y, joystickBG.position.z));
		// }
    }

    // private void FixedUpdate()
    // {
    // 	if(touchStart)
    // 	{
    // 		joystickBG.position = Camera.main.ScreenToWorldPoint(new Vector3(pointA_screen.x, pointA_screen.y, joystickBG.position.z));
    // 		Debug.Log(pointA + " | " + (pointB - new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y)));
    // 		Vector2 offset =  pointB - new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) - pointA + cameraStartPos;
    // 		Vector2 direction = Vector2.ClampMagnitude(offset, 1);
    // 		moveCharacter(direction);
    // 	}
    // }

    void moveCharacter(Vector2 direction)
    {
    	player.Translate(direction * speed * Time.deltaTime);
    }

    public void KeepMoveToFace()
    {
    	keepMoving = true;
    }

    public void Reset()
    {
    	keepMoving = false;
    	transform.localScale = scaleBackup;
    	transform.position = positionBackup;
    	Camera.main.transform.position = cameraStartPosBackup; 
    }

    public void SetZ(float z)
    {
    	player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, z);
    }

}
