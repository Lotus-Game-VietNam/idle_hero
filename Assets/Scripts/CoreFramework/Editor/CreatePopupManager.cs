using System;
using UnityEditor;
using UnityEngine;

public static class CreateUIContext
{
    public static void CreatePrefab(string path)
    {
        GameObject newObject = GameObject.Instantiate(Resources.Load<GameObject>(path));
        Place(newObject);
    }

    public static void CreateObject(string name, params Type[] types)
    {
#if UNITY_EDITOR
        GameObject newObject = ObjectFactory.CreateGameObject(name, types);
        Place(newObject);
#endif
    }

    public static void Place(GameObject gameObject)
    {
        gameObject.name = gameObject.name.Replace("(Clone)", "");
        if (Application.isPlaying)
        {
            gameObject.transform.SetParent(null);
            gameObject.transform.SetAsLastSibling();
        }
        else
        {
#if UNITY_EDITOR
            SceneView lastView = SceneView.lastActiveSceneView;
            gameObject.transform.position = lastView ? lastView.pivot : Vector3.zero;

            UnityEditor.SceneManagement.StageUtility.PlaceGameObjectInCurrentStage(gameObject);
            GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create Object: {gameObject.name}");
            Selection.activeGameObject = gameObject;

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
#endif
        }

    }
}