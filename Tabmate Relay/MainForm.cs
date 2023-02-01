
using InTheHand.Net.Sockets;
using KEUtils.About;
using KEUtils.ScrolledText;
using KEUtils.Utils;
using Newtonsoft.Json;
using SharpLib.Hid.Device;
using SharpLib.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using TabmateRelay.Properties;

namespace TabmateRelay {
    public partial class MainForm : Form {
        public static readonly string NL = Environment.NewLine;
        // These identify the tabmate
        public const uint TABMATE_PRODUCT_ID = 0x8502;
        public const uint TABMATE_VENDOR_ID = 0x0a5c;
        public ushort usagePage = 1;
        public ushort usageCollection = 5;

        // Use explicit SharpLib.Hid to avoid collision with Utils, Event, ...
        private SharpLib.Hid.Handler handler;

        // Use this to call the handler on the UI thread
        public delegate void OnHidEventDelegate(object sender, SharpLib.Hid.Event aHidEvent);
        // Avoid logging too many events, otherwise we just hang when testing
        // high frequency device like Virpil joysticks and Oculus Rift S
        DateTime logPeriodStartTime;
        int eventCount;
        const int MAX_EVENTS_PER_PERIOD = 10;
        const int PERIOD_DURATION_MS = 500;

        private bool logOn = true;
        private bool logInputReport = false;
        private bool logFlag = false;
        private bool logButtonIndex = true;
        private bool logButtonType = true;
        private bool logButtonLabel = true;
        private bool logButtonKeyString = true;
        private bool logButtonName = false;
        private bool logActiveWindow = false;

        private int nEvents = 0;

        public KeyDef[] Configuration { get; set; }

        public MainForm() {
            InitializeComponent();

            // Get configuration
            GetConfigurationFromSettings();
            //// Set to default configuration
            //Configuration = DefaultConfiguration();

            usagePage = Settings.Default.UsagePage;
            usageCollection = Settings.Default.UsageCollection;
            logOn = Settings.Default.LogOn;
            logInputReport = Settings.Default.LogInputReport;
            logFlag = Settings.Default.LogFlag;
            logButtonIndex = Settings.Default.LogButtonIndex;
            logButtonType = Settings.Default.LogButtonType;
            logButtonLabel = Settings.Default.LogButtonLabel;
            logButtonName = Settings.Default.LogButtonName;
            logButtonKeyString = Settings.Default.LogButtonKeyString;
            logActiveWindow = Settings.Default.LogActiveWindow;

            logOnToolStripMenuItem.Checked = logOn;
            logInputReportToolStripMenuItem.Checked = logInputReport;
            logFlagToolStripMenuItem.Checked = logFlag;
            logButtonIndexToolStripMenuItem.Checked = logButtonIndex;
            logButtonTypeToolStripMenuItem.Checked = logButtonType;
            logButtonNameToolStripMenuItem.Checked = logButtonName;
            logButtonLabelToolStripMenuItem.Checked = logButtonLabel;
            logButtonKeyStringToolStripMenuItem.Checked = logButtonKeyString;
            logActiveWindowToolStripMenuItem.Checked = logActiveWindow;

            LogAppendTextAndNL("Started: " + Timestamp());

            //getHidDeviceList();
            //InitializeBluetooth();
            StartTabmate();
        }

        public void StartTabmate() {
            LogAppendTextAndNL($"{NL}{Timestamp()} Starting Tabmate");
            // Dispose of any existing handler
            if (handler != null) {
                handler.Dispose();
                handler = null;
            }
            string msg;

            // Make a RAWINPUTDEVICE array with one item
            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[1];
            rid[0].usUsagePage = usagePage;
            rid[0].usUsage = usageCollection;
            // Default is 0, needs to be added to work when not in focus
            rid[0].dwFlags = RawInputDeviceFlags.RIDEV_INPUTSINK;
            rid[0].hwndTarget = Handle;

            // Set the handler, using defaults for the other input values
            handler = new SharpLib.Hid.Handler(rid);
            if (!handler.IsRegistered) {
                msg = "Failed to register handler" + NL
                    + Marshal.GetLastWin32Error().ToString();
                Utils.errMsg(msg);
                LogAppendTextAndNL(msg);
                return;
            }
            handler.OnHidEvent += HandleHidEventThreadSafe;
            Input device = FindTabmate();
            if (device == null) {
                msg = "Tabmate not currently found but listening for it";
            } else {
                msg = "Tabmate found";
            }
            LogAppendTextAndNL($"{Timestamp()} {msg}");
        }

