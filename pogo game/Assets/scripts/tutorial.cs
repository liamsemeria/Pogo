using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorial : MonoBehaviour
{
    GameObject slot;
    Text prompter;
    int step = 0;
    bool show = true;
    bool finished  = false;
    public bool isstart;
    public string triggerString;
    // Start is called before the first frame update
    void Start()
    {
        slot = GameObject.FindWithTag("tutor");
        prompter = slot.GetComponent<Text>();
        prompter.color = new Color32(255, 220, 220, 255);
    }

    // Update is called once per frame
    void Update()
    {


        if (isstart)
        {


                if (show) prompter.text = "use the arrow keys to move on the ground and in the air";
                if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && show) StartCoroutine(delaystep());
                if (slash.inputAim && Input.GetKeyDown(slash.Slash) && finished)
                {
                    prompter.text = "";
                }
        }

    }


    IEnumerator delaystep()
    {

        prompter.text = "";
        show = false;
        yield return new WaitForSeconds(3);
        prompter.text = "use the arrow keys to angle your reticle and (c) to boost in the opposite direction on hit";
        finished = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Player")
            prompter.text = triggerString;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            prompter.text = "";
    }
}
