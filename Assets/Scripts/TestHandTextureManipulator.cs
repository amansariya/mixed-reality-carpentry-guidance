using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHandTextureManipulator : MonoBehaviour
{
    [SerializeField] private GameObject handToManipulate;

    // Start is called before the first frame update
    void Start()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = handToManipulate.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            Mesh mesh = new Mesh();
            skinnedMeshRenderer.BakeMesh(mesh);
            Vector2[] uv = mesh.uv;
            for (int i = 0; i < uv.Length; i++)
            {
                Debug.Log("UV[" + i + "] = " + uv[i]);
            }
        }
        else
        {
            Debug.LogError("SkinnedMeshRenderer not found on the GameObject.");
        }
    }
}