        /// <summary>
        /// Process HID events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="hidEvent"></param>
        public void HandleHidEventThreadSafe(object sender, SharpLib.Hid.Event hidEvent) {
            if (hidEvent.IsStray) {
                //Stray event just ignore it
                return;
            }

            if (InvokeRequired) {
                // Not in the proper thread, invoke ourselves.
                OnHidEventDelegate d = new OnHidEventDelegate(HandleHidEventThreadSafe);
                Invoke(d, new object[] { sender, hidEvent });
            } else {
                // We are in the proper thread                
                // Check if our log period has expired
                if (hidEvent.Time > logPeriodStartTime.AddMilliseconds(PERIOD_DURATION_MS)) {
                    // Our period has expired, reset our period start time and number of allowed event logs in that period
                    // Mark the time of the last event we logged
                    logPeriodStartTime = hidEvent.Time;
                    eventCount = 0;
                }

                // Check if we are still allowed to log events in that period of time
                // to avaid spamming too many events
                if (eventCount < MAX_EVENTS_PER_PERIOD) {
                    eventCount++;
                    ProcessEvent(hidEvent);
                }
            }
        }

        public void ProcessEvent(SharpLib.Hid.Event hidEvent) {
            // Be sure it comes from Tabmate
            if (hidEvent.Device.ProductId != TABMATE_PRODUCT_ID ||
                hidEvent.Device.VendorId != TABMATE_VENDOR_ID) {
                return;
            }
            List<ButtonEventData> data = new List<ButtonEventData>();
            byte[] val = hidEvent.InputReport;
            ulong flag = (ulong)BitConverter.ToInt64(val, 1);
            ulong pos;
            int button;
            bool wasPressed;
            KeyDef keyDef;
            for (int i = 0; i < 60; i++) {
                button = i;
                keyDef = Configuration[button];
                wasPressed = keyDef.Pressed;
                pos = (ulong)1 << i;
                ulong bit = pos & flag;
                try {
                    if (bit == pos) {
                        // Button is down
                        data.Add(new ButtonEventData(button, keyDef, false));
                    } else if (wasPressed && keyDef.Type == KeyDef.KeyType.HOLD) {
                        // Button was down but is not now
                        data.Add(new ButtonEventData(button, keyDef, wasPressed));
                    }
                } catch (Exception ex) {
                    LogAppendTextAndNL($"{Timestamp()} {ex}");
                }
            }

            nEvents++;

            // Write to log
            if (logOn) LogEvent(hidEvent, flag, data);

            if (data.Count == 0) return;

            // Process input
            // Don't send key sequences to our window.
            //LogAppendTextAndNL($"Window handle is {Handle:X8}" +
            //    $" Foreground window handle is {Tools.HForegroundWindow:X8}" +
            //    $" ({Tools.getForegroundWindowTitle()})");
            if (Handle.Equals(WinUtils.HForegroundWindow)) {
                return;
            }

            foreach (ButtonEventData item in data) {
                try {
                    if (!item.WasPressed) {
                        item.KeyDef.HandleKey();
                    } else {
                        item.KeyDef.HandleHoldKeyWasPressed();
                    }
                } catch (Exception ex) {
                    LogAppendTextAndNL($"{Timestamp()} {ex}");
                }
            }
        }

