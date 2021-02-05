namespace OMPlot
{
    partial class Plot
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Plot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "Plot";
            this.Size = new System.Drawing.Size(800, 450);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Plot_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Plot_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Plot_MouseDoubleClick);
            this.MouseCaptureChanged += new System.EventHandler(this.Plot_MouseCaptureChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Plot_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Plot_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Plot_MouseUp);
            this.Resize += new System.EventHandler(this.Control_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
