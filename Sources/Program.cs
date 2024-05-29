#pragma warning disable CS0465 // Finalize' メソッドを導入すると、デストラクターの呼び出しに影響する可能性があります

using Amaoto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DxLibDLL.DX;

namespace SchoolFestival_Raking_System.Sources
{
    internal class Program
    {
        static bool isFullScreen = false;

        static void Main()
        {
            Initialize();

            while(ProcessMessage() == 0)
            {
                ClearDrawScreen();

                Input.Update();
                Mouse.Update();


                Main_Display.Draw();

                SetDrawBlendMode(DX_BLENDMODE_ALPHA, 196);
                DrawCircle(Mouse.Point.x, Mouse.Point.y, 10, GetColor(30, 30, 30), TRUE);
                SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0);

                if (Input.IsPushedKey(KEY_INPUT_F11))
                {
                    isFullScreen = !isFullScreen;

                    if (isFullScreen)
                    {
                        ChangeWindowMode(FALSE);
                    }
                    else
                    {
                        ChangeWindowMode(TRUE);
                    }
                }

                ScreenFlip();
            }

            Finalize();
        }

        public static Texture Background;

        public static Main Main_Display;
        public static Data Data;

        public static Amaoto.Input Input;
        public static Amaoto.Mouse Mouse;

        static void Initialize()
        {
            #region [ Initialize DxLib ]

            unsafe
            {
                SetUseASyncChangeWindowModeFunction(TRUE, null, null);
                SetChangeScreenModeGraphicsSystemResetFlag(FALSE);
            }
            SetUseTransColor(FALSE);
            SetWindowText("SchoolFestival Ranking System");
            SetGraphMode(1920, 1080, 32);
            SetWindowSize(1280, 720);
            ChangeWindowMode(TRUE);
            SetAlwaysRunFlag(TRUE);
            SetWaitVSyncFlag(FALSE);
            SetOutApplicationLogValidFlag(FALSE);
            SetUseTransColor(FALSE);
            SetDoubleStartValidFlag(TRUE);
            if (DxLib_Init() == -1) return;
            SetDrawScreen(DX_SCREEN_BACK);

            #endregion

            Main_Display = new Main();
            Data = new Data();

            Input = new Amaoto.Input();
            Mouse = new Amaoto.Mouse();

            Background = new Texture("data.png");
        }

        static void Finalize()
        {
            DxLib_End();
        }
    }
}
