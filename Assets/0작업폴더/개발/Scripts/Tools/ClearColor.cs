// From documentary: https://docs.unity3d.com/ScriptReference/Color-clear.html

// Attach this script to a GameObject with a Renderer (go to Create>3D Object and select one of the first 6 options to create a GameObject with a Renderer automatically attached).
// This script changes the Color of your GameObject’s Material when your mouse hovers over it in Play Mode.

using UnityEngine;

public class ClearColor : MonoBehaviour
{
    private Camera _camera;
    //private Renderer _renderer;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        //_renderer = GetComponent<Renderer>();
        //_renderer.material.color = Color.clear;
        _camera.backgroundColor = Color.clear;
    }
}