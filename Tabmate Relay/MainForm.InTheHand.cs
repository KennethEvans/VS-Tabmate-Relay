#undef USE_CONNECT

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using KEUtils.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
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
            //if (device != null) {
            //    LogAppendTextAndNL(Timestamp() + " TABMATE found");
            //    LogAppendTextAndNL("Device connected: " + device.Connected);
            //} else {
            //    LogAppendTextAndNL(Timestamp() + " TABMATE not found");
            //}
#if USE_CONNECT
            // Try to connect on a new thread
            if (device != null) {
                Thread thread = new Thread(new ThreadStart(connectClient));
                thread.Name = "Connection Thread";
                thread.Priority = ThreadPriority.AboveNormal;
                thread.Start();
            }
#endif
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
            if (device == null) {
                LogAppendTextAndNL($"{Timestamp()} Device not picked");
            } else {
                LogAppendTextAndNL($"{Timestamp()}" +
                    $" Picked {device.DeviceName}:" +
                    $" Connected: {device.Connected}");
            }
        }

#if USE_CONNECT
        /// <summary>
        /// This connects the client. It is intended to be run on a separate thread.
        /// Connecting to the client does not work, so this method is unused.
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
            BeginInvoke(new Action(() => {
                LogAppendTextAndNL(Timestamp() + " Client trying to connect");
            }));
            device.SetServiceState(BluetoothService.HumanInterfaceDevice, true);
            try {
                client.Connect(device.DeviceAddress, BluetoothService.HumanInterfaceDevice);
                BeginInvoke(new Action(() => {
                    LogAppendTextAndNL(Timestamp() + " Client Connected");
                }));
            } catch (Exception ex) {
                Utils.excMsg("Client failed to connect", ex);
                BeginInvoke(new Action(() => {
                    LogAppendTextAndNL(Timestamp() + " Client failed to connect");
                }));
            }
        }
#endif

#if USE_CONNECT
        /// <summary>
        /// Tries to connect to the Bluetooth client. This cannot be done and
        /// this method is not used.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnToolsConnectClick(object sender, EventArgs e) {
            if (client == null) {
                Utils.errMsg("There is no Bluetooth client");
                return;
            }
            if (device == null) {
                Utils.errMsg("There is no device connected");
                return;
            }
            Thread thread = new Thread(new ThreadStart(connectClient));
            thread.Name = "Connection Thread";
            thread.Priority = ThreadPriority.AboveNormal;
            thread.Start();
        }
#endif
    }
}
