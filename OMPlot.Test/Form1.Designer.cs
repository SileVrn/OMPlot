namespace OMPlot.Test
{
    partial class Form1
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.plot1 = new OMPlot.Plot();
            this.SuspendLayout();
            // 
            // plot1
            // 
            this.plot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plot1.Location = new System.Drawing.Point(0, 0);
            this.plot1.Name = "plot1";
            this.plot1.Size = new System.Drawing.Size(1056, 631);
            this.plot1.TabIndex = 0;
            this.plot1.Title = null;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1056, 631);
            this.Controls.Add(this.plot1);
            this.Name = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Plot plot1;
    }
}

