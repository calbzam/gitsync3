#if UNITY_EDITOR
using System.Reflection;

public static class ClearConsole
{
    // https://www.youtube.com/watch?v=MrS7tHMuBHk
    public static void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
#endif
