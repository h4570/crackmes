using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Keygen
{

    // '     DO NOT REMOVE CREDITS! IF YOU USE PLEASE CREDIT!     ''

    /// <summary>
    /// LogIn GDI+ Theme
    /// Creator: Xertz (HF)
    /// Version: 1.4
    /// Control Count: 28
    /// Date Created: 18/12/2013
    /// Date Changed: 07/09/2014
    /// UID: 1602992
    /// For any bugs / errors, PM me.
    /// </summary>
    /// <remarks></remarks>

    static class DrawHelpers
    {

        #region Functions

        public static GraphicsPath RoundRectangle(Rectangle Rectangle, int Curve)
        {
            var P = new GraphicsPath();
            int ArcRectangleWidth = Curve * 2;
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90f);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90f);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0f, 90f);
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90f, 90f);
            P.AddLine(new Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), new Point(Rectangle.X, Curve + Rectangle.Y));
            return P;
        }

        public static GraphicsPath RoundRect(float x, float y, float w, float h, float r = 0.3f, bool TL = true, bool TR = true, bool BR = true, bool BL = true)
        {
            GraphicsPath RoundRectRet = default;
            float d = Math.Min(w, h) * r;
            float xw = x + w;
            float yh = y + h;
            RoundRectRet = new GraphicsPath();
            if (TL)
                RoundRectRet.AddArc(x, y, d, d, 180f, 90f);
            else
                RoundRectRet.AddLine(x, y, x, y);
            if (TR)
                RoundRectRet.AddArc(xw - d, y, d, d, 270f, 90f);
            else
                RoundRectRet.AddLine(xw, y, xw, y);
            if (BR)
                RoundRectRet.AddArc(xw - d, yh - d, d, d, 0f, 90f);
            else
                RoundRectRet.AddLine(xw, yh, xw, yh);
            if (BL)
                RoundRectRet.AddArc(x, yh - d, d, d, 90f, 90f);
            else
                RoundRectRet.AddLine(x, yh, x, yh);
            RoundRectRet.CloseFigure();
            return RoundRectRet;
        }

        public enum MouseState : byte
        {
            None = 0,
            Over = 1,
            Down = 2,
            Block = 3
        }

        #endregion

    }

    public class LogInThemeContainer : ContainerControl
    {

        #region Declarations
        private __CloseChoice _CloseChoice = __CloseChoice.Form;
        private int _FontSize = 12;
        private readonly Font _Font;
        private int MouseXLoc;
        private int MouseYLoc;
        private bool CaptureMovement = false;
        private const int MoveHeight = 35;
        private Point MouseP = new Point(0, 0);
        private Color _FontColour = Color.FromArgb(255, 255, 255);
        private Color _BaseColour = Color.FromArgb(35, 35, 35);
        private Color _ContainerColour = Color.FromArgb(54, 54, 54);
        private Color _BorderColour = Color.FromArgb(60, 60, 60);
        private Color _HoverColour = Color.FromArgb(42, 42, 42);
        #endregion

        #region Size Handling

        private int _LockWidth;
        protected int LockWidth
        {
            get
            {
                return _LockWidth;
            }
            set
            {
                _LockWidth = value;
                if (!(LockWidth == 0) && IsHandleCreated)
                    Width = LockWidth;
            }
        }

        private int _LockHeight;
        protected int LockHeight
        {
            get
            {
                return _LockHeight;
            }
            set
            {
                _LockHeight = value;
                if (!(LockHeight == 0) && IsHandleCreated)
                    Height = LockHeight;
            }
        }

        private Rectangle Frame;
        protected sealed override void OnSizeChanged(EventArgs e)
        {
            if (_Movable && !_ControlMode)
            {
                Frame = new Rectangle(7, 7, Width - 14, 35);
            }

            Invalidate();

            base.OnSizeChanged(e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (!(_LockWidth == 0))
                width = _LockWidth;
            if (!(_LockHeight == 0))
                height = _LockHeight;
            base.SetBoundsCore(x, y, width, height, specified);
        }

        #endregion

        #region State Handling

        private DrawHelpers.MouseState State = DrawHelpers.MouseState.None;
        private void SetState(DrawHelpers.MouseState current)
        {
            State = current;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized))
            {
                if (_Sizable && !_ControlMode)
                    InvalidateMouse();
            }
            base.OnMouseMove(e);
            SetState(DrawHelpers.MouseState.Over);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
                SetState(DrawHelpers.MouseState.None);
            else
                SetState(DrawHelpers.MouseState.Block);
            base.OnEnabledChanged(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            SetState(DrawHelpers.MouseState.Over);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            SetState(DrawHelpers.MouseState.Over);
            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            SetState(DrawHelpers.MouseState.None);
            if (GetChildAtPoint(PointToClient(MousePosition)) != null)
            {
                if (_Sizable && !_ControlMode)
                {
                    Cursor = Cursors.Default;
                    Previous = 0;
                }
            }
            base.OnMouseLeave(e);
        }

        private Point GetMouseLocation;
        private Size OldSize;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                SetState(DrawHelpers.MouseState.Down);
            if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized || _ControlMode))
            {
                if (_Movable && Frame.Contains(e.Location))
                {
                    Capture = false;
                    WM_LMBUTTONDOWN = true;
                    DefWndProc(ref Messages[0]);
                }
                else if (_Sizable && !(Previous == 0))
                {
                    Capture = false;
                    WM_LMBUTTONDOWN = true;
                    DefWndProc(ref Messages[Previous]);
                }
            }
            GetMouseLocation = PointToClient(MousePosition);
            if (GetMouseLocation.X > Width - 39 && GetMouseLocation.X < Width - 16 && GetMouseLocation.Y < 22)
            {
                if (_AllowClose)
                {
                    if (_CloseChoice == __CloseChoice.Application)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        ParentForm.Close();
                    }
                }
            }
            else if (GetMouseLocation.X > Width - 64 && GetMouseLocation.X < Width - 41 && GetMouseLocation.Y < 22)
            {
                if (_AllowMaximize)
                {
                    switch (FindForm().WindowState)
                    {
                        case FormWindowState.Maximized:
                            {
                                FindForm().WindowState = FormWindowState.Normal;
                                break;
                            }
                        case FormWindowState.Normal:
                            {
                                OldSize = Size;
                                FindForm().WindowState = FormWindowState.Maximized;
                                break;
                            }
                    }
                }
            }
            else if (GetMouseLocation.X > Width - 89 && GetMouseLocation.X < Width - 66 && GetMouseLocation.Y < 22)
            {
                if (_AllowMinimize)
                {
                    switch (FindForm().WindowState)
                    {
                        case FormWindowState.Normal:
                            {
                                OldSize = Size;
                                FindForm().WindowState = FormWindowState.Minimized;
                                break;
                            }
                        case FormWindowState.Maximized:
                            {
                                FindForm().WindowState = FormWindowState.Minimized;
                                break;
                            }
                    }
                }
            }
            base.OnMouseDown(e);
        }

        private Message[] Messages = new Message[9];
        private void InitializeMessages()
        {
            Messages[0] = Message.Create(Parent.Handle, 161, new IntPtr(2), IntPtr.Zero);
            for (int I = 1; I <= 8; I++)
                Messages[I] = Message.Create(Parent.Handle, 161, new IntPtr(I + 9), IntPtr.Zero);
        }

        private Point GetIndexPoint;
        private bool B1, B2, B3, B4;
        private int GetMouseIndex()
        {
            GetIndexPoint = PointToClient(MousePosition);
            B1 = GetIndexPoint.X < 6;
            B2 = GetIndexPoint.X > Width - 6;
            B3 = GetIndexPoint.Y < 6;
            B4 = GetIndexPoint.Y > Height - 6;
            if (B1 && B3)
                return 4;
            if (B1 && B4)
                return 7;
            if (B2 && B3)
                return 5;
            if (B2 && B4)
                return 8;
            if (B1)
                return 1;
            if (B2)
                return 2;
            if (B3)
                return 3;
            if (B4)
                return 6;
            return 0;
        }

        private int Current, Previous;
        private void InvalidateMouse()
        {
            Current = GetMouseIndex();
            if (Current == Previous)
                return;
            Previous = Current;
            switch (Previous)
            {
                case 0:
                    {
                        Cursor = Cursors.Default;
                        break;
                    }
                case 1:
                case 2:
                    {
                        Cursor = Cursors.SizeWE;
                        break;
                    }
                case 3:
                case 6:
                    {
                        Cursor = Cursors.SizeNS;
                        break;
                    }
                case 4:
                case 8:
                    {
                        Cursor = Cursors.SizeNWSE;
                        break;
                    }
                case 5:
                case 7:
                    {
                        Cursor = Cursors.SizeNESW;
                        break;
                    }
            }
        }

        private bool WM_LMBUTTONDOWN;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (WM_LMBUTTONDOWN && m.Msg == 513)
            {
                WM_LMBUTTONDOWN = false;

                SetState(DrawHelpers.MouseState.Over);
                if (!_SmartBounds)
                    return;

                if (IsParentMdi)
                {
                    CorrectBounds(new Rectangle(Point.Empty, Parent.Parent.Size));
                }
                else
                {
                    try
                    {
                        CorrectBounds(Screen.FromControl(Parent).WorkingArea);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void CorrectBounds(Rectangle bounds)
        {
            if (Parent.Width > bounds.Width)
                Parent.Width = bounds.Width;
            if (Parent.Height > bounds.Height)
                Parent.Height = bounds.Height;
            int X = Parent.Location.X;
            int Y = Parent.Location.Y;
            if (X < bounds.X)
                X = bounds.X;
            if (Y < bounds.Y)
                Y = bounds.Y;
            int Width = bounds.X + bounds.Width;
            int Height = bounds.Y + bounds.Height;
            if (X + Parent.Width > Width)
                X = Width - Parent.Width;
            if (Y + Parent.Height > Height)
                Y = Height - Parent.Height;
            // 'Weird allows proper full screen
            // Parent.Size = New Size(Width, Height)
            if (FindForm().WindowState == FormWindowState.Maximized | FindForm().WindowState == FormWindowState.Minimized)
            {
                Parent.Size = OldSize;
            }
        }

        protected sealed override void OnHandleCreated(EventArgs e)
        {
            if (!(_LockWidth == 0))
                Width = _LockWidth;
            if (!(_LockHeight == 0))
                Height = _LockHeight;
            if (!_ControlMode)
                base.Dock = DockStyle.Fill;
        }

        protected sealed override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent is null)
                return;
            _IsParentForm = Parent is Form;
            if (!_ControlMode)
            {
                InitializeMessages();
                Parent.BackColor = BackColor;
            }
        }

        #endregion

        #region Properties

        public enum __CloseChoice
        {
            Form,
            Application
        }
        public __CloseChoice CloseChoice
        {
            get
            {
                return _CloseChoice;
            }
            set
            {
                _CloseChoice = value;
            }
        }

        private bool _Movable = true;
        public bool Movable
        {
            get
            {
                return _Movable;
            }
            set
            {
                _Movable = value;
            }
        }

        private bool _Sizable = true;
        public bool Sizable
        {
            get
            {
                return _Sizable;
            }
            set
            {
                _Sizable = value;
            }
        }

        private bool _ControlMode;
        protected bool ControlMode
        {
            get
            {
                return _ControlMode;
            }
            set
            {
                _ControlMode = value;

                Invalidate();
            }
        }

        private bool _SmartBounds = true;
        public bool SmartBounds
        {
            get
            {
                return _SmartBounds;
            }
            set
            {
                _SmartBounds = value;
            }
        }

        private bool _IsParentForm;
        protected bool IsParentForm
        {
            get
            {
                return _IsParentForm;
            }
        }

        protected bool IsParentMdi
        {
            get
            {
                if (Parent is null)
                    return false;
                return Parent.Parent != null;
            }
        }

        [Category("Control")]
        public int FontSize
        {
            get
            {
                return _FontSize;
            }
            set
            {
                _FontSize = value;
            }
        }

        private bool _AllowMinimize = true;
        [Category("Control")]
        public bool AllowMinimize
        {
            get
            {
                return _AllowMinimize;
            }
            set
            {
                _AllowMinimize = value;
            }
        }

        private bool _AllowMaximize = true;
        [Category("Control")]
        public bool AllowMaximize
        {
            get
            {
                return _AllowMaximize;
            }
            set
            {
                _AllowMaximize = value;
            }
        }

        private bool _ShowIcon = true;
        [Category("Control")]
        public bool ShowIcon
        {
            get
            {
                return _ShowIcon;
            }
            set
            {
                _ShowIcon = value;
                Invalidate();
            }
        }

        private bool _AllowClose = true;
        [Category("Control")]
        public bool AllowClose
        {
            get
            {
                return _AllowClose;
            }
            set
            {
                _AllowClose = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color HoverColour
        {
            get
            {
                return _HoverColour;
            }
            set
            {
                _HoverColour = value;
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color ContainerColour
        {
            get
            {
                return _ContainerColour;
            }
            set
            {
                _ContainerColour = value;
            }
        }

        [Category("Colours")]
        public Color FontColour
        {
            get
            {
                return _FontColour;
            }
            set
            {
                _FontColour = value;
            }
        }

        #endregion

        #region Draw Control

        public LogInThemeContainer()
        {
            _Font = new Font("Segoe UI", _FontSize);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            BackColor = _BaseColour;
            Dock = DockStyle.Fill;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ParentForm.FormBorderStyle = FormBorderStyle.None;
            ParentForm.AllowTransparency = false;
            ParentForm.TransparencyKey = Color.Fuchsia;
            ParentForm.FindForm().StartPosition = FormStartPosition.CenterScreen;
            Dock = DockStyle.Fill;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            var G = e.Graphics;
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.FillRectangle(new SolidBrush(_BaseColour), new Rectangle(0, 0, Width, Height));
            G.FillRectangle(new SolidBrush(_ContainerColour), new Rectangle(2, 35, Width - 4, Height - 37));
            G.DrawRectangle(new Pen(_BorderColour), new Rectangle(0, 0, Width, Height));
            Point[] ControlBoxPoints = new Point[] { new Point(Width - 90, 0), new Point(Width - 90, 22), new Point(Width - 15, 22), new Point(Width - 15, 0) };
            G.DrawLines(new Pen(_BorderColour), ControlBoxPoints);
            G.DrawLine(new Pen(_BorderColour), Width - 65, 0, Width - 65, 22);
            GetMouseLocation = PointToClient(MousePosition);
            switch (State)
            {
                case DrawHelpers.MouseState.Over:
                    {
                        if (GetMouseLocation.X > Width - 39 && GetMouseLocation.X < Width - 16 && GetMouseLocation.Y < 22)
                        {
                            G.FillRectangle(new SolidBrush(_HoverColour), new Rectangle(Width - 39, 0, 23, 22));
                        }
                        else if (GetMouseLocation.X > Width - 64 && GetMouseLocation.X < Width - 41 && GetMouseLocation.Y < 22)
                        {
                            G.FillRectangle(new SolidBrush(_HoverColour), new Rectangle(Width - 64, 0, 23, 22));
                        }
                        else if (GetMouseLocation.X > Width - 89 && GetMouseLocation.X < Width - 66 && GetMouseLocation.Y < 22)
                        {
                            G.FillRectangle(new SolidBrush(_HoverColour), new Rectangle(Width - 89, 0, 23, 22));
                        }

                        break;
                    }
            }
            G.DrawLine(new Pen(_BorderColour), Width - 40, 0, Width - 40, 22);
            // 'Close Button
            G.DrawLine(new Pen(_FontColour, 2f), Width - 33, 6, Width - 22, 16);
            G.DrawLine(new Pen(_FontColour, 2f), Width - 33, 16, Width - 22, 6);
            // 'Minimize Button
            G.DrawLine(new Pen(_FontColour), Width - 83, 16, Width - 72, 16);
            // 'Maximize Button
            G.DrawLine(new Pen(_FontColour), Width - 58, 16, Width - 47, 16);
            G.DrawLine(new Pen(_FontColour), Width - 58, 16, Width - 58, 6);
            G.DrawLine(new Pen(_FontColour), Width - 47, 16, Width - 47, 6);
            G.DrawLine(new Pen(_FontColour), Width - 58, 6, Width - 47, 6);
            G.DrawLine(new Pen(_FontColour), Width - 58, 7, Width - 47, 7);
            if (_ShowIcon)
            {
                G.DrawIcon(FindForm().Icon, new Rectangle(6, 6, 22, 22));
                G.DrawString(Text, _Font, new SolidBrush(_FontColour), new RectangleF(31f, 0f, Width - 110, 35f), new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near });
            }
            else
            {
                G.DrawString(Text, _Font, new SolidBrush(_FontColour), new RectangleF(4f, 0f, Width - 110, 35f), new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near });
            }
            G.InterpolationMode = (InterpolationMode)7;
        }

        #endregion

    }

    [DefaultEvent("TextChanged")]
    public class LogInUserTextBox : Control
    {

        #region Declarations
        private DrawHelpers.MouseState State = DrawHelpers.MouseState.None;
        private TextBox _TB;

        private TextBox TB
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _TB;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _TB = value;
            }
        }
        private Color _BaseColour = Color.FromArgb(42, 42, 42);
        private Color _TextColour = Color.FromArgb(255, 255, 255);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        #endregion

        #region TextBox Properties

        private HorizontalAlignment _TextAlign = HorizontalAlignment.Left;
        [Category("Options")]
        public HorizontalAlignment TextAlign
        {
            get
            {
                return _TextAlign;
            }
            set
            {
                _TextAlign = value;
                if (TB != null)
                {
                    TB.TextAlign = value;
                }
            }
        }
        private int _MaxLength = 32767;
        [Category("Options")]
        public int MaxLength
        {
            get
            {
                return _MaxLength;
            }
            set
            {
                _MaxLength = value;
                if (TB != null)
                {
                    TB.MaxLength = value;
                }
            }
        }
        private bool _ReadOnly;
        [Category("Options")]
        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
                if (TB != null)
                {
                    TB.ReadOnly = value;
                }
            }
        }
        private bool _UseSystemPasswordChar;
        [Category("Options")]
        public bool UseSystemPasswordChar
        {
            get
            {
                return _UseSystemPasswordChar;
            }
            set
            {
                _UseSystemPasswordChar = value;
                if (TB != null)
                {
                    TB.UseSystemPasswordChar = value;
                }
            }
        }
        private bool _Multiline;
        [Category("Options")]
        public bool Multiline
        {
            get
            {
                return _Multiline;
            }
            set
            {
                _Multiline = value;
                if (TB != null)
                {
                    TB.Multiline = value;

                    if (value)
                    {
                        TB.Height = Height - 11;
                    }
                    else
                    {
                        Height = TB.Height + 11;
                    }

                }
            }
        }
        [Category("Options")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                if (TB != null)
                {
                    TB.Text = value;
                }
            }
        }
        [Category("Options")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                if (TB != null)
                {
                    TB.Font = value;
                    TB.Location = new Point(3, 5);
                    TB.Width = Width - 35;

                    if (!_Multiline)
                    {
                        Height = TB.Height + 11;
                    }
                }
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Controls.Contains(TB))
            {
                Controls.Add(TB);
            }
        }
        private void OnBaseTextChanged(object s, EventArgs e)
        {
            Text = TB.Text;
        }
        private void OnBaseKeyDown(object s, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                TB.SelectAll();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                TB.Copy();
                e.SuppressKeyPress = true;
            }
        }
        protected override void OnResize(EventArgs e)
        {
            TB.Location = new Point(5, 5);
            TB.Width = Width - 35;

            if (_Multiline)
            {
                TB.Height = Height - 11;
            }
            else
            {
                Height = TB.Height + 11;
            }

            base.OnResize(e);
        }

        #endregion

        #region Colour Properties

        [Category("Colours")]
        public Color BackgroundColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        #endregion

        #region Mouse States

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            State = DrawHelpers.MouseState.Down;
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            State = DrawHelpers.MouseState.Over;
            TB.Focus();
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            State = DrawHelpers.MouseState.None;
            Invalidate();
        }

        #endregion

        #region Draw Control
        public LogInUserTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            TB = new TextBox();
            TB.Height = 190;
            TB.Font = new Font("Segoe UI", 10f);
            TB.Text = Text;
            TB.BackColor = Color.FromArgb(42, 42, 42);
            TB.ForeColor = Color.FromArgb(255, 255, 255);
            TB.MaxLength = _MaxLength;
            TB.Multiline = false;
            TB.ReadOnly = _ReadOnly;
            TB.UseSystemPasswordChar = _UseSystemPasswordChar;
            TB.BorderStyle = BorderStyle.None;
            TB.Location = new Point(5, 5);
            TB.Width = Width - 35;
            TB.TextChanged += OnBaseTextChanged;
            TB.KeyDown += OnBaseKeyDown;
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            var G = e.Graphics;
            GraphicsPath GP;
            var Base = new Rectangle(0, 0, Width, Height);
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.Clear(BackColor);
            TB.BackColor = Color.FromArgb(42, 42, 42);
            TB.ForeColor = Color.FromArgb(255, 255, 255);
            GP = DrawHelpers.RoundRectangle(Base, 6);
            G.FillPath(new SolidBrush(Color.FromArgb(42, 42, 42)), GP);
            G.DrawPath(new Pen(new SolidBrush(Color.FromArgb(35, 35, 35)), 2f), GP);
            GP.Dispose();
            G.FillPie(new SolidBrush(FindForm().BackColor), new Rectangle(Width - 25, Height - 23, Height + 25, Height + 25), 180f, 90f);
            G.DrawPie(new Pen(Color.FromArgb(35, 35, 35), 2f), new Rectangle(Width - 25, Height - 23, Height + 25, Height + 25), 180f, 90f);
            G.InterpolationMode = (InterpolationMode)7;
        }



        #endregion

    }

    [DefaultEvent("TextChanged")]
    public class LogInPassTextBox : Control
    {

        #region Declarations
        private DrawHelpers.MouseState State = DrawHelpers.MouseState.None;
        private TextBox _TB;

        private TextBox TB
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _TB;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _TB = value;
            }
        }
        private Color _BaseColour = Color.FromArgb(255, 255, 255);
        private Color _TextColour = Color.FromArgb(50, 50, 50);
        private Color _BorderColour = Color.FromArgb(180, 187, 205);
        #endregion

        #region TextBox Properties

        private HorizontalAlignment _TextAlign = HorizontalAlignment.Left;
        [Category("Options")]
        public HorizontalAlignment TextAlign
        {
            get
            {
                return _TextAlign;
            }
            set
            {
                _TextAlign = value;
                if (TB != null)
                {
                    TB.TextAlign = value;
                }
            }
        }
        private int _MaxLength = 32767;
        [Category("Options")]
        public int MaxLength
        {
            get
            {
                return _MaxLength;
            }
            set
            {
                _MaxLength = value;
                if (TB != null)
                {
                    TB.MaxLength = value;
                }
            }
        }
        private bool _ReadOnly;
        [Category("Options")]
        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
                if (TB != null)
                {
                    TB.ReadOnly = value;
                }
            }
        }
        private bool _UseSystemPasswordChar;
        [Category("Options")]
        public bool UseSystemPasswordChar
        {
            get
            {
                return _UseSystemPasswordChar;
            }
            set
            {
                _UseSystemPasswordChar = value;
                if (TB != null)
                {
                    TB.UseSystemPasswordChar = value;
                }
            }
        }
        private bool _Multiline;
        [Category("Options")]
        public bool Multiline
        {
            get
            {
                return _Multiline;
            }
            set
            {
                _Multiline = value;
                if (TB != null)
                {
                    TB.Multiline = value;

                    if (value)
                    {
                        TB.Height = Height - 11;
                    }
                    else
                    {
                        Height = TB.Height + 11;
                    }

                }
            }
        }
        [Category("Options")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                if (TB != null)
                {
                    TB.Text = value;
                }
            }
        }
        [Category("Options")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                if (TB != null)
                {
                    TB.Font = value;
                    TB.Location = new Point(3, 5);
                    TB.Width = Width - 35;

                    if (!_Multiline)
                    {
                        Height = TB.Height + 11;
                    }
                }
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Controls.Contains(TB))
            {
                Controls.Add(TB);
            }
        }
        private void OnBaseTextChanged(object s, EventArgs e)
        {
            Text = TB.Text;
        }
        private void OnBaseKeyDown(object s, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                TB.SelectAll();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                TB.Copy();
                e.SuppressKeyPress = true;
            }
        }
        protected override void OnResize(EventArgs e)
        {
            TB.Location = new Point(5, 5);
            TB.Width = Width - 35;

            if (_Multiline)
            {
                TB.Height = Height - 11;
            }
            else
            {
                Height = TB.Height + 11;
            }

            base.OnResize(e);
        }
        #endregion

        #region Colour Properties

        [Category("Colours")]
        public Color BackgroundColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        #endregion

        #region Mouse States

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            State = DrawHelpers.MouseState.Down;
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            State = DrawHelpers.MouseState.Over;
            TB.Focus();
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            State = DrawHelpers.MouseState.None;
            Invalidate();
        }

        #endregion

        #region Draw Control
        public LogInPassTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            TB = new TextBox();
            TB.Height = 190;
            TB.Font = new Font("Segoe UI", 10f);
            TB.Text = Text;
            TB.BackColor = Color.FromArgb(42, 42, 42);
            TB.ForeColor = Color.FromArgb(255, 255, 255);
            TB.MaxLength = _MaxLength;
            TB.Multiline = false;
            TB.ReadOnly = _ReadOnly;
            TB.UseSystemPasswordChar = _UseSystemPasswordChar;
            TB.BorderStyle = BorderStyle.None;
            TB.Location = new Point(5, 5);
            TB.Width = Width - 35;
            TB.TextChanged += OnBaseTextChanged;
            TB.KeyDown += OnBaseKeyDown;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            GraphicsPath GP;
            var Base = new Rectangle(0, 0, Width, Height);
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.Clear(BackColor);
            TB.BackColor = Color.FromArgb(42, 42, 42);
            TB.ForeColor = Color.FromArgb(255, 255, 255);
            GP = DrawHelpers.RoundRectangle(Base, 6);
            G.FillPath(new SolidBrush(Color.FromArgb(42, 42, 42)), GP);
            G.DrawPath(new Pen(new SolidBrush(Color.FromArgb(35, 35, 35)), 2f), GP);
            GP.Dispose();
            G.FillPie(new SolidBrush(FindForm().BackColor), new Rectangle(Width - 25, Height - 60, Height + 25, Height + 25), 90f, 90f);
            G.DrawPie(new Pen(Color.FromArgb(35, 35, 35), 2f), new Rectangle(Width - 25, Height - 60, Height + 25, Height + 25), 90f, 90f);
            G.FillEllipse(new SolidBrush(_TextColour), new Rectangle(10, 5, 10, 7));
            G.InterpolationMode = (InterpolationMode)7;
        }
        #endregion

    }

    public class LogInLogButton : Control
    {

        #region Declarations
        private DrawHelpers.MouseState State = DrawHelpers.MouseState.None;
        private Color _ArcColour = Color.FromArgb(43, 43, 43);
        private Color _ArrowColour = Color.FromArgb(235, 233, 234);
        private Color _ArrowBorderColour = Color.FromArgb(170, 170, 170);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _HoverColour = Color.FromArgb(0, 130, 169);
        private Color _PressedColour = Color.FromArgb(0, 145, 184);
        private Color _NormalColour = Color.FromArgb(0, 160, 199);
        #endregion

        #region Mouse States

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            State = DrawHelpers.MouseState.Down;
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            State = DrawHelpers.MouseState.Over;
            Invalidate();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            State = DrawHelpers.MouseState.Over;
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            State = DrawHelpers.MouseState.None;
            Invalidate();
        }

        #endregion

        #region Colour Properties
        [Category("Colours")]
        public Color ArcColour
        {
            get
            {
                return _ArcColour;
            }
            set
            {
                _ArcColour = value;
            }
        }
        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }
        [Category("Colours")]
        public Color ArrowColour
        {
            get
            {
                return _ArrowColour;
            }
            set
            {
                _ArrowColour = value;
            }
        }
        [Category("Colours")]
        public Color ArrowBorderColour
        {
            get
            {
                return _ArrowBorderColour;
            }
            set
            {
                _ArrowBorderColour = value;
            }
        }
        [Category("Colours")]
        public Color HoverColour
        {
            get
            {
                return _HoverColour;
            }
            set
            {
                _HoverColour = value;
            }
        }
        [Category("Colours")]
        public Color PressedColour
        {
            get
            {
                return _PressedColour;
            }
            set
            {
                _PressedColour = value;
            }
        }
        [Category("Colours")]
        public Color NormalColour
        {
            get
            {
                return _NormalColour;
            }
            set
            {
                _NormalColour = value;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Size = new Size(50, 50);
        }

        #endregion

        #region Draw Control

        public LogInLogButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Size = new Size(50, 50);
            BackColor = Color.FromArgb(54, 54, 54);
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            var G = e.Graphics;
            GraphicsPath GP = new GraphicsPath(), GP1 = new GraphicsPath();
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.Clear(BackColor);
            Point[] P = new Point[] { new Point(18, 22), new Point(28, 22), new Point(28, 18), new Point(34, 25), new Point(28, 32), new Point(28, 28), new Point(18, 28) };
            switch (State)
            {
                case DrawHelpers.MouseState.None:
                    {
                        G.FillEllipse(new SolidBrush(Color.FromArgb(56, 56, 56)), new Rectangle((int)Math.Round(3d / 2d) + 1, (int)Math.Round(3d / 2d) + 1, Width - 3 - 3, Height - 3 - 3));
                        G.DrawArc(new Pen(new SolidBrush(_ArcColour), 1 + 3), (int)Math.Round(3d / 2d) + 1, (int)Math.Round(3d / 2d) + 1, Width - 3 - 3, Height - 3 - 3, -90, 360);
                        G.DrawEllipse(new Pen(_BorderColour), new Rectangle(1, 1, Height - 3, Height - 3));
                        G.FillEllipse(new SolidBrush(_NormalColour), new Rectangle((int)Math.Round(3d / 2d) + 3, (int)Math.Round(3d / 2d) + 3, Height - 11, Height - 11));
                        G.FillPolygon(new SolidBrush(_ArrowColour), P);
                        G.DrawPolygon(new Pen(_ArrowBorderColour), P);
                        break;
                    }
                case DrawHelpers.MouseState.Over:
                    {
                        G.DrawArc(new Pen(new SolidBrush(_ArcColour), 1 + 3), (int)Math.Round(3d / 2d) + 1, (int)Math.Round(3d / 2d) + 1, Width - 3 - 3, Height - 3 - 3, -90, 360);
                        G.DrawEllipse(new Pen(_BorderColour), new Rectangle(1, 1, Height - 3, Height - 3));
                        G.FillEllipse(new SolidBrush(_HoverColour), new Rectangle(6, 6, Height - 13, Height - 13));
                        G.FillPolygon(new SolidBrush(_ArrowColour), P);
                        G.DrawPolygon(new Pen(_ArrowBorderColour), P);
                        break;
                    }
                case DrawHelpers.MouseState.Down:
                    {
                        G.DrawArc(new Pen(new SolidBrush(_ArcColour), 1 + 3), (int)Math.Round(3d / 2d) + 1, (int)Math.Round(3d / 2d) + 1, Width - 3 - 3, Height - 3 - 3, -90, 360);
                        G.DrawEllipse(new Pen(_BorderColour), new Rectangle(1, 1, Height - 3, Height - 3));
                        G.FillEllipse(new SolidBrush(_PressedColour), new Rectangle(6, 6, Height - 13, Height - 13));
                        G.FillPolygon(new SolidBrush(_ArrowColour), P);
                        G.DrawPolygon(new Pen(_ArrowBorderColour), P);
                        break;
                    }
            }
            GP.Dispose();
            GP1.Dispose();
            G.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        #endregion

    }

    [DefaultEvent("CheckedChanged")]
    public class LogInCheckBox : Control
    {

        #region Declarations
        private bool _Checked;
        private DrawHelpers.MouseState State = DrawHelpers.MouseState.None;
        private Color _CheckedColour = Color.FromArgb(173, 173, 174);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _BackColour = Color.FromArgb(42, 42, 42);
        private Color _TextColour = Color.FromArgb(255, 255, 255);
        #endregion

        #region Colour & Other Properties

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BackColour;
            }
            set
            {
                _BackColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color CheckedColour
        {
            get
            {
                return _CheckedColour;
            }
            set
            {
                _CheckedColour = value;
            }
        }

        [Category("Colours")]
        public Color FontColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        public bool Checked
        {
            get
            {
                return _Checked;
            }
            set
            {
                _Checked = value;
                Invalidate();
            }
        }

        public event CheckedChangedEventHandler CheckedChanged;

        public delegate void CheckedChangedEventHandler(object sender);
        protected override void OnClick(EventArgs e)
        {
            _Checked = !_Checked;
            CheckedChanged?.Invoke(this);
            base.OnClick(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 22;
        }
        #endregion

        #region Mouse States

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            State = DrawHelpers.MouseState.Down;
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            State = DrawHelpers.MouseState.Over;
            Invalidate();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            State = DrawHelpers.MouseState.Over;
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            State = DrawHelpers.MouseState.None;
            Invalidate();
        }

        #endregion

        #region Draw Control
        public LogInCheckBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
            Size = new Size(100, 22);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var Base = new Rectangle(0, 0, 20, 20);
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Color.FromArgb(54, 54, 54));
            g.FillRectangle(new SolidBrush(_BackColour), Base);
            g.DrawRectangle(new Pen(_BorderColour), new Rectangle(1, 1, 18, 18));
            switch (State)
            {
                case DrawHelpers.MouseState.Over:
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(50, 49, 51)), Base);
                        g.DrawRectangle(new Pen(_BorderColour), new Rectangle(1, 1, 18, 18));
                        break;
                    }
            }
            if (Checked)
            {
                Point[] P = new Point[] { new Point(4, 11), new Point(6, 8), new Point(9, 12), new Point(15, 3), new Point(17, 6), new Point(9, 16) };
                g.FillPolygon(new SolidBrush(_CheckedColour), P);
            }
            g.DrawString(Text, Font, new SolidBrush(_TextColour), new Rectangle(24, 1, Width, Height - 2), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
            g.InterpolationMode = (InterpolationMode)7;
        }
        #endregion

    }

    public class LogInNormalTextBox : Control
    {

        #region Declarations
        private DrawHelpers.MouseState State = DrawHelpers.MouseState.None;
        private TextBox _TB;

        private TextBox TB
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _TB;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _TB = value;
            }
        }
        private Color _BaseColour = Color.FromArgb(42, 42, 42);
        private Color _TextColour = Color.FromArgb(255, 255, 255);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Styles _Style = Styles.NotRounded;
        private HorizontalAlignment _TextAlign = HorizontalAlignment.Left;
        private int _MaxLength = 32767;
        private bool _ReadOnly;
        private bool _UseSystemPasswordChar;
        private bool _Multiline;
        #endregion

        #region TextBox Properties

        public enum Styles
        {
            Rounded,
            NotRounded
        }

        [Category("Options")]
        public HorizontalAlignment TextAlign
        {
            get
            {
                return _TextAlign;
            }
            set
            {
                _TextAlign = value;
                if (TB != null)
                {
                    TB.TextAlign = value;
                }
            }
        }

        [Category("Options")]
        public int MaxLength
        {
            get
            {
                return _MaxLength;
            }
            set
            {
                _MaxLength = value;
                if (TB != null)
                {
                    TB.MaxLength = value;
                }
            }
        }

        [Category("Options")]
        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
                if (TB != null)
                {
                    TB.ReadOnly = value;
                }
            }
        }

        [Category("Options")]
        public bool UseSystemPasswordChar
        {
            get
            {
                return _UseSystemPasswordChar;
            }
            set
            {
                _UseSystemPasswordChar = value;
                if (TB != null)
                {
                    TB.UseSystemPasswordChar = value;
                }
            }
        }

        [Category("Options")]
        public bool Multiline
        {
            get
            {
                return _Multiline;
            }
            set
            {
                _Multiline = value;
                if (TB != null)
                {
                    TB.Multiline = value;

                    if (value)
                    {
                        TB.Height = Height - 11;
                    }
                    else
                    {
                        Height = TB.Height + 11;
                    }

                }
            }
        }

        [Category("Options")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                if (TB != null)
                {
                    TB.Text = value;
                }
            }
        }

        [Category("Options")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                if (TB != null)
                {
                    TB.Font = value;
                    TB.Location = new Point(3, 5);
                    TB.Width = Width - 6;

                    if (!_Multiline)
                    {
                        Height = TB.Height + 11;
                    }
                }
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Controls.Contains(TB))
            {
                Controls.Add(TB);
            }
        }

        private void OnBaseTextChanged(object s, EventArgs e)
        {
            Text = TB.Text;
        }

        private void OnBaseKeyDown(object s, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                TB.SelectAll();
                e.SuppressKeyPress = true;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                TB.Copy();
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            TB.Location = new Point(5, 5);
            TB.Width = Width - 10;

            if (_Multiline)
            {
                TB.Height = Height - 11;
            }
            else
            {
                Height = TB.Height + 11;
            }

            base.OnResize(e);
        }

        public Styles Style
        {
            get
            {
                return _Style;
            }
            set
            {
                _Style = value;
            }
        }

        public void SelectAll()
        {
            TB.Focus();
            TB.SelectAll();
        }


        #endregion

        #region Colour Properties

        [Category("Colours")]
        public Color BackgroundColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        #endregion

        #region Mouse States

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            State = DrawHelpers.MouseState.Down;
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            State = DrawHelpers.MouseState.Over;
            TB.Focus();
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            State = DrawHelpers.MouseState.None;
            Invalidate();
        }

        #endregion

        #region Draw Control
        public LogInNormalTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            TB = new TextBox();
            TB.Height = 190;
            TB.Font = new Font("Segoe UI", 10f);
            TB.Text = Text;
            TB.BackColor = Color.FromArgb(42, 42, 42);
            TB.ForeColor = Color.FromArgb(255, 255, 255);
            TB.MaxLength = _MaxLength;
            TB.Multiline = false;
            TB.ReadOnly = _ReadOnly;
            TB.UseSystemPasswordChar = _UseSystemPasswordChar;
            TB.BorderStyle = BorderStyle.None;
            TB.Location = new Point(5, 5);
            TB.Width = Width - 35;
            TB.TextChanged += OnBaseTextChanged;
            TB.KeyDown += OnBaseKeyDown;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            GraphicsPath GP;
            var Base = new Rectangle(0, 0, Width, Height);
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(BackColor);
            TB.BackColor = Color.FromArgb(42, 42, 42);
            TB.ForeColor = Color.FromArgb(255, 255, 255);
            switch (_Style)
            {
                case Styles.Rounded:
                    {
                        GP = DrawHelpers.RoundRectangle(Base, 6);
                        g.FillPath(new SolidBrush(Color.FromArgb(42, 42, 42)), GP);
                        g.DrawPath(new Pen(new SolidBrush(Color.FromArgb(35, 35, 35)), 2f), GP);
                        GP.Dispose();
                        break;
                    }
                case Styles.NotRounded:
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(42, 42, 42)), new Rectangle(0, 0, Width - 1, Height - 1));
                        g.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(35, 35, 35)), 2f), new Rectangle(0, 0, Width, Height));
                        break;
                    }
            }
            g.InterpolationMode = (InterpolationMode)7;
        }

        #endregion

    }

    public class LogInRadialProgressBar : Control
    {

        #region Declarations
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _BaseColour = Color.FromArgb(42, 42, 42);
        private Color _ProgressColour = Color.FromArgb(23, 119, 151);
        private int _Value = 0;
        private int _Maximum = 100;
        private int _StartingAngle = 110;
        private int _RotationAngle = 255;
        private readonly Font _Font = new Font("Segoe UI", 20f);
        #endregion

        #region Properties

        [Category("Control")]
        public int Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                switch (value)
                {
                    case var @case when @case < _Value:
                        {
                            _Value = value;
                            break;
                        }
                }
                _Maximum = value;
                Invalidate();
            }
        }

        [Category("Control")]
        public int Value
        {
            get
            {
                switch (_Value)
                {
                    case 0:
                        {
                            return 0;
                        }

                    default:
                        {
                            return _Value;
                        }
                }
            }

            set
            {
                switch (value)
                {
                    case var @case when @case > _Maximum:
                        {
                            value = _Maximum;
                            Invalidate();
                            break;
                        }
                }
                _Value = value;
                Invalidate();
            }
        }

        public void Increment(int Amount)
        {
            Value += Amount;
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color ProgressColour
        {
            get
            {
                return _ProgressColour;
            }
            set
            {
                _ProgressColour = value;
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Control")]
        public int StartingAngle
        {
            get
            {
                return _StartingAngle;
            }
            set
            {
                _StartingAngle = value;
            }
        }

        [Category("Control")]
        public int RotationAngle
        {
            get
            {
                return _RotationAngle;
            }
            set
            {
                _RotationAngle = value;
            }
        }

        #endregion

        #region Draw Control
        public LogInRadialProgressBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Size = new Size(78, 78);
            BackColor = Color.FromArgb(54, 54, 54);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            G.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.Clear(BackColor);
            switch (_Value)
            {
                case 0:
                    {
                        G.DrawArc(new Pen(new SolidBrush(_BorderColour), 1 + 5), (int)Math.Round(3d / 2d) + 1, (int)Math.Round(3d / 2d) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle - 3, _RotationAngle + 5);
                        G.DrawArc(new Pen(new SolidBrush(_BaseColour), 1 + 3), (int)Math.Round(3d / 2d) + 1, (int)Math.Round(3d / 2d) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, _RotationAngle);
                        G.DrawString(_Value.ToString(), _Font, Brushes.White, new Point((int)Math.Round(Width / 2d), (int)Math.Round(Height / 2d - 1d)), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        break;
                    }
                case var @case when @case == _Maximum:
                    {
                        G.DrawArc(new Pen(new SolidBrush(_BorderColour), 1 + 5), (int)Math.Round(3d / 2d) + 1, (int)Math.Round(3d / 2d) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle - 3, _RotationAngle + 5);
                        G.DrawArc(new Pen(new SolidBrush(_BaseColour), 1 + 3), (int)Math.Round(3d / 2d) + 1, (int)Math.Round(3d / 2d) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, _RotationAngle);
                        G.DrawArc(new Pen(new SolidBrush(_ProgressColour), 1 + 3), (int)Math.Round(3d / 2d) + 1, (int)Math.Round(3d / 2d) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, _RotationAngle);
                        G.DrawString(_Value.ToString(), _Font, Brushes.White, new Point((int)Math.Round(Width / 2d), (int)Math.Round(Height / 2d - 1d)), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        break;
                    }

                default:
                    {
                        G.DrawArc(new Pen(new SolidBrush(_BorderColour), 1 + 5), (int)Math.Round(3d / 2d) + 1, (int)Math.Round(3d / 2d) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle - 3, _RotationAngle + 5);
                        G.DrawArc(new Pen(new SolidBrush(_BaseColour), 1 + 3), (int)Math.Round(3d / 2d) + 1, (int)Math.Round(3d / 2d) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, _RotationAngle);
                        G.DrawArc(new Pen(new SolidBrush(_ProgressColour), 1 + 3), (int)Math.Round(3d / 2d) + 1, (int)Math.Round(3d / 2d) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, (int)Math.Round(_RotationAngle / (double)_Maximum * _Value));
                        G.DrawString(_Value.ToString(), _Font, Brushes.White, new Point((int)Math.Round(Width / 2d), (int)Math.Round(Height / 2d - 1d)), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        break;
                    }
            }
            G.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }
        #endregion

    }

    [DefaultEvent("CheckedChanged")]
    public class LogInRadioButton : Control
    {

        #region Declarations
        private bool _Checked;
        private DrawHelpers.MouseState State = DrawHelpers.MouseState.None;
        private Color _HoverColour = Color.FromArgb(50, 49, 51);
        private Color _CheckedColour = Color.FromArgb(173, 173, 174);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _BackColour = Color.FromArgb(54, 54, 54);
        private Color _TextColour = Color.FromArgb(255, 255, 255);
        #endregion

        #region Colour & Other Properties

        [Category("Colours")]
        public Color HighlightColour
        {
            get
            {
                return _HoverColour;
            }
            set
            {
                _HoverColour = value;
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BackColour;
            }
            set
            {
                _BackColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color CheckedColour
        {
            get
            {
                return _CheckedColour;
            }
            set
            {
                _CheckedColour = value;
            }
        }

        [Category("Colours")]
        public Color FontColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        public event CheckedChangedEventHandler CheckedChanged;

        public delegate void CheckedChangedEventHandler(object sender);
        public bool Checked
        {
            get
            {
                return _Checked;
            }
            set
            {
                _Checked = value;
                InvalidateControls();
                CheckedChanged?.Invoke(this);
                Invalidate();
            }
        }

        protected override void OnClick(EventArgs e)
        {
            if (!_Checked)
                Checked = true;
            base.OnClick(e);
        }
        private void InvalidateControls()
        {
            if (!IsHandleCreated || !_Checked)
                return;
            foreach (Control C in Parent.Controls)
            {
                if (!ReferenceEquals(C, this) && C is LogInRadioButton)
                {
                    ((LogInRadioButton)C).Checked = false;
                    Invalidate();
                }
            }
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            InvalidateControls();
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 22;
        }
        #endregion

        #region Mouse States

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            State = DrawHelpers.MouseState.Down;
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            State = DrawHelpers.MouseState.Over;
            Invalidate();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            State = DrawHelpers.MouseState.Over;
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            State = DrawHelpers.MouseState.None;
            Invalidate();
        }

        #endregion

        #region Draw Control
        public LogInRadioButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
            Size = new Size(100, 22);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            var Base = new Rectangle(1, 1, Height - 2, Height - 2);
            var Circle = new Rectangle(6, 6, Height - 12, Height - 12);
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.Clear(_BackColour);
            G.FillEllipse(new SolidBrush(_BackColour), Base);
            G.DrawEllipse(new Pen(_BorderColour, 2f), Base);
            if (Checked)
            {
                switch (State)
                {
                    case DrawHelpers.MouseState.Over:
                        {
                            G.FillEllipse(new SolidBrush(_HoverColour), new Rectangle(2, 2, Height - 4, Height - 4));
                            break;
                        }
                }
                G.FillEllipse(new SolidBrush(_CheckedColour), Circle);
            }
            else
            {
                switch (State)
                {
                    case DrawHelpers.MouseState.Over:
                        {
                            G.FillEllipse(new SolidBrush(_HoverColour), new Rectangle(2, 2, Height - 4, Height - 4));
                            break;
                        }
                }
            }
            G.DrawString(Text, Font, new SolidBrush(_TextColour), new Rectangle(24, 3, Width, Height), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
            G.InterpolationMode = (InterpolationMode)7;
        }
        #endregion

    }

    public class LogInLabel : Label
    {

        #region Declaration
        private Color _FontColour = Color.FromArgb(255, 255, 255);
        #endregion

        #region Property & Event

        [Category("Colours")]
        public Color FontColour
        {
            get
            {
                return _FontColour;
            }
            set
            {
                _FontColour = value;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        #endregion

        #region Draw Control

        public LogInLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Font = new Font("Segoe UI", 9f);
            ForeColor = _FontColour;
            BackColor = Color.Transparent;
            Text = Text;
        }

        #endregion

    }

    public class LogInButton : Control
    {

        #region Declarations
        private readonly Font _Font = new Font("Segoe UI", 9f);
        private Color _ProgressColour = Color.FromArgb(0, 191, 255);
        private Color _BorderColour = Color.FromArgb(25, 25, 25);
        private Color _FontColour = Color.FromArgb(255, 255, 255);
        private Color _MainColour = Color.FromArgb(42, 42, 42);
        private Color _HoverColour = Color.FromArgb(52, 52, 52);
        private Color _PressedColour = Color.FromArgb(47, 47, 47);
        private DrawHelpers.MouseState State = new DrawHelpers.MouseState();
        #endregion

        #region Mouse States

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            State = DrawHelpers.MouseState.Down;
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            State = DrawHelpers.MouseState.Over;
            Invalidate();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            State = DrawHelpers.MouseState.Over;
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            State = DrawHelpers.MouseState.None;
            Invalidate();
        }

        #endregion

        #region Properties

        [Category("Colours")]
        public Color ProgressColour
        {
            get
            {
                return _ProgressColour;
            }
            set
            {
                _ProgressColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color FontColour
        {
            get
            {
                return _FontColour;
            }
            set
            {
                _FontColour = value;
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _MainColour;
            }
            set
            {
                _MainColour = value;
            }
        }

        [Category("Colours")]
        public Color HoverColour
        {
            get
            {
                return _HoverColour;
            }
            set
            {
                _HoverColour = value;
            }
        }

        [Category("Colours")]
        public Color PressedColour
        {
            get
            {
                return _PressedColour;
            }
            set
            {
                _PressedColour = value;
            }
        }

        #endregion

        #region Draw Control
        public LogInButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Size = new Size(75, 30);
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.InterpolationMode = (InterpolationMode)7;
            G.Clear(BackColor);
            switch (State)
            {
                case DrawHelpers.MouseState.None:
                    {
                        G.FillRectangle(new SolidBrush(_MainColour), new Rectangle(0, 0, Width, Height));
                        G.DrawRectangle(new Pen(_BorderColour, 2f), new Rectangle(0, 0, Width, Height));
                        G.DrawString(Text, _Font, Brushes.White, new Point((int)Math.Round(Width / 2d), (int)Math.Round(Height / 2d)), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        break;
                    }
                case DrawHelpers.MouseState.Over:
                    {
                        G.FillRectangle(new SolidBrush(_HoverColour), new Rectangle(0, 0, Width, Height));
                        G.DrawRectangle(new Pen(_BorderColour, 1f), new Rectangle(1, 1, Width - 2, Height - 2));
                        G.DrawString(Text, _Font, Brushes.White, new Point((int)Math.Round(Width / 2d), (int)Math.Round(Height / 2d)), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        break;
                    }
                case DrawHelpers.MouseState.Down:
                    {
                        G.FillRectangle(new SolidBrush(_PressedColour), new Rectangle(0, 0, Width, Height));
                        G.DrawRectangle(new Pen(_BorderColour, 1f), new Rectangle(1, 1, Width - 2, Height - 2));
                        G.DrawString(Text, _Font, Brushes.White, new Point((int)Math.Round(Width / 2d), (int)Math.Round(Height / 2d)), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        break;
                    }
            }
        }

        #endregion

    }

    public class LogInButtonWithProgress : Control
    {

        #region Declarations
        private int _Value = 0;
        private int _Maximum = 100;
        private Font _Font = new Font("Segoe UI", 9f);
        private Color _ProgressColour = Color.FromArgb(0, 191, 255);
        private Color _BorderColour = Color.FromArgb(25, 25, 25);
        private Color _FontColour = Color.FromArgb(255, 255, 255);
        private Color _MainColour = Color.FromArgb(42, 42, 42);
        private Color _HoverColour = Color.FromArgb(52, 52, 52);
        private Color _PressedColour = Color.FromArgb(47, 47, 47);
        private DrawHelpers.MouseState State = new DrawHelpers.MouseState();
        #endregion

        #region Mouse States

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            State = DrawHelpers.MouseState.Down;
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            State = DrawHelpers.MouseState.Over;
            Invalidate();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            State = DrawHelpers.MouseState.Over;
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            State = DrawHelpers.MouseState.None;
            Invalidate();
        }

        #endregion

        #region Properties

        [Category("Colours")]
        public Color ProgressColour
        {
            get
            {
                return _ProgressColour;
            }
            set
            {
                _ProgressColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color FontColour
        {
            get
            {
                return _FontColour;
            }
            set
            {
                _FontColour = value;
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _MainColour;
            }
            set
            {
                _MainColour = value;
            }
        }

        [Category("Colours")]
        public Color HoverColour
        {
            get
            {
                return _HoverColour;
            }
            set
            {
                _HoverColour = value;
            }
        }

        [Category("Colours")]
        public Color PressedColour
        {
            get
            {
                return _PressedColour;
            }
            set
            {
                _PressedColour = value;
            }
        }

        [Category("Control")]
        public int Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                switch (value)
                {
                    case var @case when @case < _Value:
                        {
                            _Value = value;
                            break;
                        }
                }
                _Maximum = value;
                Invalidate();
            }
        }

        [Category("Control")]
        public int Value
        {
            get
            {
                switch (_Value)
                {
                    case 0:
                        {
                            return 0;
                        }

                    default:
                        {
                            return _Value;
                        }

                }
            }
            set
            {
                switch (value)
                {
                    case var @case when @case > _Maximum:
                        {
                            value = _Maximum;
                            Invalidate();
                            break;
                        }
                }
                _Value = value;
                Invalidate();
            }
        }

        public void Increment(int Amount)
        {
            Value += Amount;
        }

        #endregion

        #region Draw Control
        public LogInButtonWithProgress()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Size = new Size(75, 30);
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(BackColor);
            switch (State)
            {
                case DrawHelpers.MouseState.None:
                    {
                        g.FillRectangle(new SolidBrush(_MainColour), new Rectangle(0, 0, Width, Height - 4));
                        g.DrawRectangle(new Pen(_BorderColour, 2f), new Rectangle(0, 0, Width, Height - 4));
                        g.DrawString(Text, _Font, Brushes.White, new Point((int)Math.Round(Width / 2d), (int)Math.Round(Height / 2d - 2d)), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        break;
                    }
                case DrawHelpers.MouseState.Over:
                    {
                        g.FillRectangle(new SolidBrush(_HoverColour), new Rectangle(0, 0, Width, Height - 4));
                        g.DrawRectangle(new Pen(_BorderColour, 1f), new Rectangle(1, 1, Width - 2, Height - 5));
                        g.DrawString(Text, _Font, Brushes.White, new Point((int)Math.Round(Width / 2d), (int)Math.Round(Height / 2d - 2d)), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        break;
                    }
                case DrawHelpers.MouseState.Down:
                    {
                        g.FillRectangle(new SolidBrush(_PressedColour), new Rectangle(0, 0, Width, Height - 4));
                        g.DrawRectangle(new Pen(_BorderColour, 1f), new Rectangle(1, 1, Width - 2, Height - 5));
                        g.DrawString(Text, _Font, Brushes.White, new Point((int)Math.Round(Width / 2d), (int)Math.Round(Height / 2d - 2d)), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        break;
                    }
            }
            switch (_Value)
            {
                case 0:
                    {
                        break;
                    }
                case var @case when @case == _Maximum:
                    {
                        g.FillRectangle(new SolidBrush(_ProgressColour), new Rectangle(0, Height - 4, Width, Height - 4));
                        g.DrawRectangle(new Pen(_BorderColour, 2f), new Rectangle(0, 0, Width, Height));
                        break;
                    }

                default:
                    {
                        g.FillRectangle(new SolidBrush(_ProgressColour), new Rectangle(0, Height - 4, (int)Math.Round(Width / (double)_Maximum * _Value), Height - 4));
                        g.DrawRectangle(new Pen(_BorderColour, 2f), new Rectangle(0, 0, Width, Height));
                        break;
                    }
            }
            g.InterpolationMode = (InterpolationMode)7;
        }

        #endregion

    }

    public class LogInGroupBox : ContainerControl
    {

        #region Declarations
        private Color _MainColour = Color.FromArgb(47, 47, 47);
        private Color _HeaderColour = Color.FromArgb(42, 42, 42);
        private Color _TextColour = Color.FromArgb(255, 255, 255);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        #endregion

        #region Properties

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        [Category("Colours")]
        public Color HeaderColour
        {
            get
            {
                return _HeaderColour;
            }
            set
            {
                _HeaderColour = value;
            }
        }

        [Category("Colours")]
        public Color MainColour
        {
            get
            {
                return _MainColour;
            }
            set
            {
                _MainColour = value;
            }
        }

        #endregion

        #region Draw Control
        public LogInGroupBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Size = new Size(160, 110);
            Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Color.FromArgb(54, 54, 54));
            g.FillRectangle(new SolidBrush(_MainColour), new Rectangle(0, 28, Width, Height));
            g.FillRectangle(new SolidBrush(_HeaderColour), new Rectangle(0, 0, (int)Math.Round(g.MeasureString(Text, Font).Width + 7f), 28));
            g.DrawString(Text, Font, new SolidBrush(_TextColour), new Point(5, 5));
            Point[] P = new Point[] { new Point(0, 0), new Point((int)Math.Round(g.MeasureString(Text, Font).Width + 7f), 0), new Point((int)Math.Round(g.MeasureString(Text, Font).Width + 7f), 28), new Point(Width - 1, 28), new Point(Width - 1, Height - 1), new Point(1, Height - 1), new Point(1, 1) };
            g.DrawLines(new Pen(_BorderColour), P);
            g.DrawLine(new Pen(_BorderColour, 2f), new Point(0, 28), new Point((int)Math.Round(g.MeasureString(Text, Font).Width + 7f), 28));
            g.InterpolationMode = (InterpolationMode)7;
        }
        #endregion

    }

    public class LogInSeperator : Control
    {

        #region Declarations
        private Color _SeperatorColour = Color.FromArgb(35, 35, 35);
        private Style _Alignment = Style.Horizontal;
        private float _Thickness = 1f;
        #endregion

        #region Properties

        public enum Style
        {
            Horizontal,
            Verticle
        }

        [Category("Control")]
        public float Thickness
        {
            get
            {
                return _Thickness;
            }
            set
            {
                _Thickness = value;
            }
        }

        [Category("Control")]
        public Style Alignment
        {
            get
            {
                return _Alignment;
            }
            set
            {
                _Alignment = value;
            }
        }

        [Category("Colours")]
        public Color SeperatorColour
        {
            get
            {
                return _SeperatorColour;
            }
            set
            {
                _SeperatorColour = value;
            }
        }

        #endregion

        #region Draw Control
        public LogInSeperator()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size(20, 20);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            var Base = new Rectangle(0, 0, Width - 1, Height - 1);
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            switch (_Alignment)
            {
                case Style.Horizontal:
                    {
                        G.DrawLine(new Pen(_SeperatorColour, _Thickness), new Point(0, (int)Math.Round(Height / 2d)), new Point(Width, (int)Math.Round(Height / 2d)));
                        break;
                    }
                case Style.Verticle:
                    {
                        G.DrawLine(new Pen(_SeperatorColour, _Thickness), new Point((int)Math.Round(Width / 2d), 0), new Point((int)Math.Round(Width / 2d), Height));
                        break;
                    }
            }
            G.InterpolationMode = (InterpolationMode)7;
        }
        #endregion

    }

    public class LogInNumeric : Control
    {

        #region Variables

        private DrawHelpers.MouseState State = DrawHelpers.MouseState.None;
        private int MouseXLoc, MouseYLoc;
        private long _Value;
        private long _Minimum = 0L;
        private long _Maximum = 9999999L;
        private bool BoolValue;
        private Color _BaseColour = Color.FromArgb(42, 42, 42);
        private Color _ButtonColour = Color.FromArgb(47, 47, 47);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _SecondBorderColour = Color.FromArgb(0, 191, 255);
        private Color _FontColour = Color.FromArgb(255, 255, 255);

        #endregion

        #region Properties & Events

        public long Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value <= _Maximum & value >= _Minimum)
                    _Value = value;
                Invalidate();
            }
        }

        public long Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                if (value > _Minimum)
                    _Maximum = value;
                if (_Value > _Maximum)
                    _Value = _Maximum;
                Invalidate();
            }
        }

        public long Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                if (value < _Maximum)
                    _Minimum = value;
                if (_Value < _Minimum)
                    _Value = Minimum;
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            MouseXLoc = e.Location.X;
            MouseYLoc = e.Location.Y;
            Invalidate();
            if (e.X < Width - 47)
                Cursor = Cursors.IBeam;
            else
                Cursor = Cursors.Hand;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (MouseXLoc > Width - 47 && MouseXLoc < Width - 3)
            {
                if (MouseXLoc < Width - 23)
                {
                    if (Value + 1L <= _Maximum)
                        _Value += 1L;
                }
                else if (Value - 1L >= _Minimum)
                    _Value -= 1L;
            }
            else
            {
                BoolValue = !BoolValue;
                Focus();
            }
            Invalidate();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            try
            {
                if (BoolValue)
                    _Value = Convert.ToInt64(_Value.ToString() + e.KeyChar.ToString());
                if (_Value > _Maximum)
                    _Value = _Maximum;
                Invalidate();
            }
            catch
            {
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Back)
            {
                Value = 0L;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 24;
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color ButtonColour
        {
            get
            {
                return _ButtonColour;
            }
            set
            {
                _ButtonColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color SecondBorderColour
        {
            get
            {
                return _SecondBorderColour;
            }
            set
            {
                _SecondBorderColour = value;
            }
        }

        [Category("Colours")]
        public Color FontColour
        {
            get
            {
                return _FontColour;
            }
            set
            {
                _FontColour = value;
            }
        }

        #endregion

        #region Draw Control

        public LogInNumeric()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 10f);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var Base = new Rectangle(0, 0, Width, Height);
            var CenterSF = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(BackColor);
            g.FillRectangle(new SolidBrush(_BaseColour), Base);
            g.FillRectangle(new SolidBrush(_ButtonColour), new Rectangle(Width - 48, 0, 48, Height));
            g.DrawRectangle(new Pen(_BorderColour, 2f), Base);
            g.DrawLine(new Pen(_SecondBorderColour), new Point(Width - 48, 1), new Point(Width - 48, Height - 2));
            g.DrawLine(new Pen(_BorderColour), new Point(Width - 24, 1), new Point(Width - 24, Height - 2));
            g.DrawLine(new Pen(_FontColour), new Point(Width - 36, 7), new Point(Width - 36, 17));
            g.DrawLine(new Pen(_FontColour), new Point(Width - 31, 12), new Point(Width - 41, 12));
            g.DrawLine(new Pen(_FontColour), new Point(Width - 17, 13), new Point(Width - 7, 13));
            g.DrawString(Value.ToString(), Font, new SolidBrush(_FontColour), new Rectangle(5, 1, Width, Height), new StringFormat() { LineAlignment = StringAlignment.Center });
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        #endregion

    }

    public class LogInColourTable : ProfessionalColorTable
    {

        #region Declarations

        private Color _BackColour = Color.FromArgb(42, 42, 42);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _SelectedColour = Color.FromArgb(47, 47, 47);

        #endregion

        #region Properties

        [Category("Colours")]
        public Color SelectedColour
        {
            get
            {
                return _SelectedColour;
            }
            set
            {
                _SelectedColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color BackColour
        {
            get
            {
                return _BackColour;
            }
            set
            {
                _BackColour = value;
            }
        }

        public override Color ButtonSelectedBorder
        {
            get
            {
                return _BackColour;
            }
        }

        public override Color CheckBackground
        {
            get
            {
                return _BackColour;
            }
        }

        public override Color CheckPressedBackground
        {
            get
            {
                return _BackColour;
            }
        }

        public override Color CheckSelectedBackground
        {
            get
            {
                return _BackColour;
            }
        }

        public override Color ImageMarginGradientBegin
        {
            get
            {
                return _BackColour;
            }
        }

        public override Color ImageMarginGradientEnd
        {
            get
            {
                return _BackColour;
            }
        }

        public override Color ImageMarginGradientMiddle
        {
            get
            {
                return _BackColour;
            }
        }

        public override Color MenuBorder
        {
            get
            {
                return _BorderColour;
            }
        }

        public override Color MenuItemBorder
        {
            get
            {
                return _BackColour;
            }
        }

        public override Color MenuItemSelected
        {
            get
            {
                return _SelectedColour;
            }
        }

        public override Color SeparatorDark
        {
            get
            {
                return _BorderColour;
            }
        }

        public override Color ToolStripDropDownBackground
        {
            get
            {
                return _BackColour;
            }
        }

        #endregion

    }

    public class LogInListBox : Control
    {

        #region Variables

        private ListBox ListB;
        private string[] _Items = new[] { "" };
        private Color _BaseColour = Color.FromArgb(42, 42, 42);
        private Color _SelectedColour = Color.FromArgb(55, 55, 55);
        private Color _ListBaseColour = Color.FromArgb(47, 47, 47);
        private Color _TextColour = Color.FromArgb(255, 255, 255);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);

        #endregion

        #region Properties

        [Category("Control")]
        public string[] Items
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
                ListB.Items.Clear();
                ListB.Items.AddRange(value);
                Invalidate();
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color SelectedColour
        {
            get
            {
                return _SelectedColour;
            }
            set
            {
                _SelectedColour = value;
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color ListBaseColour
        {
            get
            {
                return _ListBaseColour;
            }
            set
            {
                _ListBaseColour = value;
            }
        }

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        public string SelectedItem
        {
            get
            {
                return Convert.ToString(ListB.SelectedItem);
            }
        }

        public int SelectedIndex
        {
            get
            {
                return ListB.SelectedIndex;
                if (ListB.SelectedIndex < 0)
                    return default;
            }
        }

        public void Clear()
        {
            ListB.Items.Clear();
        }

        public void ClearSelected()
        {
            for (int i = ListB.SelectedItems.Count - 1; i >= 0; i -= 1)
                ListB.Items.Remove(ListB.SelectedItems[i]);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Controls.Contains(ListB))
            {
                Controls.Add(ListB);
            }
        }

        public void AddRange(object[] items)
        {
            ListB.Items.Remove("");
            ListB.Items.AddRange(items);
        }

        public void AddItem(object item)
        {
            ListB.Items.Remove("");
            ListB.Items.Add(item);
        }

        #endregion

        #region Draw Control

        public void Drawitem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            e.DrawBackground();
            e.DrawFocusRectangle();
            {
                var withBlock = e.Graphics;
                withBlock.SmoothingMode = SmoothingMode.HighQuality;
                withBlock.PixelOffsetMode = PixelOffsetMode.HighQuality;
                withBlock.InterpolationMode = InterpolationMode.HighQualityBicubic;
                withBlock.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                if (e.State.ToString().IndexOf("Selected,") >= 0)
                {
                    withBlock.FillRectangle(new SolidBrush(_SelectedColour), new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 1));
                    withBlock.DrawString(" " + ListB.Items[e.Index].ToString(), new Font("Segoe UI", 9f, FontStyle.Bold), new SolidBrush(_TextColour), e.Bounds.X, e.Bounds.Y + 2);
                }
                else
                {
                    withBlock.FillRectangle(new SolidBrush(_ListBaseColour), new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                    withBlock.DrawString(" " + ListB.Items[e.Index].ToString(), new Font("Segoe UI", 8f), new SolidBrush(_TextColour), e.Bounds.X, e.Bounds.Y + 2);
                }
                withBlock.Dispose();
            }
        }

        public LogInListBox()
        {
            ListB = new ListBox();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            ListB.DrawMode = DrawMode.OwnerDrawFixed;
            ListB.ScrollAlwaysVisible = false;
            ListB.HorizontalScrollbar = false;
            ListB.BorderStyle = BorderStyle.None;
            ListB.BackColor = _BaseColour;
            ListB.Location = new Point(3, 3);
            ListB.Font = new Font("Segoe UI", 8f);
            ListB.ItemHeight = 20;
            ListB.Items.Clear();
            ListB.IntegralHeight = false;
            Size = new Size(130, 100);
            ListB.DrawItem += Drawitem;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            var Base = new Rectangle(0, 0, Width, Height);
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            G.Clear(BackColor);
            ListB.Size = new Size(Width - 6, Height - 5);
            G.FillRectangle(new SolidBrush(_BaseColour), Base);
            G.DrawRectangle(new Pen(_BorderColour, 3f), new Rectangle(0, 0, Width, Height - 1));
            G.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        #endregion

    }

    public class LogInTitledListBox : Control
    {

        #region Variables

        private ListBox ListB;
        private string[] _Items = new[] { "" };
        private Color _BaseColour = Color.FromArgb(42, 42, 42);
        private Color _SelectedColour = Color.FromArgb(55, 55, 55);
        private Color _ListBaseColour = Color.FromArgb(47, 47, 47);
        private Color _TextColour = Color.FromArgb(255, 255, 255);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Font _TitleFont = new Font("Segeo UI", 10f, FontStyle.Bold);

        #endregion

        #region Properties

        [Category("Control")]
        public Font TitleFont
        {
            get
            {
                return _TitleFont;
            }
            set
            {
                _TitleFont = value;
            }
        }

        [Category("Control")]
        public string[] Items
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
                ListB.Items.Clear();
                ListB.Items.AddRange(value);
                Invalidate();
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color SelectedColour
        {
            get
            {
                return _SelectedColour;
            }
            set
            {
                _SelectedColour = value;
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color ListBaseColour
        {
            get
            {
                return _ListBaseColour;
            }
            set
            {
                _ListBaseColour = value;
            }
        }

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        public string SelectedItem
        {
            get
            {
                return Convert.ToString(ListB.SelectedItem);
            }
        }

        public int SelectedIndex
        {
            get
            {
                return ListB.SelectedIndex;
                if (ListB.SelectedIndex < 0)
                    return default;
            }
        }

        public void Clear()
        {
            ListB.Items.Clear();
        }

        public void ClearSelected()
        {
            for (int i = ListB.SelectedItems.Count - 1; i >= 0; i -= 1)
                ListB.Items.Remove(ListB.SelectedItems[i]);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Controls.Contains(ListB))
            {
                Controls.Add(ListB);
            }
        }

        public void AddRange(object[] items)
        {
            ListB.Items.Remove("");
            ListB.Items.AddRange(items);
        }

        public void AddItem(object item)
        {
            ListB.Items.Remove("");
            ListB.Items.Add(item);
        }

        #endregion

        #region Draw Control

        public void Drawitem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            e.DrawBackground();
            e.DrawFocusRectangle();
            {
                var withBlock = e.Graphics;
                withBlock.SmoothingMode = SmoothingMode.HighQuality;
                withBlock.PixelOffsetMode = PixelOffsetMode.HighQuality;
                withBlock.InterpolationMode = InterpolationMode.HighQualityBicubic;
                withBlock.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                if (e.State.ToString().IndexOf("Selected,") >= 0)
                {
                    withBlock.FillRectangle(new SolidBrush(_SelectedColour), new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 1));
                    withBlock.DrawString(" " + ListB.Items[e.Index].ToString(), new Font("Segoe UI", 9f, FontStyle.Bold), new SolidBrush(_TextColour), e.Bounds.X, e.Bounds.Y + 2);
                }
                else
                {
                    withBlock.FillRectangle(new SolidBrush(_ListBaseColour), new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
                    withBlock.DrawString(" " + ListB.Items[e.Index].ToString(), new Font("Segoe UI", 8f), new SolidBrush(_TextColour), e.Bounds.X, e.Bounds.Y + 2);
                }
                withBlock.Dispose();
            }
        }

        public LogInTitledListBox()
        {
            ListB = new ListBox();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            ListB.DrawMode = DrawMode.OwnerDrawFixed;
            ListB.ScrollAlwaysVisible = false;
            ListB.HorizontalScrollbar = false;
            ListB.BorderStyle = BorderStyle.None;
            ListB.BackColor = BaseColour;
            ListB.Location = new Point(3, 28);
            ListB.Font = new Font("Segoe UI", 8f);
            ListB.ItemHeight = 20;
            ListB.Items.Clear();
            ListB.IntegralHeight = false;
            Size = new Size(130, 100);
            ListB.DrawItem += Drawitem;
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            var G = e.Graphics;
            var Base = new Rectangle(0, 0, Width, Height);
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            G.Clear(BackColor);
            ListB.Size = new Size(Width - 6, Height - 30);
            G.FillRectangle(new SolidBrush(BaseColour), Base);
            G.DrawRectangle(new Pen(_BorderColour, 3f), new Rectangle(0, 0, Width, Height - 1));
            G.DrawLine(new Pen(_BorderColour, 2f), new Point(0, 27), new Point(Width - 1, 27));
            G.DrawString(Text, _TitleFont, new SolidBrush(_TextColour), new Rectangle(2, 5, Width - 5, 20), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            G.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        #endregion

    }

    public class LogInContextMenu : ContextMenuStrip
    {

        #region Declarations

        private Color _FontColour = Color.FromArgb(55, 255, 255);

        #endregion

        #region Properties

        public Color FontColour
        {
            get
            {
                return _FontColour;
            }
            set
            {
                _FontColour = value;
            }
        }

        #endregion

        #region Draw Control

        public LogInContextMenu()
        {
            Renderer = new ToolStripProfessionalRenderer(new LogInColourTable());
            ShowCheckMargin = false;
            ShowImageMargin = false;
            ForeColor = Color.FromArgb(255, 255, 255);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            base.OnPaint(e);
        }

        #endregion

    }

    public class LogInProgressBar : Control
    {

        #region Declarations
        private Color _ProgressColour = Color.FromArgb(0, 160, 199);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _BaseColour = Color.FromArgb(42, 42, 42);
        private Color _FontColour = Color.FromArgb(50, 50, 50);
        private Color _SecondColour = Color.FromArgb(0, 145, 184);
        private int _Value = 0;
        private int _Maximum = 100;
        private bool _TwoColour = true;
        #endregion

        #region Properties

        public Color SecondColour
        {
            get
            {
                return _SecondColour;
            }
            set
            {
                _SecondColour = value;
            }
        }

        [Category("Control")]
        public bool TwoColour
        {
            get
            {
                return _TwoColour;
            }
            set
            {
                _TwoColour = value;
            }
        }

        [Category("Control")]
        public int Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                switch (value)
                {
                    case var @case when @case < _Value:
                        {
                            _Value = value;
                            break;
                        }
                }
                _Maximum = value;
                Invalidate();
            }
        }

        [Category("Control")]
        public int Value
        {
            get
            {
                switch (_Value)
                {
                    case 0:
                        {
                            return 0;
                            Invalidate();
                            break;
                        }

                    default:
                        {
                            return _Value;
                            Invalidate();
                            break;
                        }
                }
            }
            set
            {
                switch (value)
                {
                    case var @case when @case > _Maximum:
                        {
                            value = _Maximum;
                            Invalidate();
                            break;
                        }
                }
                _Value = value;
                Invalidate();
            }
        }

        [Category("Colours")]
        public Color ProgressColour
        {
            get
            {
                return _ProgressColour;
            }
            set
            {
                _ProgressColour = value;
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color FontColour
        {
            get
            {
                return _FontColour;
            }
            set
            {
                _FontColour = value;
            }
        }

        #endregion

        #region Events

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 25;
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            Height = 25;
        }

        public void Increment(int Amount)
        {
            Value += Amount;
        }

        #endregion

        #region Draw Control
        public LogInProgressBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            var Base = new Rectangle(0, 0, Width, Height);
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.Clear(BackColor);
            int ProgVal = (int)Math.Round(_Value / (double)_Maximum * Width);
            switch (Value)
            {
                case 0:
                    {
                        G.FillRectangle(new SolidBrush(_BaseColour), Base);
                        G.FillRectangle(new SolidBrush(_ProgressColour), new Rectangle(0, 0, ProgVal - 1, Height));
                        G.DrawRectangle(new Pen(_BorderColour, 3f), Base);
                        break;
                    }
                case var @case when @case == _Maximum:
                    {
                        G.FillRectangle(new SolidBrush(_BaseColour), Base);
                        G.FillRectangle(new SolidBrush(_ProgressColour), new Rectangle(0, 0, ProgVal - 1, Height));
                        if (_TwoColour)
                        {
                            G.SetClip(new Rectangle(0, -10, (int)Math.Round(Width * _Value / (double)_Maximum - 1d), Height - 5));
                            for (double i = 0d, loopTo = (Width - 1) * _Maximum / (double)_Value; i <= loopTo; i += 25d)
                                G.DrawLine(new Pen(new SolidBrush(_SecondColour), 7f), new Point((int)Math.Round(i), 0), new Point((int)Math.Round(i - 15d), Height));
                            G.ResetClip();
                        }
                        else
                        {
                        }
                        G.DrawRectangle(new Pen(_BorderColour, 3f), Base);
                        break;
                    }

                default:
                    {
                        G.FillRectangle(new SolidBrush(_BaseColour), Base);
                        G.FillRectangle(new SolidBrush(_ProgressColour), new Rectangle(0, 0, ProgVal - 1, Height));
                        if (_TwoColour)
                        {
                            G.SetClip(new Rectangle(0, 0, (int)Math.Round(Width * _Value / (double)_Maximum - 1d), Height - 1));
                            for (double i = 0d, loopTo1 = (Width - 1) * _Maximum / (double)_Value; i <= loopTo1; i += 25d)
                                G.DrawLine(new Pen(new SolidBrush(_SecondColour), 7f), new Point((int)Math.Round(i), 0), new Point((int)Math.Round(i - 10d), Height));
                            G.ResetClip();
                        }
                        else
                        {
                        }
                        G.DrawRectangle(new Pen(_BorderColour, 3f), Base);
                        break;
                    }
            }
            G.InterpolationMode = (InterpolationMode)7;
        }

        #endregion

    }

    public class LogInRichTextBox : Control
    {

        #region Declarations
        private RichTextBox TB;
        private Color _BaseColour = Color.FromArgb(42, 42, 42);
        private Color _TextColour = Color.FromArgb(255, 255, 255);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        #endregion

        #region Properties

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        #endregion

        #region Events

        public void AppendText(string AppendingText)
        {
            TB.Focus();
            TB.AppendText(AppendingText);
            Invalidate();
        }

        public override string Text
        {
            get
            {
                return TB.Text;
            }
            set
            {
                TB.Text = value;
                Invalidate();
            }
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            TB.BackColor = BackColor;
            Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            TB.ForeColor = ForeColor;
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            TB.Size = new Size(Width - 10, Height - 11);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            TB.Font = Font;
        }

        public void TextChanges()
        {
            TB.Text = Text;
        }

        #endregion

        #region Draw Control

        public LogInRichTextBox()
        {
            TB = new RichTextBox();
            {
                var withBlock = TB;
                withBlock.Multiline = true;
                withBlock.BackColor = _BaseColour;
                withBlock.ForeColor = _TextColour;
                withBlock.Text = string.Empty;
                withBlock.BorderStyle = BorderStyle.None;
                withBlock.Location = new Point(5, 5);
                withBlock.Font = new Font("Segeo UI", 9f);
                withBlock.Size = new Size(Width - 10, Height - 10);
            }
            Controls.Add(TB);
            Size = new Size(135, 35);
            DoubleBuffered = true;
            TextChanged += (_, __) => TextChanges();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var Base = new Rectangle(0, 0, Width - 1, Height - 1);
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(_BaseColour);
            g.DrawRectangle(new Pen(_BorderColour, 2f), ClientRectangle);
            g.InterpolationMode = (InterpolationMode)7;
        }

        #endregion

    }

    public class LogInStatusBar : Control
    {

        #region Variables
        private Color _BaseColour = Color.FromArgb(42, 42, 42);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _TextColour = Color.White;
        private Color _RectColour = Color.FromArgb(21, 117, 149);
        private bool _ShowLine = true;
        private LinesCount _LinesToShow = LinesCount.One;
        private Alignments _Alignment = Alignments.Left;
        private bool _ShowBorder = true;
        #endregion

        #region Properties

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        public enum LinesCount : int
        {
            One = 1,
            Two = 2
        }

        public enum Alignments
        {
            Left,
            Center,
            Right
        }

        [Category("Control")]
        public Alignments Alignment
        {
            get
            {
                return _Alignment;
            }
            set
            {
                _Alignment = value;
            }
        }

        [Category("Control")]
        public LinesCount LinesToShow
        {
            get
            {
                return _LinesToShow;
            }
            set
            {
                _LinesToShow = value;
            }
        }

        public bool ShowBorder
        {
            get
            {
                return _ShowBorder;
            }
            set
            {
                _ShowBorder = value;
            }
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            Dock = DockStyle.Bottom;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        [Category("Colours")]
        public Color RectangleColor
        {
            get
            {
                return _RectColour;
            }
            set
            {
                _RectColour = value;
            }
        }

        public bool ShowLine
        {
            get
            {
                return _ShowLine;
            }
            set
            {
                _ShowLine = value;
            }
        }

        #endregion

        #region Draw Control

        public LogInStatusBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 9f);
            ForeColor = Color.White;
            Size = new Size(Width, 20);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            var Base = new Rectangle(0, 0, Width, Height);
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            G.Clear(BaseColour);
            G.FillRectangle(new SolidBrush(BaseColour), Base);
            if (_ShowLine == true)
            {
                switch (_LinesToShow)
                {
                    case LinesCount.One:
                        {
                            if (_Alignment == Alignments.Left)
                            {
                                G.DrawString(Text, Font, new SolidBrush(_TextColour), new Rectangle(22, 2, Width, Height), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                            }
                            else if (_Alignment == Alignments.Center)
                            {
                                G.DrawString(Text, Font, new SolidBrush(_TextColour), new Rectangle(0, 0, Width, Height), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                            }
                            else
                            {
                                G.DrawString(Text, Font, new SolidBrush(_TextColour), new Rectangle(0, 0, Width - 5, Height), new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
                            }
                            G.FillRectangle(new SolidBrush(_RectColour), new Rectangle(5, 9, 14, 3));
                            break;
                        }
                    case LinesCount.Two:
                        {
                            if (_Alignment == Alignments.Left)
                            {
                                G.DrawString(Text, Font, new SolidBrush(_TextColour), new Rectangle(22, 2, Width, Height), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                            }
                            else if (_Alignment == Alignments.Center)
                            {
                                G.DrawString(Text, Font, new SolidBrush(_TextColour), new Rectangle(0, 0, Width, Height), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                            }
                            else
                            {
                                G.DrawString(Text, Font, new SolidBrush(_TextColour), new Rectangle(0, 0, Width - 22, Height), new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
                            }
                            G.FillRectangle(new SolidBrush(_RectColour), new Rectangle(5, 9, 14, 3));
                            G.FillRectangle(new SolidBrush(_RectColour), new Rectangle(Width - 20, 9, 14, 3));
                            break;
                        }
                }
            }
            else
            {
                G.DrawString(Text, Font, Brushes.White, new Rectangle(5, 2, Width, Height), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
            }
            if (_ShowBorder)
            {
                G.DrawLine(new Pen(_BorderColour, 2f), new Point(0, 0), new Point(Width, 0));
            }
            else
            {
            }
            G.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        #endregion

    }

    [DefaultEvent("ToggleChanged")]
    public class LogInOnOffSwitch : Control
    {

        #region Declarations

        public event ToggleChangedEventHandler ToggleChanged;

        public delegate void ToggleChangedEventHandler(object sender);
        private Toggles _Toggled = Toggles.NotToggled;
        private int MouseXLoc;
        private int ToggleLocation = 0;
        private Color _BaseColour = Color.FromArgb(42, 42, 42);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _TextColour = Color.FromArgb(255, 255, 255);
        private Color _NonToggledTextColour = Color.FromArgb(125, 125, 125);
        private Color _ToggledColour = Color.FromArgb(23, 119, 151);

        #endregion

        #region Properties & Events

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
                Invalidate();
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
                Invalidate();
            }
        }

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
                Invalidate();
            }
        }

        [Category("Colours")]
        public Color NonToggledTextColourderColour
        {
            get
            {
                return _NonToggledTextColour;
            }
            set
            {
                _NonToggledTextColour = value;
                Invalidate();
            }
        }

        [Category("Colours")]
        public Color ToggledColour
        {
            get
            {
                return _ToggledColour;
            }
            set
            {
                _ToggledColour = value;
                Invalidate();
            }
        }

        public enum Toggles
        {
            Toggled,
            NotToggled
        }

        public event ToggledChangedEventHandler ToggledChanged;

        public delegate void ToggledChangedEventHandler();

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            MouseXLoc = e.Location.X;
            Invalidate();
            if (e.X < Width - 40 && e.X > 40)
                Cursor = Cursors.IBeam;
            else
                Cursor = Cursors.Arrow;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (MouseXLoc > Width - 39)
            {
                _Toggled = Toggles.Toggled;
                ToggledValue();
            }
            else if (MouseXLoc < 39)
            {
                _Toggled = Toggles.NotToggled;
                ToggledValue();
            }
            Invalidate();
        }

        public Toggles Toggled
        {
            get
            {
                return _Toggled;
            }
            set
            {
                _Toggled = value;
                Invalidate();
            }
        }

        private void ToggledValue()
        {
            if (Convert.ToBoolean(_Toggled))
            {
                if (ToggleLocation < 100)
                {
                    ToggleLocation += 10;
                }
            }
            else if (ToggleLocation > 0)
            {
                ToggleLocation -= 10;
            }
            Invalidate();
        }

        #endregion

        #region Draw Control

        public LogInOnOffSwitch()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            BackColor = Color.FromArgb(54, 54, 54);
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            var G = e.Graphics;
            G.Clear(BackColor);
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            G.InterpolationMode = InterpolationMode.HighQualityBicubic;
            G.FillRectangle(new SolidBrush(_BaseColour), new Rectangle(0, 0, 39, Height));
            G.FillRectangle(new SolidBrush(_BaseColour), new Rectangle(Width - 40, 0, Width, Height));
            G.FillRectangle(new SolidBrush(_BaseColour), new Rectangle(38, 9, Width - 40, 5));
            Point[] P = new[] { new Point(0, 0), new Point(39, 0), new Point(39, 9), new Point(Width - 40, 9), new Point(Width - 40, 0), new Point(Width - 2, 0), new Point(Width - 2, Height - 1), new Point(Width - 40, Height - 1), new Point(Width - 40, 14), new Point(39, 14), new Point(39, Height - 1), new Point(0, Height - 1), new Point() };
            G.DrawLines(new Pen(_BorderColour, 2f), P);
            if (_Toggled == Toggles.Toggled)
            {
                G.FillRectangle(new SolidBrush(_ToggledColour), new Rectangle((int)Math.Round(Width / 2d), 10, (int)Math.Round(Width / 2d - 38d), 3));
                G.FillRectangle(new SolidBrush(_ToggledColour), new Rectangle(Width - 39, 2, 36, Height - 5));
                G.DrawString("ON", new Font("Microsoft Sans Serif", 7f, FontStyle.Bold), new SolidBrush(_TextColour), new Rectangle(2, -1, (int)Math.Round(Width - 20 + 20d / 3d), Height), new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
                G.DrawString("OFF", new Font("Microsoft Sans Serif", 7f, FontStyle.Bold), new SolidBrush(_NonToggledTextColour), new Rectangle((int)Math.Round(20d - 20d / 3d - 6d), -1, (int)Math.Round(Width - 20 + 20d / 3d), Height), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
            }
            else if (_Toggled == Toggles.NotToggled)
            {
                G.DrawString("OFF", new Font("Microsoft Sans Serif", 7f, FontStyle.Bold), new SolidBrush(_TextColour), new Rectangle((int)Math.Round(20d - 20d / 3d - 6d), -1, (int)Math.Round(Width - 20 + 20d / 3d), Height), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                G.DrawString("ON", new Font("Microsoft Sans Serif", 7f, FontStyle.Bold), new SolidBrush(_NonToggledTextColour), new Rectangle(2, -1, (int)Math.Round(Width - 20 + 20d / 3d), Height), new StringFormat() { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
            }

            G.DrawLine(new Pen(_BorderColour, 2f), new Point((int)Math.Round(Width / 2d), 0), new Point((int)Math.Round(Width / 2d), Height));
        }

        #endregion

    }

    public class LogInComboBox : ComboBox
    {

        #region Declarations
        private int _StartIndex = 0;
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _BaseColour = Color.FromArgb(42, 42, 42);
        private Color _FontColour = Color.FromArgb(255, 255, 255);
        private Color _LineColour = Color.FromArgb(23, 119, 151);
        private Color _SqaureColour = Color.FromArgb(47, 47, 47);
        private Color _ArrowColour = Color.FromArgb(30, 30, 30);
        private Color _SqaureHoverColour = Color.FromArgb(52, 52, 52);
        private DrawHelpers.MouseState State = DrawHelpers.MouseState.None;
        #endregion

        #region Properties & Events

        [Category("Colours")]
        public Color LineColour
        {
            get
            {
                return _LineColour;
            }
            set
            {
                _LineColour = value;
            }
        }

        [Category("Colours")]
        public Color SqaureColour
        {
            get
            {
                return _SqaureColour;
            }
            set
            {
                _SqaureColour = value;
            }
        }

        [Category("Colours")]
        public Color ArrowColour
        {
            get
            {
                return _ArrowColour;
            }
            set
            {
                _ArrowColour = value;
            }
        }

        [Category("Colours")]
        public Color SqaureHoverColour
        {
            get
            {
                return _SqaureHoverColour;
            }
            set
            {
                _SqaureHoverColour = value;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            State = DrawHelpers.MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            State = DrawHelpers.MouseState.None;
            Invalidate();
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color FontColour
        {
            get
            {
                return _FontColour;
            }
            set
            {
                _FontColour = value;
            }
        }

        public int StartIndex
        {
            get
            {
                return _StartIndex;
            }
            set
            {
                _StartIndex = value;
                try
                {
                    base.SelectedIndex = value;
                }
                catch
                {
                }
                Invalidate();
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Invalidate();
            OnMouseClick(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Invalidate();
            base.OnMouseUp(e);
        }

        #endregion

        #region Draw Control

        public void ReplaceItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var Rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 1, e.Bounds.Height + 1);
            try
            {
                {
                    var withBlock = e.Graphics;
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        withBlock.FillRectangle(new SolidBrush(_SqaureColour), Rect);
                        withBlock.DrawString(GetItemText(Items[e.Index]), Font, new SolidBrush(_FontColour), 1f, e.Bounds.Top + 2);
                    }
                    else
                    {
                        withBlock.FillRectangle(new SolidBrush(_BaseColour), Rect);
                        withBlock.DrawString(GetItemText(Items[e.Index]), Font, new SolidBrush(_FontColour), 1f, e.Bounds.Top + 2);
                    }
                }
            }
            catch
            {
            }
            e.DrawFocusRectangle();
            Invalidate();

        }

        public LogInComboBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            Width = 163;
            Font = new Font("Segoe UI", 10f);
            DrawItem += ReplaceItem;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(BackColor);
            try
            {
                var Square = new Rectangle(Width - 25, 0, Width, Height);
                g.FillRectangle(new SolidBrush(_BaseColour), new Rectangle(0, 0, Width - 25, Height));
                switch (State)
                {
                    case DrawHelpers.MouseState.None:
                        {
                            g.FillRectangle(new SolidBrush(_SqaureColour), Square);
                            break;
                        }
                    case DrawHelpers.MouseState.Over:
                        {
                            g.FillRectangle(new SolidBrush(_SqaureHoverColour), Square);
                            break;
                        }
                }
                g.DrawLine(new Pen(_LineColour, 2f), new Point(Width - 26, 1), new Point(Width - 26, Height - 1));
                try
                {
                    g.DrawString(Text, Font, new SolidBrush(_FontColour), new Rectangle(3, 0, Width - 20, Height), new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near });
                }
                catch
                {
                }
                g.DrawRectangle(new Pen(_BorderColour, 2f), new Rectangle(0, 0, Width, Height));
                Point[] P = new Point[] { new Point(Width - 17, 11), new Point(Width - 13, 5), new Point(Width - 9, 11) };
                g.FillPolygon(new SolidBrush(_BorderColour), P);
                g.DrawPolygon(new Pen(_ArrowColour), P);
                Point[] P1 = new Point[] { new Point(Width - 17, 15), new Point(Width - 13, 21), new Point(Width - 9, 15) };
                g.FillPolygon(new SolidBrush(_BorderColour), P1);
                g.DrawPolygon(new Pen(_ArrowColour), P1);
            }
            catch
            {
            }
            g.InterpolationMode = (InterpolationMode)7;

        }

        #endregion

    }

    public class LogInTabControl : TabControl
    {

        #region Declarations

        private Color _TextColour = Color.FromArgb(255, 255, 255);
        private Color _BackTabColour = Color.FromArgb(54, 54, 54);
        private Color _BaseColour = Color.FromArgb(35, 35, 35);
        private Color _ActiveColour = Color.FromArgb(47, 47, 47);
        private Color _BorderColour = Color.FromArgb(30, 30, 30);
        private Color _UpLineColour = Color.FromArgb(0, 160, 199);
        private Color _HorizLineColour = Color.FromArgb(23, 119, 151);
        private StringFormat CenterSF = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        #endregion

        #region Properties

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color UpLineColour
        {
            get
            {
                return _UpLineColour;
            }
            set
            {
                _UpLineColour = value;
            }
        }

        [Category("Colours")]
        public Color HorizontalLineColour
        {
            get
            {
                return _HorizLineColour;
            }
            set
            {
                _HorizLineColour = value;
            }
        }

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        [Category("Colours")]
        public Color BackTabColour
        {
            get
            {
                return _BackTabColour;
            }
            set
            {
                _BackTabColour = value;
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color ActiveColour
        {
            get
            {
                return _ActiveColour;
            }
            set
            {
                _ActiveColour = value;
            }
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            Alignment = TabAlignment.Top;
        }

        #endregion

        #region Draw Control

        public LogInTabControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 10f);
            SizeMode = TabSizeMode.Normal;
            ItemSize = new Size(240, 32);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(_BaseColour);
            try
            {
                SelectedTab.BackColor = _BackTabColour;
            }
            catch
            {
            }
            try
            {
                SelectedTab.BorderStyle = BorderStyle.FixedSingle;
            }
            catch
            {
            }
            g.DrawRectangle(new Pen(_BorderColour, 2f), new Rectangle(0, 0, Width, Height));
            for (int i = 0, loopTo = TabCount - 1; i <= loopTo; i++)
            {
                var Base = new Rectangle(new Point(GetTabRect(i).Location.X, GetTabRect(i).Location.Y), new Size(GetTabRect(i).Width, GetTabRect(i).Height));
                var BaseSize = new Rectangle(Base.Location, new Size(Base.Width, Base.Height));
                if (i == SelectedIndex)
                {
                    g.FillRectangle(new SolidBrush(_BaseColour), BaseSize);
                    g.FillRectangle(new SolidBrush(_ActiveColour), new Rectangle(Base.X + 1, Base.Y - 3, Base.Width, Base.Height + 5));
                    g.DrawString(TabPages[i].Text, Font, new SolidBrush(_TextColour), new Rectangle(Base.X + 7, Base.Y, Base.Width - 3, Base.Height), CenterSF);
                    g.DrawLine(new Pen(_HorizLineColour, 2f), new Point(Base.X + 3, (int)Math.Round(Base.Height / 2d + 2d)), new Point(Base.X + 9, (int)Math.Round(Base.Height / 2d + 2d)));
                    g.DrawLine(new Pen(_UpLineColour, 2f), new Point(Base.X + 3, Base.Y - 3), new Point(Base.X + 3, Base.Height + 5));
                }
                else
                {
                    g.DrawString(TabPages[i].Text, Font, new SolidBrush(_TextColour), BaseSize, CenterSF);
                }
            }
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        #endregion

    }

    public class LogInTrackBar : Control
    {

        #region Declaration
        private int _Maximum = 10;
        private int _Value = 0;
        private bool CaptureMovement = false;
        private Rectangle Bar;
        private Size Track = new Size(25, 14);
        private Color _TextColour = Color.FromArgb(255, 255, 255);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _BarBaseColour = Color.FromArgb(47, 47, 47);
        private Color _StripColour = Color.FromArgb(42, 42, 42);
        private Color _StripAmountColour = Color.FromArgb(23, 119, 151);
        #endregion

        #region Properties

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }

        [Category("Colours")]
        public Color BarBaseColour
        {
            get
            {
                return _BarBaseColour;
            }
            set
            {
                _BarBaseColour = value;
            }
        }

        [Category("Colours")]
        public Color StripColour
        {
            get
            {
                return _StripColour;
            }
            set
            {
                _StripColour = value;
            }
        }

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        [Category("Colours")]
        public Color StripAmountColour
        {
            get
            {
                return _StripAmountColour;
            }
            set
            {
                _StripAmountColour = value;
            }
        }

        public int Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                if (value > 0)
                    _Maximum = value;
                if (value < _Value)
                    _Value = value;
                Invalidate();
            }
        }

        public event ValueChangedEventHandler ValueChanged;

        public delegate void ValueChangedEventHandler();

        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                switch (value)
                {
                    case var @case when @case == _Value:
                        {
                            return;
                        }
                    case var case1 when case1 < 0:
                        {
                            _Value = 0;
                            break;
                        }
                    case var case2 when case2 > _Maximum:
                        {
                            _Value = _Maximum;
                            break;
                        }

                    default:
                        {
                            _Value = value;
                            break;
                        }
                }
                Invalidate();
                ValueChanged?.Invoke();
            }
        }



        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            var MovementPoint = new Rectangle(new Point(e.Location.X, e.Location.Y), new Size(1, 1));
            var Bar = new Rectangle(10, 10, Width - 21, Height - 21);
            if (new Rectangle(new Point(Bar.X + (int)Math.Round(Bar.Width * (Value / (double)Maximum)) - (int)Math.Round(Track.Width / 2d - 1d), 0), new Size(Track.Width, Height)).IntersectsWith(MovementPoint))
            {
                CaptureMovement = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            CaptureMovement = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (CaptureMovement)
            {
                var MovementPoint = new Point(e.X, e.Y);
                var Bar = new Rectangle(10, 10, Width - 21, Height - 21);
                Value = (int)Math.Round(Maximum * ((MovementPoint.X - Bar.X) / (double)Bar.Width));
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            CaptureMovement = false;
        }

        #endregion

        #region Draw Control

        public LogInTrackBar()
        {
            Bar = new Rectangle(0, 10, Width - 21, Height - 21);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.FromArgb(54, 54, 54);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            Bar = new Rectangle(13, 11, Width - 27, Height - 21);
            g.Clear(BackColor);
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.FillRectangle(new SolidBrush(_StripColour), new Rectangle(3, (int)Math.Round(Height / 2d - 4d), Width - 5, 8));
            g.DrawRectangle(new Pen(_BorderColour, 2f), new Rectangle(4, (int)Math.Round(Height / 2d - 4d), Width - 5, 8));
            g.FillRectangle(new SolidBrush(_StripAmountColour), new Rectangle(4, (int)Math.Round(Height / 2d - 4d), (int)Math.Round(Bar.Width * (Value / (double)Maximum)) + (int)Math.Round(Track.Width / 2d), 8));
            g.FillRectangle(new SolidBrush(_BarBaseColour), Bar.X + (int)Math.Round(Bar.Width * (Value / (double)Maximum)) - (int)Math.Round(Track.Width / 2d), Bar.Y + (int)Math.Round(Bar.Height / 2d) - (int)Math.Round(Track.Height / 2d), Track.Width, Track.Height);
            g.DrawRectangle(new Pen(_BorderColour, 2f), Bar.X + (int)Math.Round(Bar.Width * (Value / (double)Maximum)) - (int)Math.Round(Track.Width / 2d), Bar.Y + (int)Math.Round(Bar.Height / 2d) - (int)Math.Round(Track.Height / 2d), Track.Width, Track.Height);
            g.DrawString(_Value.ToString(), new Font("Segoe UI", 6.5f, FontStyle.Regular), new SolidBrush(_TextColour), new Rectangle(Bar.X + (int)Math.Round(Bar.Width * (Value / (double)Maximum)) - (int)Math.Round(Track.Width / 2d), Bar.Y + (int)Math.Round(Bar.Height / 2d) - (int)Math.Round(Track.Height / 2d), Track.Width - 1, Track.Height), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        }

        #endregion

    }

    [DefaultEvent("Scroll")]
    public class LogInVerticalScrollBar : Control
    {

        #region Declarations

        private int ThumbMovement;
        private Rectangle TSA;
        private Rectangle BSA;
        private Rectangle Shaft;
        private Rectangle Thumb;
        private bool ShowThumb;
        private bool ThumbPressed;
        private int _ThumbSize = 24;
        public int _Minimum = 0;
        public int _Maximum = 100;
        public int _Value = 0;
        public int _SmallChange = 1;
        private int _ButtonSize = 16;
        public int _LargeChange = 10;
        private Color _ThumbBorder = Color.FromArgb(35, 35, 35);
        private Color _LineColour = Color.FromArgb(23, 119, 151);
        private Color _ArrowColour = Color.FromArgb(37, 37, 37);
        private Color _BaseColour = Color.FromArgb(47, 47, 47);
        private Color _ThumbColour = Color.FromArgb(55, 55, 55);
        private Color _ThumbSecondBorder = Color.FromArgb(65, 65, 65);
        private Color _FirstBorder = Color.FromArgb(55, 55, 55);
        private Color _SecondBorder = Color.FromArgb(35, 35, 35);

        #endregion

        #region Properties & Events

        [Category("Colours")]
        public Color ThumbBorder
        {
            get
            {
                return _ThumbBorder;
            }
            set
            {
                _ThumbBorder = value;
            }
        }

        [Category("Colours")]
        public Color LineColour
        {
            get
            {
                return _LineColour;
            }
            set
            {
                _LineColour = value;
            }
        }

        [Category("Colours")]
        public Color ArrowColour
        {
            get
            {
                return _ArrowColour;
            }
            set
            {
                _ArrowColour = value;
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color ThumbColour
        {
            get
            {
                return _ThumbColour;
            }
            set
            {
                _ThumbColour = value;
            }
        }

        [Category("Colours")]
        public Color ThumbSecondBorder
        {
            get
            {
                return _ThumbSecondBorder;
            }
            set
            {
                _ThumbSecondBorder = value;
            }
        }

        [Category("Colours")]
        public Color FirstBorder
        {
            get
            {
                return _FirstBorder;
            }
            set
            {
                _FirstBorder = value;
            }
        }

        [Category("Colours")]
        public Color SecondBorder
        {
            get
            {
                return _SecondBorder;
            }
            set
            {
                _SecondBorder = value;
            }
        }

        public event ScrollEventHandler Scroll;

        public delegate void ScrollEventHandler(object sender);

        public int Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                _Minimum = value;
                if (value > _Value)
                    _Value = value;
                if (value > _Maximum)
                    _Maximum = value;
                InvalidateLayout();
            }
        }

        public int Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                if (value < _Value)
                    _Value = value;
                if (value < _Minimum)
                    _Minimum = value;
            }
        }

        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                switch (value)
                {
                    case var @case when @case == _Value:
                        {
                            return;
                        }
                    case var case1 when case1 < _Minimum:
                        {
                            _Value = _Minimum;
                            break;
                        }
                    case var case2 when case2 > _Maximum:
                        {
                            _Value = _Maximum;
                            break;
                        }

                    default:
                        {
                            _Value = value;
                            break;
                        }
                }
                InvalidatePosition();
                Scroll?.Invoke(this);
            }
        }

        public int SmallChange
        {
            get
            {
                return _SmallChange;
            }
            set
            {
                switch (value)
                {
                    case var @case when @case < 1:
                        {
                            break;
                        }
                    case var case1 when case1 > Convert.ToInt32(_SmallChange == value):
                        {
                            break;
                        }
                }
            }
        }

        public int LargeChange
        {
            get
            {
                return _LargeChange;
            }
            set
            {
                switch (value)
                {
                    case var @case when @case < 1:
                        {
                            break;
                        }

                    default:
                        {
                            _LargeChange = value;
                            break;
                        }
                }
            }
        }

        public int ButtonSize
        {
            get
            {
                return _ButtonSize;
            }
            set
            {
                switch (value)
                {
                    case var @case when @case < 16:
                        {
                            _ButtonSize = 16;
                            break;
                        }

                    default:
                        {
                            _ButtonSize = value;
                            break;
                        }
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            InvalidateLayout();
        }

        private void InvalidateLayout()
        {
            TSA = new Rectangle(0, 1, Width, 0);
            Shaft = new Rectangle(0, TSA.Bottom - 1, Width, Height - 3);
            ShowThumb = Convert.ToBoolean(_Maximum - _Minimum);
            if (ShowThumb)
            {
                Thumb = new Rectangle(1, 0, Width - 3, _ThumbSize);
            }
            Scroll?.Invoke(this);
            InvalidatePosition();
        }

        private void InvalidatePosition()
        {
            Thumb.Y = (int)Math.Round((_Value - _Minimum) / (double)(_Maximum - _Minimum) * (Shaft.Height - _ThumbSize) + 1d);
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ShowThumb)
            {
                if (TSA.Contains(e.Location))
                {
                    ThumbMovement = _Value - _SmallChange;
                }
                else if (BSA.Contains(e.Location))
                {
                    ThumbMovement = _Value + _SmallChange;
                }
                else if (Thumb.Contains(e.Location))
                {
                    ThumbPressed = true;
                    return;
                }
                else if (e.Y < Thumb.Y)
                {
                    ThumbMovement = _Value - _LargeChange;
                }
                else
                {
                    ThumbMovement = _Value + _LargeChange;
                }
                Value = Math.Min(Math.Max(ThumbMovement, _Minimum), _Maximum);
                InvalidatePosition();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (ThumbPressed && ShowThumb)
            {
                int ThumbPosition = e.Y - TSA.Height - _ThumbSize / 2;
                int ThumbBounds = Shaft.Height - _ThumbSize;
                ThumbMovement = (int)Math.Round(ThumbPosition / (double)ThumbBounds * (_Maximum - _Minimum)) + _Minimum;
                Value = Math.Min(Math.Max(ThumbMovement, _Minimum), _Maximum);
                InvalidatePosition();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            ThumbPressed = false;
        }

        #endregion

        #region Draw Control

        public LogInVerticalScrollBar()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Size = new Size(24, 50);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(_BaseColour);
            Point[] P = new Point[] { new Point((int)Math.Round(Width / 2d), 5), new Point((int)Math.Round(Width / 4d), 13), new Point((int)Math.Round(Width / 2d - 2d), 13), new Point((int)Math.Round(Width / 2d - 2d), Height - 13), new Point((int)Math.Round(Width / 4d), Height - 13), new Point((int)Math.Round(Width / 2d), Height - 5), new Point((int)Math.Round(Width - Width / 4d - 1d), Height - 13), new Point((int)Math.Round(Width / 2d + 2d), Height - 13), new Point((int)Math.Round(Width / 2d + 2d), 13), new Point((int)Math.Round(Width - Width / 4d - 1d), 13) };
            g.FillPolygon(new SolidBrush(_ArrowColour), P);
            g.FillRectangle(new SolidBrush(_ThumbColour), Thumb);
            g.DrawRectangle(new Pen(_ThumbBorder), Thumb);
            g.DrawRectangle(new Pen(_ThumbSecondBorder), Thumb.X + 1, Thumb.Y + 1, Thumb.Width - 2, Thumb.Height - 2);
            g.DrawLine(new Pen(_LineColour, 2f), new Point((int)Math.Round(Thumb.Width / 2d + 1d), Thumb.Y + 4), new Point((int)Math.Round(Thumb.Width / 2d + 1d), Thumb.Bottom - 4));
            g.DrawRectangle(new Pen(_FirstBorder), 0, 0, Width - 1, Height - 1);
            g.DrawRectangle(new Pen(_SecondBorder), 1, 1, Width - 3, Height - 3);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        }

        #endregion

    }

    [DefaultEvent("Scroll")]
    public class LogInHorizontalScrollBar : Control
    {

        #region Declarations

        private int ThumbMovement;
        private Rectangle LSA;
        private Rectangle RSA;
        private Rectangle Shaft;
        private Rectangle Thumb;
        private bool ShowThumb;
        private bool ThumbPressed;
        private int _ThumbSize = 24;
        private int _Minimum = 0;
        private int _Maximum = 100;
        private int _Value = 0;
        private int _SmallChange = 1;
        private int _ButtonSize = 16;
        private int _LargeChange = 10;
        private Color _ThumbBorder = Color.FromArgb(35, 35, 35);
        private Color _LineColour = Color.FromArgb(23, 119, 151);
        private Color _ArrowColour = Color.FromArgb(37, 37, 37);
        private Color _BaseColour = Color.FromArgb(47, 47, 47);
        private Color _ThumbColour = Color.FromArgb(55, 55, 55);
        private Color _ThumbSecondBorder = Color.FromArgb(65, 65, 65);
        private Color _FirstBorder = Color.FromArgb(55, 55, 55);
        private Color _SecondBorder = Color.FromArgb(35, 35, 35);
        private bool ThumbDown = false;

        #endregion

        #region Properties & Events

        [Category("Colours")]
        public Color ThumbBorder
        {
            get
            {
                return _ThumbBorder;
            }
            set
            {
                _ThumbBorder = value;
            }
        }

        [Category("Colours")]
        public Color LineColour
        {
            get
            {
                return _LineColour;
            }
            set
            {
                _LineColour = value;
            }
        }

        [Category("Colours")]
        public Color ArrowColour
        {
            get
            {
                return _ArrowColour;
            }
            set
            {
                _ArrowColour = value;
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color ThumbColour
        {
            get
            {
                return _ThumbColour;
            }
            set
            {
                _ThumbColour = value;
            }
        }

        [Category("Colours")]
        public Color ThumbSecondBorder
        {
            get
            {
                return _ThumbSecondBorder;
            }
            set
            {
                _ThumbSecondBorder = value;
            }
        }

        [Category("Colours")]
        public Color FirstBorder
        {
            get
            {
                return _FirstBorder;
            }
            set
            {
                _FirstBorder = value;
            }
        }

        [Category("Colours")]
        public Color SecondBorder
        {
            get
            {
                return _SecondBorder;
            }
            set
            {
                _SecondBorder = value;
            }
        }

        public event ScrollEventHandler Scroll;

        public delegate void ScrollEventHandler(object sender);

        public int Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                _Minimum = value;
                if (value > _Value)
                    _Value = value;
                if (value > _Maximum)
                    _Maximum = value;
                InvalidateLayout();
            }
        }

        public int Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                if (value < _Value)
                    _Value = value;
                if (value < _Minimum)
                    _Minimum = value;
            }
        }

        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                switch (value)
                {
                    case var @case when @case == _Value:
                        {
                            return;
                        }
                    case var case1 when case1 < _Minimum:
                        {
                            _Value = _Minimum;
                            break;
                        }
                    case var case2 when case2 > _Maximum:
                        {
                            _Value = _Maximum;
                            break;
                        }

                    default:
                        {
                            _Value = value;
                            break;
                        }
                }
                InvalidatePosition();
                Scroll?.Invoke(this);
            }
        }

        public int SmallChange
        {
            get
            {
                return _SmallChange;
            }
            set
            {
                switch (value)
                {
                    case var @case when @case < 1:
                        {
                            break;
                        }
                    case var case1 when case1 > Convert.ToInt32(_SmallChange == value):
                        {
                            break;
                        }
                }
            }
        }

        public int LargeChange
        {
            get
            {
                return _LargeChange;
            }
            set
            {
                switch (value)
                {
                    case var @case when @case < 1:
                        {
                            break;
                        }

                    default:
                        {
                            _LargeChange = value;
                            break;
                        }
                }
            }
        }

        public int ButtonSize
        {
            get
            {
                return _ButtonSize;
            }
            set
            {
                switch (value)
                {
                    case var @case when @case < 16:
                        {
                            _ButtonSize = 16;
                            break;
                        }

                    default:
                        {
                            _ButtonSize = value;
                            break;
                        }
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            InvalidateLayout();
        }

        private void InvalidateLayout()
        {
            LSA = new Rectangle(0, 1, 0, Height);
            Shaft = new Rectangle(LSA.Right + 1, 0, Width - 3, Height);
            ShowThumb = Convert.ToBoolean(_Maximum - _Minimum);
            Thumb = new Rectangle(0, 1, _ThumbSize, Height - 3);
            Scroll?.Invoke(this);
            InvalidatePosition();
        }

        private void InvalidatePosition()
        {
            Thumb.X = (int)Math.Round((_Value - _Minimum) / (double)(_Maximum - _Minimum) * (Shaft.Width - _ThumbSize) + 1d);
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ShowThumb)
            {
                if (LSA.Contains(e.Location))
                {
                    ThumbMovement = _Value - _SmallChange;
                }
                else if (RSA.Contains(e.Location))
                {
                    ThumbMovement = _Value + _SmallChange;
                }
                else if (Thumb.Contains(e.Location))
                {
                    ThumbDown = true;
                    return;
                }
                else if (e.X < Thumb.X)
                {
                    ThumbMovement = _Value - _LargeChange;
                }
                else
                {
                    ThumbMovement = _Value + _LargeChange;
                }
                Value = Math.Min(Math.Max(ThumbMovement, _Minimum), _Maximum);
                InvalidatePosition();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (ThumbDown && ShowThumb)
            {
                int ThumbPosition = e.X - LSA.Width - _ThumbSize / 2;
                int ThumbBounds = Shaft.Width - _ThumbSize;

                ThumbMovement = (int)Math.Round(ThumbPosition / (double)ThumbBounds * (_Maximum - _Minimum)) + _Minimum;

                Value = Math.Min(Math.Max(ThumbMovement, _Minimum), _Maximum);
                InvalidatePosition();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            ThumbDown = false;
        }

        #endregion

        #region Draw Control

        public LogInHorizontalScrollBar()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Height = 18;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Color.FromArgb(47, 47, 47));
            Point[] P = new Point[] { new Point(5, (int)Math.Round(Height / 2d)), new Point(13, (int)Math.Round(Height / 4d)), new Point(13, (int)Math.Round(Height / 2d - 2d)), new Point(Width - 13, (int)Math.Round(Height / 2d - 2d)), new Point(Width - 13, (int)Math.Round(Height / 4d)), new Point(Width - 5, (int)Math.Round(Height / 2d)), new Point(Width - 13, (int)Math.Round(Height - Height / 4d - 1d)), new Point(Width - 13, (int)Math.Round(Height / 2d + 2d)), new Point(13, (int)Math.Round(Height / 2d + 2d)), new Point(13, (int)Math.Round(Height - Height / 4d - 1d)) };
            g.FillPolygon(new SolidBrush(_ArrowColour), P);
            g.FillRectangle(new SolidBrush(_ThumbColour), Thumb);
            g.DrawRectangle(new Pen(_ThumbBorder), Thumb);
            g.DrawRectangle(new Pen(_ThumbSecondBorder), Thumb.X + 1, Thumb.Y + 1, Thumb.Width - 2, Thumb.Height - 2);
            g.DrawLine(new Pen(_LineColour, 2f), new Point(Thumb.X + 4, (int)Math.Round(Thumb.Height / 2d + 1d)), new Point(Thumb.Right - 4, (int)Math.Round(Thumb.Height / 2d + 1d)));
            g.DrawRectangle(new Pen(_FirstBorder), 0, 0, Width - 1, Height - 1);
            g.DrawRectangle(new Pen(_SecondBorder), 1, 1, Width - 3, Height - 3);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        #endregion

    }

    public class LogInTitledListBoxWBuiltInScrollBar : Control
    {

        #region Declarations

        private List<LogInListBoxItem> _Items = new List<LogInListBoxItem>();
        private readonly List<LogInListBoxItem> _SelectedItems = new List<LogInListBoxItem>();
        private bool _MultiSelect = true;
        private int ItemHeight = 24;
        private readonly LogInVerticalScrollBar VerticalScrollbar;
        private Color _BaseColour = Color.FromArgb(55, 55, 55);
        private Color _SelectedItemColour = Color.FromArgb(50, 50, 50);
        private Color _NonSelectedItemColour = Color.FromArgb(47, 47, 47);
        private Color _TitleAreaColour = Color.FromArgb(42, 42, 42);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _TextColour = Color.FromArgb(255, 255, 255);
        private int _SelectedHeight = 1;

        #endregion

        #region Properties

        [Category("Colours")]
        public Color TitleAreaColour
        {
            get
            {
                return _TitleAreaColour;
            }
            set
            {
                _TitleAreaColour = value;
            }
        }

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        [Category("Control")]
        public int SelectedHeight
        {
            get
            {
                return _SelectedHeight;
            }
            set
            {
                if (value < 1)
                {
                    _SelectedHeight = Height;
                }
                else
                {
                    _SelectedHeight = value;
                }
                InvalidateScroll();
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color SelectedItemColour
        {
            get
            {
                return _SelectedItemColour;
            }
            set
            {
                _SelectedItemColour = value;
            }
        }

        [Category("Colours")]
        public Color NonSelectedItemColour
        {
            get
            {
                return _NonSelectedItemColour;
            }
            set
            {
                _NonSelectedItemColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }


        private void HandleScroll(object sender)
        {
            Invalidate();
        }

        private void InvalidateScroll()
        {
            Debug.Print(Height.ToString());
            if ((int)Math.Round(Math.Round(_Items.Count * ItemHeight / (double)_SelectedHeight)) < _Items.Count * ItemHeight / (double)_SelectedHeight)
            {
                VerticalScrollbar._Maximum = (int)Math.Round(Math.Ceiling(_Items.Count * ItemHeight / (double)_SelectedHeight));
            }
            else if ((int)Math.Round(Math.Round(_Items.Count * ItemHeight / (double)_SelectedHeight)) == 0)
            {
                VerticalScrollbar._Maximum = 1;
            }
            else
            {
                VerticalScrollbar._Maximum = (int)Math.Round(Math.Round(_Items.Count * ItemHeight / (double)_SelectedHeight));
            }
            Invalidate();
        }

        private void InvalidateLayout()
        {
            VerticalScrollbar.Location = new Point(Width - VerticalScrollbar.Width - 2, 2);
            VerticalScrollbar.Size = new Size(18, Height - 4);
            Invalidate();
        }

        public class LogInListBoxItem
        {
            public string Text { get; set; }
            public override string ToString()
            {
                return Text;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public LogInListBoxItem[] Items
        {
            get
            {
                return _Items.ToArray();
            }
            set
            {
                _Items = new List<LogInListBoxItem>(value);
                Invalidate();
                InvalidateScroll();
            }
        }

        public LogInListBoxItem[] SelectedItems
        {
            get
            {
                return _SelectedItems.ToArray();
            }
        }

        public bool MultiSelect
        {
            get
            {
                return _MultiSelect;
            }
            set
            {
                _MultiSelect = value;

                if (_SelectedItems.Count > 1)
                {
                    _SelectedItems.RemoveRange(1, _SelectedItems.Count - 1);
                }

                Invalidate();
            }
        }

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                ItemHeight = (int)Math.Round(Graphics.FromHwnd(Handle).MeasureString("@", Font).Height);
                if (VerticalScrollbar != null)
                {
                    VerticalScrollbar._SmallChange = 1;
                    VerticalScrollbar._LargeChange = 1;

                }
                base.Font = value;
                InvalidateLayout();
            }
        }

        public void AddItem(string Items)
        {
            var Item = new LogInListBoxItem();
            Item.Text = Items;
            _Items.Add(Item);
            Invalidate();
            InvalidateScroll();
        }

        public void AddItems(string[] Items)
        {
            foreach (var I in Items)
            {
                var Item = new LogInListBoxItem();
                Item.Text = I;
                _Items.Add(Item);
            }
            Invalidate();
            InvalidateScroll();
        }

        public void RemoveItemAt(int index)
        {
            _Items.RemoveAt(index);
            Invalidate();
            InvalidateScroll();
        }

        public void RemoveItem(LogInListBoxItem item)
        {
            _Items.Remove(item);
            Invalidate();
            InvalidateScroll();
        }

        public void RemoveItems(LogInListBoxItem[] items)
        {
            foreach (LogInListBoxItem I in items)
                _Items.Remove(I);
            Invalidate();
            InvalidateScroll();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            _SelectedHeight = Height;
            InvalidateScroll();
            InvalidateLayout();
            base.OnSizeChanged(e);
        }

        private void Vertical_MouseDown(object sender, MouseEventArgs e)
        {
            Focus();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            if (e.Button == MouseButtons.Left)
            {
                int Offset = VerticalScrollbar.Value * (VerticalScrollbar.Maximum + (Height - ItemHeight));

                int Index = (e.Y + Offset) / ItemHeight;

                if (Index > _Items.Count - 1)
                    Index = -1;

                if (!(Index == -1))
                {

                    if (ModifierKeys == Keys.Control && _MultiSelect)
                    {
                        if (_SelectedItems.Contains(_Items[Index]))
                        {
                            _SelectedItems.Remove(_Items[Index]);
                        }
                        else
                        {
                            _SelectedItems.Add(_Items[Index]);
                        }
                    }
                    else
                    {
                        _SelectedItems.Clear();
                        _SelectedItems.Add(_Items[Index]);
                    }
                    Debug.Print(_SelectedItems[0].Text);
                }

                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int Move = -(e.Delta * SystemInformation.MouseWheelScrollLines / 120 * (2 / 2));
            int Value = Math.Max(Math.Min(VerticalScrollbar.Value + Move, VerticalScrollbar.Maximum), VerticalScrollbar.Minimum);
            VerticalScrollbar.Value = Value;
            base.OnMouseWheel(e);
        }

        #endregion

        #region Draw Control

        public LogInTitledListBoxWBuiltInScrollBar()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            VerticalScrollbar = new LogInVerticalScrollBar();
            VerticalScrollbar.SmallChange = 1;
            VerticalScrollbar.LargeChange = 1;
            VerticalScrollbar.Scroll += HandleScroll;
            VerticalScrollbar.MouseDown += Vertical_MouseDown;
            Controls.Add(VerticalScrollbar);
            InvalidateLayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var G = e.Graphics;
            G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            G.SmoothingMode = SmoothingMode.HighQuality;
            G.PixelOffsetMode = PixelOffsetMode.HighQuality;
            G.Clear(_BaseColour);
            LogInListBoxItem AllItems;
            int Offset = VerticalScrollbar.Value * (VerticalScrollbar.Maximum + (Height - ItemHeight));
            int StartIndex;
            if (Offset == 0)
                StartIndex = 0;
            else
                StartIndex = Offset / ItemHeight / VerticalScrollbar.Maximum;
            int EndIndex = Math.Min(StartIndex + Height / ItemHeight, _Items.Count - 1);

            for (int I = StartIndex, loopTo = _Items.Count - 1; I <= loopTo; I++)
            {
                AllItems = Items[I];
                int Y = ItemHeight + I * ItemHeight + 1 - Offset + (int)Math.Round(ItemHeight / 2d - 8d);
                if (_SelectedItems.Contains(AllItems))
                {
                    G.FillRectangle(new SolidBrush(_SelectedItemColour), new Rectangle(0, ItemHeight + I * ItemHeight + 1 - Offset, Width - 19, ItemHeight - 1));
                }
                else
                {
                    G.FillRectangle(new SolidBrush(_NonSelectedItemColour), new Rectangle(0, ItemHeight + I * ItemHeight + 1 - Offset, Width - 19, ItemHeight - 1));
                }
                G.DrawLine(new Pen(_BorderColour), 0, ItemHeight + I * ItemHeight + 1 - Offset + ItemHeight - 1, Width - 18, ItemHeight + I * ItemHeight + 1 - Offset + ItemHeight - 1);
                G.DrawString(AllItems.Text, new Font("Segoe UI", 8f), new SolidBrush(_TextColour), 9f, Y);
                G.ResetClip();
            }
            G.FillRectangle(new SolidBrush(_TitleAreaColour), new Rectangle(0, 0, Width, ItemHeight));
            G.DrawRectangle(new Pen(Color.FromArgb(35, 35, 35)), 1, 1, Width - 3, ItemHeight - 2);
            G.DrawString(Text, new Font("Segoe UI", 10f, FontStyle.Bold), new SolidBrush(_TextColour), new Rectangle(0, 0, Width, ItemHeight + 2), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            G.DrawRectangle(new Pen(Color.FromArgb(35, 35, 35), 2f), 1, 0, Width - 2, Height - 1);
            G.DrawLine(new Pen(_BorderColour), 0, ItemHeight, Width, ItemHeight);
            G.DrawLine(new Pen(_BorderColour, 2f), VerticalScrollbar.Location.X - 1, 0, VerticalScrollbar.Location.X - 1, Height);
            G.InterpolationMode = (InterpolationMode)7;
        }

        #endregion

    }

    public class LogInListBoxWBuiltInScrollBar : Control
    {

        #region Declarations

        private List<LogInListBoxItem> _Items = new List<LogInListBoxItem>();
        private readonly List<LogInListBoxItem> _SelectedItems = new List<LogInListBoxItem>();
        private bool _MultiSelect = true;
        private int ItemHeight = 24;
        private readonly LogInVerticalScrollBar VerticalScrollbar;
        private Color _BaseColour = Color.FromArgb(55, 55, 55);
        private Color _SelectedItemColour = Color.FromArgb(50, 50, 50);
        private Color _NonSelectedItemColour = Color.FromArgb(47, 47, 47);
        private Color _BorderColour = Color.FromArgb(35, 35, 35);
        private Color _TextColour = Color.FromArgb(255, 255, 255);
        private int _SelectedHeight = 1;

        #endregion

        #region Properties

        [Category("Colours")]
        public Color TextColour
        {
            get
            {
                return _TextColour;
            }
            set
            {
                _TextColour = value;
            }
        }

        [Category("Control")]
        public int SelectedHeight
        {
            get
            {
                return _SelectedHeight;
            }
            set
            {
                if (value < 1)
                {
                    _SelectedHeight = Height;
                }
                else
                {
                    _SelectedHeight = value;
                }
                InvalidateScroll();
            }
        }

        [Category("Colours")]
        public Color BaseColour
        {
            get
            {
                return _BaseColour;
            }
            set
            {
                _BaseColour = value;
            }
        }

        [Category("Colours")]
        public Color SelectedItemColour
        {
            get
            {
                return _SelectedItemColour;
            }
            set
            {
                _SelectedItemColour = value;
            }
        }

        [Category("Colours")]
        public Color NonSelectedItemColour
        {
            get
            {
                return _NonSelectedItemColour;
            }
            set
            {
                _NonSelectedItemColour = value;
            }
        }

        [Category("Colours")]
        public Color BorderColour
        {
            get
            {
                return _BorderColour;
            }
            set
            {
                _BorderColour = value;
            }
        }


        private void HandleScroll(object sender)
        {
            Invalidate();
        }

        private void InvalidateScroll()
        {
            Debug.Print(Height.ToString());
            if ((int)Math.Round(Math.Round(_Items.Count * ItemHeight / (double)_SelectedHeight)) < _Items.Count * ItemHeight / (double)_SelectedHeight)
            {
                VerticalScrollbar._Maximum = (int)Math.Round(Math.Ceiling(_Items.Count * ItemHeight / (double)_SelectedHeight));
            }
            else if ((int)Math.Round(Math.Round(_Items.Count * ItemHeight / (double)_SelectedHeight)) == 0)
            {
                VerticalScrollbar._Maximum = 1;
            }
            else
            {
                VerticalScrollbar._Maximum = (int)Math.Round(Math.Round(_Items.Count * ItemHeight / (double)_SelectedHeight));
            }
            Invalidate();
        }

        private void InvalidateLayout()
        {
            VerticalScrollbar.Location = new Point(Width - VerticalScrollbar.Width - 2, 2);
            VerticalScrollbar.Size = new Size(18, Height - 4);
            Invalidate();
        }

        public class LogInListBoxItem
        {
            public string Text { get; set; }
            public override string ToString()
            {
                return Text;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public LogInListBoxItem[] Items
        {
            get
            {
                return _Items.ToArray();
            }
            set
            {
                _Items = new List<LogInListBoxItem>(value);
                Invalidate();
                InvalidateScroll();
            }
        }

        public LogInListBoxItem[] SelectedItems
        {
            get
            {
                return _SelectedItems.ToArray();
            }
        }

        public bool MultiSelect
        {
            get
            {
                return _MultiSelect;
            }
            set
            {
                _MultiSelect = value;

                if (_SelectedItems.Count > 1)
                {
                    _SelectedItems.RemoveRange(1, _SelectedItems.Count - 1);
                }

                Invalidate();
            }
        }

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                ItemHeight = (int)Math.Round(Graphics.FromHwnd(Handle).MeasureString("@", Font).Height);
                if (VerticalScrollbar != null)
                {
                    VerticalScrollbar._SmallChange = 1;
                    VerticalScrollbar._LargeChange = 1;

                }
                base.Font = value;
                InvalidateLayout();
            }
        }

        public void AddItem(string Items)
        {
            var Item = new LogInListBoxItem();
            Item.Text = Items;
            _Items.Add(Item);
            Invalidate();
            InvalidateScroll();
        }

        public void AddItems(string[] Items)
        {
            foreach (var I in Items)
            {
                var Item = new LogInListBoxItem();
                Item.Text = I;
                _Items.Add(Item);
            }
            Invalidate();
            InvalidateScroll();
        }

        public void RemoveItemAt(int index)
        {
            _Items.RemoveAt(index);
            Invalidate();
            InvalidateScroll();
        }

        public void RemoveItem(LogInListBoxItem item)
        {
            _Items.Remove(item);
            Invalidate();
            InvalidateScroll();
        }

        public void RemoveItems(LogInListBoxItem[] items)
        {
            foreach (LogInListBoxItem I in items)
                _Items.Remove(I);
            Invalidate();
            InvalidateScroll();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            _SelectedHeight = Height;
            InvalidateScroll();
            InvalidateLayout();
            base.OnSizeChanged(e);
        }

        private void Vertical_MouseDown(object sender, MouseEventArgs e)
        {
            Focus();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            if (e.Button == MouseButtons.Left)
            {
                int Offset = VerticalScrollbar.Value * (VerticalScrollbar.Maximum + (Height - ItemHeight));

                int Index = (e.Y + Offset) / ItemHeight;

                if (Index > _Items.Count - 1)
                    Index = -1;

                if (!(Index == -1))
                {

                    if (ModifierKeys == Keys.Control && _MultiSelect)
                    {
                        if (_SelectedItems.Contains(_Items[Index]))
                        {
                            _SelectedItems.Remove(_Items[Index]);
                        }
                        else
                        {
                            _SelectedItems.Add(_Items[Index]);
                        }
                    }
                    else
                    {
                        _SelectedItems.Clear();
                        _SelectedItems.Add(_Items[Index]);
                    }
                    Debug.Print(_SelectedItems[0].Text);
                }

                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int Move = -(e.Delta * SystemInformation.MouseWheelScrollLines / 120 * (2 / 2));
            int Value = Math.Max(Math.Min(VerticalScrollbar.Value + Move, VerticalScrollbar.Maximum), VerticalScrollbar.Minimum);
            VerticalScrollbar.Value = Value;
            base.OnMouseWheel(e);
        }

        #endregion

        #region Draw Control

        public LogInListBoxWBuiltInScrollBar()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            VerticalScrollbar = new LogInVerticalScrollBar();
            VerticalScrollbar._SmallChange = 1;
            VerticalScrollbar._LargeChange = 1;
            VerticalScrollbar.Scroll += HandleScroll;
            VerticalScrollbar.MouseDown += Vertical_MouseDown;
            Controls.Add(VerticalScrollbar);
            InvalidateLayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(_BaseColour);
            LogInListBoxItem AllItems;
            int Offset = VerticalScrollbar.Value * (VerticalScrollbar.Maximum + (Height - ItemHeight));
            int StartIndex;
            if (Offset == 0)
                StartIndex = 0;
            else
                StartIndex = Offset / ItemHeight / VerticalScrollbar.Maximum;
            int EndIndex = Math.Min(StartIndex + Height / ItemHeight, _Items.Count - 1);
            g.DrawLine(new Pen(_BorderColour, 2f), VerticalScrollbar.Location.X - 1, 0, VerticalScrollbar.Location.X - 1, Height);

            for (int I = StartIndex, loopTo = _Items.Count - 1; I <= loopTo; I++)
            {
                AllItems = Items[I];
                int Y = I * ItemHeight + 1 - Offset + (int)Math.Round(ItemHeight / 2d - 8d);
                if (_SelectedItems.Contains(AllItems))
                {
                    g.FillRectangle(new SolidBrush(_SelectedItemColour), new Rectangle(0, I * ItemHeight + 1 - Offset, Width - 19, ItemHeight - 1));
                }
                else
                {
                    g.FillRectangle(new SolidBrush(_NonSelectedItemColour), new Rectangle(0, I * ItemHeight + 1 - Offset, Width - 19, ItemHeight - 1));
                }
                g.DrawLine(new Pen(_BorderColour), 0, I * ItemHeight + 1 - Offset + ItemHeight - 1, Width - 18, I * ItemHeight + 1 - Offset + ItemHeight - 1);
                g.DrawString(AllItems.Text, new Font("Segoe UI", 8f), new SolidBrush(_TextColour), 9f, Y);
                g.ResetClip();
            }
            g.DrawRectangle(new Pen(Color.FromArgb(35, 35, 35), 2f), 1, 1, Width - 2, Height - 2);
            // .DrawLine(New Pen(_BorderColour), 0, ItemHeight, Width, ItemHeight)
            g.DrawLine(new Pen(_BorderColour, 2f), VerticalScrollbar.Location.X - 1, 0, VerticalScrollbar.Location.X - 1, Height);
            g.InterpolationMode = (InterpolationMode)7;

        }

        #endregion

    }

    [DefaultEvent("SelectedIndexChanged")]
    public class LogInPaginator : Control
    {

        #region Declarations
        private GraphicsPath GP1, GP2;

        private Rectangle R1;

        private Size SZ1;
        private Point PT1;

        private Pen P1, P2, P3;
        private SolidBrush B1, B2;
        #endregion

        #region Functions
        public GraphicsPath RoundRectangle(Rectangle Rectangle, int Curve)
        {
            var P = new GraphicsPath();
            int ArcRectangleWidth = Curve * 2;
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90f);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90f);
            P.AddArc(new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0f, 90f);
            P.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90f, 90f);
            P.AddLine(new Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), new Point(Rectangle.X, Curve + Rectangle.Y));
            return P;
        }

        public GraphicsPath RoundRect(float x, float y, float w, float h, float r = 0.3f, bool TL = true, bool TR = true, bool BR = true, bool BL = true)
        {
            GraphicsPath RoundRectRet = default;
            float d = Math.Min(w, h) * r;
            float xw = x + w;
            float yh = y + h;
            RoundRectRet = new GraphicsPath();
            if (TL)
                RoundRectRet.AddArc(x, y, d, d, 180f, 90f);
            else
                RoundRectRet.AddLine(x, y, x, y);
            if (TR)
                RoundRectRet.AddArc(xw - d, y, d, d, 270f, 90f);
            else
                RoundRectRet.AddLine(xw, y, xw, y);
            if (BR)
                RoundRectRet.AddArc(xw - d, yh - d, d, d, 0f, 90f);
            else
                RoundRectRet.AddLine(xw, yh, xw, yh);
            if (BL)
                RoundRectRet.AddArc(x, yh - d, d, d, 90f, 90f);
            else
                RoundRectRet.AddLine(x, yh, x, yh);
            RoundRectRet.CloseFigure();
            return RoundRectRet;
        }
        #endregion

        #region Properties & Events
        public event SelectedIndexChangedEventHandler SelectedIndexChanged;

        public delegate void SelectedIndexChangedEventHandler(object sender, EventArgs e);

        private int _SelectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                _SelectedIndex = Math.Max(Math.Min(value, MaximumIndex), 0);
                Invalidate();
            }
        }

        private int _NumberOfPages;
        public int NumberOfPages
        {
            get
            {
                return _NumberOfPages;
            }
            set
            {
                _NumberOfPages = value;
                _SelectedIndex = Math.Max(Math.Min(_SelectedIndex, MaximumIndex), 0);
                Invalidate();
            }
        }

        public int MaximumIndex
        {
            get
            {
                return NumberOfPages - 1;
            }
        }

        private int ItemWidth;
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                Invalidate();
            }
        }

        private void InvalidateItems(PaintEventArgs e)
        {
            var S = e.Graphics.MeasureString("000 ..", Font).ToSize();
            ItemWidth = S.Width + 10;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int NewIndex;
                int OldIndex = _SelectedIndex;
                if (_SelectedIndex < 4)
                {
                    NewIndex = e.X / ItemWidth;
                }
                else if (_SelectedIndex > 3 && _SelectedIndex < MaximumIndex - 3)
                {
                    NewIndex = e.X / ItemWidth;
                    switch (NewIndex)
                    {
                        case 2:
                            {
                                NewIndex = OldIndex;
                                break;
                            }
                        case var @case when @case < 2:
                            {
                                NewIndex = OldIndex - (2 - NewIndex);
                                break;
                            }
                        case var case1 when case1 > 2:
                            {
                                NewIndex = OldIndex + (NewIndex - 2);
                                break;
                            }
                    }
                }
                else
                {
                    NewIndex = MaximumIndex - (4 - e.X / ItemWidth);
                }
                if (NewIndex < _NumberOfPages && !(NewIndex == OldIndex))
                {
                    SelectedIndex = NewIndex;
                    SelectedIndexChanged?.Invoke(this, null);
                }
            }
            base.OnMouseDown(e);
        }

        #endregion

        #region Draw Control

        public LogInPaginator()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.FromArgb(54, 54, 54);
            Size = new Size(202, 26);
            B1 = new SolidBrush(Color.FromArgb(50, 50, 50));
            B2 = new SolidBrush(Color.FromArgb(55, 55, 55));
            P1 = new Pen(Color.FromArgb(35, 35, 35));
            P2 = new Pen(Color.FromArgb(23, 119, 151));
            P3 = new Pen(Color.FromArgb(35, 35, 35));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            InvalidateItems(e);
            var g = e.Graphics;
            g.Clear(BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            bool LeftEllipse, RightEllipse;
            if (_SelectedIndex < 4)
            {
                for (int I = 0, loopTo = Math.Min(MaximumIndex, 4); I <= loopTo; I++)
                {
                    RightEllipse = I == 4 && MaximumIndex > 4;
                    DrawBox(I * ItemWidth, I, false, RightEllipse, g);
                }
            }
            else if (_SelectedIndex > 3 && _SelectedIndex < MaximumIndex - 3)
            {
                for (int I = 0; I <= 4; I++)
                {
                    LeftEllipse = I == 0;
                    RightEllipse = I == 4;
                    DrawBox(I * ItemWidth, _SelectedIndex + I - 2, LeftEllipse, RightEllipse, g);
                }
            }
            else
            {
                for (int I = 0; I <= 4; I++)
                {
                    LeftEllipse = I == 0 && MaximumIndex > 4;
                    DrawBox(I * ItemWidth, MaximumIndex - (4 - I), LeftEllipse, false, g);
                }
            }
        }

        private void DrawBox(int x, int index, bool leftEllipse, bool rightEllipse, Graphics g)
        {
            R1 = new Rectangle(x, 0, ItemWidth - 4, Height - 1);
            GP1 = RoundRectangle(R1, 4);
            GP2 = RoundRectangle(new Rectangle(R1.X + 1, R1.Y + 1, R1.Width - 2, R1.Height - 2), 4);
            string T = (index + 1).ToString();
            if (leftEllipse)
                T = ".. " + T;
            if (rightEllipse)
                T = T + " ..";
            SZ1 = g.MeasureString(T, Font).ToSize();
            PT1 = new Point(R1.X + (R1.Width / 2 - SZ1.Width / 2), R1.Y + (R1.Height / 2 - SZ1.Height / 2));
            if (index == _SelectedIndex)
            {
                g.FillPath(B1, GP1);
                var F = new Font(Font, FontStyle.Underline);
                g.DrawString(T, F, Brushes.Black, PT1.X + 1, PT1.Y + 1);
                g.DrawString(T, F, Brushes.White, PT1);
                F.Dispose();
                g.DrawPath(P1, GP2);
                g.DrawPath(P2, GP1);
            }
            else
            {
                g.FillPath(B2, GP1);
                g.DrawString(T, Font, Brushes.Black, PT1.X + 1, PT1.Y + 1);
                g.DrawString(T, Font, Brushes.White, PT1);
                g.DrawPath(P3, GP2);
                g.DrawPath(P1, GP1);
            }
        }

        #endregion

    }
}