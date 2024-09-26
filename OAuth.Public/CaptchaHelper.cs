using OneForAll.Core.Utility;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OAuth.Public
{
    /// <summary>
    /// 验证码 
    /// </summary>
    public class CaptchaHelper
    {
        /// <summary>
        /// 获取Base64验证码
        /// </summary>
        /// <returns>结果</returns>
        public string GetImageBase64(string code, int width = 200, int height = 100)
        {
            using (SKBitmap bitmap = new SKBitmap(width, height))
            {
                using (SKCanvas canvas = new SKCanvas(bitmap))
                {
                    canvas.Clear(SKColors.White);
                    using (SKPaint textPaint = new SKPaint())
                    {
                        textPaint.TextSize = 40;
                        textPaint.IsAntialias = true;
                        textPaint.Color = GenerateRandomColor();
                        textPaint.TextAlign = SKTextAlign.Center;
                        textPaint.Typeface = SKTypeface.FromFamilyName("Arial");

                        float xPoint = new Random().Next(0, width / 2);
                        float yPoint = height / 2;
                        for (int i = 0; i < code.Length; i++)
                        {
                            canvas.DrawText(code[i].ToString(), xPoint + (i * 25), yPoint, textPaint);
                        }
                    }

                    // 添加噪声线（可选）
                    DrawNoiseLines(canvas, width, height);

                    // 保存到内存流并转换为字节数组
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Encode(SKEncodedImageFormat.Png, 100).SaveTo(ms);
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        // 随机颜色
        private static SKColor GenerateRandomColor()
        {
            Random random = new Random();
            return new SKColor((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
        }

        // 噪线
        private static void DrawNoiseLines(SKCanvas canvas, int width, int height)
        {
            Random random = new Random();
            using (SKPaint noisePaint = new SKPaint())
            {
                noisePaint.Color = SKColors.Gray;
                noisePaint.StrokeWidth = 1;
                noisePaint.Style = SKPaintStyle.Stroke;

                for (int i = 0; i < 10; i++)
                {
                    int x1 = random.Next(width);
                    int y1 = random.Next(height);
                    int x2 = random.Next(width);
                    int y2 = random.Next(height);
                    canvas.DrawLine(x1, y1, x2, y2, noisePaint);
                }
            }
        }
    }
}