        public void LogEvent(SharpLib.Hid.Event hidEvent, ulong flag,
            List<ButtonEventData> data) {
            DateTime time = hidEvent.Time;
            StringBuilder sb = new StringBuilder();
            if (logInputReport) {
                string strVal = hidEvent.InputReportString();
                sb.Append($"{strVal} ");
                string updown = "??";
                if (hidEvent.IsButtonUp) {
                    updown = "UP";
                } else if (hidEvent.IsButtonDown) {
                    updown = "DOWN";
                }
                sb.Append($"{updown} ");
            }
            if (logFlag) {
                sb.Append($"Flag: {flag:x016} ");
            }
            if (logButtonIndex || logButtonType || logButtonKeyString ||
                logButtonLabel || logButtonName) {
                StringBuilder sb1 = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                if (flag == 0) {
                    sb.Append("{[All buttons Up]} ");
                } else {
                    foreach (ButtonEventData item in data) {
                        sb1.Clear();
                        if (logButtonIndex) {
                            sb1.Append(item.Index).Append(" ");
                        }
                        if (logButtonName) {
                            sb1.Append($"<{item.KeyDef.Name}");
                            if (logButtonLabel) sb1.Append(":");
                            else sb1.Append("> ");
                        }
                        if (logButtonLabel) {
                            if (logButtonName) sb1.Append($"{item.KeyDef.Label}> ");
                            else sb1.Append($"<{item.KeyDef.Label}> ");
                        }
                        if (logButtonKeyString) {
                            sb1.Append($"{item.KeyDef.KeyString} ");
                        }
                        if (logButtonType) {
                            string updown = item.WasPressed ? "(Up)" : "(Dn)";
                            string typeStr = item.KeyDef.Type.ToString().Substring(0, 1);
                            sb1.Append($"{typeStr}{updown}");
                            sb1.Append(" ");
                        }
                        if (sb1.Length > 0) {
                            sb2.Append("[");
                            string truncated = sb1.ToString();
                            if (truncated.EndsWith(" ")) {
                                truncated = truncated.Substring(0, truncated.Length - 1);
                            }
                            sb2.Append(truncated).Append("], ");
                        }
                    }
                    if (sb2.Length > 0) {
                        sb.Append("{");
                        string truncated = sb2.ToString();
                        if (truncated.EndsWith(", ")) {
                            truncated = truncated.Substring(0, truncated.Length - 2);
                        }
                        sb.Append(truncated).Append("} ");
                    }
                }
            }
            if (logActiveWindow) {
                sb.Append($"Window: {WinUtils.GetForegroundWindowTitle()} ");
            }
            LogAppendTextAndNL($"  {time.ToString("hh:mm:ss tt")} {sb}");
        }

        public Input FindTabmate() {
            RAWINPUTDEVICELIST[] deviceList = GetHidDeviceList();
            if (deviceList == null) return null;
            Input input;
            for (int i = 0; i < deviceList.Length; i++) {
                RAWINPUTDEVICELIST rAWINPUTDEVICELIST = deviceList[i];
                try {
                    input = new Input(rAWINPUTDEVICELIST.hDevice);
                    if (input.ProductId == TABMATE_PRODUCT_ID
                        && input.VendorId == TABMATE_VENDOR_ID
                        && input.IsGamePad) {
                        return input;
                    }
                } catch {
                    continue;
                }
            }
            return null;
        }

        public RAWINPUTDEVICELIST[] GetHidDeviceList() {
            RAWINPUTDEVICELIST[] inputdeviceList = null;
            // Get number of devices
            uint puiNumDevices = 0u;
            int iRes = Function.GetRawInputDeviceList(inputdeviceList, ref puiNumDevices, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            if (iRes == -1) {
                Utils.errMsg($"GetHidDeviceList: Got error:{NL}{GetLastError.String()}");
                return inputdeviceList;
            }

            // Get the list now that we have puiNumDevices
            inputdeviceList = new RAWINPUTDEVICELIST[puiNumDevices];
            iRes = Function.GetRawInputDeviceList(inputdeviceList, ref puiNumDevices, (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));
            if (iRes != puiNumDevices) {
                Utils.errMsg($"GetHidDeviceList: Wrong number of devices {iRes}/{puiNumDevices}");
                return inputdeviceList;
            }

            return inputdeviceList;
        }

        public string GetHidDeviceListString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(NL + Timestamp() + " HID Device List" + NL);
            RAWINPUTDEVICELIST[] deviceList = GetHidDeviceList();
            if (deviceList == null) {
                sb.AppendLine("getHidDeviceListString: Failed to get HID device list");
            } else {
                sb.AppendLine($"{deviceList.Length} HID Devices{NL}");
                for (int i = 0; i < deviceList.Length; i++) {
                    RAWINPUTDEVICELIST rAWINPUTDEVICELIST = deviceList[i];
                    Input input;
                    try {
                        input = new Input(rAWINPUTDEVICELIST.hDevice);
                        sb.Append(HIDDeviceInfo(input, true));
                        sb.AppendLine();
                    } catch {
                        continue;
                    }
                }
            }
            return sb.ToString();
        }

        public void LogAppendTextAndNL(string text) {
            textBoxLog.AppendText(text + NL);
        }

        public void LogAppendText(string text) {
            textBoxLog.AppendText(text);
        }

