using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BAZIS8
{

    struct dxf //примитивы DXF
    {
        public double xx1, yy1, xx2, yy2, rr, xx0, yy0, u1, u2, t;
        public string linia;
        public string colordxf; // цвет из dxf файла
    };
    struct ln { public double x1, y1, x2, y2; };//координаты прямоугольников
    struct ot
    {
        public double x0, y0, z0, r, g; //координаты глубина радиус
        public PictureBox viz; // визуализация
        public int vid; //торцевое 0 вертикальное 1
        public string OO; // точка отсчёта



        public int edit;
        public int ANcol; // количество сверел из вудвопа
        public double ABstep; // шаг сверел из вудвопа
        public double WIugol; // угол двойных вертикальных сверел из вудвопа
        public string NAPR; // направление сверел из вудвопа для торцевых


    };

    struct HIRZT //HIRZT файл
    {
        public double QX, QY, OFS, N, DIA, PRF; //координаты сверла шаг кол-во диаметр глубина
        public string DRILL, OO, PL; //тип сверления    точка сверления плоскость   
        public int vid; //торцевое 0 вертикальное 1 
    };

    public partial class Form1 : Form
    {
        bool WWF = false;  //внешний вызов УП


        public string outFileHIRZT;
        public string outFileWoodWOP;
        public string inputFileWW;
        public string DeS;//децимал сепаратор системы тут

        public string[] NAMEWW = new string[5];// имена переменных из вудвоп файла
        public string[] ZZWW = new string[5];// значения переменных из вудвоп файла

        dxf[] primit = new dxf[5000];// сюда сложим координаты прочитанные из файла DXF
        dxf[] kontur = new dxf[1000];// сюда разложим контур на линии
        int dxfCount = 0; // счётчик примитивов из файла DXF
                          //   int primitcol = 0; //количество примитивов
        int primitcol1 = 0; // частей в первом примитиве
                            //  int primitret = 0; // флаг нахождения контура
                            // bool SvEdit=false;

        ln[] PR = new ln[100];// массив прямоугольников
        ot[] Sv = new ot[300];// массив сверления

        int X0 = 25, Y0 = 60, XX = 950, YY = 500;// координаты рабочего пространства
        double MX, MY, MM; //масштаб
        int XS, YS = 0; //смещение

        double XD, YD, ZD; // деталь

        int countT = 0, countV = 0;//счётчики свёрел
        bool dnd = false; // drug&drop
        Pen p1 = new Pen(Color.Blue, 1);
        Pen p2 = new Pen(Color.Red, 3);
        Pen p3 = new Pen(Color.Black, 1);
        Pen p4 = new Pen(Color.Green, 4);

        Graphics g;

        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "|*.dxf";
            openFileWW.FileName = "";
            openFileWW.Filter = "|*.mpr";
            countT = countV = 0;
            outFileHIRZT = outFileWoodWOP = "";

            //принудительно ставим точку в разделитель десятичного числа
            CultureInfo inf = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            System.Threading.Thread.CurrentThread.CurrentCulture = inf;
            inf.NumberFormat.NumberDecimalSeparator = ".";
            DeS += ".";

        }


        public void OnKeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape) //удалить сверло
            {
                label1.Focus();
                Sv[0].viz.BackColor = Color.LightGray;

            }

            if (e.KeyCode == Keys.S && e.Alt) // 
            {
                SvNext.PerformClick();

            }


            if (e.KeyCode == Keys.A && e.Alt) // 
            {
                SvPrev.PerformClick();

            }

            if (e.KeyCode == Keys.X && e.Alt) //удалить сверло
            {
                deleteSv.PerformClick();
            }

            if (e.KeyCode == Keys.G && e.Alt) //глухое торцевое
            {
                SKV.PerformClick();
            }


            if (label1.Focused)
            {

                label1.ForeColor = Color.Blue;


                if (e.KeyCode == Keys.W) // Y++
                {
                    double temp_y = Convert.ToDouble(textBoxY.Text);
                    temp_y++;
                    textBoxY.Text = Convert.ToString(temp_y);
                }

                if (e.KeyCode == Keys.S) // Y--
                {
                    double temp_y = Convert.ToDouble(textBoxY.Text);
                    temp_y--;
                    textBoxY.Text = Convert.ToString(temp_y);
                }

                if (e.KeyCode == Keys.A) // X--
                {
                    double temp_x = Convert.ToDouble(textBoxX.Text);
                    temp_x--;
                    textBoxX.Text = Convert.ToString(temp_x);
                }

                if (e.KeyCode == Keys.D) // X++
                {
                    double temp_x = Convert.ToDouble(textBoxX.Text);
                    temp_x++;
                    textBoxX.Text = Convert.ToString(temp_x);
                }



            }

        }




        public void init(object sender, EventArgs e) //рисуем рамки и возможно координатную сетку
        {

            toolTip1.SetToolTip(OPN, "Открыть файл DXF");
            toolTip1.SetToolTip(saveCad4, "Сохранить CAD/4");
            toolTip1.SetToolTip(saveWW, "Сохранить WoodWOP");
            toolTip1.SetToolTip(FileName, "Имя файла для записи");
            toolTip1.SetToolTip(AddDim, "Новая деталь");
            toolTip1.SetToolTip(textBoxXD, "Размер детали X");
            toolTip1.SetToolTip(textBoxYD, "Размер детали Y");
            toolTip1.SetToolTip(textBoxZD, "Толщина детали");
            toolTip1.SetToolTip(SvPrev, "Сверло -");
            toolTip1.SetToolTip(SvNext, "Сверло +");
            toolTip1.SetToolTip(textBoxX, "Координаты сверла X");
            toolTip1.SetToolTip(textBoxY, "Координаты сверла Y");
            toolTip1.SetToolTip(textBoxD, "Диаметр сверла");
            toolTip1.SetToolTip(textBoxG, "Глубина сверления");
            toolTip1.SetToolTip(rotateL, "Повернуть против часовой");
            toolTip1.SetToolTip(rotateR, "Повернуть по часовой");
            toolTip1.SetToolTip(MirrorX, "Отразить по горизонтали");
            toolTip1.SetToolTip(MirrorY, "Отразить по вертикали");
            toolTip1.SetToolTip(AddSv, "Добавить сверло");
            toolTip1.SetToolTip(deleteSv, "Удалить сверло");
            toolTip1.SetToolTip(DropListClear, "Очистить список файлов");
            toolTip1.SetToolTip(label0, "Начальная точка");
            toolTip1.SetToolTip(button1, "Сверелние всех отверстий от точки 0");
            toolTip1.SetToolTip(button2, "Сверелние всех отверстий от точки 1");
            toolTip1.SetToolTip(button3, "Сверелние всех отверстий от точки 2");
            toolTip1.SetToolTip(button4, "Сверелние всех отверстий от точки 3");
            toolTip1.SetToolTip(SKV, "Глухое/сквозное отверстие ");
            toolTip1.SetToolTip(OWW, "Открыть файл WoodWOP (MPR) ");
            toolTip1.SetToolTip(OPNCAD4, "Открыть файл HIRZT (PD4) ");

            toolTip1.SetToolTip(help_button, "Показать/скрыть подсказку");



            g = Graphics.FromHwnd(this.Handle);

            g.DrawRectangle(p1, X0, Y0, XX, YY);

            string[] args = System.Environment.GetCommandLineArgs();
            string filePath = args[0];
            WWF = false;
            for (int i = 0; i <= args.Length - 1; i++)
            {
                if (args[i].ToLower().EndsWith(".pd4") == true)
                {
                    nameFILE = FileName.Text = args[i];
                    saveFileDialog1.InitialDirectory = nameFILE.Remove(nameFILE.LastIndexOf("\\"));
                    WWF = true;
                    OPNCAD4.PerformClick();
                }

                if (args[i].ToLower().EndsWith(".mpr") == true)
                {
                    nameFILE = FileName.Text = args[i];
                    saveFileDialog1.InitialDirectory = nameFILE.Remove(nameFILE.LastIndexOf("\\"));
                    WWF = true;
                    OWW.PerformClick();

                }

                if (args[i].ToLower().EndsWith(".dxf") == true)
                {
                    nameFILE = FileName.Text = args[i];
                    saveFileDialog1.InitialDirectory = nameFILE.Remove(nameFILE.LastIndexOf("\\"));
                    WWF = true;
                    OPN.PerformClick();

                }
            }

            textBoxZ.Text = (Convert.ToDouble(textBoxZD.Text) / 2).ToString();



        }

        void sortdetail() //сортируем контур детали при возможных глюках базиса в далнейшем сюда прикрутить кривые базис контуры масштабы и проч

        {
            int J, tt;
            ln templn;
            templn.x1 = templn.x2 = templn.y1 = templn.y2 = 0;
            for (tt = 1; tt <= 4; tt++)
            {
                for (J = tt; J <= 4; J++)
                {
                    if ((PR[J].x1 == templn.x2) && (PR[J].y1 == templn.y2) && (J != tt))
                    {
                        templn = PR[J];
                        PR[J] = PR[tt];
                        PR[tt] = templn;
                        J = 5;
                    }
                    if ((PR[J].x1 == templn.x2) && (PR[J].y1 == templn.y2) && (J == tt))
                    {
                        templn = PR[J];
                        J = 5;

                    }
                }
            }

            XD = PR[2].x2;//габариты детали
            YD = PR[2].y2;

        }


        private void sortSv() // сортировка свёрел слева направо снизу вверх

        {
            ot tempSv;
            int i, j;


          

            for (i = 1; i < countV + 1; i++)
            {
                for (j = i + 1; j < countV + 1; j++)
                {
                    if ((Sv[j].y0 < Sv[i].y0))
                    {
                        tempSv = Sv[j];
                        Sv[j] = Sv[i];
                        Sv[i] = tempSv;
                    }
                }
            }

            for (i = 1; i < countV + 1; i++)
            {
                for (j = i + 1; j < countV + 1; j++)
                {
                    if ((Sv[j].x0 < Sv[i].x0))
                    {
                        tempSv = Sv[j];
                        Sv[j] = Sv[i];
                        Sv[i] = tempSv;
                    }
                }
            }

            for (i = 1; i < countV + 1; i++)
            {
                for (j = i + 1; j < countV + 1; j++)
                {
                    if ((Sv[j].x0 == Sv[i].x0) && (Sv[j].y0 < Sv[i].y0))
                    {
                        tempSv = Sv[j];
                        Sv[j] = Sv[i];
                        Sv[i] = tempSv;
                    }
                }
            }


        }

        void workdraw() //рисуем деталь
        {



            if (XD == 0) { return; }
            if (YD == 0) { return; }
            this.Invalidate();
            Graphics g = Graphics.FromHwnd(this.Handle);

            XX = Width - 450;
            YY = Height - 150;

            MX = (XX - X0) / XD;
            MY = (YY - Y0) / YD;

            MM = MX; if (MY < MX) MM = MY;


            label4.Text = "M:" + Math.Round(MM,3).ToString();

            /*
            if (MM > 1) { MM -= 0.1; };
            if (MM <= 1) { MM -= 0.05; };
            */




            textBoxXD.Text = Convert.ToString(XD);
            textBoxYD.Text = Convert.ToString(YD);
            textBoxZD.Text = Convert.ToString(ZD);

            if (!woodwopopen) { FileName.Text = textBoxXD.Text + "x" + textBoxYD.Text; }


            XS = Convert.ToInt32((XX - XD * MM) / 2) + X0;//вычисляем середину
            YS = Convert.ToInt32((YY - YD * MM) / 2) + Y0;


            this.Controls.Remove(Sv[0].viz);
            Sv[0].viz = new PictureBox();
            Sv[0].viz.BackColor = Color.Gray;
            Sv[0].viz.Left = XS;
            Sv[0].viz.Top = YS;
            Sv[0].viz.BorderStyle = BorderStyle.None;






            Sv[0].viz.Width = Convert.ToInt32(XD * MM);

            Sv[0].viz.Height = Convert.ToInt32(YD * MM);

           


            button1.Left = Sv[0].viz.Left - 30;
            button1.Top = Sv[0].viz.Top + Sv[0].viz.Height;

            button2.Left = Sv[0].viz.Left - 30;
            button2.Top = Sv[0].viz.Top - 30;

            button3.Left = Sv[0].viz.Left + Sv[0].viz.Width;
            button3.Top = Sv[0].viz.Top - 30;

            button4.Left = Sv[0].viz.Left + Sv[0].viz.Width;
            button4.Top = Sv[0].viz.Top + Sv[0].viz.Height;

            this.Controls.Add(Sv[0].viz);

            rotateL.Enabled = rotateR.Enabled = MirrorX.Enabled = MirrorY.Enabled = AddSv.Enabled = true;


        }

        void drawsv()
        {

            int i, mmm = 1;
            sortSv();
            infosv();

            

            for (i = 1; i < countV + 1; i++)
            {
               
                if (Sv[i].vid == 1) //вертикальные
                {

                    Sv[i].viz = new PictureBox();
                    Sv[i].viz.Name = Convert.ToString(i);
                    Sv[i].viz.Cursor = Cursors.Hand;

                    //Sv[i].viz.Left = Convert.ToInt32((Sv[i].x0 * MM)  - Sv[i].r);
                    //Sv[i].viz.Top = Convert.ToInt32((YD - Sv[i].y0) * MM  - Sv[i].r);




                    if (Convert.ToInt32(Sv[i].r * 2 * MM) < 20) {
                        mmm = 1;
                    }

                    Sv[i].viz.Width = Sv[i].viz.Height = Convert.ToInt32(Sv[i].r * 2 * MM * mmm );

                    //   if (Sv[i].viz.Width >= 5) { Sv[i].viz.Width = Sv[i].viz.Height = Convert.ToInt32(8 * MM); }

                    
                    Sv[i].viz.Left = Convert.ToInt32((Sv[i].x0 - Sv[i].r) * MM );
                        
                    Sv[i].viz.Top = Convert.ToInt32((YD - Sv[i].y0 - Sv[i].r) * MM );


                    

                    Sv[i].viz.Click += new EventHandler(SVClick);

                    System.Drawing.Drawing2D.GraphicsPath myPath = new System.Drawing.Drawing2D.GraphicsPath();

                    myPath.AddEllipse(0, 0, Sv[i].viz.Width, Sv[i].viz.Height);

                    Region myRegion = new Region(myPath);
                    Sv[i].viz.Region = myRegion;
                    Sv[i].viz.BackColor = Color.Blue;


                    Sv[0].viz.Controls.Add(Sv[i].viz);



                }

                if (Sv[i].vid == -1)
                {

                    Sv[i].viz = new PictureBox();
                    Sv[i].viz.Name = Convert.ToString(i);


                    // Sv[i].viz.Left = Convert.ToInt32((Sv[i].x0 * MM)  - Sv[i].r);
                    //Sv[i].viz.Top = Convert.ToInt32((YD - Sv[i].y0) * MM  - Sv[i].r);
                   

                    Sv[i].viz.Width = Sv[i].viz.Height = 10;



                    Sv[i].viz.Left = 10;
                    Sv[i].viz.Top = 10;


                    Sv[i].viz.Click += new EventHandler(SVClick);

                    System.Drawing.Drawing2D.GraphicsPath myPath = new System.Drawing.Drawing2D.GraphicsPath();

                    myPath.AddEllipse(0, 0, Sv[i].viz.Width, Sv[i].viz.Height);

                    Region myRegion = new Region(myPath);
                    Sv[i].viz.Region = myRegion;
                    Sv[i].viz.BackColor = Color.Black;


                    Sv[0].viz.Controls.Add(Sv[i].viz);



                }

                if (Sv[i].vid == 0) //торцевые
                {
                    if (Sv[i].y0 == YD) // сверху
                    {
                        if (Sv[i].OO == "-1") { Sv[i].OO = "1"; }
                        Sv[i].viz = new PictureBox();
                        Sv[i].viz.Name = Convert.ToString(i);

                        Sv[i].viz.Left = Convert.ToInt32((Sv[i].x0 - Sv[i].r) * MM);
                        Sv[i].viz.Top = 0;
                        //Sv[i].viz.BorderStyle = BorderStyle.FixedSingle;

                        Sv[i].viz.Width = Convert.ToInt32(Sv[i].r * 2 * MM);
                        Sv[i].viz.Height = Convert.ToInt32(Sv[i].g * MM);


                      //  if (Sv[i].viz.Width <= 3) { Sv[i].viz.Width = Sv[i].viz.Height = 5; }

                        Sv[i].viz.Click += new EventHandler(SVClick);


                        Sv[i].viz.BackColor = Color.Blue;


                        Sv[0].viz.Controls.Add(Sv[i].viz);
                    }

                    if (Sv[i].y0 == 0) // снизу
                    {
                        if (Sv[i].OO == "-1") { Sv[i].OO = "0"; }
                        Sv[i].viz = new PictureBox();
                        Sv[i].viz.Name = Convert.ToString(i);

                        Sv[i].viz.Left = Convert.ToInt32((Sv[i].x0 - Sv[i].r) * MM ) ;
                        Sv[i].viz.Top = Convert.ToInt32(YD * MM - Sv[i].g * MM);
                        
                        //Sv[i].viz.BorderStyle = BorderStyle.FixedSingle;

                        Sv[i].viz.Width = Convert.ToInt32(Sv[i].r * 2 * MM);
                        Sv[i].viz.Height = Convert.ToInt32(Sv[i].g * MM);


                      //  if (Sv[i].viz.Width <= 3) { Sv[i].viz.Width = Sv[i].viz.Height = 5; }

                        Sv[i].viz.Click += new EventHandler(SVClick);
                       

                        Sv[i].viz.BackColor = Color.Blue;


                        Sv[0].viz.Controls.Add(Sv[i].viz);
                    }

                    if (Sv[i].x0 == 0) // слева
                    {
                        if (Sv[i].OO == "-1") { Sv[i].OO = "0"; }
                        Sv[i].viz = new PictureBox();
                        Sv[i].viz.Name = Convert.ToString(i);

                        Sv[i].viz.Left = 0;
                        Sv[i].viz.Top = Convert.ToInt32((YD - Sv[i].y0 - Sv[i].r) * MM);

                        //Sv[i].viz.BorderStyle = BorderStyle.FixedSingle;

                        Sv[i].viz.Width = Convert.ToInt32(Sv[i].g * MM);
                        Sv[i].viz.Height = Convert.ToInt32(Sv[i].r * 2 * MM);


                        //if (Sv[i].viz.Width <= 3) { Sv[i].viz.Width = Sv[i].viz.Height = 5; }

                        Sv[i].viz.Click += new EventHandler(SVClick);


                        Sv[i].viz.BackColor = Color.Blue;


                        Sv[0].viz.Controls.Add(Sv[i].viz);
                    }

                    if (Sv[i].x0 == XD) // справа
                    {
                        if (Sv[i].OO == "-1") { Sv[i].OO = "3"; }
                        Sv[i].viz = new PictureBox();
                        Sv[i].viz.Name = Convert.ToString(i);

                        Sv[i].viz.Left = Convert.ToInt32((XD - Sv[i].g) * MM);
                        Sv[i].viz.Top = Convert.ToInt32((YD - Sv[i].y0 - Sv[i].r) * MM);

                        //Sv[i].viz.BorderStyle = BorderStyle.FixedSingle;

                        Sv[i].viz.Width = Convert.ToInt32(Sv[i].g * MM);
                        Sv[i].viz.Height = Convert.ToInt32(Sv[i].r * 2 * MM);


                        //if (Sv[i].viz.Width <= 3) { Sv[i].viz.Width = Sv[i].viz.Height = 5; }

                        Sv[i].viz.Click += new EventHandler(SVClick);


                        Sv[i].viz.BackColor = Color.Blue;


                        Sv[0].viz.Controls.Add(Sv[i].viz);
                    }

                }

            }


        }

        void SVClick(object sender, EventArgs e)
        {
            int i;

            for (i = 1; i < countV + 1; i++)
            {
                Sv[i].viz.BackColor = Color.Blue;
                Sv[i].edit = 0;      //обнуляем edit
            }

            PictureBox temp = (PictureBox)sender;
            label1.Text = Convert.ToString(temp.Name);
            i = Convert.ToInt32(label1.Text);
            deleteSv.Enabled = true;
            Sv[i].edit = 1; // новый edit
            sortSv();
            infosv();
            smena();

            temp.BackColor = Color.Red;
        }

        private void rotateR_Click(object sender, EventArgs e)
        {
            double temp;

            temp = XD;
            XD = YD;
            YD = temp;
            workdraw();

            int i, tOO;

            double tx, ty;




            for (i = 1; i < countV + 1; i++)
            {
                if (Sv[i].vid == 1)
                {
                    tx = Sv[i].x0;
                    ty = Sv[i].y0;
                    tOO = Convert.ToInt32(Sv[i].OO);
                    tOO++;
                    if (tOO > 3) { tOO = 0; };
                    Sv[i].x0 = ty;
                    Sv[i].y0 = YD - tx;
                    Sv[i].OO = Convert.ToString(tOO);
                }

                if (Sv[i].vid == 0)
                {
                    tx = Sv[i].x0;
                    ty = Sv[i].y0;
                    tOO = Convert.ToInt32(Sv[i].OO);
                    tOO++;
                    if (tOO > 3) { tOO = 0; };
                    Sv[i].x0 = ty;
                    Sv[i].y0 = YD - tx;
                    Sv[i].OO = Convert.ToString(tOO);
                }
            }



            drawsv();
            smena();
        }

        private void rotateL_Click(object sender, EventArgs e)
        {
            double temp;

            temp = XD;
            XD = YD;
            YD = temp;
            workdraw();

            int i, tOO;

            double tx, ty;

            for (i = 1; i < countV + 1; i++)
            {
                if (Sv[i].vid == 1)
                {
                    tx = Sv[i].x0;
                    ty = Sv[i].y0;
                    tOO = Convert.ToInt32(Sv[i].OO);
                    tOO--;
                    if (tOO < 0) { tOO = 3; };

                    Sv[i].x0 = XD - ty;
                    Sv[i].y0 = tx;
                    Sv[i].OO = Convert.ToString(tOO);
                }

                if (Sv[i].vid == 0)
                {
                    tx = Sv[i].x0;
                    ty = Sv[i].y0;
                    tOO = Convert.ToInt32(Sv[i].OO);
                    tOO--;
                    if (tOO < 0) { tOO = 3; };

                    Sv[i].x0 = XD - ty;
                    Sv[i].y0 = tx;
                    Sv[i].OO = Convert.ToString(tOO);
                }

            }


            drawsv();
            smena();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 1; i < countV + 1; i++)
            {
                if (Sv[i].vid == 1) { Sv[i].OO = "0"; }
                if (Sv[i].vid == 0)
                {

                    if ((Sv[i].OO == "0") && (Sv[i].x0 == 0)) { Sv[i].OO = "0"; }
                    if ((Sv[i].OO == "1") && (Sv[i].x0 == 0)) { Sv[i].OO = "0"; }
                    if ((Sv[i].OO == "2") && (Sv[i].x0 == XD)) { Sv[i].OO = "3"; }
                    if ((Sv[i].OO == "3") && (Sv[i].x0 == XD)) { Sv[i].OO = "3"; }

                    if ((Sv[i].OO == "0") && (Sv[i].y0 == 0)) { Sv[i].OO = "0"; }
                    if ((Sv[i].OO == "1") && (Sv[i].y0 == YD)) { Sv[i].OO = "1"; }
                    if ((Sv[i].OO == "2") && (Sv[i].y0 == YD)) { Sv[i].OO = "1"; }
                    if ((Sv[i].OO == "3") && (Sv[i].y0 == 0)) { Sv[i].OO = "0"; }
                }
                smena();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 1; i < countV + 1; i++)
            {
                if (Sv[i].vid == 1) { Sv[i].OO = "1"; }
                if (Sv[i].vid == 0)
                {
                    if ((Sv[i].OO == "0") && (Sv[i].x0 == 0)) { Sv[i].OO = "1"; }
                    if ((Sv[i].OO == "1") && (Sv[i].x0 == 0)) { Sv[i].OO = "1"; }
                    if ((Sv[i].OO == "2") && (Sv[i].x0 == XD)) { Sv[i].OO = "2"; }
                    if ((Sv[i].OO == "3") && (Sv[i].x0 == XD)) { Sv[i].OO = "2"; }

                    if ((Sv[i].OO == "0") && (Sv[i].y0 == 0)) { Sv[i].OO = "0"; }
                    if ((Sv[i].OO == "1") && (Sv[i].y0 == YD)) { Sv[i].OO = "1"; }
                    if ((Sv[i].OO == "2") && (Sv[i].y0 == YD)) { Sv[i].OO = "1"; }
                    if ((Sv[i].OO == "3") && (Sv[i].y0 == 0)) { Sv[i].OO = "0"; }
                }
            }
            smena();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 1; i < countV + 1; i++)
            {

                if (Sv[i].vid == 1) { Sv[i].OO = "2"; }
                if (Sv[i].vid == 0)
                {
                    if ((Sv[i].OO == "0") && (Sv[i].x0 == 0)) { Sv[i].OO = "1"; }
                    if ((Sv[i].OO == "0") && (Sv[i].x0 == XD)) { Sv[i].OO = "2"; }
                    if ((Sv[i].OO == "1") && (Sv[i].x0 == 0)) { Sv[i].OO = "1"; }
                    if ((Sv[i].OO == "2") && (Sv[i].x0 == XD)) { Sv[i].OO = "2"; }
                    if ((Sv[i].OO == "3") && (Sv[i].x0 == XD)) { Sv[i].OO = "2"; }

                    if ((Sv[i].OO == "0") && (Sv[i].y0 == 0)) { Sv[i].OO = "3"; }
                    if ((Sv[i].OO == "1") && (Sv[i].y0 == YD)) { Sv[i].OO = "2"; }
                    if ((Sv[i].OO == "2") && (Sv[i].y0 == YD)) { Sv[i].OO = "2"; }
                    if ((Sv[i].OO == "3") && (Sv[i].y0 == 0)) { Sv[i].OO = "3"; }
                }
            }
            smena();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 1; i < countV + 1; i++)
            {

                if (Sv[i].vid == 1) { Sv[i].OO = "3"; }
                if (Sv[i].vid == 0)
                {
                    if ((Sv[i].OO == "0") && (Sv[i].x0 == 0)) { Sv[i].OO = "0"; }
                    if ((Sv[i].OO == "0") && (Sv[i].x0 == XD)) { Sv[i].OO = "3"; }
                    if ((Sv[i].OO == "1") && (Sv[i].x0 == 0)) { Sv[i].OO = "0"; }
                    if ((Sv[i].OO == "2") && (Sv[i].x0 == XD)) { Sv[i].OO = "3"; }
                    if ((Sv[i].OO == "3") && (Sv[i].x0 == XD)) { Sv[i].OO = "3"; }

                    if ((Sv[i].OO == "0") && (Sv[i].y0 == 0)) { Sv[i].OO = "3"; }
                    if ((Sv[i].OO == "1") && (Sv[i].y0 == YD)) { Sv[i].OO = "2"; }
                    if ((Sv[i].OO == "2") && (Sv[i].y0 == YD)) { Sv[i].OO = "2"; }
                    if ((Sv[i].OO == "3") && (Sv[i].y0 == 0)) { Sv[i].OO = "3"; }
                }
            }

            smena();
        }

        void smena() // обновить данные по сверлу в текстбоксах
        {
            int i;

            if (label1.Text == "")
            {
                textBoxX.Text = textBoxY.Text = textBoxD.Text = textBoxG.Text = "";
            }

            if (label1.Text != "")
            {

                for (i = 1; i < countV + 1; i++)
                {

                    Sv[i].viz.BackColor = Color.Blue;
                    if (Sv[i].g < Convert.ToDouble(textBoxZD.Text) - 2) { Sv[i].viz.BackColor = Color.Yellow; }
                    if (Sv[i].edit == 1) { label1.Text = Convert.ToString(i); }

                }

                i = Convert.ToInt32(label1.Text);
                Sv[i].viz.BackColor = Color.Red;

                button11.BackColor = button22.BackColor = button33.BackColor = button44.BackColor = Color.LightGray;
                if (Sv[i].OO == "0")
                {
                    textBoxX.Text = Convert.ToString(Sv[i].x0);
                    textBoxY.Text = Convert.ToString(Sv[i].y0);
                    textBoxZ.Text = Convert.ToString(Sv[i].z0);
                    
                    textBoxD.Text = Convert.ToString(Sv[Convert.ToInt32(label1.Text)].r * 2);

                    textBoxG.Text = Convert.ToString(Sv[i].g);
                    label0.Text = Convert.ToString(Sv[i].OO);
                    //button2.Enabled = button3.Enabled = button4.Enabled = true;
                    //button1.Enabled = true;

                    button22.Enabled = button33.Enabled = button44.Enabled = true;
                    button11.Enabled = false;


                    button11.BackColor = Color.Green;
                }

                if (Sv[i].OO == "1")
                {
                    textBoxX.Text = Convert.ToString(Sv[Convert.ToInt32(label1.Text)].x0);
                    textBoxY.Text = Convert.ToString(YD - Sv[Convert.ToInt32(label1.Text)].y0);
                    textBoxZ.Text = Convert.ToString(Sv[i].z0);
                    textBoxD.Text = Convert.ToString(Sv[Convert.ToInt32(label1.Text)].r * 2);
                    textBoxG.Text = Convert.ToString(Sv[Convert.ToInt32(label1.Text)].g);
                    label0.Text = Convert.ToString(Sv[Convert.ToInt32(label1.Text)].OO);
                    //button1.Enabled = button3.Enabled = button4.Enabled = true;
                    //button2.Enabled = true;

                    button11.Enabled = button33.Enabled = button44.Enabled = true;
                    button22.Enabled = true;
                    button22.BackColor = Color.Green;
                }

                if (Sv[i].OO == "2")
                {
                    textBoxX.Text = Convert.ToString(XD - Sv[Convert.ToInt32(label1.Text)].x0);
                    textBoxY.Text = Convert.ToString(YD - Sv[Convert.ToInt32(label1.Text)].y0);
                    textBoxZ.Text = Convert.ToString(Sv[i].z0);
                    textBoxD.Text = Convert.ToString(Sv[Convert.ToInt32(label1.Text)].r * 2);
                    textBoxG.Text = Convert.ToString(Sv[Convert.ToInt32(label1.Text)].g);
                    label0.Text = Convert.ToString(Sv[Convert.ToInt32(label1.Text)].OO);
                    //button1.Enabled = button2.Enabled = button4.Enabled = true;
                    //button3.Enabled = true;

                    button11.Enabled = button22.Enabled = button44.Enabled = true;
                    button33.Enabled = false;
                    button33.BackColor = Color.Green;
                }

                if (Sv[i].OO == "3")
                {
                    textBoxX.Text = Convert.ToString(XD - Sv[Convert.ToInt32(label1.Text)].x0);
                    textBoxY.Text = Convert.ToString(Sv[Convert.ToInt32(label1.Text)].y0);
                    textBoxZ.Text = Convert.ToString(Sv[i].z0);
                    textBoxD.Text = Convert.ToString(Sv[Convert.ToInt32(label1.Text)].r * 2);
                    textBoxG.Text = Convert.ToString(Sv[Convert.ToInt32(label1.Text)].g);
                    label0.Text = Convert.ToString(Sv[Convert.ToInt32(label1.Text)].OO);
                    //button1.Enabled = button2.Enabled = button3.Enabled = true;
                    //button4.Enabled = false;

                    button11.Enabled = button22.Enabled = button33.Enabled = true;
                    button44.Enabled = false;
                    button44.BackColor = Color.Green;
                }
            }
        }

        private void filterOnlyReal(object sender, KeyPressEventArgs e)// только числа
        {
            if (!(Char.IsDigit(e.KeyChar)) && !((e.KeyChar == '.') && (((TextBox)sender).Text.IndexOf(".") == -1) && (((TextBox)sender).Text.Length != 0)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void infosv() // массив сверления в textcontrol.Text
        {
            int i;


            textcontrol.Text = "";

            for (i = 1; i < countV + 1; i++)
            {

                textcontrol.Text += i + "->" + Sv[i].x0 + " " + Sv[i].y0 + " " + /*Sv[i].g + " " + Sv[i].r + */" edit" + Sv[i].edit + " >" + Sv[i].OO + "\r\n";
            }
        }

        private void textBoxX_TextChanged(object sender, EventArgs e) // X сверла
        {
            int i = 0;
            double tempX;
            if (label1.Text != "")
            {
                i = Convert.ToInt32(label1.Text);



                if ((textBoxX.Text != "") && (Sv[i].vid == 0)) //торцевые
                {
                    tempX = Convert.ToDouble(textBoxX.Text);

                    if ((Sv[i].y0 == 0) || (Sv[i].y0 == YD)) // сверху и снизу
                    {

                        if ((Sv[i].OO == "1") || (Sv[i].OO == "0"))//слева
                        {
                            if (Convert.ToDouble(textBoxX.Text) > XD) { textBoxX.Text = Convert.ToString(Sv[i].x0); }// нужно тестить

                            Sv[i].x0 = Convert.ToDouble(textBoxX.Text);
                            Sv[i].viz.Left = Convert.ToInt32((Sv[i].x0 - Sv[i].r) * MM);
                        }

                        if ((Sv[i].OO == "2") || (Sv[i].OO == "3"))//справа
                        {
                            if (Convert.ToDouble(textBoxX.Text) > XD) { textBoxX.Text = Convert.ToString(XD - Sv[i].x0); }// нужно тестить

                            Sv[i].x0 = XD - Convert.ToDouble(textBoxX.Text);
                            Sv[i].viz.Left = Convert.ToInt32((Sv[i].x0 - Sv[i].r) * MM);
                        }
                       
                    }


                    

                }



                if ((textBoxX.Text != "") && (Sv[i].vid == 1)) //вертикальные
                {
                    tempX = Convert.ToDouble(textBoxX.Text);
                    if ((tempX != 0) && (tempX != XD)) // проверяем смену на торцевые
                    {
                        if (((Sv[i].OO == "0") || (Sv[i].OO == "1"))) //слева
                        {
                            if (tempX > XD) { textBoxX.Text = Convert.ToString(Sv[i].x0); }// нужно тестить

                            i = Convert.ToInt32(label1.Text);
                            Sv[i].x0 = tempX;
                            Sv[i].viz.Left = Convert.ToInt32((Sv[i].x0 - Sv[i].r) * MM);
                        }

                        if (((Sv[i].OO == "2") || (Sv[i].OO == "3"))) //справа
                        {
                            if (tempX > XD) { textBoxX.Text = Convert.ToString(Sv[i].x0); }// нужно тестить

                            i = Convert.ToInt32(label1.Text);
                            Sv[i].x0 = XD - Convert.ToDouble(textBoxX.Text);
                            Sv[i].viz.Left = Convert.ToInt32((Sv[i].x0 - Sv[i].r) * MM);
                        }
                    }

                    if ((tempX == 0) || (tempX == XD)) // проверяем смену на торцевые
                    {
                        Sv[i].x0 = tempX;
                        Sv[i].z0 = Convert.ToDouble(textBoxZD.Text) / 2;
                        textBoxZ.Text = Sv[i].z0.ToString();
                        Sv[i].vid = 0;


                        workdraw();
                        drawsv();
                        smena();



                    }


                }
            }
        }

        private void textBoxY_TextChanged(object sender, EventArgs e) // Y сверла
        {
            double tempY;
            int i = 0;
            if (label1.Text != "")
            {
                i = Convert.ToInt32(label1.Text);

                if ((textBoxY.Text != "") && (Sv[i].vid == 0)) //торцевые
                {
                    if ((Sv[i].x0 == 0) || (Sv[i].x0 == XD)) // справа слева
                    {

                        if ((Sv[i].OO == "0") || (Sv[i].OO == "3"))//снизу
                        {
                            if (Convert.ToDouble(textBoxY.Text) > YD) { textBoxY.Text = Convert.ToString(Sv[i].y0); }// нужно тестить

                            Sv[i].y0 = Convert.ToDouble(textBoxY.Text);

                            Sv[i].viz.Top = Convert.ToInt32((YD - Sv[i].y0 - Sv[i].r) * MM) ;
                        }

                        if ((Sv[i].OO == "1") || (Sv[i].OO == "2"))//сверху
                        {
                            if (Convert.ToDouble(textBoxY.Text) > YD) { textBoxY.Text = Convert.ToString(YD - Sv[i].y0); }// нужно тестить

                            Sv[i].y0 = YD - Convert.ToDouble(textBoxY.Text);

                            Sv[i].viz.Top = Convert.ToInt32((YD - Sv[i].y0 - Sv[i].r) * MM);
                        }

                    }



                }

            }




            if ((textBoxY.Text != "") && (Sv[i].vid == 1)) // вертикальные
            {
                tempY = Convert.ToDouble(textBoxY.Text);
                if ((Sv[i].OO == "0") || (Sv[i].OO == "3"))
                {
                    if (Convert.ToDouble(textBoxY.Text) > YD) { textBoxY.Text = Convert.ToString(Sv[i].y0); }// нужно тестить

                    i = Convert.ToInt32(label1.Text);
                    Sv[i].y0 = Convert.ToDouble(textBoxY.Text);
                    Sv[i].viz.Top = Convert.ToInt32( (YD - Sv[i].y0 - Sv[i].r) * MM);
                }

                if ((Sv[i].OO == "1") || (Sv[i].OO == "2"))
                {
                    if (Convert.ToDouble(textBoxY.Text) > YD) { textBoxY.Text = Convert.ToString(Sv[i].y0); }// нужно тестить

                    i = Convert.ToInt32(label1.Text);
                    Sv[i].y0 = YD - Convert.ToDouble(textBoxY.Text);
                    Sv[i].viz.Top = Convert.ToInt32((YD - Sv[i].y0  - Sv[i].r) * MM);
                }

                if ((tempY == 0) || (tempY == YD)) // проверяем смену на торцевые
                {
                    Sv[i].y0 = tempY;
                    Sv[i].z0 = Convert.ToDouble(textBoxZD.Text) / 2;
                    textBoxZ.Text = Sv[i].z0.ToString();
                    Sv[i].vid = 0;
                    workdraw();
                    drawsv();
                    smena();

                }


            }

        }

        private void textBoxXD_TextChanged(object sender, EventArgs e) // X детали
        {
            if (textBoxXD.Text != "")
            {
                for (var i = 1; i < countV + 1; i++)

                {
                    if (Sv[i].vid == 1)
                    {
                        if (Sv[i].OO == "2") { Sv[i].x0 = Convert.ToDouble(textBoxXD.Text) - (XD - Sv[i].x0); }
                        if (Sv[i].OO == "3") { Sv[i].x0 = Convert.ToDouble(textBoxXD.Text) - (XD - Sv[i].x0); }
                    }

                    if (Sv[i].vid == 0)
                    {
                        if (Sv[i].OO == "2") { Sv[i].x0 = Convert.ToDouble(textBoxXD.Text) - (XD - Sv[i].x0); }
                        if (Sv[i].OO == "3") { Sv[i].x0 = Convert.ToDouble(textBoxXD.Text) - (XD - Sv[i].x0); }
                    }

                }


                XD = Convert.ToDouble(textBoxXD.Text);

                workdraw();
                drawsv();

            }
        }

        private void textBoxYD_TextChanged(object sender, EventArgs e) // Y детали
        {
            if (textBoxYD.Text != "")
            {
                for (var i = 1; i < countV + 1; i++)
                {
                    if (Sv[i].vid == 1)
                    {
                        if (Sv[i].OO == "1") { Sv[i].y0 = Convert.ToDouble(textBoxYD.Text) - (YD - Sv[i].y0); }
                        if (Sv[i].OO == "2") { Sv[i].y0 = Convert.ToDouble(textBoxYD.Text) - (YD - Sv[i].y0); }
                    }
                    if (Sv[i].vid == 0)
                    {
                        if (Sv[i].OO == "1") { Sv[i].y0 = Convert.ToDouble(textBoxYD.Text) - (YD - Sv[i].y0); }
                        if (Sv[i].OO == "2") { Sv[i].y0 = Convert.ToDouble(textBoxYD.Text) - (YD - Sv[i].y0); }
                    }
                }

                YD = Convert.ToDouble(textBoxYD.Text);

                workdraw();
                drawsv();


            }


        }



        private void textBoxZD_TextChanged(object sender, EventArgs e)
        {
            if (textBoxZD.Text != "") { textBoxZD.Text = textBoxZD.Text; }
            if (textBoxZD.Text == "")
            {
                textBoxZD.Text = "16";

                textBoxZD.SelectionStart = 0;
                textBoxZD.SelectionLength = textBoxZD.Text.Length;
            }
            ZD = Convert.ToDouble(textBoxZD.Text);
        }

        private void textBoxD_TextChanged(object sender, EventArgs e) // смена диаметра
        {
            if ((textBoxD.Text != "") && (label1.Text != ""))
            {
                int i, mmm = 1;
                i = Convert.ToInt32(label1.Text);

                Sv[i].r = Convert.ToDouble(textBoxD.Text) / 2;

                if (Sv[i].vid == 1)
                {

                    if (Convert.ToInt32(Sv[i].r * 2 * MM) < 20)
                    {
                        mmm = 1;
                    }

                    Sv[i].viz.Width = Sv[i].viz.Height = Convert.ToInt32(Sv[i].r * 2 * MM * mmm);
                   
                    // if (Sv[i].viz.Width <= 5) { Sv[i].viz.Width = Sv[i].viz.Height = 5; }

                    Sv[i].viz.Left = Convert.ToInt32((Sv[i].x0 - Sv[i].r) * MM);
                    Sv[i].viz.Top = Convert.ToInt32((YD - Sv[i].y0 - Sv[i].r) * MM);

                    System.Drawing.Drawing2D.GraphicsPath myPath = new System.Drawing.Drawing2D.GraphicsPath();

                    myPath.AddEllipse(0, 0, Sv[i].viz.Width, Sv[i].viz.Height);

                    Region myRegion = new Region(myPath);
                    Sv[i].viz.Region = myRegion;
                }

                if (Sv[i].vid == 0)//торцевые
                {
                    if ((Sv[i].y0 == 0) || (Sv[i].y0 == YD))//снизу сверху
                    {
                        Sv[i].viz.Width = Convert.ToInt32(Sv[i].r * 2 * MM); //Convert.ToInt32(Convert.ToDouble(textBoxD.Text));
                        //if (Sv[i].viz.Width <= 5) { Sv[i].viz.Width = 5; }
                        Sv[i].viz.Left = Convert.ToInt32((Sv[i].x0 - Sv[i].r) * MM);
                    }

                    if ((Sv[i].x0 == 0) || (Sv[i].x0 == XD))//справа слева
                    {
                        Sv[i].viz.Height = Convert.ToInt32(Sv[i].r * 2 * MM);// Convert.ToInt32(Convert.ToDouble(textBoxD.Text));
                      //  if (Sv[i].viz.Height <= 5) { Sv[i].viz.Height = 5; }
                        Sv[i].viz.Top = Convert.ToInt32((YD - Sv[i].y0 - Sv[i].r) * MM);
                    }

                }

            }
        }


        private void textBoxG_TextChanged(object sender, EventArgs e) // смена глубины
        {
            if ((textBoxG.Text != "") && (label1.Text != ""))
            {
                int i;
                i = Convert.ToInt32(label1.Text);

                Sv[i].g = Convert.ToDouble(textBoxG.Text);



                if (Sv[i].vid == 0)//торцевые
                {
                    if (Sv[i].y0 == 0)//снизу
                    {
                        Sv[i].viz.Height = Convert.ToInt32(Convert.ToDouble(textBoxG.Text) * MM);

                        Sv[i].viz.Top = Convert.ToInt32((YD) * MM) - Sv[i].viz.Height;
                    }

                    if (Sv[i].y0 == YD)//сверху
                    {
                        Sv[i].viz.Height = Convert.ToInt32(Convert.ToDouble(textBoxG.Text) * MM);

                        Sv[i].viz.Top = 0;
                    }

                    if (Sv[i].x0 == 0) //слева
                    {
                        Sv[i].viz.Width = Convert.ToInt32(Convert.ToDouble(textBoxG.Text) * MM);
                       
                        Sv[i].viz.Left = 0;
                    }

                    if (Sv[i].x0 == XD) //справа
                    {
                        Sv[i].viz.Width = Convert.ToInt32(Convert.ToDouble(textBoxG.Text) * MM);


                        Sv[i].viz.Left = Convert.ToInt32((XD - Sv[i].g) * MM);
                    }


                }
            }


        }
        // WOOD WOP
        // WOOD WOP
        // WOOD WOP
        // WOOD WOP
        // WOOD WOP
        // WOOD WOP
        // WOOD WOP
        // WOOD WOP
        // WOOD WOP

        private void saveWW_Click(object sender, EventArgs e)
        {
            textcontrol.Text = "";


            textcontrol.Text += "[H" + "\r\n";
            textcontrol.Text += "VERSION=\"4.0 Alpha\"" + "\r\n";
            textcontrol.Text += "OP=\"1\"" + "\r\n";
            textcontrol.Text += "WRK2=\"0\"" + "\r\n";
            textcontrol.Text += "O2=\"0\"" + "\r\n";
            textcontrol.Text += "O4=\"0\"" + "\r\n";
            textcontrol.Text += "O3=\"0\"" + "\r\n";
            textcontrol.Text += "O5=\"0\"" + "\r\n";
            textcontrol.Text += "ML=\"2000\"" + "\r\n";
            textcontrol.Text += "DN=\"STANDARD\"" + "\r\n";
            textcontrol.Text += "GP=\"0\"\r\nGY=\"0\"\r\nGXY=\"0\"\r\nNP=\"1\"\r\nNE=\"0\"\r\nNA=\"0\"\r\nBFS=\"0\"\r\nUS=\"0\"\r\nCB=\"0\"\r\nUP=\"0\"\r\nDW=\"0\"\r\nMAT=\"HOMAG\"\r\nINCH=\"0\"\r\nVIEW=\"NOMIRROR\"\r\nANZ=\"1\"\r\nBES=\"0\"\r\nENT=\"0\"\r\n";

            textcontrol.Text += "_BSX=" + XD + "\r\n";
            textcontrol.Text += "_BSY=" + YD + "\r\n";
            textcontrol.Text += "_BSZ=" + textBoxZD.Text + "\r\n"; // ТОЛЩИНА ДЕТАЛИ

            textcontrol.Text += "FNX=0.000000\r\nFNY=0.000000\r\nRNX=0.000000\r\nRNY=0.000000\r\nRNZ=0.000000\r\n";

            textcontrol.Text += "_RX=" + XD + "\r\n";
            textcontrol.Text += "_BY=" + YD + "\r\n\r\n";
            textcontrol.Text += "[001" + "\r\n";
            textcontrol.Text += "l=\"" + XD + "\"\r\n";
            textcontrol.Text += "KM=\"length\"" + "\r\n";
            textcontrol.Text += "w=\"" + YD + "\"\r\n";
            textcontrol.Text += "KM=\"width\"" + "\r\n";
            textcontrol.Text += "t=\"" + textBoxZD.Text + "\"\r\n";
            textcontrol.Text += "KM=\"thickness\"" + "\r\n";
            textcontrol.Text += "" + "\r\n";




            textcontrol.Text += "<100 \\WerkStck\\" + "\r\n";

            textcontrol.Text += "LA=\"l\"\r\nBR=\"w\"\r\nDI=\"t\"\r\nFNX=\"0\"\r\nFNY=\"0\"\r\nAX=\"0\"\r\nAY=\"0\"\r\n\r\n";
            int i;

            textcontrol.Text += "\r\n<101 \\Kommentar\\\r\n";

            String[] commentstring = commentBox.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int commentSTR = 0; commentSTR < commentstring.Length; commentSTR++)

            {

                textcontrol.Text += "KM=\"" + commentstring[commentSTR] + "\"\r\n";

            };

            double tx, ty, tz;
            for (i = 1; i < countV + 1; i++)
            {

                if (Sv[i].vid == 0) // торцевые
                {

                    tx = Sv[i].x0;
                    ty = Sv[i].y0;
                    tz = Sv[i].z0;




                    if ((Sv[i].OO == "1")) // сверху влево и сверху вниз Convert.ToString(tx)
                    {


                        ty = YD - ty;

                        textcontrol.Text += "<103 \\BohrHoriz\\" + "\r\n";
                        textcontrol.Text += "MI=\"0\"\r\n";
                        textcontrol.Text += "XA=\"" + Convert.ToString(tx) + "\"" + "\r\n";
                        textcontrol.Text += "YA=\"" + Convert.ToString(ty) + "\"" + "\r\n";
                        textcontrol.Text += "ZA=\"" + tz + "\"\r\n";

                        textcontrol.Text += "DU=\"" + Convert.ToString(Sv[i].r * 2) + "\"\r\n";

                        textcontrol.Text += "TI=\"" + Convert.ToString(Sv[i].g) + "\"\r\n";
                        textcontrol.Text += "ANA=\"20\"\r\n";

                        if (Sv[i].y0 == YD) { textcontrol.Text += "BM=\"YP\"\r\n"; } // сверху вниз
                        if (Sv[i].x0 == 0) { textcontrol.Text += "BM=\"XP\"\r\n"; } // слева направо

                        textcontrol.Text += "AN=\"1\"\r\nAB=\"32\"\r\n";
                        textcontrol.Text += "HP=\"0\"\r\nSP=\"0\"\r\nYVE=\"0\"\r\n";
                        textcontrol.Text += "WW=\"50,51,52,53,93,94,95,56,153,151\"";
                        textcontrol.Text += "ASG=\"2\"\r\nKAT=\"Horizontalbohren\"\r\nMNM=\"Горизонтальное сверление\"\r\n";
                        textcontrol.Text += "MX=\"0\"\r\nMY=\"0\"\r\nMZ=\"0\"\r\nMXF=\"1\"\r\nMYF=\"1\"\r\nMZF=\"1\"\r\n";
                        textcontrol.Text += "KO=\"3\"\r\n\r\n";
                    }

                    if ((Sv[i].OO == "0")) // снизу влево и снизу вверх
                    {
                        textcontrol.Text += "<103 \\BohrHoriz\\" + "\r\n";
                        textcontrol.Text += "MI=\"0\"\r\n";
                        textcontrol.Text += "XA=\"" + Convert.ToString(tx) + "\"" + "\r\n";
                        textcontrol.Text += "YA=\"" + Convert.ToString(ty) + "\"" + "\r\n";
                        textcontrol.Text += "ZA=\"" + tz + "\"\r\n";

                        textcontrol.Text += "DU=\"" + Convert.ToString(Sv[i].r * 2) + "\"\r\n";

                        textcontrol.Text += "TI=\"" + Convert.ToString(Sv[i].g) + "\"\r\n";
                        textcontrol.Text += "ANA=\"20\"\r\n";

                        if (Sv[i].y0 == 0) { textcontrol.Text += "BM=\"YP\"\r\n"; } // снизу вверх
                        if (Sv[i].x0 == 0) { textcontrol.Text += "BM=\"XP\"\r\n"; } // слева направо


                        textcontrol.Text += "AN=\"1\"\r\nAB=\"32\"\r\n";
                        textcontrol.Text += "HP=\"0\"\r\nSP=\"0\"\r\nYVE=\"0\"\r\n";
                        textcontrol.Text += "WW=\"50,51,52,53,93,94,95,56,153,151\"";
                        textcontrol.Text += "ASG=\"2\"\r\nKAT=\"Horizontalbohren\"\r\nMNM=\"Горизонтальное сверление\"\r\n";
                        textcontrol.Text += "MX=\"0\"\r\nMY=\"0\"\r\nMZ=\"0\"\r\nMXF=\"1\"\r\nMYF=\"1\"\r\nMZF=\"1\"\r\n";
                        textcontrol.Text += "KO=\"0\"\r\n\r\n";
                    }

                    if ((Sv[i].OO == "2")) // сверху вправо сверху вниз
                    {
                        ty = YD - ty;
                        tx = XD - tx;


                        textcontrol.Text += "<103 \\BohrHoriz\\" + "\r\n";
                        textcontrol.Text += "MI=\"0\"\r\n";
                        textcontrol.Text += "XA=\"" + Convert.ToString(tx) + "\"" + "\r\n";
                        textcontrol.Text += "YA=\"" + Convert.ToString(ty) + "\"" + "\r\n";
                        textcontrol.Text += "ZA=\"" + tz + "\"\r\n";

                        textcontrol.Text += "DU=\"" + Convert.ToString(Sv[i].r * 2) + "\"\r\n";

                        textcontrol.Text += "TI=\"" + Convert.ToString(Sv[i].g) + "\"\r\n";
                        textcontrol.Text += "ANA=\"20\"\r\n";

                        if (Sv[i].y0 == YD) { textcontrol.Text += "BM=\"YP\"\r\n"; } // снизу вверх
                        if (Sv[i].x0 == XD) { textcontrol.Text += "BM=\"XP\"\r\n"; } // слева направо

                        textcontrol.Text += "AN=\"1\"\r\nAB=\"32\"\r\n";
                        textcontrol.Text += "HP=\"0\"\r\nSP=\"0\"\r\nYVE=\"0\"\r\n";
                        textcontrol.Text += "WW=\"50,51,52,53,93,94,95,56,153,151\"";
                        textcontrol.Text += "ASG=\"2\"\r\nKAT=\"Horizontalbohren\"\r\nMNM=\"Горизонтальное сверление\"\r\n";
                        textcontrol.Text += "MX=\"0\"\r\nMY=\"0\"\r\nMZ=\"0\"\r\nMXF=\"1\"\r\nMYF=\"1\"\r\nMZF=\"1\"\r\n";
                        textcontrol.Text += "KO=\"2\"\r\n\r\n";
                    }

                    if ((Sv[i].OO == "3")) // снизу вверх снизу влево
                    {


                        tx = XD - tx;

                        textcontrol.Text += "<103 \\BohrHoriz\\" + "\r\n";
                        textcontrol.Text += "MI=\"0\"\r\n";
                        textcontrol.Text += "XA=\"" + Convert.ToString(tx) + "\"" + "\r\n";
                        textcontrol.Text += "YA=\"" + Convert.ToString(ty) + "\"" + "\r\n";
                        textcontrol.Text += "ZA=\"" + tz + "\"\r\n";

                        textcontrol.Text += "DU=\"" + Convert.ToString(Sv[i].r * 2) + "\"\r\n";

                        textcontrol.Text += "TI=\"" + Convert.ToString(Sv[i].g) + "\"\r\n";
                        textcontrol.Text += "ANA=\"20\"\r\n";

                        if (Sv[i].y0 == 0) { textcontrol.Text += "BM=\"YP\"\r\n"; } // снизу вверх
                        if (Sv[i].x0 == XD) { textcontrol.Text += "BM=\"XP\"\r\n"; } // слева направо

                        textcontrol.Text += "AN=\"1\"\r\nAB=\"32\"\r\n";
                        textcontrol.Text += "HP=\"0\"\r\nSP=\"0\"\r\nYVE=\"0\"\r\n";
                        textcontrol.Text += "WW=\"50,51,52,53,93,94,95,56,153,151\"";
                        textcontrol.Text += "ASG=\"2\"\r\nKAT=\"Horizontalbohren\"\r\nMNM=\"Горизонтальное сверление\"\r\n";
                        textcontrol.Text += "MX=\"0\"\r\nMY=\"0\"\r\nMZ=\"0\"\r\nMXF=\"1\"\r\nMYF=\"1\"\r\nMZF=\"1\"\r\n";

                        textcontrol.Text += "KO=\"1\"\r\n\r\n";

                    }
                }







                if (Sv[i].vid == 1) // вертикальные ss ls lsl sss 
                {
                    tx = Sv[i].x0;
                    ty = Sv[i].y0;



                    if ((Sv[i].OO == "0")) // слева снизу
                    {
                        textcontrol.Text += "<102 \\BohrVert\\" + "\r\n";
                        textcontrol.Text += "XA=\"" + Convert.ToString(Sv[i].x0) + "\"" + "\r\n";
                        textcontrol.Text += "YA=\"" + Convert.ToString(Sv[i].y0) + "\"" + "\r\n";


                        if (Sv[i].g < Convert.ToDouble(textBoxZD.Text))
                        {
                            textcontrol.Text += "BM=\"LS\"" + "\r\n";
                            textcontrol.Text += "TI=\"" + Convert.ToString(Sv[i].g) + "\"\r\n";
                        }

                        if (Sv[i].g >= Convert.ToDouble(textBoxZD.Text))
                        {
                            textcontrol.Text += "BM=\"LSL\"" + "\r\n";
                        }


                        textcontrol.Text += "DU=\"" + Convert.ToString(Sv[i].r * 2) + "\"\r\n";

                        textcontrol.Text += "AN=\"1\"\r\nMI=\"0\"\r\nS_=\"2\"\r\nAB=\"32\"\r\nWI=\"0\"\r\nHP=\"0\"\r\nSP=\"0\"\r\nYVE=\"0\"\r\n";


                        textcontrol.Text += "WW=\"60,61,62,88,90,91,92,150\"" + "\r\n";
                        textcontrol.Text += "ASG=\"2\"\r\nKAT=\"Bohren vertikal\"\r\n";
                        textcontrol.Text += "MNM=\"Вертикальное сверление\"" + "\r\n";
                        textcontrol.Text += "MX=\"0\"\r\nMY=\"0\"\r\nMZ=\"0\"\r\nMXF=\"1\"\r\nMYF=\"1\"\r\nMZF=\"1\"\r\n";

                        textcontrol.Text += "KO=\"0\"" + "\r\n\r\n";
                    }

                    if ((Sv[i].OO == "1"))// слева сверху
                    {
                        textcontrol.Text += "<102 \\BohrVert\\" + "\r\n";
                        textcontrol.Text += "XA=\"" + Convert.ToString(Sv[i].x0) + "\"" + "\r\n";
                        textcontrol.Text += "YA=\"" + Convert.ToString(YD - Sv[i].y0) + "\"" + "\r\n";


                        if (Sv[i].g < Convert.ToDouble(textBoxZD.Text))
                        {
                            textcontrol.Text += "BM=\"LS\"" + "\r\n";
                            textcontrol.Text += "TI=\"" + Convert.ToString(Sv[i].g) + "\"\r\n";
                        }

                        if (Sv[i].g >= Convert.ToDouble(textBoxZD.Text))
                        {
                            textcontrol.Text += "BM=\"LSL\"" + "\r\n";
                        }


                        textcontrol.Text += "DU=\"" + Convert.ToString(Sv[i].r * 2) + "\"\r\n";

                        textcontrol.Text += "AN=\"1\"\r\nMI=\"0\"\r\nS_=\"2\"\r\nAB=\"32\"\r\nWI=\"0\"\r\nHP=\"0\"\r\nSP=\"0\"\r\nYVE=\"0\"\r\n";


                        textcontrol.Text += "WW=\"60,61,62,88,90,91,92,150\"" + "\r\n";
                        textcontrol.Text += "ASG=\"2\"\r\nKAT=\"Bohren vertikal\"\r\n";
                        textcontrol.Text += "MNM=\"Вертикальное сверление\"" + "\r\n";
                        textcontrol.Text += "MX=\"0\"\r\nMY=\"0\"\r\nMZ=\"0\"\r\nMXF=\"1\"\r\nMYF=\"1\"\r\nMZF=\"1\"\r\n";

                        textcontrol.Text += "KO=\"3\"" + "\r\n\r\n";
                    }

                    if ((Sv[i].OO == "2")) // справа сверху
                    {
                        textcontrol.Text += "<102 \\BohrVert\\" + "\r\n";
                        textcontrol.Text += "XA=\"" + Convert.ToString(XD - Sv[i].x0) + "\"" + "\r\n";
                        textcontrol.Text += "YA=\"" + Convert.ToString(YD - Sv[i].y0) + "\"" + "\r\n";


                        if (Sv[i].g < Convert.ToDouble(textBoxZD.Text))
                        {
                            textcontrol.Text += "BM=\"LS\"" + "\r\n";
                            textcontrol.Text += "TI=\"" + Convert.ToString(Sv[i].g) + "\"\r\n";
                        }

                        if (Sv[i].g >= Convert.ToDouble(textBoxZD.Text))
                        {
                            textcontrol.Text += "BM=\"LSL\"" + "\r\n";
                        }


                        textcontrol.Text += "DU=\"" + Convert.ToString(Sv[i].r * 2) + "\"\r\n";

                        textcontrol.Text += "AN=\"1\"\r\nMI=\"0\"\r\nS_=\"2\"\r\nAB=\"32\"\r\nWI=\"0\"\r\nHP=\"0\"\r\nSP=\"0\"\r\nYVE=\"0\"\r\n";


                        textcontrol.Text += "WW=\"60,61,62,88,90,91,92,150\"" + "\r\n";
                        textcontrol.Text += "ASG=\"2\"\r\nKAT=\"Bohren vertikal\"\r\n";
                        textcontrol.Text += "MNM=\"Вертикальное сверление\"" + "\r\n";
                        textcontrol.Text += "MX=\"0\"\r\nMY=\"0\"\r\nMZ=\"0\"\r\nMXF=\"1\"\r\nMYF=\"1\"\r\nMZF=\"1\"\r\n";
                        textcontrol.Text += "KO=\"2\"" + "\r\n\r\n";

                    }

                    if ((Sv[i].OO == "3"))// справа снизу
                    {
                        textcontrol.Text += "<102 \\BohrVert\\" + "\r\n";
                        textcontrol.Text += "XA=\"" + Convert.ToString(XD - Sv[i].x0) + "\"" + "\r\n";
                        textcontrol.Text += "YA=\"" + Convert.ToString(Sv[i].y0) + "\"" + "\r\n";


                        if (Sv[i].g < Convert.ToDouble(textBoxZD.Text))
                        {
                            textcontrol.Text += "BM=\"LS\"" + "\r\n";
                            textcontrol.Text += "TI=\"" + Convert.ToString(Sv[i].g) + "\"\r\n";
                        }

                        if (Sv[i].g >= Convert.ToDouble(textBoxZD.Text))
                        {
                            textcontrol.Text += "BM=\"LSL\"" + "\r\n";
                        }

                        textcontrol.Text += "DU=\"" + Convert.ToString(Sv[i].r * 2) + "\"\r\n";

                        textcontrol.Text += "AN=\"1\"\r\nMI=\"0\"\r\nS_=\"2\"\r\nAB=\"32\"\r\nWI=\"0\"\r\nHP=\"0\"\r\nSP=\"0\"\r\nYVE=\"0\"\r\n";


                        textcontrol.Text += "WW=\"60,61,62,88,90,91,92,150\"" + "\r\n";
                        textcontrol.Text += "ASG=\"2\"\r\nKAT=\"Bohren vertikal\"\r\n";
                        textcontrol.Text += "MNM=\"Вертикальное сверление\"" + "\r\n";
                        textcontrol.Text += "MX=\"0\"\r\nMY=\"0\"\r\nMZ=\"0\"\r\nMXF=\"1\"\r\nMYF=\"1\"\r\nMZF=\"1\"\r\n";

                        textcontrol.Text += "KO=\"1\"" + "\r\n\r\n";

                    }

                }
            }





            //textcontrol.Text += "??=\"0\"\r\n!" + "\r\n";
            textcontrol.Text += "!" + "\r\n";



            textcontrol.Text += "" + "\r\n";
            textcontrol.Text += "" + "\r\n";
            textcontrol.Text += "" + "\r\n";
            textcontrol.Text += "" + "\r\n";
            textcontrol.Text += "" + "\r\n";
            textcontrol.Text += "" + "\r\n";




            if (dnd)
            {
                outFileWoodWOP = ListFileDrop[FileDroplistBox.SelectedIndex];

                outFileWoodWOP = outFileWoodWOP.Replace(".pd4", "");

                if (!outFileWoodWOP.ToLower().EndsWith(".mpr")) { outFileWoodWOP += ".mpr"; }




                System.IO.StreamWriter sw = new
                           System.IO.StreamWriter(outFileWoodWOP);

                sw.WriteLine(textcontrol.Text);
                sw.Close();
                WWF = true;

            }

            if ((FileName.Text != "") && ((textBoxXD.Text != "") || (textBoxYD.Text != "")) && !WWF)//здесь обрабатываеться наш файл если файл DXF не был открыт работает наполовину)))
            {
                saveFileDialog1.Filter = "|*.mpr";
                saveFileDialog1.FileName = FileName.Text;




                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {


                    outFileWoodWOP = saveFileDialog1.FileName;



                    if (!outFileWoodWOP.ToLower().EndsWith(".mpr")) { outFileWoodWOP += ".mpr"; }




                    StreamWriter sw = new StreamWriter(outFileWoodWOP, false, Encoding.GetEncoding("Windows-1251"));


                    sw.WriteLine(textcontrol.Text);
                    sw.Close();
                }
            }


            /*      saveFileDialog1.Filter = "|*.mpr";



                  if ((openFileDialog1.FileName == "") && ((textBoxXD.Text != "") || (textBoxYD.Text != "")) && (FileName.Text != "")) //здесь обрабатываеться наш файл если файл DXF не был открыт работает наполовину)))
                  {
                      saveFileDialog1.FileName = FileName.Text;

                     // MessageBox.Show( saveFileDialog1.InitialDirectory);

                      if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                      {
                          outFileWoodWOP = saveFileDialog1.FileName;

                         // if (!outFileWoodWOP.EndsWith(".pd4")) { outFileWoodWOP += ".mpr"; }


                                  FileName.Text = outFileWoodWOP;


                                  using (StreamWriter sw = new StreamWriter(outFileWoodWOP, false, Encoding.GetEncoding(1251)))
                                  {
                                      sw.Write(textcontrol.Text);
                                      sw.Close();
                                      return;
                                  }


                      }



                  }

                  if ((FileName.Text != "") && (openFileDialog1.FileName!=""))
                  {
                      outFileWoodWOP = openFileDialog1.FileName; MessageBox.Show(outFileWoodWOP);
                      outFileWoodWOP = outFileWoodWOP.Remove(outFileWoodWOP.LastIndexOf(".")); MessageBox.Show(outFileWoodWOP);

                      outFileWoodWOP = outFileWoodWOP.Remove(outFileWoodWOP.LastIndexOf("\\")+1) +"ww4"; MessageBox.Show(outFileWoodWOP);



                      if (!(Directory.Exists(outFileWoodWOP))) { Directory.CreateDirectory(outFileWoodWOP); }

                      saveFileDialog1.InitialDirectory = outFileWoodWOP;//заходим в папку WW4

                      outFileWoodWOP = outFileWoodWOP + "\\" + FileName.Text + ".mpr";

                      saveFileDialog1.FileName = FileName.Text;
                      if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                      {

                          using (StreamWriter sw = new StreamWriter(outFileWoodWOP, false, Encoding.GetEncoding(1251)))
                          {
                              sw.Write(textcontrol.Text);
                              sw.Close();

                              saveFileDialog1.InitialDirectory = openFileDialog1.InitialDirectory = outFileWoodWOP.Remove(outFileWoodWOP.LastIndexOf("\\") - 3);//выходим из папки WW4
                              return;

                          }
                      }
                  }*/
        }
        // CAD 4
        // CAD 4
        // CAD 4
        // CAD 4
        // CAD 4
        // CAD 4
        // CAD 4
        // CAD 4
        // CAD 4
        private void saveCad4_Click(object sender, EventArgs e)
        {
            textcontrol.Text = "";
            textcontrol.Text += "BEGIN ID CID3" + "\r\n";
            textcontrol.Text += "  '=  " + "\r\n";
            textcontrol.Text += "  REL= 3.10" + "\r\n";
            textcontrol.Text += "  AXIS= x+,y-,z-" + "\r\n";
            textcontrol.Text += "END ID" + "\r\n";
            textcontrol.Text += "BEGIN MAINDATA" + "\r\n";
            textcontrol.Text += "LX= " + textBoxXD.Text + ".00\r\n";
            textcontrol.Text += "LY= " + textBoxYD.Text + ".00\r\n";
            textcontrol.Text += "LZ= " + textBoxZD.Text + ".00\r\n";
            textcontrol.Text += "OX= 0.00" + "\r\n";
            textcontrol.Text += "OY= 0.00" + "\r\n";
            textcontrol.Text += "OZ= 0.00" + "\r\n";
            textcontrol.Text += "END MAINDATA" + "\r\n";

            textcontrol.Text += "BEGIN PARAMETRIC SECTION" + "\r\n";
            textcontrol.Text += "" + "\r\n";
            textcontrol.Text += "[ 0.0 ] PROGETTO  _ (" + "\r\n";
            textcontrol.Text += "		  LPX = " + textBoxXD.Text + " NOP \"Height\"" + "\r\n";
            textcontrol.Text += "		, LPY = " + textBoxYD.Text + " NOP \"Width\"" + "\r\n";
            textcontrol.Text += "		, LPZ = " + textBoxZD.Text + " NOP \"Thickness\"" + "\r\n";
            textcontrol.Text += "		 ) EOS " + "\r\n";
            textcontrol.Text += "" + "\r\n\r\n\r\n";

            int i;

            double tx = 0, ty = 0;

            for (i = 1; i < countV + 1; i++)
            {





                if (Sv[i].vid == 1)
                {
                    if (Sv[i].OO == "0") { tx = Sv[i].x0; ty = Sv[i].y0; }
                    if (Sv[i].OO == "1") { tx = Sv[i].x0; ty = YD - Sv[i].y0; }
                    if (Sv[i].OO == "2") { tx = XD - Sv[i].x0; ty = YD - Sv[i].y0; }
                    if (Sv[i].OO == "3") { tx = XD - Sv[i].x0; ty = Sv[i].y0; }

                    textcontrol.Text += "[ 0." + i + " ] PATH    \"\" \"Drill\\1_Single.pm4\"  (QX = " + Convert.ToString(tx);
                    textcontrol.Text += " \"Quota X\",QY = " + Convert.ToString(ty) + " \"Quota Y\",DIA = " + Sv[i].r * 2;
                    textcontrol.Text += " \"Diameter\",PRF = " + Sv[i].g;
                    textcontrol.Text += " \"Depth\")" + "\r\n";
                }

                if (Sv[i].vid == 0)
                {
                    //   ty = Convert.ToDouble(textBoxZD.Text) / 2;
                    ty = Sv[i].z0;
                    if (((Sv[i].OO == "0") && (Sv[i].y0 == 0)) || ((Sv[i].OO == "1") && (Sv[i].y0 == YD))) { tx = Sv[i].x0; }
                    if (((Sv[i].OO == "3") && (Sv[i].y0 == 0)) || ((Sv[i].OO == "2") && (Sv[i].y0 == YD))) { tx = XD - Sv[i].x0; }
                    if (((Sv[i].OO == "0") && (Sv[i].x0 == 0)) || ((Sv[i].OO == "3") && (Sv[i].x0 == XD))) { tx = Sv[i].y0; }
                    if (((Sv[i].OO == "1") && (Sv[i].x0 == 0)) || ((Sv[i].OO == "2") && (Sv[i].x0 == XD))) { tx = YD - Sv[i].y0; }

                    textcontrol.Text += "[ 0." + i + " ] PATH    \"\" \"Drill\\1_Single.pm4\"  (QX = " + Convert.ToString(tx);
                    textcontrol.Text += " \"Quota X\",QY = " + Convert.ToString(ty) + " \"Quota Y\",DIA = " + Sv[i].r * 2;
                    textcontrol.Text += " \"Diameter\",PRF = " + Sv[i].g;
                    textcontrol.Text += " \"Depth\")" + "\r\n";
                }
            }



            textcontrol.Text += "\r\n";
            textcontrol.Text += "----------   LAVORAZIONE BASE DEL PROGETTO   ------------------------------------------------" + "\r\n";
            textcontrol.Text += "" + "\r\n\r\n\r\n";


            textcontrol.Text += "[  1.0 ] LAYER ON START 1 \"WRK\" COLORE 3" + "\r\n";
            textcontrol.Text += "[  1.1 ] LAYER  EOS" + "\r\n";
            textcontrol.Text += "[  2.0 ] LAYER ON START 2 \"DOC\" COLORE 2" + "\r\n";
            textcontrol.Text += "[  2.1 ] LAYER  EOS" + "\r\n";


            textcontrol.Text += "\r\n";


            textcontrol.Text += "[  0.0 ] PUNTO [ 0 , 0 , LPZ ]" + "\r\n";
            textcontrol.Text += "[  0.1 ] CREA_SPACE " + textBoxXD.Text + " " + textBoxYD.Text + " " + textBoxZD.Text + "\r\n";


            for (i = 1; i < countV + 1; i++)
            {


                if ((Sv[i].vid == 0)) //торцевые
                {
                    if ((Sv[i].OO == "3") && (Sv[i].x0 == XD)) //снизу права
                    {
                        textcontrol.Text += "[ " + (i * 3 - 2) + ".0 ] CREA_LAB DESTRA" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 1) + ".0 ] SEGMENTO MATITA PER [LPX,0.0,0.0] INCLINAZIONE ASSE_X ANGOLO 0 MISURA 1" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 0) + ".0 ] INSERISCI FORI  #" + i;
                        textcontrol.Text += "\r\n";
                    }

                    if ((Sv[i].OO == "2") && (Sv[i].x0 == XD)) //сверху справа
                    {
                        textcontrol.Text += "[ " + (i * 3 - 2) + ".0 ] CREA_LAB DESTRA" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 1) + ".0 ] SEGMENTO MATITA PER [LPX,LPY,0.0] INCLINAZIONE ASSE_X ANGOLO 0 MISURA 1" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 0) + ".0 ] INSERISCI FORI SIMMETRICO ASSE_Y PIANO #" + i;
                        textcontrol.Text += "\r\n";
                    }

                    if ((Sv[i].OO == "0") && (Sv[i].x0 == 0)) //снизу слева
                    {
                        textcontrol.Text += "[ " + (i * 3 - 2) + ".0 ] CREA_LAB SINISTRA" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 1) + ".0 ] SEGMENTO MATITA PER [0.0,0.0,0.0] INCLINAZIONE ASSE_X ANGOLO 0 MISURA 1" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 0) + ".0 ] INSERISCI FORI SIMMETRICO ASSE_Y PIANO #" + i;
                        textcontrol.Text += "\r\n";
                    }

                    if ((Sv[i].OO == "1") && (Sv[i].x0 == 0)) //сверху слева
                    {
                        textcontrol.Text += "[ " + (i * 3 - 2) + ".0 ] CREA_LAB SINISTRA" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 1) + ".0 ] SEGMENTO MATITA PER [0.0,LPY,0.0] INCLINAZIONE ASSE_X ANGOLO 0 MISURA 1" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 0) + ".0 ] INSERISCI FORI  #" + i;
                        textcontrol.Text += "\r\n";
                    }

                    if ((Sv[i].OO == "0") && (Sv[i].y0 == 0)) //слева снизу
                    {
                        textcontrol.Text += "[ " + (i * 3 - 2) + ".0 ] CREA_LAB SOTTO" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 1) + ".0 ] SEGMENTO MATITA PER [0.0,0.0,0.0] INCLINAZIONE ASSE_X ANGOLO 0 MISURA 1" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 0) + ".0 ] INSERISCI FORI  #" + i;
                        textcontrol.Text += "\r\n";
                    }

                    if ((Sv[i].OO == "3") && (Sv[i].y0 == 0)) //справа снизу
                    {
                        textcontrol.Text += "[ " + (i * 3 - 2) + ".0 ] CREA_LAB SOTTO" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 1) + ".0 ] SEGMENTO MATITA PER [LPX,0.0,0.0] INCLINAZIONE ASSE_X ANGOLO 0 MISURA 1" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 0) + ".0 ] INSERISCI FORI SIMMETRICO ASSE_Y PIANO #" + i;
                        textcontrol.Text += "\r\n";
                    }

                    if ((Sv[i].OO == "1") && (Sv[i].y0 == YD)) //слева сверху
                    {
                        textcontrol.Text += "[ " + (i * 3 - 2) + ".0 ] CREA_LAB SOPRA" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 1) + ".0 ] SEGMENTO MATITA PER [0.0,LPY,0.0] INCLINAZIONE ASSE_X ANGOLO 0 MISURA 1" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 0) + ".0 ] INSERISCI FORI SIMMETRICO ASSE_Y PIANO #" + i;
                        textcontrol.Text += "\r\n";
                    }

                    if ((Sv[i].OO == "2") && (Sv[i].y0 == YD)) //справа сверху
                    {
                        textcontrol.Text += "[ " + (i * 3 - 2) + ".0 ] CREA_LAB SOPRA" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 1) + ".0 ] SEGMENTO MATITA PER [LPX,LPY,0.0] INCLINAZIONE ASSE_X ANGOLO 0 MISURA 1" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 0) + ".0 ] INSERISCI FORI  #" + i;
                        textcontrol.Text += "\r\n";
                    }

                }


                if ((Sv[i].vid == 1)) //вертикальные
                {
                    textcontrol.Text += "[ " + (i * 3 - 2) + ".0 ] CREA_LAB FRONTALE" + "\r\n";
                    if (Sv[i].OO == "0") //слева снизу
                    {
                        textcontrol.Text += "[ " + (i * 3 - 1) + ".0 ] SEGMENTO MATITA PER [0.0,0.0,LPZ] INCLINAZIONE ASSE_X ANGOLO 0 MISURA 1" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 0) + ".0 ] INSERISCI FORI  #" + i;
                        textcontrol.Text += "\r\n";
                    }

                    if (Sv[i].OO == "1") //слева сверху
                    {
                        textcontrol.Text += "[ " + (i * 3 - 1) + ".0 ] SEGMENTO MATITA PER [0.0,LPY,LPZ] INCLINAZIONE ASSE_X ANGOLO 0 MISURA 1" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 0) + ".0 ] INSERISCI FORI SIMMETRICO ASSE_X PIANO #" + i;
                        textcontrol.Text += "\r\n";
                    }

                    if (Sv[i].OO == "2") //справа сверху
                    {
                        textcontrol.Text += "[ " + (i * 3 - 1) + ".0 ] SEGMENTO MATITA PER [LPX,LPY,LPZ] INCLINAZIONE ASSE_X ANGOLO 0 MISURA 1" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 0) + ".0 ] INSERISCI FORI SIMMETRICO ASSE_X ASSE_Y PIANO #" + i;
                        textcontrol.Text += "\r\n";
                    }

                    if (Sv[i].OO == "3") //справа снизу
                    {
                        textcontrol.Text += "[ " + (i * 3 - 1) + ".0 ] SEGMENTO MATITA PER [LPX,0.0,LPZ] INCLINAZIONE ASSE_X ANGOLO 0 MISURA 1" + "\r\n";
                        textcontrol.Text += "[ " + (i * 3 - 0) + ".0 ] INSERISCI FORI SIMMETRICO ASSE_Y PIANO #" + i;
                        textcontrol.Text += "\r\n";
                    }

                }


            }


            textcontrol.Text += "\r\n\r\n\r\n";
            textcontrol.Text += "[  999.  ] FINE_MACRO " + "\r\n";
            textcontrol.Text += "\r\n";

            int TYPE;

            for (i = 1; i < countV + 1; i++)
            {
                TYPE = 2;//проходное сверло

                tx = Sv[i].x0;
                ty = Sv[i].y0;



                if (Sv[i].vid == 1) //вертикальные горизонтальные по умолчанию глухие 
                {
                    if (Sv[i].g < Convert.ToDouble(textBoxZD.Text)) { TYPE = 1; } // глухое сверло
                    if (Sv[i].r > 5) { TYPE = 3; } // тип фреза если диаметр сверла больше 10



                    textcontrol.Text += "&&& [" + i + ",1]";
                    textcontrol.Text += " #NUM_CAT=1,#ID_TOOLNAME=*,#PRIORITY=*,#TYPEDRILL=*,#DWNSPEED=*,#RIFDRILL=*,#TYPE=" + TYPE;
                    textcontrol.Text += ",#AZ=270.000000,#AR=0.000000,#WORKSIDE=0,#DIAM=" + (Sv[i].r * 2) + ",#ID_DEPTH=";
                    textcontrol.Text += Sv[i].g + "\r\n";
                }
            }


            textcontrol.Text += "\r\n";
            textcontrol.Text += "\r\n";
            textcontrol.Text += "\r\n";



            textcontrol.Text += "END PARAMETRIC SECTION\r\n";


            if (dnd)
            {
                outFileHIRZT = ListFileDrop[FileDroplistBox.SelectedIndex];

                outFileHIRZT = outFileHIRZT.Replace(".mpr", "");

                if (!outFileHIRZT.ToLower().EndsWith(".pd4")) { outFileHIRZT += ".pd4"; }




                System.IO.StreamWriter sw = new
                           System.IO.StreamWriter(outFileHIRZT);

                sw.WriteLine(textcontrol.Text);
                sw.Close();
                WWF = true;

            }

            if ((FileName.Text != "") && ((textBoxXD.Text != "") || (textBoxYD.Text != "")) && !WWF)//здесь обрабатываеться наш файл если файл DXF не был открыт работает наполовину)))
            {
                saveFileDialog1.Filter = "|*.pd4";
                saveFileDialog1.FileName = FileName.Text;



                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {


                    outFileHIRZT = saveFileDialog1.FileName;



                    if (!outFileHIRZT.ToLower().EndsWith(".pd4")) { outFileHIRZT += ".pd4"; }




                    System.IO.StreamWriter sw = new
                               System.IO.StreamWriter(outFileHIRZT);

                    sw.WriteLine(textcontrol.Text);
                    sw.Close();
                }
            }




        }



        private void MirrorX_Click(object sender, EventArgs e) // отразить ??
        {
            int i;
            string o = "";
            for (i = 1; i < countV + 1; i++)
            {

                Sv[i].x0 = XD - Sv[i].x0;

                if (Sv[i].OO == "0") { o = "3"; }
                if (Sv[i].OO == "1") { o = "2"; }
                if (Sv[i].OO == "2") { o = "1"; }
                if (Sv[i].OO == "3") { o = "0"; }

                Sv[i].OO = o;
            }
            infosv();
            workdraw();
            drawsv();
            smena();
        }

        private void MirrorY_Click(object sender, EventArgs e)// отразить ??
        {
            int i;
            string o = "";
            for (i = 1; i < countV + 1; i++)
            {

                Sv[i].y0 = YD - Sv[i].y0;

                if (Sv[i].OO == "0") { o = "1"; }
                if (Sv[i].OO == "1") { o = "0"; }
                if (Sv[i].OO == "2") { o = "3"; }
                if (Sv[i].OO == "3") { o = "2"; }

                Sv[i].OO = o;

            }

            infosv();
            workdraw();
            drawsv();
            smena();
        }

        private void SvPrev_Click(object sender, EventArgs e) //двигаемся по списку свёрел назад
        {

            int i;
            if (countV != 0)
            {
                if (label1.Text != "")
                {

                    i = Convert.ToInt32(label1.Text);

                    Sv[i].edit = 0;
                    i--;
                    if (i == 0) { i = countV; }
                    Sv[i].edit = 1;
                    label1.Text = Convert.ToString(i);
                    deleteSv.Enabled = true;
                    smena();

                    if (Sv[i].vid == 1) { label5.Visible = textBoxZ.Visible = false; }
                    else { label5.Visible = textBoxZ.Visible = true; }
                }
                sortSv();
                infosv();
            }
        }

        private void SvNext_Click(object sender, EventArgs e) //двигаемся по списку свёрел вперёд
        {
            int i;
            if (countV != 0)
            {
                if (label1.Text != "")
                {
                    i = Convert.ToInt32(label1.Text);
                    Sv[i].edit = 0;
                    i++;
                    if (i > countV) { i = 1; }
                    Sv[i].edit = 1;
                    label1.Text = Convert.ToString(i);
                    deleteSv.Enabled = true;
                    smena();
                    if (Sv[i].vid == 1) { label5.Visible = textBoxZ.Visible = false; }
                    else { label5.Visible = textBoxZ.Visible = true; }

                }
                /*    if (label1.Text == "")
                    {
                        label1.Text = "1";
                        Sv[1].edit = 1;
                        deleteSv.Enabled = true;
                        smena();
                    }*/
                sortSv();
                infosv();
            }
        }

        private void deleteSv_Click(object sender, EventArgs e) //удалить сверло
        {
            int i;
            ot tempSv;
            if (countV != 0)
            {
                if (label1.Text != "")
                {
                    i = Convert.ToInt32(label1.Text);


                    tempSv = Sv[countV];
                    Sv[countV] = Sv[i];
                    Sv[i] = tempSv;
                    countV--;
                    i--;
                    if (i != 0)
                    {
                        label1.Text = Convert.ToString(i);

                    }
                    if (i == 0)
                    {
                        label1.Text = "";
                        deleteSv.Enabled = false;
                    }

                    if ((i == 0) && (countV != 0))
                    {
                        i = countV;
                        label1.Text = Convert.ToString(i);
                        deleteSv.Enabled = true;

                    }

                    workdraw();
                    drawsv();
                    smena();
                }
            }

        }

        private void AddSv_Click(object sender, EventArgs e) //добавить сверло
        {
            int i;


            for (i = 1; i < countV + 1; i++) { Sv[i].edit = 0; }// добавляем всё в массив сортировки

            countV++;

            label1.Text = Convert.ToString(countV);//указатель на последнее сверло
            i = Convert.ToInt32(label1.Text);
            deleteSv.Enabled = true;

            Sv[i].x0 = 50;
            Sv[i].y0 = 50;
            Sv[i].r = 4;
            Sv[i].g = 19;
            Sv[i].OO = "0";
            Sv[i].vid = 1;
            Sv[i].edit = 1; // исключаем последнее сверло из сортировки


            workdraw();
            drawsv();
            smena();
           // drawsv();


            textBoxX.Focus();

        }

        private void AddDim_Click(object sender, EventArgs e) //добавить деталь
        {
            woodwopopen = false;
            ZD = Convert.ToDouble(textBoxZD.Text);
            textBoxZ.Text = (ZD / 2).ToString();
            textBoxXD.Text = textBoxYD.Text = "400";




            commentBox.Text = "";
            textcontrol.Text = "";
            label1.Text = "";
            workdraw();
            AddSv.Enabled = true;
            textBoxXD.Focus();
            countV = 0;
            smena();


        }

        private void GUpDown_ValueChanged(object sender, EventArgs e)// регулируем глубину сверления
        {
            string tempG;
            double tempGG;

            if ((textBoxG.Text != "") && (label1.Text != ""))
            {
                tempG = textBoxG.Text;
                tempGG = Convert.ToDouble(tempG) + Convert.ToDouble(GUpDown.Value);
                textBoxG.Text = Convert.ToString(tempGG);
                GUpDown.Value = 0;
            }
        }



        // дублируем кнопки точек отсчёта сверления


        private void button11_Click(object sender, EventArgs e)
        {
            Sv[Convert.ToInt32(label1.Text)].OO = "0";
            smena();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Sv[Convert.ToInt32(label1.Text)].OO = "1";
            smena();
        }

        private void button33_Click(object sender, EventArgs e)
        {
            Sv[Convert.ToInt32(label1.Text)].OO = "2";
            smena();
        }

        private void help_button_Click(object sender, EventArgs e)
        {
            if (helptext.Visible == false) { helptext.Visible = true; return; }
            if (helptext.Visible == true) { helptext.Visible = false; return; }
        }

        private void button44_Click(object sender, EventArgs e)
        {
            Sv[Convert.ToInt32(label1.Text)].OO = "3";
            smena();
        }

        private void FileName_Enter(object sender, EventArgs e)
        {
            woodwopopen = true;
        }










        private void OPNCAD4_Click(object sender, EventArgs e)
        {

            HIRZT[] HFILE = new HIRZT[300];
            for (int I = 0; I < 300; I++) { HFILE[I].vid = -1; HFILE[I].OO = null; }
            textWW.Visible = false;
            textcontrol.Visible = false;
            openFileWW.Filter = "|*.pd4";
            commentBox.Text = "";
            bool openfile = false;
            string line = "", line2 = "";
            string CREA_LAB = "";
            textcontrol.Text = textWW.Text = "";


            string nameFILE = "";

            if (!WWF) { if (openFileWW.ShowDialog() == System.Windows.Forms.DialogResult.OK) { openfile = true; nameFILE = openFileWW.FileName; } }

            if (WWF) { openfile = true; nameFILE = FileName.Text; }
            WWF = false;

            if (openfile)
            {

                using (var sr = new StreamReader(nameFILE, encoding: Encoding.GetEncoding(1251)))
                {

                    woodwopopen = true;


                    nameFILE = nameFILE.Remove(nameFILE.LastIndexOf("."));
                    nameFILE = nameFILE.Remove(0, nameFILE.LastIndexOf("\\") + 1);
                    FileName.Text = nameFILE;



                    countV = countT = 0;

                    int I = 1, COUNT1 = 0, COUNT2 = 0;


                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();



                        if (line.IndexOf("LX=") >= 0)
                        {
                            XD = Convert.ToDouble(Regex.Match(line, @"LX= [0-9]+").Value.Replace("LX= ", ""));
                            // textBoxXD.Text = Convert.ToString(XD);

                        }

                        if (line.IndexOf("LY=") >= 0)
                        {
                            YD = Convert.ToDouble(Regex.Match(line, @"LY= [0-9]+").Value.Replace("LY= ", ""));
                            //textBoxYD.Text = Convert.ToString(YD);
                        }

                        if (line.IndexOf("LZ=") >= 0)
                        {
                            ZD = Convert.ToDouble(Regex.Match(line, @"LZ= [0-9]+").Value.Replace("LZ= ", ""));
                            //textBoxZD.Text = Convert.ToString(ZD);
                        }


                        if (line.IndexOf("Drill") >= 0) //
                        {




                            I = Convert.ToInt32(Regex.Match(line, @"\.[0-9]+").Value.Replace(".", ""));

                            HFILE[I].DRILL = Regex.Match(line, @"Drill\\[0-9]").Value.Replace("Drill\\", "");



                            HFILE[I].QX = Convert.ToDouble(Regex.Match(line, @"QX = [0-9,.]+").Value.Replace("QX = ", ""));
                            HFILE[I].QY = Convert.ToDouble(Regex.Match(line, @"QY = [0-9,.]+").Value.Replace("QY = ", ""));

                            HFILE[I].OFS = -1;
                            if (line.IndexOf("OFS") >= 0)
                            {
                                HFILE[I].OFS = Convert.ToDouble(Regex.Match(line, @"OFS = [0-9,.]+").Value.Replace("OFS = ", ""));

                            }
                            HFILE[I].N = -1;
                            if (line.IndexOf("N = ") >= 0)
                            {
                                HFILE[I].N = Convert.ToDouble(Regex.Match(line, @"N = [0-9]+").Value.Replace("N = ", ""));

                            }
                            HFILE[I].DIA = Convert.ToDouble(Regex.Match(line, @"DIA = [0-9,.]+").Value.Replace("DIA = ", ""));

                            HFILE[I].PRF = Convert.ToDouble(Regex.Match(line, @"PRF = [0-9,.]+").Value.Replace("PRF = ", ""));

                            HFILE[I].OO = "+"; //есть данные по координатам сверла





                        }
                        int TRIM = 0;
                        if (line.IndexOf("TRIM ELIMINA #") >= 0)
                        {
                            TRIM = Convert.ToInt32(Regex.Match(line, @"TRIM ELIMINA #[0-9]+").Value.Replace("TRIM ELIMINA #", ""));
                            HFILE[TRIM].vid = -1;
                        }


                        if (line.IndexOf("FRONTALE") >= 0) { CREA_LAB = "FRONT"; }
                        if (line.IndexOf("DESTRA") >= 0) { CREA_LAB = "RIGHT"; }
                        if (line.IndexOf("SINISTRA") >= 0) { CREA_LAB = "LEFT"; }
                        if (line.IndexOf("SOPRA") >= 0) { CREA_LAB = "UP"; }
                        if (line.IndexOf("SOTTO") >= 0) { CREA_LAB = "DOWN"; }

                        if ((CREA_LAB != "") && (line.IndexOf("SEGMENTO MATITA") >= 0))
                        {
                            if (line.IndexOf("CREA_LAB") >= 0) { line = sr.ReadLine(); }// если создаётся плоскость читаем следущую строку
                            line2 = sr.ReadLine();

                            if (line2 != null)
                            {
                                if (line2.IndexOf("INSERISCI") >= 0)
                                {
                                    COUNT1 = Convert.ToInt32(line2.Split('#')[1]); //номер сверла в хирц файле



                                    if ((CREA_LAB == "FRONT") && (HFILE[COUNT1].OO == "+"))  //вертикальные
                                    {
                                        HFILE[COUNT1].vid = 1;
                                        HFILE[COUNT1].PL = CREA_LAB;

                                        if (line.IndexOf("0.0,0.0,LPZ") >= 0) { HFILE[COUNT1].OO = "0"; COUNT2++; }
                                        if (line.IndexOf("0.0,LPY,LPZ") >= 0) { HFILE[COUNT1].OO = "1"; COUNT2++; }
                                        if (line.IndexOf("LPX,LPY,LPZ") >= 0) { HFILE[COUNT1].OO = "2"; COUNT2++; }
                                        if (line.IndexOf("LPX,0.0,LPZ") >= 0) { HFILE[COUNT1].OO = "3"; COUNT2++; }

                                        textcontrol.Text += CREA_LAB + line2;
                                        textcontrol.Text += "\r\n";
                                    }

                                    if ((CREA_LAB == "LEFT") && (HFILE[COUNT1].OO == "+"))  //торцевые слева
                                    {
                                        HFILE[COUNT1].vid = 0;
                                        HFILE[COUNT1].PL = CREA_LAB;

                                        if (line.IndexOf("0.0,0.0,LPZ") >= 0) { HFILE[COUNT1].OO = "0"; COUNT2++; }
                                        if (line.IndexOf("0.0,0.0,0.0") >= 0) { HFILE[COUNT1].OO = "0"; COUNT2++; }
                                        if (line.IndexOf("0.0,LPY,LPZ") >= 0) { HFILE[COUNT1].OO = "1"; COUNT2++; }
                                        if (line.IndexOf("0.0,LPY,0.0") >= 0) { HFILE[COUNT1].OO = "1"; COUNT2++; }


                                        textcontrol.Text += CREA_LAB + line2;
                                        textcontrol.Text += "\r\n";


                                    }

                                    if ((CREA_LAB == "RIGHT") && (HFILE[COUNT1].OO == "+"))  //торцевые справа
                                    {
                                        HFILE[COUNT1].vid = 0;
                                        HFILE[COUNT1].PL = CREA_LAB;

                                        if (line.IndexOf("LPX,0.0,LPZ") >= 0) { HFILE[COUNT1].OO = "3"; COUNT2++; }
                                        if (line.IndexOf("LPX,0.0,0.0") >= 0) { HFILE[COUNT1].OO = "3"; COUNT2++; }
                                        if (line.IndexOf("LPX,LPY,LPZ") >= 0) { HFILE[COUNT1].OO = "2"; COUNT2++; }
                                        if (line.IndexOf("LPX,LPY,0.0") >= 0) { HFILE[COUNT1].OO = "2"; COUNT2++; }


                                        textcontrol.Text += CREA_LAB + line2;
                                        textcontrol.Text += "\r\n";


                                    }

                                    if ((CREA_LAB == "UP") && (HFILE[COUNT1].OO == "+"))  //торцевые сверху
                                    {
                                        HFILE[COUNT1].vid = 0;
                                        HFILE[COUNT1].PL = CREA_LAB;

                                        if (line.IndexOf("0.0,LPY,LPZ") >= 0) { HFILE[COUNT1].OO = "1"; COUNT2++; }
                                        if (line.IndexOf("0.0,LPY,0.0") >= 0) { HFILE[COUNT1].OO = "1"; COUNT2++; }
                                        if (line.IndexOf("LPX,LPY,LPZ") >= 0) { HFILE[COUNT1].OO = "2"; COUNT2++; }
                                        if (line.IndexOf("LPX,LPY,0.0") >= 0) { HFILE[COUNT1].OO = "2"; COUNT2++; }


                                        textcontrol.Text += CREA_LAB + line2;
                                        textcontrol.Text += "\r\n";


                                    }

                                    if ((CREA_LAB == "DOWN") && (HFILE[COUNT1].OO == "+"))  //торцевые снизу
                                    {
                                        HFILE[COUNT1].vid = 0;
                                        HFILE[COUNT1].PL = CREA_LAB;

                                        if (line.IndexOf("0.0,0.0,LPZ") >= 0) { HFILE[COUNT1].OO = "0"; COUNT2++; }
                                        if (line.IndexOf("0.0,0.0,0.0") >= 0) { HFILE[COUNT1].OO = "0"; COUNT2++; }
                                        if (line.IndexOf("LPX,0.0,LPZ") >= 0) { HFILE[COUNT1].OO = "3"; COUNT2++; }
                                        if (line.IndexOf("LPX,0.0,0.0") >= 0) { HFILE[COUNT1].OO = "3"; COUNT2++; }


                                        textcontrol.Text += CREA_LAB + line2;
                                        textcontrol.Text += "\r\n";


                                    }
                                }
                            }





                            if (line2.IndexOf("FINE") >= 0) { sr.Close(); break; }

                        }





                        if (line.IndexOf("FINE") >= 0) { sr.Close(); break; }

                    }


                    COUNT1 = 1;


                    int sverlo;
                    for (I = 0; I < 300; I++)
                    {


                        if (HFILE[I].vid == 1)
                        {
                            Sv[COUNT1].x0 = HFILE[I].QX;
                            Sv[COUNT1].y0 = HFILE[I].QY;
                            Sv[COUNT1].z0 = 0;
                            Sv[COUNT1].g = HFILE[I].PRF;
                            Sv[COUNT1].r = HFILE[I].DIA / 2;
                            Sv[COUNT1].edit = 0;
                            Sv[COUNT1].OO = HFILE[I].OO;
                            Sv[COUNT1].vid = 1;



                            if (Sv[COUNT1].OO == "1") { Sv[COUNT1].y0 = YD - Sv[COUNT1].y0; }
                            else if (Sv[COUNT1].OO == "2") { Sv[COUNT1].x0 = XD - Sv[COUNT1].x0; Sv[COUNT1].y0 = YD - Sv[COUNT1].y0; }
                            else if (Sv[COUNT1].OO == "3") { Sv[COUNT1].x0 = XD - Sv[COUNT1].x0; }

                            COUNT1++;

                            if (HFILE[I].DRILL != "1")
                            {

                                for (sverlo = 1; sverlo < Convert.ToInt32(HFILE[I].N); sverlo++)
                                {
                                    Sv[COUNT1].g = HFILE[I].PRF;
                                    Sv[COUNT1].r = HFILE[I].DIA / 2;
                                    Sv[COUNT1].edit = 0;
                                    Sv[COUNT1].OO = HFILE[I].OO;
                                    Sv[COUNT1].vid = 1;

                                    if (HFILE[I].DRILL != "2")
                                    {
                                        Sv[COUNT1].x0 = HFILE[I].QX;
                                        Sv[COUNT1].y0 = HFILE[I].QY + HFILE[I].OFS * sverlo;

                                    }

                                    if (HFILE[I].DRILL != "3")
                                    {
                                        Sv[COUNT1].x0 = HFILE[I].QX + HFILE[I].OFS * sverlo;
                                        Sv[COUNT1].y0 = HFILE[I].QY;

                                    }

                                    if (Sv[COUNT1].OO == "1") { Sv[COUNT1].y0 = YD - Sv[COUNT1].y0; }
                                    else if (Sv[COUNT1].OO == "2") { Sv[COUNT1].x0 = XD - Sv[COUNT1].x0; Sv[COUNT1].y0 = YD - Sv[COUNT1].y0; }
                                    else if (Sv[COUNT1].OO == "3") { Sv[COUNT1].x0 = XD - Sv[COUNT1].x0; }

                                    COUNT1++;



                                }

                            }




                        }

                        if (((HFILE[I].vid == 0) && (HFILE[I].PL == "LEFT")) || ((HFILE[I].vid == 0) && (HFILE[I].PL == "RIGHT")))
                        {
                            Sv[COUNT1].x0 = 0;
                            Sv[COUNT1].y0 = HFILE[I].QX;
                            Sv[COUNT1].z0 = HFILE[I].QY;
                            Sv[COUNT1].g = HFILE[I].PRF;
                            Sv[COUNT1].r = HFILE[I].DIA / 2;
                            Sv[COUNT1].edit = 0;
                            Sv[COUNT1].OO = HFILE[I].OO;
                            Sv[COUNT1].vid = 0;



                            if (Sv[COUNT1].OO == "1") { Sv[COUNT1].y0 = YD - Sv[COUNT1].y0; }
                            else if (Sv[COUNT1].OO == "2") { Sv[COUNT1].x0 = XD - Sv[COUNT1].x0; Sv[COUNT1].y0 = YD - Sv[COUNT1].y0; }
                            else if (Sv[COUNT1].OO == "3") { Sv[COUNT1].x0 = XD - Sv[COUNT1].x0; }


                            COUNT1++;

                            if (HFILE[I].DRILL != "1")
                            {

                                for (sverlo = 1; sverlo < Convert.ToInt32(HFILE[I].N); sverlo++)
                                {
                                    Sv[COUNT1].g = HFILE[I].PRF;
                                    Sv[COUNT1].r = HFILE[I].DIA / 2;
                                    Sv[COUNT1].edit = 0;
                                    Sv[COUNT1].OO = HFILE[I].OO;
                                    Sv[COUNT1].vid = 0;
                                    Sv[COUNT1].x0 = 0;
                                    Sv[COUNT1].y0 = HFILE[I].QX + HFILE[I].OFS * sverlo;


                                    if (Sv[COUNT1].OO == "1") { Sv[COUNT1].y0 = YD - Sv[COUNT1].y0; }
                                    else if (Sv[COUNT1].OO == "2") { Sv[COUNT1].x0 = XD - Sv[COUNT1].x0; Sv[COUNT1].y0 = YD - Sv[COUNT1].y0; }
                                    else if (Sv[COUNT1].OO == "3") { Sv[COUNT1].x0 = XD - Sv[COUNT1].x0; }



                                    COUNT1++;

                                }
                            }




                        }


                        if (((HFILE[I].vid == 0) && (HFILE[I].PL == "UP")) || ((HFILE[I].vid == 0) && (HFILE[I].PL == "DOWN")))
                        {
                            Sv[COUNT1].x0 = HFILE[I].QX;
                            Sv[COUNT1].y0 = 0;
                            Sv[COUNT1].z0 = HFILE[I].QY;
                            Sv[COUNT1].g = HFILE[I].PRF;
                            Sv[COUNT1].r = HFILE[I].DIA / 2;
                            Sv[COUNT1].edit = 0;
                            Sv[COUNT1].OO = HFILE[I].OO;
                            Sv[COUNT1].vid = 0;



                            if (Sv[COUNT1].OO == "1") { Sv[COUNT1].y0 = YD - Sv[COUNT1].y0; }
                            else if (Sv[COUNT1].OO == "2") { Sv[COUNT1].x0 = XD - Sv[COUNT1].x0; Sv[COUNT1].y0 = YD - Sv[COUNT1].y0; }
                            else if (Sv[COUNT1].OO == "3") { Sv[COUNT1].x0 = XD - Sv[COUNT1].x0; }


                            COUNT1++;

                            if (HFILE[I].DRILL != "1")
                            {

                                for (sverlo = 1; sverlo < Convert.ToInt32(HFILE[I].N); sverlo++)
                                {
                                    Sv[COUNT1].g = HFILE[I].PRF;
                                    Sv[COUNT1].r = HFILE[I].DIA / 2;
                                    Sv[COUNT1].edit = 0;
                                    Sv[COUNT1].OO = HFILE[I].OO;
                                    Sv[COUNT1].vid = 0;
                                    Sv[COUNT1].x0 = HFILE[I].QX + HFILE[I].OFS * sverlo;
                                    Sv[COUNT1].y0 = 0;


                                    if (Sv[COUNT1].OO == "1") { Sv[COUNT1].y0 = YD - Sv[COUNT1].y0; }
                                    else if (Sv[COUNT1].OO == "2") { Sv[COUNT1].x0 = XD - Sv[COUNT1].x0; Sv[COUNT1].y0 = YD - Sv[COUNT1].y0; }
                                    else if (Sv[COUNT1].OO == "3") { Sv[COUNT1].x0 = XD - Sv[COUNT1].x0; }



                                    COUNT1++;

                                }
                            }




                        }


                    }




                    textWW.Visible = false;
                    textcontrol.Visible = false;
                    countV = COUNT1 - 1;

                    label1.Text = "1";
                    workdraw();


                    drawsv();

                    if (countV > 0) // ставим указатель на 1 сверло если есть
                    {
                        sortSv();
                        label1.Text = "1";
                        Sv[1].edit = 1;

                        smena();
                        infosv();
                        deleteSv.Enabled = true;
                    }


                }



            }



        }

        private void label1_Leave(object sender, EventArgs e)
        {
            Sv[0].viz.BackColor = Color.Gray;
            label1.ForeColor = Color.Red;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        public string[] ListFileDrop = new string[50];// имена файлов Drug&Drop
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            FileDroplistBox.Visible = true;
            int i = FileDroplistBox.Items.Count;
            foreach (string file in files)
            {
                if ((file.ToLower().EndsWith(".pd4")) || (file.ToLower().EndsWith(".mpr")) || (file.ToLower().EndsWith(".dxf")))
                {
                    ListFileDrop[i] = file;

                    FileDroplistBox.Items.Add(file.Remove(0, file.LastIndexOf("\\") + 1));


                    i++;
                }
            }

            WWF = false;
            FileDroplistBox.SelectedIndex = 0;
        }

        private void FileDroplistBox_DoubleClick(object sender, EventArgs e)
        {
            string FileSelected = ListFileDrop[FileDroplistBox.SelectedIndex];
            // MessageBox.Show(FileSelected);



            if (FileSelected.ToLower().EndsWith(".pd4") == true)
            {
                FileName.Text = FileSelected;
                WWF = true;
                OPNCAD4.PerformClick();

            }

            if (FileSelected.ToLower().EndsWith(".mpr") == true)
            {
                FileName.Text = FileSelected;
                WWF = true;
                OWW.PerformClick();

            }


            if (FileSelected.ToLower().EndsWith(".dxf") == true)
            {
                FileName.Text = FileSelected;
                WWF = true;
                OPN.PerformClick();

            }

            WWF = false;

        }

        private void FileDroplistBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void PD4toMPR_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < FileDroplistBox.Items.Count; i++)
            {
                FileDroplistBox.SetSelected(i, true);
                string FileSelected = ListFileDrop[FileDroplistBox.SelectedIndex];
                // MessageBox.Show(FileSelected);

                WWF = false;

                if (FileSelected.ToLower().EndsWith(".pd4") == true)
                {
                    FileName.Text = FileSelected;
                    WWF = true; dnd = true;
                    OPNCAD4.PerformClick();
                    saveWW.PerformClick();

                }

                if (FileSelected.ToLower().EndsWith(".mpr") == true)
                {
                    FileName.Text = FileSelected;
                    WWF = true; dnd = true;
                    OWW.PerformClick();
                    saveCad4.PerformClick();

                }
            }

            WWF = false; dnd = false;
        }

        private void DropListClear_Click(object sender, EventArgs e)
        {
            FileDroplistBox.Items.Clear();
            Array.Clear(ListFileDrop, 0, 50);
        }

        string nameFILE;

        private void textBoxZ_TextChanged(object sender, EventArgs e) //Z смещение в торце
        {

            int i = 0;
            if (label1.Text != "")
            {
                i = Convert.ToInt32(label1.Text);
                if ((textBoxZ.Text != "") && (Sv[i].vid == 0)) //торцевые
                {
                    Sv[i].z0 = Convert.ToDouble(textBoxZ.Text);
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            workdraw();

            drawsv();

            if (countV > 0) // ставим указатель на 1 сверло если есть
            {

               // label1.Text = "1";
               // Sv[1].edit = 1;
                smena();
               // infosv();
                deleteSv.Enabled = true;
            }


        }

        private void saveBPP_Click(object sender, EventArgs e)
        {
            
                save_BIESSE(1);



        }


        private void save_BIESSE(int cf)
        {
            string BPPString = "";
            {
                BPPString += "[HEADER]" + "\r\n";
                BPPString += "TYPE = BPP" + "\r\n";
                BPPString += "VER = 150" + "\r\n";
                BPPString += "[DESCRIPTION]" + "\r\n";
                BPPString += "|" + "\r\n";
                BPPString += "[VARIABLES]" + "\r\n";
                BPPString += "PAN=LPX|" + textBoxXD.Text + "||4|" + "\r\n";
                BPPString += "PAN=LPY|" + textBoxYD.Text + "||4|" + "\r\n";
                BPPString += "PAN=LPZ|" + textBoxZD.Text + "||4|" + "\r\n";
                BPPString += "PAN=ORLST|\"1\"||3|" + "\r\n";
                BPPString += "PAN=SIMMETRY|0||1|" + "\r\n";
                BPPString += "PAN=TLCHK|0||1|" + "\r\n";
                BPPString += "PAN=TOOLING|\"\"||3|" + "\r\n";
                BPPString += "PAN=CUSTSTR|\"\"||3|" + "\r\n";
                BPPString += "PAN=FCN|1.000000||2|" + "\r\n";
                BPPString += "PAN=XCUT|0||4|" + "\r\n";
                BPPString += "PAN=YCUT|0||4|" + "\r\n";
                BPPString += "PAN=JIGTH|0||4|" + "\r\n";
                BPPString += "PAN=CKOP|0||1|" + "\r\n";
                BPPString += "PAN=UNIQUE|0||1|" + "\r\n";
                BPPString += "PAN=MATERIAL|\"wood\"||3|" + "\r\n";
                BPPString += "PAN=PUTLST|\"\"||3|" + "\r\n";
                BPPString += "PAN=OPPWKRS|0||1|" + "\r\n";
                BPPString += "PAN=UNICLAMP|0||1|" + "\r\n";
                BPPString += "PAN=CHKCOLL|0||1|" + "\r\n";
                BPPString += "PAN=WTPIANI|0||1|" + "\r\n";
                BPPString += "PAN=COLLTOOL|0||1|" + "\r\n";
                BPPString += "PAN=CALCEDTH|0||1|" + "\r\n";
                BPPString += "PAN=ENABLELABEL|0||1|" + "\r\n";
                BPPString += "PAN=LOCKWASTE|0||1|" + "\r\n";
                BPPString += "PAN=LOADEDGEOPT|0||1|" + "\r\n";
                BPPString += "PAN=ITLTYPE|0||1|" + "\r\n";
                BPPString += "PAN=RUNPAV|0||1|" + "\r\n";
                BPPString += "PAN=FLIPEND|0||1|" + "\r\n";
                BPPString += "PAN=ENABLEMACHLINKS|0||1|" + "\r\n";
                BPPString += "PAN=ENABLEPURSUITS|0||1|" + "\r\n";
                BPPString += "PAN=ENABLEFASTVERTBORINGS|0||1|" + "\r\n";
                BPPString += "PAN=FASTVERTBORINGSVALUE|0||4|" + "\r\n";
                BPPString += "" + "\r\n";
                BPPString += "[PROGRAM]" + "\r\n";
                BPPString += "" + "\r\n";
            }

            string tempLINE = "";
            for (int ln = 0; ln < commentBox.Lines.Length; ln++)
            {
                tempLINE = commentBox.Lines[ln];
                tempLINE = tempLINE.Replace("снизу", "к себе");
                tempLINE = tempLINE.Replace("сверху", "от себя");


                BPPString += "'" + tempLINE + "\r\n";


            }



            BPPString += "" + "\r\n";

            int i;
            string ID = "";
            double tx = 0, ty = 0, tz = 0;

            for (i = 1; i < countV + 1; i++)
            {

                ID = "P" + (1000 + i);
                int skv = 0;
                int tip = 0;

                if (Sv[i].vid == 1)
                {

                    if ((Sv[i].g < 5) && (Sv[i].r == 1.5)) { Sv[i].g = 2; Sv[i].r = 2.5; tip = 1; }


                    if (Sv[i].OO == "0")
                    {
                        tx = Sv[i].x0; ty = Sv[i].y0;
                        BPPString += "@ BV, \"\", \"\", " + i + ", \"\", 0 : 0, \"2\", " + //точка отсчёта 2
                            tx + ", " +
                            ty + ", " +
                            "0, ";

                        if (Sv[i].g > ZD)
                        {
                            BPPString += 5 + ", ";
                            skv = 1;
                        }
                        else
                        {
                            BPPString += Sv[i].g + ", ";
                        }

                        BPPString += Sv[i].r * 2 + ", " + skv + ", -1, 0, 0, 0, 0, 0, 1, \"\", 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, -1, \"" + ID + "\", 0, \"\", \"\", "
                            + tip + ", 0, 0, 0, 0, \"\", 0, 0, 0, 0, 0, \"\", \"\", \"BV\", 0, 0, 0, 0, -1, 0, 0, 0" +
                            "\r\n";
                    }

                    if (Sv[i].OO == "1")
                    {
                        tx = Sv[i].x0; ty = YD - Sv[i].y0;
                        BPPString += "@ BV, \"\", \"\", " + i + ", \"\", 0 : 0, \"1\", " + //точка отсчёта 1
                            tx + ", " +
                            ty + ", " +
                            "0, ";

                        if (Sv[i].g > ZD)
                        {
                            BPPString += 5 + ", ";
                            skv = 1;
                        }
                        else
                        {
                            BPPString += Sv[i].g + ", ";
                        }

                        BPPString += Sv[i].r * 2 + ", " + skv + ", -1, 0, 0, 0, 0, 0, 1, \"\", 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, -1, \"" + ID + "\", 0, \"\", \"\", "
                            + tip + ", 0, 0, 0, 0, \"\", 0, 0, 0, 0, 0, \"\", \"\", \"BV\", 0, 0, 0, 0, -1, 0, 0, 0" +
                            "\r\n";

                    }

                    if (Sv[i].OO == "2")
                    {
                        tx = XD - Sv[i].x0; ty = YD - Sv[i].y0;
                        BPPString += "@ BV, \"\", \"\", " + i + ", \"\", 0 : 0, \"4\", " + //точка отсчёта 4
                             tx + ", " +
                             ty + ", " +
                             "0, ";

                        if (Sv[i].g > ZD)
                        {
                            BPPString += 5 + ", ";
                            skv = 1;
                        }
                        else
                        {
                            BPPString += Sv[i].g + ", ";
                        }

                        BPPString += Sv[i].r * 2 + ", " + skv + ", -1, 0, 0, 0, 0, 0, 1, \"\", 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, -1, \"" + ID + "\", 0, \"\", \"\", "
                            + tip + ", 0, 0, 0, 0, \"\", 0, 0, 0, 0, 0, \"\", \"\", \"BV\", 0, 0, 0, 0, -1, 0, 0, 0" +
                            "\r\n";


                    }

                    if (Sv[i].OO == "3")
                    {
                        tx = XD - Sv[i].x0; ty = Sv[i].y0;
                        BPPString += "@ BV, \"\", \"\", " + i + ", \"\", 0 : 0, \"3\", " + //точка отсчёта 3
                             tx + ", " +
                             ty + ", " +
                             "0, ";

                        if (Sv[i].g > ZD)
                        {
                            BPPString += 5 + ", ";
                            skv = 1;
                        }
                        else
                        {
                            BPPString += Sv[i].g + ", ";
                        }

                        BPPString += Sv[i].r * 2 + ", " + skv + ", -1, 0, 0, 0, 0, 0, 1, \"\", 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, -1, \"" + ID + "\", 0, \"\", \"\", "
                            + tip + ", 0, 0, 0, 0, \"\", 0, 0, 0, 0, 0, \"\", \"\", \"BV\", 0, 0, 0, 0, -1, 0, 0, 0" +
                            "\r\n";


                    }
                }



                if (Sv[i].vid == 0) // торцевые
                {

                    tx = Sv[i].x0;
                    ty = Sv[i].y0;
                    tz = Sv[i].z0;


                    if ((Sv[i].OO == "1") && (Sv[i].x0 == 0)) //слева направо сверху 
                    {

                        ty = YD - ty;

                        BPPString += "@ BH, \"\", \"\", " + i
                            + " \"\", 0 : 1, \"1\","
                            + ty
                            + ", 0, 0, "
                            + Sv[i].g
                            + ", "
                            + Sv[i].r * 2
                            + ", 0, -1, 32, 32, 50, 0, 45, 0, \"\", 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, -1,"
                            + "\"" + ID + "\""
                            + ", 0, \"\", \"\", 0, 0, 0, 0, 0, \"\", 0, 0, 0, 0, 0, \"\", \"\", \"BH\", 0, 0, 0, 0, -1, 0, 0, 0"
                            + "\r\n";

                    }

                    if ((Sv[i].OO == "1") && (Sv[i].y0 == YD)) //сверху вниз слева 
                    {



                        BPPString += "@ BH, \"\", \"\", " + i
                            + " \"\", 0 : 4, \"4\","
                            + tx
                            + ", 0, 0, "
                            + Sv[i].g
                            + ", "
                            + Sv[i].r * 2
                            + ", 0, -1, 32, 32, 50, 0, 45, 0, \"\", 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, -1,"
                            + "\"" + ID + "\""
                            + ", 0, \"\", \"\", 0, 0, 0, 0, 0, \"\", 0, 0, 0, 0, 0, \"\", \"\", \"BH\", 0, 0, 0, 0, -1, 0, 0, 0"
                            + "\r\n";

                    }

                    if ((Sv[i].OO == "0") && (Sv[i].x0 == 0)) //слева направо снизу 
                    {



                        BPPString += "@ BH, \"\", \"\", " + i
                            + " \"\", 0 : 1, \"4\","
                            + ty
                            + ", 0, 0, "
                            + Sv[i].g
                            + ", "
                            + Sv[i].r * 2
                            + ", 0, -1, 32, 32, 50, 0, 45, 0, \"\", 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, -1,"
                            + "\"" + ID + "\""
                            + ", 0, \"\", \"\", 0, 0, 0, 0, 0, \"\", 0, 0, 0, 0, 0, \"\", \"\", \"BH\", 0, 0, 0, 0, -1, 0, 0, 0"
                            + "\r\n";

                    }

                    if ((Sv[i].OO == "0") && (Sv[i].y0 == 0)) //снизу вверх слева
                    {



                        BPPString += "@ BH, \"\", \"\", " + i
                            + " \"\", 0 : 2, \"1\","
                            + tx
                            + ", 0, 0, "
                            + Sv[i].g
                            + ", "
                            + Sv[i].r * 2
                            + ", 0, -1, 32, 32, 50, 0, 45, 0, \"\", 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, -1,"
                            + "\"" + ID + "\""
                            + ", 0, \"\", \"\", 0, 0, 0, 0, 0, \"\", 0, 0, 0, 0, 0, \"\", \"\", \"BH\", 0, 0, 0, 0, -1, 0, 0, 0"
                            + "\r\n";

                    }


                    if ((Sv[i].OO == "2") && (Sv[i].x0 == XD)) //справа налево сверху 
                    {

                        ty = YD - ty;

                        BPPString += "@ BH, \"\", \"\", " + i
                            + " \"\", 0 : 3, \"4\","
                            + ty
                            + ", 0, 0, "
                            + Sv[i].g
                            + ", "
                            + Sv[i].r * 2
                            + ", 0, -1, 32, 32, 50, 0, 45, 0, \"\", 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, -1,"
                            + "\"" + ID + "\""
                            + ", 0, \"\", \"\", 0, 0, 0, 0, 0, \"\", 0, 0, 0, 0, 0, \"\", \"\", \"BH\", 0, 0, 0, 0, -1, 0, 0, 0"
                            + "\r\n";

                    }

                    if ((Sv[i].OO == "2") && (Sv[i].y0 == YD)) //сверху вниз справа
                    {

                        tx = XD - tx;

                        BPPString += "@ BH, \"\", \"\", " + i
                            + " \"\", 0 : 4, \"1\","
                            + tx
                            + ", 0, 0, "
                            + Sv[i].g
                            + ", "
                            + Sv[i].r * 2
                            + ", 0, -1, 32, 32, 50, 0, 45, 0, \"\", 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, -1,"
                            + "\"" + ID + "\""
                            + ", 0, \"\", \"\", 0, 0, 0, 0, 0, \"\", 0, 0, 0, 0, 0, \"\", \"\", \"BH\", 0, 0, 0, 0, -1, 0, 0, 0"
                            + "\r\n";

                    }

                    if ((Sv[i].OO == "3") && (Sv[i].x0 == XD)) //справа налево снизу 
                    {



                        BPPString += "@ BH, \"\", \"\", " + i
                            + " \"\", 0 : 3, \"1\","
                            + ty
                            + ", 0, 0, "
                            + Sv[i].g
                            + ", "
                            + Sv[i].r * 2
                            + ", 0, -1, 32, 32, 50, 0, 45, 0, \"\", 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, -1,"
                            + "\"" + ID + "\""
                            + ", 0, \"\", \"\", 0, 0, 0, 0, 0, \"\", 0, 0, 0, 0, 0, \"\", \"\", \"BH\", 0, 0, 0, 0, -1, 0, 0, 0"
                            + "\r\n";

                    }


                    if ((Sv[i].OO == "3") && (Sv[i].y0 == 0)) //снизу вверх справа 
                    {

                        tx = XD - tx;

                        BPPString += "@ BH, \"\", \"\", " + i
                            + " \"\", 0 : 2, \"4\","
                            + tx
                            + ", 0, 0, "
                            + Sv[i].g
                            + ", "
                            + Sv[i].r * 2
                            + ", 0, -1, 32, 32, 50, 0, 45, 0, \"\", 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, -1,"
                            + "\"" + ID + "\""
                            + ", 0, \"\", \"\", 0, 0, 0, 0, 0, \"\", 0, 0, 0, 0, 0, \"\", \"\", \"BH\", 0, 0, 0, 0, -1, 0, 0, 0"
                            + "\r\n";

                    }










                }
            }

            
            BPPString += "" + "\r\n";
            BPPString += "[VBSCRIPT]" + "\r\n";
            BPPString += "" + "\r\n";
            BPPString += "[MACRODATA]" + "\r\n";
            BPPString += "" + "\r\n";
            BPPString += "[TDCODES]" + "\r\n";
            BPPString += "" + "\r\n";
            BPPString += "[PCF]" + "\r\n";
            BPPString += "" + "\r\n";
            BPPString += "[TOOLING]" + "\r\n";
            BPPString += "" + "\r\n";
            BPPString += "[SUBPROGS]" + "\r\n";



            BPPString += "" + "\r\n";




            if (cf == 2)
            {

                string BPPFileName = folderBrowserDialog1.SelectedPath + "\\" + FileName.Text + ".bpp";

                StreamWriter sw = new StreamWriter(BPPFileName, false, Encoding.GetEncoding("Windows-1251"));

                sw.WriteLine(BPPString);
                sw.Close();

            }



            if (cf == 1)
            {

                saveFileDialog1.Filter = "BPP|*.bpp";
                saveFileDialog1.FileName = FileName.Text;

                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string BPPFileName = System.IO.Path.GetFileName(saveFileDialog1.FileName);
                    string BPPFileCatalog = System.IO.Path.GetDirectoryName(saveFileDialog1.FileName);
                    string BPPFileNameExt = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog1.FileName);


                    //saveFileDialog1.InitialDirectory = LITECatalog;

                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, Encoding.GetEncoding("Windows-1251"));

                    sw.WriteLine(BPPString);
                    sw.Close();


                }
                else
                    return;

            }


        }

        private void SaveBPPall_Click(object sender, EventArgs e)
        {
            if (FileDroplistBox.Items.Count > 0)
            {
                if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    for (int i = 0; i < FileDroplistBox.Items.Count; i++)
                    {
                        FileDroplistBox.SetSelected(i, true);
                        string FileSelected = ListFileDrop[FileDroplistBox.SelectedIndex];

                        if (FileSelected.ToLower().EndsWith(".mpr") == true)
                        {
                            FileName.Text = FileSelected;
                            WWF = true; dnd = true;
                            OWW.PerformClick();

                            save_BIESSE(2);

                        }

                    }
                }
            }
            else
                return;

        }

        private void OPN_Click(object sender, EventArgs e)
        {
            woodwopopen = false;
            nameFILE = "";
            openFileWW.Filter = "|*.dxf";
            this.Invalidate();
            dxfCount = 0;
            string line = "";
            int i;
            dxf tempprimit;

            double t, x1, x2, y1, y2, u, r;
            textcontrol.Text = ""; // текст бокс обнулить

            bool openfile = false;

            if (!WWF)
            {
                if (openFileWW.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    openfile = true;
                    nameFILE = openFileWW.FileName;

                    // MessageBox.Show(nameFILE);

                    outFileWoodWOP = nameFILE;
                    outFileWoodWOP = outFileWoodWOP.Remove(outFileWoodWOP.LastIndexOf(".")) + ".mpr";

                    outFileHIRZT = nameFILE;
                    outFileHIRZT = outFileHIRZT.Remove(outFileWoodWOP.LastIndexOf(".")) + ".pd4";
                }
            }

            if (WWF)
            {

                openfile = true; nameFILE = FileName.Text;
            }




            WWF = false;
            if (openfile)
            {




                commentBox.Text = "";

                countT = countV = 0;


                using (var sr = new StreamReader(nameFILE, encoding: Encoding.GetEncoding(1251)))
                {

                    // woodwopopen = true;

                    while ((line = sr.ReadLine()) != "BLOCKS") { }



                    while (((line = sr.ReadLine()) != "OBJECTS"))
                    {

                        if (line == "LINE") //читаем линии
                        {


                            while ((line = sr.ReadLine()) != "AcDbLine") { }
                            line = sr.ReadLine(); line = sr.ReadLine();

                            primit[dxfCount].xx1 = Convert.ToDouble(line);
                            primit[dxfCount].xx1 = Math.Round(primit[dxfCount].xx1 * 1000) / 1000;

                            line = sr.ReadLine();
                            line = sr.ReadLine();

                            primit[dxfCount].yy1 = Convert.ToDouble(line);
                            primit[dxfCount].yy1 = Math.Round(primit[dxfCount].yy1 * 1000) / 1000;

                            line = sr.ReadLine();
                            line = sr.ReadLine();

                            primit[dxfCount].xx2 = Convert.ToDouble(line);
                            primit[dxfCount].xx2 = Math.Round(primit[dxfCount].xx2 * 1000) / 1000;

                            line = sr.ReadLine();
                            line = sr.ReadLine();

                            primit[dxfCount].yy2 = Convert.ToDouble(line);
                            primit[dxfCount].yy2 = Math.Round(primit[dxfCount].yy2 * 1000) / 1000;

                            primit[dxfCount].rr = 0;
                            primit[dxfCount].xx0 = 0;
                            primit[dxfCount].yy0 = 0;
                            primit[dxfCount].linia = "LINE";
                            dxfCount++;
                        }

                        if (line == "ARC") // читаем дуги
                        {
                            while ((line = sr.ReadLine()) != "AcDbCircle") { }
                            line = sr.ReadLine();
                            line = sr.ReadLine();
                            //textcontrol.Text += "ARC > X0=" + line + "\r\n";
                            x0 = Convert.ToDouble(line);

                            primit[dxfCount].xx0 = Convert.ToDouble(line);
                            primit[dxfCount].xx0 = Math.Round(primit[dxfCount].xx0 * 1000) / 1000;


                            line = sr.ReadLine();
                            line = sr.ReadLine();
                            //textcontrol.Text += " Y0=" + line + "\r\n";
                            y0 = Convert.ToDouble(line);

                            primit[dxfCount].yy0 = Convert.ToDouble(line);
                            primit[dxfCount].yy0 = Math.Round(primit[dxfCount].yy0 * 1000) / 1000;

                            line = sr.ReadLine();
                            line = sr.ReadLine();
                            //textcontrol.Text += " R=" + line + "\r\n";
                            r = Convert.ToDouble(line);

                            primit[dxfCount].rr = Convert.ToDouble(line);
                            primit[dxfCount].rr = Math.Round(primit[dxfCount].rr * 1000) / 1000;

                            line = sr.ReadLine(); line = sr.ReadLine(); line = sr.ReadLine();
                            line = sr.ReadLine();
                            //textcontrol.Text += " U1=" + line + "\r\n";

                            primit[dxfCount].u1 = Convert.ToDouble(line);
                            primit[dxfCount].u1 = Math.Round(primit[dxfCount].u1 * 1000) / 1000;

                            u = Convert.ToDouble(line);
                            u = Math.PI / 180 * u;

                            x1 = r * Math.Cos(u);

                            x1 = x1 + x0;
                            //textcontrol.Text += " x1=" + (x1) + "\r\n";

                            primit[dxfCount].xx1 = x1;
                            primit[dxfCount].xx1 = Math.Round(primit[dxfCount].xx1 * 1000) / 1000;

                            y1 = (r * Math.Sin(u));
                            y1 = y1 + y0;

                            primit[dxfCount].yy1 = y1;
                            primit[dxfCount].yy1 = Math.Round(primit[dxfCount].yy1 * 1000) / 1000;

                            //textcontrol.Text += " y1=" + (y1) + "\r\n";



                            line = sr.ReadLine();
                            line = sr.ReadLine();
                            //textcontrol.Text += " U2=" + line + "\r\n";

                            primit[dxfCount].u2 = Convert.ToDouble(line);
                            primit[dxfCount].u2 = Math.Round(primit[dxfCount].u2 * 1000) / 1000;

                            u = Convert.ToDouble(line);
                            u = Math.PI / 180 * u;

                            x2 = r * Math.Cos(u);
                            //textcontrol.Text += " x2=" + (x2 + x0) + "\r\n";
                            x2 = x2 + x0;
                            primit[dxfCount].xx2 = x2;
                            primit[dxfCount].xx2 = Math.Round(primit[dxfCount].xx2 * 1000) / 1000;

                            y2 = (r * Math.Sin(u));
                            //textcontrol.Text += " y2=" + (y2 + y0) + "\r\n";
                            y2 = y2 + y0;
                            primit[dxfCount].yy2 = y2;
                            primit[dxfCount].yy2 = Math.Round(primit[dxfCount].yy2 * 1000) / 1000;

                            primit[dxfCount].linia = "ARC";

                            dxfCount++;
                            //textcontrol.Text +=  "\r\r\n";



                        }

                        if (line == "CIRCLE") // читаем круги
                        {
                            /* while ((line = sr.ReadLine()) != " 62") { }
                             line = sr.ReadLine();
                             primit[dxfCount].colordxf = line;*/


                            primit[dxfCount].colordxf = "";
                            while ((line = sr.ReadLine()) != "AcDbCircle")
                            {
                                if (line == " 62")
                                {
                                    line = sr.ReadLine();
                                    primit[dxfCount].colordxf = line;
                                }
                            }
                            line = sr.ReadLine();
                            line = sr.ReadLine();


                            primit[dxfCount].xx0 = Convert.ToDouble(line);
                            primit[dxfCount].xx0 = Math.Round(primit[dxfCount].xx0 * 1000) / 1000;

                            line = sr.ReadLine();
                            line = sr.ReadLine();


                            primit[dxfCount].yy0 = Convert.ToDouble(line);
                            primit[dxfCount].yy0 = Math.Round(primit[dxfCount].yy0 * 1000) / 1000;

                            line = sr.ReadLine();
                            line = sr.ReadLine();

                            primit[dxfCount].rr = Convert.ToDouble(line);
                            primit[dxfCount].rr = Math.Round(primit[dxfCount].rr * 1000) / 1000;

                            primit[dxfCount].xx1 = primit[dxfCount].yy1 = primit[dxfCount].xx2 = primit[dxfCount].yy2 = 0;
                            primit[dxfCount].linia = "CIRCLE";
                            dxfCount++;
                        }

                        if (line == "LWPOLYLINE") // читаем полилинии
                        {
                            x0 = y0 = t = 0;
                            while ((line = sr.ReadLine()) != "AcDbPolyline") { }


                            line = sr.ReadLine(); line = sr.ReadLine(); line = sr.ReadLine(); line = sr.ReadLine(); line = sr.ReadLine(); line = sr.ReadLine(); line = sr.ReadLine();
                            line = sr.ReadLine();

                            x1 = Convert.ToDouble(line);

                            primit[dxfCount].xx1 = Convert.ToDouble(line);
                            primit[dxfCount].xx1 = Math.Round(primit[dxfCount].xx1 * 1000) / 1000;

                            line = sr.ReadLine();
                            line = sr.ReadLine();

                            y1 = Convert.ToDouble(line);

                            primit[dxfCount].yy1 = Convert.ToDouble(line);
                            primit[dxfCount].yy1 = Math.Round(primit[dxfCount].yy1 * 1000) / 1000;

                            line = sr.ReadLine();
                            if (line == " 42")
                            {
                                line = sr.ReadLine();
                                //textcontrol.Text += " TAN1=" + line;
                                t = Convert.ToDouble(line);
                                primit[dxfCount].t = t;
                                line = sr.ReadLine();

                            }

                            line = sr.ReadLine();

                            x2 = Convert.ToDouble(line);

                            primit[dxfCount].xx2 = Convert.ToDouble(line);
                            primit[dxfCount].xx2 = Math.Round(primit[dxfCount].xx2 * 1000) / 1000;


                            line = sr.ReadLine();
                            line = sr.ReadLine();

                            y2 = Convert.ToDouble(line);

                            primit[dxfCount].yy2 = Convert.ToDouble(line);
                            primit[dxfCount].yy2 = Math.Round(primit[dxfCount].yy2 * 1000) / 1000;

                            line = sr.ReadLine();
                            if (line == " 42")
                            {
                                line = sr.ReadLine();
                                //textcontrol.Text += " TAN2=" + line;
                                line = sr.ReadLine();
                            }
                            primit[dxfCount].xx0 = primit[dxfCount].yy0 = primit[dxfCount].rr = 0; primit[dxfCount].linia = "LINE";
                            if (t != 0) { polyline(t, x1, y1, x2, y2, x0, y0); }




                            dxfCount++;
                        }
                    }

                    sr.Close();
                }
                dxfCount--;





                for (i = 0; i < dxfCount + 1; i++)
                {
                    //textcontrol.Text += Convert.ToString(primit[i].linia) + "  ";
                    if (primit[i].linia != "CIRCLE")
                    {

                        // textcontrol.Text += Convert.ToString(primit[i].xx1) + "  ";
                        // textcontrol.Text += Convert.ToString(primit[i].yy1) + "  ";
                        // textcontrol.Text += Convert.ToString(primit[i].xx2) + "  ";
                        // textcontrol.Text += Convert.ToString(primit[i].yy2) + "  ";
                    }
                    if (primit[i].linia == "CIRCLE")
                    {
                        // textcontrol.Text += Convert.ToString(primit[i].xx0) + "  ";
                        // textcontrol.Text += Convert.ToString(primit[i].yy0) + "  ";



                        //textcontrol.Text += "R=" + Convert.ToString(primit[i].rr) + " ";

                    }

                    if (primit[i].linia == "ARC")
                    {
                        //textcontrol.Text += Convert.ToString(primit[i].xx0) + "  ";
                        //textcontrol.Text += Convert.ToString(primit[i].yy0) + "  ";



                        //textcontrol.Text += "u1=" + Convert.ToString(primit[i].u1) + " ";
                        //textcontrol.Text += "u2=" + Convert.ToString(primit[i].u2) + " ";
                    }

                    if (primit[i].linia == "POLY")
                    {
                        //textcontrol.Text += Convert.ToString(primit[i].xx0) + "  ";
                        //textcontrol.Text += Convert.ToString(primit[i].yy0) + "  ";

                        primit[i].u1 = FindUgol(primit[i].xx1, primit[i].yy1, i);
                        primit[i].u2 = FindUgol(primit[i].xx2, primit[i].yy2, i);

                        if ((primit[i].u1 < primit[i].u2) && (primit[i].t < 0)) { primit[i].u1 += 360; }

                        //textcontrol.Text += "u1= " + Convert.ToString(primit[i].u1) + " ";
                        //textcontrol.Text += "u2= " + Convert.ToString(primit[i].u2) + " ";
                        //textcontrol.Text += "tan= " + Convert.ToString(primit[i].t) + " ";


                    }


                }










                // сортировка 

                tempprimit = primit[0];



                int j;

                for (j = 0; j < dxfCount; j++)
                {
                    for (i = j; i < dxfCount; i++)
                    {
                        if ((primit[j].xx2 == primit[i + 1].xx1) && (primit[j].yy2 == primit[i + 1].yy1) && (primit[i + 1].linia != "CIRCLE"))
                        {
                            tempprimit = primit[i + 1];

                            primit[i + 1] = primit[j + 1];
                            primit[j + 1] = tempprimit;


                        }

                        if ((primit[j].xx2 == primit[i + 1].xx2) && (primit[j].yy2 == primit[i + 1].yy2) && (primit[i + 1].linia != "CIRCLE"))
                        {
                            tempprimit = primit[i + 1];

                            primit[i + 1] = primit[j + 1];
                            primit[j + 1] = tempprimit;

                            tempprimit = primit[j + 1];

                            primit[j + 1].xx1 = tempprimit.xx2;
                            primit[j + 1].yy1 = tempprimit.yy2;
                            primit[j + 1].xx2 = tempprimit.xx1;
                            primit[j + 1].yy2 = tempprimit.yy1;
                            primit[j + 1].u1 = tempprimit.u2;
                            primit[j + 1].u2 = tempprimit.u1;
                            primit[j + 1].t *= -1;

                        }
                    }

                }

                XS = 10;
                YS = 550;
                i = 0;



                j = 0;

                for (i = 0; i < dxfCount + 1; i++)
                {


                    if ((primit[i].linia == "LINE") || (primit[i].linia == "POLY"))
                    {


                        textcontrol.Text += Convert.ToString(primit[i].linia) + "  ";
                        textcontrol.Text += Convert.ToString(primit[i].xx1) + "  ";
                        textcontrol.Text += Convert.ToString(primit[i].yy1) + "  ";
                        textcontrol.Text += Convert.ToString(primit[i].xx2) + "  ";
                        textcontrol.Text += Convert.ToString(primit[i].yy2) + "  ";

                        if (primit[i].linia == "POLY")
                        {
                            textcontrol.Text += "u1=" + Convert.ToString(primit[i].u1) + " ";
                            textcontrol.Text += "u2=" + Convert.ToString(primit[i].u2) + " ";
                            textcontrol.Text += "tan= " + Convert.ToString(primit[i].t) + " ";

                        }

                    }

                    if (primit[i].linia == "ARC")
                    {

                        x1 = primit[i].xx0 + primit[i].rr * Math.Cos(primit[i].u1 * Math.PI / 180);
                        y1 = primit[i].yy0 + primit[i].rr * Math.Sin(primit[i].u1 * Math.PI / 180);



                        textcontrol.Text += Convert.ToString(primit[i].linia) + "  ";
                        textcontrol.Text += Convert.ToString(primit[i].xx1) + "  ";
                        textcontrol.Text += Convert.ToString(primit[i].yy1) + "  ";
                        textcontrol.Text += Convert.ToString(primit[i].xx2) + "  ";
                        textcontrol.Text += Convert.ToString(primit[i].yy2) + "  ";
                        textcontrol.Text += Convert.ToString(primit[i].xx0) + "  ";
                        textcontrol.Text += Convert.ToString(primit[i].yy0) + "  ";



                        textcontrol.Text += "u1=" + Convert.ToString(primit[i].u1) + "\r\n";
                        textcontrol.Text += "u2=" + Convert.ToString(primit[i].u2) + "\r\n";
                    }
                    textcontrol.Text += "\r\n";

                    if ((primit[i].xx2 == primit[0].xx1) && (primit[i].yy2 == primit[0].yy1)) //первый замкнутый контур
                    {
                        textcontrol.Text += (i + 1) + "\r\n";
                        primitcol1 = i;
                        break;
                    }
                }


                j = 0; // разделяем дуги и полилинии на линии
                t = 1;// шаг дуги

                MX = 0.25;

                for (i = 0; i < dxfCount + 1; i++)
                {
                    if ((primit[i].linia == "LINE"))
                    {
                        g.DrawLine(p2, Convert.ToInt32(primit[i].xx1 * MX) + XS, Convert.ToInt32(-primit[i].yy1 * MX) + YS, Convert.ToInt32(primit[i].xx2 * MX) + XS, Convert.ToInt32(-primit[i].yy2 * MX) + YS);

                        kontur[j].xx1 = primit[i].xx1;
                        kontur[j].yy1 = primit[i].yy1;
                        kontur[j].xx2 = primit[i].xx2;
                        kontur[j].yy2 = primit[i].yy2;
                        kontur[j].linia = "LINE";
                        j++;
                    }

                    if (primit[i].linia == "ARC")
                    {

                        x1 = primit[i].xx0 + primit[i].rr * Math.Cos(primit[i].u1 * Math.PI / 180);
                        y1 = primit[i].yy0 + primit[i].rr * Math.Sin(primit[i].u1 * Math.PI / 180);

                        if (primit[i].u1 < primit[i].u2)
                        {
                            for (u = primit[i].u1 + t; u != primit[i].u2; u += t)
                            {

                                if ((u + t) > primit[i].u2) { u = primit[i].u2 - t; }
                                x2 = primit[i].xx0 + primit[i].rr * Math.Cos((u + t) * Math.PI / 180);
                                y2 = primit[i].yy0 + primit[i].rr * Math.Sin((u + t) * Math.PI / 180);


                                Thread.Sleep(10);

                                kontur[j].xx1 = x1;
                                kontur[j].yy1 = y1;
                                kontur[j].xx2 = x2;
                                kontur[j].yy2 = y2;
                                kontur[j].linia = "LINE";
                                j++;

                                g.DrawLine(p2, Convert.ToInt32(x1 * MX) + XS, Convert.ToInt32(-y1 * MX) + YS, Convert.ToInt32(x2 * MX) + XS, Convert.ToInt32(-y2 * MX) + YS);

                                x1 = primit[i].xx0 + primit[i].rr * Math.Cos((u + t) * Math.PI / 180);
                                y1 = primit[i].yy0 + primit[i].rr * Math.Sin((u + t) * Math.PI / 180);
                                if ((u + t) == primit[i].u2) { break; }
                            }
                        }

                        if (primit[i].u1 > primit[i].u2)
                        {
                            for (u = primit[i].u1 - t; u != primit[i].u2; u -= t)
                            {

                                if ((u - t) < primit[i].u2) { u = primit[i].u2 - t; }
                                x2 = primit[i].xx0 + primit[i].rr * Math.Cos((u + t) * Math.PI / 180);
                                y2 = primit[i].yy0 + primit[i].rr * Math.Sin((u + t) * Math.PI / 180);


                                Thread.Sleep(10);

                                kontur[j].xx1 = x1;
                                kontur[j].yy1 = y1;
                                kontur[j].xx2 = x2;
                                kontur[j].yy2 = y2;
                                kontur[j].linia = "LINE";
                                j++;

                                g.DrawLine(p2, Convert.ToInt32(x1 * MX) + XS, Convert.ToInt32(-y1 * MX) + YS, Convert.ToInt32(x2 * MX) + XS, Convert.ToInt32(-y2 * MX) + YS);

                                x1 = primit[i].xx0 + primit[i].rr * Math.Cos((u + t) * Math.PI / 180);
                                y1 = primit[i].yy0 + primit[i].rr * Math.Sin((u + t) * Math.PI / 180);
                                if ((u + t) == primit[i].u2) { break; }
                            }
                        }
                    }


                    if (primit[i].linia == "POLY")
                    {

                        x1 = primit[i].xx0 + primit[i].rr * Math.Cos(primit[i].u1 * Math.PI / 180);
                        y1 = primit[i].yy0 + primit[i].rr * Math.Sin(primit[i].u1 * Math.PI / 180);



                        if ((primit[i].u1 < primit[i].u2) && (primit[i].t < 0)) { primit[i].u2 -= 360; }
                        if ((primit[i].u1 > primit[i].u2) && (primit[i].t > 0)) { primit[i].u2 += 360; }

                        if ((primit[i].u1 < primit[i].u2) && (primit[i].t > 0)) //против часовой
                        {
                            for (u = primit[i].u1 + t; u != primit[i].u2; u += t)
                            {

                                if ((u + t) > primit[i].u2) { u = primit[i].u2 - t; }
                                x2 = primit[i].xx0 + primit[i].rr * Math.Cos((u + t) * Math.PI / 180);
                                y2 = primit[i].yy0 + primit[i].rr * Math.Sin((u + t) * Math.PI / 180);

                                kontur[j].xx1 = x1;
                                kontur[j].yy1 = y1;
                                kontur[j].xx2 = x2;
                                kontur[j].yy2 = y2;
                                kontur[j].linia = "LINE";
                                j++;

                                Thread.Sleep(10);
                                g.DrawLine(p2, Convert.ToInt32(x1 * MX) + XS, Convert.ToInt32(-y1 * MX) + YS, Convert.ToInt32(x2 * MX) + XS, Convert.ToInt32(-y2 * MX) + YS);

                                x1 = primit[i].xx0 + primit[i].rr * Math.Cos((u + t) * Math.PI / 180);
                                y1 = primit[i].yy0 + primit[i].rr * Math.Sin((u + t) * Math.PI / 180);
                                if ((u + t) == primit[i].u2) { break; }
                            }
                        }

                        if ((primit[i].u1 > primit[i].u2) && (primit[i].t < 0))
                        {
                            for (u = primit[i].u1 - t; u != primit[i].u2; u -= t)
                            {

                                if ((u - t) < primit[i].u2) { u = primit[i].u2 - t; }
                                x2 = primit[i].xx0 + primit[i].rr * Math.Cos((u + t) * Math.PI / 180);
                                y2 = primit[i].yy0 + primit[i].rr * Math.Sin((u + t) * Math.PI / 180);

                                kontur[j].xx1 = x1;
                                kontur[j].yy1 = y1;
                                kontur[j].xx2 = x2;
                                kontur[j].yy2 = y2;
                                kontur[j].linia = "LINE";
                                j++;

                                Thread.Sleep(10);
                                g.DrawLine(p2, Convert.ToInt32(x1 * MX) + XS, Convert.ToInt32(-y1 * MX) + YS, Convert.ToInt32(x2 * MX) + XS, Convert.ToInt32(-y2 * MX) + YS);

                                x1 = primit[i].xx0 + primit[i].rr * Math.Cos((u + t) * Math.PI / 180);
                                y1 = primit[i].yy0 + primit[i].rr * Math.Sin((u + t) * Math.PI / 180);
                                if ((u + t) == primit[i].u2) { break; }
                            }
                        }

                    }
                }




                double x0d, y0d;//ищем габариты детали
                XD = YD = x0d = y0d = 0;
                textcontrol.Text = "";

                for (i = 0; i < j; i++)
                {
                    //g.DrawLine(p2, Convert.ToInt32(kontur[i].xx1) + XS, Convert.ToInt32(-kontur[i].yy1) + YS, Convert.ToInt32(kontur[i].xx2) + XS, Convert.ToInt32(-kontur[i].yy2) + YS);
                    //Thread.Sleep(20);
                    if (kontur[i].xx1 > XD) { XD = kontur[i].xx1; }
                    if (kontur[i].xx2 > XD) { XD = kontur[i].xx2; }
                    if (kontur[i].yy1 > YD) { YD = kontur[i].yy1; }
                    if (kontur[i].yy2 > YD) { YD = kontur[i].yy2; }

                    if (kontur[i].xx1 < x0d) { x0d = kontur[i].xx1; }
                    if (kontur[i].xx2 < x0d) { x0d = kontur[i].xx2; }
                    if (kontur[i].yy1 < y0d) { y0d = kontur[i].yy1; }
                    if (kontur[i].yy2 < y0d) { y0d = kontur[i].yy2; }

                    textcontrol.Text += "X" + Convert.ToString(Math.Round(kontur[i].xx1 * 1000) / 1000) + " ";
                    textcontrol.Text += "Y" + Convert.ToString(Math.Round(kontur[i].yy1 * 1000) / 1000) + "\r\n";
                }

                textcontrol.Text += "X" + Convert.ToString(Math.Round(kontur[0].xx1 * 1000) / 1000) + " ";
                textcontrol.Text += "Y" + Convert.ToString(Math.Round(kontur[0].yy1 * 1000) / 1000) + "\r\n";

                XD = Math.Round(XD, 0, MidpointRounding.AwayFromZero);
                YD = Math.Round(YD, 0, MidpointRounding.AwayFromZero);

                textcontrol.Left = 1180;
                textcontrol.Top = 20;
                textcontrol.Width = 100;
                textcontrol.Height = 600;
                dnd = false;
                ADDVertSV();
                ADDHorizSV();
                workdraw();
                drawsv();

                if (countV > 0) // ставим указатель на 1 сверло если есть
                {
                    label1.Text = "1";
                    Sv[1].edit = 1;
                    smena();
                    infosv();
                }

            } //----------
        }


        double FindUgol(double x, double y, int i)
        {
            double temp = -1;

            if ((y - primit[i].yy0) >= 0)
            {
                temp = (x - primit[i].xx0) / primit[i].rr;
                temp = Math.Acos(temp);
                temp /= (Math.PI / 180);
                return temp;


            }

            if ((y - primit[i].yy0) < 0)
            {
                temp = (x - primit[i].xx0) / primit[i].rr;
                temp = Math.Acos(temp);
                temp /= (Math.PI / 180);
                temp = -temp + 360;
                return temp;


            }
            return temp;

        }


        void ADDHorizSV()
        {
            int i;
            double temp;

            for (i = primitcol1 + 1; i < dxfCount + 1; i++)
            {

                if (primit[i].linia == "LINE") // вертикальные отверстия
                {

                    if (((primit[i].yy1 == primit[i].yy2) && (primit[i].yy1 == 0)) || ((primit[i].yy1 == primit[i].yy2) && (primit[i].yy1 == YD)))
                    {
                        textcontrol.Text += Convert.ToString(primit[i].linia) + "  ";
                        temp = primit[i].xx1 + ((primit[i].xx2 - primit[i].xx1) / 2);

                        textcontrol.Text += Convert.ToString(temp) + "  ";

                        Sv[countV + 1].vid = 0;
                        Sv[countV + 1].OO = "-1";
                        Sv[countV + 1].x0 = temp;

                        temp = Math.Abs(primit[i + 1].yy1 - primit[i + 1].yy2);

                        Sv[countV + 1].y0 = primit[i].yy1;
                        Sv[countV + 1].g = temp;
                        Sv[countV + 1].r = Math.Abs(primit[i].xx2 - primit[i].xx1) / 2;
                        Sv[countV + 1].edit = 0;
                        if ((Sv[countV + 1].r * 2 == 5) && (Sv[countV + 1].g == 34)) { Sv[countV + 1].g = 35; } //глубина еврика 35
                        countV++;





                        textcontrol.Text += Convert.ToString(temp) + "  ";
                        textcontrol.Text += "\r\n";
                        i += 3;
                    }

                    if (((primit[i].xx1 == primit[i].xx2) && (primit[i].xx1 == 0)) || ((primit[i].xx1 == primit[i].xx2) && (primit[i].xx1 == XD)))
                    {
                        textcontrol.Text += Convert.ToString(primit[i].linia) + "  ";
                        temp = primit[i].yy1 + ((primit[i].yy2 - primit[i].yy1) / 2);

                        textcontrol.Text += Convert.ToString(temp) + "  ";

                        Sv[countV + 1].vid = 0;
                        Sv[countV + 1].OO = "-1";
                        Sv[countV + 1].y0 = temp;



                        temp = Math.Abs(primit[i + 1].xx1 - primit[i + 1].xx2);

                        Sv[countV + 1].x0 = primit[i].xx1;
                        Sv[countV + 1].g = temp;
                        Sv[countV + 1].r = Math.Abs(primit[i].yy2 - primit[i].yy1) / 2;
                        Sv[countV + 1].edit = 0;
                        if ((Sv[countV + 1].r * 2 == 5) && (Sv[countV + 1].g == 34)) { Sv[countV + 1].g = 35; } //глубина еврика 35
                        countV++;

                        textcontrol.Text += Convert.ToString(temp) + "  ";


                        textcontrol.Text += "\r\n";
                        i += 3;
                    }








                }
            }


        }

        void ADDVertSV()
        {
            int i;

            for (i = primitcol1; i < dxfCount + 1; i++)
            {
                if (primit[i].linia == "CIRCLE") // вертикальные отверстия
                {

                    countV++;

                    Sv[countV].vid = 1;// вертикальное сверление ставим 1
                    Sv[countV].OO = "0";// начальная точка 0


                    Sv[countV].x0 = primit[i].xx0;

                    Sv[countV].y0 = primit[i].yy0;

                    Sv[countV].r = primit[i].rr;
                    Sv[countV].edit = 0;

                    if (textBoxZD.Text == "") { textBoxZD.Text = "16"; }
                    Sv[countV].g = Convert.ToDouble(textBoxZD.Text) + 3; //ставим все сквозный толщина заготовки + 3 мм

                    if (primit[i].colordxf == "5") // если цвет 5 то берём глубины из текст боксов (работает если основной цвет не ставиться не проверял)
                    {

                        if (Sv[countV].r * 2 == 3) { Sv[countV].g = Convert.ToDouble(textBoxD3.Text); }  // расставляем глубины по умолчанию из текст боксов
                        if (Sv[countV].r * 2 == 5) { Sv[countV].g = Convert.ToDouble(textBoxD5.Text); }
                        if (Sv[countV].r * 2 == 8) { Sv[countV].g = Convert.ToDouble(textBoxD8.Text); }
                        if (Sv[countV].r * 2 == 15) { Sv[countV].g = Convert.ToDouble(textBoxD15.Text); }
                        if (Sv[countV].r * 2 == 35) { Sv[countV].g = Convert.ToDouble("13"); }//петля

                    }

                }
            }
        }

























        double x0, y0;
        void polyline(double t, double x1, double y1, double x2, double y2, double x0, double y0)
        {
            //textcontrol.Text += "\r\r\n\r\r\n";
            double u, dl, xt, yt, rad;

            u = 90 - (Math.Abs(2 * Math.Atan(t) * 180 / Math.PI));// проверяем угол
                                                                  //textcontrol.Text += "u = " + Convert.ToString(u) + "\r\r\n\r\r\n";
            u = Math.PI / 180 * u; // переводим угол в радианы




            dl = (Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1))) / 2; // длина половины хорды
                                                                                 //textcontrol.Text += "dl = " + Convert.ToString(dl) + "\r\r\n\r\r\n";

            rad = dl / Math.Cos(u); // вычисляем радиус дуги
                                    // textcontrol.Text += "rad = " + Convert.ToString(rad) + "\r\r\n\r\r\n";

            x0 = (x2 + x1) / 2; y0 = (y2 + y1) / 2; // средняя точка хорды

            x0 = x0 - x1; y0 = y0 - y1; // смещаем среднюю точку в 0.0


            if (t < 0) { u = -u; } // загиб дуги
            xt = x0 * Math.Cos(u) - y0 * Math.Sin(u); // поворачиваем среднюю точку 
            yt = y0 * Math.Cos(u) + x0 * Math.Sin(u); // в сторону центра дуги
            x0 = xt; y0 = yt;

            xt = x0 * (rad / dl); // двигаем точку на
            yt = y0 * (rad / dl); // расстояние радиуса

            x0 = xt + x1; y0 = yt + y1; // возвращаем на место
                                        //textcontrol.Text += Convert.ToString(x0) + "  " + Convert.ToString(y0) + "\r\r\n\r\r\n";

            primit[dxfCount].xx0 = Math.Round(x0 * 1000) / 1000;
            primit[dxfCount].yy0 = Math.Round(y0 * 1000) / 1000;
            primit[dxfCount].rr = Math.Round(rad * 1000) / 1000;
            primit[dxfCount].linia = "POLY";
        }

        private void SKV_Click(object sender, EventArgs e) //кнопка сквозное/глухое отверстие
        {
            int i;
            double tempG;
            if (label1.Text == "") { return; }
            i = Convert.ToInt32(label1.Text);
            tempG = 0;


            if (Sv[i].vid == 1)
            {
                if (Convert.ToDouble(textBoxG.Text) < Convert.ToDouble(textBoxZD.Text))
                {
                    tempG = Convert.ToDouble(textBoxZD.Text) + 3;
                }

                if (Convert.ToDouble(textBoxG.Text) > Convert.ToDouble(textBoxZD.Text))
                {
                    if (Sv[i].r * 2 == 3) { tempG = Convert.ToDouble(textBoxD3.Text); }  // расставляем глубины по умолчанию из текст боксов
                    if (Sv[i].r * 2 == 5) { tempG = Convert.ToDouble(textBoxD5.Text); }
                    if (Sv[i].r * 2 == 8) { tempG = Convert.ToDouble(textBoxD8.Text); }
                    if (Sv[i].r * 2 == 15) { tempG = Convert.ToDouble(textBoxD15.Text); }
                }

                textBoxG.Text = Convert.ToString(tempG);
            }
        }
        //---------------------------------------------------------------------- читаем вудвоп

        public bool woodwopopen = false;
        private void OWW_Click(object sender, EventArgs e)
        {
            woodwopopen = false;
            textWW.Visible = false;
            openFileWW.Filter = "|*.mpr";
            commentBox.Text = "";

            string line = "";
            bool openfile = false;


            string nameFILE = "";
            textcontrol.Text = textWW.Text = "";


            if (!WWF) { if (openFileWW.ShowDialog() == System.Windows.Forms.DialogResult.OK) { openfile = true; nameFILE = openFileWW.FileName; } }

            if (WWF) { openfile = true; nameFILE = FileName.Text; }

            WWF = false;
            if (openfile)
            {

                using (var sr = new StreamReader(nameFILE, encoding: Encoding.GetEncoding(1251)))
                {

                    woodwopopen = true;


                    nameFILE = nameFILE.Remove(nameFILE.LastIndexOf("."));
                    nameFILE = nameFILE.Remove(0, nameFILE.LastIndexOf("\\") + 1);
                    FileName.Text = nameFILE;

                    /*
                     *  outFileHIRZT = outFileHIRZT);
                outFileHIRZT = outFileHIRZT.Remove(outFileHIRZT.LastIndexOf("\\") + 1) + "cad4";
                     * 
                     */


                    string[] tempWW;

                    countV = countT = 0;

                    int I, COUNT1 = 0, commFL = 0, CountNameWW = 0;




                    while ((line = sr.ReadLine()) != "!")
                    {
                        Sv[countV].edit = 0;

                        textcontrol.Text += line + "\r\n";

                        textcontrol.Text = line;
                        
                        if (line.IndexOf("[001") >= 0) //ищем имена перемнных
                        {
                            
                            while (((line = sr.ReadLine()) != "") && (line != " ") && (line[0] != '<'))
                            {

                                if (line.IndexOf('=') != -1)
                                {
                                    tempWW = line.Split('=');
                                    if (tempWW[0] != "KM")
                                    {
                                       // MessageBox.Show(line);
                                        NAMEWW[CountNameWW] = tempWW[0];

                                        tempWW[1] = tempWW[1].Replace("\"", "");
                                        ZZWW[CountNameWW] = tempWW[1];



                                        CountNameWW++;
                                    }
                                }

                            }

                        }




                        if (line.IndexOf("<101") >= 0) //коментарий
                        { commFL = 1; }


                        if (line.IndexOf("<102") >= 0) //вертикальные
                        {
                            textWW.Text += "\r\n";
                            countV++;
                            Sv[countV].edit = 0;
                            Sv[countV].OO = "0";
                            Sv[countV].vid = 1;
                            Sv[countV].NAPR = "";
                            textWW.Text += "вертикальное>" + Sv[countV].vid + "\r\n";


                        }
                        if (line.IndexOf("<103") >= 0)//горизонтальные
                        {
                            textWW.Text += "\r\n";
                            countV++;
                            Sv[countV].edit = 0;
                            Sv[countV].OO = "0";
                            Sv[countV].vid = 0;
                            Sv[countV].NAPR = "";
                            textWW.Text += "горизонтальное>" + Sv[countV].vid + "\r\n";


                        }

                        if (line.IndexOf("<109") >= 0) //пропил
                        {
                            textWW.Text += "\r\n";
                            countV++;
                            Sv[countV].edit = 0;
                            Sv[countV].OO = "0";
                            Sv[countV].vid = -1;
                            Sv[countV].NAPR = "";
                            textWW.Text += "пропил>" + Sv[countV].vid + "\r\n";


                        }

                        tempWW = line.Split('=');

                        if (line.IndexOf("_BSX=") >= 0)//длина
                        {
                            textWW.Text += " -- " + countV + " -- ";
                            XD = Math.Round(Convert.ToDouble(tempWW[1]), 1);
                            textWW.Text += tempWW[0] + "--->" + XD + "\r\n";
                            textBoxXD.Text = Convert.ToString(XD);


                        }

                        if (line.IndexOf("_BSY=") >= 0)//ширина
                        {
                            textWW.Text += " -- " + countV + " -- ";
                            YD = Math.Round(Convert.ToDouble(tempWW[1]), 1);
                            textWW.Text += tempWW[0] + "--->" + YD + "\r\n";
                            textBoxYD.Text = Convert.ToString(YD);
                        }

                        if (line.IndexOf("_BSZ=") >= 0)//толщина
                        {
                            textWW.Text += " -- " + countV + " -- ";
                            ZD = Math.Round(Convert.ToDouble(tempWW[1]), 1);
                            textWW.Text += tempWW[0] + "--->" + ZD + "\r\n";
                            textBoxZD.Text = Convert.ToString(ZD);
                        }

                        //-----------------------------------------------------------------------------------------------------------------
                        if (line.IndexOf("LA=") >= 0) { tempWW[1] = tempWW[1].Replace("\"", ""); textWW.Text += tempWW[0] + "--->" + tempWW[1] + "\r\n"; }
                        if (line.IndexOf("BR=") >= 0) { tempWW[1] = tempWW[1].Replace("\"", ""); textWW.Text += tempWW[0] + "--->" + tempWW[1] + "\r\n"; }
                        if (line.IndexOf("DI=") >= 0) { tempWW[1] = tempWW[1].Replace("\"", ""); textWW.Text += tempWW[0] + "--->" + tempWW[1] + "\r\n"; }
                        //-----------------------------------------------------------------------------------------------------------------

                        if ((line.IndexOf("KM=") >= 0) && (commFL == 1)) { tempWW[1] = tempWW[1].Replace("\"", ""); commentBox.Text += tempWW[1] + "\r\n"; } // коментарий

                        if (line.IndexOf("XA=") >= 0)//XXX
                        {
                            textWW.Text += " -- " + countV + " -- ";
                            textWW.Text += tempWW[0] + "--->" + tempWW[1] + "\r\n";


                            for (I = 0; I < CountNameWW; I++)
                            {
                                if (tempWW[1].IndexOf(NAMEWW[I]) >= 0)//

                                {
                                    tempWW[1] = tempWW[1].Replace(NAMEWW[I], ZZWW[I]);
                                    tempWW[1] = tempWW[1].Replace('\"', ' ');

                                }



                            }

                            tempWW[1] = tempWW[1].Replace('\"', ' ');

                            var result = new DataTable().Compute(tempWW[1], null);
                            tempWW[1] = Convert.ToString(result);




                            Sv[countV].x0 = Convert.ToDouble(tempWW[1]);






                        }

                        if (line.IndexOf("YA=") == 0) //YYY
                        {
                            textWW.Text += " -- " + countV + " -- ";
                            textWW.Text += tempWW[0] + "--->" + tempWW[1] + "\r\n";


                            for (I = 0; I < CountNameWW; I++)
                            {
                                if (tempWW[1].IndexOf(NAMEWW[I]) >= 0)//

                                {
                                    tempWW[1] = tempWW[1].Replace(NAMEWW[I], ZZWW[I]);
                                    tempWW[1] = tempWW[1].Replace('\"', ' ');


                                }



                            }

                            tempWW[1] = tempWW[1].Replace("\"", "");

                            var result = new DataTable().Compute(tempWW[1], null);
                            tempWW[1] = Convert.ToString(result);




                            Sv[countV].y0 = Convert.ToDouble(tempWW[1]);




                        }

                        //-----------------------------------------------------------------
                        if (line.IndexOf("ZA=") == 0) //ZZ
                        {
                            textWW.Text += " -- " + countV + " -- ";
                            textWW.Text += tempWW[0] + "--->" + tempWW[1] + "\r\n";

                            //_BSZ
                            if (tempWW[1].IndexOf("_BSZ") >= 0) { tempWW[1] = tempWW[1].Replace("_BSZ", ZD.ToString()); }

                            for (I = 0; I < CountNameWW; I++)
                            {
                                if (tempWW[1].IndexOf(NAMEWW[I]) >= 0)//

                                {
                                    tempWW[1] = tempWW[1].Replace(NAMEWW[I], ZZWW[I]);
                                    tempWW[1] = tempWW[1].Replace('\"', ' ');


                                }



                            }

                            tempWW[1] = tempWW[1].Replace("\"", "");

                            var result = new DataTable().Compute(tempWW[1], null);
                            tempWW[1] = Convert.ToString(result);




                            Sv[countV].z0 = Convert.ToDouble(tempWW[1]);




                        }
                        //-----------------------------------------------------------------
                        //вход сверла                   
                        if (line.IndexOf("BM=") == 0)
                        {
                            textWW.Text += " -- " + countV + " -- ";
                            tempWW[1] = tempWW[1].Replace("\"", "");
                            Sv[countV].NAPR = tempWW[1];
                            if ((tempWW[1] == "LSL") || (tempWW[1] == "SSS")) { Sv[countV].g = Convert.ToDouble(ZD + 3); }

                            textWW.Text += tempWW[0] + "-вход сверла->" + Sv[countV].NAPR + "\r\n";
                        }
                        //диаметр
                        if (line.IndexOf("DU=") == 0)
                        {

                            textWW.Text += tempWW[0] + "-диаметр->" + tempWW[1] + "\r\n";
                            tempWW[1] = tempWW[1].Replace("\"", "");
                            Sv[countV].r = Convert.ToDouble(tempWW[1]) / 2;

                        }
                        //глубина
                        if (line.IndexOf("TI=") == 0)
                        {
                            textWW.Text += tempWW[0] + "-глубина-+++>" + tempWW[1] + "\r\n";
                            tempWW[1] = tempWW[1].Replace("\"", "");
                            Sv[countV].g = Convert.ToDouble(tempWW[1]);
                        }
                        //точка-база                    
                        if (line.IndexOf("KO=") == 0)
                        {
                            tempWW[1] = tempWW[1].Replace("\"", "");

                            var result = new DataTable().Compute(tempWW[1], null);
                            tempWW[1] = Convert.ToString(result);

                            Sv[countV].OO = tempWW[1];

                            textWW.Text += tempWW[0] + "-точка-база->" + tempWW[1] + "\r\n";



                            if (Sv[countV].OO == "1") { Sv[countV].x0 = XD - Sv[countV].x0; Sv[countV].OO = "3"; }
                            else if (Sv[countV].OO == "2") { Sv[countV].x0 = XD - Sv[countV].x0; Sv[countV].y0 = YD - Sv[countV].y0; }
                            else if (Sv[countV].OO == "3") { Sv[countV].y0 = YD - Sv[countV].y0; Sv[countV].OO = "1"; }







                        }
                        //-------------------------------------------------------------
                        if ((Sv[countV].x0 == XD) && (Sv[countV].OO == "0")) { Sv[countV].x0 = XD; Sv[countV].OO = "3"; }
                        else if ((Sv[countV].x0 == XD) && (Sv[countV].OO == "1")) { Sv[countV].x0 = XD; Sv[countV].OO = "2"; }
                        else if ((Sv[countV].x0 == 0) && (Sv[countV].OO == "2")) { Sv[countV].x0 = 0; Sv[countV].OO = "1"; }
                        else if ((Sv[countV].x0 == 0) && (Sv[countV].OO == "3")) { Sv[countV].x0 = 0; Sv[countV].OO = "0"; }

                        if ((Sv[countV].y0 == YD) && (Sv[countV].OO == "0")) { Sv[countV].y0 = YD; Sv[countV].OO = "1"; }
                        else if ((Sv[countV].y0 == YD) && (Sv[countV].OO == "3")) { Sv[countV].y0 = YD; Sv[countV].OO = "2"; }
                        else if ((Sv[countV].y0 == 0) && (Sv[countV].OO == "1")) { Sv[countV].y0 = 0; Sv[countV].OO = "0"; }
                        else if ((Sv[countV].y0 == 0) && (Sv[countV].OO == "2")) { Sv[countV].y0 = 0; Sv[countV].OO = "3"; }

                        //--------------------------------------------------------------
                        //количество свёрел 
                        if (line.IndexOf("AN=") == 0)
                        {
                            tempWW[1] = tempWW[1].Replace("\"", "");

                            Sv[countV].ANcol = Convert.ToInt32(tempWW[1]);
                            textWW.Text += tempWW[0] + "-количество свёрел->" + Sv[countV].ANcol + "\r\n";


                        }
                        //шаг
                        if (line.IndexOf("AB=") == 0)
                        {
                            for (I = 0; I < CountNameWW; I++)
                            {
                                if (tempWW[1].IndexOf(NAMEWW[I]) >= 0)//

                                {
                                    tempWW[1] = tempWW[1].Replace(NAMEWW[I], ZZWW[I]);
                                    tempWW[1] = tempWW[1].Replace('\"', ' ');


                                }



                            }



                            tempWW[1] = tempWW[1].Replace("\"", "");

                            var result = new DataTable().Compute(tempWW[1], null);

                            Sv[countV].ABstep = Convert.ToDouble(result);
                            textWW.Text += tempWW[0] + "-шаг->" + Sv[countV].ABstep + "\r\n";

                        }
                        //угол
                        if (line.IndexOf("WI=") == 0)
                        {
                            tempWW[1] = tempWW[1].Replace("\"", "");
                            Sv[countV].WIugol = Convert.ToDouble(tempWW[1]);
                            textWW.Text += tempWW[0] + "-угол->" + Sv[countV].WIugol + "\r\n";

                        }

                    }

                    /// var result = new DataTable().Compute("-80 + 496/2", null);

                    textWW.Text += countV + "\r\n";

                    textcontrol.Text += line + "\r\n";

                    sr.Close();
                    COUNT1 = countV;//количество сверел без учета шаговых

                    for (I = 1; I <= COUNT1; I++)
                    {

                        if (Sv[I].ANcol > 1)
                        {
                            for (int J = 0; J < Sv[I].ANcol - 1; J++)  //расставляем свёрла в шаг
                            {


                                countV++;
                                Sv[countV] = Sv[I];
                                textWW.Text += Sv[countV].NAPR + "\r\n";
                                if ((Sv[countV].NAPR == "XP") && ((Sv[countV].OO == "0") || (Sv[countV].OO == "3"))) { Sv[countV].y0 += (Sv[countV].ABstep * (J + 1)); }
                                if ((Sv[countV].NAPR == "XP") && ((Sv[countV].OO == "1") || (Sv[countV].OO == "2"))) { Sv[countV].y0 -= (Sv[countV].ABstep * (J + 1)); }

                                if ((Sv[countV].NAPR == "XM") && ((Sv[countV].OO == "3") || (Sv[countV].OO == "0"))) { Sv[countV].y0 += (Sv[countV].ABstep * (J + 1)); }
                                if ((Sv[countV].NAPR == "XM") && ((Sv[countV].OO == "2") || (Sv[countV].OO == "1"))) { Sv[countV].y0 -= (Sv[countV].ABstep * (J + 1)); }

                                if ((Sv[countV].NAPR == "YM") && ((Sv[countV].OO == "1") || (Sv[countV].OO == "0"))) { Sv[countV].x0 += (Sv[countV].ABstep * (J + 1)); }
                                if ((Sv[countV].NAPR == "YM") && ((Sv[countV].OO == "2") || (Sv[countV].OO == "3"))) { Sv[countV].x0 -= (Sv[countV].ABstep * (J + 1)); }

                                if ((Sv[countV].NAPR == "YP") && ((Sv[countV].OO == "0") || (Sv[countV].OO == "1"))) { Sv[countV].x0 += (Sv[countV].ABstep * (J + 1)); }
                                if ((Sv[countV].NAPR == "YP") && ((Sv[countV].OO == "3") || (Sv[countV].OO == "2"))) { Sv[countV].x0 -= (Sv[countV].ABstep * (J + 1)); }



                                //вертикальное сверление
                                if (Sv[countV].vid == 1)
                                {

                                    if (Sv[countV].WIugol < 0) Sv[countV].WIugol += 360;

                                    if (Sv[countV].OO == "0")
                                    {
                                        if (Sv[countV].WIugol == 0) { Sv[countV].x0 += (Sv[countV].ABstep * (J + 1)); }
                                        if (Sv[countV].WIugol == 180) { Sv[countV].x0 -= (Sv[countV].ABstep * (J + 1)); }

                                        if (Sv[countV].WIugol == 90) { Sv[countV].y0 += (Sv[countV].ABstep * (J + 1)); }
                                        if (Sv[countV].WIugol == 270) { Sv[countV].y0 -= (Sv[countV].ABstep * (J + 1)); }

                                    }

                                    if (Sv[countV].OO == "1")
                                    {
                                        if (Sv[countV].WIugol == 0) { Sv[countV].x0 += (Sv[countV].ABstep * (J + 1)); }
                                        if (Sv[countV].WIugol == 180) { Sv[countV].x0 -= (Sv[countV].ABstep * (J + 1)); }

                                        if (Sv[countV].WIugol == 90) { Sv[countV].y0 -= (Sv[countV].ABstep * (J + 1)); }
                                        if (Sv[countV].WIugol == 270) { Sv[countV].y0 += (Sv[countV].ABstep * (J + 1)); }


                                    }

                                    if (Sv[countV].OO == "2")
                                    {
                                        if (Sv[countV].WIugol == 0) { Sv[countV].x0 -= (Sv[countV].ABstep * (J + 1)); }
                                        if (Sv[countV].WIugol == 180) { Sv[countV].x0 += (Sv[countV].ABstep * (J + 1)); }

                                        if (Sv[countV].WIugol == 90) { Sv[countV].y0 -= (Sv[countV].ABstep * (J + 1)); }
                                        if (Sv[countV].WIugol == 270) { Sv[countV].y0 += (Sv[countV].ABstep * (J + 1)); }

                                    }

                                    if (Sv[countV].OO == "3")
                                    {
                                        if (Sv[countV].WIugol == 0) { Sv[countV].x0 -= (Sv[countV].ABstep * (J + 1)); }
                                        if (Sv[countV].WIugol == 180) { Sv[countV].x0 += (Sv[countV].ABstep * (J + 1)); }

                                        if (Sv[countV].WIugol == 90) { Sv[countV].y0 += (Sv[countV].ABstep * (J + 1)); }
                                        if (Sv[countV].WIugol == 270) { Sv[countV].y0 -= (Sv[countV].ABstep * (J + 1)); }

                                    }

                                }
                            }

                        }


                    }

                    textWW.Visible = false;
                    textcontrol.Visible = false;

                    if (textBoxXD.Text == "") { return; }
                    if (textBoxYD.Text == "") { return; }
                    if (textBoxZD.Text == "") { return; }



                    workdraw();
                    drawsv();

                    if (countV > 0) // ставим указатель на 1 сверло если есть
                    {

                        label1.Text = "1";
                        Sv[1].edit = 1;
                        smena();
                        infosv();
                        deleteSv.Enabled = true;
                    }


                }
            }
        }







    }

}
