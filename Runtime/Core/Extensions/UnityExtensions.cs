using UnityEngine;

namespace UniEyeController.Core.Extensions
{
    public static class UnityExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static string GetFullPath(this Transform transform, Transform root = null)
        {
            var path = transform.name;
            var parent = transform.parent;
            while (parent != null && parent != root)
            {
                path = $"{parent.name}/{path}";
                parent = parent.parent;
            }

            return path;
        }
    }
}