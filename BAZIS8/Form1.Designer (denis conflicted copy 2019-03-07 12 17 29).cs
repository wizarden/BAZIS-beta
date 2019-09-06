namespace BAZIS8
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textcontrol = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelD = new System.Windows.Forms.Label();
            this.labelG = new System.Windows.Forms.Label();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.textBoxD = new System.Windows.Forms.TextBox();
            this.textBoxG = new System.Windows.Forms.TextBox();
            this.rotateL = new System.Windows.Forms.Button();
            this.rotateR = new System.Windows.Forms.Button();
            this.label0 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBoxYD = new System.Windows.Forms.TextBox();
            this.textBoxXD = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelXD = new System.Windows.Forms.Label();
            this.textBoxZD = new System.Windows.Forms.TextBox();
            this.labelYD = new System.Windows.Forms.Label();
            this.SvPrev = new System.Windows.Forms.Button();
            this.SvNext = new System.Windows.Forms.Button();
            this.saveWW = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.FileName = new System.Windows.Forms.TextBox();
            this.labelD3 = new System.Windows.Forms.Label();
            this.textBoxD3 = new System.Windows.Forms.TextBox();
            this.textBoxD5 = new System.Windows.Forms.TextBox();
            this.labelD5 = new System.Windows.Forms.Label();
            this.textBoxD8 = new System.Windows.Forms.TextBox();
            this.labelD8 = new System.Windows.Forms.Label();
            this.textBoxD15 = new System.Windows.Forms.TextBox();
            this.labelD15 = new System.Windows.Forms.Label();
            this.MirrorX = new System.Windows.Forms.Button();
            this.MirrorY = new System.Windows.Forms.Button();
            this.saveCad4 = new System.Windows.Forms.Button();
            this.deleteSv = new System.Windows.Forms.Button();
            this.AddSv = new System.Windows.Forms.Button();
            this.AddDim = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.commentBox = new System.Windows.Forms.TextBox();
            this.GUpDown = new System.Windows.Forms.NumericUpDown();
            this.button11 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.button33 = new System.Windows.Forms.Button();
            this.button44 = new System.Windows.Forms.Button();
            this.OPN = new System.Windows.Forms.Button();
            this.SKV = new System.Windows.Forms.Button();
            this.OWW = new System.Windows.Forms.Button();
            this.openFileWW = new System.Windows.Forms.OpenFileDialog();
            this.textWW = new System.Windows.Forms.TextBox();
            this.help_button = new System.Windows.Forms.Button();
            this.helptext = new System.Windows.Forms.TextBox();
            this.OPNCAD4 = new System.Windows.Forms.Button();
            this.FileDroplistBox = new System.Windows.Forms.ListBox();
            this.PD4toMPR = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.DropListClear = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxZ = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.saveBPP = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SaveBPPall = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textcontrol
            // 
            this.textcontrol.Location = new System.Drawing.Point(384, 538);
            this.textcontrol.Multiline = true;
            this.textcontrol.Name = "textcontrol";
            this.textcontrol.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textcontrol.ShortcutsEnabled = false;
            this.textcontrol.Size = new System.Drawing.Size(494, 74);
            this.textcontrol.TabIndex = 1;
            this.textcontrol.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Arial Unicode MS", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(935, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "0";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Leave += new System.EventHandler(this.label1_Leave);
            // 
            // labelX
            // 
            this.labelX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelX.AutoSize = true;
            this.labelX.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX.Location = new System.Drawing.Point(870, 133);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(26, 25);
            this.labelX.TabIndex = 3;
            this.labelX.Text = "X";
            // 
            // labelY
            // 
            this.labelY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelY.AutoSize = true;
            this.labelY.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelY.Location = new System.Drawing.Point(870, 175);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(26, 25);
            this.labelY.TabIndex = 4;
            this.labelY.Text = "Y";
            // 
            // labelD
            // 
            this.labelD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelD.AutoSize = true;
            this.labelD.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelD.Location = new System.Drawing.Point(870, 257);
            this.labelD.Name = "labelD";
            this.labelD.Size = new System.Drawing.Size(27, 25);
            this.labelD.TabIndex = 5;
            this.labelD.Text = "D";
            // 
            // labelG
            // 
            this.labelG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelG.AutoSize = true;
            this.labelG.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelG.Location = new System.Drawing.Point(869, 290);
            this.labelG.Name = "labelG";
            this.labelG.Size = new System.Drawing.Size(28, 25);
            this.labelG.TabIndex = 6;
            this.labelG.Text = "G";
            // 
            // textBoxX
            // 
            this.textBoxX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxX.BackColor = System.Drawing.Color.White;
            this.textBoxX.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxX.ForeColor = System.Drawing.Color.Red;
            this.textBoxX.Location = new System.Drawing.Point(899, 129);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(80, 33);
            this.textBoxX.TabIndex = 7;
            this.textBoxX.TextChanged += new System.EventHandler(this.textBoxX_TextChanged);
            this.textBoxX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterOnlyReal);
            // 
            // textBoxY
            // 
            this.textBoxY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxY.BackColor = System.Drawing.Color.White;
            this.textBoxY.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxY.ForeColor = System.Drawing.Color.Red;
            this.textBoxY.Location = new System.Drawing.Point(899, 171);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(80, 33);
            this.textBoxY.TabIndex = 8;
            this.textBoxY.TextChanged += new System.EventHandler(this.textBoxY_TextChanged);
            this.textBoxY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterOnlyReal);
            // 
            // textBoxD
            // 
            this.textBoxD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxD.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxD.Location = new System.Drawing.Point(900, 256);
            this.textBoxD.Name = "textBoxD";
            this.textBoxD.Size = new System.Drawing.Size(63, 25);
            this.textBoxD.TabIndex = 9;
            this.textBoxD.TextChanged += new System.EventHandler(this.textBoxD_TextChanged);
            this.textBoxD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterOnlyReal);
            // 
            // textBoxG
            // 
            this.textBoxG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxG.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxG.Location = new System.Drawing.Point(900, 287);
            this.textBoxG.Name = "textBoxG";
            this.textBoxG.Size = new System.Drawing.Size(68, 25);
            this.textBoxG.TabIndex = 10;
            this.textBoxG.TextChanged += new System.EventHandler(this.textBoxG_TextChanged);
            this.textBoxG.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterOnlyReal);
            // 
            // rotateL
            // 
            this.rotateL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rotateL.Enabled = false;
            this.rotateL.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rotateL.Location = new System.Drawing.Point(458, 629);
            this.rotateL.Name = "rotateL";
            this.rotateL.Size = new System.Drawing.Size(86, 29);
            this.rotateL.TabIndex = 11;
            this.rotateL.Text = "<<";
            this.rotateL.UseVisualStyleBackColor = true;
            this.rotateL.Click += new System.EventHandler(this.rotateL_Click);
            // 
            // rotateR
            // 
            this.rotateR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rotateR.Enabled = false;
            this.rotateR.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rotateR.Location = new System.Drawing.Point(559, 629);
            this.rotateR.Name = "rotateR";
            this.rotateR.Size = new System.Drawing.Size(86, 29);
            this.rotateR.TabIndex = 12;
            this.rotateR.Text = ">>";
            this.rotateR.UseVisualStyleBackColor = true;
            this.rotateR.Click += new System.EventHandler(this.rotateR_Click);
            // 
            // label0
            // 
            this.label0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label0.AutoSize = true;
            this.label0.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label0.Location = new System.Drawing.Point(1010, 173);
            this.label0.Name = "label0";
            this.label0.Size = new System.Drawing.Size(30, 39);
            this.label0.TabIndex = 13;
            this.label0.Text = "-";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Turquoise;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(886, 574);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 30);
            this.button1.TabIndex = 14;
            this.button1.Text = "0";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.Turquoise;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(886, 538);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 30);
            this.button2.TabIndex = 15;
            this.button2.Text = "1";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.Turquoise;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(917, 538);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(30, 30);
            this.button3.TabIndex = 16;
            this.button3.Text = "2";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.BackColor = System.Drawing.Color.Turquoise;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.Location = new System.Drawing.Point(917, 574);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(30, 30);
            this.button4.TabIndex = 17;
            this.button4.Text = "3";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBoxYD
            // 
            this.textBoxYD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxYD.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxYD.Location = new System.Drawing.Point(917, 631);
            this.textBoxYD.Name = "textBoxYD";
            this.textBoxYD.Size = new System.Drawing.Size(80, 25);
            this.textBoxYD.TabIndex = 21;
            this.textBoxYD.TextChanged += new System.EventHandler(this.textBoxYD_TextChanged);
            this.textBoxYD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterOnlyReal);
            // 
            // textBoxXD
            // 
            this.textBoxXD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxXD.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxXD.Location = new System.Drawing.Point(798, 631);
            this.textBoxXD.Name = "textBoxXD";
            this.textBoxXD.Size = new System.Drawing.Size(80, 25);
            this.textBoxXD.TabIndex = 20;
            this.textBoxXD.TextChanged += new System.EventHandler(this.textBoxXD_TextChanged);
            this.textBoxXD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterOnlyReal);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(1005, 634);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 18);
            this.label3.TabIndex = 23;
            this.label3.Text = "Z:";
            // 
            // labelXD
            // 
            this.labelXD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelXD.AutoSize = true;
            this.labelXD.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelXD.Location = new System.Drawing.Point(767, 634);
            this.labelXD.Name = "labelXD";
            this.labelXD.Size = new System.Drawing.Size(23, 18);
            this.labelXD.TabIndex = 18;
            this.labelXD.Text = "X:";
            // 
            // textBoxZD
            // 
            this.textBoxZD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxZD.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxZD.Location = new System.Drawing.Point(1035, 631);
            this.textBoxZD.Name = "textBoxZD";
            this.textBoxZD.Size = new System.Drawing.Size(80, 25);
            this.textBoxZD.TabIndex = 24;
            this.textBoxZD.Text = "16";
            this.textBoxZD.TextChanged += new System.EventHandler(this.textBoxZD_TextChanged);
            this.textBoxZD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterOnlyReal);
            // 
            // labelYD
            // 
            this.labelYD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelYD.AutoSize = true;
            this.labelYD.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelYD.Location = new System.Drawing.Point(886, 634);
            this.labelYD.Name = "labelYD";
            this.labelYD.Size = new System.Drawing.Size(23, 18);
            this.labelYD.TabIndex = 19;
            this.labelYD.Text = "Y:";
            // 
            // SvPrev
            // 
            this.SvPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SvPrev.BackColor = System.Drawing.Color.DodgerBlue;
            this.SvPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SvPrev.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SvPrev.Location = new System.Drawing.Point(900, 48);
            this.SvPrev.Name = "SvPrev";
            this.SvPrev.Size = new System.Drawing.Size(29, 66);
            this.SvPrev.TabIndex = 25;
            this.SvPrev.Text = "<";
            this.SvPrev.UseVisualStyleBackColor = false;
            this.SvPrev.Click += new System.EventHandler(this.SvPrev_Click);
            // 
            // SvNext
            // 
            this.SvNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SvNext.BackColor = System.Drawing.Color.DodgerBlue;
            this.SvNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SvNext.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SvNext.Location = new System.Drawing.Point(984, 48);
            this.SvNext.Name = "SvNext";
            this.SvNext.Size = new System.Drawing.Size(29, 66);
            this.SvNext.TabIndex = 26;
            this.SvNext.Text = ">";
            this.SvNext.UseVisualStyleBackColor = false;
            this.SvNext.Click += new System.EventHandler(this.SvNext_Click);
            // 
            // saveWW
            // 
            this.saveWW.BackColor = System.Drawing.Color.OrangeRed;
            this.saveWW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveWW.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.saveWW.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.saveWW.Location = new System.Drawing.Point(322, 4);
            this.saveWW.Name = "saveWW";
            this.saveWW.Size = new System.Drawing.Size(100, 30);
            this.saveWW.TabIndex = 27;
            this.saveWW.Text = "Save WW";
            this.saveWW.UseVisualStyleBackColor = false;
            this.saveWW.Click += new System.EventHandler(this.saveWW_Click);
            // 
            // FileName
            // 
            this.FileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileName.BackColor = System.Drawing.Color.PowderBlue;
            this.FileName.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FileName.Location = new System.Drawing.Point(559, 4);
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(338, 29);
            this.FileName.TabIndex = 28;
            this.FileName.Tag = "";
            this.FileName.Enter += new System.EventHandler(this.FileName_Enter);
            // 
            // labelD3
            // 
            this.labelD3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelD3.AutoSize = true;
            this.labelD3.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelD3.Location = new System.Drawing.Point(151, 604);
            this.labelD3.Name = "labelD3";
            this.labelD3.Size = new System.Drawing.Size(33, 21);
            this.labelD3.TabIndex = 30;
            this.labelD3.Text = "Ø3";
            this.labelD3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelD3.Visible = false;
            // 
            // textBoxD3
            // 
            this.textBoxD3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxD3.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxD3.Location = new System.Drawing.Point(182, 600);
            this.textBoxD3.Name = "textBoxD3";
            this.textBoxD3.Size = new System.Drawing.Size(31, 29);
            this.textBoxD3.TabIndex = 31;
            this.textBoxD3.Text = "3";
            this.textBoxD3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxD3.Visible = false;
            this.textBoxD3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterOnlyReal);
            // 
            // textBoxD5
            // 
            this.textBoxD5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxD5.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxD5.Location = new System.Drawing.Point(182, 636);
            this.textBoxD5.Name = "textBoxD5";
            this.textBoxD5.Size = new System.Drawing.Size(31, 29);
            this.textBoxD5.TabIndex = 33;
            this.textBoxD5.Text = "12";
            this.textBoxD5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxD5.Visible = false;
            this.textBoxD5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterOnlyReal);
            // 
            // labelD5
            // 
            this.labelD5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelD5.AutoSize = true;
            this.labelD5.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelD5.Location = new System.Drawing.Point(151, 640);
            this.labelD5.Name = "labelD5";
            this.labelD5.Size = new System.Drawing.Size(33, 21);
            this.labelD5.TabIndex = 32;
            this.labelD5.Text = "Ø5";
            this.labelD5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelD5.Visible = false;
            // 
            // textBoxD8
            // 
            this.textBoxD8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxD8.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxD8.Location = new System.Drawing.Point(271, 600);
            this.textBoxD8.Name = "textBoxD8";
            this.textBoxD8.Size = new System.Drawing.Size(31, 29);
            this.textBoxD8.TabIndex = 35;
            this.textBoxD8.Text = "10";
            this.textBoxD8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxD8.Visible = false;
            this.textBoxD8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterOnlyReal);
            // 
            // labelD8
            // 
            this.labelD8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelD8.AutoSize = true;
            this.labelD8.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelD8.Location = new System.Drawing.Point(240, 604);
            this.labelD8.Name = "labelD8";
            this.labelD8.Size = new System.Drawing.Size(33, 21);
            this.labelD8.TabIndex = 34;
            this.labelD8.Text = "Ø8";
            this.labelD8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelD8.Visible = false;
            // 
            // textBoxD15
            // 
            this.textBoxD15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxD15.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxD15.Location = new System.Drawing.Point(271, 636);
            this.textBoxD15.Name = "textBoxD15";
            this.textBoxD15.Size = new System.Drawing.Size(31, 29);
            this.textBoxD15.TabIndex = 37;
            this.textBoxD15.Text = "13";
            this.textBoxD15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxD15.Visible = false;
            this.textBoxD15.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterOnlyReal);
            // 
            // labelD15
            // 
            this.labelD15.AllowDrop = true;
            this.labelD15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelD15.AutoSize = true;
            this.labelD15.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelD15.Location = new System.Drawing.Point(230, 640);
            this.labelD15.Name = "labelD15";
            this.labelD15.Size = new System.Drawing.Size(43, 21);
            this.labelD15.TabIndex = 36;
            this.labelD15.Tag = "";
            this.labelD15.Text = "Ø15";
            this.labelD15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelD15.Visible = false;
            // 
            // MirrorX
            // 
            this.MirrorX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MirrorX.Enabled = false;
            this.MirrorX.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MirrorX.Location = new System.Drawing.Point(357, 629);
            this.MirrorX.Name = "MirrorX";
            this.MirrorX.Size = new System.Drawing.Size(86, 29);
            this.MirrorX.TabIndex = 39;
            this.MirrorX.Text = "MirrorX";
            this.MirrorX.UseVisualStyleBackColor = true;
            this.MirrorX.Click += new System.EventHandler(this.MirrorX_Click);
            // 
            // MirrorY
            // 
            this.MirrorY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MirrorY.Enabled = false;
            this.MirrorY.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MirrorY.Location = new System.Drawing.Point(661, 629);
            this.MirrorY.Name = "MirrorY";
            this.MirrorY.Size = new System.Drawing.Size(86, 29);
            this.MirrorY.TabIndex = 40;
            this.MirrorY.Text = "MirrorY";
            this.MirrorY.UseVisualStyleBackColor = true;
            this.MirrorY.Click += new System.EventHandler(this.MirrorY_Click);
            // 
            // saveCad4
            // 
            this.saveCad4.BackColor = System.Drawing.Color.OrangeRed;
            this.saveCad4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveCad4.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.saveCad4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.saveCad4.Location = new System.Drawing.Point(428, 4);
            this.saveCad4.Name = "saveCad4";
            this.saveCad4.Size = new System.Drawing.Size(100, 30);
            this.saveCad4.TabIndex = 41;
            this.saveCad4.Text = "Save CAD/4";
            this.saveCad4.UseVisualStyleBackColor = false;
            this.saveCad4.Click += new System.EventHandler(this.saveCad4_Click);
            // 
            // deleteSv
            // 
            this.deleteSv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteSv.BackColor = System.Drawing.Color.Red;
            this.deleteSv.Enabled = false;
            this.deleteSv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteSv.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deleteSv.Location = new System.Drawing.Point(50, 620);
            this.deleteSv.Name = "deleteSv";
            this.deleteSv.Size = new System.Drawing.Size(40, 40);
            this.deleteSv.TabIndex = 42;
            this.deleteSv.Text = "X";
            this.deleteSv.UseVisualStyleBackColor = false;
            this.deleteSv.Click += new System.EventHandler(this.deleteSv_Click);
            // 
            // AddSv
            // 
            this.AddSv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddSv.BackColor = System.Drawing.Color.LimeGreen;
            this.AddSv.Enabled = false;
            this.AddSv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddSv.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddSv.Location = new System.Drawing.Point(4, 620);
            this.AddSv.Name = "AddSv";
            this.AddSv.Size = new System.Drawing.Size(40, 40);
            this.AddSv.TabIndex = 43;
            this.AddSv.Text = "+";
            this.AddSv.UseVisualStyleBackColor = false;
            this.AddSv.Click += new System.EventHandler(this.AddSv_Click);
            // 
            // AddDim
            // 
            this.AddDim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddDim.BackColor = System.Drawing.Color.YellowGreen;
            this.AddDim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddDim.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddDim.Location = new System.Drawing.Point(1130, 625);
            this.AddDim.Name = "AddDim";
            this.AddDim.Size = new System.Drawing.Size(121, 36);
            this.AddDim.TabIndex = 44;
            this.AddDim.Text = "New Panel";
            this.AddDim.UseVisualStyleBackColor = false;
            this.AddDim.Click += new System.EventHandler(this.AddDim_Click);
            // 
            // commentBox
            // 
            this.commentBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commentBox.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.commentBox.Location = new System.Drawing.Point(1055, 363);
            this.commentBox.Multiline = true;
            this.commentBox.Name = "commentBox";
            this.commentBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.commentBox.Size = new System.Drawing.Size(196, 249);
            this.commentBox.TabIndex = 52;
            this.toolTip1.SetToolTip(this.commentBox, "Коментарий");
            // 
            // GUpDown
            // 
            this.GUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GUpDown.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GUpDown.Location = new System.Drawing.Point(962, 288);
            this.GUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.GUpDown.Name = "GUpDown";
            this.GUpDown.Size = new System.Drawing.Size(19, 25);
            this.GUpDown.TabIndex = 45;
            this.GUpDown.ValueChanged += new System.EventHandler(this.GUpDown_ValueChanged);
            // 
            // button11
            // 
            this.button11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button11.Enabled = false;
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button11.Location = new System.Drawing.Point(868, 83);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(25, 31);
            this.button11.TabIndex = 46;
            this.button11.Text = "0";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button22
            // 
            this.button22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button22.Enabled = false;
            this.button22.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button22.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button22.Location = new System.Drawing.Point(868, 48);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(25, 31);
            this.button22.TabIndex = 47;
            this.button22.Text = "1";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new System.EventHandler(this.button22_Click);
            // 
            // button33
            // 
            this.button33.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button33.Enabled = false;
            this.button33.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button33.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button33.Location = new System.Drawing.Point(1018, 48);
            this.button33.Name = "button33";
            this.button33.Size = new System.Drawing.Size(25, 31);
            this.button33.TabIndex = 48;
            this.button33.Text = "2";
            this.button33.UseVisualStyleBackColor = true;
            this.button33.Click += new System.EventHandler(this.button33_Click);
            // 
            // button44
            // 
            this.button44.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button44.Enabled = false;
            this.button44.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button44.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button44.Location = new System.Drawing.Point(1018, 83);
            this.button44.Name = "button44";
            this.button44.Size = new System.Drawing.Size(25, 31);
            this.button44.TabIndex = 49;
            this.button44.Text = "3";
            this.button44.UseVisualStyleBackColor = true;
            this.button44.Click += new System.EventHandler(this.button44_Click);
            // 
            // OPN
            // 
            this.OPN.BackColor = System.Drawing.SystemColors.Highlight;
            this.OPN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OPN.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OPN.Location = new System.Drawing.Point(4, 4);
            this.OPN.Name = "OPN";
            this.OPN.Size = new System.Drawing.Size(100, 30);
            this.OPN.TabIndex = 50;
            this.OPN.Text = "Open DXF";
            this.OPN.UseVisualStyleBackColor = false;
            this.OPN.Click += new System.EventHandler(this.OPN_Click);
            // 
            // SKV
            // 
            this.SKV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SKV.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.SKV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SKV.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SKV.Location = new System.Drawing.Point(988, 287);
            this.SKV.Name = "SKV";
            this.SKV.Size = new System.Drawing.Size(43, 26);
            this.SKV.TabIndex = 51;
            this.SKV.Text = "B/T";
            this.SKV.UseVisualStyleBackColor = false;
            this.SKV.Click += new System.EventHandler(this.SKV_Click);
            // 
            // OWW
            // 
            this.OWW.BackColor = System.Drawing.Color.LimeGreen;
            this.OWW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OWW.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OWW.Location = new System.Drawing.Point(110, 4);
            this.OWW.Name = "OWW";
            this.OWW.Size = new System.Drawing.Size(100, 30);
            this.OWW.TabIndex = 53;
            this.OWW.Text = "OPEN WW4";
            this.OWW.UseVisualStyleBackColor = false;
            this.OWW.Click += new System.EventHandler(this.OWW_Click);
            // 
            // openFileWW
            // 
            this.openFileWW.FileName = "openFileDialog1";
            // 
            // textWW
            // 
            this.textWW.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textWW.Location = new System.Drawing.Point(7, 306);
            this.textWW.Multiline = true;
            this.textWW.Name = "textWW";
            this.textWW.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textWW.Size = new System.Drawing.Size(220, 258);
            this.textWW.TabIndex = 54;
            this.textWW.Visible = false;
            // 
            // help_button
            // 
            this.help_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.help_button.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.help_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.help_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.help_button.Location = new System.Drawing.Point(1008, 4);
            this.help_button.Name = "help_button";
            this.help_button.Size = new System.Drawing.Size(41, 30);
            this.help_button.TabIndex = 55;
            this.help_button.Text = "?";
            this.help_button.UseVisualStyleBackColor = false;
            this.help_button.Click += new System.EventHandler(this.help_button_Click);
            // 
            // helptext
            // 
            this.helptext.Location = new System.Drawing.Point(378, 149);
            this.helptext.Multiline = true;
            this.helptext.Name = "helptext";
            this.helptext.Size = new System.Drawing.Size(321, 289);
            this.helptext.TabIndex = 56;
            this.helptext.Text = "Горячие клавиши\r\n\r\nAlt+X - удалить свело\r\nAlt+G - глухое/сквозное\r\n\r\nEsc - убрать" +
    " фокус с текстбоксов\r\n\r\n\r\nпереместить сверло\r\n\r\nA - влево X--\r\nD -вправо X++\r\nW " +
    "- вверх Y++\r\nS -вниз Y--";
            this.helptext.Visible = false;
            // 
            // OPNCAD4
            // 
            this.OPNCAD4.BackColor = System.Drawing.Color.LimeGreen;
            this.OPNCAD4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OPNCAD4.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OPNCAD4.Location = new System.Drawing.Point(216, 4);
            this.OPNCAD4.Name = "OPNCAD4";
            this.OPNCAD4.Size = new System.Drawing.Size(100, 30);
            this.OPNCAD4.TabIndex = 57;
            this.OPNCAD4.Text = "OPEN Cad/4";
            this.OPNCAD4.UseVisualStyleBackColor = false;
            this.OPNCAD4.Click += new System.EventHandler(this.OPNCAD4_Click);
            // 
            // FileDroplistBox
            // 
            this.FileDroplistBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FileDroplistBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FileDroplistBox.FormattingEnabled = true;
            this.FileDroplistBox.Location = new System.Drawing.Point(1055, 38);
            this.FileDroplistBox.Name = "FileDroplistBox";
            this.FileDroplistBox.Size = new System.Drawing.Size(196, 290);
            this.FileDroplistBox.TabIndex = 58;
            this.FileDroplistBox.SelectedValueChanged += new System.EventHandler(this.FileDroplistBox_DoubleClick);
            // 
            // PD4toMPR
            // 
            this.PD4toMPR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PD4toMPR.Location = new System.Drawing.Point(1055, 334);
            this.PD4toMPR.Name = "PD4toMPR";
            this.PD4toMPR.Size = new System.Drawing.Size(196, 23);
            this.PD4toMPR.TabIndex = 59;
            this.PD4toMPR.Text = "HIRZT<>WoodWOP";
            this.PD4toMPR.UseVisualStyleBackColor = true;
            this.PD4toMPR.Click += new System.EventHandler(this.PD4toMPR_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(1060, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 16);
            this.label2.TabIndex = 60;
            this.label2.Text = "Drag\'n\'Drop File List";
            // 
            // DropListClear
            // 
            this.DropListClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DropListClear.BackColor = System.Drawing.Color.Red;
            this.DropListClear.Location = new System.Drawing.Point(1221, 8);
            this.DropListClear.Name = "DropListClear";
            this.DropListClear.Size = new System.Drawing.Size(30, 27);
            this.DropListClear.TabIndex = 61;
            this.DropListClear.UseVisualStyleBackColor = false;
            this.DropListClear.Click += new System.EventHandler(this.DropListClear_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(934, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 16);
            this.label4.TabIndex = 62;
            this.label4.Text = "M:";
            // 
            // textBoxZ
            // 
            this.textBoxZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxZ.BackColor = System.Drawing.Color.White;
            this.textBoxZ.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxZ.ForeColor = System.Drawing.Color.Red;
            this.textBoxZ.Location = new System.Drawing.Point(899, 211);
            this.textBoxZ.Name = "textBoxZ";
            this.textBoxZ.Size = new System.Drawing.Size(80, 33);
            this.textBoxZ.TabIndex = 63;
            this.textBoxZ.TextChanged += new System.EventHandler(this.textBoxZ_TextChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(871, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 25);
            this.label5.TabIndex = 64;
            this.label5.Text = "Z";
            // 
            // saveBPP
            // 
            this.saveBPP.BackColor = System.Drawing.Color.OrangeRed;
            this.saveBPP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveBPP.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.saveBPP.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.saveBPP.Location = new System.Drawing.Point(421, 462);
            this.saveBPP.Name = "saveBPP";
            this.saveBPP.Size = new System.Drawing.Size(150, 35);
            this.saveBPP.TabIndex = 65;
            this.saveBPP.Text = "Save BPP";
            this.saveBPP.UseVisualStyleBackColor = false;
            this.saveBPP.Visible = false;
            this.saveBPP.Click += new System.EventHandler(this.saveBPP_Click);
            // 
            // SaveBPPall
            // 
            this.SaveBPPall.BackColor = System.Drawing.Color.OrangeRed;
            this.SaveBPPall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveBPPall.Font = new System.Drawing.Font("Arial Unicode MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SaveBPPall.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.SaveBPPall.Location = new System.Drawing.Point(233, 462);
            this.SaveBPPall.Name = "SaveBPPall";
            this.SaveBPPall.Size = new System.Drawing.Size(150, 35);
            this.SaveBPPall.TabIndex = 66;
            this.SaveBPPall.Text = "Save BPP All";
            this.SaveBPPall.UseVisualStyleBackColor = false;
            this.SaveBPPall.Visible = false;
            this.SaveBPPall.Click += new System.EventHandler(this.SaveBPPall_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1252, 661);
            this.Controls.Add(this.SaveBPPall);
            this.Controls.Add(this.saveBPP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxZ);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DropListClear);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PD4toMPR);
            this.Controls.Add(this.FileDroplistBox);
            this.Controls.Add(this.OPNCAD4);
            this.Controls.Add(this.helptext);
            this.Controls.Add(this.help_button);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.textWW);
            this.Controls.Add(this.OWW);
            this.Controls.Add(this.commentBox);
            this.Controls.Add(this.SKV);
            this.Controls.Add(this.OPN);
            this.Controls.Add(this.button44);
            this.Controls.Add(this.button33);
            this.Controls.Add(this.button22);
            this.Controls.Add(this.GUpDown);
            this.Controls.Add(this.AddDim);
            this.Controls.Add(this.AddSv);
            this.Controls.Add(this.deleteSv);
            this.Controls.Add(this.saveCad4);
            this.Controls.Add(this.MirrorY);
            this.Controls.Add(this.MirrorX);
            this.Controls.Add(this.textBoxD15);
            this.Controls.Add(this.labelD15);
            this.Controls.Add(this.textBoxD8);
            this.Controls.Add(this.labelD8);
            this.Controls.Add(this.textBoxD5);
            this.Controls.Add(this.labelD5);
            this.Controls.Add(this.textBoxD3);
            this.Controls.Add(this.labelD3);
            this.Controls.Add(this.FileName);
            this.Controls.Add(this.saveWW);
            this.Controls.Add(this.SvNext);
            this.Controls.Add(this.SvPrev);
            this.Controls.Add(this.labelYD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxZD);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.labelXD);
            this.Controls.Add(this.textBoxXD);
            this.Controls.Add(this.textBoxYD);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label0);
            this.Controls.Add(this.rotateR);
            this.Controls.Add(this.rotateL);
            this.Controls.Add(this.textBoxG);
            this.Controls.Add(this.textBoxD);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.labelG);
            this.Controls.Add(this.labelD);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textcontrol);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1150, 500);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "FCNC 2018";
            this.Load += new System.EventHandler(this.init);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.GUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textcontrol;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelD;
        private System.Windows.Forms.Label labelG;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.TextBox textBoxD;
        private System.Windows.Forms.TextBox textBoxG;
        private System.Windows.Forms.Button rotateL;
        private System.Windows.Forms.Button rotateR;
        private System.Windows.Forms.Label label0;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBoxYD;
        private System.Windows.Forms.TextBox textBoxXD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelXD;
        private System.Windows.Forms.TextBox textBoxZD;
        private System.Windows.Forms.Label labelYD;
        private System.Windows.Forms.Button SvPrev;
        private System.Windows.Forms.Button SvNext;
        private System.Windows.Forms.Button saveWW;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label labelD3;
        private System.Windows.Forms.TextBox textBoxD3;
        private System.Windows.Forms.TextBox textBoxD5;
        private System.Windows.Forms.Label labelD5;
        private System.Windows.Forms.TextBox textBoxD8;
        private System.Windows.Forms.Label labelD8;
        private System.Windows.Forms.TextBox textBoxD15;
        private System.Windows.Forms.Label labelD15;
        private System.Windows.Forms.Button MirrorX;
        private System.Windows.Forms.Button MirrorY;
        private System.Windows.Forms.Button saveCad4;
        private System.Windows.Forms.Button deleteSv;
        private System.Windows.Forms.Button AddSv;
        private System.Windows.Forms.Button AddDim;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown GUpDown;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Button button33;
        private System.Windows.Forms.Button button44;
        private System.Windows.Forms.Button OPN;
        private System.Windows.Forms.Button SKV;
        private System.Windows.Forms.TextBox commentBox;
        private System.Windows.Forms.Button OWW;
        private System.Windows.Forms.OpenFileDialog openFileWW;
        private System.Windows.Forms.TextBox textWW;
        public System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.Button help_button;
        private System.Windows.Forms.TextBox helptext;
        private System.Windows.Forms.Button OPNCAD4;
        private System.Windows.Forms.ListBox FileDroplistBox;
        private System.Windows.Forms.Button PD4toMPR;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DropListClear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxZ;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button saveBPP;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button SaveBPPall;
    }
}

