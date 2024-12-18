using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(PixelPerfectCamera))]
public class BlackBars : MonoBehaviour
{
    private PixelPerfectCamera pixelPerfectCamera;
    public Vector2 targetResolution = new Vector2(320, 180);
    public Vector2 aspectRatio = new Vector2(16, 9);

    void Start()
    {
        Camera.main.aspect = aspectRatio.x / aspectRatio.y;

        pixelPerfectCamera = gameObject.GetComponent<PixelPerfectCamera>();

        pixelPerfectCamera.refResolutionX = (int)targetResolution.x;
        pixelPerfectCamera.refResolutionY = (int)targetResolution.y;
        pixelPerfectCamera.upscaleRT = true;
        pixelPerfectCamera.cropFrameX = true;
        pixelPerfectCamera.cropFrameY = true;
        pixelPerfectCamera.stretchFill = true;
    }
}