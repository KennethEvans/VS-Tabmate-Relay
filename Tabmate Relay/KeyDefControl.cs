using KEUtils.Utils;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TabmateRelay {

    public partial class KeyDefControl : UserControl {
        KeyDef KeyDef { get; set; }
        KeyDef KeyDef0 { get; set; }

        public KeyDefControl(KeyDef keyDef) {
            InitializeComponent();

            KeyDef = KeyDef0 = keyDef;
            SetValues();
        }

        public void SetValues() {
            labelButton.Text = KeyDef.Button;
            textBoxLabel.Text = KeyDef.Label;
            textBoxKeyString.Text = KeyDef.KeyString;
            switch (KeyDef.Type) {
                case KeyDef.KeyType.NORMAL:
                    radioButtonNormal.Checked = true;
                    break;
                case KeyDef.KeyType.HOLD:
                    radioButtonHold.Checked = true;
                    break;
                case KeyDef.KeyType.COMMAND:
                    radioButtonCommand.Checked = true;
                    break;
                case KeyDef.KeyType.UNUSED:
                    radioButtonUnused.Checked = true;
                    break;
            }
        }

        public KeyDef GetValues() {
            KeyDef newKeyDef= new KeyDef();
            newKeyDef.Button = labelButton.Text;
            newKeyDef.Label = textBoxLabel.Text;
            newKeyDef.KeyString = textBoxKeyString.Text;
            if (radioButtonNormal.Checked) newKeyDef.Type = KeyDef.KeyType.NORMAL;
            else if (radioButtonHold.Checked) newKeyDef.Type = KeyDef.KeyType.HOLD;
            else if (radioButtonCommand.Checked) newKeyDef.Type = KeyDef.KeyType.COMMAND;
            else if (radioButtonUnused.Checked) newKeyDef.Type = KeyDef.KeyType.UNUSED;
            return newKeyDef;
        }

        public void Reset() {
            KeyDef = KeyDef0;
            SetValues();
        }

        private void OnResetClick(object sender, System.EventArgs e) {
            Reset();
        }

        private void OnCopyClick(object sender, System.EventArgs e) {
            KeyDef newKeyDef = GetValues();
            try {
                string json =
                    JsonConvert.SerializeObject(newKeyDef, Formatting.Indented);
                Clipboard.SetText(json);
            } catch (System.Exception ex) {
                Utils.excMsg("Error sending key up events", ex);
            }
        }

        private void OnPasteClick(object sender, System.EventArgs e) {
            IDataObject ClipData = Clipboard.GetDataObject();
            if (!ClipData.GetDataPresent(DataFormats.Text)) {
                Utils.errMsg("Clipboard does not contain a key definition");
                return;
            }
            string json = Clipboard.GetData(DataFormats.Text).ToString();
            KeyDef newKeyDef;
            try {
                newKeyDef = JsonConvert.DeserializeObject<KeyDef>(json);
                // Dont't paste the name of the key
                newKeyDef.Button = KeyDef.Button;
            } catch (Exception ex) {
                Utils.excMsg(
                    "Error converting clipboard contents to a "
                    + "key definition", ex);
                return;
            }
            KeyDef = newKeyDef;
            SetValues();
        }

        private void OnActionClick(object sender, EventArgs e) {
            contextMenuStrip1.Show(buttonAction, new Point(buttonAction.Width, 0));
        }
    }
}
