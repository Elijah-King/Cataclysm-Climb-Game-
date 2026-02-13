using UnityEngine;

public class EndLevelScript : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;

    public int GetSceneToLoad()
    {
        return sceneToLoad;
    }
}
