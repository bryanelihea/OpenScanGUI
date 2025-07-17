using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class BsdlSelectorForm : Form
{
    private ComboBox comboBox;
    private Button okButton;

    public string SelectedBsdl { get; private set; }

    public BsdlSelectorForm(List<string> bsdlList)
    {
        Text = "Select BSDL";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(300, 120);
        MinimizeBox = false;
        MaximizeBox = false;
        ShowInTaskbar = false;

        comboBox = new ComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Location = new Point(10, 10),
            Size = new Size(260, 24)
        };
        comboBox.Items.AddRange(bsdlList.ToArray());
        if (comboBox.Items.Count > 0)
            comboBox.SelectedIndex = 0;
        Controls.Add(comboBox);

        okButton = new Button()
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Location = new Point(200, 45),
            Size = new Size(70, 25)
        };
        okButton.Click += (sender, e) =>
        {
            SelectedBsdl = comboBox.SelectedItem?.ToString();
            Close();
        };
        Controls.Add(okButton);

        AcceptButton = okButton;
    }
}
