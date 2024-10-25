using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/IntCount", order = 1)]
public class IntCount : ScriptableObject
{
    public int value;
    private int startValue;
    private void OnEnable()
    {
        startValue = value;
    }
    public void ResetValue()
    {
        value = startValue;
    }
    public void SetValue(int targetValue)
    {
        value = targetValue;
    }
    public void ChangeValue(int valueChange)
    {
        value += valueChange;
    }
}