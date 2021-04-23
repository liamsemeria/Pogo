using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SLASH 19

public class slashCam : MonoBehaviour
{
    // shake when a slash is performed
    // follow the player vertically, trying to keep the player in the bottom third of the screen
    // when the player touches a horizontal boundary switch 
    Transform target;
    Vector3 targetpos;
    float camSize;
    float halfscreen;
    float vertiSpeed;
    float horizSpeed;
    float switchSpeed;
    Vector3 vec; // the vector that determines camera movement
    Vector3 horizontalFollow; // the moveTowards for horizontal movement, used to change camera movement 
    Vector3 horizontalPull; //  Pull usually the same value as Follow, this ones used to see if the cameras movetowards is weaker than Follow when frozen
    Vector3 verticalFollow; // vertical counter part
    Vector3 verticalPull; // vertical counter part
    Transform targetR;
    public static Vector4 camConstraints; // (leftx,boty,topy,rightx)
    public static Vector3 screenCenter;
    public Transform backround;

    //camera shake
    public static int shakeFrames;
    Vector3 shaker;
    float size;
    public static Vector3 initial;
    bool switching = false;
    public static bool updatedConstraints;
    Vector2 nearest;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
       vertiSpeed = 40f; // 40
        horizSpeed = 20f; // 20
        switchSpeed = 450f;
        size = .01f;
        Camera cam = Camera.main;
        // if the level is reset, check to see if the camera is at 0
        //tiles.cellSize = new Vector3(cam.aspect / 2, 1 , 0);
        camSize = cam.orthographicSize;
        halfscreen = cam.aspect * camSize;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //backround.position = new Vector3(transform.position.x,transform.position.y,backround.position.z);
        if (switching && targetpos.x == Mathf.RoundToInt(transform.position.x) && targetpos.y == Mathf.RoundToInt(transform.position.y))
        {
            switching = false;
        }
        if (!switching)
        {
            targetpos = target.position;
            horizSpeed = 20;
            vertiSpeed = 40;
        }


        // HORIZONTAL SWITCHING
        if ((target.transform.position.x < transform.position.x - halfscreen) || (target.transform.position.x > transform.position.x + halfscreen))
        {
            horizSpeed = switchSpeed;
            if (updatedConstraints)
            {
                nearest = nearestConstraints();
                targetpos = new Vector3(nearest.x, nearest.y, target.position.z);
                updatedConstraints = false;
                switching = true;
            }
        }
       //else horizSpeed = 40;

        // VERTICAL SWITCHING
        if ((target.transform.position.y < transform.position.y - camSize) || (target.transform.position.y > transform.position.y + camSize))
        {
            vertiSpeed = switchSpeed;
            if (updatedConstraints)
            {
                nearest = nearestConstraints();
                targetpos = new Vector3(nearest.x, nearest.y, target.position.z);
                updatedConstraints = false;
                switching = true;
            }
        }
        //else vertiSpeed = 40;

        // HORIZONTAL TRACKING
        horizontalPull = Vector3.MoveTowards(transform.position, targetpos, horizSpeed * Time.deltaTime);
        // camera can only move within its constraints
        if ((camConstraints.x <= transform.position.x) && (transform.position.x <= camConstraints.w))
            horizontalFollow = Vector3.MoveTowards(transform.position, targetpos, horizSpeed * Time.deltaTime);
        // if the camera reaches an edge and the player is giving it room again, allow it to move
        // if the horizontalPull is greater than it was when the camera froze, then the player isnt at the max camera location anymore
        else if ((horizontalPull.x < horizontalFollow.x) && (transform.position.x > camConstraints.w)) // right side
            horizontalFollow = Vector3.MoveTowards(transform.position, targetpos, horizSpeed * Time.deltaTime);
        else if ((horizontalPull.x > horizontalFollow.x) && (transform.position.x < camConstraints.x)) // left side
            horizontalFollow = Vector3.MoveTowards(transform.position, targetpos, horizSpeed * Time.deltaTime);


        // VERTICAL TRACKING
        verticalPull = Vector3.MoveTowards(transform.position, targetpos, vertiSpeed * Time.deltaTime);
        // camera can only move within its constraints
        if ((camConstraints.y <= transform.position.y) && (transform.position.y < camConstraints.z))
            verticalFollow = Vector3.MoveTowards(transform.position, targetpos, vertiSpeed * Time.deltaTime);
        // if the camera reaches an edge and the player is giving it room again, allow it to move
        // if the horizontalPull is greater than it was when the camera froze, then the player isnt at the max camera location anymore
        else if ((verticalPull.y < verticalFollow.y) && (transform.position.y > camConstraints.z)) // up side
            verticalFollow = Vector3.MoveTowards(transform.position, targetpos, vertiSpeed * Time.deltaTime);
        else if ((verticalPull.y > horizontalFollow.y) && (transform.position.y < camConstraints.y)) // down side
            verticalFollow = Vector3.MoveTowards(transform.position, targetpos, vertiSpeed * Time.deltaTime);

            
        transform.position = new Vector3(horizontalFollow.x, verticalFollow.y, -camSize);
        //Debug.Log(transform.position);
        //else Vector3.MoveTowards(transform.position, target.transform.position, horizSpeed * Time.deltaTime);


        // CAMERA SHAKE
        // TODO rewrite shake to adjust to horizontal camera movement
        if (shakeFrames != 0)
        {
            shakeFrames--;
            shaker = new Vector3(initial.x, initial.y + Random.insideUnitCircle.y * size, initial.z);
            transform.position = shaker;

        }
        //transform.position = new Vector3(horizontalFollow.x, verticalFollow.y, -camSize);
    }


    public static void shake(int frames)
    {
        Camera cam = Camera.main;
        if (shakeFrames == 0)
        {
            initial = cam.gameObject.transform.position;
        }
        shakeFrames = frames;
    }

    Vector2 nearestConstraints()
    {
        float x;
        float y;
        // figure the one with the least distance
        if (Mathf.Abs(camConstraints.x - transform.position.x) < Mathf.Abs(camConstraints.w - transform.position.x))
        {
            x = camConstraints.x;
        }
        else x = camConstraints.w;

        if (Mathf.Abs(camConstraints.y - transform.position.y) < Mathf.Abs(camConstraints.z - transform.position.y))
        {
            y = camConstraints.y;
        }
        else y = camConstraints.z;
        return new Vector2(x,y);
    }

}
