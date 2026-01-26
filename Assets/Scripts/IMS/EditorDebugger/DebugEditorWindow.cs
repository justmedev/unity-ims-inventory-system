using System.Collections;
using System.Reflection;
using IMS.Exceptions;
using IMS.UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace IMS.EditorDebugger
{
    /// <summary>
    ///     A Unity editor window to debug Inventory instances. Uses reflection to search for a variable of type
    ///     `Inventory` and track its changes.
    /// </summary>
    /// <example>
    /// MyComponent.cs:
    /// <code>
    /// public class MyComponent {
    ///     private Inventory _inventory;
    ///
    ///     private void Start() {
    ///         _inventory = new Inventory(...);
    ///     }
    /// }
    /// </code>
    /// </example>
    internal class InventoryDebugger : EditorWindow
    {
        private Inventory _targetInventory;
        private ListView _listView;
        private Label _statusLabel;
        private ObjectField _gameObjectField;

        [MenuItem("Window/IMS/Inventory Debugger")]
        private static void ShowWindow()
        {
            GetWindow<InventoryDebugger>("Inventory Debugger");
        }

        private void CreateGUI()
        {
            // Root Container
            var root = rootVisualElement;
            root.style.PaddingAll(10);

            // Header
            var windowTitle = new Label("IMS Inventory Debugger")
            {
                style = { fontSize = 18, unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 10 }
            };
            root.Add(windowTitle);

            // Selection Area
            _gameObjectField = new ObjectField("Target GameObject")
            {
                objectType = typeof(GameObject),
                allowSceneObjects = true
            };
            root.Add(_gameObjectField);

            var findButton = new Button(FindInventoryInTarget) { text = "Attach to Inventory" };
            root.Add(findButton);

            _statusLabel = new Label("Status: No Inventory Attached")
            {
                style = { marginTop = 5, marginBottom = 10, color = Color.gray }
            };
            root.Add(_statusLabel);

            // List View for Slots
            _listView = new ListView
            {
                makeItem = MakeSlotItem,
                bindItem = BindSlotItem,
                fixedItemHeight = 45,
                showBoundCollectionSize = true,
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                style = { flexGrow = 1, borderTopWidth = 1, borderTopColor = Color.gray, marginTop = 10 }
            };

            root.Add(_listView);

            // Refresh timer (to catch external changes)
            root.schedule.Execute(() => _listView.RefreshItems()).Every(500);
        }

        private void FindInventoryInTarget()
        {
            if (_gameObjectField.value is GameObject go)
            {
                // We use reflection to find any field or property of type Inventory 
                // on any component attached to the GameObject.
                var components = go.GetComponents<MonoBehaviour>();
                foreach (var comp in components)
                {
                    if (comp == null) continue;

                    var fields = comp.GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var f in fields)
                    {
                        if (f.FieldType == typeof(Inventory))
                        {
                            _targetInventory = (Inventory)f.GetValue(comp);
                            break;
                        }
                    }

                    if (_targetInventory != null) break;
                }

                if (_targetInventory != null)
                {
                    _statusLabel.text = $"Status: Connected to '{_targetInventory.Name}'";
                    _statusLabel.style.color = Color.green;
                    _listView.itemsSource = (IList)_targetInventory.Slots;
                    _listView.Rebuild();
                }
                else
                {
                    _statusLabel.text = "Status: Could not find Inventory field in GameObject components.";
                    _statusLabel.style.color = Color.red;
                }
            }
        }

        private static VisualElement MakeSlotItem()
        {
            var container = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row, alignItems = Align.Center, justifyContent = Justify.SpaceBetween,
                    paddingRight = 5
                }
            };

            var labelContainer = new VisualElement { style = { flexDirection = FlexDirection.Column } };
            var indexLabel = new Label { name = "slot-index", style = { fontSize = 10, color = Color.gray } };
            var contentLabel = new Label
                { name = "slot-content", style = { unityFontStyleAndWeight = FontStyle.Bold } };
            labelContainer.Add(indexLabel);
            labelContainer.Add(contentLabel);

            var deleteButton = new Button
            {
                name = "delete-btn", text = "Delete",
                style = { backgroundColor = new Color(0.4f, 0.1f, 0.1f), color = Color.white }
            };

            container.Add(labelContainer);
            container.Add(deleteButton);
            return container;
        }

        private void BindSlotItem(VisualElement ve, int i)
        {
            if (_targetInventory == null) return;

            var slot = _targetInventory.Slots[i];
            var indexLabel = ve.Q<Label>("slot-index");
            var contentLabel = ve.Q<Label>("slot-content");
            var deleteBtn = ve.Q<Button>("delete-btn");

            indexLabel.text = $"Slot {slot.Index}";

            if (slot.IsEmpty)
            {
                contentLabel.text = "Empty";
                contentLabel.style.color = Color.gray;
                deleteBtn.SetEnabled(false);
            }
            else
            {
                var stack = slot.GetItemStack();
                contentLabel.text = $"{stack.Item.GetName()} (x{stack.Quantity})";
                contentLabel.style.color = Color.white;
                deleteBtn.SetEnabled(true);

                deleteBtn.clicked += () =>
                {
                    try
                    {
                        slot.RemoveItemStack();
                        _targetInventory.PropagateChange(slot.Index);
                        _listView.RefreshItems();
                    }
                    catch (InventorySlotEmptyException)
                    {
                    }
                };
            }
        }
    }
}