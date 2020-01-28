﻿
//===============================================================================
//
//  IMPORTANT NOTICE, PLEASE READ CAREFULLY:
//
//  => This code is licensed under the GNU General Public License (GPL v3). A copy of the license is available at:
//        https://www.gnu.org/licenses/gpl.txt
//
//  => As stated in the license text linked above, "The GNU General Public License does not permit incorporating your program into proprietary programs". It also does not permit incorporating this code into non-GPL-licensed code (such as MIT-licensed code) in such a way that results in a non-GPL-licensed work (please refer to the license text for the precise terms).
//
//  => Licenses that permit proprietary use are available at:
//        http://www.cshtml5.com
//
//  => Copyright 2019 Userware/CSHTML5. This code is part of the CSHTML5 product (cshtml5.com).
//
//===============================================================================



using CSHTML5.Internal;
using System;
using System.Collections.Generic;

#if MIGRATION
using System.Windows.Controls.Primitives;
#else
using Windows.UI.Xaml.Controls.Primitives;
#endif

#if MIGRATION
namespace System.Windows.Controls
#else
namespace Windows.UI.Xaml.Controls
#endif
{
    //------------
    // Note: unlike in .NET, we use the "ItemContainerGenerator" to store both the container (such as ListBoxItem or ComboBoxItem) AND the stuff generated by the DataTemplates.
    //------------

    public class ItemContainerGenerator
    {
        Dictionary<object, List<DependencyObject>> _itemsToContainers = new Dictionary<object, List<DependencyObject>>(); // Note: this maps each item (for example a string or a business object) to the corresponding element that is added to the visual tree (such a datatemplate) or to the native DOM element in case of native combo box for example. The reason why each single element can be associated to multiple objects is because of Strings and other value types: for example, if two identical strings are added to the ItemsControl, they will be the same key of the dictionary.
        List<DependencyObject> _containers = new List<DependencyObject>(); //this list is kept to get the index from the container (with minimum work to keep it updated, it might not be the most efficient method perf-wise).

        /// <summary>
        /// Returns the container corresponding to the specified item, or null if no container was found.
        /// </summary>
        /// <param name="item">The item to retrieve the container for.</param>
        /// <returns>A container that corresponds to the specified item, if the item has a container
        /// and exists in the collection; otherwise, null.</returns>
        public DependencyObject ContainerFromItem(object item)
        {
            if (_itemsToContainers.ContainsKey(item))
            {
                List<DependencyObject> containersAssociatedToTheItem = _itemsToContainers[item];
                if (containersAssociatedToTheItem != null && containersAssociatedToTheItem.Count > 0)
                {
                    DependencyObject container = containersAssociatedToTheItem[0];
                    return container;
                }
                else
                    return null;
            }
            else
                return null;
        }


        /// <summary>
        /// Returns the container at the given index, or null if the index is negative or above the amount of containers.
        /// </summary>
        /// <param name="index">The index at which the container is located.</param>
        /// <returns>The container at the given index, or null if the index is negative or above the amount of containers.</returns>
        public DependencyObject ContainerFromIndex(int index)
        {
            if (index > -1 && _containers.Count > index)
            {
                return _containers[index];
            }
            else
                return null;
        }

        public int IndexFromContainer(DependencyObject container)
        {
            return _containers.IndexOf(container);
        }

        /// <summary>
        /// Removes the container associated to the item.
        /// </summary>
        /// <param name="container">The container to remove.</param>
        /// <param name="correspondingItem">The item that corresponds to the container to remove.</param>
        /// <returns>True if found and removed, false otherwise.</returns>
        public bool INTERNAL_TryUnregisterContainer(DependencyObject container, object correspondingItem)
        {
            int indexOfContainerInContainerList = _containers.IndexOf(container);
            if (indexOfContainerInContainerList != -1)
            {
                _containers.Remove(container);
            }
            if (_itemsToContainers.ContainsKey(correspondingItem))
            {
                List<DependencyObject> containersAssociatedToTheItem = _itemsToContainers[correspondingItem];
                if (containersAssociatedToTheItem != null)
                {
                    if (containersAssociatedToTheItem.Remove(container))
                    {

                        if (containersAssociatedToTheItem.Count == 0)
                            _itemsToContainers.Remove(correspondingItem);

                        return true;
                    }
                }
            }

            return false;
        }

        ///// <summary>
        ///// Removes the container at the given position. IMPORTANT NOTE: Contrary to expectations, this method is not as efficient as INTERNAL_TryUnregisterContainer so if given the choice, use that one instead.
        ///// </summary>
        ///// <param name="index">The index of the Container to remove.</param>
        ///// <returns>True if found and removed, false otherwise</returns>
        //public bool INTERNAL_TryUnregisterContainerAt(int index)
        //{
        //    //Note: this method has not been tested yet.
        //    var container = ContainerFromIndex(index);
        //    foreach(object item in _itemsToContainers.Keys)
        //    {
        //        List<object> containersAssociatedToTheItem = _itemsToContainers[item];
        //        if (containersAssociatedToTheItem.Remove(container))
        //        {

        //            if (containersAssociatedToTheItem.Count == 0)
        //                _itemsToContainers.Remove(item);

        //            _containers.Remove(container);
        //            return true;
        //        }
        //    }
        //    return false;
        //}


        /// <summary>
        /// Resets the ItemContainerGenerator.
        /// </summary>
        public void INTERNAL_Clear()
        {
            _itemsToContainers.Clear();
            _containers.Clear();
        }


        /// <summary>
        /// Adds a container to the collection of containers.
        /// </summary>
        /// <param name="container">The container to add.</param>
        /// <param name="correspondingItem">The item that corresponds to the container to add.</param>
        public void INTERNAL_RegisterContainer(DependencyObject container, object correspondingItem)
        {
            List<DependencyObject> containersAssociatedToTheItem;
            if (_itemsToContainers.ContainsKey(correspondingItem))
            {
                containersAssociatedToTheItem = _itemsToContainers[correspondingItem];
            }
            else
            {
                containersAssociatedToTheItem = new List<DependencyObject>();
                _itemsToContainers.Add(correspondingItem, containersAssociatedToTheItem);
            }
            containersAssociatedToTheItem.Add(container);
            _containers.Add(container);
        }

        /// <summary>
        /// Gets all the containers associated to all the items.
        /// </summary>
        public IEnumerable<DependencyObject> INTERNAL_AllContainers
        {
            get
            {
                foreach (List<DependencyObject> containers in
#if BRIDGE
                    INTERNAL_BridgeWorkarounds.GetDictionaryValues_SimulatorCompatible(_itemsToContainers)
#else
                    _itemsToContainers.Values
#endif

                    )
                {
                    foreach (DependencyObject container in containers)
                    {
                        yield return container;
                    }
                }
            }
        }
    }
}

