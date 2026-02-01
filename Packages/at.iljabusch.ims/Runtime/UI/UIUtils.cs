using UnityEngine.UIElements;

namespace IMS.UI
{
    /// <summary>
    ///     Helpful collection of extension methods for VisualElements
    /// </summary>
    internal static class UIExtensions
    {
        internal static void MarginAll(this IStyle ve, StyleLength length)
        {
            ve.marginTop = length;
            ve.marginBottom = length;
            ve.marginLeft = length;
            ve.marginRight = length;
        }

        internal static void PaddingAll(this IStyle ve, StyleLength length)
        {
            ve.paddingTop = length;
            ve.paddingBottom = length;
            ve.paddingLeft = length;
            ve.paddingRight = length;
        }
    }
}