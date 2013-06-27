using ScrollsModLoader.Interfaces;
using Mono.Cecil;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Interop;
using System.Drawing;   
using UnityEngine;
using System.Windows;
using System.Windows.Forms;


namespace BorderlessWindow
{
    public class BorderlessWindow : BaseMod
    {
        //public Rect screenPosition;

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lp1, string lp2);
    
        const uint SWP_SHOWWINDOW = 0x0040;
        const int GWL_STYLE = -16;
        const int WS_BORDER = 1;

        public BorderlessWindow()
        {
        }


        public static string GetName()
        {
            return "BW";
        }

        public static int GetVersion()
        {
            return 1;
        }

        //only return MethodDefinitions you obtained through the scrollsTypes object
        //safety first! surround with try/catch and return an empty array in case it fails
        public static MethodDefinition[] GetHooks(TypeDefinitionCollection scrollsTypes, int version)
        {
            try
            {
                return new MethodDefinition[] {
                    scrollsTypes["Login"].Methods.GetMethod("loadSettings")[0],
            	};
            }
            catch
            {
                Console.WriteLine("Fail van!");
                return new MethodDefinition[] { };
            }
        }


        public override bool BeforeInvoke(InvocationInfo info, out object returnValue)
        {
            returnValue = null;

            int unityScreenWidth = UnityEngine.Screen.width;
            int unityScreenHeight = UnityEngine.Screen.height;

           // int asd =  System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;

            IntPtr handle = FindWindow(null, "Scrolls");

            SetWindowLong(handle, GWL_STYLE, WS_BORDER);
            SetWindowPos(handle, 0, 0, 0, unityScreenWidth, unityScreenHeight, SWP_SHOWWINDOW);
            

            return false;
        }

        public override void AfterInvoke(InvocationInfo info, ref object returnValue)
        {
            throw new NotImplementedException();
        }
    }
}

