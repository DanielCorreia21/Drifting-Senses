using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float runSpeed = 40f;

    private CharacterController2D _controller;
    private float _horizontalMove = 0f;
    private bool _jump = false;
    private bool _crouch = false;
    private bool _dash = false;

    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
        }

        if (Input.GetButtonDown("Dash"))
        {
            Debug.Log("Dash");
            _dash = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            _crouch = true;
        }else if (Input.GetButtonUp("Crouch"))
        {
            _crouch = false;
        }
    }

    void FixedUpdate()
    {
        _controller.Move(_horizontalMove * Time.fixedDeltaTime, _crouch, _jump, _dash);
        _jump = false;
        _dash = false;
    }
}
