using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _p;
    private float _horInput, _verInput = 0f;
    private float _fdt;
    private bool _cameraShifted = false;

    private void Start()
    {
        _p = Player.Instance;
        _fdt = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    private void Update()
    {
        _horInput = Input.GetAxisRaw("Horizontal") * _p.speed;
        _verInput = Input.GetAxisRaw("Vertical") * _p.speed;
        if (Input.GetKeyDown(KeyCode.X))
        {
            ShiftCamera();
        }
    }

    private void FixedUpdate()
    {
        if (_horInput != 0 || _verInput != 0)
        {
            _p.Move(_horInput * _fdt, _verInput * _fdt);
        }
    }
    
    private void ShiftCamera()
    {
        var camPos = _p.cam.transform.localPosition;
        if (_cameraShifted)
        {
            camPos.y = 2.0f;
            _p.cam.transform.localPosition = camPos;
        }
        else
        {
            camPos.y = 10.0f;
            _p.cam.transform.localPosition = camPos;
        }
    }
}
