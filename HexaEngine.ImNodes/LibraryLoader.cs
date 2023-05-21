namespace HexaEngine.ImNodesNET
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public static class LibraryLoader
    {
        static LibraryLoader()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Extension = ".dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Extension = ".dylib";
            }
            else
            {
                Extension = ".so";
            }
        }

        public static void SetImportResolver()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);
        }

        public static string Extension { get; }

        private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return LoadLocalLibrary("cimgui");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return LoadLocalLibrary("cimgui");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return LoadLocalLibrary("cimgui");
            }
            else
            {
                return LoadLocalLibrary("cimgui");
            }
        }

        public static IntPtr LoadLocalLibrary(string libraryName)
        {
            if (!libraryName.EndsWith(Extension, StringComparison.OrdinalIgnoreCase))
            {
                libraryName += Extension;
            }

            var osPlatform = GetOSPlatform();
            var architecture = GetArchitecture();

            var libraryPath = GetNativeAssemblyPath(osPlatform, architecture, libraryName);

            static string GetOSPlatform()
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "win";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return "linux";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return "osx";
                }

                throw new ArgumentException("Unsupported OS platform.");
            }

            static string GetArchitecture()
            {
                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.X86: return "x86";
                    case Architecture.X64: return "x64";
                    case Architecture.Arm: return "arm";
                    case Architecture.Arm64: return "arm64";
                }

                throw new ArgumentException("Unsupported architecture.");
            }

            static string GetNativeAssemblyPath(string osPlatform, string architecture, string libraryName)
            {
                var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (assemblyLocation == null)
                {
                    throw new Exception();
                }

                var paths = new[]
                {
                    Path.Combine(assemblyLocation, libraryName),
                    Path.Combine(assemblyLocation, "runtimes", osPlatform, "native", libraryName),
                    Path.Combine(assemblyLocation, "runtimes", $"{osPlatform}-{architecture}", "native", libraryName),
                };

                foreach (var path in paths)
                {
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }

                return libraryName;
            }

            IntPtr handle;

            handle = NativeLibrary.Load(libraryPath);

            if (handle == IntPtr.Zero)
            {
                throw new DllNotFoundException($"Unable to load library '{libraryName}'.");
            }

            return handle;
        }
    }
}