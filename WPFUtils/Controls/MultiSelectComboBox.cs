using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WPFUtils.Controls
{
    // this is strongly inspired by https://www.programmerall.com/article/8486854271/


    /// <summary>
    /// A combobox which allows multiple selection
    /// <remarks>
    /// <para>
    /// The control is made of:
    ///   <list type="bullet">
    ///      <item>A horizontal listbox which replaces the normal combobox text control</item>
    ///      <item>A toggle button at its right which on click pop up the list box</item>
    ///      <item>A pop-up filled with a vertical listbox. Each item has an associated checkbox</item>
    ///   </list>
    /// </para>
    /// </remarks>
    /// </summary>
    public class MultiSelectComboBox : ComboBox
    {
        public class ItemData
        {
            public int Id { get; set; }
            public string ViewName { get; set; } = "";
            public bool IsCheck { get; set; }
        }

        static MultiSelectComboBox()
        {
            // select the control template
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(typeof(MultiSelectComboBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_popupCtrl = (ListBox)Template.FindName("CB_Part_ListBox", this);
            m_selectionCtrl = (ListBox)Template.FindName("CB_Part_ListBoxChk", this);
            m_selectionCtrl.ItemsSource = m_checkedItems;
            m_popupCtrl.SelectionChanged += OnPopupCtrlSelectionChanged;
            m_selectionCtrl.SelectionChanged += OnSelectionCtrlSelectionChanged;
            InitPopupListBox(ItemsSource);

        }

        private void InitPopupListBox(IEnumerable? items)
        {
            if (items != null && m_popupCtrl != null)
            {
                foreach (var item in items)
                {
                    ItemData bdc = (ItemData)item;
                    if (bdc.IsCheck)
                    {
                        m_popupCtrl.SelectedItems.Add(bdc);
                    }
                }
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            // we use an array instead of an enumeration as we are doing multiple enumerations
            var items = newValue as object[] ?? newValue.Cast<object>().ToArray();
            InitPopupListBox(items);
            base.OnItemsSourceChanged(oldValue, items);
        }

        private void OnSelectionCtrlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // update the pop-up list
            foreach (var item in e.RemovedItems)
            {
                var removedItem = (ItemData)item;

                for (var i = 0; i < m_popupCtrl.SelectedItems.Count; i++)
                {
                    var selectedItem = (ItemData)m_popupCtrl.SelectedItems[i];
                    if (selectedItem.Id == removedItem.Id)
                    {
                        m_popupCtrl.SelectedItems.Remove(m_popupCtrl.SelectedItems[i]);
                    }
                }
            }
        }

        private void OnPopupCtrlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // update the item source
            int nChanges = 0;
            foreach (var item in e.RemovedItems)
            {
                var datachk = (ItemData)item;
                datachk.IsCheck = false;
                nChanges++;
                m_checkedItems.Remove(datachk);
            }
            foreach (var item in e.AddedItems)
            {
                // insert it such as the items are still sorted by Id
                var datachk = (ItemData)item;
                bool done = false;
                datachk.IsCheck = true;
                for (int idx = 0; idx < m_checkedItems.Count; ++idx)
                {
                    if (m_checkedItems[idx].Id == datachk.Id)
                    {
                        done = true;
                        break;
                    }
                    if (m_checkedItems[idx].Id > datachk.Id)
                    {
                        done = true;
                        nChanges++;
                        m_checkedItems.Insert(idx, datachk);
                        break;
                    }
                }
                if (!done)
                {
                    nChanges++;
                    m_checkedItems.Add(datachk);
                }
            }

            if (nChanges != 0)
            {
                CheckedItems = m_checkedItems;
            }
        }

        /// <summary>
        /// A dependent property which allows the data source to be notified of changes
        /// </summary>
        public IList<ItemData> CheckedItems
        {
            get => (IList<ItemData>) GetValue(s_checkeIemdProperty);
            set => SetValue(s_checkeIemdProperty, value);
        }

        private static readonly DependencyProperty s_checkeIemdProperty = DependencyProperty.Register("CheckedItems", typeof(IList<ItemData>), typeof(MultiSelectComboBox));
        public readonly ObservableCollection<ItemData> m_checkedItems = new();
        private ListBox? m_selectionCtrl;
        private ListBox? m_popupCtrl;
    }
}
