using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenO : MonoBehaviour
{
    /// <Rooms>
    ///  The camera follows the player around while staying in the room its in, so the camera needs to know...
    ///     what room its in, the bounds of the room
    ///  Each room needs to store the players first position in the room to store a spawn point when they die
    ///  Each Room should be able to reload and respawn the player
    ///  Each room should have an initial camera position for when the camera switches to it
    /// </Rooms>


    public Vector2 startpt; // respawn point of player
    
    Vector2 camerastartpt;
    GameObject player;
    Rigidbody2D rb;
    bool searchingPlayer = true;
    float halfscreen;
    float camSize;
    Camera cam;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
        cam = Camera.main;
        camSize = cam.orthographicSize;
        halfscreen = cam.aspect * cam.orthographicSize;
        slashCam.screenCenter = new Vector3(transform.position.x, transform.position.y, -camSize);
        //setConstraints();
    }

    void Update()
    {
        // TODO check this shit to make it more efficient

        // update player spawm
        if (playerInRoom() && searchingPlayer)
        {
            searchingPlayer = false;
            slash.startpt = player.transform.position;
            //cam.gameObject.transform.localPosition = transform.position;
            setConstraints();
            slashCam.updatedConstraints = true;

        }

        // TODO move the camera when its not meeting the updating constraints
        // move towards the screen untill inconstraints is true
 
        if (!playerInRoom()) searchingPlayer = true;

        


    }

    // get set the startpt of the screen
    public void setStartPt(Vector2 start)
    {startpt = start;}
    public Vector2 getStartPt()
    {return startpt;}

    // set the camera constraints based on the room
    public void setConstraints()
    {
        int leftx = (int)(transform.position.x - transform.localScale.x / 2 + halfscreen);
        int boty = (int)(transform.position.y - transform.localScale.y / 2 + camSize);
        int topy = (int)(transform.position.y + transform.localScale.y / 2 - camSize);
        int rightx = (int)(transform.position.x + transform.localScale.x / 2 - halfscreen);
        //Debug.Log((int)(transform.position.x + transform.localScale.x / 2 - halfscreen));
        slashCam.camConstraints = new Vector4(leftx,boty,topy,rightx);
        //Debug.Log(slashCam.camConstraints);
    }

    //// check to see if the camera is in the constraints
    public bool inConstraints()
    {
        bool left = cam.gameObject.transform.position.x >= transform.position.x - transform.localScale.x / 2 + halfscreen;
        bool right = cam.gameObject.transform.position.x <= transform.position.x + transform.localScale.x / 2 - halfscreen;
        bool up = cam.gameObject.transform.position.y <= transform.position.y + transform.localScale.y / 2 - camSize;
        bool down = cam.gameObject.transform.position.y >= transform.position.y - transform.localScale.y / 2 + camSize;
        //Debug.Log(transform.position.x + transform.localScale.x / 2 + halfscreen);
        return left && right && up && down;
    }

    // check to see if the players in the room
    public bool playerInRoom()
    {
        return (player.transform.position.x < transform.position.x + transform.localScale.x / 2)
        && (player.transform.position.x > transform.position.x - transform.localScale.x / 2)
        && (player.transform.position.y < transform.position.y + transform.localScale.y / 2)
        && (player.transform.position.y > transform.position.y - transform.localScale.y / 2);
    }

    // reload a screen screen gets reloaded on death
    public void reload()
    {
        // put the player back to startpt
        player.transform.position = new Vector3(startpt.x,startpt.y,-1);
        rb.velocity = Vector2.zero;
    }

}
