using ScrollsModLoader.Interfaces;
using Mono.Cecil;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine;
 

namespace BorderlessWindow
{
	public class BorderlessWindow : BaseMod
	{

        public Rect screenPosition;

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        const uint SWP_SHOWWINDOW = 0x0040;
        const int GWL_STYLE = -16;
        const int WS_BORDER = 1;

		//initialize everything here, Game is loaded at this point
		public BorderlessWindow ()
		{
		}


		public static string GetName ()
		{
            return "BW";
		}

		public static int GetVersion ()
		{
            return 1;
		}

		//only return MethodDefinitions you obtained through the scrollsTypes object
		//safety first! surround with try/catch and return an empty array in case it fails
		public static MethodDefinition[] GetHooks (TypeDefinitionCollection scrollsTypes, int version)
		{
            try
            {
                return new MethodDefinition[] {
                    scrollsTypes["M"].Methods.GetMethod("ChatMessage", new Type[]{typeof(RoomChatMessageMessage)}),
            	};
            }
            catch
            {
                Console.WriteLine("Fail van!");
                return new MethodDefinition[] { };
            }
		}

		
		public override bool BeforeInvoke (InvocationInfo info, out object returnValue)
		{

            //Gotta fetch screen resolution somehow

            returnValue = null;
            SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_BORDER);
            SetWindowPos(GetForegroundWindow(), 0, 0, 0, (int)1280, (int)720, SWP_SHOWWINDOW);
            return false;
		}

		public override void AfterInvoke (InvocationInfo info, ref object returnValue)
		{


			throw new NotImplementedException ();
		}
	}
}

