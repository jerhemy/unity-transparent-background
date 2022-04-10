using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotate : MonoBehaviour
{
    public GameObject Model;
    public Vector2 RoatationSensitivity = Vector2.right;

    private Collider _collider;
    private Camera _camera;
    private bool _rotationEnabled;
    private Vector2 _prevMouse;
    private Vector2 _mouse;

    void Awake()
    {
        _camera = Camera.main;
        _collider = Model.GetComponent<Collider>();
        if (_collider == null)
        {
            Debug.LogError("Model is missing a collider. Rotations will not work!");
            Destroy(this);
        }
    }

    void Update()
    {
        _mouse = Input.mousePosition;

        if(Input.GetMouseButtonDown(0)) //Left Click
        {
            Ray ray = _camera.ScreenPointToRay(_mouse);
            if(Physics.Raycast(ray, out _))
                _rotationEnabled = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _rotationEnabled = false;
        }
        else if (_rotationEnabled)
        {
            Vector2 spin = (_mouse - _prevMouse) * RoatationSensitivity;
            Model.transform.Rotate(new Vector2(spin.y, -spin.x) * Time.deltaTime);
        }

        _prevMouse = _mouse;
    }
}
