using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUtils.Controls
{
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
            public string ViewName { get; set; }
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
            m_popupCtrl = Template.FindName("CB_Part_ListBox", this) as ListBox;
            m_selectionCtrl = Template.FindName("CB_Part_ListBoxChk", this) as ListBox;
            m_selectionCtrl.ItemsSource = ChekedItems;
            m_popupCtrl.SelectionChanged += OnPopupCtrlSelectionChanged;
            m_selectionCtrl.SelectionChanged += OnSelectionCtrlSelectionChanged;

            if (ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                {
                    MultiCbxBaseData bdc = item as MultiCbxBaseData;
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
                MultiCbxBaseData datachk = item as MultiCbxBaseData;

                for (int i = 0; i < m_selectionCtrl.SelectedItems.Count; i++)
                {
                    MultiCbxBaseData datachklist = m_selectionCtrl.SelectedItems[i] as MultiCbxBaseData;
                    if (datachklist.Id == datachk.Id)
                    {
                        m_selectionCtrl.SelectedItems.Remove(m_selectionCtrl.SelectedItems[i]);
                    }
                }
            }
        }

        private void OnPopupCtrlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                MultiCbxBaseData datachk = item as MultiCbxBaseData;
                datachk.IsCheck = true;
                if (ChekedItems.IndexOf(datachk) < 0)
                {
                    ChekedItems.Add(datachk);
                }
            }

            foreach (var item in e.RemovedItems)
            {
                MultiCbxBaseData datachk = item as MultiCbxBaseData;
                datachk.IsCheck = false;
                ChekedItems.Remove(datachk);
            }
        }

        public ObservableCollection<MultiCbxBaseData> ChekedItems = new();
        private ListBox m_selectionCtrl;
        private ListBox m_popupCtrl;
    }
}
