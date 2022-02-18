using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ColorDetection : MonoBehaviour
{
    public Transform _colorDetectionPoint;
    private Color _color_Get;

    private Texture2D _texture2D;

    private Vector2 rayUVPosition;
    private Color pixelColor;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        colorDetect();
    }

    public void colorDetect()
    {

        RaycastHit hitInfo;

        if (Physics.Raycast(_colorDetectionPoint.position, Vector3.down, out hitInfo, 0.20f))
        {



            //Goal:从Rendertexture中取出射线碰撞的时候所检测到的颜色
            //在maskRenderTexture上取颜色
            //将maskRenderTexture保存在一张texture2D上

            //Test how to get mask's pixel
            RenderTexture maskRenderTexture = hitInfo.collider.GetComponent<Paintable>().GetExtend();

            RenderTexture test = RenderTexture.GetTemporary(1024,1024,0);

            if (Input.GetKeyDown(KeyCode.F4))
            {
                SaveRenderTexture(maskRenderTexture);
                //Debug.Log(hitInfo.textureCoord);
                //Debug.Log(_texture2D.GetPixel((int)hitInfo.textureCoord.x,(int)hitInfo.textureCoord.y));
                Debug.Log(_texture2D.isReadable);
            }
        }
    }

    public Texture2D TextureToTexture2D(Texture texture)
    {
        //New an texture
        Texture2D texture2D = new Texture2D(texture.width,texture.height,
            TextureFormat.RGBA32,false);

        //Render texture on it
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height,32);
        Graphics.Blit(texture,renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new  Rect(0,0,renderTexture.width,renderTexture.height),0,0);
        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }


    private void SaveRenderTexture(RenderTexture rt)
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        _texture2D = png;   //keep png 
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        png.Apply();
        RenderTexture.active = active;

        //Write into file
        byte[] bytes = png.EncodeToPNG();
        string path = string.Format("Assets/texture2D/Temp_Texture/rt_{0}_{1}_{2}.png", DateTime.Now.Hour,
            DateTime.Now.Minute, DateTime.Now.Second);
        FileStream fs = File.Open(path, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(fs);
        writer.Write(bytes);
        writer.Flush();
        writer.Close();
        fs.Close();
        Destroy(png);
        png = null;
        Debug.Log("保存成功" + path);
    }

}
