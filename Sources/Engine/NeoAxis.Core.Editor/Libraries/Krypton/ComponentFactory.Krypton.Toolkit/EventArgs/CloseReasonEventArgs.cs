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
using System.Windows.Forms;
using System.Diagnostics;

namespace Internal.ComponentFactory.Krypton.Toolkit
{
	/// <summary>
	/// Details for close reason event handlers.
	/// </summary>
	public class CloseReasonEventArgs : CancelEventArgs
	{
		#region Instance Fields
        private ToolStripDropDownCloseReason _closeReason;
        #endregion

		#region Identity
        /// <summary>
        /// Initialize a new instance of the CloseReasonEventArgs class.
		/// </summary>
        /// <param name="closeReason">Reason for the close action occuring.</param>
        public CloseReasonEventArgs(ToolStripDropDownCloseReason closeReason)
		{
            _closeReason = closeReason;
		}
        #endregion

		#region Public
		/// <summary>
		/// Gets access to the reason for the context menu closing.
		/// </summary>
        public ToolStripDropDownCloseReason CloseReason
		{
            get { return _closeReason; }
		}
        #endregion
	}
}

#endif