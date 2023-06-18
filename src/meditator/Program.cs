// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Valve.VR;

Console.WriteLine("Hello, World!");



public enum DeviceType
{
    HMD,
    CONTROLLER,
    TRACKER,
    TRACKING_REFERENCE
}


public static unsafe class DeviceFactory
{
    private static VRDriver* driver;

    public static string IServerTrackedDeviceProvider_Version = "IServerTrackedDeviceProvider_004";

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = "HmdDriverFactory")]
    public static void* DriverEntry(char* interface_name, int* return_code)
    {
        var interfaceName = Marshal.PtrToStringAnsi((nint)interface_name);

        if (interfaceName is null)
        {
            *return_code = (int)EVRInitError.Init_NotInitialized;
            return null;
        }

        if (interfaceName.Equals(IServerTrackedDeviceProvider_Version))
        {
            if (driver != null)
                return driver;
            var mem = NativeMemory.Alloc((nuint)sizeof(VRDriver), 1);

            return driver = VRDriver.Create(mem);
        }


        *return_code = (int)EVRInitError.Init_InterfaceNotFound;
        return null;
    }
}

public unsafe struct VRDriver
{
    /// <summary>
    /// Returns all devices being managed by this driver
    /// </summary>
    /// <returns>All managed devices</returns>
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public delegate* unmanaged[Cdecl]<NativeList<VRDevice>> GetDevices = null;

    public VRDriver()
    {
    }

    public static VRDriver* Create(void* mem)
    {
        throw new NotImplementedException();
    }
}

public struct VRDevice
{

}

public struct NativeList<T> where T : unmanaged
{

}

public interface IVRDriver
{
    /// <summary>
    /// Returns all devices being managed by this driver
    /// </summary>
    /// <returns>All managed devices</returns>
    IEnumerable<IVRDevice> GetDevices();

    /// <summary>
    /// Returns all OpenVR events that happened on the current frame
    /// </summary>
    /// <returns>Current frame's OpenVR events</returns>
    List<VREvent> GetOpenVREvents();

    /// <summary>
    /// Returns the milliseconds between last frame and this frame
    /// </summary>
    /// <returns>MS between last frame and this frame</returns>
    long GetLastFrameTime();


    /// <summary>
    /// Adds a device to the driver
    /// </summary>
    /// <param name="device">Device instance</param>
    /// <returns>True on success, false on failure</returns>
    bool AddDevice(IVRDevice device);


    /// <summary>
    /// Returns the value of a settings key
    /// </summary>
    /// <param name="key">The settings key</param>
    /// <returns>Value of the key, std::monostate if the value is malformed or missing</returns>
    SettingsValue GetSettingsValue(string key);


    /// <summary>
    /// Gets the OpenVR VRDriverInput pointer
    /// </summary>
    /// <returns>OpenVR VRDriverInput pointer</returns>
    IVRDriverInput GetInput();

    /// <summary>
    /// Gets the OpenVR VRDriverProperties pointer
    /// </summary>
    /// <returns>OpenVR VRDriverProperties pointer</returns>
    CVRPropertyHelpers GetProperties();

    /// <summary>
    /// Gets the OpenVR VRServerDriverHost pointer
    /// </summary>
    /// <returns>OpenVR VRServerDriverHost pointer</returns>
    IVRServerDriverHost GetDriverHost();

    /// <summary>
    /// Gets the current UniverseTranslation
    /// </summary>
    UniverseTranslation? GetCurrentUniverse();

    /// <summary>
    /// Writes a log message
    /// </summary>
    /// <param name="message">Message to log</param>
    void Log(string message);
}

public interface IVRDevice : ITrackedDeviceServerDriver
{
    /// <summary>
    /// Returns the serial string for this device
    /// </summary>
    /// <returns>Device serial</returns>
    string GetSerial();
}

public interface IVRDriverInput
{

}

public struct VREvent
{

}

public struct SettingsValue
{

}