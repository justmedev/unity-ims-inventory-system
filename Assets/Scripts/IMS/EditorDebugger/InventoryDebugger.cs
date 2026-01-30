#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
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
    ///     A Unity editor window to debug Inventory instances. Uses reflection to search for all variables of type
    ///     <c>Inventory</c> on a GameObject and allows switching between them via tabs.
    /// </summary>
    /// <example>
    ///     MyComponent.cs:
    ///     <code>
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
        private readonly List<Inventory> _detectedInventories = new();
        private ObjectField _gameObjectField;
        private VisualElement _listContainer;

        private ListView _listView;
        private Inventory _selectedInventory;
        private Label _statusLabel;
        private VisualElement _tabHeader;

        private void CreateGUI()
        {
            var root = rootVisualElement;
            root.style.PaddingAll(10);

            // Header
            var windowTitle = new Label("IMS Inventory Debugger")
            {
                style = { fontSize = 18, unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 10 }
            };
            root.Add(windowTitle);

            // Selection Area
            var selectionRow = new VisualElement { style = { flexDirection = FlexDirection.Row, marginBottom = 5 } };
            _gameObjectField = new ObjectField("Target GameObject")
            {
                objectType = typeof(GameObject),
                allowSceneObjects = true,
                style = { flexGrow = 1 }
            };
            selectionRow.Add(_gameObjectField);

            var findButton = new Button(FindInventoriesInTarget) { text = "Scan & Attach" };
            selectionRow.Add(findButton);
            root.Add(selectionRow);

            _statusLabel = new Label("Status: No Inventory Attached")
            {
                style = { marginBottom = 10, color = Color.gray }
            };
            root.Add(_statusLabel);

            // Tab Header Container
            _tabHeader = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexWrap = Wrap.Wrap,
                    borderBottomWidth = 1,
                    borderBottomColor = Color.gray,
                    marginBottom = 5
                }
            };
            root.Add(_tabHeader);

            // List View Container (hidden until an inventory is selected)
            _listContainer = new VisualElement { style = { flexGrow = 1, display = DisplayStyle.None } };
            _listView = new ListView
            {
                makeItem = MakeSlotItem,
                bindItem = BindSlotItem,
                fixedItemHeight = 45,
                showBoundCollectionSize = true,
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                style = { flexGrow = 1 }
            };
            _listContainer.Add(_listView);
            root.Add(_listContainer);

            // Refresh timer to catch changes in quantity or content during playmode
            root.schedule.Execute(() => _listView.RefreshItems()).Every(500);
        }

        [MenuItem("Window/IMS/Inventory Debugger")]
        private static void ShowWindow()
        {
            GetWindow<InventoryDebugger>("Inventory Debugger");
        }

        private void FindInventoriesInTarget()
        {
            _detectedInventories.Clear();
            _tabHeader.Clear();
            _selectedInventory = null;
            _listContainer.style.display = DisplayStyle.None;

            if (_gameObjectField.value is GameObject go)
            {
                var components = go.GetComponents<MonoBehaviour>();
                foreach (var comp in components)
                {
                    if (comp == null) continue;

                    var type = comp.GetType();

                    // Find Fields
                    var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var f in fields)
                    {
                        if (f.FieldType == typeof(Inventory) && f.GetValue(comp) is Inventory inv)
                            RegisterInventory(inv);
                    }

                    // Find Properties
                    var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                   BindingFlags.Instance);
                    foreach (var p in props)
                    {
                        if (p.PropertyType == typeof(Inventory) && p.GetValue(comp) is Inventory inv)
                            RegisterInventory(inv);
                    }
                }

                if (_detectedInventories.Count > 0)
                {
                    _statusLabel.text = $"Status: Found {_detectedInventories.Count} inventories.";
                    _statusLabel.style.color = Color.green;
                    SelectInventory(_detectedInventories[0]);
                }
                else
                {
                    _statusLabel.text = "Status: No Inventory instances found on this GameObject.";
                    _statusLabel.style.color = Color.red;
                }
            }
        }

        private void RegisterInventory(Inventory inv)
        {
            if (_detectedInventories.Contains(inv)) return;
            _detectedInventories.Add(inv);

            var tabBtn = new Button(() => SelectInventory(inv))
            {
                text = (string.IsNullOrEmpty(inv.Name) ? $"Inventory {_detectedInventories.Count - 1}" : inv.Name) +
                       $"[{inv.Id}]",
                style =
                {
                    borderTopLeftRadius = 4, borderTopRightRadius = 4,
                    borderBottomLeftRadius = 0, borderBottomRightRadius = 0,
                    marginRight = 2, paddingLeft = 8, paddingRight = 8
                }
            };
            _tabHeader.Add(tabBtn);
        }

        private void SelectInventory(Inventory inv)
        {
            _selectedInventory = inv;
            _listContainer.style.display = DisplayStyle.Flex;
            _listView.itemsSource = (IList)inv.Slots;
            _listView.Rebuild();

            // Highlight active tab button
            foreach (var child in _tabHeader.Children())
            {
                if (child is Button b)
                {
                    var isActive = b.text == inv.Name;
                    b.style.backgroundColor =
                        isActive ? new Color(0.35f, 0.35f, 0.35f) : new Color(0.22f, 0.22f, 0.22f);
                    b.style.borderBottomWidth = isActive ? 2 : 0;
                    b.style.borderBottomColor = Color.cyan;
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
            if (_selectedInventory == null) return;

            var slot = _selectedInventory.Slots[i];
            var indexLabel = ve.Q<Label>("slot-index");
            var contentLabel = ve.Q<Label>("slot-content");
            var deleteBtn = ve.Q<Button>("delete-btn");

            indexLabel.text = $"Slot {slot.Index}";

            if (slot.IsEmpty)
            {
                contentLabel.text = "Empty";
                contentLabel.style.color = Color.gray;
                deleteBtn.SetEnabled(false);
                // Clear any previous callbacks for recycled elements
                deleteBtn.clickable = new Clickable(() => { });
            }
            else
            {
                var stack = slot.GetItemStack();
                contentLabel.text = $"{stack.Item.GetName()} (x{stack.Quantity})";
                contentLabel.style.color = Color.white;
                deleteBtn.SetEnabled(true);

                // Use a fresh clickable to avoid event stacking on pooled items
                deleteBtn.clickable = new Clickable(() =>
                {
                    try
                    {
                        slot.RemoveItemStack();
                        _selectedInventory.PropagateChange(slot.Index);
                        _listView.RefreshItems();
                    }
                    catch (InventorySlotEmptyException)
                    {
                    }
                });
            }
        }
    }
}
#endif