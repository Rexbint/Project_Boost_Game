using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // PARAMETERS
    [SerializeField] float rocketThrust = 100f;
    [SerializeField] float rocketRotation = 100f;

    // CACHE
    [SerializeField] AudioClip mainEngineSFX;
    [SerializeField] ParticleSystem leftSideParticles;
    [SerializeField] ParticleSystem rightSideParticles;
    [SerializeField] ParticleSystem mainBoosterParticles;
    Rigidbody rocketRigidbody;
    AudioSource audioSource;

    // STATE

    void Start()
    {
        rocketRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessInput();
        ProcessRotation();
    }

    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }
    
    void StartThrusting()
    {
        rocketRigidbody.AddRelativeForce(Vector3.up * Time.deltaTime * rocketThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSFX);
        }
        if (!mainBoosterParticles.isPlaying)
        {
            mainBoosterParticles.Play();
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        mainBoosterParticles.Stop();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    void StopRotating()
    {
        rightSideParticles.Stop();
        leftSideParticles.Stop();
    }

    void RotateRight()
    {
        ApplyRotation(-rocketRotation);
        if (!rightSideParticles.isPlaying)
        {
            rightSideParticles.Play();
        }
    }

    void RotateLeft()
    {
        ApplyRotation(rocketRotation);
        if (!leftSideParticles.isPlaying)
        {
            leftSideParticles.Play();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rocketRigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);
        rocketRigidbody.freezeRotation = false;
    }
}
