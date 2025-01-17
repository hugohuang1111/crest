﻿// Crest Ocean System

// This file is subject to the MIT License as seen in the root of this folder structure (LICENSE)

using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine.XR;

/// <summary>
/// A simple and dumb camera script that can be controlled using WASD and the mouse.
/// </summary>
public class CamController : MonoBehaviour
{
    /// <summary>
    /// The version of this asset. Can be used to migrate across versions. This value should
    /// only be changed when the editor upgrades the version.
    /// </summary>
    [SerializeField, HideInInspector]
#pragma warning disable 414
    int _version = 0;
#pragma warning restore 414

    public float linSpeed = 10f;
    public float rotSpeed = 70f;

    public bool simForwardInput = false;
    public bool _requireLMBToMove = false;

    Vector2 _lastMousePos = -Vector2.one;
    bool _dragging = false;

    public float _fixedDt = 1 / 60f;

    Transform _targetTransform;

#pragma warning disable CS0108
    // In editor we need to use "new" to suppress warning but then gives warning when building so use pragma instead.
    Camera camera;
#pragma warning restore CS0108

    [System.Serializable]
    class DebugFields
    {
        [Tooltip("Disables the XR occlusion mesh for debugging purposes. Only works with legacy XR.")]
        public bool disableOcclusionMesh = false;

        [Tooltip("Sets the XR occlusion mesh scale. Useful for debugging refractions. Only works with legacy XR."), UnityEngine.Range(1f, 2f)]
        public float occlusionMeshScale = 1f;
    }

    [SerializeField] DebugFields _debug = new DebugFields();

    void Awake()
    {
        _targetTransform = transform;

        camera = GetComponent<Camera>();
        if (camera == null)
        {
            enabled = false;
            return;
        }

#if ENABLE_VR && ENABLE_VR_MODULE
        // We cannot change the Camera's transform when XR is enabled. This is not an issue with the new XR plugin.
        if (XRSettings.enabled)
        {
            // Disable XR temporarily so we can change the transform of the camera.
            XRSettings.enabled = false;
            // The VR camera is moved in local space, so we can move the camera if we move its parent we create instead.
            var parent = new GameObject("VRCameraOffset");
            parent.transform.parent = _targetTransform.parent;
            // Copy the transform over to the parent.
            parent.transform.position = _targetTransform.position;
            parent.transform.rotation = _targetTransform.rotation;
            // Parent camera to offset and reset transform. Scale changes slightly in editor so we will reset that too.
            _targetTransform.parent = parent.transform;
            _targetTransform.localPosition = Vector3.zero;
            _targetTransform.localRotation = Quaternion.identity;
            _targetTransform.localScale = Vector3.one;
            // We want to manipulate this transform.
            _targetTransform = parent.transform;
            XRSettings.enabled = true;

            // Seems like the best place to put this for now. Most XR debugging happens using this component.
            XRSettings.useOcclusionMesh = !_debug.disableOcclusionMesh;
            XRSettings.occlusionMaskScale = _debug.occlusionMeshScale;
        }
#endif
    }

    void Update()
    {
        float dt = Time.deltaTime;
        if (_fixedDt > 0f)
            dt = _fixedDt;

        UpdateMovement(dt);

#if ENABLE_VR && ENABLE_VR_MODULE
        // These aren't useful and can break for XR hardware.
        if (!XRSettings.enabled || XRSettings.loadedDeviceName == "MockHMD")
#endif
        {
            UpdateDragging(dt);
            UpdateKillRoll();
        }

#if ENABLE_VR && ENABLE_VR_MODULE
        if (XRSettings.enabled)
        {
            // Check if property has changed.
            if (XRSettings.useOcclusionMesh == _debug.disableOcclusionMesh)
            {
                XRSettings.useOcclusionMesh = !_debug.disableOcclusionMesh;
            }

            XRSettings.occlusionMaskScale = _debug.occlusionMeshScale;
        }
#endif
    }

    void UpdateMovement(float dt)
    {

#if ENABLE_INPUT_SYSTEM
        if (!Mouse.current.leftButton.isPressed && _requireLMBToMove) return;
        float forward = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0);
#else
        if (!Input.GetMouseButton(0) && _requireLMBToMove) return;
        float forward = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
#endif
        if (simForwardInput)
        {
            forward = 1f;
        }

        _targetTransform.position += linSpeed * _targetTransform.forward * forward * dt;
        var speed = linSpeed;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current.leftShiftKey.isPressed)
#else
        if (Input.GetKey(KeyCode.LeftShift))
