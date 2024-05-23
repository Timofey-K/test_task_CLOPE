
namespace Test_CLOPE
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
            this.btn_open = new System.Windows.Forms.Button();
            this.rTB_journal = new System.Windows.Forms.RichTextBox();
            this.btn_ex_CLOPE_1 = new System.Windows.Forms.Button();
            this.tb_withR = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_test_mushrooms = new System.Windows.Forms.Button();
            this.btn_ex_CLOPE_2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(12, 12);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(140, 70);
            this.btn_open.TabIndex = 0;
            this.btn_open.Text = "Загрузить данные";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // rTB_journal
            // 
            this.rTB_journal.Location = new System.Drawing.Point(158, 12);
            this.rTB_journal.Name = "rTB_journal";
            this.rTB_journal.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rTB_journal.Size = new System.Drawing.Size(644, 575);
            this.rTB_journal.TabIndex = 1;
            this.rTB_journal.Text = "";
            // 
            // btn_ex_CLOPE_1
            // 
            this.btn_ex_CLOPE_1.Location = new System.Drawing.Point(12, 132);
            this.btn_ex_CLOPE_1.Name = "btn_ex_CLOPE_1";
            this.btn_ex_CLOPE_1.Size = new System.Drawing.Size(140, 70);
            this.btn_ex_CLOPE_1.TabIndex = 2;
            this.btn_ex_CLOPE_1.Text = "Выполнить метод CLOPE \r\nпервая итерация";
            this.btn_ex_CLOPE_1.UseVisualStyleBackColor = true;
            this.btn_ex_CLOPE_1.Click += new System.EventHandler(this.btn_ex_CLOPE_1_Click);
            // 
            // tb_withR
            // 
            this.tb_withR.Location = new System.Drawing.Point(12, 106);
            this.tb_withR.Name = "tb_withR";
            this.tb_withR.Size = new System.Drawing.Size(140, 20);
            this.tb_withR.TabIndex = 3;
            this.tb_withR.Text = "2,6";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Параметр r";
            // 
            // btn_test_mushrooms
            // 
            this.btn_test_mushrooms.Enabled = false;
            this.btn_test_mushrooms.Location = new System.Drawing.Point(12, 356);
            this.btn_test_mushrooms.Name = "btn_test_mushrooms";
            this.btn_test_mushrooms.Size = new System.Drawing.Size(140, 72);
            this.btn_test_mushrooms.TabIndex = 5;
            this.btn_test_mushrooms.Text = "проверка чистоты кластеризации на примре грибов";
            this.btn_test_mushrooms.UseVisualStyleBackColor = true;
            this.btn_test_mushrooms.Click += new System.EventHandler(this.btn_test_mushrooms_Click);
            // 
            // btn_ex_CLOPE_2
            // 
            this.btn_ex_CLOPE_2.Enabled = false;
            this.btn_ex_CLOPE_2.Location = new System.Drawing.Point(12, 208);
            this.btn_ex_CLOPE_2.Name = "btn_ex_CLOPE_2";
            this.btn_ex_CLOPE_2.Size = new System.Drawing.Size(140, 70);
            this.btn_ex_CLOPE_2.TabIndex = 6;
            this.btn_ex_CLOPE_2.Text = "Выполнить метод CLOPE\r\nпоследующие итерации";
            this.btn_ex_CLOPE_2.UseVisualStyleBackColor = true;
            this.btn_ex_CLOPE_2.Click += new System.EventHandler(this.btn_ex_CLOPE_2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 599);
            this.Controls.Add(this.btn_ex_CLOPE_2);
            this.Controls.Add(this.btn_test_mushrooms);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_withR);
            this.Controls.Add(this.btn_ex_CLOPE_1);
            this.Controls.Add(this.rTB_journal);
            this.Controls.Add(this.btn_open);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.RichTextBox rTB_journal;
        private System.Windows.Forms.Button btn_ex_CLOPE_1;
        private System.Windows.Forms.TextBox tb_withR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_test_mushrooms;
        private System.Windows.Forms.Button btn_ex_CLOPE_2;
    }
}

