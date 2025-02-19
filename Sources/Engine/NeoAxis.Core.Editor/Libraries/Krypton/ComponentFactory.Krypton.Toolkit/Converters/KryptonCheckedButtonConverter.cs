#if !DEPLOY
// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2012. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 17/267 Nepean Hwy, 
//  Seaford, Vic 3198, Australia and are supplied subject to licence terms.
// 
//
// *****************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace Internal.ComponentFactory.Krypton.Toolkit
{
    internal class KryptonCheckedButtonConverter : ReferenceConverter
    {
        #region Identity
        /// <summary>
        /// Initialize a new instance of the KryptonCheckedButtonConverter class.
        /// </summary>
        public KryptonCheckedButtonConverter()
            : base(typeof(KryptonCheckButton))
        {
        }
        #endregion

        #region Protected Overrides
        /// <summary>
        /// Returns a value indicating whether a particular value can be added to the standard values collection.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides an additional context.</param>
        /// <param name="value">The value to check.</param>
        /// <returns></returns>
        protected override bool IsValueAllowed(ITypeDescriptorContext context, object value)
        {
            // Get access to the check set component that owns the property
            KryptonCheckSet checkSet = context.Instance as KryptonCheckSet;

            // Just in case the converter is used on a different type of component
            if (checkSet != null)
            {
                // We only allow check buttons inside the check set definition
                return checkSet.CheckButtons.Contains(value as KryptonCheckButton);
            }
            else
                return false;
        }
        #endregion
    }
}

#endif