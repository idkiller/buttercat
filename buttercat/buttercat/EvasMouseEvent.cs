using ElmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace buttercat
{
    public class EvasMouseUpArgs : EventArgs
    {
        public Point Point { get; set; }

        public static EvasMouseUpArgs Create(IntPtr data, IntPtr obj, IntPtr info)
        {
            if (info == IntPtr.Zero)
                return null;

            try
            {
                var evt = Marshal.PtrToStructure<Evas_Event_Mouse_Up>(info);
                return new EvasMouseUpArgs
                {
                    Point = new Point
                    {
                        X = evt.canvas.x,
                        Y = evt.canvas.y
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Fail to Mouse Up event info marshalling.");
                Console.WriteLine(e);
            }

            return null;
        }
    }

    public class EvasMouseDownArgs : EventArgs
        {
            public Point Point { get; set; }

            public static EvasMouseDownArgs Create(IntPtr data, IntPtr obj, IntPtr info)
            {
                if (info == IntPtr.Zero)
                    return null;

                try
                {
                    var evt = Marshal.PtrToStructure<Evas_Event_Mouse_Down>(info);
                    return new EvasMouseDownArgs
                    {
                        Point = new Point
                        {
                            X = evt.canvas.x,
                            Y = evt.canvas.y
                        }
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Fail to Mouse Down event info marshalling.");
                    Console.WriteLine(e);
                }

                return null;
            }
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Evas_Event_Mouse_Up
    {
        public int button;
        public Evas_Point output;
        public Evas_Coord_Point canvas;
        public IntPtr data;
        public IntPtr modifier;
        public IntPtr locks;
        public Evas_Button_Flags flags;
        public uint timestamp;
        public Evas_Event_Flags event_flags;
        public IntPtr dev;
        public IntPtr event_Src;
        public double radius;
        public double radius_x, radius_y;
        public double pressure, angle;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Evas_Event_Mouse_Down
        {
            public int button;
            public Evas_Point output;
            public Evas_Coord_Point canvas;
            public IntPtr data;
            public IntPtr modifier;
            public IntPtr locks;
            public Evas_Button_Flags flags;
            public uint timestamp;
            public Evas_Event_Flags event_flags;
            public IntPtr dev;
            public IntPtr event_Src;
            public double radius;
            public double radius_x, radius_y;
            public double pressure, angle;
        }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Evas_Point
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Evas_Coord_Point
    {
        public int x;
        public int y;
    }

    [Flags]
    public enum Evas_Button_Flags
    {
        EVAS_BUTTON_NONE = 0,
        EVAS_BUTTON_DOUBLE_CLICK = 1,
        EVAS_BUTTON_TRIPLE_CLICK = 2
    }

    [Flags]
    public enum Evas_Event_Flags
    {
        EVAS_EVENT_FLAG_NONE = 0,
        EVAS_EVENT_FLAG_ON_HOLD = 1,
        EVAS_EVENT_FLAG_ON_SCROLL = 2
    }
}
