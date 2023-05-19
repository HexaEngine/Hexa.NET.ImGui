namespace HexaEngine.Core.Graphics
{
    using HexaEngine.Core.Debugging;
    using System;
    using System.Buffers.Binary;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Thread-safe shader cache
    /// </summary>
    public static class ShaderCache
    {
        private const string file = "cache/shadercache.bin";
        private static readonly List<ShaderCacheEntry> entries = new();
        private static readonly SemaphoreSlim s = new(1);

        static ShaderCache()
        {
            _ = Directory.CreateDirectory("cache");
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            Load();
        }

        private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            Save();
            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].Free();
            }
        }

        public static bool DisableCache { get; set; } = false;

        public static IReadOnlyList<ShaderCacheEntry> Entries => entries;

        public static unsafe void CacheShader(string path, ShaderMacro[] macros, InputElementDescription[] inputElements, Shader* shader)
        {
            lock (entries)
            {
                if (DisableCache)
                {
                    return;
                }

                var entry = new ShaderCacheEntry(path, macros, inputElements, shader->Clone());
                entries.RemoveAll(x => x.Equals(entry));
                entries.Add(entry);
                SaveAsync();
            }
        }

        public static unsafe bool GetShader(string path, ShaderMacro[] macros, Shader** shader, [MaybeNullWhen(false)] out InputElementDescription[]? inputElements)
        {
            lock (entries)
            {
                *shader = default;
                inputElements = null;
                if (DisableCache)
                {
                    return false;
                }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                var ventry = new ShaderCacheEntry(path, macros, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                var entry = entries.FirstOrDefault(x => x.Equals(ventry));
                if (entry != default)
                {
                    inputElements = entry.InputElements;
                    *shader = entry.Shader->Clone();
                    return true;
                }
                return false;
            }
        }

        public static void Clear()
        {
            lock (entries)
            {
                ImGuiConsole.Log(LogSeverity.Info, "Clearing shader cache ...");
                for (int i = 0; i < entries.Count; i++)
                {
                    entries[i].Free();
                }
                entries.Clear();
                ImGuiConsole.Log(LogSeverity.Info, "Clearing shader cache ... done");
            }
        }

        private static unsafe bool GetShaderInternal(string path, ShaderMacro[] macros, Shader** shader, [MaybeNullWhen(false)] out InputElementDescription[]? inputElements)
        {
            lock (entries)
            {
                *shader = null;
                inputElements = null;
                if (DisableCache)
                {
                    return false;
                }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                var ventry = new ShaderCacheEntry(path, macros, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                var entry = entries.FirstOrDefault(x => x.Equals(ventry));
                if (entry != default)
                {
                    inputElements = entry.InputElements;
                    *shader = entry.Shader;
                    return true;
                }
                return false;
            }
        }

        private static void Load()
        {
            if (!File.Exists(file))
            {
                return;
            }

            lock (entries)
            {
                var span = (Span<byte>)File.ReadAllBytes(file);
                var decoder = Encoding.UTF8.GetDecoder();
                var count = BinaryPrimitives.ReadInt32LittleEndian(span);
                entries.EnsureCapacity(count);

                int idx = 4;
                for (int i = 0; i < count; i++)
                {
                    var entry = new ShaderCacheEntry();
                    idx += entry.Read(span[idx..], decoder);
                    entries.Add(entry);
                }
            }
        }

        private static void Save()
        {
            s.Wait();
            lock (entries)
            {
                var encoder = Encoding.UTF8.GetEncoder();
                var size = 4 + entries.Sum(x => x.SizeOf(encoder));
                var span = size < 2048 ? stackalloc byte[size] : new byte[size];

                BinaryPrimitives.WriteInt32LittleEndian(span, entries.Count);

                int idx = 4;
                for (var i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];
                    idx += entry.Write(span[idx..], encoder);
                }

                File.WriteAllBytes(file, span.ToArray());
            }
            s.Release();
        }

        private static Task SaveAsync()
        {
            return Task.Run(() =>
            {
                s.Wait();
                lock (entries)
                {
                    var encoder = Encoding.UTF8.GetEncoder();
                    var size = 4 + entries.Sum(x => x.SizeOf(encoder));
                    var span = size < 2048 ? stackalloc byte[size] : new byte[size];

                    BinaryPrimitives.WriteInt32LittleEndian(span, entries.Count);

                    int idx = 4;
                    for (var i = 0; i < entries.Count; i++)
                    {
                        var entry = entries[i];
                        idx += entry.Write(span[idx..], encoder);
                    }

                    File.WriteAllBytes(file, span.ToArray());
                }
                s.Release();
            });
        }
    }

    public unsafe struct ShaderCacheEntry : IEquatable<ShaderCacheEntry>
    {
        public string Name;
        public ShaderMacro[] Macros;
        public InputElementDescription[] InputElements;
        public Shader* Shader;

        public ShaderCacheEntry(string name, ShaderMacro[] macros, InputElementDescription[] inputElements, Shader* bytecode)
        {
            Name = name;
            Macros = macros;
            InputElements = inputElements;
            Shader = bytecode;
        }

        public int Write(Span<byte> dest, Encoder encoder)
        {
            int idx = 0;
            idx += WriteString(dest[idx..], Name, encoder);
            BinaryPrimitives.WriteInt32LittleEndian(dest[idx..], Macros.Length);
            idx += 4;
            for (int i = 0; i < Macros.Length; i++)
            {
                var macro = Macros[i];
                idx += WriteString(dest[idx..], macro.Name, encoder);
                idx += WriteString(dest[idx..], macro.Definition, encoder);
            }
            BinaryPrimitives.WriteInt32LittleEndian(dest[idx..], InputElements.Length);
            idx += 4;
            for (int i = 0; i < InputElements.Length; i++)
            {
                var element = InputElements[i];
                idx += WriteString(dest[idx..], element.SemanticName, encoder);
                idx += WriteInt32(dest[idx..], element.SemanticIndex);
                idx += WriteInt32(dest[idx..], (int)element.Format);
                idx += WriteInt32(dest[idx..], element.Slot);
                idx += WriteInt32(dest[idx..], element.AlignedByteOffset);
                idx += WriteInt32(dest[idx..], (int)element.Classification);
                idx += WriteInt32(dest[idx..], element.InstanceDataStepRate);
            }
            if (Shader != null)
            {
                BinaryPrimitives.WriteInt32LittleEndian(dest[idx..], (int)Shader->Length);
            }
            else
            {
                BinaryPrimitives.WriteInt32LittleEndian(dest[idx..], 0);
            }

            idx += 4;

            if (Shader != null)
            {
                Shader->CopyTo(dest[idx..]);
                idx += (int)Shader->Length;
            }
            else
            {
                idx += 0;
            }

            return idx;
        }

        public int Read(ReadOnlySpan<byte> src, Decoder decoder)
        {
            int idx = 0;
            idx += ReadString(src, out Name, decoder);

            // read macros
            int count = BinaryPrimitives.ReadInt32LittleEndian(src[idx..]);
            idx += 4;
            Macros = new ShaderMacro[count];
            for (var i = 0; i < count; i++)
            {
                idx += ReadString(src[idx..], out string name, decoder);
                idx += ReadString(src[idx..], out string definition, decoder);
                Macros[i] = new ShaderMacro(name, definition);
            }

            int countElements = BinaryPrimitives.ReadInt32LittleEndian(src[idx..]);
            idx += 4;
            InputElements = new InputElementDescription[countElements];
            for (var i = 0; i < countElements; i++)
            {
                idx += ReadString(src[idx..], out string semanticName, decoder);
                idx += ReadInt32(src[idx..], out int semanticIndex);
                idx += ReadInt32(src[idx..], out int format);
                idx += ReadInt32(src[idx..], out int slot);
                idx += ReadInt32(src[idx..], out int alignedByteOffset);
                idx += ReadInt32(src[idx..], out int classification);
                idx += ReadInt32(src[idx..], out int instanceDataStepRate);
                InputElements[i] = new(semanticName, semanticIndex, (Format)format, alignedByteOffset, slot, (InputClassification)classification, instanceDataStepRate);
            }

            int len = BinaryPrimitives.ReadInt32LittleEndian(src[idx..]);
            idx += 4;
            Shader = Alloc<Shader>();
            Shader->Bytecode = Alloc<byte>(len);
            Shader->Length = (nuint)len;
            fixed (void* ptr = src.Slice(idx, len))
            {
                Buffer.MemoryCopy(ptr, Shader->Bytecode, len, len);
            }
            idx += len;
            return idx;
        }

        private static int WriteString(Span<byte> dest, string str, Encoder encoder)
        {
            BinaryPrimitives.WriteInt32LittleEndian(dest, encoder.GetByteCount(str, true));
            return encoder.GetBytes(str, dest[4..], true) + 4;
        }

        private static int ReadString(ReadOnlySpan<byte> src, out string str, Decoder decoder)
        {
            int len = BinaryPrimitives.ReadInt32LittleEndian(src);
            ReadOnlySpan<byte> bytes = src.Slice(4, len);
            int charCount = decoder.GetCharCount(bytes, true);
            Span<char> chars = charCount < 2048 ? stackalloc char[charCount] : new char[charCount];
            decoder.GetChars(bytes, chars, true);
            str = new(chars);
            return len + 4;
        }

        private static int WriteInt32(Span<byte> dest, int value)
        {
            BinaryPrimitives.WriteInt32LittleEndian(dest, value);
            return 4;
        }

        private static int ReadInt32(ReadOnlySpan<byte> src, out int value)
        {
            value = BinaryPrimitives.ReadInt32LittleEndian(src);
            return 4;
        }

        private static int SizeOf(string str, Encoder encoder)
        {
            return 4 + encoder.GetByteCount(str, true);
        }

        public int SizeOf(Encoder encoder)
        {
            if (Shader != null)
            {
                return 20 +
                    SizeOf(Name, encoder) +
                    Macros.Sum(x => SizeOf(x.Name, encoder) + SizeOf(x.Definition, encoder)) +
                    InputElements.Sum(x => SizeOf(x.SemanticName, encoder) + 24) +
                    (int)Shader->Length;
            }
            else
            {
                return 20 +
                    SizeOf(Name, encoder) +
                    Macros.Sum(x => SizeOf(x.Name, encoder) + SizeOf(x.Definition, encoder)) +
                    InputElements.Sum(x => SizeOf(x.SemanticName, encoder) + 24);
            }
        }

        public void Free()
        {
            Utils.Free(Shader);
        }

        public bool Equals(ShaderCacheEntry other)
        {
            if (Name != other.Name)
            {
                return false;
            }

            if (Macros == other.Macros && Macros == null && other.Macros == null)
            {
                return true;
            }

            if (Macros != other.Macros && (Macros == null || other.Macros == null))
            {
                return false;
            }

            if (Macros.Length != (other.Macros?.Length ?? 0))
            {
                return false;
            }

            for (int i = 0; i < Macros.Length; i++)
            {
#nullable disable
                if (Macros[i].Name != other.Macros[i].Name ||
                    Macros[i].Definition != other.Macros[i].Definition)
                {
                    return false;
                }
#nullable enable
            }
            return true;
        }

        public override bool Equals(object? obj)
        {
            return obj is ShaderCacheEntry entry && Equals(entry);
        }

        public static bool operator ==(ShaderCacheEntry left, ShaderCacheEntry right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ShaderCacheEntry left, ShaderCacheEntry right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Macros);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}