
using InTheHand.Net.Sockets;
using KEUtils.About;
using KEUtils.ScrolledText;
using KEUtils.Utils;
using Newtonsoft.Json;
using SharpLib.Hid.Device;
using SharpLib.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TabmateRelay.Properties;
using static TabmateRelay.KeyDef;

namespace TabmateRelay {
    public partial class MainForm : Form {
        public static readonly string NL = Environment.NewLine;
        // These identify the tabmate
        public const uint TABMATE_PRODUCT_ID = 0x8502;
        public const uint TABMATE_VENDOR_ID = 0x0a5c;
        public ushort usagePage = 1;
        public ushort usageCollection = 5;
        private static ScrolledTextDialog logDialog;
        //private Input tabmateDevice;

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

        public static string[] ButtonNames { get; } = {
            "Button +",
            "Button -",
            "Button A",
            "Button B",
            "Button C",
            "Button D",
            "Wheel Up",
            "Wheel Down",
            "Wheel Push",
            "Trigger",
            "Pad Up",
            "Pad Down",
            "Pad Right",
            "Pad Left",
            "Pad Middle",
        };
        public KeyDef[] Configuration { get; set; }

        public MainForm() {
            InitializeComponent();

            // Get configuration
            GetConfigurationFromSettings();
            //// Set to default configuration
            //Configuration = DefaultConfiguration();

            usagePage = Settings.Default.UsagePage;
            usageCollection = Settings.Default.UsageCollection;

            logDialog = new ScrolledTextDialog(
            Utils.getDpiAdjustedSize(this, new Size(600, 400)),
                "Tabmate Relay Log");
            logDialog.appendTextAndNL("Started: " + Timestamp());
            logDialog.ButtonCancel.Visible = false;
            //ShowLogDialogInFront();


            //getHidDeviceList();
            //InitializeBluetooth();
            StartTabmate();
            ShowLogDialogInFront();
        }

        public void StartTabmate() {
            LogAppendTextAndNL($"{NL} {Timestamp()} Starting Tabmate");
            // Dispose of any existing handler
            if (handler != null) {
                handler.Dispose();
                handler = null;
            }

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
                string msg = "Failed to register handler" + NL
                    + Marshal.GetLastWin32Error().ToString();
                Utils.errMsg(msg);
                LogAppendTextAndNL(msg);
                return;
            }
            handler.OnHidEvent += HandleHidEventThreadSafe;
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

            // Write to log
            byte[] val = hidEvent.InputReport;
            ushort[] flags = new ushort[4];
            flags[0] = (ushort)BitConverter.ToInt16(val, 1);
            flags[1] = (ushort)BitConverter.ToInt16(val, 3);
            flags[2] = (ushort)BitConverter.ToInt16(val, 5);
            flags[3] = (ushort)BitConverter.ToInt16(val, 7);

            // Write to log
            LogEvent(hidEvent, flags);

            // Process input
            ushort pos;
            int button;
            bool wasPressed;
            KeyDef keyDef;
            for (int j = 0; j < 4; j++) {
                for (int i = 0; i < 15; i++) {
                    button = 15 * j + i;
                    pos = (ushort)(1 << i);
                    keyDef = Configuration[button];
                    wasPressed = keyDef.Pressed;
                    try {
                        if ((pos & flags[j]) == 1) {
                            // Button is down
                            keyDef.HandleKey();
                        } else if (keyDef.Type == KeyDef.KeyType.HOLD && wasPressed) {
                            // Button was down but is not now
                            keyDef.HandleHoldKeyWasPressed();
                        } else continue;
                    } catch (Exception ex) {
                        LogAppendTextAndNL($"{Timestamp()} {ex.ToString()}");
                    }
                }
            }
        }

