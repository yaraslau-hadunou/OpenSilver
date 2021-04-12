using System;
using System.Collections.Generic;
using System.Text;

#if MIGRATION
namespace System.Windows.Controls
#else
namespace Windows.UI.Xaml.Controls
#endif
{
    public class TileViewItem : ContentControl
    {
        private Button _maximizeButton;
        internal TileView _tileViewParent = null; // Note: if this item is in TileView.Items, it is set during TileView.OnApplyTemplate

        public TileViewItem()
        {
            this.DefaultStyleKey = typeof(TileViewItem);
        }

#if MIGRATION
        public override void OnApplyTemplate()
#else
        protected override void OnApplyTemplate() 
#endif
        {
            base.OnApplyTemplate();
            _maximizeButton = GetTemplateChild("PART_MaximizeButton") as Button;

            _maximizeButton.Click += MaximizeButton_Click;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            // if the parent is a TileView, tell it that this tile wants to be maximized.
            if (_tileViewParent != null)
            {
                _tileViewParent.MaximizeTile(this);
            }
        }

        internal void Maximize()
        {
            VisualStateManager.GoToState(this, "Maximized", false);
            if (OnMaximize != null)
            {
                OnMaximize(this, null);
            }
        }

        internal void Minimize()
        {
            VisualStateManager.GoToState(this, "Minimized", false);
            if (OnMinimize != null)
            {
                OnMinimize(this, null);
            }
        }

        public event EventHandler OnMaximize;
        public event EventHandler OnMinimize;

        #region HeaderedContentControl

        #region public object Header
        /// <summary>
        /// Gets or sets the content for the header of the control.
        /// </summary>
        /// <value>
        /// The content for the header of the control. The default value is
        /// null.
        /// </value>
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Identifies the
        /// <see cref="TileViewItem.Header" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="TileViewItem.Header" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty HeaderProperty =
                DependencyProperty.Register(
                        "Header",
                        typeof(object),
                        typeof(TileViewItem),
                        new PropertyMetadata(OnHeaderPropertyChanged));

        /// <summary>
        /// HeaderProperty property changed handler.
        /// </summary>
        /// <param name="d">HeaderedContentControl whose Header property is changed.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs, which contains the old and new value.</param>
        private static void OnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TileViewItem ctrl = (TileViewItem)d;
            ctrl.OnHeaderChanged(e.OldValue, e.NewValue);
        }
        #endregion public object Header

        #region public DataTemplate HeaderTemplate
        /// <summary>
        /// Gets or sets the template that is used to display the content of the
        /// control's header.
        /// </summary>
        /// <value>
        /// The template that is used to display the content of the control's
        /// header. The default is null.
        /// </value>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the
        /// <see cref="TileViewItem.HeaderTemplate" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="TileViewItem.HeaderTemplate" />
        /// dependency property.
        /// </value>
        public static readonly DependencyProperty HeaderTemplateProperty =
                DependencyProperty.Register(
                        "HeaderTemplate",
                        typeof(DataTemplate),
                        typeof(TileViewItem),
                        new PropertyMetadata(OnHeaderTemplatePropertyChanged));

        /// <summary>
        /// HeaderTemplateProperty property changed handler.
        /// </summary>
        /// <param name="d">HeaderedContentControl whose HeaderTemplate property is changed.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs, which contains the old and new value.</param>
        private static void OnHeaderTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TileViewItem ctrl = (TileViewItem)d;
            ctrl.OnHeaderTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }
        #endregion public DataTemplate HeaderTemplate

        /// <summary>
        /// Called when the value of the <see cref="TileViewItem.Header" />
        /// property changes.
        /// </summary>
        /// <param name="oldHeader">
        /// The old value of the <see cref="TileViewItem.Header" /> property.
        /// </param>
        /// <param name="newHeader">
        /// The new value of the <see cref="TileViewItem.Header" /> property.
        /// </param>
        protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="TileViewItem.HeaderTemplate" />
        /// property changes.
        /// </summary>
        /// <param name="oldHeaderTemplate">
        /// The old value of the <see cref="TileViewItem.HeaderTemplate" />
        /// property.
        /// </param>
        /// <param name="newHeaderTemplate">
        /// The new value of the <see cref="TileViewItem.HeaderTemplate" />
        /// property.
        /// </param>
        protected virtual void OnHeaderTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
        {
        }

        #endregion HeaderedContentControl
    }
}
