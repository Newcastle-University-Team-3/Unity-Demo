using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PaintInMesh : Singleton<PaintInMesh>
{
    //连接一个shader
    public Shader paintInMesh;
    public Shader extendMesh;

    private int positionID = Shader.PropertyToID("_PosW");
    private int textureID = Shader.PropertyToID("_MainTex");
    private int uvoffsetID = Shader.PropertyToID("_OffsetUV");
    private int uvIslandsID = Shader.PropertyToID("_UVIslands");  //展开后的uv

    private Material paintMaterial;
    private Material extendMaterial;

    private CommandBuffer commandBuffer;

    public override void Awake()
    {
        base.Awake();

        paintMaterial = new Material(paintMaterial);
        extendMaterial = new Material(extendMaterial);
        commandBuffer = new CommandBuffer();
        commandBuffer.name = "CommandBuffer - " + gameObject.name;
    }

    public void initializeTexture(TestPaint testPaint)
    {
        //获取搭载脚本物体的mask和uv数据，Renderer
        RenderTexture mask = testPaint.GetmaskRenderTexture();
        RenderTexture uv = testPaint.GetUVRenderTexture();
        RenderTexture extend = testPaint.GetExtendRenderTexture();
        Renderer renderer = testPaint.GetRenderer();

        //设置渲染目标
        commandBuffer.SetRenderTarget(mask);
        commandBuffer.SetRenderTarget(extend);
        commandBuffer.DrawRenderer(renderer,paintMaterial,0);

        Graphics.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();
    }

    //Paint
    public void paint(Paintable p,Vector3 position,Color color)
    {
        //获取mask
        RenderTexture mask = p.GetMask();
        RenderTexture uv = p.GetUVIslands();
        RenderTexture extend = p.GetExtend();
        Renderer renderer = p.GetRenderer();

        paintMaterial.SetVector(positionID,position);
        extendMaterial.SetFloat(uvoffsetID,p.extendIsLandOffset);
        extendMaterial.SetTexture(uvIslandsID,uv);

        //设置渲染目标
        commandBuffer.SetRenderTarget(mask);
        commandBuffer.DrawRenderer(renderer,paintMaterial,0);

        commandBuffer.SetRenderTarget(extend);
        commandBuffer.Blit(mask,extend,extendMaterial);

        Graphics.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();
    }
}
