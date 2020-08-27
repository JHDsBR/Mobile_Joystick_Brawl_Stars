using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlStarsJoystick : MonoBehaviour
{
    [Tooltip("Se o controle pode ser usado em qualquer parte da tela")]
	public bool OnFullscreen;
    // [Space(10, order = 0)]
    // [Header("Header with some space around it", order = 1)]
    // [Space(40, order = 2)]

    // [Tooltip("Se o controle pode ser usado em qualquer parte da tela")]
	public float PlayerMovementSpeed = 2;
	public bool SpeedSensitive;

	public float BackgroundMovementSpeed = 7, DeadArea = 0.3f, AreaBoundary = 0;
	public Transform joystickBackground, joystickForground;
	public Transform player;
	
	private float AreaLimite;
	private int touchNum = 0;
	private bool touchStart, ajustPos, keepMoving, canTouch;
	private Vector2 pointA, pointB, cameraStartPos, offset;
	private Vector3 joystickBG_backup, pointA_screen, scaleBackup, positionBackup, cameraStartPosBackup;
	private string side;




    // Start is called before the first frame update
    void Start()
    {
    	if(!OnFullscreen)
    	{

	    	if((Input.deviceOrientation == DeviceOrientation.Portrait) || (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown))
	    	{
	    		// retrato
	    		side = "up";
	    		if(Camera.main.WorldToScreenPoint(joystickBackground.position).y > Screen.height/2)
	    		{
	    			side = "down";
	    		}

	    	}
	    	else if((Input.deviceOrientation == DeviceOrientation.LandscapeLeft) || (Input.deviceOrientation == DeviceOrientation.LandscapeRight))
	    	{
	    		// paisagem
	    		side = "left";
	    		if(Camera.main.WorldToScreenPoint(joystickBackground.position).x > Screen.width/2)
	    		{
	    			side = "right";
	    		}
	    	}

	        StartCoroutine("CheckIfJoystickPressed");

	    	side = "right";
	    	// side = "left";
    	}
    	// Debug.Log("orientation: "+orientation);
        // player = GetComponent<Transform>();
        AreaLimite = joystickBackground.localScale.x*2f + AreaBoundary;
        scaleBackup = player.transform.localScale;
        // positionBackup = player.transform.position;
        // Debug.Log("DEBUG HERE: " + Camera.main.WorldToScreenPoint(joystickBackground.position));
        cameraStartPosBackup = Camera.main.transform.position;
        // joystickBG_backup = Camera.main.ScreenToWorldPoint(joystickBackground.position);
        joystickBG_backup = Camera.main.WorldToScreenPoint(joystickBackground.position);
    }

    // Update is called once per frame
    void Update()
    {
    	if(Input.touchCount > 0)
    	{
    		RaycastHit2D infoHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Vector2)(Input.GetTouch(0).position)), Camera.main.transform.forward);

    		if(infoHit.collider != null)
    		{
    			print("Touched_> "+infoHit.transform.gameObject.name);
    		}
    	}
		 // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		 //         RaycastHit hit = new RaycastHit();
		 //         if(Input.GetMouseButton(0)) {
		 //                if(Physics.Raycast(ray, out hit)) {
		 //                   print(hit.collider.name);
		 //                 }
		 //              }
   //  	 Touch touch = Input.touches[0];
		 // Ray touchRay = Camera.main.ScreenPointToRay(touch.position);
		 // RaycastHit2D[] hits = Physics2D.RaycastAll(touch.position, touch.position);
		 // foreach( RaycastHit2D hit in hits ) {
		  
		 //     Debug.Log("touching object name="+hit.collider.name);
		  
		 // }
        // Debug.Log("||||||"+Camera.main.WorldToScreenPoint(joystickBackground.position).x);
        // Debug.Log("------"+Camera.main.ScreenToWorldPoint(joystickBackground.position));
        if(canTouch && !touchStart)
        {
	    	cameraStartPos = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        	pointA_screen = new Vector3(Input.GetTouch(touchNum).position.x, Input.GetTouch(touchNum).position.y, Camera.main.transform.position.z);
        	// pointA_screen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
        	pointA = Camera.main.ScreenToWorldPoint(pointA_screen);
	    	// Debug.Log(pointA);
    		joystickBackground.position = Camera.main.ScreenToWorldPoint(new Vector3(pointA_screen.x, pointA_screen.y, joystickBackground.position.z));
    		// aa.position = new Vector3(pointA.x, pointA.y, aa.position.z);
        }
        if(canTouch)
        {
        	touchStart = true;
        	// pointB = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        	pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(touchNum).position.x, Input.GetTouch(touchNum).position.y, Camera.main.transform.position.z));
    		// bb.position = new Vector3(pointB.x, pointB.y, bb.position.z);
        }
        else
        {
        	touchStart = false;
        	joystickBackground.position = Camera.main.ScreenToWorldPoint(joystickBG_backup);
        	joystickForground.localPosition = Vector3.zero;
        	// joystickBackground.position = new Vector3(Camera.main.ScreenToWorldPoint(joystickBG_backup).x, Camera.main.ScreenToWorldPoint(joystickBG_backup).y, joystickBackground.position.z);
        }
    	if(touchStart)
    	{
    		offset =  pointB - new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) - pointA + cameraStartPos;
			Vector2 dis = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) - cameraStartPos;
    			
    		Vector2 direction = Vector2.ClampMagnitude(offset, 1);
    		

    		if(offset.magnitude > DeadArea)
    		{
	    		moveCharacter(direction);
    		}
    		// Debug.Log("||||  "+(pointB-pointA));
    		// Debug.Log("Point A: " + pointA);
    		if(offset.magnitude > AreaLimite)
    		{
    			// ajustPos = true;
    			// float BackgroundMovementSpeed = 6f;
    			// pointA = Vector2.Lerp(pointA, pointB, BackgroundMovementSpeed * Time.deltaTime);

    			pointA = Vector2.MoveTowards(pointA, pointB-dis, BackgroundMovementSpeed * Time.deltaTime);
    		// 	// joystickBackground.position = new Vector3(pointA.x+Camera.main.transform.position.x, pointA.y+Camera.main.transform.position.y, joystickBackground.position.z);
    		// 	// joystickBackground.position = Vector3.MoveTowards(joystickBackground.position, new Vector3(pointB.x, pointB.y, joystickBackground.position.z), BackgroundMovementSpeed * Time.deltaTime);
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
			joystickBackground.position = new Vector3(pointA.x+dis.x, pointA.y+dis.y, joystickBackground.position.z);
			joystickForground.position = new Vector3(pointB.x, pointB.y, joystickForground.position.z);
    		// aa.position = new Vector3(pointA.x+Camera.main.transform.position.x, pointA.y+Camera.main.transform.position.y, aa.position.z);
    		// bb.position = new Vector3(pointB.x, pointB.y, bb.position.z);
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
    	if(SpeedSensitive)
    	{
	    	player.Translate(direction * PlayerMovementSpeed * Time.deltaTime);
    	}
    	else
    	{
	    	player.Translate(direction * PlayerMovementSpeed * Time.deltaTime);
	    	player.position = Vector3.MoveTowards(player.position, player.position+new Vector3(direction.x, direction.y, 0), PlayerMovementSpeed / 2 * Time.deltaTime);
    	}
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

    public string GetSideTouched(Vector2 pos)
    {
    	string side_tmp = "";

    	// if(Camera.main.WorldToScreenPoint(pos).x < Screen.width/2)
    	// Debug.Log(pos.x);
    	if(side == "left" || side == "right")
    	{
	    	if(pos.x < Screen.width/2)
	    	{
	    		side_tmp = "left";
	    	}
	    	else
	    	{
	    		side_tmp = "right";
	    	}
    	}
    	else
    	{
	    	if(pos.y < Screen.height/2)
	    	{
	    		side_tmp = "down";
	    	}
	    	else
	    	{
	    		side_tmp = "up";
	    	}

    	}





    	return side_tmp;
    }

    private IEnumerator CheckIfJoystickPressed()
    {

    	while(true)
    	{
    		bool aux = false;
    		for(int c=0; c<Input.touchCount; c++)
    		{
    			if(GetSideTouched(Input.GetTouch(c).position) == side)
    			{
    				touchNum = c;
    				canTouch = true;
    				aux = true;
    				break;
    			}


    			yield return null;
    		}

    		if(!aux)
    		{
    			canTouch = false;
    		}

    		yield return null;
    	}
    }

}
