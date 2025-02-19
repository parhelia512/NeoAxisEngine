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
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using Internal.ComponentFactory.Krypton.Toolkit;

namespace Internal.ComponentFactory.Krypton.Ribbon
{
	/// <summary>
	/// Storage for application button related properties.
	/// </summary>
    public class PaletteRibbonImages : Storage
    {
        #region Instance Fields
        private PaletteRedirectCheckBox _redirectCheckBox;
        private PaletteRedirectRadioButton _redirectRadioButton;
        private CheckBoxImages _imagesCheckBox;
        private RadioButtonImages _imagesRadioButton;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the PaletteRibbonImages class.
		/// </summary>
        /// <param name="redirect">Inheritence redirection instance.</param>
        /// <param name="needPaint">Paint delegate.</param>
        public PaletteRibbonImages(PaletteRedirect redirect,
                                   NeedPaintHandler needPaint)
		{
            Debug.Assert(redirect != null);
            Debug.Assert(needPaint != null);

            _imagesCheckBox = new CheckBoxImages(needPaint);
            _imagesRadioButton = new RadioButtonImages(needPaint);
            _redirectCheckBox = new PaletteRedirectCheckBox(redirect, _imagesCheckBox);
            _redirectRadioButton = new PaletteRedirectRadioButton(redirect, _imagesRadioButton);
        }
		#endregion

        #region IsDefault
        /// <summary>
        /// Gets a value indicating if all values are default.
        /// </summary>
        [Browsable(false)]
        public override bool IsDefault
        {
            get
            {
                return (_imagesCheckBox.IsDefault &&
                        _imagesRadioButton.IsDefault);
            }
        }
        #endregion

        #region CheckBox
        /// <summary>
        /// Gets and sets the ribbon check box images.
        /// </summary>
        [Category("Values")]
        [Description("Ribbon check box images.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CheckBoxImages CheckBox
        {
            get { return _imagesCheckBox; }
        }

        private bool ShouldSerializeCheckBox()
        {
            return !_imagesCheckBox.IsDefault;
        }
        #endregion

        #region RadioButton
        /// <summary>
        /// Gets and sets the ribbon radio button images.
        /// </summary>
        [Category("Values")]
        [Description("Ribbon radio button images.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RadioButtonImages RadioButton
        {
            get { return _imagesRadioButton; }
        }

        private bool ShouldSerializeRadioButton()
        {
            return !_imagesRadioButton.IsDefault;
        }
        #endregion

        #region Implementation
        internal PaletteRedirectCheckBox InternalCheckBox
        {
            get { return _redirectCheckBox; }
        }

        internal PaletteRedirectRadioButton InternalRadioButton
        {
            get { return _redirectRadioButton; }
        }
        #endregion
    }
}

#endif