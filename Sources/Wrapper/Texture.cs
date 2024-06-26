﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DxLibDLL;

namespace Amaoto
{
    /// <summary>
    /// テクスチャ管理を行うクラス。
    /// </summary>
    public class Texture : IDisposable
    {
        /// <summary>
        /// テクスチャを生成します。
        /// </summary>
        public Texture()
        {
            Rotation = 0.0f;
            ScaleX = 1.0f;
            ScaleY = 1.0f;
            Opacity = 255.0f;
            R = 255;
            G = 255;
            B = 255;
            ReferencePoint = ReferencePoint.TopLeft;
        }

        /// <summary>
        /// テクスチャを生成します。
        /// </summary>
        /// <param name="fileName">ファイル名。</param>
        public Texture(string fileName)
            : this()
        {
            ID = DX.LoadGraph(fileName);
            if (ID != -1)
            {
                IsEnable = true;
            }
            FileName = fileName;
        }

        /// <summary>
        /// DXLibのグラフィックハンドルから生成します。
        /// </summary>
        /// <param name="handle">DxLibのグラフィックハンドル。</param>
        public Texture(int handle)
            : this()
        {
            ID = handle;
            if (ID != -1)
            {
                IsEnable = true;
            }
            FileName = null;
        }

        /// <summary>
        /// ビットマップからテクスチャを生成します。
        /// </summary>
        /// <param name="bitmap">ビットマップ。</param>
        public Texture(Bitmap bitmap)
            : this()
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                var buf = ms.ToArray();

