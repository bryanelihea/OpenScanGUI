using System;
using System.Runtime.InteropServices;

public static class PyBridge
{
    [DllImport("pybridge.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int init_python(string scriptName, string pythonHome);

    [DllImport("pybridge.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr call_func(string funcName, string arg);

    [DllImport("pybridge.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void finalize_python();

    public static string CallFunc(string func, string arg = "")
    {
        var resp = Marshal.PtrToStringAnsi(call_func(func, arg));

        Console.WriteLine($"{func} : {(arg == "" ? "NoArgs" : arg)} -> {resp}");
        return resp;
    }

    [DllImport("pybridge.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr get_debug_output();

    [DllImport("pybridge.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void clear_debug_output();

    public static string GetDebugOutput()
    {
        IntPtr ptr = get_debug_output();
        return Marshal.PtrToStringAnsi(ptr);  // ✅ valid now, since it's a copied std::string
    }

    public static void ClearDebugOutput()
    {
        clear_debug_output();
    }


}