        public string Info() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Information" + NL);
            Input tabmateDevice = FindTabmate();
            if (tabmateDevice != null) {
                sb.AppendLine("Tabmate Info:");
                sb.Append(HIDDeviceInfo(tabmateDevice));
            } else {
                sb.AppendLine("Tabmate Info: No Tabmate device found");
            }
            sb.AppendLine($"Number of Tabmate events: {nEvents}");

            if (client != null) {
                sb.AppendLine(NL + "Bluetooth Client Info:");
                sb.AppendLine("Bluetooth Client: " + client);
                sb.AppendLine("Bluetooth Client connected: " + client.Connected);
            } else {
                sb.AppendLine(NL + "Bluetooth Client: None");
            }
            if (device != null) {
                sb.AppendLine(NL + "Bluetooth Device Info:");
                sb.Append(DeviceInfo(device));
            } else {
                sb.AppendLine(NL + "Bluetooth Device: None");
            }
            return sb.ToString();
        }

        public string HIDDeviceInfo(Input input, bool verbose = false) {
            if (input == null) {
                return "HIDDeviceInfo: Input is null";
            }
            StringBuilder sb = new StringBuilder();
            try {
                sb.AppendLine($"{input.FriendlyName}");
                if (verbose) sb.AppendLine($"Name: {input.Name}");
                sb.AppendLine($"VendorId: 0x{input.VendorId:x4} ({input.VendorId})");
                sb.AppendLine($"ProductID: 0x{input.ProductId:x4} ({input.ProductId})");
                sb.AppendLine($"Manufacturer: {input.Manufacturer}");
                sb.AppendLine($"Product: {input.Product}");
                sb.AppendLine($"UsagePage: {input.UsagePage}");
                sb.AppendLine($"UsageCollection: {input.UsageCollection}");
                sb.AppendLine($"ButtonCount: {input.ButtonCount}");
                sb.AppendLine($"InputCapabilitiesDescription: {input.InputCapabilitiesDescription}");
                sb.AppendLine($"InstancePath: {input.InstancePath}");
            } catch (Exception ex) {
                sb.AppendLine(ex.ToString());
            }
            return sb.ToString();
        }

        public static String Timestamp() {
            DateTime now = DateTime.Now;
            return now.ToString();
        }

        public void SaveConfigurationToSettings() {
            try {
                string json =
                    JsonConvert.SerializeObject(Configuration, Formatting.Indented);
                Settings.Default.Configuration = json;
                Settings.Default.Save();
            } catch (System.Exception ex) {
                Utils.excMsg("Error saving Configuration", ex);
            }
        }

        public void GetConfigurationFromSettings() {
            string json = Settings.Default.Configuration;
            if (String.IsNullOrEmpty(json) || json.Equals("null")) {
                Configuration = ConfigurationDialog.DefaultConfiguration();
            } else {
                try {
                    Configuration = JsonConvert.DeserializeObject<KeyDef[]>(json);
                    return;
                } catch (Exception ex) {
                    Utils.excMsg(
                        "Error restoring Configuration from settings, using default", ex);
                    Configuration = ConfigurationDialog.DefaultConfiguration();
                }
            }
        }

        public string FormattedMacAddress(InTheHand.Net.BluetoothAddress address) {
            string addr = address.ToString();
            while (addr.Length < 12) {
                addr = "0" + addr;
            }
            StringBuilder mac = new StringBuilder();
            mac.AppendFormat("{0:x2}", addr.Substring(0, 2)).Append(":");
            mac.AppendFormat("{0:x2}", addr.Substring(2, 2)).Append(":");
            mac.AppendFormat("{0:x2}", addr.Substring(4, 2)).Append(":");
            mac.AppendFormat("{0:x2}", addr.Substring(6, 2)).Append(":");
            mac.AppendFormat("{0:x2}", addr.Substring(8, 2)).Append(":");
            mac.AppendFormat("{0:x2}", addr.Substring(10, 2));
            return mac.ToString();
        }

        protected override void WndProc(ref Message message) {
            switch (message.Msg) {
                case Const.WM_INPUT:
                    // This is where the input comes in
                    message.Result = new IntPtr(0);
                    // Can be null when disabling with high event frequency device
                    if (handler != null) {
                        handler.ProcessInput(ref message);
                    }
                    break;
            }
            //Is that needed? Check the docs.
            base.WndProc(ref message);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e) {
            if (handler != null) {
                handler.Dispose();
                handler = null;
            }
            if (client != null) {
                client.Close();
            }
        }

