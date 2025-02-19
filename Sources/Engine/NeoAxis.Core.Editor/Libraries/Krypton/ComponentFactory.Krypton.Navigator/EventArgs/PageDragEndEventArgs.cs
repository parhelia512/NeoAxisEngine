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

namespace Internal.ComponentFactory.Krypton.Navigator
{
	/// <summary>
    /// Details for an event that provides pages associated with a page dragging event.
	/// </summary>
	public class PageDragEndEventArgs : EventArgs
	{
		#region Instance Fields
        private bool _dropped;
        private KryptonPageCollection _pages;
		#endregion

		#region Identity
		/// <summary>
        /// Initialize a new instance of the PageDragEndEventArgs class.
		/// </summary>
        /// <param name="dropped">True if a drop was performed; otherwise false.</param>
        /// <param name="pages">Array of event associated pages.</param>
        public PageDragEndEventArgs(bool dropped,
                                    KryptonPage[] pages)
		{
            _dropped = dropped;
            _pages = new KryptonPageCollection();

            if (pages != null)
                _pages.AddRange(pages);
		}
        #endregion

        #region Dropped
        /// <summary>
        /// Gets a value indicating if the drop was performed.
        /// </summary>
        public bool Dropped
        {
            get { return _dropped; }
        }
        #endregion

        #region Pages
        /// <summary>
        /// Gets access to the collection of pages.
        /// </summary>
        public KryptonPageCollection Pages
        {
            get { return _pages; }
        }
        #endregion
    }
}

#endif