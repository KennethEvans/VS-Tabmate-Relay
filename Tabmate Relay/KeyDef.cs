using Newtonsoft.Json;
using System.Windows.Forms;
using System;
using System.Diagnostics;
using System.Configuration;
using System.Drawing;
using Windows.Win32.Foundation;
using WindowsInput.Native;
using WindowsInput;

namespace TabmateRelay {

    public class KeyDef {
        public static readonly string NL = Environment.NewLine;

        public enum KeyType { NORMAL, HOLD, COMMAND, UNUSED };

        public string Name { get; set; }
        public string KeyString { get; set; }
        public KeyType Type { get; set; } = KeyType.NORMAL;
        public bool Pressed { get; set; }

        [JsonConstructor]
        public KeyDef(string name="", string keyString="", 
            KeyType type=KeyType.UNUSED) {
            Name = name;
            KeyString = keyString;
            Type = type;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="keyDef"></param>
        public KeyDef(KeyDef keyDef) {
            this.Name = keyDef.Name;
            this.KeyString = keyDef.KeyString;
            this.Type = keyDef.Type;
        }

        public bool Equals(KeyDef other) {
            if (other == null) return false;
            if (this.Name.Equals(other.Name) &&
                this.KeyString.Equals(other.KeyString) &&
                this.Type == other.Type) {
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
            if(Type == KeyType.HOLD && Pressed) {
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
                        throw new KeyDefException("Unable to get key code for " + Name);
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
                            + KeyString + "| for " + Name);

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
