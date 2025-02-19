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

namespace Internal.ComponentFactory.Krypton.Navigator
{
	/// <summary>
	/// Details for a direction button (next/previous) action event.
	/// </summary>
    public class DirectionActionEventArgs : KryptonPageEventArgs
	{
		#region Instance Fields
		private DirectionButtonAction _action;
		#endregion

		#region Identity
		/// <summary>
        /// Initialize a new instance of the DirectionActionEventArgs class.
		/// </summary>
		/// <param name="page">Page effected by event.</param>
		/// <param name="index">Index of page in the owning collection.</param>
        /// <param name="action">Previous/Next action to take.</param>
        public DirectionActionEventArgs(KryptonPage page, 
                                        int index,
                                        DirectionButtonAction action)
			: base(page, index)
		{
            _action = action;
		}
		#endregion

        #region Action
        /// <summary>
		/// Gets and sets the next/previous action to take.
		/// </summary>
        public DirectionButtonAction Action
		{
            get { return _action; }
            set { _action = value; }
		}
		#endregion
	}
}

#endif