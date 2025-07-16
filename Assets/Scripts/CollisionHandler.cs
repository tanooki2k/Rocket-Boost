using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 3f;
    [SerializeField] private AudioClip successSFX;
    [SerializeField] private AudioClip crashSFX;
    [SerializeField] private ParticleSystem successParticle;
    [SerializeField] private ParticleSystem crashParticle;
    [SerializeField] [Range(100, 5000)] private float bounceMultiplier;

    private AudioSource _audioSource;

    private bool _isControllable = true;
    private bool _isCollidable = true;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    void OnCollisionEnter(Collision other)
    {
        if (!_isControllable || !_isCollidable) { return; }
        
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Everything is looking good!");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                var reactionVector = GetReactionVector(other);
                StartCrashSequence(reactionVector);
                break;
        }
    }

    private static Vector3 GetReactionVector(Collision other)
    {
        Vector3 reactionVector = Vector3.zero;
        if (other.contacts.Length > 0)
        {
            ContactPoint contact = other.contacts[0];
            Vector3 collisionNormal = contact.normal;

            // The reaction vector is the opposite of the collision normal
            reactionVector = -collisionNormal;
        }

        return reactionVector;
    }

    private void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            _isCollidable = !_isCollidable;
        }
    }
    
    private void StartCrashSequence(Vector3 reactionCollisionVector)
    {
        DisableGameplay();
        StartCrashEffects();
        GetComponent<Rigidbody>().AddForce(bounceMultiplier * reactionCollisionVector);
        Invoke("ReloadLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        DisableGameplay();
        StartSuccessEffects();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void DisableGameplay()
    {
        _isControllable = false;
        _audioSource.Stop();
        GetComponent<Movement>().enabled = false;
    }

    private void StartCrashEffects()
    {
        _audioSource.PlayOneShot(crashSFX);
        crashParticle.Play();
    }

    private void StartSuccessEffects()
    {
        _audioSource.PlayOneShot(successSFX);
        successParticle.Play();
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
    
    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = (currentScene + 1) % SceneManager.sceneCountInBuildSettings;

        SceneManager.LoadScene(nextScene);
    }
    
}
