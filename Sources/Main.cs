using Amaoto;
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
            Program.Background.Draw(0, 0);

            if (isCreatingScoreData)
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
            MyDrawStringToHandle(960, 10, "紙飛行機 スコアランキング速報", GetColor(255, 255, 255), FontHandle, GetColor(150, 150, 255), true);

            int[] row_width = { 100, 400, 300 };
            int[] row_x = { 0, row_width[0], row_width[0] + row_width[1], row_width[0] + row_width[1] + row_width[2] };
            int column_height = 68;

            #region [ Draw DataGrid ]

            int origin_x_first = 100, origin_y_first = 300;

            MyDrawStringToHandle(265, 200, "飛行距離部門", GetColor(255, 255, 255), DataGridTitle_FontHandle, GetColor(0, 0, 0));

            #region [ Draw DataGrid Base ]

            SetDrawBlendMode(DX_BLENDMODE_ALPHA, 212);

            for (int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 11; j++)
                {
                    uint color = GetColor(240, 240, 240);

                    if (j == 1)
                        color = GetColor(230, 180, 34);
                    else if(j == 2)
                        color = GetColor(192, 192, 192);
                    else if (j == 3)
                        color = GetColor(196, 112, 34);

                    DrawBox(
                        origin_x_first + row_x[i], 
                        origin_y_first + j * column_height, 
                        origin_x_first + row_x[i] + row_width[i] - 2, 
                        origin_y_first + (j + 1) * column_height - 2,
                        color, 
                        TRUE);
                }
            }

            SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0);

            #endregion

            #region [ Draw DataGrid HeaderCell ]

            MyDrawStringToHandle(origin_x_first + 13, origin_y_first + 10, "No", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));
            MyDrawStringToHandle(origin_x_first + 13 + row_x[1], origin_y_first + 10, "Name", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));
            MyDrawStringToHandle(origin_x_first + 13 + row_x[2], origin_y_first + 10, "Score", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));

            #endregion

            #region [ Draw DataGrid CellData ]

            for (int i = 0; i < 10; i++)
            {
                uint color = GetColor(0, 0, 0);

                if (i == 0)
                    color = GetColor(230 - 60, 180 - 60, 0);
                else if (i == 1)
                    color = GetColor(192 - 60, 192 - 60, 192 - 60);
                else if (i == 2)
                    color = GetColor(196 - 60, 112 - 60, 0);

                //No
                MyDrawStringToHandle(origin_x_first + 13, origin_y_first + 6 + (i + 1) * column_height, (i + 1).ToString(), GetColor(255, 255, 255), DataGrid_FontHandle, color);
                
                if(Program.Data.Flying_Distance_Data.Count - 1 >= i)
                {
                    //Name
                    MyDrawStringToHandle(origin_x_first + 13 + row_x[1], origin_y_first + 6 + (i + 1) * column_height,
                        Program.Data.Flying_Distance_Data[i].Name, GetColor(255, 255, 255), DataGrid_FontHandle, color);

                    //Score
                    MyDrawStringToHandle(origin_x_first + 13 + row_x[2], origin_y_first + 6 + (i + 1) * column_height,
                        Program.Data.Flying_Distance_Data[i].Score.ToString(), GetColor(255, 255, 255), DataGrid_FontHandle, color);
                }
            }

            #endregion

            #region [ Draw DataGrid DataRemove ]

            for (int i = 0; i < 10; i++)
            {
                //origin_x_first + 13 + row_x[1], origin_y_first + 6 + (i + 1) * column_height

                if (Program.Data.Flying_Distance_Data.Count <= i) continue;

                SetDrawBlendMode(DX_BLENDMODE_ALPHA, 212);

                if (Program.Mouse.isExistedInArea(
                    origin_x_first + row_x[3] + 6, origin_y_first + (i + 1) * column_height,
                    origin_x_first + row_x[3] + 6 + 60, origin_y_first + (i + 1) * column_height + 60))
                {
                    if(Program.Mouse.IsPushedButton(MouseButton.Left))
                    {
                        Program.Data.Flying_Distance_Data.RemoveAt(i);
                    }

                    DrawRoundRect(origin_x_first + row_x[3] + 6, origin_y_first + (i + 1) * column_height,
                                  origin_x_first + row_x[3] + 6 + 60, origin_y_first + (i + 1) * column_height + 60,
                                  5, 5,
                                  GetColor(255, 150, 150),
                                  TRUE);
                }
                else
                {
                    DrawRoundRect(origin_x_first + row_x[3] + 6, origin_y_first + (i + 1) * column_height,
                                  origin_x_first + row_x[3] + 6 + 60, origin_y_first + (i + 1) * column_height + 60,
                                  5, 5,
                                  GetColor(255, 100, 100),
                                  TRUE);
                }

                SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0);

                MyDrawStringToHandle(origin_x_first + row_x[3] + 7, origin_y_first + (i + 1) * column_height + 2, "×", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(255, 255, 255));
            }

            #endregion

            #endregion

            #region [ Draw DataGrid ]

            int origin_x_second = 960 + 60, origin_y_second = 300;

            MyDrawStringToHandle(225 + 960, 200, "飛行時間部門", GetColor(255, 255, 255), DataGridTitle_FontHandle, GetColor(0, 0, 0));

            #region [ Draw DataGrid Base ]

            SetDrawBlendMode(DX_BLENDMODE_ALPHA, 212);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    uint color = GetColor(240, 240, 240);

                    if (j == 1)
                        color = GetColor(230, 180, 34);
                    else if (j == 2)
                        color = GetColor(192, 192, 192);
                    else if (j == 3)
                        color = GetColor(196, 112, 34);

                    DrawBox(
                        origin_x_second + row_x[i],
                        origin_y_second + j * column_height,
                        origin_x_second + row_x[i] + row_width[i] - 2,
                        origin_y_second + (j + 1) * column_height - 2,
                        color,
                        TRUE);
                }
            }

            SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0);

            #endregion

            #region [ Draw DataGrid HeaderCell ]

            MyDrawStringToHandle(origin_x_second + 13, origin_y_second + 10, "No", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));
            MyDrawStringToHandle(origin_x_second + 13 + row_x[1], origin_y_second + 10, "Name", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));
            MyDrawStringToHandle(origin_x_second + 13 + row_x[2], origin_y_second + 10, "Score", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));

            #endregion

            #region [ Draw DataGrid CellData ]

            #region [ No ]

            for (int i = 0; i < 10; i++)
            {
                uint color = GetColor(0, 0, 0);

                if (i == 0)
                    color = GetColor(230 - 60, 180 - 60, 0);
                else if (i == 1)
                    color = GetColor(192 - 60, 192 - 60, 192 - 60);
                else if (i == 2)
                    color = GetColor(196 - 60, 112 - 60, 0);

                MyDrawStringToHandle(origin_x_second + 13, origin_y_second + 6 + (i + 1) * column_height, (i + 1).ToString(), GetColor(255, 255, 255), DataGrid_FontHandle, color);

                if (Program.Data.Flying_Time_Data.Count - 1 >= i)
                {
                    //Name
                    MyDrawStringToHandle(origin_x_second + 13 + row_x[1], origin_y_second + 6 + (i + 1) * column_height,
                        Program.Data.Flying_Time_Data[i].Name, GetColor(255, 255, 255), DataGrid_FontHandle, color);

                    //Score
                    MyDrawStringToHandle(origin_x_second + 13 + row_x[2], origin_y_second + 6 + (i + 1) * column_height,
                        Program.Data.Flying_Time_Data[i].Score.ToString(), GetColor(255, 255, 255), DataGrid_FontHandle, color);
                }
            }

            #endregion

            #endregion

            #region [ Draw DataGrid DataRemove ]

            for (int i = 0; i < 10; i++)
            {
                //origin_x_first + 13 + row_x[1], origin_y_first + 6 + (i + 1) * column_height

                if (Program.Data.Flying_Time_Data.Count <= i) continue;

                SetDrawBlendMode(DX_BLENDMODE_ALPHA, 212);

                if (Program.Mouse.isExistedInArea(
                    origin_x_second + row_x[3] + 6, origin_y_second + (i + 1) * column_height,
                    origin_x_second + row_x[3] + 6 + 60, origin_y_second + (i + 1) * column_height + 60))
                {
                    if (Program.Mouse.IsPushedButton(MouseButton.Left))
                    {
                        Program.Data.Flying_Time_Data.RemoveAt(i);
                    }

                    DrawRoundRect(origin_x_second + row_x[3] + 6, origin_y_second + (i + 1) * column_height,
                                  origin_x_second + row_x[3] + 6 + 60, origin_y_second + (i + 1) * column_height + 60,
                                  5, 5,
                                  GetColor(255, 150, 150),
                                  TRUE);
                }
                else
                {
                    DrawRoundRect(origin_x_second + row_x[3] + 6, origin_y_second + (i + 1) * column_height,
                                  origin_x_second + row_x[3] + 6 + 60, origin_y_second + (i + 1) * column_height + 60,
                                  5, 5,
                                  GetColor(255, 100, 100),
                                  TRUE);
                }

                SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0);

                MyDrawStringToHandle(origin_x_second + row_x[3] + 7, origin_y_second + (i + 1) * column_height + 2, "×", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(255, 255, 255));
            }

            #endregion

            #endregion


            #region [ Draw ScoreAdd ]

            #region [ Flying_Distance ]

            DrawBox(20, 120, 240, 280, GetColor(190, 190, 0), TRUE);

            if(Program.Mouse.isExistedInArea(20 + 5, 120 + 5, 240 - 5, 280 - 5))
            {
                if (Program.Mouse.IsPushedButton(MouseButton.Left))
                {
                    this.eScoreSet_Type = ScoreSet_Type.Flying_Distance;
                    this.isCreatingScoreData = true;

                    this.Initialize_SettingDisplay();
                }

                DrawBox(20 + 5, 120 + 5, 240 - 5, 280 - 5, GetColor(205, 205, 205), TRUE);
            }
            else
            {
                DrawBox(20 + 5, 120 + 5, 240 - 5, 280 - 5, GetColor(255, 255, 255), TRUE);
            }

            MyDrawStringToHandle(20 + 20, 120 + 20, "スコア\n 追加", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));

            #endregion

            #region [ Flying_Time ]

            DrawBox(1920 - 20, 120, 1920 - 240, 280, GetColor(190, 190, 0), TRUE);

            if (Program.Mouse.isExistedInArea(1920 - 20 - 5, 120 + 5, 1920 - 240 + 5, 280 - 5))
            {
                if(Program.Mouse.IsPushedButton(MouseButton.Left))
                {
                    this.eScoreSet_Type = ScoreSet_Type.Flying_Time;
                    this.isCreatingScoreData = true;

                    this.Initialize_SettingDisplay();
                }

                DrawBox(1920 - 20 - 5, 120 + 5, 1920 - 240 + 5, 280 - 5, GetColor(205, 205, 205), TRUE);
            }
            else
            {
                DrawBox(1920 - 20 - 5, 120 + 5, 1920 - 240 + 5, 280 - 5, GetColor(255, 255, 255), TRUE);
            }

            MyDrawStringToHandle(1920 - 20 - 20 - 180, 120 + 20, "スコア\n 追加", GetColor(255, 255, 255), DataGrid_FontHandle, GetColor(0, 0, 0));

            #endregion
            #endregion

        }

        public void Initialize_SettingDisplay()
        {
            strSettingName = "";
            strSettingScore = "0";
        }

        public void Draw_SettingDisplay()
        {
            MyDrawStringToHandle(960, 80, "スコア追加", GetColor(255, 255, 255), FontHandle, GetColor(0, 0, 0), true);

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
            MyDrawStringToHandle(705, 365, strSettingScore, GetColor(255, 255, 255), NameSet_FontHandle, GetColor(0, 0, 0), false, false, true);

            #endregion

            DrawBox(
                Pos_ScoreSet.X - (2 + 1) * Button_Size_Score_Width - Button_Interval,
                Pos_ScoreSet.Y + (4 + 1) * Button_Size + Button_Interval,
                Pos_ScoreSet.X + Button_Interval,
                Pos_ScoreSet.Y - Button_Interval,
                GetColor(100, 100, 100),
                TRUE);

            for (int i = 0; i < ScoreChar_List.Length / 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    string strShowChar = ScoreChar_List_UseOnly[i * 5 + j].ToString();
                    string strProcessChar = ScoreChar_List[i * 5 + j].ToString();

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
                                if (strProcessChar == "C")
                                {
                                    strSettingScore = "0";
                                }
                                else if (strProcessChar == "A")
                                {
                                    if (strSettingScore == "0") continue;

                                    strSettingScore = strSettingScore.Remove(strSettingScore.Length - 1, 1);

                                    if (strSettingScore == "")
                                        strSettingScore = "0";
                                }
                                else if (strProcessChar == "0")
                                {
                                    string strFinalChar = strSettingScore[strSettingScore.Length - 1].ToString();

                                    if (strFinalChar == "0" && strFinalChar.Length == 1) continue;

                                    strSettingScore += "0";
                                }
                                else if (strProcessChar == ".")
                                {
                                    if (strSettingScore.Contains('.')) continue;

                                    strSettingScore += strProcessChar;
                                }
                                else
                                {
                                    if (strSettingScore[0] == '0' && strSettingScore.Length == 1)
                                        strSettingScore = "";

                                    strSettingScore += strProcessChar;
                                }
                            }

                            DrawBox(pos_x1, pos_y1, pos_x2, pos_y2, GetColor(255, 255, 255), TRUE);
                        }

                        if(strShowChar == "Ａ")
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

            if(Program.Mouse.isExistedInArea(40, Pos_ContPlate.Y, 270, 700))
            {
                if (Program.Mouse.IsPushedButton(MouseButton.Left))
                {
                    isCreatingScoreData = false;
                }

                DrawBox(40, Pos_ContPlate.Y, 270, 700, GetColor(255, 150, 150), TRUE);
            }
            else
            {
                DrawBox(40, Pos_ContPlate.Y, 270, 700, GetColor(255, 100, 100), TRUE);
            }

            if (Program.Mouse.isExistedInArea(40, 710, 270, 1054))
            {
                if(Program.Mouse.IsPushedButton(MouseButton.Left))
                {
                    if(strSettingName != "")
                    {
                        if (eScoreSet_Type == ScoreSet_Type.Flying_Distance)
                        {
                            Data.RankingData data = new Data.RankingData();

                            data.Name = this.strSettingName;
                            data.Score = int.Parse(this.strSettingScore);

                            Program.Data.Flying_Distance_Data.Add(data);

                            Program.Data.Sort_List(ref Program.Data.Flying_Distance_Data);
                        }
                        else if (eScoreSet_Type == ScoreSet_Type.Flying_Time)
                        {
                            Data.RankingData data = new Data.RankingData();

                            data.Name = this.strSettingName;
                            data.Score = int.Parse(this.strSettingScore);

                            Program.Data.Flying_Time_Data.Add(data);

                            Program.Data.Sort_List(ref Program.Data.Flying_Time_Data);
                        }

                        isCreatingScoreData = false;
                    }
                }

                DrawBox(40, 710, 270, 1054, GetColor(150, 255, 150), TRUE);
            }
            else
            {
                DrawBox(40, 710, 270, 1054, GetColor(100, 255, 100), TRUE);
            }


            MyDrawStringToHandle(40 + 10, Pos_ContPlate.Y + 35, "閉じる", GetColor(255, 255, 255), NameSetTitle_FontHandle, GetColor(0, 0, 0));
            MyDrawStringToHandle(44, 710 + 90, "スコア", GetColor(255, 255, 255), NameSetTitle_FontHandle, GetColor(0, 0, 0));
            MyDrawStringToHandle(44 + 65 / 2, 710 + 65 + 90, "登録", GetColor(255, 255, 255), NameSetTitle_FontHandle, GetColor(0, 0, 0));

            #endregion
        }

        //警告作成？
        private Counter ctWarningAnime;

        private int MaxNameSize = 6;

        private string strSettingName = "";
        private string strSettingScore = "0";
        private string[] strChangableChar = { "かきくけこさしすせそたちつてと", "がぎぐげござじずぜぞだぢづでど", "はひふへほ", "ばびぶべぼ", "ぱぴぷぺぽ" };

        private string NameChar_List = "！？ABCあいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもや　ゆ　よらりるれろわをん　　";
        private string ScoreChar_List = "C963A 852. 7410";
        private string ScoreChar_List_UseOnly = "Ｃ９６３Ａ　８５２.　７４１０";

        private bool isCreatingScoreData = false;

        private ScoreSet_Type eScoreSet_Type = ScoreSet_Type.Flying_Distance;

        private enum ScoreSet_Type
        {
            Flying_Distance,
            Flying_Time,
        }

        #region [ Font ] 

        private int FontHandle = CreateFontToHandle("HG創英角ﾎﾟｯﾌﾟ体", 90, 0, DX_FONTTYPE_ANTIALIASING_EDGE_8X8, -1, 7);
        private int DataGrid_FontHandle = CreateFontToHandle("HG創英角ﾎﾟｯﾌﾟ体", 50, 0, DX_FONTTYPE_ANTIALIASING_EDGE_8X8, -1, 4);
        private int DataGridTitle_FontHandle = CreateFontToHandle("HG創英角ﾎﾟｯﾌﾟ体", 70, 0, DX_FONTTYPE_ANTIALIASING_EDGE_8X8, -1, 5);

        private int NameSet_FontHandle = CreateFontToHandle("HG創英角ﾎﾟｯﾌﾟ体", 115, 0, DX_FONTTYPE_ANTIALIASING_EDGE_8X8, -1, 7);
        private int NameSetTitle_FontHandle = CreateFontToHandle("HG創英角ﾎﾟｯﾌﾟ体", 65, 0, DX_FONTTYPE_ANTIALIASING_EDGE_8X8, -1, 4);

        private void MyDrawStringToHandle(int x, int y, string text, uint color, int handle, uint edgecolor, bool x_center = false, bool y_center = false, bool origin_left = false)
        {
            int width, height, line;
            GetDrawStringSizeToHandle(out width, out height, out line, text, text.Length, handle);

            int adjust_x, adjust_y;

            adjust_x = origin_left ? -width : ( x_center ? -width / 2 : 0 ) ;
            adjust_y = y_center ? -height / 2 : 0;

            DrawStringToHandle(x + adjust_x, y + adjust_y, text, color, handle, edgecolor);
        }

        #endregion
    }
}
