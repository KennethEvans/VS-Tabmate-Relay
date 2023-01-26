using KEUtils.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TabmateRelay {
    public partial class ConfigurationDialog : Form {
        public KeyDef[] Configuration { get; set; }
        public KeyDef[] Configuration0 { get; set; }

        public ConfigurationDialog(KeyDef[] keyDefs) {
            InitializeComponent();

            Configuration = Configuration0 = keyDefs;
            SetValues();
        }

        private void SetValues() {
            int button;
            KeyDef keyDef;
            for (int j = 0; j < 4; j++) {
                for (int i = 0; i < 15; i++) {
                    button = 15 * j + i;
                    keyDef = Configuration[button];
                    KeyDefControl control = new KeyDefControl(keyDef);
                    control.Dock = DockStyle.Fill;
                    control.BorderStyle = BorderStyle.Fixed3D;
                    tableLayoutPanelKeyDefs.Controls.Add(control, j, i + 1);
                }
            }
        }

        private void GetValues() {
            int button;
            Control control;
            KeyDefControl keyDefControl;
            for (int j = 0; j < 4; j++) {
                for (int i = 0; i < 15; i++) {
                    button = 15 * j + i;
                    control = tableLayoutPanelKeyDefs.GetControlFromPosition(j, i + 1);
                    if (control is KeyDefControl) {
                        keyDefControl = (KeyDefControl) control;
                        Configuration[button] = keyDefControl.GetValues();
                    }
                }
            }
        }

        /// <summary>
        /// Reads a configuration from the specified file name.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Configuration readConfig(string fileName) {
            try {
                Configuration newConfig = JsonConvert.
                    DeserializeObject<Configuration>
                    (System.IO.File.ReadAllText(fileName));
                return newConfig;
            } catch (Exception ex) {
                Utils.excMsg("Error reading configuration from "
                     + fileName, ex);
                return null;
            }
        }

        /// <summary>
        /// Writes the specified configuration to the specified file.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="fileName"></param>
        public static void writeConfig(Configuration config, string fileName) {
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            System.IO.File.WriteAllText(fileName, json);
        }



        private void OnFormClosing(object sender, FormClosingEventArgs e) {
            // Just hide rather than close if the user did it
            if (e.CloseReason == CloseReason.UserClosing) {
                e.Cancel = true;
                Visible = false;
            }
        }

        private void OnVisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                this.DialogResult = DialogResult.None;
            }
        }

        private void OnButtonResetClick(object sender, EventArgs e) {
            Cursor= Cursors.WaitCursor;
            Configuration = Configuration0;
            SetValues();
            Cursor = Cursors.Default;
        }

        private void OnButtonCancelClick(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            Configuration = Configuration0;
            this.Visible = false;
        }

        private void OnButtonOkClick(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            GetValues();
            this.Visible = false;
        }

    }
}
