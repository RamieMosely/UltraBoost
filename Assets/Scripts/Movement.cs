using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    

    // PARAMETERS - for tuning, typically set in the editor
    // Cache - references for readability or speed
    //state private instance (member) variables


    //Parameters
    [SerializeField] float mainThrust = 0f;
    [SerializeField] AudioClip thrust;
    [SerializeField] float rotationSpeed = 0f;

    [SerializeField] ParticleSystem particleMainThrust;
    [SerializeField] ParticleSystem particleLeftThrust;
    [SerializeField] ParticleSystem particleRightThrust;

    //Cache
    Rigidbody rocketRigidbody;
    AudioSource rocketAudio;
    bool isRocketSoundPlaying;



    // Start is called before the first frame update
    void Start()
    {
        rocketRigidbody = GetComponent<Rigidbody>();
        rocketAudio = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
        
    }

    void ProcessThrust()
    {
        //Activate Thrusters
        ActivateMainThrust();
    }

    void ActivateMainThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rocketRigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!rocketAudio.isPlaying && !particleMainThrust.isPlaying)
            {
                rocketAudio.PlayOneShot(thrust);
                particleMainThrust.Play();
            }
        }
        else
        {
            rocketAudio.Stop();
            particleMainThrust.Stop();
        }
    }

    void ProcessRotation()
    {
        //Rotate Left
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationSpeed);
            if(!particleLeftThrust.isPlaying)
            {
                particleLeftThrust.Play();
            }

        }
        //Rotate Right
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationSpeed);
            if (!particleRightThrust.isPlaying)
            {
                particleRightThrust.Play();
            }

        }
        else
        {
            particleLeftThrust.Stop();
            particleRightThrust.Stop();

        }

    }

    void ApplyRotation(float rotationThisFrame)
    {
        rocketRigidbody.freezeRotation = true; //freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);

        rocketRigidbody.freezeRotation = false; //unfreezing rotation so physics system can take over;
    }
}
