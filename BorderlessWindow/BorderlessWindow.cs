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

        public int unityScreenWidth = UnityEngine.Screen.width;
        public int unityScreenHeight = UnityEngine.Screen.height;
        public IntPtr handle = FindWindow(null, "Scrolls");
    
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
                return new MethodDefinition[] {      scrollsTypes["MainMenu"].Methods.GetMethod("Start")[0],
                };
               
            }
            catch
            {
                return new MethodDefinition[] { };
            }
        }


        public override bool BeforeInvoke(InvocationInfo info, out object returnValue)
        {
            returnValue = null;

             if(info.targetMethod.Equals("Start")){

                SetWindowLong(handle, GWL_STYLE, WS_BORDER);
                SetWindowPos(handle, 0, 0, 0, unityScreenWidth, unityScreenHeight, SWP_SHOWWINDOW);
             }
          

            return false;
        }

        public override void AfterInvoke(InvocationInfo info, ref object returnValue)
        {
            return;
        }
    }
}

