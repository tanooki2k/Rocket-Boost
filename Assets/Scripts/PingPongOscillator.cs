using UnityEngine;

public class PingPongOscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float speed;
    
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _movementFactor;
    private float _timer;
    private float _distance;
    
    void Start()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition + movementVector;
        _distance = movementVector.magnitude;
    }

    void Update()
    {
        _timer += Time.deltaTime;
    
        float tripDuration = _distance / speed;  // Duration of a one-way trip = distance / speed
        _movementFactor = Mathf.PingPong(_timer / tripDuration, 1f);  // Full ping-pong cycle (forward + back) takes twice the tripDuration

        
        transform.position = Vector3.Lerp(_startPosition, _endPosition, _movementFactor);
    }
}
