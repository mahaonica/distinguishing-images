using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace Constellation
{
    public class PointFigure
    {
        protected int[,] arr;
        protected int[,] tmpArr;
        public PointFigure(int n, int m)
        {
            Width = n;
            Height = m;
            arr = new int[Height, Width];
        }

        public PointFigure(int[,] arr)
        {
            Width = arr.GetLength(0);
            Height = arr.GetLength(1);
            this.arr = arr.Clone() as int[,];
        }

        public PointFigure(PointFigure other)
        {
            if (other.arr != null)
                arr = other.arr.Clone() as int[,];
            if (other.tmpArr != null)
                tmpArr = other.tmpArr.Clone() as int[,];
            Height = other.Height;
            Width = other.Width;
        }

        public void Clear()
        {
            arr = new int[Height, Width];
            tmpArr = null;
        }

        public int GetPixel(int i, int j)
        {
            return arr[i, j];
        }

        public void DrawPixel(int i, int j)
        {
            arr[i, j] = 1;
        }

        public void ClearPixel(int i, int j)
        {
            arr[i, j] = 0;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int Count 
        { 
            get 
            {
                int sum = 0;
                foreach (var i in arr)
                {
                    if (i == 1)
                        sum++;
                }
                return sum;
            } 
        }
    }

    public class StarPicture : PointFigure
    {
        private int[,,] fragments2x2 = new int[16, 2, 2] {
            {
                {0, 0},
                {0, 0}
            },
            {
                {1, 0},
                {0, 0}
            },
            {
                {0, 1},
                {0, 0}
            },
            {
                {0, 0},
                {1, 0}
            },
            {
                {0, 0},
                {0, 1}
            },
            {
                {1, 1},
                {0, 0}
            },
            {
                {0, 0},
                {1, 1}
            },
            {
                {0, 1},
                {0, 1}
            },
            {
                {1, 0},
                {1, 0}
            },
            {
                {1, 0},
                {0, 1}
            },
            {
                {0, 1},
                {1, 0}
            },
            {
                {1, 1},
                {1, 0}
            },
            {
                {1, 1},
                {0, 1}
            },
            {
                {0, 1},
                {1, 1}
            },
            {
                {1, 0},
                {1, 1}
            },
            {
                {1, 1},
                {1, 1}
            },
        };

        public int AreaSize { get; private set; }

        public StarPicture(int n, int m, int areaSize) : base(n, m) { 
            AreaSize = areaSize;
        }

        public StarPicture(int[,] arr, int areaSize) : base(arr) { 
            AreaSize = areaSize; 
        }

        public StarPicture(StarPicture other) : base(other) { 
            AreaSize = other.AreaSize;
            if (other.detectedPoints != null)
                detectedPoints = new List<Point>(other.detectedPoints);
            if(other.pointsCharacters != null)
                pointsCharacters = (from p in other.pointsCharacters select p.Clone() as int[]).ToList();
        }

        public void DrawPoint(double x, double y)
        {
            int xc = (int)Math.Truncate(x * Width);
            int yc = (int)Math.Truncate(y * Height);
            if (GetPixel(yc, xc) == 0)
                DrawPixel(yc, xc);
            else
                ClearPixel(yc, xc);
        }

        public int[] GetCharacters()
        {
            int[] ans = new int[fragments2x2.GetLength(0)];
            for (int i = 0; i < Height - 1; i++)
            {
                for (int j = 0; j < Width - 1; j++)
                {
                    for (int k = 0; k < ans.Length; k++)
                    {
                        if (GetPixel(i, j) == fragments2x2[k, 0, 0] && GetPixel(i + 1, j) == fragments2x2[k, 1, 0] && GetPixel(i, j + 1) == fragments2x2[k, 0, 1] && GetPixel(i + 1, j + 1) == fragments2x2[k, 1, 1])
                            ans[k]++;
                    }
                }
            }
            return ans;
        }

        //public bool IsCorrectIsolatedPointsSet
        //{
        //    get
        //    {
        //        int[] crt = GetCharacters();
        //        if (crt[1] == crt[2] && crt[2] == crt[3] && crt[3] == crt[4] && crt[5] == 0 && crt[6] == 0 && crt[7] == 0 && crt[8] == 0 && crt[9] == 0 && crt[10] == 0 && crt[11] == 0 && crt[12] == 0 && crt[13] == 0 && crt[14] == 0 && crt[15] == 0)
        //            return true;
        //        else
        //            return false;
        //    }
        //}

        //вариант разрешающий точки на границе
        public bool IsCorrectIsolatedPointsSet
        {
            get
            {
                int[] crt = GetCharacters();
                if (crt[5] == 0 && crt[6] == 0 && crt[7] == 0 && crt[8] == 0 && crt[9] == 0 && crt[10] == 0 && crt[11] == 0 && crt[12] == 0 && crt[13] == 0 && crt[14] == 0 && crt[15] == 0)
                    return true;
                else
                    return false;
            }
        }

        public List<Point> detectedPoints {get; private set;}

        public List<int[]> pointsCharacters { get; private set; }

        private int[,] GetAreaSubArray(Point p)
        {
            int[,] tmp = new int[AreaSize * 2 + 1, AreaSize * 2 + 1];
            for (int i = p.Y - AreaSize; i - (p.Y - AreaSize) < AreaSize * 2 + 1; i++)
            {
                for (int j = p.X - AreaSize; j - (p.X - AreaSize) < AreaSize * 2 + 1; j++)
                {
                    if (j >= 0 && j < Width && i >= 0 && i < Height)
                    {
                        tmp[i - (p.Y - AreaSize), j - (p.X - AreaSize)] = arr[i, j];
                    }
                }
            }
            return tmp;
        }

        public void DetectPoints()
        {
            detectedPoints = new List<Point>();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (GetPixel(i, j) == 1)
                        detectedPoints.Add(new Point(j, i));
                }
            }
        }

        new public void Clear()
        {
            base.Clear();
            detectedPoints = null;
            pointsCharacters = null;
        }

        public void DrawLines()
        {
            tmpArr = arr.Clone() as int[,];
            DetectPoints();
            for (int i = 0; i < detectedPoints.Count - 1; i++)
            {
                for (int j = i + 1; j < detectedPoints.Count; j++)
                {
                    DrawLine(detectedPoints[i].Y, detectedPoints[i].X, detectedPoints[j].Y, detectedPoints[j].X);
                }
            }
            SetPointsCharacters();
        }

        public void SetPointsCharacters()
        {
            pointsCharacters = (from p in detectedPoints select (new StarPicture(GetAreaSubArray(p), AreaSize)).GetCharacters()).ToList();
        }

        public void ClearLines()
        {
            if (tmpArr != null)
            {
                arr = tmpArr.Clone() as int[,];
                detectedPoints = null;
                pointsCharacters = null;
                tmpArr = null;
            }
        }

        private void Swap(ref int x, ref int y)
        {
            var temp = x;
            x = y;
            y = temp;
        }

        public void DrawLine(int x0, int y0, int x1, int y1)
        {
            var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0); // Проверяем рост отрезка по оси икс и по оси игрек
            // Отражаем линию по диагонали, если угол наклона слишком большой
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            // Если линия растёт не слева направо, то меняем начало и конец отрезка местами
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2; // Здесь используется оптимизация с умножением на dx, чтобы избавиться от лишних дробей
            int ystep = (y0 < y1) ? 1 : -1; // Выбираем направление роста координаты y
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                DrawPixel(steep ? y : x, steep ? x : y); // Не забываем вернуть координаты на место
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }

        public void PrintGraphics(Graphics g, int gWidth, int gHeight)
        {
            g.Clear(Color.White);
            Pen blackPen = new Pen(Color.RosyBrown);
            float scaleX = (float)gWidth / Width;
            float scaleY = (float)gHeight / Height;
            for (int i = 0; i < Height; i++)
            {
                g.DrawLine(blackPen, 0, i * scaleY, gWidth, i * scaleY);
            }
            for (int j = 0; j < Width; j++)
            {
                g.DrawLine(blackPen, j * scaleX, 0, j * scaleX, gHeight);
            }
            Brush blackBrush = new SolidBrush(Color.Black);
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if(GetPixel(i, j) != 0)
                        g.FillRectangle(blackBrush, j * scaleX, i * scaleY, scaleX, scaleY);
                }
            }
        }
    }
}
