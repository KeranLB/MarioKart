using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class KartControler : MonoBehaviour
{
    #region rewired
    private Player _player;
    public int playerId = 0;
    #endregion
    [SerializeField] private Rigidbody _rb;

    [Header("Parameters Speed :")]
    [SerializeField] private float _speed; // Debug
    [SerializeField, Range(1, 75)] private float _speedMax;
    [SerializeField, Range(1,100)] private float _boostSpeed;
    [SerializeField, Range(10,100)] private float _speedRotation;
    [SerializeField, Range(0,1)] private float _accelerationFactor;

    private float _accelerationLerpFactor, _rotationInput;
    [SerializeField] private AnimationCurve _accelerationCurve;
    private bool _isAccelerating, _isBoosting, _isClutching;

    // Start is called before the first frame update
    void Start()
    {
        _player = ReInput.players.GetPlayer(playerId);
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.GetButtonDown("Acceleration"))
        {
            _isAccelerating = true;
        }
        if (_player.GetButtonUp("Acceleration"))
        {
            _isAccelerating = false;
        }

        if (_player.GetButtonDown("Clutch"))
        {
            _isClutching = true;
        }
        if (_player.GetButtonUp("Clutch"))
        {
            _isClutching = false;
        }

        _rotationInput = _player.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (_isAccelerating)
        {
            _accelerationLerpFactor += _accelerationFactor;
        }
        else if (_isClutching)
        {
            _accelerationLerpFactor -= _accelerationFactor * 2;
        }
        else
        {
            _accelerationLerpFactor -= _accelerationFactor;
        }

        _accelerationLerpFactor = Mathf.Clamp01(_accelerationLerpFactor);

        _speed = _accelerationCurve.Evaluate(_accelerationLerpFactor) * _speedMax;

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + _speedRotation * Time.deltaTime * _rotationInput , 0);
        
        _rb.MovePosition(transform.position + -transform.right * _speed * Time.fixedDeltaTime);
    }
}
