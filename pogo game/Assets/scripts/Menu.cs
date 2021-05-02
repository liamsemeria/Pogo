using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public static bool isOpen;
    public List<Text> options;
    List<string> old_options;
    List<string> selected_options;
    int selectedindex = 0;
    bool flip = false;
    GameObject player;
    GameObject settings;
    GameObject counter;
    Color32 textcol;
    Color32 highlight;
    bool updatedControls = false;
    public bool isMain = false;

    void Awake()
    {
        old_options = new List<string>();
        selected_options = new List<string>();
        // copy options
        for (int i = 0; i < options.Count; i++)
        {
            old_options.Add(options[i].text);
            selected_options.Add(options[i].text);
            selected_options[i] += " (c)";
        }
    }
    void Start()
    {
        if (!isMain) close();
        else open();
        player = GameObject.FindWithTag("Player");
        textcol = new Color32(255, 220, 220, 255);
        highlight = new Color32(185, 0, 0, 255);
        settings = GameObject.FindWithTag("settings");
        counter = GameObject.FindWithTag("counter");
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) open();

        if (isOpen)
        {
            // highlight the selected text
            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectedindex)
                {
                    options[i].color = highlight;
                    options[i].text = selected_options[i];
                }
                else
                {
                    options[i].color = textcol;
                    options[i].text = old_options[i];
                }
            }
            // traverse menu (circular)
            if (Input.GetKeyDown(KeyCode.DownArrow)) selectedindex++;
            if (Input.GetKeyDown(KeyCode.UpArrow)) selectedindex--;
            if (selectedindex == options.Count) selectedindex = 0;
            if (selectedindex == -1) selectedindex = options.Count - 1;

            if (Input.GetKeyDown(KeyCode.C) && (selectedindex != -1))
            {
                switch (selectedindex)
                {
                    case 0: // game: continue main: start
                        if (isMain)
                        {
                            SceneManager.LoadScene(1);
                        }
                        else close();
                        break;
                    case 1: // game: retry main: exit
                        if (isMain) Application.Quit();
                        else player.transform.position = slash.startpt;
                        close();
                        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        break;
                    case 2: // reset game
                        counter.transform.position = Vector3.zero;
                        SceneManager.LoadScene(1);
                        break;
                    case 3: // exit to menu
                        SceneManager.LoadScene(0);
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


}
