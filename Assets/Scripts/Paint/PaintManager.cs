using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
public class PaintManager : Singleton<PaintManager>
{
    public Shader texturePaint; //Paint with texture
    public Shader extendIslands;    //Unfold texture and Get UVs that located on it

    private int prepareUVID = Shader.PropertyToID("_PrepareUV");
    private int positionID = Shader.PropertyToID("_PainterPosition");
    private int radiusID = Shader.PropertyToID("_Radius");
    private int strengthID = Shader.PropertyToID("_Strength");
    private int hardnessID = Shader.PropertyToID("_Hardness");
    private int textureID = Shader.PropertyToID("_MainTex");
    private int colorID = Shader.PropertyToID("_PainterColor");

    ////?
    private int uvOffsetID = Shader.PropertyToID("_OffsetUV");
    private int uvIslandsID = Shader.PropertyToID("_UVIsLands");

    private Material paintMaterial;
    private Material extendMaterial;

    private CommandBuffer commandBuffer;

    //Save RenderTexture:Mask
    [HideInInspector]
    public Texture2D maskSaved;

    public override void Awake()
    {
        //base.Awake();
        paintMaterial = new Material(texturePaint);
        extendMaterial = new Material(extendIslands);

        commandBuffer = new CommandBuffer();
        commandBuffer.name = "CommandBuffer - " + gameObject.name;

        maskSaved = new Texture2D(1024,1024);
    }

    public void initTextures(Paintable paintable)
    {
        RenderTexture mask = paintable.GetMask();
        RenderTexture uvIsland = paintable.GetUVIslands();
        RenderTexture extend = paintable.GetExtend();
        RenderTexture support = paintable.GetSupport();

        Renderer rend = paintable.GetRenderer();

        commandBuffer.SetRenderTarget(mask);
        commandBuffer.SetRenderTarget(extend);
        commandBuffer.SetRenderTarget(support);

        paintMaterial.SetFloat(prepareUVID,1);
        commandBuffer.SetRenderTarget(uvIsland);
        commandBuffer.DrawRenderer(rend,paintMaterial,0);

        Graphics.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();

    }

    public void paint(Paintable paintable, Vector3 pos, float radius =1.0f, float hardness = 0.5f, float strength = 0.5f,
        Color? color = null)
    {
        RenderTexture mask = paintable.GetMask();
        RenderTexture uvIslands = paintable.GetUVIslands();    
        RenderTexture extend = paintable.GetExtend();       
        RenderTexture support = paintable.GetSupport();

        Renderer renderer = paintable.GetRenderer();

        paintMaterial.SetFloat(prepareUVID,0);
        paintMaterial.SetVector(positionID,pos);
        paintMaterial.SetFloat(radiusID,radius);
        paintMaterial.SetFloat(strengthID,strength);
        paintMaterial.SetFloat(hardnessID,hardness);
        paintMaterial.SetTexture(textureID,support);
        paintMaterial.SetColor(colorID,color??Color.red);
        
        ////?
        extendMaterial.SetFloat(uvOffsetID,paintable.extendIsLandOffset);
        extendMaterial.SetTexture(uvIslandsID,uvIslands);


        //Start render
        commandBuffer.SetRenderTarget(mask);
        commandBuffer.DrawRenderer(renderer,paintMaterial,0);

        commandBuffer.SetRenderTarget(support);
        commandBuffer.Blit(mask,support);

        commandBuffer.SetRenderTarget(extend);
        commandBuffer.Blit(mask,extend,extendMaterial);

        //Do Render
        Graphics.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();

        //Save mask
        //RenderTexture active = RenderTexture.active;
        
        //maskSaved.ReadPixels(new Rect(0,0,RenderTexture.active.width,RenderTexture.active.height),0,0);
    }

    //private void SaveRenderTexture(RenderTexture rt)
    //{
    //    RenderTexture active = RenderTexture.active;
    //    RenderTexture.active = rt;  
    //    Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
    //    png.ReadPixels(new Rect(0,0,rt.width,rt.height),0,0);
    //    png.Apply();
    //    RenderTexture.active = active;
        
    //    //Write into file
    //    byte[] bytes = png.EncodeToPNG();
    //    string path = string.Format("Assets/texture2D/Temp_Texture/rt_{0}_{1}_{2}.png", DateTime.Now.Hour,
    //        DateTime.Now.Minute, DateTime.Now.Second);
    //    FileStream fs = File.Open(path, FileMode.Create);
    //    BinaryWriter writer = new BinaryWriter(fs);
    //    writer.Write(bytes);
    //    writer.Flush();
    //    writer.Close();
    //    fs.Close();
    //    Destroy(png);
    //    png = null;
    //    Debug.Log("保存成功" + path);
    //}

}
