#if !NO_UI_WEB_BROWSER
namespace Internal.Xilium.CefGlue.Platform
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Internal.Xilium.CefGlue;
    using Internal.Xilium.CefGlue.Interop;
    using Internal.Xilium.CefGlue.Platform.Windows;

    internal unsafe sealed class CefWindowInfoWindowsImpl : CefWindowInfo
    {
        private cef_window_info_t_windows* _self;

        public CefWindowInfoWindowsImpl()
            : base(true)
        {
            _self = cef_window_info_t_windows.Alloc();
        }

        public CefWindowInfoWindowsImpl(cef_window_info_t* ptr)
            : base(false)
        {
            if (CefRuntime.Platform != CefRuntimePlatform.Windows)
                throw new InvalidOperationException();

            _self = (cef_window_info_t_windows*)ptr;
        }

        internal override cef_window_info_t* GetNativePointer()
        {
            return (cef_window_info_t*)_self;
        }

        protected internal override void DisposeNativePointer()
        {
            cef_window_info_t_windows.Free(_self);
            _self = null;
        }

        public override IntPtr ParentHandle
        {
            get { ThrowIfDisposed(); return _self->parent_window; }
            set { ThrowIfDisposed(); _self->parent_window = value; }
        }

        public override IntPtr Handle
        {
            get { ThrowIfDisposed(); return _self->window; }
            set { ThrowIfDisposed(); _self->window = value; }
        }

        public override string Name
        {
            get { ThrowIfDisposed(); return cef_string_t.ToString(&_self->window_name); }
            set { ThrowIfDisposed(); cef_string_t.Copy(value, &_self->window_name); }
        }

        public override int X
        {
            get { ThrowIfDisposed(); return _self->x; }
            set { ThrowIfDisposed(); _self->x = value; }
        }

        public override int Y
        {
            get { ThrowIfDisposed(); return _self->y; }
            set { ThrowIfDisposed(); _self->y = value; }
        }

        public override int Width
        {
            get { ThrowIfDisposed(); return _self->width; }
            set { ThrowIfDisposed(); _self->width = value; }
        }

        public override int Height
        {
            get { ThrowIfDisposed(); return _self->height; }
            set { ThrowIfDisposed(); _self->height = value; }
        }

        public override WindowStyle Style
        {
            get { ThrowIfDisposed(); return (WindowStyle)_self->style; }
            set { ThrowIfDisposed(); _self->style = (uint)value; }
        }

        public override WindowStyleEx StyleEx
        {
            get { ThrowIfDisposed(); return (WindowStyleEx)_self->ex_style; }
            set { ThrowIfDisposed(); _self->ex_style = (uint)value; }
        }

        public override IntPtr MenuHandle
        {
            get { ThrowIfDisposed(); return _self->menu; }
            set { ThrowIfDisposed(); _self->menu = value; }
        }

        public override bool Hidden
        {
            get { return default(bool); }
            set { }
        }

        public override bool WindowlessRenderingEnabled
        {
            get { ThrowIfDisposed(); return _self->windowless_rendering_enabled != 0; }
            set { ThrowIfDisposed(); _self->windowless_rendering_enabled = value ? 1 : 0; }
        }

        public override bool TransparentPaintingEnabled
        {
            get { ThrowIfDisposed(); return _self->transparent_painting_enabled != 0; }
            set { ThrowIfDisposed(); _self->transparent_painting_enabled = value ? 1 : 0; }
        }
    }
}

#endif