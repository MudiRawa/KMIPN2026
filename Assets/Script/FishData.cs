using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Fishing/Fish Data")]
public class FishData : ScriptableObject
{
public string fishID;

public string fishName;

[TextArea]
public string description;

public Sprite icon;

public int sellPrice;

public bool isRare;

}
