namespace TabmateRelay {
    partial class KeyDefControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tableLayoutPanelTop = new System.Windows.Forms.TableLayoutPanel();
            this.name = new System.Windows.Forms.Label();
            this.tableLayoutPanelKeydef = new System.Windows.Forms.TableLayoutPanel();
            this.labelKeyString = new System.Windows.Forms.Label();
            this.textBoxKeyString = new System.Windows.Forms.TextBox();
            this.flowLayoutPanelType = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButtonNormal = new System.Windows.Forms.RadioButton();
            this.radioButtonHold = new System.Windows.Forms.RadioButton();
            this.radioButtonCommand = new System.Windows.Forms.RadioButton();
            this.radioButtonUnused = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonPaste = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.tableLayoutPanelTop.SuspendLayout();
            this.tableLayoutPanelKeydef.SuspendLayout();
            this.flowLayoutPanelType.SuspendLayout();
            this.flowLayoutPanelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelTop
            // 
            this.tableLayoutPanelTop.AutoSize = true;
            this.tableLayoutPanelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelTop.ColumnCount = 1;
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTop.Controls.Add(this.name, 0, 0);
            this.tableLayoutPanelTop.Controls.Add(this.tableLayoutPanelKeydef, 0, 1);
            this.tableLayoutPanelTop.Controls.Add(this.flowLayoutPanelType, 0, 2);
            this.tableLayoutPanelTop.Controls.Add(this.flowLayoutPanelButtons, 0, 3);
            this.tableLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTop.Name = "tableLayoutPanelTop";
            this.tableLayoutPanelTop.RowCount = 4;
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTop.Size = new System.Drawing.Size(523, 233);
            this.tableLayoutPanelTop.TabIndex = 0;
            // 
            // name
            // 
            this.name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.name.AutoSize = true;
            this.name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.Location = new System.Drawing.Point(210, 3);
            this.name.Margin = new System.Windows.Forms.Padding(3);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(103, 32);
            this.name.TabIndex = 0;
            this.name.Text = "Button";
            this.name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanelKeydef
            // 
            this.tableLayoutPanelKeydef.AutoSize = true;
            this.tableLayoutPanelKeydef.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelKeydef.ColumnCount = 2;
            this.tableLayoutPanelKeydef.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelKeydef.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelKeydef.Controls.Add(this.labelKeyString, 0, 0);
            this.tableLayoutPanelKeydef.Controls.Add(this.textBoxKeyString, 1, 0);
            this.tableLayoutPanelKeydef.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelKeydef.Location = new System.Drawing.Point(3, 40);
            this.tableLayoutPanelKeydef.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanelKeydef.Name = "tableLayoutPanelKeydef";
            this.tableLayoutPanelKeydef.RowCount = 1;
            this.tableLayoutPanelKeydef.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelKeydef.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelKeydef.Size = new System.Drawing.Size(517, 42);
            this.tableLayoutPanelKeydef.TabIndex = 10;
            // 
            // labelKeyString
            // 
            this.labelKeyString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelKeyString.AutoSize = true;
            this.labelKeyString.BackColor = System.Drawing.SystemColors.Control;
            this.labelKeyString.Location = new System.Drawing.Point(3, 0);
            this.labelKeyString.Name = "labelKeyString";
            this.labelKeyString.Size = new System.Drawing.Size(145, 42);
            this.labelKeyString.TabIndex = 0;
            this.labelKeyString.Text = "Key String";
            this.labelKeyString.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxKeyString
            // 
            this.textBoxKeyString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxKeyString.Location = new System.Drawing.Point(154, 2);
            this.textBoxKeyString.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxKeyString.Name = "textBoxKeyString";
            this.textBoxKeyString.Size = new System.Drawing.Size(360, 38);
            this.textBoxKeyString.TabIndex = 1;
            // 
            // flowLayoutPanelType
            // 
            this.flowLayoutPanelType.AutoSize = true;
            this.flowLayoutPanelType.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelType.Controls.Add(this.radioButtonNormal);
            this.flowLayoutPanelType.Controls.Add(this.radioButtonHold);
            this.flowLayoutPanelType.Controls.Add(this.radioButtonCommand);
            this.flowLayoutPanelType.Controls.Add(this.radioButtonUnused);
            this.flowLayoutPanelType.Location = new System.Drawing.Point(3, 87);
            this.flowLayoutPanelType.Name = "flowLayoutPanelType";
            this.flowLayoutPanelType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.flowLayoutPanelType.Size = new System.Drawing.Size(511, 84);
            this.flowLayoutPanelType.TabIndex = 8;
            // 
            // radioButtonNormal
            // 
            this.radioButtonNormal.AutoSize = true;
            this.radioButtonNormal.Checked = true;
            this.radioButtonNormal.Location = new System.Drawing.Point(3, 3);
            this.radioButtonNormal.Name = "radioButtonNormal";
            this.radioButtonNormal.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.radioButtonNormal.Size = new System.Drawing.Size(162, 36);
            this.radioButtonNormal.TabIndex = 0;
            this.radioButtonNormal.TabStop = true;
            this.radioButtonNormal.Text = "Normal";
            this.radioButtonNormal.UseVisualStyleBackColor = true;
            // 
            // radioButtonHold
            // 
            this.radioButtonHold.AutoSize = true;
            this.radioButtonHold.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonHold.Location = new System.Drawing.Point(171, 3);
            this.radioButtonHold.Name = "radioButtonHold";
            this.radioButtonHold.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.radioButtonHold.Size = new System.Drawing.Size(130, 36);
            this.radioButtonHold.TabIndex = 1;
            this.radioButtonHold.Text = "Hold";
            this.radioButtonHold.UseVisualStyleBackColor = true;
            // 
            // radioButtonCommand
            // 
            this.radioButtonCommand.AutoSize = true;
            this.radioButtonCommand.Location = new System.Drawing.Point(307, 3);
            this.radioButtonCommand.Name = "radioButtonCommand";
            this.radioButtonCommand.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.radioButtonCommand.Size = new System.Drawing.Size(201, 36);
            this.radioButtonCommand.TabIndex = 2;
            this.radioButtonCommand.Text = "Command";
            this.radioButtonCommand.UseVisualStyleBackColor = true;
            // 
            // radioButtonUnused
            // 
            this.radioButtonUnused.AutoSize = true;
            this.radioButtonUnused.Location = new System.Drawing.Point(3, 45);
            this.radioButtonUnused.Name = "radioButtonUnused";
            this.radioButtonUnused.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.radioButtonUnused.Size = new System.Drawing.Size(169, 36);
            this.radioButtonUnused.TabIndex = 3;
            this.radioButtonUnused.Text = "Unused";
            this.radioButtonUnused.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelButtons
            // 
            this.flowLayoutPanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.flowLayoutPanelButtons.AutoSize = true;
            this.flowLayoutPanelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelButtons.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanelButtons.Controls.Add(this.buttonReset);
            this.flowLayoutPanelButtons.Controls.Add(this.buttonCopy);
            this.flowLayoutPanelButtons.Controls.Add(this.buttonPaste);
            this.flowLayoutPanelButtons.Location = new System.Drawing.Point(110, 177);
            this.flowLayoutPanelButtons.Name = "flowLayoutPanelButtons";
            this.flowLayoutPanelButtons.Size = new System.Drawing.Size(303, 53);
            this.flowLayoutPanelButtons.TabIndex = 9;
            this.flowLayoutPanelButtons.WrapContents = false;
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCopy.AutoSize = true;
            this.buttonCopy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonCopy.Location = new System.Drawing.Point(107, 2);
            this.buttonCopy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(90, 42);
            this.buttonCopy.TabIndex = 0;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.OnCopyClick);
            // 
            // buttonPaste
            // 
            this.buttonPaste.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonPaste.AutoSize = true;
            this.buttonPaste.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonPaste.Location = new System.Drawing.Point(203, 2);
            this.buttonPaste.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonPaste.Name = "buttonPaste";
            this.buttonPaste.Size = new System.Drawing.Size(97, 42);
            this.buttonPaste.TabIndex = 1;
            this.buttonPaste.Text = "Paste";
            this.buttonPaste.UseVisualStyleBackColor = true;
            this.buttonPaste.Click += new System.EventHandler(this.OnPasteClick);
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonReset.AutoSize = true;
            this.buttonReset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonReset.Location = new System.Drawing.Point(3, 2);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(98, 42);
            this.buttonReset.TabIndex = 2;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.OnResetClick);
            // 
            // KeyDefControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelTop);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "KeyDefControl";
            this.Size = new System.Drawing.Size(523, 233);
            this.tableLayoutPanelTop.ResumeLayout(false);
            this.tableLayoutPanelTop.PerformLayout();
            this.tableLayoutPanelKeydef.ResumeLayout(false);
            this.tableLayoutPanelKeydef.PerformLayout();
            this.flowLayoutPanelType.ResumeLayout(false);
            this.flowLayoutPanelType.PerformLayout();
            this.flowLayoutPanelButtons.ResumeLayout(false);
            this.flowLayoutPanelButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTop;
        private System.Windows.Forms.Label name;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelKeydef;
        private System.Windows.Forms.Label labelKeyString;
        private System.Windows.Forms.TextBox textBoxKeyString;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelType;
        private System.Windows.Forms.RadioButton radioButtonNormal;
        private System.Windows.Forms.RadioButton radioButtonHold;
        private System.Windows.Forms.RadioButton radioButtonCommand;
        private System.Windows.Forms.RadioButton radioButtonUnused;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButtons;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonPaste;
        private System.Windows.Forms.Button buttonReset;
    }
}
