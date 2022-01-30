using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WPFUtils.Controls
{
    // this is strongly inspired by 


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
        public class MultiCbxBaseData
        {
            public int Id { get; set; }
            public string ViewName { get; set; } = "";
            public bool IsCheck { get; set; }
        }

        static MultiSelectComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(typeof(MultiSelectComboBox)));
        }
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(e.Property, e.NewValue);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_popupCtrl = (ListBox)Template.FindName("CB_Part_ListBox", this);
            m_selectionCtrl = (ListBox)Template.FindName("CB_Part_ListBoxChk", this);
            m_selectionCtrl.ItemsSource = CheckedItems;
            m_popupCtrl.SelectionChanged += OnPopupCtrlSelectionChanged;
            m_selectionCtrl.SelectionChanged += OnSelectionCtrlSelectionChanged;

            if (ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                {
                    MultiCbxBaseData bdc = (MultiCbxBaseData)item;
                    if (bdc.IsCheck)
                    {
                        m_popupCtrl.SelectedItems.Add(bdc);
                    }
                }
            }
        }

        private void OnSelectionCtrlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.RemovedItems)
            {
                var datachk = (MultiCbxBaseData)item;

                for (var i = 0; i < m_popupCtrl.SelectedItems.Count; i++)
                {
                    var datachklist = (MultiCbxBaseData)m_popupCtrl.SelectedItems[i];
                    if (datachklist.Id == datachk.Id)
                    {
                        m_popupCtrl.SelectedItems.Remove(m_popupCtrl.SelectedItems[i]);
                    }
                }
            }
        }

        private void OnPopupCtrlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                // insert it such as the items are still sorted by Id
                var datachk = (MultiCbxBaseData)item;
                bool done = false;
                datachk.IsCheck = true;
                for (int idx = 0; idx < CheckedItems.Count; ++idx)
                {
                    if (CheckedItems[idx].Id == datachk.Id)
                    {
                        done = true;
                        break;}
                    if (CheckedItems[idx].Id > datachk.Id)
                    {
                        done = true;
                        CheckedItems.Insert(idx, datachk);
                        break;
                    }
                }
                if (!done)
                {
                    CheckedItems.Add(datachk);
                }
            }
            foreach (var item in e.RemovedItems)
            {
                var datachk = (MultiCbxBaseData)item;
                datachk.IsCheck = false;
                CheckedItems.Remove(datachk);
            }
        }

        public ObservableCollection<MultiCbxBaseData> CheckedItems = new();
        private ListBox m_selectionCtrl;
        private ListBox m_popupCtrl;
    }
}
