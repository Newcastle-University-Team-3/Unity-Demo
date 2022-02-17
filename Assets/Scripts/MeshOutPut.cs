using UnityEngine;
using UnityEditor;
using System.Collections;

public class MeshOutPut : MonoBehaviour
{

    public string objectName;
    public Transform obj;

    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            SaveAsset();
        }
    }

    void SaveAsset()
    {
        Mesh m1 = obj.GetComponent<MeshFilter>().mesh;
        AssetDatabase.CreateAsset(m1, "Assets/" + objectName + ".asset");
    }

}