using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using KEUtils.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TabmateRelay {
    /// <summary>
    /// This partial class contains all the InTheHand stuff.
    /// 
    /// </summary>
    public partial class MainForm {
        private BluetoothClient client;
        private BluetoothDeviceInfo device;

        public void InitializeBluetooth() {
            client = new BluetoothClient();
            IEnumerable<BluetoothDeviceInfo> pairedDevices = client.PairedDevices;
            device = null;
            foreach (BluetoothDeviceInfo info in pairedDevices) {
                if (info.DeviceName.Contains("TABMATE")) {
                    device = info;
                    break;
                }
            }
            if (logDialog != null) {
                if (device != null) {
                    logDialog.appendTextAndNL(Timestamp() + " TABMATE found");
                    logDialog.appendTextAndNL("Device connected: " + device.Connected);
                } else {
                    logDialog.appendTextAndNL(Timestamp() + " TABMATE not found");
                }
                logDialog.appendTextAndNL("Client connected: " + client.Connected);
            }
            //// Try to connect on a new thread
            //if (device != null) {
            //    Thread thread = new Thread(new ThreadStart(connectClient));
            //    thread.Name = "Connection Thread";
            //    thread.Priority = ThreadPriority.AboveNormal;
            //    thread.Start();
            //}
        }

        public string DeviceInfo(BluetoothDeviceInfo info, bool verbose = false) {
            string msg = "";
            msg += "DeviceName: " + info.DeviceName + NL;
            msg += "Device Address: " + FormattedMacAddress(info.DeviceAddress) + NL;
            msg += "Device connected: " + info.Connected + NL;
            msg += "ClassOfDevice: " + info.ClassOfDevice + NL;
            msg += "Installed Services" + NL;
            IReadOnlyCollection<Guid> services = info.InstalledServices;
            foreach (Guid service in services) {
                msg += "    " + service + NL;
            }

            return msg;
        }

        public async Task PickBluetoothDevice() {
            BluetoothDevicePicker picker = new BluetoothDevicePicker();
            device = await picker.PickSingleDeviceAsync();
            if (logDialog != null) {
                logDialog.appendTextAndNL(Timestamp() + " Picked " + device.DeviceName);
                logDialog.appendTextAndNL("Device connected: " + device.Connected);
            }
        }

        /// <summary>
        /// This connects the client. It is intended to be run on a separate thread.
        /// </summary>
        public void connectClient() {
            if (client == null) {
                Utils.errMsg("Cannot connect: client is null");
                return;
            }
            if (device == null) {
                Utils.errMsg("Cannot connect: client is null");
                return;
            }
            if (logDialog != null) {
                BeginInvoke(new Action(() => {
                    logDialog.appendTextAndNL(Timestamp() + " Client trying to connect");
                }));
            }
            device.SetServiceState(BluetoothService.HumanInterfaceDevice, true);
            try {
                client.Connect(device.DeviceAddress, BluetoothService.HumanInterfaceDevice);
                if (logDialog != null) {
                    BeginInvoke(new Action(() => {
                        logDialog.appendTextAndNL(Timestamp() + " Client Connected");
                    }));
                }
            } catch (Exception ex) {
                Utils.excMsg("Client failed to connect", ex);
                if (logDialog != null) {
                    BeginInvoke(new Action(() => {
                        logDialog.appendTextAndNL(Timestamp() + " Client failed to connect");
                        ShowLogDialogInFront();
                    }));
                }
            }
        }






    }
}
