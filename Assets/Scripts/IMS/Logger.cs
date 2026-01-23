using UnityEngine;

namespace IMS
{
    internal class Logger
    {
        private readonly string _tag;

        internal Logger(string tag)
        {
            _tag = tag;
        }

        internal void Info(object message)
        {
            Debug.Log($"<color=lightblue>[{_tag}] " + message + "</color>");
        }

        internal void Warn(object message)
        {
            Debug.LogWarning($"[{_tag}] " + message);
        }
    }
}