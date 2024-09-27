using UnityEngine;

[CreateAssetMenu(fileName = "New Profile", menuName = "Dating Game/Profile Data")]
public class ProfileData : ScriptableObject
{
    public Sprite profilePicture;
    public string name;
    public int age;
    public string occupation;
    public string likes;
    public string dislikes;
    [TextArea(3, 10)]
    public string bio;
    public bool isImpostor;
    public int difficulty;
}