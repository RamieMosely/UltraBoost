using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delay = 1.5f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip landWin;

    [SerializeField] ParticleSystem particleCrash;
    [SerializeField] ParticleSystem particleLandWin;

    AudioSource rocketAudio;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        rocketAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        DebugNextLevel();
        TheDisableCollisions();

    }

    void DebugNextLevel()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();

        }
    }

    void TheDisableCollisions()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; //toggle collision
            Debug.Log("Collisions Disabled");
            //GetComponent<CollisionHandler>().enabled = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(isTransitioning || collisionDisabled)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("You collided with a friendly!");
                break;

            case "Fuel":
                Debug.Log("You Collided with Fuel!");
                break;

            case "Finished":
                StartVictorySequence();
                Invoke("LoadNextLevel", delay);
                break;

            default:
                StartCrashSequence();
                Invoke("ReloadLevel", delay);
                break;
        }

    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        rocketAudio.Stop();
        particleCrash.Play();
        rocketAudio.PlayOneShot(crash);
        GetComponent<Movement>().enabled = false;

    }

    void StartVictorySequence()
    {
        isTransitioning = true;
        rocketAudio.Stop();
        particleLandWin.Play();
        rocketAudio.PlayOneShot(landWin);
        GetComponent<Movement>().enabled = false;
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex ;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }
}
