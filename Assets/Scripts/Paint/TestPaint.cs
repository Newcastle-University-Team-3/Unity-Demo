using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPaint : MonoBehaviour
{
    private const int TextureSize = 1024;

    //public float extendIsLandOffset = 1

    public RenderTexture uvRenderTexture;   //当前的贴图uv值
    public RenderTexture extendRenderTexture;   //展开后的贴图
    public RenderTexture maskRenderTexture;

    private Renderer renderer;

    private int maskTextureID = Shader.PropertyToID("_MaskTexture");

    public RenderTexture GetUVRenderTexture() => uvRenderTexture;
    public RenderTexture GetmaskRenderTexture() => maskRenderTexture;
    public RenderTexture GetExtendRenderTexture() => extendRenderTexture;
    public Renderer GetRenderer() => renderer;

    private void Start()
    {
        //初始化
        uvRenderTexture = new RenderTexture(TextureSize,TextureSize,0);
        uvRenderTexture.filterMode = FilterMode.Bilinear;

        maskRenderTexture = new RenderTexture(TextureSize, TextureSize, 0);
        maskRenderTexture.filterMode = FilterMode.Bilinear;

        extendRenderTexture = new RenderTexture(TextureSize, TextureSize, 0);
        extendRenderTexture.filterMode = FilterMode.Bilinear;

        renderer = GetComponent<Renderer>();
        renderer.material.SetTexture(maskTextureID,extendRenderTexture);    //把mask的颜色设置为展开后取得的贴图的值

        //为每个物体初始化生成自己的maskTexture？
        PaintInMesh.instance.initializeTexture(this);
    }

    private void OnDisable()
    {
        extendRenderTexture.Release();
        uvRenderTexture.Release();
        maskRenderTexture.Release();
    }
}
