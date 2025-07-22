using UnityEngine;
using UnityEngine.Serialization;

public class OscillationManager : MonoBehaviour
{
    public static float GlobalTime => Time.timeSinceLevelLoad;
}

public class HydraulicPressOscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float period;

    [SerializeField] [Range(0, 360)] private float phase;

    [SerializeField] [Range(0f, 5f)] private float timeToWaitMinima;

    [FormerlySerializedAs("timeToWait")] [SerializeField] [Range(0f, 5f)]
    private float timeToWaitMaxima;

    private Vector3 _startPosition;
    private Vector3 _movementFactor;
    private float _rawCosineFactor;
    private float _cosineAdjustmentFactor;
    private float _cycles;
    private const float MathTau = 2 * Mathf.PI;
    private const double Epsilon = 1e-2;
    private bool _freeze = true;
    private float _timeToWait;
    private float _startTImer;
    private float _angleCompensation;
    private float _phaseRad;

    void Start()
    {
        _startPosition = transform.position;
        _phaseRad = phase * Mathf.PI / 180;
    }

    void Update()
    {
        if (OscillationManager.GlobalTime - _startTImer < _timeToWait)
        {
            return;
        }

        _cycles = (OscillationManager.GlobalTime - (_startTImer + _timeToWait)) / period;

        _rawCosineFactor = -Mathf.Cos(_cycles * MathTau - (_phaseRad + _angleCompensation));
        _cosineAdjustmentFactor = (_rawCosineFactor + 1) / 2;

        _movementFactor = movementVector * _cosineAdjustmentFactor;
        transform.position = _startPosition + _movementFactor;

        if (_cosineAdjustmentFactor < Epsilon || _cosineAdjustmentFactor > 1 - Epsilon)
        {
            if (_freeze)
            {
                _startTImer = OscillationManager.GlobalTime;
                _freeze = false;

                if (_cosineAdjustmentFactor < Epsilon)
                {
                    _angleCompensation = -_phaseRad;
                    _timeToWait = timeToWaitMinima;
                }
                else
                {
                    _angleCompensation = Mathf.PI - _phaseRad;
                    _timeToWait = timeToWaitMaxima;
                }
            }
        }
        else
        {
            _freeze = true;
        }
    }
}