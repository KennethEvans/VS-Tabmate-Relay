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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.specialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logInputReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logFlagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logButtonIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logButtonTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logButtonLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logButtonKeyStringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logButtonNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logActiveWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.startTabmateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findTabmateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listHIDDevicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.pickDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabmateBluetoothInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listDevicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxLog = new System.Windows.Forms.RichTextBox();
            this.flowLayoutPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.tableLayoutPanelTop = new System.Windows.Forms.TableLayoutPanel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemMaximize = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.flowLayoutPanelButtons.SuspendLayout();
            this.tableLayoutPanelTop.SuspendLayout();
            this.contextMenuStripNotifyIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1332, 52);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minimizeToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(87, 48);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // minimizeToolStripMenuItem
            // 
            this.minimizeToolStripMenuItem.Name = "minimizeToolStripMenuItem";
            this.minimizeToolStripMenuItem.Size = new System.Drawing.Size(451, 54);
            this.minimizeToolStripMenuItem.Text = "Send to System Tray";
            this.minimizeToolStripMenuItem.Click += new System.EventHandler(this.OnFileMinimizeClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(448, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(451, 54);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnFileExitClick);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationToolStripMenuItem,
            this.loggingToolStripMenuItem,
            this.toolStripSeparator1,
            this.infoToolStripMenuItem,
            this.toolStripSeparator5,
            this.startTabmateToolStripMenuItem,
            this.findTabmateToolStripMenuItem,
            this.listHIDDevicesToolStripMenuItem,
            this.toolStripSeparator4,
            this.pickDeviceToolStripMenuItem,
            this.tabmateBluetoothInfoToolStripMenuItem,
            this.listDevicesToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(111, 48);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.specialToolStripMenuItem});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(581, 54);
            this.configurationToolStripMenuItem.Text = "Configuration";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.OnToolsConfigurationEditClick);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(306, 54);
            this.editToolStripMenuItem.Text = "Edit...";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.OnToolsConfigurationEditClick);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(306, 54);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OnToolsConfigurationOpenClick);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(306, 54);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.OnToolsConfigurationSaveAsClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(303, 6);
            // 
            // specialToolStripMenuItem
            // 
            this.specialToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultToolStripMenuItem,
            this.testToolStripMenuItem});
            this.specialToolStripMenuItem.Name = "specialToolStripMenuItem";
            this.specialToolStripMenuItem.Size = new System.Drawing.Size(306, 54);
            this.specialToolStripMenuItem.Text = "Special";
            // 
            // defaultToolStripMenuItem
            // 
            this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
            this.defaultToolStripMenuItem.Size = new System.Drawing.Size(279, 54);
            this.defaultToolStripMenuItem.Text = "Default";
            this.defaultToolStripMenuItem.Click += new System.EventHandler(this.OnToolsConfigurationSpecialDefaultClick);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(279, 54);
            this.testToolStripMenuItem.Text = "Test";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.OnToolsConfigurationTestClick);
            // 
            // loggingToolStripMenuItem
            // 
            this.loggingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logInputReportToolStripMenuItem,
            this.logFlagToolStripMenuItem,
            this.logButtonIndexToolStripMenuItem,
            this.logButtonTypeToolStripMenuItem,
            this.logButtonLabelToolStripMenuItem,
            this.logButtonKeyStringToolStripMenuItem,
            this.logButtonNameToolStripMenuItem,
            this.logActiveWindowToolStripMenuItem,
            this.logOnToolStripMenuItem});
            this.loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            this.loggingToolStripMenuItem.Size = new System.Drawing.Size(581, 54);
            this.loggingToolStripMenuItem.Text = "Logging";
            // 
            // logInputReportToolStripMenuItem
            // 
            this.logInputReportToolStripMenuItem.CheckOnClick = true;
            this.logInputReportToolStripMenuItem.Name = "logInputReportToolStripMenuItem";
            this.logInputReportToolStripMenuItem.Size = new System.Drawing.Size(440, 54);
            this.logInputReportToolStripMenuItem.Text = "Log Input Report";
            this.logInputReportToolStripMenuItem.CheckedChanged += new System.EventHandler(this.OnToolsLogInputReportChecked);
            // 
            // logFlagToolStripMenuItem
            // 
            this.logFlagToolStripMenuItem.CheckOnClick = true;
            this.logFlagToolStripMenuItem.Name = "logFlagToolStripMenuItem";
            this.logFlagToolStripMenuItem.Size = new System.Drawing.Size(440, 54);
            this.logFlagToolStripMenuItem.Text = "Log Flag";
            this.logFlagToolStripMenuItem.CheckedChanged += new System.EventHandler(this.OnToolsLogFlagChecked);
            // 
            // logButtonIndexToolStripMenuItem
            // 
            this.logButtonIndexToolStripMenuItem.CheckOnClick = true;
            this.logButtonIndexToolStripMenuItem.Name = "logButtonIndexToolStripMenuItem";
            this.logButtonIndexToolStripMenuItem.Size = new System.Drawing.Size(440, 54);
            this.logButtonIndexToolStripMenuItem.Text = "Log Index";
            this.logButtonIndexToolStripMenuItem.CheckedChanged += new System.EventHandler(this.OnToolsLogButtonIndexChecked);
            // 
            // logButtonTypeToolStripMenuItem
            // 
            this.logButtonTypeToolStripMenuItem.CheckOnClick = true;
            this.logButtonTypeToolStripMenuItem.Name = "logButtonTypeToolStripMenuItem";
            this.logButtonTypeToolStripMenuItem.Size = new System.Drawing.Size(440, 54);
            this.logButtonTypeToolStripMenuItem.Text = "Log Type";
            this.logButtonTypeToolStripMenuItem.CheckedChanged += new System.EventHandler(this.OnToolsLogButtonTypeChecked);
            // 
            // logButtonLabelToolStripMenuItem
            // 
            this.logButtonLabelToolStripMenuItem.CheckOnClick = true;
            this.logButtonLabelToolStripMenuItem.Name = "logButtonLabelToolStripMenuItem";
            this.logButtonLabelToolStripMenuItem.Size = new System.Drawing.Size(440, 54);
            this.logButtonLabelToolStripMenuItem.Text = "Log Label";
            this.logButtonLabelToolStripMenuItem.CheckedChanged += new System.EventHandler(this.OnToolsLogButtonLabelChecked);
            // 
            // logButtonKeyStringToolStripMenuItem
            // 
            this.logButtonKeyStringToolStripMenuItem.CheckOnClick = true;
            this.logButtonKeyStringToolStripMenuItem.Name = "logButtonKeyStringToolStripMenuItem";
            this.logButtonKeyStringToolStripMenuItem.Size = new System.Drawing.Size(440, 54);
            this.logButtonKeyStringToolStripMenuItem.Text = "Log Key String";
            this.logButtonKeyStringToolStripMenuItem.CheckedChanged += new System.EventHandler(this.OnToolsLogButtonKeyStringChecked);
            // 
            // logButtonNameToolStripMenuItem
            // 
            this.logButtonNameToolStripMenuItem.CheckOnClick = true;
            this.logButtonNameToolStripMenuItem.Name = "logButtonNameToolStripMenuItem";
            this.logButtonNameToolStripMenuItem.Size = new System.Drawing.Size(440, 54);
            this.logButtonNameToolStripMenuItem.Text = "Log Name";
            this.logButtonNameToolStripMenuItem.CheckedChanged += new System.EventHandler(this.OnToolsLogButtonNameChecked);
            // 
            // logActiveWindowToolStripMenuItem
            // 
            this.logActiveWindowToolStripMenuItem.CheckOnClick = true;
            this.logActiveWindowToolStripMenuItem.Name = "logActiveWindowToolStripMenuItem";
            this.logActiveWindowToolStripMenuItem.Size = new System.Drawing.Size(440, 54);
            this.logActiveWindowToolStripMenuItem.Text = "Log Active Window";
            this.logActiveWindowToolStripMenuItem.CheckedChanged += new System.EventHandler(this.OnToolsLogActiveWindowChecked);
            // 
            // logOnToolStripMenuItem
            // 
            this.logOnToolStripMenuItem.CheckOnClick = true;
            this.logOnToolStripMenuItem.Name = "logOnToolStripMenuItem";
            this.logOnToolStripMenuItem.Size = new System.Drawing.Size(440, 54);
            this.logOnToolStripMenuItem.Text = "Logging On";
            this.logOnToolStripMenuItem.CheckedChanged += new System.EventHandler(this.OnToolsLogOnChecked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(578, 6);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(581, 54);
            this.infoToolStripMenuItem.Text = "Info...";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.OnToolsInfoClicked);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(578, 6);
            // 
            // startTabmateToolStripMenuItem
            // 
            this.startTabmateToolStripMenuItem.Name = "startTabmateToolStripMenuItem";
            this.startTabmateToolStripMenuItem.Size = new System.Drawing.Size(581, 54);
            this.startTabmateToolStripMenuItem.Text = "Restart Tabmate";
            this.startTabmateToolStripMenuItem.Click += new System.EventHandler(this.OnToolsStartTabmateClick);
            // 
            // findTabmateToolStripMenuItem
            // 
            this.findTabmateToolStripMenuItem.Name = "findTabmateToolStripMenuItem";
            this.findTabmateToolStripMenuItem.Size = new System.Drawing.Size(581, 54);
            this.findTabmateToolStripMenuItem.Text = "Tabmate Info...";
            this.findTabmateToolStripMenuItem.Click += new System.EventHandler(this.OnToolsFindTabmateClick);
            // 
            // listHIDDevicesToolStripMenuItem
            // 
            this.listHIDDevicesToolStripMenuItem.Name = "listHIDDevicesToolStripMenuItem";
            this.listHIDDevicesToolStripMenuItem.Size = new System.Drawing.Size(581, 54);
            this.listHIDDevicesToolStripMenuItem.Text = "HID Device Info...";
            this.listHIDDevicesToolStripMenuItem.Click += new System.EventHandler(this.OnToolsListHIDDevicesClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(578, 6);
            // 
            // pickDeviceToolStripMenuItem
            // 
            this.pickDeviceToolStripMenuItem.Name = "pickDeviceToolStripMenuItem";
            this.pickDeviceToolStripMenuItem.Size = new System.Drawing.Size(581, 54);
            this.pickDeviceToolStripMenuItem.Text = "Pick Bluetooth Device...";
            this.pickDeviceToolStripMenuItem.Click += new System.EventHandler(this.OnToolsPickDeviceClick);
            // 
            // tabmateBluetoothInfoToolStripMenuItem
            // 
            this.tabmateBluetoothInfoToolStripMenuItem.Name = "tabmateBluetoothInfoToolStripMenuItem";
            this.tabmateBluetoothInfoToolStripMenuItem.Size = new System.Drawing.Size(581, 54);
            this.tabmateBluetoothInfoToolStripMenuItem.Text = "Paired TABMATE Info...";
            this.tabmateBluetoothInfoToolStripMenuItem.Click += new System.EventHandler(this.OnToolsTabmateInfoClick);
            // 
            // listDevicesToolStripMenuItem
            // 
            this.listDevicesToolStripMenuItem.Name = "listDevicesToolStripMenuItem";
            this.listDevicesToolStripMenuItem.Size = new System.Drawing.Size(581, 54);
            this.listDevicesToolStripMenuItem.Text = "Paired Bluetooth Device Info...";
            this.listDevicesToolStripMenuItem.Click += new System.EventHandler(this.OnToolsPairedDeviceInfoClick);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(104, 48);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(287, 54);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.OnAboutClick);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLog.Location = new System.Drawing.Point(3, 54);
            this.textBoxLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.Size = new System.Drawing.Size(1326, 854);
            this.textBoxLog.TabIndex = 4;
            this.textBoxLog.Text = "";
            this.textBoxLog.Enter += new System.EventHandler(this.OnControlEnter);
            // 
            // flowLayoutPanelButtons
            // 
            this.flowLayoutPanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.flowLayoutPanelButtons.AutoSize = true;
            this.flowLayoutPanelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelButtons.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanelButtons.Controls.Add(this.buttonClear);
            this.flowLayoutPanelButtons.Controls.Add(this.buttonQuit);
            this.flowLayoutPanelButtons.Location = new System.Drawing.Point(575, 912);
            this.flowLayoutPanelButtons.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelButtons.Name = "flowLayoutPanelButtons";
            this.flowLayoutPanelButtons.Size = new System.Drawing.Size(181, 46);
            this.flowLayoutPanelButtons.TabIndex = 5;
            this.flowLayoutPanelButtons.WrapContents = false;
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonClear.AutoSize = true;
            this.buttonClear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonClear.Location = new System.Drawing.Point(3, 2);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(92, 42);
            this.buttonClear.TabIndex = 4;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.OnClearClick);
            // 
            // buttonQuit
            // 
            this.buttonQuit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonQuit.AutoSize = true;
            this.buttonQuit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonQuit.Location = new System.Drawing.Point(101, 2);
            this.buttonQuit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(77, 42);
            this.buttonQuit.TabIndex = 3;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.OnFileExitClick);
            // 
            // tableLayoutPanelTop
            // 
            this.tableLayoutPanelTop.AutoSize = true;
            this.tableLayoutPanelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelTop.ColumnCount = 1;
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTop.Controls.Add(this.menuStrip1, 0, 0);
            this.tableLayoutPanelTop.Controls.Add(this.textBoxLog, 0, 1);
            this.tableLayoutPanelTop.Controls.Add(this.flowLayoutPanelButtons, 0, 2);
            this.tableLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTop.Name = "tableLayoutPanelTop";
            this.tableLayoutPanelTop.RowCount = 3;
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTop.Size = new System.Drawing.Size(1329, 960);
            this.tableLayoutPanelTop.TabIndex = 6;
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "Double ckick on the icon to open Tabmate Relay or right-click on the icon for mor" +
    "e options.";
            this.notifyIcon.BalloonTipTitle = "Tabmate Relay";
            this.notifyIcon.ContextMenuStrip = this.contextMenuStripNotifyIcon;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Tabmate Relay";
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnNotifyIconDoubleClick);
            // 
            // contextMenuStripNotifyIcon
            // 
            this.contextMenuStripNotifyIcon.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.contextMenuStripNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemMaximize,
            this.toolStripMenuItemQuit});
            this.contextMenuStripNotifyIcon.Name = "contextMenuStripNotifyIcon";
            this.contextMenuStripNotifyIcon.Size = new System.Drawing.Size(223, 100);
            // 
            // toolStripMenuItemMaximize
            // 
            this.toolStripMenuItemMaximize.Name = "toolStripMenuItemMaximize";
            this.toolStripMenuItemMaximize.Size = new System.Drawing.Size(222, 48);
            this.toolStripMenuItemMaximize.Text = "Maximize";
            this.toolStripMenuItemMaximize.Click += new System.EventHandler(this.OnNotifyIconMaximizeClick);
            // 
            // toolStripMenuItemQuit
            // 
            this.toolStripMenuItemQuit.Name = "toolStripMenuItemQuit";
            this.toolStripMenuItemQuit.Size = new System.Drawing.Size(222, 48);
            this.toolStripMenuItemQuit.Text = "Quit";
            this.toolStripMenuItemQuit.Click += new System.EventHandler(this.OnFileExitClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1329, 960);
            this.Controls.Add(this.tableLayoutPanelTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Tabmate Relay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Enter += new System.EventHandler(this.OnControlEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.flowLayoutPanelButtons.ResumeLayout(false);
            this.flowLayoutPanelButtons.PerformLayout();
            this.tableLayoutPanelTop.ResumeLayout(false);
            this.tableLayoutPanelTop.PerformLayout();
            this.contextMenuStripNotifyIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem listDevicesToolStripMenuItem;
        private ToolStripMenuItem pickDeviceToolStripMenuItem;
        private ToolStripMenuItem infoToolStripMenuItem;
        private ToolStripMenuItem listHIDDevicesToolStripMenuItem;
        private ToolStripMenuItem findTabmateToolStripMenuItem;
        private ToolStripMenuItem startTabmateToolStripMenuItem;
        private ToolStripMenuItem configurationToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem specialToolStripMenuItem;
        private ToolStripMenuItem testToolStripMenuItem;
        private ToolStripMenuItem defaultToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private RichTextBox textBoxLog;
        private FlowLayoutPanel flowLayoutPanelButtons;
        private Button buttonClear;
        private Button buttonQuit;
        private TableLayoutPanel tableLayoutPanelTop;
        private ToolStripMenuItem loggingToolStripMenuItem;
        private ToolStripMenuItem logInputReportToolStripMenuItem;
        private ToolStripMenuItem logFlagToolStripMenuItem;
        private ToolStripMenuItem logButtonLabelToolStripMenuItem;
        private ToolStripMenuItem logButtonIndexToolStripMenuItem;
        private ToolStripMenuItem logButtonKeyStringToolStripMenuItem;
        private ToolStripMenuItem logButtonNameToolStripMenuItem;
        private ToolStripMenuItem logActiveWindowToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem logButtonTypeToolStripMenuItem;
        private ToolStripMenuItem logOnToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem tabmateBluetoothInfoToolStripMenuItem;
        private NotifyIcon notifyIcon;
        private ToolStripMenuItem minimizeToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ContextMenuStrip contextMenuStripNotifyIcon;
        private ToolStripMenuItem toolStripMenuItemMaximize;
        private ToolStripMenuItem toolStripMenuItemQuit;
    }
}