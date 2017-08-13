using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace FantasyTanks___Fantasy_Point_Tester
{
    /// <summary>
    /// Adapted from http://stackoverflow.com/a/34947185
    /// </summary>
    public class SortingPersistentDataGrid : DataGrid
    {
        // Dictionary to keep SortDescriptions per ItemSource
        private List<SortDescription> m_SortDescriptions = null;

        protected override void OnSorting(DataGridSortingEventArgs eventArgs)
        {
            base.OnSorting(eventArgs);
            UpdateSorting();
        }
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            ICollectionView view = CollectionViewSource.GetDefaultView(newValue);

            if (view != null && view.SortDescriptions != null)
            {
                view.SortDescriptions.Clear();

                // reset SortDescriptions for new ItemSource
                if (m_SortDescriptions != null)
                {
                    foreach (SortDescription sortDescription in m_SortDescriptions)
                    {
                        view.SortDescriptions.Add(sortDescription);

                        // I need to tell the column its SortDirection,
                        // otherwise it doesn't draw the triangle adornment
                        DataGridColumn column = Columns.FirstOrDefault(c => c.SortMemberPath == sortDescription.PropertyName);
                        if (column != null)
                            column.SortDirection = sortDescription.Direction;
                    }
                }
            }
        }

        // Store SortDescriptions in dictionary
        private void UpdateSorting()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ItemsSource);
            m_SortDescriptions = new List<SortDescription>(view.SortDescriptions);
        }
    }
}
