using UnityEngine;

public class CosineOscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float period;
    [SerializeField] [Range(0, 2 * Mathf.PI)] private float phase;

    private Vector3 _startPosition;
    private Vector3 _movementFactor;
    private float _rawCosineFactor;
    private float _cosineAdjustmentFactor;
    private float _cycles;
    private const float MathTau = 2 * Mathf.PI;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        _cycles = Time.time / period;
        
        _rawCosineFactor = -Mathf.Cos(_cycles * MathTau + phase);
        _cosineAdjustmentFactor = (_rawCosineFactor + 1) * 1/2;
        
        _movementFactor = movementVector * _cosineAdjustmentFactor;
        transform.position = _startPosition + _movementFactor;
    }
}