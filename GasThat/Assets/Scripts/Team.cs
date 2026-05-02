using UnityEngine;


[CreateAssetMenu(fileName = "Team", menuName = "ScriptableObjects/Team")]
public class Team : ScriptableObject
{
    public string teamName;
    public Color color;
}
