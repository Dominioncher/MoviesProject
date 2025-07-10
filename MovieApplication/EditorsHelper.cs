using DevExpress.Utils.Win;
using DevExpress.Xpo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Repository;
using MovieApplicationDataBase.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static DevExpress.Utils.Frames.FrameHelper;
using static MovieApplication.EditorsHelper;
using static System.Collections.Specialized.BitVector32;

namespace MovieApplication
{
    public static class EditorsHelper
    {
        public class KeyComparer<T> : IEqualityComparer<T>
        {
            private Func<T, object> _keySelector;

            public KeyComparer(Func<T, object> keySelector)
            {
                _keySelector = keySelector;
            }

            public bool Equals(T x, T y)
            {
                return _keySelector(x).Equals(_keySelector(y));
            }

            public int GetHashCode(T obj)
            {
                return _keySelector(obj).GetHashCode();
            }
        }

        public static void FillJanresCheckedComboBox(RepositoryItemCheckedComboBoxEdit edit)
        {
            var repository = new JanresRepository();
            var janres = repository.GetJanres();

            FillComboBox(edit, janres, "Name", new KeyComparer<DBJanres>(x => x.ID));

        }

        private static void FillComboBox<T>(RepositoryItemCheckedComboBoxEdit edit, List<T> dataSource, string DisplayMember, KeyComparer<T> comparer=null) where T : class
        {

            edit.EditValueType = EditValueTypeCollection.List;
            edit.EditValueChanging += (sender, e) =>
            {
                if (e.NewValue == null)
                {
                    return;
                }

                if (e.NewValue is List<object> value)
                {
                    e.NewValue = value.Cast<T>().ToList();                    
                }

                if (comparer != null)
                {
                    if (e.NewValue is List<T> input_value)
                    {
                        e.NewValue = dataSource.Where(x => input_value.Contains(x, comparer)).ToList();
                    }
                }
            };
            edit.CustomDisplayText += (sender, e) =>
            {

                if (e.Value == null || (e.Value as List<T>).Count == 0)
                {
                    return;
                }
                e.DisplayText = string.Join(", ", (e.Value as List<T>).Select(x => x.GetType().GetProperty(DisplayMember).GetValue(x)));
            };
            edit.Popup += (sender, e) =>
            {
                var popupControl = sender as IPopupControl;
                var f = popupControl.PopupWindow as CheckedPopupContainerForm;
                var listBox = f.ActiveControl as CheckedListBoxControl;
                listBox.CustomItemDisplayText += (sender2, e2) =>
                {

                    if (e2.Value == null)
                    {
                        return;
                    }

                    e2.DisplayText = (e2.Value as T).GetType().GetProperty(DisplayMember).GetValue(e2.Value).ToString();
                };
                listBox.Refresh();
            };
            edit.DataSource = dataSource;
            edit.DropDownRows = 10;
            edit.SelectAllItemVisible = false;
            edit.PopupSizeable = false;
        }
    }
}
