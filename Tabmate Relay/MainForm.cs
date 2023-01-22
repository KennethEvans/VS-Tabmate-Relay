using InTheHand.Net.Sockets;
using KEUtils.About;
using KEUtils.ScrolledText;
using KEUtils.Utils;
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

namespace TabmateRelay {
    public partial class MainForm : Form {
        public static readonly string NL = Environment.NewLine;
        // These identify the tabmate
        public const uint TABMATE_PRODUCT_ID = 0x8502;
        public const uint TABMATE_VENDOR_ID = 0x0a5c;
        private static ScrolledTextDialog logDialog;
        private Input tabmateDevice;

        // Use explicit SharpLib.Hid to avoid collision with Utils, Event, ...
        private SharpLib.Hid.Handler handler;

        // Use this to call the handler on the UI thread
        public delegate void OnHidEventDelegate(object sender, SharpLib.Hid.Event aHidEvent);
        // Avoid logging too many events otherwise we just hang when testing high frequency device like Virpil joysticks and Oculus Rift S
        DateTime logPeriodStartTime;
        int eventCount;
        const int MAX_EVENTS_PER_PERIOD = 10;
        const int PERIOD_DURATION_MS = 500;

        public MainForm() {
            InitializeComponent();

            logDialog = new ScrolledTextDialog(
            Utils.getDpiAdjustedSize(this, new Size(600, 400)),
                "Tabmate Relay Log");
            logDialog.appendTextAndNL("Started: " + Timestamp());
            logDialog.ButtonCancel.Visible = false;
            //ShowLogDialogInFront();


            //getHidDeviceList();
            InitializeBluetooth();
            StartTabmate();
            ShowLogDialogInFront();
        }

        public void StartTabmate() {
            tabmateDevice = null;
            LogAppendTextAndNL($"{NL} {Timestamp()} Starting Tabmate");
            Input input = FindTabmate();
            if (input == null) {
                LogAppendTextAndNL($"{NL} {Timestamp()} Starting Tabmate: Tabmate not found");
                return;
            }
            LogAppendTextAndNL($"{NL} {Timestamp()} Tabmate found: Start listening...");
            tabmateDevice = input;
            // Dispose of any existing handler
            if (handler != null) {
                handler.Dispose();
                handler = null;
            }

            // Make a RAWINPUTDEVICE array with one item
            RAWINPUTDEVICE[] rid = new RAWINPUTDEVICE[1];
            rid[0].usUsagePage = input.UsagePage;
            rid[0].usUsage = input.UsageCollection;
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
            DateTime time = hidEvent.Time;
            byte[] val = hidEvent.InputReport;
            string strVal = hidEvent.InputReportString();
            string updown = "??";
            if (hidEvent.IsButtonUp) {
                updown = "UP";
            } else if (hidEvent.IsButtonDown) {
                updown = "DOWN";
            }
            string button = "Button";
            if(hidEvent.Usages.Count > 0) {
                button = "Button " + hidEvent.Usages[0].ToString();
            }
            //LogAppendTextAndNL($"{time} {hidEvent}");
            LogAppendTextAndNL($"  {time.ToString("hh:mm:ss tt")} {strVal} {button} {updown}");
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
            if (tabmateDevice != null) {
                sb.AppendLine("Tabmate Info:");
                sb.Append(HIDDeviceInfo(tabmateDevice));
            } else {
                sb.AppendLine("Tabmate Info: No Tabmate device running");
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
                image = Image.FromFile(@".\Help\tabmate_info_en.png");
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
    }
}