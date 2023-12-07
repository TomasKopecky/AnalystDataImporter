using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AnalystDataImporter.Utilities
{
    public sealed class SafeCursorHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        [DllImport("user32.dll")]
        private static extern bool DestroyCursor(IntPtr handle);

        public SafeCursorHandle() : base(true) { }
        public SafeCursorHandle(IntPtr handle) : base(true)
        {
            SetHandle(handle);
        }
        protected override bool ReleaseHandle()
        {
            return DestroyCursor(handle);
        }
    }
}
