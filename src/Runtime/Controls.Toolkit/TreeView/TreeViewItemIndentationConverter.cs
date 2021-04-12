// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Globalization;

#if MIGRATION
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
#else
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
#endif

#if MIGRATION
namespace System.Windows.Controls
#else
namespace Windows.UI.Xaml.Controls
#endif
{
    /// <summary>
    /// Used to convert TreeViewItems into a value based on their depth in
    /// the TreeView.
    /// </summary>
    /// <QualityBand>Experimental</QualityBand>
    public class TreeViewItemIndentationConverter : IValueConverter
    {
        /// <summary>
        /// Initializes a new instance of the TreeViewItemIndentationConverter
        /// class.
        /// </summary>
        public TreeViewItemIndentationConverter()
        {
        }

        /// <summary>
        /// Convert a TreeViewItem into a value based on the depth of the item
        /// in the TreeView.
        /// </summary>
        /// <param name="value">The TreeViewItem.</param>
        /// <param name="targetType">
        /// The indentation type to convert to (such as Thickness or double).
        /// </param>
        /// <param name="parameter">
        /// The number of pixels to indent each level of the TreeView.  A
        /// default value of 15.0 will be used if no parameter is provided.
        /// </param>
        /// <param name="culture">
        /// The culture used to convert the TreeViewItem.
        /// </param>
        /// <returns>
        /// A value based on the depth of the item in the TreeView.
        /// </returns>
#if MIGRATION
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#else
        public object Convert(object value, Type targetType, object parameter, string culture)
#endif
        {
            TreeViewItem item = value as TreeViewItem;
            if (item != null)
            {
                // Determine the number of pixels to indent each level of the
                // TreeView (or use the default value of 15.0)
                double indentationPerDepth = 15.0;
                string indentationPerDepthParameter = parameter as string;
                if (string.IsNullOrEmpty(indentationPerDepthParameter) ||
#if NETSTANDARD
#if MIGRATION
                    double.TryParse(indentationPerDepthParameter, NumberStyles.Any, culture, out indentationPerDepth)
#else
                    double.TryParse(indentationPerDepthParameter, NumberStyles.Any, (string.IsNullOrWhiteSpace(culture) ? null : new CultureInfo(culture)), out indentationPerDepth)
#endif
#elif BRIDGE
#if MIGRATION
                    double.TryParse(indentationPerDepthParameter, culture, out indentationPerDepth)
#else
                    double.TryParse(indentationPerDepthParameter, (string.IsNullOrWhiteSpace(culture) ? null : new CultureInfo(culture)), out indentationPerDepth)
#endif
#endif
                    )
                {
                    try
                    {
                        // Convert the depth into the indentation
                        double indentation = item.GetDepth() * indentationPerDepth;
                        return WrapIndentation(indentation, targetType);
                    }
                    catch (ArgumentException)
                    {
                        // Ignore the case where a TreeViewItem isn't associated
                        // with a TreeViewItem, in which case we'll return an
                        // indentation of zero anyway
                    }
                }
            }

            return WrapIndentation(0, targetType);
        }

        /// <summary>
        /// Wrap the indentation in the desired type.
        /// </summary>
        /// <param name="indentation">
        /// The number of pixels to indent the TreeViewItem.
        /// </param>
        /// <param name="targetType">
        /// The indentation type to convert to (such as Thickness or double).
        /// </param>
        /// <returns>
        /// A value based on the depth of the item in the TreeView.
        /// </returns>
        private static object WrapIndentation(double indentation, Type targetType)
        {
            if (targetType == typeof(Thickness))
            {
                return new Thickness(indentation, 0, 0, 0);
            }
            else if (targetType == typeof(Point))
            {
                return new Point(indentation, 0);
            }
            else if (targetType == typeof(Rect))
            {
                return new Rect(indentation, 0, 0, 0);
            }
            else if (targetType == typeof(Size))
            {
                return new Size(indentation, 0);
            }
            else if (targetType == typeof(GridLength))
            {
                return new GridLength(indentation, GridUnitType.Pixel);
            }

            return indentation;
        }

        /// <summary>
        /// Convert an indentation back into a TreeViewItem.  This always throws
        /// a NotSupportedException.
        /// </summary>
        /// <param name="value">The indentation.</param>
        /// <param name="targetType">The type of the indentation.</param>
        /// <param name="parameter">
        /// The number of pixels to indent each level of the TreeView.
        /// </param>
        /// <param name="culture">
        /// The culture used to convert the TreeViewItem.
        /// </param>
        /// <returns>Always throws a NotSupportedException.</returns>
#if MIGRATION
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#else
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
#endif
        {
            throw new NotSupportedException();
        }
    }
}