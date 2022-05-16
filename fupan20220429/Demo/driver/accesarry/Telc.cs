using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace Demo.driver.cam
{
    public struct SendStruct1
    {
        public UInt32 FrameStart;
        public UInt32 PartNum;
        public byte WorkNo;
        public byte CapNo;
        public UInt16 ScoreLeng;
        public UInt16 Type;
        public UInt16 Res;
        public double Xpos;
        public double Ypos;
        public double Zpos;
        public UInt32 FrameEnd;
    };

    public struct SendStruct
    {
        public UInt32 FrameStart;
        public UInt32 PartNum;
        public byte WorkNo;
        public byte CapNo;
        public UInt16 ScoreLeng;
        public UInt16 Type;
        public UInt16 Res;
        public Int32 Xpos;
        public Int32 Ypos;
        public Int32 Zpos;
        public UInt32 FrameEnd;
    };

    public struct RecStruct
    {
        public UInt32 FrameStart;
        public UInt32 PartNum;
        public byte WorkNo;
        public byte CapNo;
        public UInt16 CMD;
        public UInt32 FrameEnd;
    };
    public class Telc
    {
        public static byte[] StructureToByte<T>(T structure)
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[size];
            IntPtr bufferIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, bufferIntPtr, false);
                Marshal.Copy(bufferIntPtr, buffer, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(bufferIntPtr);
            }
            return buffer;
        }

        public static T ByteToStructure<T>(byte[] dataBuffer)
        {
            object structure = null;
            int size = Marshal.SizeOf(typeof(T));
            IntPtr allocIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(dataBuffer, 0, allocIntPtr, size);
                structure = Marshal.PtrToStructure(allocIntPtr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(allocIntPtr);
            }
            return (T)structure;
        }


    }
}
