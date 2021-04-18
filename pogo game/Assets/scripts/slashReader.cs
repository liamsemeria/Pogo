using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SLASH 19

public class slashReader : MonoBehaviour
{
    public static bool canJump = false;



    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag != "unslash" && other.gameObject.tag != "tutor")
            canJump = true;
        else canJump = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canJump = false;
    }
}
