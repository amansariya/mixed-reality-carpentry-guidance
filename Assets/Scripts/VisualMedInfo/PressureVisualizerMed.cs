using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureVisualizerMed : MonoBehaviour
{
    [SerializeField]
    private GlobCentreValuesScriptableObject sensorDataObject;
    [SerializeField]
    private GameObject handToManipulate;
    [SerializeField]
    private Gradient pressureGradientBlue;
    [SerializeField]
    private Gradient pressureGradientRed;
    [SerializeField]
    private float spreadFactor; // Governs how far the color needs to spread from the center point

    private Dictionary<string, List<int>> regionSensorIndices;

    private void Start()
    {
        InitializeRegionSensorIndices();
        StartCoroutine(ColorHands());
    }

    private void InitializeRegionSensorIndices()
    {
        regionSensorIndices = new Dictionary<string, List<int>>
        {
            { "thumb_region", new List<int> { 0, 1, 2, 3 } },
            { "index_region", new List<int> { 4, 5 } },
            { "middle_region", new List<int> { 7, 8 } },
            { "ring_region", new List<int> { 10, 11 } },
            { "pinky_region", new List<int> { 13, 14 } },
            { "metacarpal_region", new List<int> { 6, 9, 12, 15 } },
            { "palm_left", new List<int> { 16 } },
            { "palm_bottom", new List<int> { 17 } },
            { "palm_bottom_left", new List<int> { 18 } },
            { "palm_centre", new List<int> { 19 } }
        };
    }

    private IEnumerator ColorHands()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = handToManipulate.GetComponent<SkinnedMeshRenderer>();
        Material material = skinnedMeshRenderer.materials[0];
        Texture2D originalTexture = material.mainTexture as Texture2D;

        while (true)
        {
            Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, false);
            Graphics.CopyTexture(originalTexture, newTexture);
            newTexture.Apply();

            foreach (var region in regionSensorIndices)
            {
                string regionName = region.Key;
                List<int> sensorIndices = region.Value;
                Color regionColor;
                //float maxDifference = 0;
                //float totalDifference = 0;
                //int validSensorCount = 0;
                float maxDifferenceMod = 0;
                float maxDifference = 0;

                foreach (var sensorIndex in sensorIndices)
                {
                    if (sensorIndex < sensorDataObject.sensors.Length)
                    {
                        var sensor = sensorDataObject.sensors[sensorIndex];
                        float difference = sensor.userPressure - sensor.idealPressure;
                        if (Mathf.Abs(difference) > maxDifferenceMod)
                        {
                            maxDifferenceMod = Mathf.Abs(difference);
                            maxDifference = difference;
                        }
                    }
                }

                if (maxDifference > 0)
                {
                    regionColor = pressureGradientBlue.Evaluate((maxDifference / sensorDataObject.maxPressureValue) * 2.0f);
                }
                else
                {
                    regionColor = pressureGradientRed.Evaluate((Mathf.Abs(maxDifference) / sensorDataObject.maxPressureValue) * 2.0f);
                }

                foreach (var sensorIndex in sensorIndices)
                {
                    Vector2 regionCentre = GetRegionCentre(sensorIndex);
                    int x = Mathf.FloorToInt(regionCentre.x * newTexture.width);
                    int y = Mathf.FloorToInt(regionCentre.y * newTexture.height);
                    FloodFill(newTexture, x, y, regionColor);
                }
                
                yield return new WaitForEndOfFrame();
            }

            newTexture.Apply();
            skinnedMeshRenderer.material.mainTexture = newTexture;
            yield return new WaitForSeconds(0.5f);
        }
    }


    private Vector2 GetRegionCentre(int index)
    {
        return new Vector2(sensorDataObject.sensors[index].centreCoordinate.x, sensorDataObject.sensors[index].centreCoordinate.y);
    }

    private void FloodFill(Texture2D texture, int startX, int startY, Color targetColor)
    {
        Vector2Int origin = new(startX, startY);
        Color32[] pixels = texture.GetPixels32();
        RectInt textureDim = new(0, 0, texture.width, texture.height);

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
