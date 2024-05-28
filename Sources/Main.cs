using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DxLibDLL.DX;

namespace SchoolFestival_Raking_System.Sources
{
    internal class Main
    {
        //メイン画面の実装を行います。

        public void Draw()
        {
            Draw_MainDisplay();
        }

        public void Draw_MainDisplay()
        {
            DrawBox(0, 0, 1920, 1080, GetColor(255, 255, 255), TRUE);

            MyDrawStringToHandle(960, 20, "紙飛行機 スコアランキング速報", GetColor(255, 255, 255), FontHandle, GetColor(0, 0, 0), true);

            #region [ Draw DataGrid ]

            int origin_x = 100, origin_y = 250;

            int[] row_width = { 100, 400, 300 };
            int[] row_x = { 0, row_width[0], row_width[0] + row_width[1] };
            int column_height = 72;

            #region [ Draw DataGrid Base ]

            DrawBox(
                origin_x - 2,
                origin_y - 2,
                origin_x + row_x[2] + row_width[2],
                origin_y + (10 + 1) * column_height,
                GetColor(0, 0, 0),
                TRUE
                );

            for (int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 11; j++)
                {
                    DrawBox(
                        origin_x + row_x[i], 
                        origin_y + j * column_height, 
                        origin_x + row_x[i] + row_width[i] - 2, 
                        origin_y + (j + 1) * column_height - 2, 
                        GetColor(200, 200, 200), 
                        TRUE);
                }
            }

            #endregion

            #region [ Draw DataGrid HeaderCell ]

            MyDrawStringToHandle(origin_x + 13, origin_y + 10, "No", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));
            MyDrawStringToHandle(origin_x + 13 + row_x[1], origin_y + 10, "Name", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));
            MyDrawStringToHandle(origin_x + 13 + row_x[2], origin_y + 10, "Score", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));

            #endregion

            #region [ Draw DataGrid CellData ]

            #region [ No ]

            for (int i = 0; i < 10; i++)
            {
                MyDrawStringToHandle(origin_x + 13, origin_y + 10 + (i + 1) * column_height, (i + 1).ToString(), GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));
            }

            #endregion

            #endregion

            #endregion
        }

        public void Draw_SettingDisplay()
        {

        }

        private int FontHandle = CreateFontToHandle("HG創英角ﾎﾟｯﾌﾟ体", 80, 0, DX_FONTTYPE_ANTIALIASING_EDGE_8X8, -1, 5);
        private int DataGrid_FontHandle = CreateFontToHandle("HG創英角ﾎﾟｯﾌﾟ体", 50, 0, DX_FONTTYPE_ANTIALIASING_EDGE_8X8, -1, 4);

        private void MyDrawStringToHandle(int x, int y, string text, uint color, int handle, uint edgecolor, bool x_center = false, bool y_center = false)
        {
            int width, height, line;
            GetDrawStringSizeToHandle(out width, out height, out line, text, text.Length, handle);

            int adjust_x, adjust_y;

            adjust_x = x_center ? -width / 2 : 0;
            adjust_y = y_center ? -height / 2 : 0;

            DrawStringToHandle(x + adjust_x, y + adjust_y, text, color, handle, edgecolor);
        }
    }
}
