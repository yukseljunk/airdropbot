using System;
using System.Runtime.InteropServices;

namespace AirdropBot
{
    [ComImport, Guid("79EAC9D0-BAF9-11CE-8C82-00AA004BA90B"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     ComVisible(false)]
    public interface IAuthenticate
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Authenticate(ref IntPtr phwnd, ref IntPtr pszUsername, ref IntPtr pszPassword);
    }
}