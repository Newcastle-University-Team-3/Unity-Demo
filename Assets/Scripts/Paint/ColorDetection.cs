using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class ColorDetection : MonoBehaviour
{
    public Transform _colorDetectionPoint;
    private Texture2D _texture2D;
    public Color[] pixelColor;

    private string imagePath;

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
            
            RenderTexture maskRenderTexture = hitInfo.collider.GetComponent<Paintable>().GetMask();

            RenderTexture test = RenderTexture.GetTemporary(1024,1024,0);

            int uvX = Mathf.FloorToInt(hitInfo.textureCoord.x);
            int uvY = Mathf.FloorToInt(hitInfo.textureCoord.y);


            if (Input.GetKeyDown(KeyCode.F4))
            {
                SaveRenderTexture(maskRenderTexture);
                //pixelColor = _texture2D.GetPixels(1020, 1020, 2, 2);
                //for (int i = 0; i < pixelColor.Length; i++)
                //{
                //    Debug.Log(pixelColor[i]);
                //}

                FileStream fs = new FileStream("Assets/texture2D/Temp_Texture/rt.JPG", FileMode.Open, FileAccess.Read);
                int byteLength = (int)fs.Length;
                byte[] imgBytes = new byte[byteLength];
                fs.Read(imgBytes, 0, byteLength);
                fs.Close();
                fs.Dispose();

                Texture2D t2d = new Texture2D(1024,1024); ;
                t2d.LoadImage(imgBytes);
                t2d.Apply();
                pixelColor = t2d.GetPixels(1000, 1000, 16, 16);
                for (int i = 0; i < pixelColor.Length; i++)
                {
                    Debug.Log(pixelColor[i]);
                }
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
        Texture2D texture2D = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        
        texture2D.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture2D.Apply();
        _texture2D = texture2D;   //keep png 
        RenderTexture.active = active;

        //Write into file
        byte[] bytes = texture2D.EncodeToJPG();
        string imagePath = string.Format("Assets/texture2D/Temp_Texture/rt.JPG");
        FileStream fs = File.Open(imagePath , FileMode.Create);
        BinaryWriter writer = new BinaryWriter(fs);
        writer.Write(bytes);
        writer.Flush();
        writer.Close();
        fs.Close();

        Destroy(texture2D);
        texture2D = null;
        Debug.Log("保存成功" + imagePath);
    }

}
