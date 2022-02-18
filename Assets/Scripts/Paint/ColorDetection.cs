using System;
using System.Collections;
using System.Collections.Generic;
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
        Debug.Log(transform.position);
    }

    public void colorDetect()
    {

        RaycastHit hitInfo;

        if (Physics.Raycast(_colorDetectionPoint.position,Vector3.down, out hitInfo, 0.20f))
        { 

         //Goal:从Rendertexture中取出射线碰撞的时候所检测到的颜色
         //在maskRenderTexture上取颜色
         //将maskRenderTexture保存在一张texture2D上

        //Test how to get mask's pixel
            Material material = hitInfo.collider.GetComponent<MeshRenderer>().material;
            Texture _testtexture = material.GetTexture("_MaskTexture");

            Texture2D _testtexture2D = TextureToTexture2D(_testtexture);

            rayUVPosition = hitInfo.textureCoord;

            //将float转成int，取float的整数位
            int _UVx = Mathf.FloorToInt(rayUVPosition.x);
            int _UVy = Mathf.FloorToInt(rayUVPosition.y);

            //Debug.Log("x" + _UVx + " , y"+_UVy );



            //_color_Get = _testtexture2D.GetPixel(_UVx, _UVy);


            //Debug.Log(_color_Get);
            

           
            //if (_texture2D != null) Debug.Log(_texture2D.GetPixel(_UVx, _UVy));

            //pixelColor =  _texture2D.GetPixel(_UVx, _UVy);
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

    public Texture2D SaveRenderTexture(RenderTexture renderTexture)
    {
        //var eye = Eyes[0];
        Texture2D _textureOutput = new Texture2D(renderTexture.width,renderTexture.height,TextureFormat.RGBA32,false);
        
        //Store current RenderTexture
        RenderTexture currentRT = RenderTexture.active;
        Graphics.Blit(_textureOutput,renderTexture);

        RenderTexture.active = renderTexture;
        _textureOutput.ReadPixels(new Rect(0,0,renderTexture.width,renderTexture.height),0,0);
        
        //保存准备好的texture2D
        byte[] bytes = _textureOutput.EncodeToPNG();
        //String path = string.Format(@"/Assets/texture2D/Temp_Texture",);

        return _textureOutput;
    }

}
