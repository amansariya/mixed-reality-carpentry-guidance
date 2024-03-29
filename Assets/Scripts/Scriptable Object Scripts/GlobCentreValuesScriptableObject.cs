using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GlobCentreValuesScriptableObject", order = 1)]
public class GlobCentreValuesScriptableObject : ScriptableObject
{
    [System.Serializable]
    public struct GlobCentreValues
    {
        public string sensorLocation;
        public Vector2 centreCoordinate;
        public float idealPressure;
        public float userPressure;
    }

    [SerializeField]
    public GlobCentreValues[] sensors;

    [SerializeField]
    public float maxPressureValue;
}