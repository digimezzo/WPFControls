using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Digimezzo.WPFControls
{
    /// <summary>
    /// The default WPF ListBox deselects items on MouseDown instead of MouseUp, which causes deselection 
    /// of selected items when starting a Drag operation. This ListBox enables Windows Explorer like drag/drop 
    /// behaviour by deferring deselection to MouseUp. This code is based on code from David Schmitt in this 
    /// stackoverflow article http://stackoverflow.com/questions/1553622/wpf-drag-drop-from-listbox-with-selectionmode-multiple
    /// </summary>
    /// <remarks></remarks>
    public class MultiSelectListBox : ListBox
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MultiSelectListBoxItem();
        }

        public class MultiSelectListBoxItem : ListBoxItem
        {
            private bool deferSelection = false;

            protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
            {

                if ((e.ClickCount == 1 && IsSelected))
                {
                    // The user may start a drag by clicking on the selected items
                    // Delay destroying the selection to the Up event
                    this.deferSelection = true;
                }
                else
                {
                    base.OnMouseLeftButtonDown(e);
                }
            }

            protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
            {
                if (this.deferSelection)
                {
                    try
                    {
                        base.OnMouseLeftButtonDown(e);
                    }
                    finally
                    {
                        this.deferSelection = false;
                    }

                    base.OnMouseLeftButtonUp(e);
                }
            }

            protected override void OnMouseLeave(MouseEventArgs e)
            {
                // Abort deferred Down
                this.deferSelection = false;
                base.OnMouseLeave(e);
            }
        }
    }
}