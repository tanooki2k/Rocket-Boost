using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 100f;
    [SerializeField] private float rotationStrength = 100f;
    [SerializeField] private AudioClip mainEngineSFX;
    [SerializeField] private ParticleSystem mainEngineParticle;
    [SerializeField] private ParticleSystem rightThrustParticle;
    [SerializeField] private ParticleSystem leftThrustParticle;


    Rigidbody _rb;
    AudioSource _audioSource;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            _rb.AddRelativeForce(Vector3.up * (Time.fixedDeltaTime * thrustStrength));
            if (!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(mainEngineSFX);
            }

            if (!mainEngineParticle.isPlaying)
            {
                mainEngineParticle.Play();
            }
        }
        else
        {
            _audioSource.Stop();
            mainEngineParticle.Stop();
        }
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();

        if (rotationInput < 0)
        {
            ApplyRotation(rotationStrength);
            if (!rightThrustParticle.isPlaying)
            {
                leftThrustParticle.Stop();
                rightThrustParticle.Play();
            }
        }
        else if (rotationInput > 0)
        {
            ApplyRotation(-rotationStrength);
            if (!leftThrustParticle.isPlaying)
            {
                rightThrustParticle.Stop();
                leftThrustParticle.Play();
            }
        }
        else
        {
            rightThrustParticle.Stop();
            leftThrustParticle.Stop();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        _rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * (Time.fixedDeltaTime * rotationThisFrame));
        _rb.freezeRotation = false;
    }
}