        private void OnControlEnter(object sender, EventArgs e) {
            // Make it go to the bottom
            textBoxLog.Select(textBoxLog.Text.Length, 0);
        }

        private void OnAboutClick(object sender, EventArgs e) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Image image = null;
            try {
                image = Image.FromFile(@".\Help\Tabmate Relay.png");
            } catch (Exception ex) {
                Utils.excMsg("Failed to get AboutBox image", ex);
            }
            AboutBox dlg = new AboutBox(image, assembly);
            dlg.ShowDialog();
        }

        private void OnToolsListHIDDevicesClick(object sender, EventArgs e) {
            ScrolledTextDialog dialog = new ScrolledTextDialog(new Size(800, 800),
                "HID Devices");
            dialog.ButtonCancel.Visible = false;
            dialog.ButtonClear.Visible = false;
            dialog.appendText(GetHidDeviceListString());
            dialog.Show();
        }

        private void OnToolsPairedDeviceInfoClick(object sender, EventArgs e) {
            if (client == null) {
                InitializeBluetooth();
            }
            if (client == null) {
                Utils.errMsg("The Bluetooth Client could not be started");
                return;
            }
            IEnumerable<BluetoothDeviceInfo> pairedDevices = client.PairedDevices;
            List<BluetoothDeviceInfo> pairedDevicesList = pairedDevices.ToList();
            ScrolledTextDialog dialog = new ScrolledTextDialog(new Size(800, 800),
                "Paired Devices");
            dialog.ButtonCancel.Visible = false;
            dialog.ButtonClear.Visible = false;
            string msg = pairedDevicesList.Count + " Paired Devices";
            dialog.appendTextAndNL(msg + NL);
            foreach (BluetoothDeviceInfo info in pairedDevices) {
                dialog.appendTextAndNL(DeviceInfo(info));
            }
            dialog.Show();
        }

        private void OnToolsTabmateInfoClick(object sender, EventArgs e) {
            if (client == null) {
                InitializeBluetooth();
            }
            if (client == null) {
                Utils.errMsg("The Bluetooth Client could not be started");
                return;
            }
            string msg;
            if (device == null) {
                msg = "TABMATE not found";
            } else {
                msg = DeviceInfo(device);
            }
            Utils.infoMsg(msg);
        }

        private void OnToolsPickDeviceClick(object sender, EventArgs e) {
            Task task = PickBluetoothDevice();
        }

        private void OnToolsInfoClicked(object sender, EventArgs e) {
            Utils.infoMsg(Info());
        }

        private void OnFileExitClick(object sender, EventArgs e) {
            if (client != null) {
                client.Close();
            }
            Close();
        }

        private void OnToolsFindTabmateClick(object sender, EventArgs e) {
            Input input = FindTabmate();
            if (input == null) {
                Utils.errMsg("Failed to find Tabmate");
            } else {
                //tabmateDevice = input;
                string msg = "Found Tabmate" + NL + NL + HIDDeviceInfo(input);
                Utils.infoMsg(msg);
                LogAppendTextAndNL($"{Timestamp()} {msg}");
            }
            nEvents = 0;
        }

        private void OnToolsStartTabmateClick(object sender, EventArgs e) {
            StartTabmate();
        }

        private void OnToolsConfigurationEditClick(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;
            ConfigurationDialog dialog = new ConfigurationDialog(Configuration);
            DialogResult res = dialog.ShowDialog();
            Cursor.Current = Cursors.Default;
            if (res == DialogResult.OK) {
                Configuration = dialog.Configuration;
                SaveConfigurationToSettings();
            }
        }

        private void OnToolsConfigurationOpenClick(object sender, EventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "CSV Files|*.csv";
            dlg.Title = "Select a Configuration File";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                Configuration = ConfigurationDialog.OpenConfiguration(dlg.FileName);
                SaveConfigurationToSettings();
            }
        }

        private void OnToolsConfigurationSaveAsClick(object sender, EventArgs e) {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "CSV Files|*.csv";
            dlg.Title = "Select a Configuration File";
            dlg.CheckPathExists = true;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                ConfigurationDialog.SaveConfiguration(Configuration, dlg.FileName);
            }
        }

        private void OnToolsConfigurationSpecialDefaultClick(object sender, EventArgs e) {
            Configuration = ConfigurationDialog.DefaultConfiguration();
            SaveConfigurationToSettings();
            Utils.infoMsg("Loaded default configuration");
        }

        private void OnToolsConfigurationTestClick(object sender, EventArgs e) {
            Configuration = ConfigurationDialog.TestConfiguration();
            SaveConfigurationToSettings();
            Utils.infoMsg("Loaded test configuration");
        }

        private void OnClearClick(object sender, EventArgs e) {
            textBoxLog.Text = string.Empty;
        }

        private void OnToolsLogInputReportChecked(object sender, EventArgs e) {
            bool isChecked = logInputReportToolStripMenuItem.Checked;
            if (logInputReport != isChecked) {
                logInputReport = logInputReportToolStripMenuItem.Checked;
                Settings.Default.LogInputReport = logInputReport;
                Settings.Default.Save();
            }
        }

        private void OnToolsLogOnChecked(object sender, EventArgs e) {
            bool isChecked = logOnToolStripMenuItem.Checked;
            if (logOn != isChecked) {
                logOn = logOnToolStripMenuItem.Checked;
                Settings.Default.LogOn = logOn;
                Settings.Default.Save();
            }
        }

        private void OnToolsLogFlagChecked(object sender, EventArgs e) {
            bool isChecked = logFlagToolStripMenuItem.Checked;
            if (logFlag != isChecked) {
                logFlag = logFlagToolStripMenuItem.Checked;
                Settings.Default.LogFlag = logFlag;
                Settings.Default.Save();
            }
        }

        private void OnToolsLogButtonIndexChecked(object sender, EventArgs e) {
            bool isChecked = logButtonIndexToolStripMenuItem.Checked;
            if (logButtonIndex != isChecked) {
                logButtonIndex = logButtonIndexToolStripMenuItem.Checked;
                Settings.Default.LogButtonIndex = logButtonIndex;
                Settings.Default.Save();
            }
        }

        private void OnToolsLogButtonTypeChecked(object sender, EventArgs e) {
            bool isChecked = logButtonTypeToolStripMenuItem.Checked;
            if (logButtonType != isChecked) {
                logButtonType = logButtonTypeToolStripMenuItem.Checked;
                Settings.Default.LogButtonType = logButtonType;
                Settings.Default.Save();
            }
        }

        private void OnToolsLogButtonLabelChecked(object sender, EventArgs e) {
            bool isChecked = logButtonLabelToolStripMenuItem.Checked;
            if (logButtonLabel != isChecked) {
                logButtonLabel = logButtonLabelToolStripMenuItem.Checked;
                Settings.Default.LogButtonLabel = logButtonLabel;
                Settings.Default.Save();
            }
        }

        private void OnToolsLogButtonKeyStringChecked(object sender, EventArgs e) {
            bool isChecked = logButtonKeyStringToolStripMenuItem.Checked;
            if (logButtonKeyString != isChecked) {
                logButtonKeyString = logButtonKeyStringToolStripMenuItem.Checked;
                Settings.Default.LogButtonKeyString = logButtonKeyString;
                Settings.Default.Save();
            }
        }

        private void OnToolsLogButtonNameChecked(object sender, EventArgs e) {
            bool isChecked = logButtonNameToolStripMenuItem.Checked;
            if (logButtonName != isChecked) {
                logButtonName = logButtonNameToolStripMenuItem.Checked;
                Settings.Default.LogButtonName = logButtonName;
                Settings.Default.Save();
            }
        }

        private void OnToolsLogActiveWindowChecked(object sender, EventArgs e) {
            bool isChecked = logActiveWindowToolStripMenuItem.Checked;
            if (logActiveWindow != isChecked) {
                logActiveWindow = logActiveWindowToolStripMenuItem.Checked;
                Settings.Default.LogActiveWindow = logActiveWindow;
                Settings.Default.Save();
            }
        }
    }

    /// <summary>
    /// Class to hold data for buttons involved in an event.
    /// </summary>
    public class ButtonEventData {
        public int Index { get; set; }
        public KeyDef KeyDef { get; set; }
        public bool WasPressed { get; set; }

        public ButtonEventData(int index, KeyDef keyDef, bool pressed) {
            Index = index;
            KeyDef = keyDef;
            WasPressed = pressed;
        }
    }
}