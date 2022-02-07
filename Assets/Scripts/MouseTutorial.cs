using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTutorial : MonoBehaviour
{
    //保存顶点坐标
    private Vector3[] vertices;

    //保存顺序
    private int[] triangles;
    //记录顶点数
    private int count = 0;
    //定义mesh
    private Mesh mesh;
    //链表记录所有点的坐标
    private List<Vector3> list;

    void Start()
    {
        //mesh的相关组件
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        list = new List<Vector3>();

        mesh = GetComponent<MeshFilter>().mesh;

        GetComponent<MeshRenderer>().material.color = Color.green;

        GetComponent<MeshRenderer>().material.shader = Shader.Find("Transparent/Diffuse");

        mesh.Clear();
    }

    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            count++;
            //把鼠标坐标转换成世界坐标，然后加入list之中。
            list.Add(Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x,Input.mousePosition.y,0.8f)));
        }

        if (count>=3)
        {
            triangles = new int[3 * (count - 2)];
            vertices = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                vertices[i] = list[i];
            }

            int triangle_count = count - 2;

            for (int i = 0; i < triangle_count; i++)
            {
                triangles[3 * i] = 0;
                triangles[3 * i + 1] = i+2;
                triangles[3 * i + 2] = i+1;
            }

            mesh.vertices = vertices;//设置顶点坐标
            mesh.triangles = triangles;//设置顶点索引
        }
    }
}
