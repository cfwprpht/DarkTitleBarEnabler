using DarkTitleBarEnabler.Properties;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DTBE = DarkTitleBarEnabler;

namespace DarkTitleBarEnabler {
    /*
         Colored Forms Borders.     
    */
    /// <summary>
    /// Enumuration for the BorderColorChanged EventArgs.
    /// </summary>
    public enum BorderColor {
        Normal,
        Focused,
        MouseHover,
        TextChanged,
        ValueChanged,
        CheckChanged,
        BackColor,
        CheckColor
    }

    /// <summary>
    /// Event arguments for the BorderColorChanged event.
    /// </summary>
    public class BorderColorEventArgs {
        /// <summary>
        /// Define which border has changed.
        /// </summary>
        public BorderColor Border;

        /// <summary>
        /// Class initializer.
        /// </summary>
        public BorderColorEventArgs() { }

        /// <summary>
        /// Class Initializer.
        /// </summary>
        /// <param name="border">The state of the border that has changed.
        /// Normal, Focused, MouseHover, TextChanged or ValueChanged.</param>
        public BorderColorEventArgs(BorderColor border) { Border = border; }
    }

    /// <summary>
    /// Custom Colors Extension for the custom Dark Renderer.
    /// </summary>
    public class DarkRendererColors : ProfessionalColorTable {
        /// <summary>
        /// Define a custom color for the 'MenuItemSelected' event.
        /// </summary>
        public override Color MenuItemSelected { get { return Color.FromArgb(-10526881); } }
    }

    /// <summary>
    /// Custom Dark Renderer Extension for the ToolStripItem Class.
    /// </summary>
    public class DarkRenderer : ToolStripProfessionalRenderer {
        /// <summary>
        /// Link the renderer with the custom colors.
        /// </summary>
        public DarkRenderer() : base(new DarkRendererColors()) { }

