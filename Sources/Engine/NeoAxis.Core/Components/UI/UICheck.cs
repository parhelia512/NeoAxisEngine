﻿// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace NeoAxis
{
	/// <summary>
	/// Check box with on/off state. Third state (Indeterminate) is also supported.
	/// </summary>
	public class UICheck : UIControl
	{
		bool cursorInsideArea;
		bool pushed;
		object touchDown;
		bool touchDownPointerInside;

		/////////////////////////////////////////

		public delegate void ClickDelegate( UICheck sender );
		public event ClickDelegate Click;

		/////////////////////////////////////////

		//!!!!Font property? where else

		/// <summary>
		/// The font size of the text.
		/// </summary>
		[DefaultValue( "Units 20" )]
		public Reference<UIMeasureValueDouble> FontSize
		{
			get { if( _fontSize.BeginGet() ) FontSize = _fontSize.Get( this ); return _fontSize.value; }
			set { if( _fontSize.BeginSet( this, ref value ) ) { try { FontSizeChanged?.Invoke( this ); } finally { _fontSize.EndSet(); } } }
		}
		/// <summary>Occurs when the <see cref="TitleBarFontSize"/> property value changes.</summary>
		public event Action<UICheck> FontSizeChanged;
		ReferenceField<UIMeasureValueDouble> _fontSize = new UIMeasureValueDouble( UIMeasure.Units, 20 );

		public enum CheckValue
		{
			Unchecked,
			Checked,
			Indeterminate,
		}

		/// <summary>
		/// Specifies the value of the check box.
		/// </summary>
		[DefaultValue( CheckValue.Unchecked )]
		public Reference<CheckValue> Checked
		{
			get { if( _checked.BeginGet() ) Checked = _checked.Get( this ); return _checked.value; }
			set { if( _checked.BeginSet( this, ref value ) ) { try { CheckedChanged?.Invoke( this ); } finally { _checked.EndSet(); } } }
		}
		public event Action<UICheck> CheckedChanged;
		ReferenceField<CheckValue> _checked = CheckValue.Unchecked;

		/////////////////////////////////////////

		public UICheck()
		{
			Size = new UIMeasureValueVector2( UIMeasure.Units, 25, 25 );
		}

		/// <summary>
		/// Whether control can be focused.
		/// </summary>
		[Browsable( false )]
		public override bool CanFocus
		{
			get { return EnabledInHierarchy && VisibleInHierarchy && !ReadOnlyInHierarchy; }
		}

		protected override bool OnMouseDown( EMouseButtons button )
		{
			//bool ret = base.OnMouseDown( button );

			if( button == EMouseButtons.Left && VisibleInHierarchy && cursorInsideArea && EnabledInHierarchy && !ReadOnlyInHierarchy )
			{
				Focus();
				pushed = true;
				Capture = true;
				return true;
			}

			return false;//return base.OnMouseDown( button );
		}

		protected override bool OnMouseUp( EMouseButtons button )
		{
			//bool ret = base.OnMouseUp( button );

			if( pushed && VisibleInHierarchy && EnabledInHierarchy && !ReadOnlyInHierarchy )
			{
				pushed = false;
				Capture = false;
				if( cursorInsideArea )
					PerformClick();
				return true;
			}

			return false;//base.OnMouseUp( button );
		}

		protected virtual bool OnCursorIsInArea()
		{
			//touch
			if( touchDown != null && touchDownPointerInside )
				return true;

			//control rectangle
			if( !( new Rectangle( Vector2.Zero, new Vector2( 1, 1 ) ) ).Contains( MousePosition ) )
				return false;
			////control rectangle with text
			//{
			//	GetScreenRectangle( out var rect );

			//	var renderer = ParentContainer?.Viewport?.CanvasRenderer;
			//	if( renderer != null && renderer.DefaultFont != null )
			//	{
			//		var textLength = renderer.DefaultFont.GetTextLength( renderer.DefaultFontSize, renderer, Text );
			//		rect.Right += textLength;
			//	}

			//	if( !rect.Contains( ConvertLocalToScreen( MousePosition ) ) )
			//		return false;
			//}

			if( ParentContainer != null && ParentContainer.IsControlCursorCoveredByOther( this ) )
				return false;

			//!!!!было
			////click mask
			//if( !string.IsNullOrEmpty( clickMask ) )
			//{
			//if( needUpdateClickTextureMask )
			//{
			//	clickTextureMask = TextureMaskManager.GetMask( clickMask );
			//	needUpdateClickTextureMask = false;
			//}

			//if( clickTextureMask != null )
			//{
			//	Vec2 pos = clickMaskTextureCoord.LeftTop + clickMaskTextureCoord.GetSize() * MousePosition;
			//	if( !clickTextureMask.GetValue( pos ) )
			//		return false;
			//}
			//}

			return true;
		}

		protected override bool OnKeyDown( KeyEvent e )
		{
			if( e.Key == EKeys.Space && Focused )
			{
				PerformClick();
				return true;
			}

			return base.OnKeyDown( e );
		}

		protected override void OnMouseMove( Vector2 mouse )
		{
			base.OnMouseMove( mouse );

			if( EnabledInHierarchy && VisibleInHierarchy && !ReadOnlyInHierarchy )
			{
				bool intoArea = OnCursorIsInArea();
				if( cursorInsideArea != intoArea )
				{
					cursorInsideArea = intoArea;

					if( cursorInsideArea )
						GetStyle().PerformCheckMouseEnter( this );
					else
						GetStyle().PerformCheckMouseLeave( this );
					//ParentContainer?.PlaySound( cursorInsideArea ? SoundMouseEnter : SoundMouseLeave );
				}
			}
		}

		bool CheckTouchPointerInside( TouchData e )//Vector2I positionInPixels )
		{
			if( ParentContainer != null && ParentContainer.IsControlCoveredByOther( this, e.Position ) )
				return false;
			//if( ParentContainer != null && ParentContainer.IsControlCursorCoveredByOther( this ) )
			//	return false;

			GetScreenRectangle( out var rect );
			var rectInPixels = rect * ParentContainer.Viewport.SizeInPixels.ToVector2();
			var distanceInPixels = rectInPixels.GetPointDistance( e.PositionInPixels/*positionInPixels*/.ToVector2() );

			//!!!!
			int maxDistance = ParentContainer.Viewport.SizeInPixels.MinComponent() / 30;

			return distanceInPixels <= maxDistance;
		}

		protected override bool OnTouch( TouchData e )
		{
			switch( e.Action )
			{
			case TouchData.ActionEnum.Down:
				if( VisibleInHierarchy && EnabledInHierarchy && !ReadOnlyInHierarchy && touchDown == null )
				{
					if( ParentContainer != null && ParentContainer.IsControlCoveredByOther( this, e.Position ) )
						break;
					//if( ParentContainer != null && ParentContainer.IsControlCursorCoveredByOther( this ) )
					//	break;

					GetScreenRectangle( out var rect );
					var rectInPixels = rect * ParentContainer.Viewport.SizeInPixels.ToVector2();
					var distanceInPixels = rectInPixels.GetPointDistance( e.PositionInPixels.ToVector2() );

					var item = new TouchData.TouchDownRequestToProcessTouch( this, 0, distanceInPixels, null,
						delegate ( UIControl sender, TouchData touchData, object anyData )
						{
							Focus();

							//start touch
							touchDown = e.PointerIdentifier;
							touchDownPointerInside = true;
						} );
					e.TouchDownRequestToControlActions.Add( item );
				}
				break;

			case TouchData.ActionEnum.Up:
				if( touchDown != null && ReferenceEquals( e.PointerIdentifier, touchDown ) )
				{
					if( VisibleInHierarchy && EnabledInHierarchy && !ReadOnlyInHierarchy && touchDownPointerInside )
						PerformClick();
					touchDown = null;
					touchDownPointerInside = false;
				}
				break;

			case TouchData.ActionEnum.Move:
				if( touchDown != null && ReferenceEquals( e.PointerIdentifier, touchDown ) )
					touchDownPointerInside = CheckTouchPointerInside( e ); //CheckTouchPointerInside( e.PositionInPixels );
				break;

			}

			return base.OnTouch( e );
		}

		protected override void OnUpdate( float delta )
		{
			base.OnUpdate( delta );

			if( ( pushed || cursorInsideArea ) && ( !VisibleInHierarchy || !EnabledInHierarchy || ReadOnlyInHierarchy ) )
			{
				if( pushed )
				{
					pushed = false;
					Capture = false;
				}
				cursorInsideArea = false;
			}

			if( EnabledInHierarchy && VisibleInHierarchy && !ReadOnlyInHierarchy )
			{
				bool intoArea = OnCursorIsInArea();
				if( cursorInsideArea != intoArea )
				{
					cursorInsideArea = intoArea;

					if( cursorInsideArea )
						GetStyle().PerformCheckMouseEnter( this );
					else
						GetStyle().PerformCheckMouseLeave( this );
					//ParentContainer?.PlaySound( cursorInsideArea ? SoundMouseEnter : SoundMouseLeave );
				}
			}

			if( pushed && ( !VisibleInHierarchy || !EnabledInHierarchy || ReadOnlyInHierarchy ) )
			{
				pushed = false;
				Capture = false;
			}
		}

		//!!!!было. хотя в TIckUI делается
		//protected override void OnVisibleInHierarchyChanged()
		//{
		//	base.OnVisibleInHierarchyChanged();

		//	if( pushed && !VisibleInHierarchy )
		//	{
		//		pushed = false;
		//		Capture = false;
		//	}
		//}

		protected override void OnEnabledInHierarchyChanged()
		{
			base.OnEnabledInHierarchyChanged();

			if( pushed && !EnabledInHierarchy )
			{
				pushed = false;
				Capture = false;
			}
		}

		public enum StateEnum
		{
			Normal,
			//Focused,
			Hover,
			Pushed,
			//Highlighted,
			Disabled,
		}

		[Browsable( false )]
		public StateEnum State
		{
			get
			{
				if( EnabledInHierarchy && !ReadOnlyInHierarchy )
				{
					if( Pushed )
						return CursorInsideArea ? StateEnum.Pushed : StateEnum.Hover;
					else
					{
						if( CursorInsideArea )
							return StateEnum.Hover;
						else
							return /*Highlighted ? StateEnum.Highlighted : */StateEnum.Normal;
					}
				}
				else
					return StateEnum.Disabled;
			}
		}

		//!!!!было

		//[Category( "Button" )]
		////!!!!!![Editor( typeof( EditorTextureUITypeEditor ), typeof( UITypeEditor ) )]
		//[DefaultValue( "" )]
		//public string ClickMask
		//{
		//	get { return clickMask; }
		//	set
		//	{
		//		if( clickMask == value )
		//			return;
		//		clickMask = value;
		//		needUpdateClickTextureMask = true;
		//	}
		//}

		//[DefaultValue( typeof( Rect ), "0 0 1 1" )]
		//[Category( "Button" )]
		//public Rect ClickMaskTextureCoord
		//{
		//	get { return clickMaskTextureCoord; }
		//	set { clickMaskTextureCoord = value; }
		//}

		protected virtual void OnClick() { }

		public void PerformClick()
		{
			if( Checked.Value == CheckValue.Checked )
				Checked = CheckValue.Unchecked;
			else
				Checked = CheckValue.Checked;

			GetStyle().PerformCheckClick( this );
			OnClick();
			Click?.Invoke( this );
		}

		[Browsable( false )]
		public bool CursorInsideArea
		{
			get { return cursorInsideArea; }
		}

		[Browsable( false )]
		public bool Pushed
		{
			get { return pushed; }
		}
	}
}
