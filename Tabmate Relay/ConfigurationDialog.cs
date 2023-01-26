using System;
using System.Collections.Generic;
using System.ComponentModel;
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