        /// <summary>
        /// Define a custom 'OnRenderMenuItemBackground' event.
        /// </summary>
        /// <param name="e">ToolStripItem Renderer EventArguments.</param>
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e) {
            // Draw and Paint the Menu first.
            Rectangle rectangle = new Rectangle(Point.Empty, e.Item.Size);
            Color dark = Color.FromArgb(About.darkColor);
            using (SolidBrush brush = new SolidBrush(dark)) {
                e.Graphics.FillRectangle(brush, rectangle);
            }

            // Now check if the Item is selected and enabled.
            if (e.Item.Enabled && e.Item.Selected) {
                // Fill the Item. (indicate selected)
                rectangle = new Rectangle(2, 0, e.Item.Width - 4, e.Item.Height - 1);
                using (SolidBrush brush = new SolidBrush(((DarkRendererColors)ColorTable).MenuItemSelected)) {
                    e.Graphics.FillRectangle(brush, rectangle);
                }

                // Draw a rectangle arround the selection. (Border)
                /*using (Pen pen = new Pen(((DarkRendererColors)ColorTable).MenuItemBorder)) {
                    rectangle = new Rectangle(2, 0, e.Item.Width - 4, e.Item.Height - 1);
                    e.Graphics.DrawRectangle(pen, rectangle);
                }*/
            }
        }
    }

    /// <summary>
    /// Custom TextBox Extension for Rendering various Colored Borders.
    /// </summary>
    public class ColorTextBox : TextBox {
        /// <summary>
        /// Occurs, when the border color has changed.
        /// </summary>
        public event EventHandler<BorderColorEventArgs> BorderColorChanged;

        #region Vars
        /// <summary>
        /// Define the Thikness of the border.
        /// </summary>
        public float BorderThikness {
            get { return borderThikness; } 
            set { 
                borderThikness = value;
                _ = RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME | RDW_IUPDATENOW | RDW_INVALIDATE);
            }
        }
        private float borderThikness = 1f;

        /// <summary>
        /// Define a border Color.
        /// </summary>
        public Color BorderColor {
            get { return borderColor; }
            set {
                borderColor = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.Normal)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color borderColor = Color.Black;

        /// <summary>
        /// Border Color for, when the form has focus.
        /// </summary>
        public Color BorderColorFocused {
            get { return borderColorFocused; }
            set {
                borderColorFocused = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.Focused)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color borderColorFocused = Color.FromArgb(-16764929);

        /// <summary>
        /// Define a border color wenn the mouse hover.
        /// </summary>
        public Color MouseHoverBorderColor {
            get { return mouseHoverBorderColor; }
            set {
                mouseHoverBorderColor = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.MouseHover)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color mouseHoverBorderColor = Color.FromArgb(-16764929);

        /// <summary>
        /// A border color for the textChanged event.
        /// </summary>
        public Color TextChangedBorderColor {
            get { return textChangedBorderColor; }
            set {
                textChangedBorderColor = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.TextChanged)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color textChangedBorderColor = Color.FromArgb(-16764929);

        /// <summary>
        /// Determine if the text box is empty or not.
        /// </summary>
        private bool TextEmpty = true;

        /// <summary>
        /// Determine if the mouse do or shall hover.
        /// </summary>
        private bool IsMouseHover = false;

        /// <summary>
        /// Message to trigger the Drawing function for this control.
        /// </summary>
        private static Message mes;
        #endregion Vars

        #region Function Imports
        /// <summary>
        /// Constant values for the Window draw/paint/handle functions.
        /// </summary>
        private const int  WM_NCPAINT     = 0x85;
        private const uint RDW_INVALIDATE = 0x1;
        private const uint RDW_IUPDATENOW = 0x100;
        private const uint RDW_FRAME      = 0x400;
        
        /// <summary>
        /// Import function call from user32 dll. (GetWindowDeviceContext())
        /// </summary>
        /// <param name="hwnd">The Window Handle.</param>
        /// <returns>Null on a error, else the Window Handle.</returns>
        [DllImport("user32")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        /// <summary>
        /// Import function call from user32 dll. (ReleaseDeviceContext())
        /// </summary>
        /// <param name="hWnd">The Window Handle.</param>
        /// <param name="hDC">The Device Handle</param>
        /// <returns>1 if the Handle was released, else 0.</returns>
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// Import function call from user32 dll. (RedrawWindow())
        /// </summary>
        /// <param name="hWnd">The Window Handle.</param>
        /// <param name="lprc">A pointer to a RECT structure containing the coordinates, in device units, of the update rectangle. This parameter is ignored if the hrgnUpdate parameter identifies a region.</param>
        /// <param name="hrgn">A handle to the update region. If both the hrgnUpdate and lprcUpdate parameters are NULL, the entire client area is added to the update region.</param>
        /// <param name="flags">One or more redraw flags. This parameter can be used to invalidate or validate a window, control repainting, and control which windows are affected by RedrawWindow.</param>
        /// <returns>0 on a fail, else a non zero value.</returns>
        [DllImport("user32.dll")]
        private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprc, IntPtr hrgn, uint flags);
        #endregion FunctionImports

        /// <summary>
        /// Class initializer.
        /// </summary>
        public ColorTextBox() : base() {
            mes = new Message() {
                Msg = WM_NCPAINT      // The Paint message.
            };
        }

        /// <summary>
        /// Internal Custom Window Device Control Paint Fucntion.
        /// </summary>
        /// <param name="color">The Color to use.</param>
        private void WndProcPaint(Color color) {
            var hdc = GetWindowDC(Handle);
            using (Graphics g = Graphics.FromHdcInternal(hdc)) {
                using (Pen pen = new Pen(color, BorderThikness)) {
                    g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                    ReleaseDC(Handle, hdc);
                }
            }
        }

        #region CustomBorderDrawing
        /// <summary>
        /// The initiale border drawing call. 
        /// </summary>
        /// <param name="m">Windows Message Object. Hold informations like if the window shall be paint or if it's focused or not, and so on.</param>        
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            if (TextEmpty) {                            // Only draw with a empty TextBox and Focus, else the OnTextChanged event will draw the border.
                if (m.Msg == WM_NCPAINT && Focused && BorderColorFocused != Color.Transparent && BorderStyle == BorderStyle.Fixed3D)
                    WndProcPaint(BorderColorFocused);
                else if (IsMouseHover) {                // Draws when the mouse hovers and with now focus while the TextBox is empty.
                    if (m.Msg == WM_NCPAINT && !Focused && MouseHoverBorderColor != Color.Transparent && BorderStyle == BorderStyle.Fixed3D)
                        WndProcPaint(MouseHoverBorderColor);
                } else if (!IsMouseHover) {             // Draws when form has no focus and the mouse does not hover while the TextBox is empty.
                    if (m.Msg == WM_NCPAINT && !Focused && BorderColor != Color.Transparent && BorderStyle == BorderStyle.Fixed3D)
                        WndProcPaint(BorderColor);
                }                                       // Draw a border when text has changed.
            } else if (m.Msg == WM_NCPAINT && TextChangedBorderColor != Color.Transparent && BorderStyle == BorderStyle.Fixed3D)
                WndProcPaint(TextChangedBorderColor);
        }

        /// <summary>
        /// Occures, when the border color has changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnBorderColorChanged(BorderColorEventArgs e) {
            WndProc(ref mes);                      // Reflect changes the moment they happen.
            BorderColorChanged?.Invoke(this, e);   // Fire the event, as long there is at least one event setted up.
        }

        /// <summary>
        /// Occures, when the mouse move over the control.
        /// </summary>
        /// <param name="mevent">The mouse event arguments.</param>
        protected override void OnMouseMove(MouseEventArgs mevent) {
            base.OnMouseMove(mevent);
            if (TextEmpty && !IsMouseHover) {
                IsMouseHover = true;
                WndProc(ref mes);  // Reflect changes the moment they happen.
            } else if (!TextEmpty && IsMouseHover) IsMouseHover = false; 
        }

        /// <summary>
        /// Occures, when the mouse leaves the control.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            if (IsMouseHover) {
                IsMouseHover = false;
                WndProc(ref mes);  // Reflect changes the moment they happen.
            }
        }

        /// <summary>
        /// Occures, when the text has changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnTextChanged(EventArgs e) {
            base.OnTextChanged(e);
            if (base.Text != "" && TextEmpty) {
                TextEmpty = false;
                WndProc(ref mes);  // Reflect changes the moment they happen.
            } else if (base.Text == "" && !TextEmpty) {
                TextEmpty = true;
                WndProc(ref mes);  // Reflect changes the moment they happen.
            }
        }

        /// <summary>
        /// Occures, when the controls size has changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            _ = RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME | RDW_IUPDATENOW | RDW_INVALIDATE);
        }

        /// <summary>
        /// Change the Border color of all borders at once.
        /// </summary>
        /// <param name="color">The color to set to all borders.</param>
        public void SetAllBorderColors(Color color) {
            BorderColor = BorderColorFocused = MouseHoverBorderColor = TextChangedBorderColor = color;
        }
        #endregion CustomBorderDrawing
    }

    /// <summary>
    /// Custom NumericUpDown Extension for Rendering a Colored Border.
    /// </summary>
    public class ColorNumUpDown : NumericUpDown {
        /// <summary>
        /// Occurs, when the border color has changed.
        /// </summary>
        public event EventHandler<BorderColorEventArgs> BorderColorChanged;

        #region Vars
        /// <summary>
        /// Define the Thikness of the border.
        /// </summary>
        public float BorderThikness {
            get { return borderThikness; }
            set {
                borderThikness = value;
                _ = RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME | RDW_IUPDATENOW | RDW_INVALIDATE);
            }
        }
        private float borderThikness = 1f;

        /// <summary>
        /// Define a border Color.
        /// </summary>
        public Color BorderColor {
            get { return borderColor; }
            set {
                borderColor = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.Normal)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color borderColor = Color.Black;

        /// <summary>
        /// Border Color for, when the form has focus.
        /// </summary>
        public Color BorderColorFocused {
            get { return borderColorFocused; }
            set {
                borderColorFocused = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.Focused)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color borderColorFocused = Color.FromArgb(-16764929);

        /// <summary>
        /// Define a border color for wenn the mouse hover.
        /// </summary>
        public Color MouseHoverBorderColor {
            get { return mouseHoverBorderColor; }
            set {
                mouseHoverBorderColor = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.MouseHover)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color mouseHoverBorderColor = Color.FromArgb(-16764929);

        /// <summary>
        /// Define a Border Color for when the value has changed.
        /// </summary>
        public Color ValueChangedBorderColor {
            get { return valueChangedBorderColor; }
            set {
                valueChangedBorderColor = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.ValueChanged)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color valueChangedBorderColor = Color.FromArgb(-16764929);

        /// <summary>
        /// Determine if the mouse do or shall hover.
        /// </summary>
        private bool IsMouseHover = false;

        /// <summary>
        /// Detemine if the value has changed.
        /// </summary>
        private bool HasValueChanged = false;

        /// <summary>
        /// Used to determine if the focus already happend once,
        /// to not to Refresh() loop whise.
        /// </summary>
        private bool IsFocus = false;

        /// <summary>
        /// Message to trigger the Drawing function for this control.
        /// </summary>
        private static Message mes;
        #endregion Vars

        #region Function Imports
        /// <summary>
        /// Constant values for the Window draw/paint/handle functions.
        /// </summary>
        private const int WM_NCPAINT = 0x85;
        private const uint RDW_INVALIDATE = 0x1;
        private const uint RDW_IUPDATENOW = 0x100;
        private const uint RDW_FRAME = 0x400;

        /// <summary>
        /// Import function call from user32 dll. (GetWindowDeviceContext())
        /// </summary>
        /// <param name="hwnd">The Window Handle.</param>
        /// <returns>Null on a error, else the Window Handle.</returns>
        [DllImport("user32")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        /// <summary>
        /// Import function call from user32 dll. (ReleaseDeviceContext())
        /// </summary>
        /// <param name="hWnd">The Window Handle.</param>
        /// <param name="hDC">The Device Handle</param>
        /// <returns>1 if the Handle was released, else 0.</returns>
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// Import function call from user32 dll. (RedrawWindow())
        /// </summary>
        /// <param name="hWnd">The Window Handle.</param>
        /// <param name="lprc">A pointer to a RECT structure containing the coordinates, in device units, of the update rectangle. This parameter is ignored if the hrgnUpdate parameter identifies a region.</param>
        /// <param name="hrgn">A handle to the update region. If both the hrgnUpdate and lprcUpdate parameters are NULL, the entire client area is added to the update region.</param>
        /// <param name="flags">One or more redraw flags. This parameter can be used to invalidate or validate a window, control repainting, and control which windows are affected by RedrawWindow.</param>
        /// <returns>0 on a fail, else a non zero value.</returns>
        [DllImport("user32.dll")]
        private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprc, IntPtr hrgn, uint flags);
        #endregion FunctionImports

        /// <summary>
        /// Class initializer.
        /// </summary>
        public ColorNumUpDown() : base() {
            mes = new Message() {
                Msg = WM_NCPAINT
            };

            // Add the OnMouseHover event to the childs of the NumericUpDown Control.
            // More informations can be read at the implemention it self.
            foreach (Control child in Controls) {
                // Add MouseHover event to the child.
                child.MouseMove += (sender, e) => {
                    OnMouseMoveCustom();
                };

                // Add mouse leave event to the child.
                child.MouseLeave += (sender, e) => {
                    OnMouseLeaveCustom();
                };
            }
        }

        /// <summary>
        /// Internal Custom Window Device Control Paint Function.
        /// </summary>
        /// <param name="color">The Color to use.</param>
        private void WndProcPaint(Color color) {
            var hdc = GetWindowDC(Handle);
            using (Graphics g = Graphics.FromHdcInternal(hdc)) {
                using (Pen pen = new Pen(color, BorderThikness)) {
                    g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                    ReleaseDC(Handle, hdc);
                }
            }
        }

        #region CustomBorderDrawing
        /// <summary>
        /// The initiale border drawing call. 
        /// </summary>
        /// <param name="m">Windows Message Object. Hold informations like if the window shall be paint or if it's focused or not, and so on.</param>        
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            if (!HasValueChanged) {
                if (Focused) {                   // Draws when the control has focused and the value is 0.
                    if (m.Msg == WM_NCPAINT && BorderColorFocused != Color.Transparent && BorderStyle == BorderStyle.Fixed3D)
                        WndProcPaint(borderColorFocused);
                } else if (IsMouseHover) {       // Draws when the mouse hovers while the value is 0.
                    if (m.Msg == WM_NCPAINT && MouseHoverBorderColor != Color.Transparent && BorderStyle == BorderStyle.Fixed3D)
                        WndProcPaint(MouseHoverBorderColor);
                } else {                         // Draws when the mouse does not hover while the value is 0.
                    if (m.Msg == WM_NCPAINT && BorderColor != Color.Transparent && BorderStyle == BorderStyle.Fixed3D)
                        WndProcPaint(BorderColor);
                }                                // Draws when the value is not 0.
            } else if (m.Msg == WM_NCPAINT && ValueChangedBorderColor != Color.Transparent && BorderStyle == BorderStyle.Fixed3D)
                WndProcPaint(ValueChangedBorderColor);
        }

        /// <summary>
        /// OnBorderColorChanged event draw the border.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnBorderColorChanged(BorderColorEventArgs e) {
            WndProc(ref mes);                      // Reflect changes the moment they happen.
            BorderColorChanged?.Invoke(this, e);   // Fire the event, as long there is at least one event setted up.
        }

        /// <summary>
        /// Does something on 'OnGotFocus' event.
        /// </summary>
        /// <param name="e">Event Arguments.</param>
        protected override void OnGotFocus(EventArgs e) {
            base.OnGotFocus(e);
            if (!IsFocus) {
                // Unlikely other controls does NumericUpDown not Trigger a internal drawing function
                // when the focus has changed.
                // This is why we need to trigger it our self for this extension.
                IsFocus = true;
                WndProc(ref mes);
            }
        }

        /// <summary>
        /// Does something on 'OnLostFocus' event.
        /// </summary>
        /// <param name="e">Event Arguments.</param>
        protected override void OnLostFocus(EventArgs e) {
            base.OnLostFocus(e);
            if (IsFocus) {
                // Unlikely other controls does NumericUpDown not Trigger a internal drawing function
                // when the focus has changed.
                // This is why we need to trigger it our self for this extension.
                IsFocus = false;
                WndProc(ref mes);
            }
        }

        /// <summary>
        /// Does something on 'OnPaint' event.
        /// </summary>
        /// <param name="e">Event Arguments.</param>
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            // Unlikely other controls does NumericUpDown not Trigger a internal drawing function
            // when the control is painted.
            // This is why we need to trigger it our self for this extension.
            // Otherwise the initiale shown border, and also sometimes between it, 
            // will not have the custom border size.
            WndProc(ref mes);
        }

        /// <summary>
        /// OnMouseMove for NumericUpDown would only apply for the small border,
        /// but not for the whole control and so also not for the text field and the up down buttons.
        /// To not only draw when the small border is hovered, we leave the original implementation and 
        /// define a custom one which we only use on the childs of NumericUpDown Control.
        /// This way it will act like a normal text box or button control.
        /// </summary>
        private void OnMouseMoveCustom() {
            if (!HasValueChanged && !IsMouseHover) {
                IsMouseHover = true;
                WndProc(ref mes);  // Reflect changes the moment they happen.
            } else if (HasValueChanged) IsMouseHover = false; 
        }

        /// <summary>
        /// OnMouseLeave for NumericUpDown would only apply for the small border,
        /// but not for the whole control and so also not for the text field and the up down buttons.
        /// To not only draw when the small border is hovered, we leave the original implementation and 
        /// define a custom one which we only use on the childs of NumericUpDown Control.
        /// This way it will act like a normal text box or button control.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        private void OnMouseLeaveCustom() {
            if (!HasValueChanged && IsMouseHover) {
                IsMouseHover = false;
                WndProc(ref mes);  // Reflect changes the moment they happen.
            }
        }

        /// <summary>
        /// Does something on 'OnValueChnaged' event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnValueChanged(EventArgs e) {
            base.OnValueChanged(e);
            if (Value != 0) {
                HasValueChanged = true;
                WndProc(ref mes);  // Reflect changes the moment they happen.
            } else {
                HasValueChanged = false;
                WndProc(ref mes);  // Reflect changes the moment they happen.
            }
        }

        /// <summary>
        /// Does something on 'OnSizeChanged' event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            _ = RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME | RDW_IUPDATENOW | RDW_INVALIDATE);
        }
        #endregion CustomBorderDrawing
    }

    /// <summary>
    /// Custom Button Extension for rendering a Colored Border.
    /// </summary>
    public class ColorButton : Button {
        /// <summary>
        /// Occurs, when the border color has changed.
        /// </summary>
        public event EventHandler<BorderColorEventArgs> BorderColorChanged;

        #region Vars
        /// <summary>
        /// Determine which state of the Button to Draw.
        /// </summary>
        private enum ButtonState {
            Focused,
            Hovered,
            Selected,
            Clicked,
            Nothing
        }

        /// <summary>
        /// Define the Thikness of the border.
        /// </summary>
        public float BorderThikness { get; set; } = 1f;

        /// <summary>
        /// Define a border Color.
        /// </summary>
        public Color BorderColor {
            get { return borderColor; }
            set {
                borderColor = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.Normal)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color borderColor = Color.Black;

        /// <summary>
        /// Border Color for, when the form has focus.
        /// </summary>
        public Color BorderColorFocused {
            get { return borderColorFocused; }
            set {
                borderColorFocused = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.Focused)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color borderColorFocused = Color.FromArgb(-16764929);

        /// <summary>
        /// Define a border color for wenn the mouse hover.
        /// </summary>
        public Color MouseHoverBorderColor {
            get { return mouseHoverBorderColor; }
            set {
                mouseHoverBorderColor = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.MouseHover)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color mouseHoverBorderColor = Color.FromArgb(-16764929);

        /// <summary>
        /// Determine if the mouse do or shall hover.
        /// </summary>
        private bool IsMouseHover = false;

        /// <summary>
        /// Determine if the button was clicked.
        /// </summary>
        private bool IsClicked = false;

        /// <summary>
        /// Determine if the requirements for a custom paint are given.
        /// </summary>
        private bool IsPaint = false;

        /// <summary>
        /// Used to determine if the focus already happend once,
        /// to not to Refresh() loop whise.
        /// </summary>
        private bool IsFocus = false;
        #endregion Vars

        /// <summary>
        /// Class initializer.
        /// </summary>
        public ColorButton() : base() {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
        }

        /// <summary>
        /// Internal Custom Paint Function.
        /// </summary>
        /// <param name="color">The Color to use.</param>
        /// <param name="state">The button state.</param>
        /// <param name="pevent">Paint event arguments.</param>
        private void CustomPaint(Color color, ButtonState state, PaintEventArgs pevent) {
            // Change border thikness on purpose.
            float thikness = BorderThikness;
            if (state == ButtonState.Clicked || state == ButtonState.Focused) thikness *= 2;

            // Draw the border.
            using (Pen pen = new Pen(color, thikness)) {
                pevent.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);

                // Do we mouse move?
                if (state == ButtonState.Hovered) {
                    pevent.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(20, color)),
                                                  ClientRectangle.X,
                                                  ClientRectangle.Y,
                                                  Width - 1,
                                                  Height - 1
                                                 );
                }
            }

            // Draw a dotted focus border.
            if (Focused) {
                using (Pen pen = new Pen(Brushes.Gray) { DashStyle = DashStyle.Dot })
                    pevent.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }
        }

        #region CustomBorderDrawing
        /// <summary>
        /// Occures when the control is painted.
        /// </summary>
        /// <param name="pevent">The Paint event arguments.</param>
        protected override void OnPaint(PaintEventArgs pevent) {
            if (FlatStyle == FlatStyle.Flat && FlatAppearance.BorderSize == 0) {
                IsPaint = true;
                pevent.Graphics.Clear(BackColor);

                // Draw Text, if any.
                if (Text != string.Empty) {
                    using (Brush brush = new SolidBrush(ForeColor)) {
                        SizeF text = pevent.Graphics.MeasureString(Text, Font);
                        pevent.Graphics.DrawString(Text, Font, brush,
                                                   (Width / 2) - (int)text.Width / 2,                      // Center the Text into the button.
                                                   (Height / 2) - Font.Height / 2);
                    }
                }

                // Draw image if set.
                if (BackgroundImage != null) pevent.Graphics.DrawImage(BackgroundImage, ClientRectangle);  // Note: Add ImageLayout drawing.                

                if (Focused) {                                 // Draws when the button has focus.
                    if (BorderColorFocused != Color.Transparent)
                        CustomPaint(BorderColorFocused, ButtonState.Focused, pevent);
                } else if (IsClicked) {                        // Draws when the button was clicked.
                    if (BorderColorFocused != Color.Transparent) {
                        CustomPaint(BorderColorFocused, ButtonState.Clicked, pevent);
                        IsClicked = false;
                    }
                } else if (IsMouseHover) {                     // Draws when the mouse hovers.
                    if (MouseHoverBorderColor != Color.Transparent)
                        CustomPaint(MouseHoverBorderColor, ButtonState.Hovered, pevent);
                } else if (BorderColor != Color.Transparent)   // Draws when the mouse does not hover.
                    CustomPaint(BorderColor, ButtonState.Nothing, pevent);
            } else {
                IsPaint = false;
                base.OnPaint(pevent);
            }
        }

        /// <summary>
        /// Occures, when the border color has changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnBorderColorChanged(BorderColorEventArgs e) {
            if (IsPaint) {
                Refresh();                             // Reflect changes the moment they happen.
                BorderColorChanged?.Invoke(this, e);   // Fire the event, as long there is at least one event setted up.
            }
        }

        /// <summary>
        /// Occures, when the mouse mover over the control.
        /// </summary>
        /// <param name="mevent">The mouse event arguments.</param>
        protected override void OnMouseMove(MouseEventArgs mevent) {
            if (IsPaint && !IsMouseHover) {
                IsMouseHover = true;
                Refresh();            // Reflect changes the moment they happen.
            }
            base.OnMouseMove(mevent);
        }

        /// <summary>
        /// Occures, when the mouse leave the control.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnMouseLeave(EventArgs e) {
            if (IsPaint && IsMouseHover) {
                IsMouseHover = false;
                Refresh();              // Reflect changes the moment they happen. 
            }            
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Occures, when the button is clicked.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnClick(EventArgs e) {            
            if (IsPaint) {
                IsClicked = true;
                Refresh();
            }
            base.OnClick(e);
        }

        /// <summary>
        /// Occures, when the control got focus.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnGotFocus(EventArgs e) {
            if (IsPaint && !IsFocus) {
                IsFocus = true;
                Refresh();
            }
            base.OnGotFocus(e); 
        }

        /// <summary>
        /// Occures, when the control lost the focus.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnLostFocus(EventArgs e) {
            if (IsPaint && IsFocus) {
                IsFocus = false;
                Refresh();
            }
            base.OnLostFocus(e); 
        }
        #endregion CustomBorderDrawing
    }

    /// <summary>
    /// Custom CeckBox Extension for rendering colored borders.
    /// </summary>
    public class ColorCheckBox : CheckBox {
        public event EventHandler<BorderColorEventArgs> BorderColorChanged;

        #region Vars
        /// <summary>
        /// Define the Thikness of the border.
        /// </summary>
        public float BorderThikness { get; set; } = 0.5f;

        /// <summary>
        /// Define a border Color.
        /// </summary>
        public Color BorderColor {
            get { return borderColor; }
            set {
                borderColor = value;
                if (Created) OnBorderColorChanged(this, new BorderColorEventArgs(DTBE.BorderColor.Normal));
            }
        }
        private Color borderColor = Color.FromArgb(-16764929);

        /// <summary>
        /// Define a border color for when the check changed.
        /// </summary>
        public Color BorderColorChecked {
            get { return borderColorChanged; }
            set {
                borderColorChanged = value;
                if (Created) OnBorderColorChanged(this, new BorderColorEventArgs(DTBE.BorderColor.CheckChanged));
            }
        }
        private Color borderColorChanged = Color.FromArgb(-16764929);

        /// <summary>
        /// Define a BackColor for the checkBox it self.
        /// </summary>
        public Color CheckBackColor {
            get { return checkBackColor; }
            set {
                checkBackColor = value;
                if (Created) OnBorderColorChanged(this, new BorderColorEventArgs(DTBE.BorderColor.BackColor));
            }
        }
        private Color checkBackColor = Color.Beige;

        /// <summary>
        /// Define a Color for the Checkmark to use.
        /// </summary>
        public Color CheckColor {
            get { return checkColor; }
            set {
                checkColor = value;
                if (Created) OnBorderColorChanged(this, new BorderColorEventArgs(DTBE.BorderColor.CheckColor));
            }
        }
        private Color checkColor = Color.Red;

        /// <summary>
        /// Determine if the requirements for a custom paint are given.
        /// </summary>
        private bool IsPaint = false;

        /// <summary>
        /// Used to determine if the focus already happend once,
        /// to not to Refresh() loop whise.
        /// </summary>
        private bool IsFocus = false;

        /// <summary>
        /// Structure for the checkbox to draw.
        /// </summary>
        private struct CheckBox {
            public int X;
            public int Y;
            public int Size;
            public int FontSize;
            public int TextX;

            public void Clear() { X = Y = Size = FontSize = TextX = 0; }
        }
        private CheckBox checkBox;
        #endregion Vars

        /// <summary>
        /// Class initializer.
        /// </summary>
        public ColorCheckBox() : base() {
            checkBox = new CheckBox() { X = 0, Y = 0, Size = 0, FontSize = 0, TextX = 0 };
            Padding = new Padding(3);
        }

        /// <summary>
        /// Checks if a number is even or not.
        /// </summary>
        /// <param name="num">The number to check if it is even.</param>
        /// <returns>True, if the value is a even number, else false.</returns>
        private bool IsEven(int num) { return num % 2 == 0; }

        /// <summary>
        /// Internal Custom Paint Function.
        /// </summary>
        /// <param name="color">The Color to use.</param>
        /// <param name="pevent">Paint event arguments.</param>
        private void CustomPaint(Color color, PaintEventArgs pevent) {            
            if (checkBox.X == 0) {  // Calculate location and sizes.
                checkBox.X        = (int)((Size.Height - Font.Size) / 2);
                checkBox.Y        = (int)((Size.Height - Font.Size) / 2);
                checkBox.Size     = (int)Font.Size + 4;
                checkBox.FontSize = checkBox.Size - 2;
                checkBox.TextX    = (int)(checkBox.X + checkBox.Size + (Font.Size / 2 / 2));

                // Check if positions and sizes are even numbers, if not, make them even.
                if (!IsEven(checkBox.X))             checkBox.X--;
                else if (!IsEven(checkBox.Y))        checkBox.Y--;
                else if (!IsEven(checkBox.Size))     checkBox.Size--;
                else if (!IsEven(checkBox.FontSize)) checkBox.FontSize--;
                else if (!IsEven(checkBox.TextX))    checkBox.TextX--;
                
                if (RightToLeft == RightToLeft.Yes) {  // Adjust RightToLeft.
                    checkBox.TextX = checkBox.X / 2;
                    checkBox.X     = checkBox.TextX + (int)(pevent.Graphics.MeasureString(Text, Font).Width + (Font.Size / 2 / 2));

                    // Adjust the checkBox on the right site.
                    while (ClientRectangle.Width - (checkBox.X + checkBox.Size) > 7) {
                        checkBox.X++;
                    }
                }

                // A little bit tweeking here.
                checkBox.Y -= 2;
            }

            // Draw checkBox control.
            if (Text != string.Empty) pevent.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), checkBox.TextX, checkBox.Y);                    // Draw Text.
            pevent.Graphics.FillRectangle(new SolidBrush(CheckBackColor), checkBox.X, checkBox.Y, checkBox.Size, checkBox.Size);                        // Fill out the checkbox aka draw backcolor.
            pevent.Graphics.DrawRectangle(new Pen(color, BorderThikness), checkBox.X, checkBox.Y, checkBox.Size, checkBox.Size);                        // Draw checkBox on right side.
            if (Checked) pevent.Graphics.DrawString("ü", new Font("Wingdings", checkBox.FontSize), new SolidBrush(CheckColor), checkBox.X, checkBox.Y); // Do we need to draw the checkmark?
            if (Focused) ControlPaint.DrawBorder(pevent.Graphics, ClientRectangle, Color.Gray, ButtonBorderStyle.Dotted);                               // Draw doted focus border.
        }

        #region CustomBorderDrawing
        /// <summary>
        /// Occures, when the control is painted.
        /// </summary>
        /// <param name="pevent">The paint event arguments.</param>
        protected override void OnPaint(PaintEventArgs pevent) {
            if (Created) {
                IsPaint = true;
                pevent.Graphics.Clear(BackColor);
                if (Checked) {
                    if (BorderColorChecked != Color.Transparent)
                        CustomPaint(BorderColorChecked, pevent);
                } else {
                    if (BorderColor != Color.Transparent)
                        CustomPaint(BorderColor, pevent);
                }
            }
        }

        /// <summary>
        /// Occures, when the border color has changed.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="bcevent">The BorderColorChanged event arguments.</param>
        public void OnBorderColorChanged(object sender, BorderColorEventArgs bcevent) {
            if (IsPaint) {
                Refresh();
                BorderColorChanged?.Invoke(sender, bcevent);
            }            
        }

        /// <summary>
        /// Occures, when the control got focus.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnGotFocus(EventArgs e) {
            if (IsPaint && !IsFocus) {
                IsFocus = true;
                Refresh();
            }
            base.OnGotFocus(e);
        }

        /// <summary>
        /// Occures, when the control lost the focus.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnLostFocus(EventArgs e) {
            if (IsPaint && IsFocus) {
                IsFocus = false;
                Refresh();
            }
            base.OnLostFocus(e);
        }

        /// <summary>
        /// Occures, when the check state has changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnCheckedChanged(EventArgs e) {
            if (IsPaint) Refresh();
            base.OnCheckedChanged(e);
        }

        /// <summary>
        /// Occures, when the controls font has changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnFontChanged(EventArgs e) {
            base.OnFontChanged(e);
            checkBox.Clear();
            Refresh();
        }

        /// <summary>
        /// Occures, when RightToLeft has changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnRightToLeftChanged(EventArgs e) {
            base.OnRightToLeftChanged(e);
            checkBox.Clear();
            Refresh();
        }
        #endregion CustomBorderDrawing
    }

    /// <summary>
    /// Custom RichTextBox Extension for rendering colored borders.
    /// </summary>
    public class ColorRichTextBox : RichTextBox {
        /// <summary>
        /// Occurs, when the border color has changed.
        /// </summary>
        public event EventHandler<BorderColorEventArgs> BorderColorChanged;

        #region Vars
        /// <summary>
        /// Define the Thikness of the border.
        /// </summary>
        public float BorderThikness {
            get { return borderThikness; }
            set {
                borderThikness = value;
                _ = RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME | RDW_IUPDATENOW | RDW_INVALIDATE);
            }
        }
        private float borderThikness = 2f;

        /// <summary>
        /// Define a border Color.
        /// </summary>
        public Color BorderColor {
            get { return borderColor; }
            set {
                borderColor = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.Normal)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color borderColor = Color.Black;

        /// <summary>
        /// Border Color for, when the form has focus.
        /// </summary>
        public Color BorderColorFocused {
            get { return borderColorFocused; }
            set {
                borderColorFocused = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.Focused)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color borderColorFocused = Color.FromArgb(-16764929);

        /// <summary>
        /// Define a border color wenn the mouse hover.
        /// </summary>
        public Color MouseHoverBorderColor {
            get { return mouseHoverBorderColor; }
            set {
                mouseHoverBorderColor = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.MouseHover)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color mouseHoverBorderColor = Color.FromArgb(-16764929);

        /// <summary>
        /// A border color for the textChanged event.
        /// </summary>
        public Color TextChangedBorderColor {
            get { return textChangedBorderColor; }
            set {
                textChangedBorderColor = value;
                if (Created) OnBorderColorChanged(new BorderColorEventArgs(DTBE.BorderColor.TextChanged)); // If the control was already created we can trigger the paint function to reflect changed color.
            }
        }
        private Color textChangedBorderColor = Color.FromArgb(-16764929);

        /// <summary>
        /// Determine if the text box is empty or not.
        /// </summary>
        private bool TextEmpty = true;

        /// <summary>
        /// Determine if the mouse do or shall hover.
        /// </summary>
        private bool IsMouseHover = false;

        /// <summary>
        /// Used to determine if the focus already happend once,
        /// to not to Refresh() loop whise.
        /// </summary>
        private bool IsFocus = false;

        /// <summary>
        /// Message to trigger the Drawing function for this control.
        /// </summary>
        private static Message mes;
        #endregion Vars

        #region Function Imports
        /// <summary>
        /// Constant values for the Window draw/paint/handle functions.
        /// </summary>
        private const int WM_NCPAINT = 0x85;
        private const uint RDW_INVALIDATE = 0x1;
        private const uint RDW_IUPDATENOW = 0x100;
        private const uint RDW_FRAME = 0x400;

        /// <summary>
        /// Import function call from user32 dll. (GetWindowDeviceContext())
        /// </summary>
        /// <param name="hwnd">The Window Handle.</param>
        /// <returns>Null on a error, else the Window Handle.</returns>
        [DllImport("user32")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        /// <summary>
        /// Import function call from user32 dll. (ReleaseDeviceContext())
        /// </summary>
        /// <param name="hWnd">The Window Handle.</param>
        /// <param name="hDC">The Device Handle</param>
        /// <returns>1 if the Handle was released, else 0.</returns>
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// Import function call from user32 dll. (RedrawWindow())
        /// </summary>
        /// <param name="hWnd">The Window Handle.</param>
        /// <param name="lprc">A pointer to a RECT structure containing the coordinates, in device units, of the update rectangle. This parameter is ignored if the hrgnUpdate parameter identifies a region.</param>
        /// <param name="hrgn">A handle to the update region. If both the hrgnUpdate and lprcUpdate parameters are NULL, the entire client area is added to the update region.</param>
        /// <param name="flags">One or more redraw flags. This parameter can be used to invalidate or validate a window, control repainting, and control which windows are affected by RedrawWindow.</param>
        /// <returns>0 on a fail, else a non zero value.</returns>
        [DllImport("user32.dll")]
        private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprc, IntPtr hrgn, uint flags);
        #endregion FunctionImports

        /// <summary>
        /// Class iinitializer.
        /// </summary>
        public ColorRichTextBox() : base() {
            BorderStyle = BorderStyle.FixedSingle;
            mes.Msg = WM_NCPAINT;
        }

        #region CustomBorderDrawing
        /// <summary>
        /// Internal Custom Paint Function.
        /// </summary>
        /// <param name="color">The Color to use.</param>
        /// <param name="pevent">Paint event arguments.</param>
        private void WndProcPaint(Color color) {
            var hdc = GetWindowDC(Handle);
            using (Graphics graphics = Graphics.FromHdcInternal(hdc)) {
                using (Pen pen = new Pen(color, BorderThikness)) {
                    graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);  // Draw the border.
                    ReleaseDC(Handle, hdc);
                }
            }
        }

        /// <summary>
        /// Occures, when the control is painted.
        /// </summary>
        /// <param name="m">Windows Message Object. Hold informations like if the window shall be paint or if it's focused or not, and so on.</param>
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            if (Created) {
                if (m.Msg == WM_NCPAINT && BorderStyle == BorderStyle.FixedSingle) {
                    if (!TextEmpty) {
                        if (TextChangedBorderColor != Color.Transparent)
                            WndProcPaint(TextChangedBorderColor);
                    } else if (Focused) {
                        if (BorderColorFocused != Color.Transparent)
                            WndProcPaint(BorderColorFocused);
                    } else if (IsMouseHover) {
                        if (MouseHoverBorderColor != Color.Transparent)
                            WndProcPaint(MouseHoverBorderColor);
                    } else {
                        if (BorderColor != Color.Transparent)
                            WndProcPaint(BorderColor);
                    }
                }
            }
        }

        /// <summary>
        /// OnBorderColorChanged event draw the border.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnBorderColorChanged(BorderColorEventArgs e) {
            WndProc(ref mes);                      // Reflect changes the moment they happen.
            BorderColorChanged?.Invoke(this, e);   // Fire the event, as long there is at least one event setted up.
        }

        /// <summary>
        /// Occures, when the mouse mover over the control.
        /// </summary>
        /// <param name="mevent">The mouse event arguments.</param>
        protected override void OnMouseMove(MouseEventArgs mevent) {
            if (!IsMouseHover && TextEmpty) {
                IsMouseHover = true;
                WndProc(ref mes);            // Reflect changes the moment they happen.
            } else if (IsMouseHover && !TextEmpty) IsMouseHover = false;
            base.OnMouseMove(mevent);
        }

        /// <summary>
        /// Occures, when the mouse leave the control.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnMouseLeave(EventArgs e) {
            if (IsMouseHover && TextEmpty) {
                IsMouseHover = false;
                WndProc(ref mes);              // Reflect changes the moment they happen. 
            }
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Occures, when the text has changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnTextChanged(EventArgs e) {
            base.OnTextChanged(e);
            if (base.Text != "" && TextEmpty) {
                TextEmpty = false;
                WndProc(ref mes);  // Reflect changes the moment they happen.
            } else if (base.Text == "" && !TextEmpty) {
                TextEmpty = true;
                WndProc(ref mes);  // Reflect changes the moment they happen.
            }
        }

        /// <summary>
        /// Occures, when the control got focus.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnGotFocus(EventArgs e) {
            base.OnGotFocus(e);
            if (!IsFocus) {
                IsFocus = true;
                WndProc(ref mes);
            }
        }

        /// <summary>
        /// Occures, when the control lost the focus.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnLostFocus(EventArgs e) {
            base.OnLostFocus(e);
            if (IsFocus) {
                IsFocus = false;
                WndProc(ref mes);
            }
        }

        /// <summary>
        /// Occures, when the controls size has changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            _ = RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME | RDW_IUPDATENOW | RDW_INVALIDATE);
        }

        /// <summary>
        /// Change the Border color of all borders at once.
        /// </summary>
        /// <param name="color">The color to set to all borders.</param>
        public void SetAllBorderColors(Color color) {
            BorderColor = BorderColorFocused = MouseHoverBorderColor = TextChangedBorderColor = color;
        }
        #endregion CustomBorderDrawing
    }

    /*
        Custom Dialogs.
    */
    #region Enums
    /// <summary>
    /// Enumuration for the CursorPosition.
    /// </summary>
    public enum CursorPosition {
        None,
        Left,
        Right
    }

    /// <summary>
    /// Enumuration for the Button Position.
    /// </summary>
    public enum ButtonPosition {
        None,
        Left,
        Center,
        Right
    }

    /// <summary>
    /// Enumuration for Yes No Cancel button.
    /// </summary>
    public enum Buttons {
        Ok,
        Yes,
        YesNo,
        YesNoCancel,
        OkNo,
        OkNoCancel
    }
    #endregion Enums

    /// <summary>
    /// Array Extension.
    /// </summary>
    public static class ArrayExtension {
        /// <summary>
        /// Internal used for indexer resolving.
        /// </summary>
        private static int a;

        /// <summary>
        /// Gets the index of a value within a array.
        /// </summary>
        /// <typeparam name="T">The Type of the array to use and the value to search for.</typeparam>
        /// <param name="source">The source Array.</param>
        /// <param name="value">The value to check for existens.</param>
        /// <returns>The index of the value within the array if found, else -1.</returns>
        public static int IndexOf<T>(this T[] source, T value) {
            if (source == null) throw new FormatException("Null Refernce", new Exception("The Array to check for a value existens is not Initialized."));
            for (int i = 0; i < source.Length; i++) if (source[i].Equals(value)) return i;
            return -1;
        }

        /// <summary>
        /// Add a value to a Array.
        /// </summary>
        /// <typeparam name="T">The Type of the array to use and the value to add.</typeparam>
        /// <param name="source">The source Array.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>The new Array with the added value.</returns>
        public static void Add<T>(this T[] source, T value) {
            T[] newArray = new T[source.Length + 1];
            Array.Copy(source, newArray, source.Length);
            newArray[newArray.Length - 1] = value;
            source = newArray;
        }

        /// <summary>
        /// Remove a value from the Array.
        /// </summary>
        /// <typeparam name="T">The Type of the array to use and the value to remove.</typeparam>
        /// <param name="source">The source Array.</param>
        /// <param name="value">The value to remove.</param>
        /// <returns>The new Array with removed value.</returns>
        public static void Remove<T>(this T[] source, T value) {
            T[] newArray = new T[0];
            if ((a = source.IndexOf(value)) != -1) {                
                for (int z = 0; z < source.Length; z++) if (z != a) newArray.Add(source[z]);
                source = newArray;
            }
        }

        /// <summary>
        /// Remove last value from the Array.
        /// </summary>
        /// <typeparam name="T">The Type of the array to use.</typeparam>
        /// <param name="source">The source Array.</param>
        /// /// <returns>The new Array with the added value.</returns>
        public static void RemoveLast<T>(this T[] source) { source.Remove(source[source.Length - 1]); }
    }

    /// <summary>
    /// Bitmap Extension.
    /// </summary>
    public static class BitmapExtension {
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap Resize(this Image image, int width, int height) {
            Rectangle newRect = new Rectangle(0, 0, width, height);
            Bitmap newImage = new Bitmap(width, height);

            newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(newImage)) {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes()) {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, newRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return newImage;
        }
    }

    /// <summary>
    /// Anchor Styles shortages.
    /// </summary>
    public class Anch {
        /// <summary>
        /// Define to use All AnchorStyles.
        /// </summary>
        public static readonly AnchorStyles All = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        /// <summary>
        /// Define to use only Bottom and Right AnchorStyle.
        /// </summary>
        public static readonly AnchorStyles BR = AnchorStyles.Bottom | AnchorStyles.Right;

        /// <summary>
        /// Define to use only Bottom and Left AnchorStyle.
        /// </summary>
        public static readonly AnchorStyles BL = AnchorStyles.Bottom | AnchorStyles.Left;

        /// <summary>
        /// Define to use only Bottom and Left AnchorStyle.
        /// </summary>
        public static readonly AnchorStyles BLR = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        /// <summary>
        /// Define to use only Top and Left AnchorStyle.
        /// </summary>
        public static readonly AnchorStyles TL = AnchorStyles.Top | AnchorStyles.Left;

        /// <summary>
        /// Define to use only Top and Right AnchorStyle.
        /// </summary>
        public static readonly AnchorStyles TR = AnchorStyles.Top | AnchorStyles.Right;

        /// <summary>
        /// Define to use only Left and Right AnchorStyle.
        /// </summary>
        public static readonly AnchorStyles LR = AnchorStyles.Left | AnchorStyles.Right;

        /// <summary>
        /// Define to use Top, Left and Right AnchorStyle.
        /// </summary>
        public static readonly AnchorStyles TLR = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    }

    /// <summary>
    /// Button Extension.
    /// </summary>
    public static class ButtonExtension {
        /// <summary>
        /// Center a Button.
        /// </summary>
        /// <param name="source">The Button to center.</param>
        /// <param name="parent">The Parent Control to center in.</param>
        public static void Center(this Button source, Control parent) { source.Location = new Point(((parent.ClientSize.Width / 2) - source.Size.Width / 2), source.Location.Y); }

        /// <summary>
        /// Center a range of Buttons.
        /// </summary>
        /// <param name="source">The Button to center.</param>
        /// <param name="parent">The Parent Control to center in.</param>
        /// <param name="otherButtons">The Other Buttons to Center too.</param>
        public static void Center(this Button source, Control parent, Button[] otherButtons) {
            int middleOfButtons = source.Size.Width;
            foreach (Button button in otherButtons) middleOfButtons += button.Size.Width + 5;
            middleOfButtons /= 2;
            source.Location = new Point((parent.ClientSize.Width / 2) - middleOfButtons, source.Location.Y);
            Button toUse = source;
            foreach (Button button in otherButtons) {
                button.Location = new Point(toUse.Location.X + (toUse.Size.Width + 5), toUse.Location.Y);
                toUse = button;
            }
        }

        /// <summary>
        /// Allign a Button on the Left.
        /// </summary>
        /// <param name="source">The Button to allign on the Left.</param>
        public static void Left(this Button source) { source.Location = new Point(10, source.Location.Y); }

        /// <summary>
        /// Allign Buttons on the Left.
        /// </summary>
        /// <param name="source">The Button to allign on the Left.</param>
        /// <param name="otherButtons">The Other Buttons to allign too.</param>
        public static void Left(this Button source, Button[] otherButtons) {
            source.Left();
            Button toUse = source;
            foreach (Button button in otherButtons) { button.Location = new Point(toUse.Location.X + toUse.Size.Width + 5, button.Location.Y); toUse = button; }
        }

        /// <summary>
        /// Allign Buttons on the Right.
        /// </summary>
        /// <param name="source">The Button to allign on the Right.</param>
        /// <param name="parent">The Parent Control to allign in.</param>
        public static void Right(this Button source, Control parent) { source.Location = new Point(((parent.ClientSize.Width - 10) - source.Size.Width), source.Location.Y); }

        /// <summary>
        /// Allign Buttons on the Right.
        /// </summary>
        /// <param name="source">The Button to allign on the Right.</param>
        /// <param name="parent">The Parent Control to allign in.</param>
        /// <param name="otherButtons">The Other Buttons to allign too.</param>
        public static void Right(this Button source, Control parent, Button[] otherButtons) {
            Button toUse = otherButtons[otherButtons.Length - 1];
            otherButtons[otherButtons.Length - 1].Right(parent);
            otherButtons.RemoveLast();
            if (otherButtons.Length > 1) Array.Reverse(otherButtons);
            otherButtons.Add(source);
            foreach (Button button in otherButtons) { button.Location = new Point(toUse.Location.X + toUse.Size.Width + 5, button.Location.Y); toUse = button; }
        }
    }

    /// <summary>
    /// A MessageBox Wrapper with predifend Casts.
    /// </summary>
    public static class MessagBox {
        #region Vars
        /// <summary>
        /// Controls for our custom Dialogs.
        /// </summary>
        private static Form dialog;
        private static Panel panel;
        private static PictureBox pictureDialog;
        private static Label label;
        private static TextBox textBox;
        private static CheckBox checkBox;
        private static ColorButton buttonOK;
        private static ColorButton buttonNo;
        private static ColorButton buttonCancel;

        /// <summary>
        /// The Size of the Dialog. (only used for the Input Dialog to adjust)
        /// </summary>
        public static Size Size;

        /// <summary>
        /// Define a Font to use for the message text.
        /// </summary>
        public static Font Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

        /// <summary>
        /// The Icon for the Dialog to use.
        /// </summary>
        public static Icon DialogIcon = Resources.Empty;

        /// <summary>
        /// The Label aka Message Back Color.
        /// </summary>
        public static Color DialogBack = Color.White;

        /// <summary>
        /// The Label aka Message Fore Color.
        /// </summary>
        public static Color DialogFore = SystemColors.ControlText;

        /// <summary>
        /// The InputBox Back Color.
        /// </summary>
        public static Color TextBack = Color.White;

        /// <summary>
        /// The InputBox Fore Color.
        /// </summary>
        public static Color TextFore = SystemColors.ControlText;

        /// <summary>
        /// The Form Back Color.
        /// </summary>
        public static Color FormBack = SystemColors.Control;

        /// <summary>
        /// The Form Fore Color.
        /// </summary>
        public static Color FormFore = SystemColors.ControlText;

        /// <summary>
        /// The Buttons Border Color.
        /// </summary>
        public static Color ButtonBorder = Color.Black;

        /// <summary>
        /// The Buttons Back Color.
        /// </summary>
        public static Color ButtonBack = Color.White;
                
        /// <summary>
        /// Question Image for the Dialog.
        /// </summary>
        public static Image QuestionImage = Resources.Question.Resize(32, 32);

        /// <summary>
        /// Error Image for the Dialog.
        /// </summary>
        public static Image ErrorImage = Resources.Error.Resize(32, 32);

        /// <summary>
        /// Determine to use a Panel Control for the Dialog.
        /// </summary>
        public static bool UsePanel = true;

        /// <summary>
        /// The Form AutoSize defination.
        /// </summary>
        public static bool AutoSize = true;

        /// <summary>
        /// Show a Icon for this Dialog. (default is false)
        /// </summary>
        public static bool ShowIcon = false;

        /// <summary>
        /// Determine to use Auto LineBreak on the Messages.
        /// </summary>
        public static bool AutoLineBreak = false;

        /// <summary>
        /// Determine to Center the Button(s) for the Messages.
        /// </summary>
        public static bool CenterButton = false;

        /// <summary>
        /// Determine to use Icons for the buttons or not.
        /// </summary>
        public static bool UseIcons = false;

        /// <summary>
        /// Reflects the checkBox checked state.
        /// </summary>
        public static bool Checked = false;

        /// <summary>
        /// Reflects the InputBox Text.
        /// </summary>
        public static string UsrInput = string.Empty;

        /// <summary>
        /// Center the Form to his Parent by default.
        /// </summary>
        public static FormStartPosition Start = FormStartPosition.CenterParent;

        /// <summary>
        /// Set Border Style to FixedSingle by Default.
        /// </summary>
        public static FormBorderStyle Border = FormBorderStyle.FixedSingle;

        /// <summary>
        /// The position of the Cursor within the TextBox.
        /// </summary>
        public static CursorPosition Cursor = CursorPosition.Right;

        /// <summary>
        /// The position of the buttons. Left, Cener or Right. (default is Right)
        /// </summary>
        public static ButtonPosition Button = ButtonPosition.Right;
        #endregion Vars

        #region Functions
        /// <summary>
        /// Initialize a basic Dialog Form.
        /// </summary>
        /// <param name="text">The Message Text.</param>
        /// <param name="caption">The Name of the Dialog.</param>
        private static void InitializeDialog(string text, string caption) {
            // Clear the check and the Text Box, also in case we use it to determine which dialog do actually run.
            checkBox = null;
            textBox = null;
            pictureDialog = null;
            buttonNo = buttonCancel = null;

            // Initialize Components, Draw the Form.
            if (UsePanel) panel = new Panel() { Anchor = Anch.All, BackColor = DialogBack, Location = new Point(0, 0), Size = new Size(137, 66), TabIndex = 0 };
            label = new Label() { Anchor = Anch.TL, AutoSize = true, Font = Font, ForeColor = DialogFore, Text = text, Location = new Point(12, 17), Size = new Size(38, 15), TabIndex = 0 };
            buttonOK = new ColorButton() { Anchor = Anch.BR, Location = new Point(41, 79), Size = new Size(86, 26), TabIndex = 1, Text = "OK", BackColor = ButtonBack, BorderColor = ButtonBorder, BorderThikness = 0.5f, UseVisualStyleBackColor = true };
            buttonOK.Click += (sender, e) => { dialog.DialogResult = DialogResult.OK; };
            dialog = new Form() {
                AutoScaleDimensions = new SizeF(6F, 13F), AutoScaleMode = AutoScaleMode.Font, ClientSize = new Size(137, 116), MaximizeBox = false, MinimizeBox = false,
                ShowIcon = ShowIcon, Icon = DialogIcon, StartPosition = Start, FormBorderStyle = Border, Text = caption, BackColor = FormBack, ForeColor = FormFore
            };

            // Define OnLoad Event.
            dialog.Load += Dialog_Load;

            // Throw all Components together.
            if (UsePanel) {
                panel.Controls.Add(label);
                dialog.Controls.Add(panel);
            } else dialog.Controls.Add(label);
            dialog.Controls.Add(buttonOK);
        }

        /// <summary>
        /// Add a CheckBox to the Dialog.
        /// </summary>
        /// <param name="text">The CheckBox Text.</param>
        private static void AddCheckBox(string text) {
            checkBox = new CheckBox() {
                Anchor = Anch.BL, AutoSize = true, Text = text, Font = new Font("Neo Sans", 8.239999F), Location = new Point(12, 47), Size = new Size(15, 14), TabIndex = 2, UseVisualStyleBackColor = true
            };
            if (UsePanel) panel.Controls.Add(checkBox);
            else dialog.Controls.Add(checkBox);
        }

        /// <summary>
        /// Add a PictureBox to the dialog which will be used as icon box.
        /// </summary>
        /// <param name="picture">The icon to use.</param>
        private static void AddPicBox(Image picture) {
            if (picture.Size.Width > 32 || picture.Size.Height > 32) picture = picture.Resize(32, 32);
            pictureDialog = new PictureBox() {
                Anchor = Anch.TL, Image = picture, Location = new Point(27, 26), Size = new Size(32, 32), TabIndex = 3, TabStop = false,
                SizeMode = PictureBoxSizeMode.CenterImage
            };
            dialog.Size = new Size(214, 183);
            label.Location = new Point(75, 26);
            label.Anchor = Anch.TL;
            if (UsePanel) panel.Controls.Add(pictureDialog);
            else dialog.Controls.Add(pictureDialog);
        }

        /// <summary>
        /// Add a second Button to the Dialog.
        /// </summary>
        /// <param name="Text">Button Text.</param>
        private static void AddSecondButton(Buttons button) {
            if (button == Buttons.Ok) return;
            else if (button == Buttons.Yes) { buttonOK.Text = "YES"; return; }

            buttonNo = new ColorButton() { Anchor = Anch.BR, Location = new Point(132, 79), Size = new Size(86, 26), TabIndex = 2, Text = "NO", BorderThikness = 0.5f, BackColor = ButtonBack, BorderColor = ButtonBorder, UseVisualStyleBackColor = true };
            buttonNo.Click += (sender, e) => { dialog.DialogResult = DialogResult.No; };
            dialog.ClientSize = new Size(226, 116);
            buttonOK.Location = new Point(41, 79);
            dialog.Controls.Add(buttonNo);
            if (button == Buttons.YesNo) {
                buttonOK.Text = "YES";
                buttonOK.Click -= (sender, e) => { dialog.DialogResult = DialogResult.OK; };
                buttonOK.Click += (sender, e) => { dialog.DialogResult = DialogResult.Yes; };
            } else if (button == Buttons.OkNoCancel || button == Buttons.YesNoCancel) AddThirdButton(button);
        }

        /// <summary>
        /// Add a third Button to the Dialog.
        /// </summary>
        /// <param name="Text">Button Text.</param>
        private static void AddThirdButton(Buttons button) {
            buttonCancel = new ColorButton() { Anchor = Anch.BR, Location = new Point(224, 79), Size = new Size(86, 26), TabIndex = 3, Text = "Cancel", BorderThikness = 0.5f, BackColor = ButtonBack, BorderColor = ButtonBorder, UseVisualStyleBackColor = true };
            buttonCancel.Click += (sender, e) => { dialog.DialogResult = DialogResult.Cancel; };
            dialog.ClientSize = new Size(319, 116);
            buttonOK.Location = new Point(41, 79);
            buttonNo.Location = new Point(132, 79);
            dialog.Controls.Add(buttonCancel);
            if (button == Buttons.YesNoCancel) {
                buttonOK.Text = "YES";
                buttonOK.Click -= (sender, e) => { dialog.DialogResult = DialogResult.OK; };
                buttonOK.Click += (sender, e) => { dialog.DialogResult = DialogResult.Yes; };
            }
        }

        /// <summary>
        /// Reset the public Class vars to their standart.
        /// </summary>
        public static void Reset() {
            Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DialogBack = Color.White;
            DialogFore = SystemColors.ControlText;
            TextBack = Color.White;
            TextFore = SystemColors.ControlText;
            FormBack = SystemColors.Control;
            FormFore = SystemColors.ControlText;
            Start = FormStartPosition.CenterParent;
            Border = FormBorderStyle.FixedSingle;
            Cursor = CursorPosition.Right;
            Button = ButtonPosition.Right;
            Size = Size.Empty;
            DialogIcon = Resources.Empty;            
            QuestionImage = Resources.Question.Resize(32, 32);
            ErrorImage = Resources.Error.Resize(32, 32);
            AutoSize = UsePanel = true;
            ShowIcon = AutoLineBreak = UseIcons = false;
            dialog = null;
            label = null;
            buttonOK = buttonNo = buttonCancel = null;
            textBox = null;
            checkBox = null;
            panel = null;
            pictureDialog = null;
            DialogIcon = null;
        }

        /// <summary>
        /// On Load of the Dialog Form do.
        /// </summary>
        /// <param name="sender">The Sender.</param>
        /// <param name="e">The Event Arguments.</param>
        private static void Dialog_Load(object sender, EventArgs e) {
            // Set a Custom Icon for this Dialog.
            if (ShowIcon && DialogIcon != null) dialog.Icon = DialogIcon;

            // Adjust TextBox if used.
            if (textBox != null) {
                if (Cursor == CursorPosition.Right) textBox.SelectionStart = textBox.Text.Length;
                else if (Cursor == CursorPosition.Left) textBox.SelectionStart = 0;
            }

            // Adjust Text if AutoLineBreak is used.
            if (AutoLineBreak) {
                if (label.Text.Length > 72) {
                    string sub;
                    int z;
                    for (int i = 0; i < label.Text.Length; i += 74) {
                        sub = label.Text.Substring(i, 72);
                        if (!sub.Contains(".")) {
                            if (!sub.Contains("?")) {
                                if (!sub.Contains("!")) {
                                    if (!sub.Contains(" ")) {
                                        label.Text = label.Text.Insert(i, "\n");
                                        z = i;
                                    } else {
                                        z = sub.LastIndexOf(" ") + 1;
                                        label.Text = label.Text.Insert(z, "\n");
                                    }
                                } else {
                                    z = sub.LastIndexOf("!") + 1;
                                    label.Text = label.Text.Insert(z, "\n");
                                }
                            } else {
                                z = sub.LastIndexOf("?") + 1;
                                label.Text = label.Text.Insert(z, "\n");
                            }
                        } else {
                            z = sub.LastIndexOf(".") + 1;
                            label.Text = label.Text.Insert(z, "\n");
                        }
                        i += z - i;
                    }
                }
            }

            // Adjust Dialog Form to match the text message.
            if (pictureDialog == null && textBox == null) dialog.ClientSize = new Size(label.Size.Width + 23, label.Size.Height + 101);
            else if (pictureDialog != null && textBox == null) {
                string[] adjust = label.Text.Split('\n');
                int widthNew = label.Size.Width + 92;
                if (widthNew < 164) widthNew = 165;
                dialog.ClientSize = new Size(widthNew, label.Size.Height + 107);
                pictureDialog.Location = new Point(pictureDialog.Location.X, pictureDialog.Location.Y + ((adjust.Length - 1) * 5));
            }

            // Adjust CheckBox if used.
            if (checkBox != null) {
                dialog.ClientSize = new Size(dialog.ClientSize.Width, dialog.ClientSize.Height + 12);
                int length = checkBox.Size.Width - dialog.ClientSize.Width;
                if (length > 0) dialog.ClientSize = new Size(dialog.ClientSize.Width + length + 20, dialog.ClientSize.Height);
            }

            // Adjust buttons.
            if (buttonNo != null && buttonCancel == null) { if (dialog.ClientSize.Width < 226) dialog.ClientSize = new Size(226, dialog.ClientSize.Height); }
            else if (buttonNo != null && buttonCancel != null) { if (dialog.ClientSize.Width < 319) dialog.ClientSize = new Size(319, dialog.ClientSize.Height); }
            if (Button != ButtonPosition.Right) {
                if (Button == ButtonPosition.Left) {
                    if (buttonNo != null && buttonCancel == null) {
                        buttonOK.Anchor = buttonNo.Anchor = Anch.BL;
                        buttonOK.Left(new ColorButton[] { buttonNo });
                    } else if (buttonNo != null && buttonCancel != null) {
                        buttonOK.Anchor = buttonNo.Anchor = buttonCancel.Anchor = Anch.BL;
                        buttonOK.Left(new ColorButton[] { buttonNo, buttonCancel });
                    } else {
                        buttonOK.Anchor = Anch.BL;
                        buttonOK.Left();
                    }
                } else {
                    if (buttonNo != null && buttonCancel == null) {
                        buttonOK.Anchor = buttonNo.Anchor = AnchorStyles.Bottom;
                        buttonOK.Center(dialog, new ColorButton[] { buttonNo });
                    } else if (buttonNo != null && buttonCancel != null) {
                        buttonOK.Anchor = buttonNo.Anchor = buttonCancel.Anchor = AnchorStyles.Bottom;
                        buttonOK.Center(dialog, new ColorButton[] { buttonNo, buttonCancel });
                    } else {
                        buttonOK.Anchor = AnchorStyles.Bottom;
                        buttonOK.Center(dialog);
                    }
                }
            }
        }

        /// <summary>
        /// A Simple message dialog.
        /// </summary>
        /// <param name="text">The message to display.</param>
        /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static void Show(string text, [Optional] params object[] args) {
            if (args != null) text = string.Format(text, args);
            InitializeDialog(text, string.Empty);
            dialog.ShowDialog();
        }

        /// <summary>
        /// A Simple message dialog.
        /// </summary>
        /// <param name="caption">The title to display.</param>
        /// <param name="text">The message to display.</param>
        /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static void Show(string caption, string text, [Optional] params object[] args) {
            if (args != null) text = string.Format(text, args);
            InitializeDialog(text, caption);
            dialog.ShowDialog();
        }

        /// <summary>
        /// A Simple message dialog.
        /// </summary>
        /// <param name="button">Define other buttons to use.</param>
        /// <param name="caption">The title to display.</param>
        /// <param name="text">The message to display.</param>
        /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static DialogResult Show(Buttons button, string caption, string text, [Optional] params object[] args) {
            if (args != null) text = string.Format(text, args);
            InitializeDialog(text, caption);
            AddSecondButton(button);
            return dialog.ShowDialog();
        }

        /// <summary>
        /// A Simple message dialog with a CheckBox added.
        /// </summary>
        /// <param name="check">The Text of the CheckBox to display.</param>
        /// <param name="caption">The title to display.</param>
        /// <param name="text">The message to display.</param>
        /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static DialogResult Show(string check, string caption, string text, [Optional] params object[] args) {
            if (args != null) text = string.Format(text, args);
            InitializeDialog(text, caption);
            AddCheckBox(check);
            DialogResult result = dialog.ShowDialog();
            Checked = checkBox.Checked;
            return result;
        }

        /// <summary>
        /// A Simple message dialog with a CheckBox added.
        /// </summary>
        /// <param name="check">The Text of the CheckBox to display.</param>
        /// <param name="checkState">Define the checkBox checked state.</param>
        /// <param name="caption">The title to display.</param>
        /// <param name="text">The message to display.</param>
        /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static DialogResult Show(string check, bool checkState, string caption, string text, [Optional] params object[] args) {
            if (args != null) text = string.Format(text, args);
            InitializeDialog(text, caption);
            AddCheckBox(check);
            checkBox.Checked = checkState;
            DialogResult result = dialog.ShowDialog();
            Checked = checkBox.Checked;
            return result;
        }

        /// <summary>
        /// A Simple message dialog with a CheckBox added.
        /// </summary>
        /// <param name="button">Define other buttons to use.</param>
        /// <param name="check">The Text of the CheckBox to display.</param>
        /// <param name="checkState">Define the checkBox checked state.</param>
        /// <param name="caption">The title to display.</param>
        /// <param name="text">The message to display.</param>
        /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static DialogResult Show(Buttons button, string check, bool checkState, string caption, string text, [Optional] params object[] args) {
            if (args != null) text = string.Format(text, args);
            InitializeDialog(text, caption);
            AddCheckBox(check);
            AddSecondButton(button);
            checkBox.Checked = checkState;
            DialogResult result = dialog.ShowDialog();
            Checked = checkBox.Checked;
            return result;
        }

        /// <summary>
        /// A Simple message dialog.
        /// </summary>
        /// <param name="icon">The icon or image to show within the dialog.</param>
        /// <param name="text">The message to display.</param>
        /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static void Show(Image icon, string text, [Optional] params object[] args) {
            if (args != null) text = string.Format(text, args);
            InitializeDialog(text, string.Empty);
            AddPicBox(icon);
            dialog.ShowDialog();
        }

        /// <summary>
        /// A Simple message dialog.
        /// </summary>
        /// <param name="icon">The icon or image to show within the dialog.</param>
        /// <param name="caption">The title to display.</param>
        /// <param name="text">The message to display.</param>
        /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static void Show(Image icon, string caption, string text, [Optional] params object[] args) {
            if (args != null) text = string.Format(text, args);
            InitializeDialog(text, caption);
            AddPicBox(icon);
            dialog.ShowDialog();
        }

        /// <summary>
        /// A Simple message dialog.
        /// </summary>
        /// <param name="icon">The icon or image to show within the dialog.</param>
        /// <param name="button">Define other buttons to use.</param>
        /// <param name="caption">The title to display.</param>
        /// <param name="text">The message to display.</param>
        /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static DialogResult Show(Image icon, Buttons button, string caption, string text, [Optional] params object[] args) {
            if (args != null) text = string.Format(text, args);
            InitializeDialog(text, caption);
            AddSecondButton(button);
            AddPicBox(icon);
            return dialog.ShowDialog();
        }

        /// <summary>
        /// A Simple message dialog with a CheckBox added.
        /// </summary>
        /// <param name="icon">The icon or image to show within the dialog.</param>
        /// <param name="check">The Text of the CheckBox to display.</param>
        /// <param name="caption">The title to display.</param>
        /// <param name="text">The message to display.</param>
        /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static DialogResult Show(Image icon, string check, string caption, string text, [Optional] params object[] args) {
            if (args != null) text = string.Format(text, args);
            InitializeDialog(text, caption);
            AddCheckBox(check);
            AddPicBox(icon);
            DialogResult result = dialog.ShowDialog();
            Checked = checkBox.Checked;
            return result;
        }

        /// <summary>
        /// A Simple message dialog with a CheckBox added.
        /// </summary>
        /// <param name="icon">The icon or image to show within the dialog.</param>
        /// <param name="check">The Text of the CheckBox to display.</param>
        /// <param name="checkState">Define the checkBox checked state.</param>
        /// <param name="caption">The title to display.</param>
        /// <param name="text">The message to display.</param>
        /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static DialogResult Show(Image icon, string check, bool checkState, string caption, string text, [Optional] params object[] args) {
            if (args != null) text = string.Format(text, args);
            InitializeDialog(text, caption);
            AddCheckBox(check);
            AddPicBox(icon);
            checkBox.Checked = checkState;
            DialogResult result = dialog.ShowDialog();
            Checked = checkBox.Checked;
            return result;
        }

        /// <summary>
        /// A Simple message dialog with a CheckBox added.
        /// </summary>
        /// <param name="icon">The icon or image to show within the dialog.</param>
        /// <param name="button">Define other buttons to use.</param>
        /// <param name="check">The Text of the CheckBox to display.</param>
        /// <param name="checkState">Define the checkBox checked state.</param>
        /// <param name="caption">The title to display.</param>
        /// <param name="text">The message to display.</param>
        /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static DialogResult Show(Image icon, Buttons button, string check, bool checkState, string caption, string text, [Optional] params object[] args) {
            if (args != null) text = string.Format(text, args);
            InitializeDialog(text, caption);
            AddCheckBox(check);
            AddSecondButton(button);
            AddPicBox(icon);
            checkBox.Checked = checkState;
            DialogResult result = dialog.ShowDialog();
            Checked = checkBox.Checked;
            return result;
        }

        /// <summary>
        /// Show a Question dialog, specifing a title.
        /// </summary>
        /// <param name="title">The Title of hte MessageBox Dialog.</param>
        /// <param name="message">The message to show.</param>
        /// /// <param name="args">Addional arguments for the message.</param>
        /// <returns>The Dialog Result.</returns>
        public static DialogResult Question(string title, string message, [Optional] params object[] args) {
            if (args != null) message = string.Format(message, args);
            return Show(QuestionImage, Buttons.OkNo, title, message);
        }

        /// <summary>
        /// Show a error dialog, specifing a title.
        /// </summary>
        /// <param name="title">The Title of hte MessageBox Dialog.</param>
        /// <param name="message">The message to show.</param>
        /// <param name="args">Addional arguments for the message.</param>
        public static void Error(string title, string message, [Optional] params object[] args) {
            if (args != null) message = string.Format(message, args);
            _ = Show(ErrorImage, Buttons.Ok, title, message);
        }
        #endregion Functions
    }
}
