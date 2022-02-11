using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class Grid : MonoBehaviour
{
    //水平尺寸和垂直尺寸
    public int xSize, ySize;
    private Vector3[] vertices;


    private void Awake()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0,y=0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++,i++)
            {
                vertices[i] = new Vector3(x, y);
                yield return wait;
            }
        }

        //绘制三角面
        int[] triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = xSize + 1;
        triangles[2] = 1;
        

    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i],0.1f);
        }
    }
}
