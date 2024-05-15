using UnityEngine;

public class ColorFingerTipSkinned : MonoBehaviour
{
    public Vector2 fingerTipUV = new Vector2(0.29f, 0.35f); // Example UV coordinates
    public Color targetColor = Color.red;
    [SerializeField] private GameObject handToManipulate;

    void Start()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = handToManipulate.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer component not found on the object.");
            return;
        }

        Material material = skinnedMeshRenderer.materials[0]; // Use material if you don't care about affecting other objects that use the same material, otherwise use skinnedMeshRenderer.sharedMaterial
        if (material == null)
        {
            Debug.LogError("Material not found on the SkinnedMeshRenderer.");
            return;
        }
        else
        {
            Debug.LogWarning(material);
        }

        Texture2D originalTexture = material.mainTexture as Texture2D;
        if (originalTexture == null)
        {
            Debug.LogError("No main texture found on the material.");
            return;
        }

        // Duplicate the texture
        Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, false);
        Graphics.CopyTexture(originalTexture, newTexture);
        newTexture.Apply();

        // Convert the UV coordinates to pixel coordinates on the texture
        int x = Mathf.FloorToInt(fingerTipUV.x * newTexture.width);
        int y = Mathf.FloorToInt(fingerTipUV.y * newTexture.height);
        Debug.Log(newTexture.width + " " + newTexture.height + " " + x + " " + y);

        newTexture.SetPixel(x, y, targetColor);

        //color a specific area around the uv coordinates
        for (int i = x - 50; i <= x + 50; i++)
        {
            for (int j = y - 50; j <= y + 50; j++)
            {
                newTexture.SetPixel(i, j, targetColor);
            }
        }

        newTexture.Apply();

        // Apply the modified texture to the skinned mesh renderer
        skinnedMeshRenderer.material.mainTexture = newTexture;
    }
}
