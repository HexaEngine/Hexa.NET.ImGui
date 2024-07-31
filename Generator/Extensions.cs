namespace Generator
{
    using CppAst;
    using System;
    using System.Runtime.InteropServices;

    public static class Extensions
    {
        public static CallingConvention GetCallingConvention(this CppCallingConvention convention)
        {
            return convention switch
            {
                CppCallingConvention.C => CallingConvention.Cdecl,
                CppCallingConvention.Win64 => CallingConvention.Winapi,
                CppCallingConvention.X86FastCall => CallingConvention.FastCall,
                CppCallingConvention.X86StdCall => CallingConvention.StdCall,
                CppCallingConvention.X86ThisCall => CallingConvention.ThisCall,
                _ => throw new NotSupportedException(),
            };
        }

        public static string GetCallingConventionDelegate(this CppCallingConvention convention)
        {
            return convention switch
            {
                CppCallingConvention.C => "Cdecl",
                CppCallingConvention.X86FastCall => "Fastcall",
                CppCallingConvention.X86StdCall => "Stdcall",
                CppCallingConvention.X86ThisCall => "Thiscall",
                _ => throw new NotSupportedException(),
            };
        }

        public static string GetCallingConventionLibrary(this CppCallingConvention convention)
        {
            return convention switch
            {
                CppCallingConvention.C => "System.Runtime.CompilerServices.CallConvCdecl",
                CppCallingConvention.X86FastCall => " System.Runtime.CompilerServices.CallConvFastcall",
                CppCallingConvention.X86StdCall => "System.Runtime.CompilerServices.CallConvStdcall",
                CppCallingConvention.X86ThisCall => "System.Runtime.CompilerServices.CallConvThiscall",
                _ => throw new NotSupportedException(),
            };
        }

        public static bool IsPointer(this CppType type)
        {
            if (type is CppPointerType)
            {
                return true;
            }

            if (type is CppQualifiedType qualifiedType)
            {
                return IsPointer(qualifiedType.ElementType);
            }

            return false;
        }

        public static bool IsPointer(this CppType type, ref int depth)
        {
            bool isPointer = false;
            CppType d = type;
            depth = 0;
            while (true)
            {
                if (d is CppPointerType pointer)
                {
                    depth++;
                    d = pointer.ElementType;
                    isPointer = true;
                }
                else
                {
                    break;
                }
            }

            return isPointer;
        }

        public static bool IsPointer(this CppType type, ref int depth, out CppType pointerType)
        {
            bool isPointer = false;
            CppType d = type;
            depth = 0;
            while (true)
            {
                if (d is CppPointerType pointer)
                {
                    depth++;
                    d = pointer.ElementType;
                    isPointer = true;
                }
                else
                {
                    break;
                }
            }
            pointerType = d;
            return isPointer;
        }

        public static bool IsDelegate(this CppPointerType cppPointer, out CppFunctionType cppFunction)
        {
            if (cppPointer.ElementType is CppFunctionType functionType)
            {
                cppFunction = functionType;
                return true;
            }
            cppFunction = null;
            return false;
        }

        public static bool IsDelegate(this CppType cppType, out CppFunctionType cppFunction)
        {
            if (cppType is CppTypedef typedefType)
            {
                return IsDelegate(typedefType.ElementType, out cppFunction);
            }
            if (cppType is CppPointerType cppPointer)
            {
                return IsDelegate(cppPointer.ElementType, out cppFunction);
            }
            if (cppType is CppFunctionType functionType)
            {
                cppFunction = functionType;
                return true;
            }
            cppFunction = null;
            return false;
        }

        public static bool IsDelegate(this CppPointerType cppPointer)
        {
            if (cppPointer.ElementType is CppFunctionType)
            {
                return true;
            }

            return false;
        }

        public static bool IsDelegate(this CppType cppType)
        {
            if (cppType is CppTypedef typedefType)
            {
                return IsDelegate(typedefType.ElementType);
            }
            if (cppType is CppPointerType cppPointer)
            {
                return IsDelegate(cppPointer.ElementType);
            }
            if (cppType is CppFunctionType functionType)
            {
                return true;
            }

            return false;
        }
    }
}