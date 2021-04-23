using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class endscreen : MonoBehaviour
{
    public List<Text> items;
    int trinketTotal = 4;
    public static GameObject counter;
    int trinkets;
    int scars;
    int minutes;
    int seconds;
    // Start is called before the first frame update
    void Start()
    {
        counter = GameObject.FindWithTag("counter");
        trinkets = (int)counter.transform.position.y;
        scars = (int)counter.transform.position.x;
        minutes = (int)counter.transform.position.z;
        seconds = minutes % 60;
        minutes = minutes / 60;
        items[1].text = "SCARS: " + scars;
        items[3].text = "TIME: " + minutes + "." + seconds;
        if (trinketTotal <= trinkets)
        {
            items[0].color = Color.red;
            items[0].text = "RED HEART";
            items[2].text = "TRINKETS: 4/4";
            
        }
        else
        {
            items[0].text = "WHITE HEART";
            items[2].text = "TRINKETS: " + trinkets + "/4";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.R))
        {
            counter.transform.position = Vector3.zero;
            SceneManager.LoadScene(0);
        }
    }
}
