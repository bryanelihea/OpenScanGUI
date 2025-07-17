using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace OpenScanGUI
{
    public partial class frmMain : Form
    {
        // we'll move these to a config.ini file or similar
        static string scriptFolder = @"C:\Users\smacdonaldsmith\source\repos\OpenScanPy";
        static string venvPath = Path.Combine(scriptFolder, ".venv");
        static string scriptName = "testscript";  // testscript.py without .py

        private List<DeviceInfo> deviceRects = new List<DeviceInfo>();
        private List<string> bsdl = new List<string>();

        private Dictionary<int, string> assignedBsdl = new Dictionary<int, string>();
        private int tap1Count = 0;
        private int tap2Count = 0;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnScanChain_Click(object sender, EventArgs e)
        {
            if (cmbControllers.Text.Contains(":"))
            {
                PyBridge.CallFunc("open_controller", cmbControllers.Text.Split(':')[0]);
            }
            else
            {
                return;
            }

            PyBridge.CallFunc("select_tap", "1");
            tap1Count = Convert.ToInt16(PyBridge.CallFunc("detect_chain_length", ""));

            PyBridge.CallFunc("select_tap", "2");
            tap2Count = Convert.ToInt16(PyBridge.CallFunc("detect_chain_length", ""));

            DrawChain(tap1Count, tap2Count);

            
        }

        private void DrawChain(int tap1Count, int tap2Count)
        {
            // Preserve existing assignments
            var previousAssignments = deviceRects.ToDictionary(d => d.Label, d => d.AssignedBsdl);

            deviceRects.Clear(); // reset on every redraw

            string imagePath = Path.Combine(Application.StartupPath, "device.png");
            if (!File.Exists(imagePath))
            {
                MessageBox.Show("device.png not found.");
                return;
            }

            Image originalImg = Image.FromFile(imagePath);
            int canvasWidth = pbxChain.Width;
            int canvasHeight = pbxChain.Height;

            int maxDeviceWidth = 80;
            int maxDeviceHeight = 80;
            int arrowLength = 20;
            int verticalSpacing = 20;
            int sectionHeight = (canvasHeight - verticalSpacing) / 2;

            Bitmap bmp = new Bitmap(canvasWidth, canvasHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Pen arrowPen = new Pen(Color.Black, 2)
                {
                    EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor
                };

                Font labelFont = new Font("Segoe UI", 9);
                Font tagFont = new Font("Segoe UI", 10, FontStyle.Bold);

                void DrawRow(int count, int yOffset, string tapLabel)
                {
                    if (count <= 0) return;

                    int deviceAreaWidth = canvasWidth - 140;
                    int totalDeviceWidth = count * maxDeviceWidth + (count - 1) * arrowLength;
                    float scale = Math.Min(1f, (float)deviceAreaWidth / totalDeviceWidth);

                    int devW = (int)(maxDeviceWidth * scale);
                    int devH = (int)(maxDeviceHeight * scale);
                    int spacing = (int)(arrowLength * scale);

                    int chainWidth = count * devW + (count - 1) * spacing;
                    int xStart = (canvasWidth - chainWidth) / 2;
                    int yCenter = yOffset + (sectionHeight - devH) / 2;
                    int arrowY = yCenter + devH / 2;

                    g.DrawString($"{tapLabel} :", tagFont, Brushes.Black, 5, yCenter + (devH - tagFont.Height) / 2);

                    SizeF tdiSize = g.MeasureString("TDI", tagFont);
                    int tdiX = xStart - spacing - 40;
                    g.DrawString("TDI", tagFont, Brushes.Black, tdiX, yCenter + (devH - tdiSize.Height) / 2);
                    g.DrawLine(arrowPen, xStart - spacing, arrowY, xStart, arrowY);

                    for (int i = 0; i < count; i++)
                    {
                        int x = xStart + i * (devW + spacing);
                        g.DrawImage(originalImg, new Rectangle(x, yCenter, devW, devH));

                        string label = $"Dev {i + 1}";
                        SizeF labelSize = g.MeasureString(label, labelFont);
                        g.DrawString(label, labelFont, Brushes.Black, x + (devW - labelSize.Width) / 2, yCenter + devH + 2);

                        string assignedBsdl = previousAssignments.TryGetValue(label, out var val) ? val : null;

                        if (!string.IsNullOrEmpty(assignedBsdl))
                        {
                            SizeF bsdlSize = g.MeasureString(assignedBsdl, labelFont);
                            g.DrawString(assignedBsdl, labelFont, Brushes.Blue,
                                         x + (devW - bsdlSize.Width) / 2, yCenter + devH + 2 + labelFont.Height);
                        }

                        deviceRects.Add(new DeviceInfo
                        {
                            Bounds = new Rectangle(x, yCenter, devW, devH),
                            Label = label,
                            Index = i + 1,
                            AssignedBsdl = assignedBsdl
                        });

                        if (i < count - 1)
                        {
                            int x1 = x + devW;
                            int x2 = x1 + spacing;
                            g.DrawLine(arrowPen, x1, arrowY, x2, arrowY);
                        }
                    }

                    int lastX = xStart + (count - 1) * (devW + spacing) + devW;
                    g.DrawLine(arrowPen, lastX, arrowY, lastX + spacing, arrowY);
                    SizeF tdoSize = g.MeasureString("TDO", tagFont);
                    g.DrawString("TDO", tagFont, Brushes.Black, lastX + spacing + 5, yCenter + (devH - tdoSize.Height) / 2);
                }

                DrawRow(tap1Count, 0, "TAP1");
                DrawRow(tap2Count, sectionHeight + verticalSpacing, "TAP2");
            }

            pbxChain.Image = bmp;
        }


        private void pbxChain_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            foreach (var dev in deviceRects)
            {
                if (dev.Bounds.Contains(me.Location))
                {
                    using (var form = new BsdlSelectorForm(bsdl))  // bsdl = List<string> from Python
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            string selected = form.SelectedBsdl;

                            dev.AssignedBsdl = selected;  // ✅ Store selection
                            DrawChain(tap1Count, tap2Count);  // ← force redraw to update image

                            Console.WriteLine($"Assigned {selected} to {dev.Label}");
                        }
                    }

                    return;
                }
            }
        }



        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                // Make sure Python can find the script
                Directory.SetCurrentDirectory(scriptFolder);

                int ok = PyBridge.init_python(scriptName, venvPath);
                if (ok == 0)
                {
                    MessageBox.Show("Failed to load Python module.");
                    return;
                }

                //tmrDebug.Enabled = true;

                foreach (var c in getControllers())
                {
                    cmbControllers.Items.Add(c);
                }

                if (cmbControllers.Items.Count > 0)
                {
                    cmbControllers.SelectedIndex = 0;
                }

                bsdl = getBsdl();


            }
            catch (DllNotFoundException ex)
            {
                MessageBox.Show("DLL not found: " + ex.Message);
            }
            catch (EntryPointNotFoundException ex)
            {
                MessageBox.Show("Method not exported from DLL: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private List<string> getBsdl()
        {
            var b = new List<string>();

            var response = PyBridge.CallFunc("get_bsdl_names", "");

            if (response != "None")
            {
                b.AddRange(response.Split(',').Select(s => s.Trim()));
            }

            return b;
        }

        private List<string> getControllers()
        {
            var controllers = new List<string>();

            var response = PyBridge.CallFunc("get_controllers", "");

            if (response != "None")
            {
                controllers.AddRange(response.Split(','));
            }

            return controllers;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            PyBridge.finalize_python();
        }

        private void tmrDebug_Tick(object sender, EventArgs e)
        {
            string output = PyBridge.GetDebugOutput();
            if (!string.IsNullOrEmpty(output))
            {
                rtbDebug.AppendText(output);
                rtbDebug.SelectionStart = rtbDebug.Text.Length;
                rtbDebug.ScrollToCaret();
                PyBridge.ClearDebugOutput();
            }
        }

        public static string GenerateZeroByteString(int numBytes)
        {
            return string.Join(" ", Enumerable.Repeat("00", numBytes));
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            var expectedId = new List<string>();

            PyBridge.CallFunc("select_tap", "1");
            PyBridge.CallFunc("tlr", "");

            foreach (var dev in deviceRects)
            {
                if (!string.IsNullOrEmpty(dev.AssignedBsdl))
                {
                    int index = dev.Index;  // Make sure you set this during DrawChain
                    string bsdl = dev.AssignedBsdl;

                    expectedId.Add(PyBridge.CallFunc("assign_device", $"{index.ToString()},{bsdl}"));
                }
            }

            PyBridge.CallFunc("send_ir_chain", "IDCODE, IDCODE, IDCODE, IDCODE");
            PyBridge.CallFunc("set_test_state", "SHIFT_DR");

            var longIdCode = reverseByteString(PyBridge.CallFunc("send_bytes_while_read_hexstring", $"{GenerateZeroByteString(4 * tap1Count)}"));

            var matchCount = 0;

            for (int i = 0; i < tap1Count; i++)
            {
               if(expectedId[i] == "0x" + longIdCode.Substring((i * 8), 8))
                {
                    matchCount++;
                }
                else
                {
                    MessageBox.Show(longIdCode.Substring((i * 8), 8));
                }
            }

            if (matchCount == tap1Count)
            {
                MessageBox.Show("All device ID codes match!");
            }
            else
            {
                MessageBox.Show($"{matchCount.ToString()} of {tap1Count.ToString()} device ID codes match!");
            }

            // now we have the chain IDCODE so we need to split it up into the X devices and then reformat the bytes
        }

        private string reverseByteString(string bytestring)
        {
            var b_array = bytestring.Substring(2, bytestring.Length - 3).Replace("x", "").Split('\\');   // remove the b' ', remove x's and split by \\
            var newString = "";

            foreach (var b in b_array)
            {
                newString = b + newString;  // now we swap the order
            }

            return newString.ToUpper();
        }

    }
    

    public class DeviceInfo
    {
        public Rectangle Bounds;
        public int Index;
        public string Label;
        public string AssignedBsdl { get; set; }

    }

}

