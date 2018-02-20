using System;
using System.Runtime.InteropServices;

namespace AirdropBot
{
    [ComImport, Guid("6d5140c1-7436-11ce-8034-00aa006009fa"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     ComVisible(false)]
    public interface IServiceProvider
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject);
    }
}