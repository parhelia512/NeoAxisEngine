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
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace Internal.ComponentFactory.Krypton.Toolkit
{
	/// <summary>
	/// Provides a movement rectangle that will be used to limit separator movement.
	/// </summary>
	public class SplitterMoveRectMenuArgs : EventArgs
	{
		#region Instance Fields
        private Rectangle _moveRect;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the SplitterMoveRectMenuArgs class.
        /// </summary>
        /// <param name="moveRect">Initial movement rectangle that limits separator movements.</param>
        public SplitterMoveRectMenuArgs(Rectangle moveRect)
        {
            _moveRect = moveRect;
        }
        #endregion

        #region Public
        /// <summary>
		/// Gets and sets the movement box for a separator.
		/// </summary>
        public Rectangle MoveRect
		{
            get { return _moveRect; }
            set { _moveRect = value; }
		}
        #endregion
	}
}

#endif