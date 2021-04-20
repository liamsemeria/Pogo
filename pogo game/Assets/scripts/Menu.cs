using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    bool isOpen;
    public List<Text> options;
    int selectedindex = 0;
    bool flip = false;
    GameObject player;
    GameObject settings;
    Color32 textcol;
    Color32 highlight;
    bool updatedControls = false;
    // player rigidbody
    // Start is called before the first frame update
    void Start()
    {
        close();
        player = GameObject.FindWithTag("Player");
        textcol = new Color32(255, 220, 220, 255);
        highlight = new Color32(255, 65, 65, 255);
        settings = GameObject.FindWithTag("settings");
    }
    // Update is called once per frame
    void Update()
    {
        if (!updatedControls)
        {
            setControls(settings.transform.position);
            updatedControls = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) open();

        if (isOpen)
        {
            // highlight the selected text
            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectedindex)
                    options[i].color = highlight;
                else options[i].color = textcol;
            }
            // traverse menu (circular)
            if (Input.GetKeyDown(slash.Down)) selectedindex++;
            if (Input.GetKeyDown(slash.Up)) selectedindex--;
            if (selectedindex == options.Count) selectedindex = 0;
            if (selectedindex == -1) selectedindex = options.Count - 1;

            if (Input.GetKeyDown(slash.Slash) && (selectedindex != -1))
            {
                switch (selectedindex)
                {
                    case 0: // continue
                        close();
                        break;
                    case 1: // retry
                        player.transform.position = slash.startpt;
                        close();
                        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        break;
                    case 2: // flip controls
                        flip = true;
                        break;
                    case 3: // exit game
                        Application.Quit();
                        break;
                }
            }
        }
        
    }

    void open()
    {
        for (int i = 0; i < options.Count; i++)
        {
            options[i].gameObject.SetActive(true);
        }
        isOpen = true;
    }

    void close()
    {
        for (int i = 0; i < options.Count; i++)
        {
            options[i].gameObject.SetActive(false);
        }
        isOpen = false;
        // flip controls if wanted
        if (flip)
        {
            flipControls();
            flip = false;
        }
    }

    void flipControls()
    {
        if (settings.transform.position != Vector3.zero)
        {
            slash.Left = KeyCode.LeftArrow;
            slash.Right = KeyCode.RightArrow;
            slash.Up = KeyCode.UpArrow;
            slash.Down = KeyCode.DownArrow;
            slash.MoveLeft = KeyCode.A;
            slash.MoveRight = KeyCode.D;
            slash.Slash = KeyCode.S;
            settings.transform.position = Vector3.zero;
        }
        else
        {
            slash.Left = KeyCode.A;
            slash.Right = KeyCode.D;
            slash.Up = KeyCode.W;
            slash.Down = KeyCode.S;
            slash.MoveLeft = KeyCode.LeftArrow;
            slash.MoveRight = KeyCode.RightArrow;
            slash.Slash = KeyCode.DownArrow;
            settings.transform.position = Vector3.right;
        }
    }

    void setControls(Vector3 setting)
    {
        if (setting==Vector3.zero)
        {
            slash.Left = KeyCode.LeftArrow;
            slash.Right = KeyCode.RightArrow;
            slash.Up = KeyCode.UpArrow;
            slash.Down = KeyCode.DownArrow;
            slash.MoveLeft = KeyCode.A;
            slash.MoveRight = KeyCode.D;
            slash.Slash = KeyCode.S;
        }
        else
        {
            slash.Left = KeyCode.A;
            slash.Right = KeyCode.D;
            slash.Up = KeyCode.W;
            slash.Down = KeyCode.S;
            slash.MoveLeft = KeyCode.LeftArrow;
            slash.MoveRight = KeyCode.RightArrow;
            slash.Slash = KeyCode.DownArrow;
        }
    }

}