        public void LogEvent(SharpLib.Hid.Event hidEvent, ushort[] flags) {
            DateTime time = hidEvent.Time;
            string strVal = hidEvent.InputReportString();
            string updown = "??";
            if (hidEvent.IsButtonUp) {
                updown = "UP";
            } else if (hidEvent.IsButtonDown) {
                updown = "DOWN";
            }
            string buttonStr = "Button";
            if (hidEvent.Usages.Count > 0) {
                buttonStr = "Button " + hidEvent.Usages[0].ToString();
            }

            LogAppendTextAndNL($"  {time.ToString("hh:mm:ss tt")} {strVal}" +
                $" flags {flags[0]:x2} {flags[1]:x2} {flags[2]:x2} {flags[3]:x2}  {buttonStr} {updown}");

            // DEBUG
            string fgTitle = Tools.getForegroundWindowTitle();
            LogAppendTextAndNL($"Foreground window: {fgTitle}");
        }

        private KeyDef[] DefaultConfiguration() {
            KeyDef[] keyDefs = new KeyDef[60];
            int button;
            int charA = (int)'A';
            string label;
            for (int j = 0; j < 4; j++) {
                for (int i = 0; i < 15; i++) {
                    button = 15 * j + i;
                    label = $"{(char)(charA + j)}{i}";
                    keyDefs[button] = new KeyDef(ButtonNames[i],
                        label, KeyDef.KeyType.NORMAL, $"Test {label}");
                }
            }
            return keyDefs;
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
            if (logDialog == null) return;
            logDialog.appendTextAndNL(text);
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
            if (client != null) {
                sb.AppendLine(NL + "Client Info:");
                sb.AppendLine("Client: " + client);
                sb.AppendLine("Client connected: " + client.Connected);
            } else {
                sb.AppendLine(NL + "Client: None");
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
                //int end1 = json.Length;
                //if (end1 > 100) end1 = 100;
                //int end2 = Settings.Default.Configuration.Length;
                //if (end2 > 100) end2 = 100;
                //Utils.infoMsg($"SaveConfigurationToSettings:{NL}" +
                //    $"{Configuration[0].KeyString}{NL}" +
                //    $"Settings: {Settings.Default.Configuration.Substring(0, end2)}...{NL}" +
                //    $"json: {json.Substring(0, end1)}...");
            } catch (System.Exception ex) {
                Utils.excMsg("Error saving Configuration", ex);
            }
        }

