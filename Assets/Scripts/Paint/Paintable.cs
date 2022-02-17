using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable : MonoBehaviour
{
    private const int TEXTURE_SIZE = 1024;

    public float extendIsLandOffset = 1;    //?
    
    private RenderTexture extendIsIandsRenderTexture;
    private RenderTexture uvRenderTexture;
    private RenderTexture maskRenderTexture;
    private RenderTexture supportRenderTexture;

    Renderer rend;

    private int maskTextureID = Shader.PropertyToID("_MaskTexture");

    public RenderTexture GetMask() => maskRenderTexture;
    public RenderTexture GetUVIslands() => uvRenderTexture;
    public RenderTexture GetExtend() => extendIsIandsRenderTexture;
    public RenderTexture GetSupport() => supportRenderTexture;
    public Renderer GetRenderer() => rend;

    void Start()
    {

        maskRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        maskRenderTexture.filterMode = FilterMode.Bilinear;

        extendIsIandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        extendIsIandsRenderTexture.filterMode = FilterMode.Bilinear;
        
        uvRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        uvRenderTexture.filterMode = FilterMode.Bilinear;
        
        supportRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        supportRenderTexture.filterMode = FilterMode.Bilinear;

        rend = GetComponent<Renderer>();
        rend.material.SetTexture(maskTextureID,extendIsIandsRenderTexture);


        PaintManager.instance.initTextures(this);

    }

    private void OnDisable()
    {
        maskRenderTexture.Release();
        extendIsIandsRenderTexture.Release();
        supportRenderTexture.Release();
        uvRenderTexture.Release();
    }
}
