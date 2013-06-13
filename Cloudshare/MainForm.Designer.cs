namespace CloudshareAPIWrapper
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.InstallButton = new System.Windows.Forms.Button();
      this.OpenZipFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.BlueprintsComboBox = new System.Windows.Forms.ComboBox();
      this.SpapshotsComboBox = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.FilenameTextBox = new System.Windows.Forms.TextBox();
      this.BrowseButton = new System.Windows.Forms.Button();
      this.LogListBox = new System.Windows.Forms.ListBox();
      this.SuspendLayout();
      // 
      // InstallButton
      // 
      this.InstallButton.Location = new System.Drawing.Point(134, 105);
      this.InstallButton.Name = "InstallButton";
      this.InstallButton.Size = new System.Drawing.Size(75, 23);
      this.InstallButton.TabIndex = 6;
      this.InstallButton.Text = "Start";
      this.InstallButton.UseVisualStyleBackColor = true;
      this.InstallButton.Click += new System.EventHandler(this.InstallButton_Click);
      // 
      // OpenZipFileDialog
      // 
      this.OpenZipFileDialog.FileName = "Jetstream.zip";
      this.OpenZipFileDialog.Filter = "ZIP files|*.zip";
      // 
      // BlueprintsComboBox
      // 
      this.BlueprintsComboBox.FormattingEnabled = true;
      this.BlueprintsComboBox.Location = new System.Drawing.Point(134, 12);
      this.BlueprintsComboBox.Name = "BlueprintsComboBox";
      this.BlueprintsComboBox.Size = new System.Drawing.Size(372, 21);
      this.BlueprintsComboBox.TabIndex = 8;
      this.BlueprintsComboBox.SelectedIndexChanged += new System.EventHandler(this.BlueprintsComboBox_SelectedIndexChanged);
      // 
      // SpapshotsComboBox
      // 
      this.SpapshotsComboBox.FormattingEnabled = true;
      this.SpapshotsComboBox.Location = new System.Drawing.Point(134, 39);
      this.SpapshotsComboBox.Name = "SpapshotsComboBox";
      this.SpapshotsComboBox.Size = new System.Drawing.Size(372, 21);
      this.SpapshotsComboBox.TabIndex = 9;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(80, 13);
      this.label1.TabIndex = 10;
      this.label1.Text = "BluePrint Name";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 42);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(83, 13);
      this.label2.TabIndex = 11;
      this.label2.Text = "Snapshot Name";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(13, 69);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(72, 13);
      this.label3.TabIndex = 12;
      this.label3.Text = "File to Upload";
      // 
      // FilenameTextBox
      // 
      this.FilenameTextBox.Location = new System.Drawing.Point(134, 66);
      this.FilenameTextBox.Name = "FilenameTextBox";
      this.FilenameTextBox.Size = new System.Drawing.Size(291, 20);
      this.FilenameTextBox.TabIndex = 13;
      // 
      // BrowseButton
      // 
      this.BrowseButton.Location = new System.Drawing.Point(431, 63);
      this.BrowseButton.Name = "BrowseButton";
      this.BrowseButton.Size = new System.Drawing.Size(75, 23);
      this.BrowseButton.TabIndex = 14;
      this.BrowseButton.Text = "Browse";
      this.BrowseButton.UseVisualStyleBackColor = true;
      this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
      // 
      // LogListBox
      // 
      this.LogListBox.FormattingEnabled = true;
      this.LogListBox.Location = new System.Drawing.Point(16, 158);
      this.LogListBox.Name = "LogListBox";
      this.LogListBox.Size = new System.Drawing.Size(659, 355);
      this.LogListBox.TabIndex = 15;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(718, 538);
      this.Controls.Add(this.LogListBox);
      this.Controls.Add(this.BrowseButton);
      this.Controls.Add(this.FilenameTextBox);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.SpapshotsComboBox);
      this.Controls.Add(this.BlueprintsComboBox);
      this.Controls.Add(this.InstallButton);
      this.Name = "MainForm";
      this.Text = "Cloudshare Installer";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button InstallButton;
    private System.Windows.Forms.OpenFileDialog OpenZipFileDialog;
    private System.Windows.Forms.ComboBox BlueprintsComboBox;
    private System.Windows.Forms.ComboBox SpapshotsComboBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox FilenameTextBox;
    private System.Windows.Forms.Button BrowseButton;
    private System.Windows.Forms.ListBox LogListBox;
  }
}

