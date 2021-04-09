using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    // Summary:
    //     Represents a weak reference, which references an object while still allowing
    //     that object to be reclaimed by garbage collection.
    public class WeakReference
    {
        object _jsWeakRef = null;

        // Summary:
        //     Initializes a new instance of the System.WeakReference class. This constructor
        //     overload cannot be implemented in Silverlight-based applications.
        //
        // Exceptions:
        //   System.NotImplementedException:
        //     This constructor is not implemented.
        protected WeakReference() { }
        //
        // Summary:
        //     Initializes a new instance of the System.WeakReference class, referencing
        //     the specified object.
        //
        // Parameters:
        //   target:
        //     The object to track or null.
        public WeakReference(object target)
        {
            Target = target;
        }
        //
        // Summary:
        //     Initializes a new instance of the System.WeakReference class, referencing
        //     the specified object and using the specified resurrection tracking.
        //
        // Parameters:
        //   target:
        //     An object to track.
        //
        //   trackResurrection:
        //     Indicates when to stop tracking the object. If true, the object is tracked
        //     after finalization; if false, the object is only tracked until finalization.
        public WeakReference(object target, bool trackResurrection)
        {
            Target = target;
            TrackResurrection = trackResurrection;
        }

        // Summary:
        //     Gets an indication whether the object referenced by the current System.WeakReference
        //     object has been garbage collected.
        //
        // Returns:
        //     true if the object referenced by the current System.WeakReference object
        //     has not been garbage collected and is still accessible; otherwise, false.
        public virtual bool IsAlive
        {
            get
            {
                bool ret = false;
                if (_jsWeakRef != null)
                {
                    ret = Bridge.Script.Write<bool>("({0}.deref() != null && {0}.deref() != undefined)", _jsWeakRef);
                }
                return ret;
            }
        }
        //
        // Summary:
        //     Gets or sets the object (the target) referenced by the current System.WeakReference
        //     object.
        //
        // Returns:
        //     null if the object referenced by the current System.WeakReference object
        //     has been garbage collected; otherwise, a reference to the object referenced
        //     by the current System.WeakReference object.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The reference to the target object is invalid. This exception can be thrown
        //     while setting this property if the value is a null reference or if the object
        //     has been finalized during the set operation.
        public virtual object Target
        {
            get
            {
                object ret = null;
                if (_jsWeakRef != null)
                {
                    var jsWeakRef = _jsWeakRef;
                    ret = Bridge.Script.Write<dynamic>(@"(function () {
    var returnValue = {0}.deref();
    if(!returnValue) {
        returnValue = null;
    }
    return returnValue;
})()", jsWeakRef);
                }
                return ret;
            }
            set
            {
                if(value != null)
                {
                    _jsWeakRef = Bridge.Script.Write<dynamic>("new WeakRef({0})", value);
                }
                else
                {
                    _jsWeakRef = null;
                }
            }
        }
        //
        // Summary:
        //     Gets an indication whether the object referenced by the current System.WeakReference
        //     object is tracked after it is finalized.
        //
        // Returns:
        //     true if the object the current System.WeakReference object refers to is tracked
        //     after finalization; or false if the object is only tracked until finalization.
        public virtual bool TrackResurrection { get; }
    }
}