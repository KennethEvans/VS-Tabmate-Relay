using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace TabmateRelay {

    public class KeyDef {
        public static readonly string NL = Environment.NewLine;

        public enum KeyType { NORMAL, HOLD, COMMAND, UNUSED };

        public string Button { get; set; }
        public string KeyString { get; set; }
        public string Label { get; set; }
        public KeyType Type { get; set; } = KeyType.NORMAL;
        public bool Pressed { get; set; }

        [JsonConstructor]
        public KeyDef(string button = "", string keyString = "",
            KeyType type = KeyType.UNUSED, string label = "") {
            Button = button;
            KeyString = keyString;
            Label = label;
            Type = type;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="keyDef"></param>
        public KeyDef(KeyDef keyDef) {
            Button = keyDef.Button;
            KeyString = keyDef.KeyString;
            Label = keyDef.Label;
            Type = keyDef.Type;
            Pressed = keyDef.Pressed;
        }

        /// <summary>
        /// 
        /// Check for equality.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>If equal.</returns>
        public bool Equals(KeyDef other) {
            if (other == null) return false;
            if (Button.Equals(other.Button) &&
                KeyString.Equals(other.KeyString) &&
                Label.Equals(other.Label) &&
                Type == other.Type &&
                Pressed == other.Pressed) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a VirtualKeyCode for the given KeyConfig.
        /// </summary>
        /// <param name="keyDef">The key definition to use.</param>
        /// <returns></returns>
        public VirtualKeyCode GetKeyCode() {
            VirtualKeyCode keyCode;
            if (KeyString.Equals("^")) { // Ctrl
                keyCode = VirtualKeyCode.CONTROL;
            } else if (KeyString.Equals("%")) { // Alt
                keyCode = VirtualKeyCode.MENU;
            } else if (KeyString.Equals("+")) { // Shift
                keyCode = VirtualKeyCode.SHIFT;
            } else {
                throw new ArgumentException(KeyString
                    + " is not supported");
            }
            return keyCode;
        }

        public void HandleHoldKeyWasPressed() {
            if (Type == KeyType.HOLD && Pressed) {
                Pressed = true;
                // This wil send KeyUp and marks it as unpressed
                HandleKey();
            }
        }

        public void HandleKey() {
            switch (Type) {
                case KeyType.NORMAL:
                    try {
                        SendKeys.Send(KeyString);
                    } catch (Exception ex) {
                        throw new KeyDefException("Error handling NORMAL key: "
                            + NL + ex.Message);
                    }
                    break;
                case KeyType.HOLD:
                    VirtualKeyCode keyCode;
                    try {
                        keyCode = GetKeyCode();
                    } catch (System.ArgumentException) {
                        throw new KeyDefException("Unable to get key code for " + Button);
                    }
                    InputSimulator sim = new InputSimulator();
                    try {
                        if (Pressed) {
                            Pressed = false;
                            sim.Keyboard.KeyUp(keyCode);
                        } else {
                            sim.Keyboard.KeyDown(keyCode);
                            Pressed = true;
                        }
                    } catch (Exception ex) {
                        throw new KeyDefException("Error handling HOLD key: "
                            + NL + ex.Message);
                    }
                    break;
                case KeyType.COMMAND:
                    // Process the string to get the filename and arguments
                    var tokens = KeyString.Split(',');
                    if (tokens.Length == 0) {
                        throw new KeyDefException("Unable to process COMMAND |"
                            + KeyString + "| for " + Button);

                    } else {
                        // Send the KeyString as a command
                        try {
                            var process = new Process {
                                StartInfo = new ProcessStartInfo {
                                    FileName = tokens[0],
                                    Arguments = tokens.Length > 1 ? tokens[1] : ""
                                }
                            };
                            process.Start();
                        } catch (System.Exception ex) {
                            throw new KeyDefException("Error handling COMMAND key: "
                                + NL + ex.Message);
                        }
                    }
                    break;
                case KeyType.UNUSED:
                    return;
            }
        }
    }

    /// <summary>
    /// Custom exception calss.
    /// </summary>
    public class KeyDefException : Exception {
        public KeyDefException(string message) : base(message) {
        }
    }
}
