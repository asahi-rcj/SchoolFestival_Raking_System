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

        private int MaxNameSize = 8;
        
        private string strSettingName = "";
        private string strSettingScore = "";
        private string[] strChangableChar = { "かきくけこさしすせそたちつてと", "がぎぐげござじずぜぞだぢづでど", "はひふへほ", "ばびぶべぼ", "ぱぴぷぺぽ" };

        public void Draw_SettingDisplay()
        {
            MyDrawStringToHandle(960, 20, "スコア追加", GetColor(255, 255, 255), FontHandle, GetColor(0, 0, 0), true);

            int Button_Size = 100;
            int Button_Size_Score_Width = 150;
            int Button_Interval = (Button_Size / 25);

            #region [ Draw NameSet ] 

            Point Pos_NameSet = new Point(1880, 550);
            Point Pos_NamePlate = new Point(1880, 250);

            #region [ Draw NamePlate ]

            DrawBox(
                Pos_NamePlate.X - (10 + 1) * Button_Size - Button_Interval,
                Pos_NamePlate.Y,
                Pos_NamePlate.X + Button_Interval,
                Pos_NamePlate.Y + 290,
                GetColor(100, 100, 100),
                TRUE);

            DrawBox(
                Pos_NamePlate.X - (10 + 1) * Button_Size - Button_Interval + 10,
                Pos_NamePlate.Y + 10,
                Pos_NamePlate.X + Button_Interval - 10,
                Pos_NamePlate.Y + 290 - 10,
                GetColor(255, 255, 255),
                TRUE);

            MyDrawStringToHandle(800, 265, "名前(最大" + MaxNameSize.ToString() + "文字)", GetColor(255, 255, 255), NameSetTitle_FontHandle, GetColor(0, 0, 0));
            MyDrawStringToHandle(815, 365, strSettingName, GetColor(255, 255, 255), NameSet_FontHandle, GetColor(0, 0, 0));

            #endregion

            DrawBox(
                Pos_NameSet.X - (10 + 1) * Button_Size - Button_Interval,
                Pos_NameSet.Y + (4 + 1) * Button_Size + Button_Interval,
                Pos_NameSet.X + Button_Interval,
                Pos_NameSet.Y - Button_Interval,
                GetColor(100, 100, 100),
                TRUE);

            for (int i = 0; i < NameChar_List.Length / 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if (NameChar_List[i * 5 + j].ToString() != "　")
                    {
                        int pos_x1 = Pos_NameSet.X - i * Button_Size - Button_Interval,
                            pos_y1 = Pos_NameSet.Y + j * Button_Size + Button_Interval,
                            pos_x2 = Pos_NameSet.X - (i + 1) * Button_Size + Button_Interval,
                            pos_y2 = Pos_NameSet.Y + (j + 1) * Button_Size - Button_Interval;

                        string strShowChar = NameChar_List[i * 5 + j].ToString();

                        if (!Program.Mouse.isExistedInArea(pos_x1, pos_y1, pos_x2, pos_y2))
                        {
                            DrawBox(pos_x1, pos_y1, pos_x2, pos_y2,
                                GetColor(230, 230, 230), TRUE);
                        }
                        else
                        {
                            if(Program.Mouse.IsPushedButton(Amaoto.MouseButton.Left))
                            {
                                if (strShowChar == "A")
                                {
                                    if (strSettingName == "") return;

                                    string final_char = strSettingName[strSettingName.Length - 1].ToString();
                                    string now_str = strSettingName.Remove(strSettingName.Length - 1, 1);

                                    if (strChangableChar[0].Contains(final_char))    //通常濁音に変換可能な文字が最後に存在していたら
                                    {
                                        now_str += strChangableChar[1][strChangableChar[0].IndexOf(final_char)].ToString();
                                        strSettingName = now_str;
                                    }
                                    else if(strChangableChar[1].Contains(final_char)) //通常非濁音に変換可能な文字が最後に存在していたら
                                    {
                                        now_str += strChangableChar[0][strChangableChar[1].IndexOf(final_char)].ToString();
                                        strSettingName = now_str;
                                    }
                                    else if (strChangableChar[2].Contains(final_char)) //は行に変換可能な文字が最後に存在していたら
                                    {
                                        now_str += strChangableChar[3][strChangableChar[2].IndexOf(final_char)].ToString();
                                        strSettingName = now_str;
                                    }
                                    else if (strChangableChar[3].Contains(final_char)) //ば行に変換可能な文字が最後に存在していたら
                                    {
                                        now_str += strChangableChar[2][strChangableChar[3].IndexOf(final_char)].ToString();
                                        strSettingName = now_str;
                                    }
                                    else if (strChangableChar[4].Contains(final_char)) //ぱ行に変換可能な文字が最後に存在していたら
                                    {
                                        now_str += strChangableChar[3][strChangableChar[4].IndexOf(final_char)].ToString();
                                        strSettingName = now_str;
                                    }
                                }
                                else if (strShowChar == "B")
                                {
                                    if (strSettingName == "") return;

                                    string final_char = strSettingName[strSettingName.Length - 1].ToString();
                                    string now_str = strSettingName.Remove(strSettingName.Length - 1, 1);

                                    if (strChangableChar[2].Contains(final_char))    //濁音に変換可能な文字が最後に存在していたら
                                    {
                                        now_str += strChangableChar[4][strChangableChar[2].IndexOf(final_char)].ToString();
                                        strSettingName = now_str;
                                    }
                                    else if (strChangableChar[3].Contains(final_char))
                                    {
                                        now_str += strChangableChar[4][strChangableChar[3].IndexOf(final_char)].ToString();
                                        strSettingName = now_str;
                                    }
                                    else if (strChangableChar[4].Contains(final_char))
                                    {
                                        now_str += strChangableChar[2][strChangableChar[4].IndexOf(final_char)].ToString();
                                        strSettingName = now_str;
                                    }
                                }
                                else if (strShowChar == "C")
                                {
                                    if (strSettingName == "") return;

                                    strSettingName = strSettingName.Remove(strSettingName.Length - 1, 1);
                                }
                                else
                                {
                                    if (strSettingName.Length == MaxNameSize) return;

                                    strSettingName += strShowChar;
                                }
                            }

                            DrawBox(pos_x1, pos_y1, pos_x2, pos_y2, GetColor(255, 255, 255), TRUE);
                        }

                        if (strShowChar == "A")
                        {
                            strShowChar = "゛";
                        }
                        else if (strShowChar == "B")
                        {
                            strShowChar = "゜";
                        }
                        else if (strShowChar == "C")
                        {
                            strShowChar = "×";
                        }

                        MyDrawStringToHandle(pos_x2 + 17, pos_y1 + 19, strShowChar, GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));
                    }
                }
            }

            #endregion

            #region [ Draw ScoreSet ]

            Point Pos_ScoreSet = new Point(750, 550);
            Point Pos_ScorePlate = new Point(750, 250);

            #region [ Draw NamePlate ]

            DrawBox(
                40,
                Pos_ScorePlate.Y,
                Pos_ScorePlate.X + Button_Interval,
                Pos_ScorePlate.Y + 290,
                GetColor(100, 100, 100),
                TRUE);

            DrawBox(
                50,
                Pos_ScorePlate.Y + 10,
                Pos_ScorePlate.X + Button_Interval - 10,
                Pos_ScorePlate.Y + 290 - 10,
                GetColor(255, 255, 255),
                TRUE);

            MyDrawStringToHandle(60, 265, "スコア", GetColor(255, 255, 255), NameSetTitle_FontHandle, GetColor(0, 0, 0));
            MyDrawStringToHandle(815, 365, strSettingScore, GetColor(255, 255, 255), NameSet_FontHandle, GetColor(0, 0, 0));

            #endregion

            DrawBox(
                Pos_ScoreSet.X - (2 + 1) * Button_Size_Score_Width - Button_Interval,
                Pos_ScoreSet.Y + (4 + 1) * Button_Size + Button_Interval,
                Pos_ScoreSet.X + Button_Interval,
                Pos_ScoreSet.Y - Button_Interval,
                GetColor(100, 100, 100),
                TRUE);

            for (int i = 0; i < ScoreChar_Lists.Length / 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    string strShowChar = ScoreChar_Lists[i * 5 + j].ToString();

                    if (strShowChar != "　")
                    {
                        int pos_x1 = Pos_ScoreSet.X - i * Button_Size_Score_Width - Button_Interval,
                            pos_y1 = Pos_ScoreSet.Y + j * Button_Size + Button_Interval,
                            pos_x2 = Pos_ScoreSet.X - (i + 1) * Button_Size_Score_Width + Button_Interval,
                            pos_y2 = Pos_ScoreSet.Y + (j + 1) * Button_Size - Button_Interval;

                        if (!Program.Mouse.isExistedInArea(pos_x1, pos_y1, pos_x2, pos_y2))
                        {
                            DrawBox(pos_x1, pos_y1, pos_x2, pos_y2, GetColor(230, 230, 230), TRUE);
                        }
                        else
                        {
                            if (Program.Mouse.IsPushedButton(Amaoto.MouseButton.Left))
                            {
                            }

                            DrawBox(pos_x1, pos_y1, pos_x2, pos_y2, GetColor(255, 255, 255), TRUE);
                        }

                        if(strShowChar == "A")
                        {
                            strShowChar = "×";
                        }

                        MyDrawStringToHandle(pos_x2 + 17 + 25, pos_y1 + 19, strShowChar, GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0)) ;
                    }
                }
            }

            #endregion

            #region [ Draw ScoreSave And ScoreRestore ]

            Point Pos_ContPlate = new Point(750, 546);

            DrawBox(40, Pos_ContPlate.Y, 270, 700, GetColor(255, 100, 100), TRUE);
            DrawBox(40, 710, 270, 1054, GetColor(255, 100, 100), TRUE);

            #endregion
        }

        private string NameChar_List = "！？ABCあいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもや　ゆ　よらりるれろわをん　　";
        private string ScoreChar_Lists = "Ｃ９６３Ａ　８５２.　７４１０";

        private bool isCreatingScoreData = false;

        #region [ Font ] 

        private int FontHandle = CreateFontToHandle("HG創英角ﾎﾟｯﾌﾟ体", 80, 0, DX_FONTTYPE_ANTIALIASING_EDGE_8X8, -1, 5);
        private int DataGrid_FontHandle = CreateFontToHandle("HG創英角ﾎﾟｯﾌﾟ体", 50, 0, DX_FONTTYPE_ANTIALIASING_EDGE_8X8, -1, 4);

        private int NameSet_FontHandle = CreateFontToHandle("HG創英角ﾎﾟｯﾌﾟ体", 115, 0, DX_FONTTYPE_ANTIALIASING_EDGE_8X8, -1, 7);
        private int NameSetTitle_FontHandle = CreateFontToHandle("HG創英角ﾎﾟｯﾌﾟ体", 65, 0, DX_FONTTYPE_ANTIALIASING_EDGE_8X8, -1, 4);

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