#endif
        {
            speed *= 3f;
        }

        _targetTransform.position += speed * _targetTransform.forward * forward * dt;
        //_transform.position += linSpeed * _transform.right * Input.GetAxis( "Horizontal" ) * dt;
#if ENABLE_INPUT_SYSTEM
        _targetTransform.position += linSpeed * _targetTransform.up * (Keyboard.current.eKey.isPressed ? 1 : 0) * dt;
        _targetTransform.position -= linSpeed * _targetTransform.up * (Keyboard.current.qKey.isPressed ? 1 : 0) * dt;
        _targetTransform.position -= linSpeed * _targetTransform.right * (Keyboard.current.aKey.isPressed ? 1 : 0) * dt;
        _targetTransform.position += linSpeed * _targetTransform.right * (Keyboard.current.dKey.isPressed ? 1 : 0) * dt;
        _targetTransform.position += speed * _targetTransform.up * (Keyboard.current.eKey.isPressed ? 1 : 0) * dt;
        _targetTransform.position -= speed * _targetTransform.up * (Keyboard.current.qKey.isPressed ? 1 : 0) * dt;
        _targetTransform.position -= speed * _targetTransform.right * (Keyboard.current.aKey.isPressed ? 1 : 0) * dt;
        _targetTransform.position += speed * _targetTransform.right * (Keyboard.current.dKey.isPressed ? 1 : 0) * dt;
#else
        _targetTransform.position += linSpeed * _targetTransform.up * (Input.GetKey(KeyCode.E) ? 1 : 0) * dt;
        _targetTransform.position -= linSpeed * _targetTransform.up * (Input.GetKey(KeyCode.Q) ? 1 : 0) * dt;
        _targetTransform.position -= linSpeed * _targetTransform.right * (Input.GetKey(KeyCode.A) ? 1 : 0) * dt;
        _targetTransform.position += linSpeed * _targetTransform.right * (Input.GetKey(KeyCode.D) ? 1 : 0) * dt;
        _targetTransform.position += speed * _targetTransform.up * (Input.GetKey(KeyCode.E) ? 1 : 0) * dt;
        _targetTransform.position -= speed * _targetTransform.up * (Input.GetKey(KeyCode.Q) ? 1 : 0) * dt;
        _targetTransform.position -= speed * _targetTransform.right * (Input.GetKey(KeyCode.A) ? 1 : 0) * dt;
        _targetTransform.position += speed * _targetTransform.right * (Input.GetKey(KeyCode.D) ? 1 : 0) * dt;
#endif
        {
            float rotate = 0f;
#if ENABLE_INPUT_SYSTEM
            rotate += (Keyboard.current.rightArrowKey.isPressed ? 1 : 0);
            rotate -= (Keyboard.current.leftArrowKey.isPressed ? 1 : 0);
#else
            rotate += (Input.GetKey(KeyCode.RightArrow) ? 1 : 0);
            rotate -= (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
#endif

            rotate *= 5f;
            Vector3 ea = _targetTransform.eulerAngles;
            ea.y += 0.1f * rotSpeed * rotate * dt;
            _targetTransform.eulerAngles = ea;
        }
    }

    void UpdateDragging(float dt)
    {
        Vector2 mousePos =
#if ENABLE_INPUT_SYSTEM
            Mouse.current.position.ReadValue();
#else
            Input.mousePosition;
#endif

        var wasLeftMouseButtonPressed =
#if ENABLE_INPUT_SYSTEM
            Mouse.current.leftButton.wasPressedThisFrame;
#else
            Input.GetMouseButtonDown(0);
#endif

        if (!_dragging && wasLeftMouseButtonPressed && camera.rect.Contains(camera.ScreenToViewportPoint(mousePos)) &&
            !Crest.OceanDebugGUI.OverGUI(mousePos))
        {
            _dragging = true;
            _lastMousePos = mousePos;
        }
#if ENABLE_INPUT_SYSTEM
        if (_dragging && Mouse.current.leftButton.wasReleasedThisFrame)
#else
        if (_dragging && Input.GetMouseButtonUp(0))
#endif
        {
            _dragging = false;
            _lastMousePos = -Vector2.one;
        }

        if (_dragging)
        {
            Vector2 delta = mousePos - _lastMousePos;

            Vector3 ea = _targetTransform.eulerAngles;
            ea.x += -0.1f * rotSpeed * delta.y * dt;
            ea.y += 0.1f * rotSpeed * delta.x * dt;
            _targetTransform.eulerAngles = ea;

            _lastMousePos = mousePos;
        }
    }

    void UpdateKillRoll()
    {
        Vector3 ea = _targetTransform.eulerAngles;
        ea.z = 0f;
        transform.eulerAngles = ea;
    }
}
