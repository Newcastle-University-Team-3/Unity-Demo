using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTQUAD : MonoBehaviour
{
    [HideInInspector] public Texture2D mainTexture2D;
    [HideInInspector] private int mainTextureID;

    private Material material;
    private void Start()
    {
        mainTexture2D = PaintManager.instance.maskSaved;
        mainTextureID = Shader.PropertyToID("_MainTex");
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        BaseMethod();
    }

    public void BaseMethod()
    {
        material.SetTexture(mainTextureID,mainTexture2D);
    }
}
