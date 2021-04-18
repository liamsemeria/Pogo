using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    public bool isFinal;
    public int sceneIndex;
    public static GameObject counter; // the gameobject representing what levels player has unlocked
    Animator animator;
    AudioSource song;
    Time gametime;
    // Start is called before the first frame update
    void Awake()
    {
        counter = GameObject.FindWithTag("counter");
        song = GameObject.FindWithTag("song").GetComponent<AudioSource>();
        DontDestroyOnLoad(song);
        DontDestroyOnLoad(counter);

    }
    void Start()
    {
        animator = GetComponent<Animator>();
        counter = GameObject.FindWithTag("counter");
        if (!song.isPlaying) song.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // z value of counter is time passed
        counter.transform.position = counter.transform.position + Vector3.forward * Time.deltaTime;

        if (isFinal && (counter.transform.position.y >= 4))
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        // decide wether the object is active
        //if (counter.transform.position.x >= sceneIndex) isActive = true;
        // enable animator based on wether or not active
        //if (!isActive) animator.enabled = false;
        //else animator.enabled = true;
        //if (Input.GetKeyDown(KeyCode.Space)) isActive = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // load level with scene index if loader flower is active
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        
    }
    // axis 0 x, axis 1, y
    public static void counterincr(bool axis)
    {
        if (!axis)
            counter.transform.position = counter.transform.position + Vector3.right;
        else counter.transform.position = counter.transform.position + Vector3.up;
    }
}
