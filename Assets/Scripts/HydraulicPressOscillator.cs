using UnityEngine;

public class HydraulicPressOscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float period;

    [SerializeField] [Range(0, 2 * Mathf.PI)]
    private float phase;

    [SerializeField] [Range(0f, 10f)] private float timeToWait;

    private Vector3 _startPosition;
    private Vector3 _movementFactor;
    private float _rawCosineFactor;
    private float _cosineAdjustmentFactor;
    private float _cycles;
    private const float MathTau = 2 * Mathf.PI;
    private const double Epsilon = 1e-3;
    private bool _freeze = true;
    private float _startTImer;
    private float _angleCompensation;

    void Start()
    {
        _startPosition = transform.position;
        _startTImer = -timeToWait;
    }

    void Update()
    {
        if (Time.time - _startTImer < timeToWait) { return; }
        
        _cycles = (Time.time - (_startTImer + timeToWait)) / period;

        _rawCosineFactor = -Mathf.Cos(_cycles * MathTau - (phase + _angleCompensation));
        _cosineAdjustmentFactor = (_rawCosineFactor + 1) / 2;

        _movementFactor = movementVector * _cosineAdjustmentFactor;
        transform.position = _startPosition + _movementFactor;

        if (_cosineAdjustmentFactor < Epsilon || _cosineAdjustmentFactor > 1 - Epsilon)
        {
            if (_freeze)
            {
                _startTImer = Time.time;
                _freeze = false;

                if (_cosineAdjustmentFactor < Epsilon)
                {
                    _angleCompensation = -phase;
                }
                else
                {
                    _angleCompensation = Mathf.PI - phase;
                }
            }
        }
        else
        {
            _freeze = true;
        }
    }
}