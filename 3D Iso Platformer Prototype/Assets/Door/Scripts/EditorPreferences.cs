using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EditorPreferences : MonoBehaviour
{
    public bool ShouldAlwaysRefreshSceneViews = true;
#if UNITY_EDITOR
    public void Update()
    {
        foreach (SceneView view in SceneView.sceneViews)
        {
            view.sceneViewState.alwaysRefresh = ShouldAlwaysRefreshSceneViews;
        }
    }
#endif
}