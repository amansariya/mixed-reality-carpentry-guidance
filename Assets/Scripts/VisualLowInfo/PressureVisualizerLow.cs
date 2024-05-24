using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureVisualizerLow : MonoBehaviour
{
    [SerializeField]
    private GlobCentreValuesScriptableObject sensorDataObject;
    [SerializeField]
    private GameObject handToManipulate;
    [SerializeField]
    private Gradient pressureGradientBlue;
    [SerializeField]
    private Gradient pressureGradientRed;

    private void Start()
    {
        StartCoroutine(ColorHands());
    }

    private IEnumerator ColorHands()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = handToManipulate.GetComponent<SkinnedMeshRenderer>();
        Material material = skinnedMeshRenderer.materials[0];
        Texture2D originalTexture = material.mainTexture as Texture2D;
        Texture2D newTexture;

        while (true)
        {
            newTexture = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, false);
            Graphics.CopyTexture(originalTexture, newTexture);
            newTexture.Apply();

            //float averageDifference = CalculateAverageDifference();
            List<float> differenceArray = CalculateMaxDifference();
            Color sensorColor;

            if (differenceArray[1] > 0)
            {
                sensorColor = pressureGradientBlue.Evaluate((differenceArray[0] / sensorDataObject.maxPressureValue) * 2.0f);
            }
            else if (differenceArray[1] < 0)
            {
                sensorColor = pressureGradientRed.Evaluate((differenceArray[0] / sensorDataObject.maxPressureValue) * 2.0f);
            }
            else
            {
                continue;
            }

            TextureFill(newTexture, sensorColor);

            newTexture.Apply();
            skinnedMeshRenderer.material.mainTexture = newTexture;
            yield return new WaitForSeconds(0.5f);
        }

    }

    private List<float> CalculateMaxDifference()
    {
        float maximumDifferenceMod = 0;
        float maxDifference = 0;
        float difference;
        foreach (var sensor in sensorDataObject.sensors)
        {
            difference = sensor.userPressure - sensor.idealPressure;
            if (maximumDifferenceMod < Mathf.Abs(difference))
            {
                maximumDifferenceMod = Mathf.Max(maximumDifferenceMod, Mathf.Abs(difference));
                maxDifference = difference;
            }
            
        }

        return new List<float> { maximumDifferenceMod, maxDifference };
    }

    private float CalculateAverageDifference()
    {
        float average = 0;
        foreach (var sensor in sensorDataObject.sensors)
        {
            float difference = sensor.userPressure - sensor.idealPressure;
            average += difference;
        }

        return average / sensorDataObject.sensors.Length;
    }

    private void TextureFill(Texture2D texture, Color targetColor)
    {
        Color32[] pixels = new Color32[texture.width * texture.height];

        // Fill the pixel array with the target color
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = targetColor;
        }

        texture.SetPixels32(pixels);
    }
}
