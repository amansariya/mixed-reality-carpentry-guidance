using System.Collections.Generic;
using UnityEngine;

public class PressureVisualizer : MonoBehaviour
{
    [SerializeField]
    private GlobCentreValuesScriptableObject sensorDataObject;
    [SerializeField]
    private GameObject handToManipulate;
    //public Texture2D handTexture;
    [SerializeField]
    private float maxPressure; // The maximum value of pressure
    [SerializeField]
    private float diffusionRate; // Rate at which the color diffuses
    [SerializeField]
    private float threshold; // Threshold for stopping the diffusion
    [SerializeField]
    private Gradient pressureGradient;

    private void Start()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = handToManipulate.GetComponent<SkinnedMeshRenderer>();
        Material material = skinnedMeshRenderer.materials[0];
        Texture2D originalTexture = material.mainTexture as Texture2D;
        Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, false);
        Graphics.CopyTexture(originalTexture, newTexture);
        newTexture.Apply();
      

        // Assuming handTexture is a cloned texture that can be written to
        foreach (var sensor in sensorDataObject.sensors)
        {
            // Convert UV coordinates to pixel coordinates
            int x = Mathf.FloorToInt(sensor.centreCoordinate.x * newTexture.width);
            int y = Mathf.FloorToInt(sensor.centreCoordinate.y * newTexture.height);

            //float randomPressure = Random.Range(0f, maxPressure);

            Color sensorColor = pressureGradient.Evaluate(sensor.pressure / maxPressure);


            // Start the flood fill process at this location
            //Color targetColor = Color.red * (randomPressure / maxPressure); // Adjust color based on pressure
            
            //for (int i = x - 50; i <= x + 50; i++)
            //{
            //    for (int j = y - 50; j <= y + 50; j++)
            //    {
            //        newTexture.SetPixel(i, j, targetColor);
            //    }
            //}

            FloodFill(newTexture, x, y, sensorColor, threshold, diffusionRate);
        }

        newTexture.Apply();
        skinnedMeshRenderer.material.mainTexture = newTexture;
    }

    //private void FloodFill(Texture2D texture, int x, int y, Color color, float stopThreshold, float rate)
    //{
    //    // Recursively apply color to the starting pixel and diffuse outwards
    //    Color pixelColor = texture.GetPixel(x, y);
    //    if (pixelColor != color && ColorDifference(pixelColor, color) > stopThreshold)
    //    {
    //        texture.SetPixel(x, y, Color.Lerp(pixelColor, color, rate));

    //        // Recursive diffusion to neighboring pixels
    //        if (x + 1 < texture.width) FloodFill(texture, x + 1, y, color * rate, stopThreshold, rate);
    //        if (x - 1 >= 0) FloodFill(texture, x - 1, y, color * rate, stopThreshold, rate);
    //        if (y + 1 < texture.height) FloodFill(texture, x, y + 1, color * rate, stopThreshold, rate);
    //        if (y - 1 >= 0) FloodFill(texture, x, y - 1, color * rate, stopThreshold, rate);
    //    }
    //}

    private void FloodFill(Texture2D texture, int startX, int startY, Color targetColor, float stopThreshold, float rate)
    {
        // Use GetPixels once to grab all the pixels
        Color[] pixels = texture.GetPixels();
        int textureWidth = texture.width;
        int textureHeight = texture.height;

        Queue<Vector2Int> pixelQueue = new Queue<Vector2Int>();
        pixelQueue.Enqueue(new Vector2Int(startX, startY));

        while (pixelQueue.Count > 0)
        {
            Vector2Int pixel = pixelQueue.Dequeue();
            int x = pixel.x;
            int y = pixel.y;
            int index = x + y * textureWidth;

            // Use the index to access the Color array
            Color pixelColor = pixels[index];
            if (ColorDifference(pixelColor, targetColor) > stopThreshold)
            {
                pixels[index] = Color.Lerp(pixelColor, targetColor, rate);

                // Add surrounding pixels to the queue
                EnqueueIfValid(pixels, pixelQueue, x + 1, y, textureWidth, textureHeight, targetColor, stopThreshold);
                EnqueueIfValid(pixels, pixelQueue, x - 1, y, textureWidth, textureHeight, targetColor, stopThreshold);
                EnqueueIfValid(pixels, pixelQueue, x, y + 1, textureWidth, textureHeight, targetColor, stopThreshold);
                EnqueueIfValid(pixels, pixelQueue, x, y - 1, textureWidth, textureHeight, targetColor, stopThreshold);
            }
        }

        // Apply all the changes at once
        texture.SetPixels(pixels);
    }

    private void EnqueueIfValid(Color[] pixels, Queue<Vector2Int> pixelQueue, int x, int y, int textureWidth, int textureHeight, Color targetColor, float stopThreshold)
    {
        if (x >= 0 && x < textureWidth && y >= 0 && y < textureHeight)
        {
            int index = x + y * textureWidth;
            Color pixelColor = pixels[index];
            if (ColorDifference(pixelColor, targetColor) > stopThreshold)
            {
                pixelQueue.Enqueue(new Vector2Int(x, y));
            }
        }
    }

    private float ColorDifference(Color a, Color b)
    {
        // Simple color difference calculation
        return Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b);
    }
}
