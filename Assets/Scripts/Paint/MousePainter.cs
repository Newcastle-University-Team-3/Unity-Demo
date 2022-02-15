using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;

public class MousePainter : MonoBehaviour
{
    public Camera cam;
    [Space]
    public bool mouseSingleClick;
    public bool test;
    [Space]
    public Color paintColor;

    public float radius = 1;
    public float strength = 1;
    public float hardness = 1;

    public Shader paintInMesh;
    public Material _paintInMeshMaterial;
    public Renderer meshRenderer;

    //private RenderTexture maskRenderTexture;
    //private CommandBuffer command;

    private int _PosW = Shader.PropertyToID("_PosW");

    private void Start()
    {
        _paintInMeshMaterial = new Material(paintInMesh);
        //maskRenderTexture = new RenderTexture(1024, 1024, 0);
    }

    void Update()
    {

        bool click;
        click = mouseSingleClick ? Input.GetMouseButtonDown(0) : Input.GetMouseButton(0);

        if (click)
        {
            Vector3 position = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                transform.position = hit.point;
                Paintable p = hit.collider.GetComponent<Paintable>();
                Debug.Log(hit.collider.tag);
                //if (p != null)
                //{
                //   PaintManager.instance.paint(p, hit.point, radius, hardness, strength, paintColor);
                //}

                if (test)
                {
                    _paintInMeshMaterial.SetVector(_PosW,hit.point);
                    meshRenderer = hit.collider.GetComponent<MeshRenderer>();

                    if (meshRenderer == null)
                    {
                        Debug.Log("Renderer does not exist");
                        return;
                    }

                    //command.SetRenderTarget(maskRenderTexture);
                    //command.DrawRenderer(meshRenderer,_paintInMeshMaterial,0);
                    //Debug.Log(1);
                    //Graphics.ExecuteCommandBuffer(command);
                    //command.Clear();
                }
            }
        }
    }

    //private void OnDisable()
    //{
    //    maskRenderTexture.Release();
    //}
}
