using System.Collections;
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

        while(true)
        {
            Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, false);
            Graphics.CopyTexture(originalTexture, newTexture);
            newTexture.Apply();

            float averageDifference = CalculateAverageDifference();
            Color sensorColor;

            if (averageDifference > 0)
            {
                sensorColor = pressureGradientBlue.Evaluate(averageDifference / sensorDataObject.maxPressureValue);
            }
            else if (averageDifference < 0)
            {
                //Debug.Log("difference enter");
                sensorColor = pressureGradientRed.Evaluate(Mathf.Abs(averageDifference) / sensorDataObject.maxPressureValue);
            }
            else
            {
                continue;
            }

            TextureFill(newTexture, sensorColor);

            newTexture.Apply();
            skinnedMeshRenderer.material.mainTexture = newTexture;
            yield return new WaitForSeconds(1);
        }
        
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
