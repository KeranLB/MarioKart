using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Unity.VisualScripting;

public class KartControler : MonoBehaviour
{
    #region rewired
    private Player _player;
    public int playerId = 0;
    #endregion
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ItemManager _itemManager;
    [SerializeField] private RaceStart _raceStart;

    [Header("Parameters Speed :")]
    [SerializeField] private float _speed; // Debug
    [SerializeField, Range(1, 75)] private float _speedMax;
    [SerializeField, Range(10, 100)] private float _speedRotation;
    [SerializeField, Range(0, 1)] private float _accelerationFactor;
    [SerializeField] private AnimationCurve _accelerationCurve;

    [Header("Parameters Boost :")]
    [SerializeField, Range(1, 100)] private float _dashForce, _boostSpeed;
    [SerializeField, Range(0, 10)] private int _boostDuration;

    private float _accelerationLerpFactor, _rotationInput, _fieldAfector;

    private bool _isAccelerating, _isBoosting, _isClutching, _isDrifting;

    // Start is called before the first frame update
    void Start()
    {
        _player = ReInput.players.GetPlayer(playerId);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_raceStart.canRace)
        {
            if (_player.GetButtonDown("Acceleration") && _raceStart.countdown == 2)
            {
                _raceStart.startBoost = true;
            }
            else if (_player.GetButtonDown("Acceleration") && _raceStart.countdown != 2)
            {
                _raceStart.startBoost = false;
            }
        }
        else
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

            if (_player.GetButtonDown("Drift"))
            {
                _isDrifting = true;
            }
            if (_player.GetButtonUp("Drift"))
            {
                _isDrifting = false;
            }

            if (_player.GetButtonDown("UseItem"))
            {
                _itemManager.UseItem();
            }

            _rotationInput = _player.GetAxis("Horizontal");
        }
    }

    public void Boost()
    {
        StartCoroutine(BoostCoroutine());
    }

    IEnumerator BoostCoroutine()
    {
        _isBoosting = true;
        yield return new WaitForSeconds(_boostDuration);
        _isBoosting = false;
    }

    public void Dash(int orientation)
    {
        _rb.MovePosition(transform.position + transform.forward * _dashForce * orientation);
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

        if (Physics.Raycast(transform.position, -transform.up, 0.5f, LayerMask.GetMask("Grass")))
        {
            _fieldAfector = 0.25f;
        }
        else
        {
            _fieldAfector = 0.5f;
        }

        _accelerationLerpFactor = Mathf.Clamp01(_accelerationLerpFactor);

        if (_isBoosting)
        {
            _speed = _boostSpeed;
        }
        else
        {
            _speed = _accelerationCurve.Evaluate(_accelerationLerpFactor) * _speedMax * _fieldAfector;
        }

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + _speedRotation * Time.deltaTime * _rotationInput , 0);

        if (_isDrifting)
        {
            _rb.MovePosition(transform.position + -transform.right * _speed * Time.fixedDeltaTime + transform.forward * -_rotationInput * _speed * Time.fixedDeltaTime);
        }
        else
        {
            _rb.MovePosition(transform.position + -transform.right * _speed * Time.fixedDeltaTime);
        }
    }
}
