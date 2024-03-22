using System.Collections.Generic;
using UnityEngine;

public class PressureVisualizer : MonoBehaviour
{
    [SerializeField]
    private GlobCentreValuesScriptableObject sensorDataObject;
    [SerializeField]
    private GameObject handToManipulate;
    [SerializeField]
    private float maxPressure; // The maximum value of pressure
    [SerializeField]
    private Gradient pressureGradient;
    [SerializeField]
    private float spreadFactor; // Governs how far the color needs to spread from the center point

    private void Start()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = handToManipulate.GetComponent<SkinnedMeshRenderer>();
        Material material = skinnedMeshRenderer.materials[0];
        Texture2D originalTexture = material.mainTexture as Texture2D;
        Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, false);
        Graphics.CopyTexture(originalTexture, newTexture);
        newTexture.Apply();

        foreach (var sensor in sensorDataObject.sensors)
        {
            // Convert UV coordinates to pixel coordinates
            int x = Mathf.FloorToInt(sensor.centreCoordinate.x * newTexture.width);
            int y = Mathf.FloorToInt(sensor.centreCoordinate.y * newTexture.height);

            Color sensorColor = pressureGradient.Evaluate(sensor.userPressure / maxPressure);

            FloodFill(newTexture, x, y, sensorColor);
        }

        newTexture.Apply();
        skinnedMeshRenderer.material.mainTexture = newTexture;
    }
    private void FloodFill(Texture2D texture, int startX, int startY, Color targetColor)
    {
        Vector2Int origin = new(startX, startY);
        Color32[] pixels = texture.GetPixels32();
        RectInt textureDim = new (0, 0, texture.width, texture.height);

        Queue<Vector2Int> pixelQueue = new();
        HashSet<Vector2Int> visited = new(); // To track visited pixels
        pixelQueue.Enqueue(origin);
        visited.Add(origin); // Mark as visited

        while (pixelQueue.Count > 0)
        {
            Vector2Int pixel = pixelQueue.Dequeue();
            float distanceFromCenter = Vector2.Distance(pixel, origin);
            float rate = (distanceFromCenter / spreadFactor);

            if (rate <= 1)
            {
                int index = pixel.x + pixel.y * textureDim.width;
                Color pixelColor = pixels[index];
                pixels[index] = Color.Lerp(targetColor, pixelColor, rate);

                EnqueueIfValid(pixelQueue, visited, pixel + Vector2Int.right, textureDim);
                EnqueueIfValid(pixelQueue, visited, pixel + Vector2Int.left, textureDim);
                EnqueueIfValid(pixelQueue, visited, pixel + Vector2Int.up, textureDim);
                EnqueueIfValid(pixelQueue, visited, pixel + Vector2Int.down, textureDim);
            }
        }

        texture.SetPixels32(pixels);
    }

    private void EnqueueIfValid(Queue<Vector2Int> pixelQueue, HashSet<Vector2Int> visited, Vector2Int point, RectInt textureDim)
    {
        if (textureDim.Contains(point) && !visited.Contains(point))
        {
            pixelQueue.Enqueue(point);
            visited.Add(point); // Mark as visited
        }
    }
}
