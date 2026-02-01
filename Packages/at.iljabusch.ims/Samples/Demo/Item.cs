using UnityEngine;

namespace IMS.Demo
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects", order = 0)]
    public class Item : ScriptableObject, IItem
    {
        [SerializeField] private string label;
        [SerializeField] private int maxStackSize;
        [SerializeField] private Sprite sprite;

        public string GetName()
        {
            return label;
        }

        public int GetMaxQuantity()
        {
            return maxStackSize;
        }

        public Sprite GetSprite()
        {
            return sprite;
        }
    }
}