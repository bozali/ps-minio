using System.Runtime.InteropServices;
using System.Security;

namespace Bal.PsMinio.Management.Extensions;

public static class SecureStringExtensions
{
    public static string ConvertToString(this SecureString value)
    {
        var ptr = IntPtr.Zero;

        try
        {
            ptr = Marshal.SecureStringToGlobalAllocUnicode(value);
            return Marshal.PtrToStringUni(ptr) ?? throw new Exception($"Could not convert ptr to string");
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(ptr);
        }
    }
}