                unsafe
                {
                    fixed (byte* p = buf)
                    {
                        DX.SetDrawValidGraphCreateFlag(DX.TRUE);
                        DX.SetDrawValidAlphaChannelGraphCreateFlag(DX.TRUE);

                        ID = DX.CreateGraphFromMem((IntPtr)p, buf.Length);

                        DX.SetDrawValidGraphCreateFlag(DX.FALSE);
                        DX.SetDrawValidAlphaChannelGraphCreateFlag(DX.FALSE);
                    }
                }
            }
        }

        ~Texture()
        {
            if(IsEnable)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (DX.DeleteGraph(ID) != -1)
            {
                IsEnable = false;
            }
        }

        /// <summary>
        /// 描画します。
        /// </summary>
        /// <param name="x">X座標。</param>
        /// <param name="y">Y座標。</param>
        /// <param name="rectangle">描画範囲。</param>
        public void Draw(float x, float y, RectangleF? rectangle = null)
        {
            var origin = new Point();
            DX.GetGraphSize(ID, out var width, out var height);
            if (rectangle == null)
            {
                // rectangleが引数として渡されなかった場合、
                // 元画像のすべてを表示するためにここで全領域のrectangleを生成する。
                rectangle = new Rectangle(0, 0, width, height);
            }
            switch (ReferencePoint)
            {
                case ReferencePoint.TopLeft:
                    origin.X = 0;
                    origin.Y = 0;
                    break;
                case ReferencePoint.TopCenter:
                    origin.X = (int)rectangle.Value.Width / 2;
                    origin.Y = 0;
                    break;
                case ReferencePoint.TopRight:
                    origin.X = (int)rectangle.Value.Width;
                    origin.Y = 0;
                    break;
                case ReferencePoint.CenterLeft:
                    origin.X = 0;
                    origin.Y = (int)rectangle.Value.Height / 2;
                    break;
                case ReferencePoint.Center:
                    origin.X = (int)rectangle.Value.Width / 2;
                    origin.Y = (int)rectangle.Value.Height / 2;
                    break;
                case ReferencePoint.CenterRight:
                    origin.X = (int)rectangle.Value.Width;
                    origin.Y = (int)rectangle.Value.Height / 2;
                    break;
                case ReferencePoint.BottomLeft:
                    origin.X = 0;
                    origin.Y = (int)rectangle.Value.Height;
                    break;
                case ReferencePoint.BottomCenter:
                    origin.X = (int)rectangle.Value.Width / 2;
                    origin.Y = (int)rectangle.Value.Height;
                    break;
                case ReferencePoint.BottomRight:
                    origin.X = (int)rectangle.Value.Width;
                    origin.Y = (int)rectangle.Value.Height;
                    break;
                default:
                    origin.X = 0;
                    origin.Y = 0;
                    break;
            }
            var blendParam = (int)Opacity;
            if (BlendMode == BlendMode.None)
            {
                // 通常合成(アルファ)。
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, blendParam);
            }
            else if (BlendMode == BlendMode.Add)
            {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ADD, blendParam);
            }
            else if (BlendMode == BlendMode.Subtract)
            {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_SUB, blendParam);
            }
            else if (BlendMode == BlendMode.Multiply)
            {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_MULA, blendParam);
            }

            DX.SetDrawMode(DX.DX_DRAWMODE_BILINEAR);

            DX.DrawRectRotaGraph3F(
                // 座標
                x,
                y,
                // 元画像座標
                (int)rectangle.Value.X,
                (int)rectangle.Value.Y,
                // 元画像幅・高さ
                (int)rectangle.Value.Width,
                (int)rectangle.Value.Height,
                // 描画基準点
                origin.X,
                origin.Y,
                // 拡大率
                ScaleX,
                ScaleY,
                // 回転角度
                Rotation,
                ID,
                DX.TRUE);
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
        }

        /// <summary>
        /// 描画します。
        /// </summary>
        /// <param name="x">X座標。</param>
        /// <param name="y">Y座標。</param>
        /// <param name="rectangle">描画範囲。</param>
        public void Draw_Modi(Point p1, Point p2, Point p3, Point p4, RectangleF? rectangle = null)
        {
            DX.GetGraphSize(ID, out var width, out var height);
            if (rectangle == null)
            {
                // rectangleが引数として渡されなかった場合、
                // 元画像のすべてを表示するためにここで全領域のrectangleを生成する。
                rectangle = new Rectangle(0, 0, width, height);
            }
            var blendParam = (int)Opacity;
            if (BlendMode == BlendMode.None)
            {
                // 通常合成(アルファ)。
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, blendParam);
            }
            else if (BlendMode == BlendMode.Add)
            {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ADD, blendParam);
            }
            else if (BlendMode == BlendMode.Subtract)
            {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_SUB, blendParam);
            }
            else if (BlendMode == BlendMode.Multiply)
            {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_MULA, blendParam);
            }

            DX.SetDrawMode(DX.DX_DRAWMODE_BILINEAR);

            DX.DrawRectModiGraph(
                p1.X, p1.Y,
                p2.X, p2.Y,
                p3.X, p3.Y,
                p4.X, p4.Y,
                // 元画像座標
                (int)rectangle.Value.X,
                (int)rectangle.Value.Y,
                // 元画像幅・高さ
                (int)rectangle.Value.Width,
                (int)rectangle.Value.Height,
                ID,
                DX.TRUE
                ); ;
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);
        }

        public void SetRGB(int R, int G, int B)
        {
            DX.SetDrawBright(R, G, B);
        }

        /// <summary>
        /// テクスチャをPNGファイルに出力します。
        /// </summary>
        /// <param name="path">保存先。</param>
        public void SaveAsPNG(string path)
        {
            DX.SaveDrawValidGraphToPNG(ID, 0, 0, TextureSize.width, TextureSize.height, path, 0);
        }


        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        /// <summary>
        /// 有効かどうか。
        /// </summary>
        public bool IsEnable { get; private set; }
        /// <summary>
        /// 合成モード。
        /// </summary>
        public BlendMode BlendMode { get; set; }
        /// <summary>
        /// ファイル名。
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// 不透明度。
        /// </summary>
        public float Opacity { get; set; }
        /// <summary>
        /// ID。
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// 角度(弧度法)。
        /// </summary>
        public float Rotation { get; set; }
        /// <summary>
        /// 描画基準点。
        /// </summary>
        public ReferencePoint ReferencePoint { get; set; }
        /// <summary>
        /// 拡大率X。
        /// </summary>
        public float ScaleX { get; set; }
        /// <summary>
        /// 拡大率Y。
        /// </summary>
        public float ScaleY { get; set; }
        /// <summary>
        /// テクスチャのサイズを返します。
        /// </summary>
        public (int width, int height) TextureSize
        {
            get
            {
                DX.GetGraphSize(ID, out var width, out var height);
                return (width, height);
            }
        }
    }
    /// <summary>
    /// 合成モード。
    /// </summary>
    public enum BlendMode
    {
        /// <summary>
        /// なし
        /// </summary>
        None,
        /// <summary>
        /// 加算合成
        /// </summary>
        Add,
        /// <summary>
        /// 減算合成
        /// </summary>
        Subtract,
        /// <summary>
        /// 乗算合成。
        /// </summary>
        Multiply
    }
    /// <summary>
    /// 描画基準点。
    /// </summary>
    public enum ReferencePoint
    {
        /// <summary>
        /// 左上
        /// </summary>
        TopLeft,
        /// <summary>
        /// 中央上
        /// </summary>
        TopCenter,
        /// <summary>
        /// 右上
        /// </summary>
        TopRight,
        /// <summary>
        /// 左中央
        /// </summary>
        CenterLeft,
        /// <summary>
        /// 中央
        /// </summary>
        Center,
        /// <summary>
        /// 右中央
        /// </summary>
        CenterRight,
        /// <summary>
        /// 左下
        /// </summary>
        BottomLeft,
        /// <summary>
        /// 中央下
        /// </summary>
        BottomCenter,
        /// <summary>
        /// 右下
        /// </summary>
        BottomRight
    }
}
