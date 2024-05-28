using System;
using System.Collections.Generic;
using System.Drawing;
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
            DrawBox(0, 0, 1920, 1080, GetColor(255, 255, 255), TRUE);

            if (!isCreatingScoreData)
            {
                Draw_SettingDisplay();
            }
            else
            {
                Draw_MainDisplay();
            }
        }

        public void Draw_MainDisplay()
        {
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
            MyDrawStringToHandle(960, 20, "スコア追加", GetColor(255, 255, 255), FontHandle, GetColor(0, 0, 0), true);

            #region [ Draw NameSet ] 

            Point Pos_NameSet = new Point(1880, 550);
            int Button_Size = 100;
            int Button_Interval = (Button_Size / 25);

            DrawBox(
                Pos_NameSet.X - (10 + 1) * Button_Size - Button_Interval,
                Pos_NameSet.Y + (4 + 1) * Button_Size + Button_Interval,
                Pos_NameSet.X + Button_Interval,
                Pos_NameSet.Y - Button_Interval,
                GetColor(50, 50, 50),
                TRUE);

            for(int i = 0; i < NameChar_List.Length / 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if (NameChar_List[i * 5 + j].ToString() != "　")
                    {
                        DrawBox(
                            Pos_NameSet.X - i * Button_Size - Button_Interval,
                            Pos_NameSet.Y + j * Button_Size + Button_Interval,
                            Pos_NameSet.X - (i + 1) * Button_Size + Button_Interval,
                            Pos_NameSet.Y + (j + 1) * Button_Size - Button_Interval,
                            GetColor(230, 230, 230),
                            TRUE);
                    }
                }
            }

            #endregion 
        }

        private string NameChar_List = "ABCDEあいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもや　ゆ　よらりるれろわをん　　";

        private bool isCreatingScoreData = false;

        #region [ Font ] 

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

        #endregion
    }
}
