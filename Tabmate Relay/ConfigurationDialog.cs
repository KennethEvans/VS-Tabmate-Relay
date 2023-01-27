using KEUtils.Utils;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using static TabmateRelay.KeyDef;

namespace TabmateRelay {
    public partial class ConfigurationDialog : Form {
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
                        keyDefControl = (KeyDefControl)control;
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
        /// Returns a KeyDefs[60] with the button names set, empty KeyString and
        /// Label, and set to UNUSED.
        /// </summary>
        /// <returns>The KeyDef array.</returns>
        public static KeyDef[] DefaultConfiguration() {
            KeyDef[] keyDefs = new KeyDef[60];
            for (int i = 0; i < 60; i++) {
                keyDefs[i] = new KeyDef(ButtonNames[i % 4], "",
                    KeyDef.KeyType.UNUSED, "");
            }
            return keyDefs;
        }

        /// <summary>
        /// Returns a KeyDefs[60] with the button names set, KeyString of the
        /// form A0, ..., D59, Label saying Test plus the KeyString, and set to
        /// NORMAL. It can be used in a text editor, for example, to see what
        /// comes out.
        /// </summary>
        /// <returns>The KeyDef array.</returns>
        public static KeyDef[] TestConfiguration() {
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

        public static KeyDef[] OpenConfiguration(string fileName) {
            KeyDef[] keyDefs = new KeyDef[60];
            string[] tokens;
            int i, j, button;
            KeyType type;
            bool first = true;
            int nLines = 0;
            try {
                foreach (string line in File.ReadAllLines(fileName)) {
                    nLines++;
                    if (first) {
                        // First line is the heading
                        first = false;
                        continue;
                    }
                    if (nLines > 61) {
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
                    keyDefs[button] = new KeyDef(tokens[3], tokens[5],
                        type, tokens[4]);
                }
                if (nLines < 61) {
                    Utils.errMsg($"Not enough lines in {fileName} for 60 buttons");
                }
                Utils.infoMsg($"Read configuration from {fileName}");
            } catch (Exception ex) {
                Utils.excMsg("Error reading configuration from "
                     + fileName, ex);
                return null;
            }
            return keyDefs;
        }

        public static void SaveConfiguration(KeyDef[] keyDefs, string fileName) {
            // Using TAB as separator
            try {
                KeyDef keyDef;
                int button;
                using (StreamWriter outputFile = File.CreateText(fileName)) {
                    outputFile.WriteLine("Button\tPage\tNumber\tName\tLabel\tKeyString\tType");
                    for (int j = 0; j < 4; j++) {
                        for (int i = 0; i < 15; i++) {
                            button = 15 * j + i;
                            keyDef = keyDefs[button];
                            outputFile.WriteLine($"{button}\t" +
                                $"{j}\t" +
                                $"{i}\t" +
                                $"{ButtonNames[i]}\t" +
                                $"{keyDef.Label}\t" +
                                $"{keyDef.KeyString}\t" +
                                $"{keyDef.Type}");
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
            Cursor = Cursors.WaitCursor;
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
