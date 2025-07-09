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
                StartCrashSequence();
                break;
        }
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
    
    private void StartCrashSequence()
    {
        _isControllable = false;
        _audioSource.Stop();
        _audioSource.PlayOneShot(crashSFX);
        crashParticle.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }
    
    private void StartSuccessSequence()
    {
        _isControllable = false;
        _audioSource.Stop();
        _audioSource.PlayOneShot(successSFX);
        successParticle.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
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
