using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
    public static bool Level1LoadedExternally { get; set; }
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // in Unity Editor, when Level_1 is loaded from MainMenu in Play Mode,
        // the next Level_1 Play Mode will start with {Level1LoadedExternally = true}
        // even without external load until the next Scripts Recompile
        Level1LoadedExternally = false;
    }
}
