using UnityEngine;


[CreateAssetMenu(fileName = "Team", menuName = "ScriptableObjects/Team")]
public class Team : ScriptableObject
{
    public string teamName;
    public Color color;
    public Material claimedMaterial;
    public Material gasMaterial;
    public GameObject particle;
}
