using System.Windows.Forms;

namespace TabmateRelay {
    partial class MainForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startTabmateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findTabmateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listHIDDevicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listDevicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pickDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1329, 49);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(87, 45);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(230, 54);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnFileExitClick);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startTabmateToolStripMenuItem,
            this.infoToolStripMenuItem,
            this.findTabmateToolStripMenuItem,
            this.listHIDDevicesToolStripMenuItem,
            this.listDevicesToolStripMenuItem,
            this.pickDeviceToolStripMenuItem,
            this.connectToolStripMenuItem,
            this.showLogToolStripMenuItem,
            this.configurationToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(111, 45);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // startTabmateToolStripMenuItem
            // 
            this.startTabmateToolStripMenuItem.Name = "startTabmateToolStripMenuItem";
            this.startTabmateToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            this.startTabmateToolStripMenuItem.Text = "Start Tabmate";
            this.startTabmateToolStripMenuItem.Click += new System.EventHandler(this.OnToolsStartTabmateClick);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            this.infoToolStripMenuItem.Text = "Info...";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.OnToolsInfoClicked);
            // 
            // findTabmateToolStripMenuItem
            // 
            this.findTabmateToolStripMenuItem.Name = "findTabmateToolStripMenuItem";
            this.findTabmateToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            this.findTabmateToolStripMenuItem.Text = "Find Tabmate";
            this.findTabmateToolStripMenuItem.Click += new System.EventHandler(this.OnToolsFindTabmateClick);
            // 
            // listHIDDevicesToolStripMenuItem
            // 
            this.listHIDDevicesToolStripMenuItem.Name = "listHIDDevicesToolStripMenuItem";
            this.listHIDDevicesToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            this.listHIDDevicesToolStripMenuItem.Text = "List HID devices...";
            this.listHIDDevicesToolStripMenuItem.Click += new System.EventHandler(this.OnToolsListHIDDevicesClick);
            // 
            // listDevicesToolStripMenuItem
            // 
            this.listDevicesToolStripMenuItem.Name = "listDevicesToolStripMenuItem";
            this.listDevicesToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            this.listDevicesToolStripMenuItem.Text = "List Paired Devices...";
            this.listDevicesToolStripMenuItem.Click += new System.EventHandler(this.OnToolsListPairedDevicesClick);
            // 
            // pickDeviceToolStripMenuItem
            // 
            this.pickDeviceToolStripMenuItem.Name = "pickDeviceToolStripMenuItem";
            this.pickDeviceToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            this.pickDeviceToolStripMenuItem.Text = "Pick Device...";
            this.pickDeviceToolStripMenuItem.Click += new System.EventHandler(this.OnToolsPickDeviceClick);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            this.connectToolStripMenuItem.Text = "Connect...";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.OnToolsConnectClick);
            // 
            // showLogToolStripMenuItem
            // 
            this.showLogToolStripMenuItem.Name = "showLogToolStripMenuItem";
            this.showLogToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            this.showLogToolStripMenuItem.Text = "Show Log...";
            this.showLogToolStripMenuItem.Click += new System.EventHandler(this.OnToolsShowLogClick);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(448, 54);
            this.configurationToolStripMenuItem.Text = "Configuration...";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.OnToolsConfigurationClick);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(104, 45);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(287, 54);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.OnAboutClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1329, 715);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Tabmate Relay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem showLogToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem listDevicesToolStripMenuItem;
        private ToolStripMenuItem pickDeviceToolStripMenuItem;
        private ToolStripMenuItem infoToolStripMenuItem;
        private ToolStripMenuItem connectToolStripMenuItem;
        private ToolStripMenuItem listHIDDevicesToolStripMenuItem;
        private ToolStripMenuItem findTabmateToolStripMenuItem;
        private ToolStripMenuItem startTabmateToolStripMenuItem;
        private ToolStripMenuItem configurationToolStripMenuItem;
    }
}