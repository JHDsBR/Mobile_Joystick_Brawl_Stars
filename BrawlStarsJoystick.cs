using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlStarsJoystick : MonoBehaviour
{
    [Tooltip("Se o controle pode ser usado em qualquer parte da tela")]
	public bool OnFullscreen;

    [Tooltip("GameObject que vai ser controlado")]
	public Transform objectToMove;

    [Tooltip("Rotacionar 'objectToMove' na direção que está se movendo")]
    public bool RotateToFace;
    // [Space(10, order = 0)]
    // [Header("Header with some space around it", order = 1)]
    // [Space(40, order = 2)]

    // [Tooltip("Se o controle pode ser usado em qualquer parte da tela")]
	public float PlayerMovementSpeed = 2;
	public bool SpeedSensitive;

	public float DeadArea = 0.3f, AreaBoundary = 0;
	public Transform joystickBackground, joystickForeground;
    [Tooltip("GameObjects que podem causar interferência, por exemplo botões")]
    public List<Collider2D> IgnoreByCollider;
    [Tooltip("GameObjects que podem causar interferência, por exemplo botões")]
	public List<string> IgnoreByName;


    private List<string> ignore = new List<string>();
	private float AreaLimite;
	private int touchNum=-1, touchs;
	private bool touchStart, ajustPos, keepMoving, canTouch, justForTest;
	private Vector2 pointA, pointB, cameraStartPos, offset, direction, dis;
	private Vector3 joystickBG_backup, pointA_screen, scaleBackup, positionBackup, cameraStartPosBackup;
	private string side = "fullScreen";
    private Touch touch_tmp;



    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine("MergeList");
        MergeList();
    	
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

	        // StartCoroutine("CheckIfJoystickPressed");

	    	// side = "right";
	    	side = "left";
    	}
    	// Debug.Log("orientation: "+orientation);
        // objectToMove = GetComponent<Transform>();
        touchs = Input.touchCount;
        AreaLimite = (joystickBackground.localScale.x*transform.localScale.x*1.865f) + AreaBoundary;
        scaleBackup = objectToMove.transform.localScale;
        // positionBackup = objectToMove.transform.position;
        // Debug.Log("DEBUG HERE: " + Camera.main.WorldToScreenPoint(joystickBackground.position));
        cameraStartPosBackup = Camera.main.transform.position;
        // joystickBG_backup = Camera.main.ScreenToWorldPoint(joystickBackground.position);
        joystickBG_backup = Camera.main.WorldToScreenPoint(joystickBackground.position);
    }

    // Update is called once per frame
    void Update()
    {

        if(touchNum == -1)
        {
            ResetJoystickPosition();
        }

        if(!touchStart)
        {

            if(Input.touchCount > 0)
            {
                touchNum = GetTouchNum();
                if(touchNum != -1)
                {
                    // touchs = Input.touchCount;
                    touchStart = true;
        	    	cameraStartPos = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
                    pointA_screen = new Vector3(Input.GetTouch(touchNum).position.x, Input.GetTouch(touchNum).position.y, Camera.main.transform.position.z);
                	pointA = Camera.main.ScreenToWorldPoint(pointA_screen);
                }

                // print("TouchNum_> "+touchNum);

            }

        }
        else
        {

            touchNum = UpdateTouchNum("++");

            if((Input.GetTouch(touchNum)).phase == TouchPhase.Ended || touchNum <= -1)
            {
                touchStart = false;
                ResetJoystickPosition();
            }
            else
            {
            	pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(touchNum).position.x, Input.GetTouch(touchNum).position.y, Camera.main.transform.position.z));
                dis = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) - cameraStartPos;
                offset =  pointB - pointA - dis;
                // offset =  pointB - new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) - pointA + cameraStartPos;
                float cos = GetCos(pointA, pointB-dis);
                float sen = GetSen(pointA, pointB-dis);
                Vector2 quadrant = GetQuadrant(pointA, pointB-dis);
                    
                direction = offset;

                if(offset.magnitude > DeadArea)
                {
                    if(RotateToFace){RotateToFaceObjectToMove();}
                    moveCharacter(direction);
                }

                if(offset.magnitude > AreaLimite)
                {
                    float aux = 1.865f;

                    Vector2 intersectionPoint = new Vector2((quadrant.x*cos*AreaLimite+pointA.x), (quadrant.y*sen*AreaLimite+pointA.y));
                    Vector2 moveTo = pointA+pointB-dis-intersectionPoint;
                    pointA = moveTo;

                }

                joystickBackground.position = new Vector3(pointA.x+dis.x, pointA.y+dis.y, joystickBackground.position.z);
                joystickForeground.position = new Vector3(pointB.x, pointB.y, joystickForeground.position.z);

            }
            touchNum = UpdateTouchNum("--");
        }

    }


    private void ResetJoystickPosition()
    {
        joystickBackground.position = Camera.main.ScreenToWorldPoint(joystickBG_backup);
        joystickForeground.localPosition = new Vector3(0,0, joystickForeground.localPosition.z);
        // print(">_ResetJoystickPosition_<");
    }


    private bool IsCollidingWithOtherObject(Touch touch)
    {
        Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(touch.position);
        Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

        //We now raycast with this information. If we have hit something we can process it.
        RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

        if (hitInformation.collider != null) {

            foreach(string name in ignore) {

                if(name == hitInformation.transform.gameObject.name)
                {
                    // print(">_colliding_<");
                    return true;

                }
            }
        }
     
        return false;
    }


    private int GetTouchNum()
    {
        for(int c=0; c<Input.touchCount; c++)
        {
            Touch t = Input.GetTouch(c);

            if(GetSideTouched(t.position) == side && t.phase == TouchPhase.Began && !IsCollidingWithOtherObject(t))
            {
                return c;
            }
        }

        return -1;
    }


    // Atualiza o valor do toque caso seja removido ou adicionado outro toque
    private int UpdateTouchNum(string val="++")
    {
        int tmp = touchNum, count = Input.touchCount;
        for(int c=0; c<Input.touchCount; c++)
        { 
            Touch t = Input.GetTouch(c);

            if(c <= touchNum)
            {
                if(t.phase == TouchPhase.Began && val == "++")
                {
                    tmp++;
                    print("++");
                }
                else if(t.phase == TouchPhase.Ended && val == "--")
                {
                    tmp--;
                    print("--");
                }
            }
        }

        return tmp;
    }


    private bool GetIfCanTouch()
    {
        for(int c=0; c<Input.touchCount; c++)
        {
            if(GetSideTouched(Input.GetTouch(c).position) == side)
            {
                touchNum = c;
                return true;
            }

        }


        return false;
    }


    void moveCharacter(Vector2 direction)
    {
    	if(SpeedSensitive)
    	{
	    	objectToMove.Translate((direction) * PlayerMovementSpeed * Time.deltaTime,  Space.World);
    	}
    	else
    	{
	    	objectToMove.position = Vector3.MoveTowards(objectToMove.position, objectToMove.position+new Vector3(direction.x, direction.y, 0), PlayerMovementSpeed * Time.deltaTime);
    	}
    }

    // private IEnumerator MergeList()
    private void MergeList()
    {
        ignore = IgnoreByName;
        foreach(Collider2D col in IgnoreByCollider)
        {
            ignore.Add(col.name);
            // yield return null;
        }
    }

    public void KeepMoveToFace()
    {
    	keepMoving = true;
    }

    // public void Reset()
    // {
    // 	keepMoving = false;
    // 	transform.localScale = scaleBackup;
    // 	transform.position = positionBackup;
    // 	Camera.main.transform.position = cameraStartPosBackup; 
    // }

    public void SetZ(float z)
    {
    	objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y, z);
    }

    public string GetSideTouched(Vector2 pos)
    {
    	string side_tmp = "fullScreen";

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
    	else if(side == "down" || side == "up")
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
                // try:
                print("C_> "+c);
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

    public void RotateToFaceObjectToMove()
    {
        Quaternion neededRotation = Quaternion.LookRotation(Vector3.forward, new Vector3(direction.x*20, direction.y*20, 0));
        objectToMove.rotation = Quaternion.Slerp(objectToMove.rotation, neededRotation, 100 * Time.deltaTime);
    }

    // Retorna a primeira posicao fora do raio de um ponto A a B 
    private Vector2 GetPosition(Vector2 point_a, Vector2 point_b, float ray)
    {
        Vector2 distance = point_b - point_a;
        float   maxValue = Mathf.Max(distance.x,distance.y);
        distance /= maxValue;

        return point_a + ray * distance ;

    }

    public Vector2 GetQuadrant(Vector2 point_a, Vector2 point_b)
    {
        float x = point_b.x-point_a.x, y = point_b.y-point_a.y, x2=0, y2=0;
        if(x != 0)
        {
            x2 = Mathf.Abs(x)/x;
        }
        if(y != 0)
        {
            y2 = Mathf.Abs(y)/y;
        }

        return new Vector2(x2, y2);
    }

    public float GetCatetoOposto(Vector2 point_a, Vector2 point_b)
    {
        return GetMagnitude(new Vector2(point_b.x, point_a.y), point_b);
    }

    public float GetCatetoAdjacente(Vector2 point_a, Vector2 point_b)
    {
        return GetMagnitude(point_a, new Vector2(point_b.x, point_a.y));
        // return point_a.x+Mathf.Abs(point_b.x);
    }

    public float GetMagnitude(Vector2 point_a, Vector2 point_b)
    {
        Vector2 distance = GetDistance(point_a, point_b);
        return Mathf.Sqrt(distance.x*distance.x + distance.y*distance.y);
    }

    public Vector2 GetDistance(Vector2 point_a, Vector2 point_b)
    {
        return point_b - point_a;
    }

    public float GetCos(Vector2 point_a, Vector2 point_b)
    {
        float catetoAdjacente   = GetCatetoAdjacente(point_a, point_b);
        float hipotenusa        = GetMagnitude(point_a, point_b);

        return catetoAdjacente/hipotenusa;
    }

    public float GetSen(Vector2 point_a, Vector2 point_b)
    {
        float catetoOposto      = GetCatetoOposto(point_a, point_b);
        float hipotenusa        = GetMagnitude(point_a, point_b);

        return catetoOposto/hipotenusa;

    }

}
