using IMS;

namespace Demo.Items
{
    public class Tomato : IItem
    {
        public string GetName()
        {
            return "Tomato";
        }

        public int GetMaxQuantity()
        {
            return 10;
        }
    }
}