        public void GetConfigurationFromSettings() {
            string json = Settings.Default.Configuration;
            if (String.IsNullOrEmpty(json) || json.Equals("null")) {
                Configuration = DefaultConfiguration();
            } else {
                try {
                    Configuration = JsonConvert.DeserializeObject<KeyDef[]>(json);
                    return;
                } catch (Exception ex) {
                    Utils.excMsg(
                        "Error restoring Configuration from settings, using default", ex);
                    Configuration = DefaultConfiguration();
                }
            }
            //int end1 = json.Length;
            //if (end1 > 100) end1 = 100;
            //int end2 = Settings.Default.Configuration.Length;
            //if (end2 > 100) end2 = 100;
            //Utils.infoMsg($"GetConfigurationFromSettings:{NL}" +
            //    $"{Configuration[0].KeyString}{NL}" +
            //    $"Settings: {Settings.Default.Configuration.Substring(0, end1)}...{NL}" +
            //    $"json: {json.Substring(0, end2)}...");
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

        private void ShowLogDialogInFront() {
            // Run on the UI thread
            BeginInvoke(new Action(() => {
                if (logDialog != null) {
                    logDialog.Visible = true;
                    logDialog.BringToFront();
                } else {
                    Utils.infoMsg("No log has been created yet");
                }
            }));
        }

        public void OpenConfiguration(string fileName) {
            string[] tokens;
            int i, j, button;
            KeyType type;
            bool first = true;
            int nLines = 0;
            try {
                foreach (string line in File.ReadAllLines(fileName)) {
                    nLines++;
                    if(first) {
                        // First line is the heading
                        first = false;
                        continue;
                    }
                    if(nLines > 61) {
                        Utils.errMsg($"Too many lines in {fileName}");
                        break;

                    }
                    tokens = line.Split('\t');
                    button = Convert.ToInt32(tokens[0]);
                    j = Convert.ToInt32(tokens[1]);
                    i = Convert.ToInt32(tokens[2]);
                    if (tokens[6].Equals("NORMAL")) type = KeyType.NORMAL;
                    else if (tokens[6].Equals("HOLD")) type = KeyType.HOLD;
                    else if (tokens[6].Equals("COMMAND")) type = KeyType.COMMAND;
                    else if (tokens[6].Equals("UNUSED")) type = KeyType.UNUSED;
                    else type = KeyType.NORMAL;
                    Configuration[button] = new KeyDef(tokens[3], tokens[5],
                        type, tokens[4]);
                }
                if (nLines < 61) {
                    Utils.errMsg($"Not enough lines in {fileName} for 60 buttons");
                }
                Utils.infoMsg($"Read configuration from {fileName}");
            } catch (Exception ex) {
                Utils.excMsg("Error reading configuration from "
                     + fileName, ex);
            }
        }

        public void SaveConfiguration(string fileName) {
            // Using TAB as separator
            try {
                KeyDef keyDef;
                int button;
                using (StreamWriter outputFile = File.CreateText(fileName)) {
                    outputFile.WriteLine("Button\tPage\tNumber\tName\tLabel\tKeyString\tType");
                    for (int j = 0; j < 4; j++) {
                        for (int i = 0; i < 15; i++) {
                            button = 15 * j + i;
                            keyDef = Configuration[button];
                            outputFile.WriteLine($"{button}\t" +
                                $"{j}\t" +
                                $"{i}\t" +
                                $"{ButtonNames[i]}\t" +
                                $"{keyDef.Label}\t" +
                                $"{keyDef.KeyString}\t" +
                                $"{(KeyType)keyDef.Type}");
                        }
                    }
                    outputFile.Close();
                    Utils.infoMsg($"Saved configuration to {fileName}");
                }
            } catch (Exception ex) {
                Utils.excMsg("Error writing" + fileName, ex);
                return;
            }
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
            if (logDialog != null) {
                logDialog.Close();
            }
        }

        private void OnToolsShowLogClick(object sender, EventArgs e) {
            ShowLogDialogInFront();
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

        private void OnToolsListPairedDevicesClick(object sender, EventArgs e) {
            if (client == null) {
                Utils.errMsg("The Bluetooth Client is null");
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
            if (logDialog != null) {
                logDialog.Close();
            }
            Close();
        }

        private void OnToolsConnectClick(object sender, EventArgs e) {
            if (device != null && client != null) {
                Thread thread = new Thread(new ThreadStart(connectClient));
                thread.Name = "Connection Thread";
                thread.Priority = ThreadPriority.AboveNormal;
                thread.Start();
            }
        }

        private void OnToolsFindTabmateClick(object sender, EventArgs e) {
            Input input = FindTabmate();
            if (input == null) {
                Utils.errMsg("Failed to find Tabmate");
            } else {
                //tabmateDevice = input;
                string msg = "Found Tabmate" + NL + NL + HIDDeviceInfo(input);
                Utils.infoMsg(msg);
                logDialog.appendTextAndNL($"{Timestamp()} {msg}");
            }
        }

        private void OnToolsStartTabmateClick(object sender, EventArgs e) {
            StartTabmate();
            ShowLogDialogInFront();
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
                OpenConfiguration(dlg.FileName);
            }
        }

        private void OnToolsConfigurationSaveAsClick(object sender, EventArgs e) {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "CSV Files|*.csv";
            dlg.Title = "Select a Configuration File";
            dlg.CheckPathExists = true;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                SaveConfiguration(dlg.FileName);
            }

        }
    }
}