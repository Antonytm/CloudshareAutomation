namespace Cloudshare
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
      this.lblBluePrint = new System.Windows.Forms.Label();
      this.lblSnapshotName = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.FilenameTextBox = new System.Windows.Forms.TextBox();
      this.BrowseButton = new System.Windows.Forms.Button();
      this.lbLogs = new System.Windows.Forms.ListBox();
      this.gbTypeBluePrint = new System.Windows.Forms.GroupBox();
      this.rbAliveEnvironment = new System.Windows.Forms.RadioButton();
      this.rbNewEnvironment = new System.Windows.Forms.RadioButton();
      this.bpLoadFile = new Cloudshare.Controls.CustomProgressBar();
      this.gbTypeBluePrint.SuspendLayout();
      this.SuspendLayout();
      // 
      // InstallButton
      // 
      this.InstallButton.Location = new System.Drawing.Point(595, 105);
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
      this.BlueprintsComboBox.Location = new System.Drawing.Point(298, 13);
      this.BlueprintsComboBox.Name = "BlueprintsComboBox";
      this.BlueprintsComboBox.Size = new System.Drawing.Size(372, 21);
      this.BlueprintsComboBox.TabIndex = 8;
      this.BlueprintsComboBox.SelectedIndexChanged += new System.EventHandler(this.BlueprintsComboBox_SelectedIndexChanged);
      // 
      // SpapshotsComboBox
      // 
      this.SpapshotsComboBox.FormattingEnabled = true;
      this.SpapshotsComboBox.Location = new System.Drawing.Point(298, 40);
      this.SpapshotsComboBox.Name = "SpapshotsComboBox";
      this.SpapshotsComboBox.Size = new System.Drawing.Size(372, 21);
      this.SpapshotsComboBox.TabIndex = 9;
      // 
      // lblBluePrint
      // 
      this.lblBluePrint.AutoSize = true;
      this.lblBluePrint.Location = new System.Drawing.Point(177, 16);
      this.lblBluePrint.Name = "lblBluePrint";
      this.lblBluePrint.Size = new System.Drawing.Size(80, 13);
      this.lblBluePrint.TabIndex = 10;
      this.lblBluePrint.Text = "BluePrint Name";
      // 
      // lblSnapshotName
      // 
      this.lblSnapshotName.AutoSize = true;
      this.lblSnapshotName.Location = new System.Drawing.Point(177, 43);
      this.lblSnapshotName.Name = "lblSnapshotName";
      this.lblSnapshotName.Size = new System.Drawing.Size(83, 13);
      this.lblSnapshotName.TabIndex = 11;
      this.lblSnapshotName.Text = "Snapshot Name";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(177, 70);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(72, 13);
      this.label3.TabIndex = 12;
      this.label3.Text = "File to Upload";
      // 
      // FilenameTextBox
      // 
      this.FilenameTextBox.Location = new System.Drawing.Point(298, 67);
      this.FilenameTextBox.Name = "FilenameTextBox";
      this.FilenameTextBox.Size = new System.Drawing.Size(291, 20);
      this.FilenameTextBox.TabIndex = 13;
      // 
      // BrowseButton
      // 
      this.BrowseButton.Location = new System.Drawing.Point(595, 64);
      this.BrowseButton.Name = "BrowseButton";
      this.BrowseButton.Size = new System.Drawing.Size(75, 23);
      this.BrowseButton.TabIndex = 14;
      this.BrowseButton.Text = "Browse";
      this.BrowseButton.UseVisualStyleBackColor = true;
      this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
      // 
      // lbLogs
      // 
      this.lbLogs.FormattingEnabled = true;
      this.lbLogs.Location = new System.Drawing.Point(16, 158);
      this.lbLogs.Name = "lbLogs";
      this.lbLogs.Size = new System.Drawing.Size(659, 355);
      this.lbLogs.TabIndex = 15;
      // 
      // gbTypeBluePrint
      // 
      this.gbTypeBluePrint.Controls.Add(this.rbAliveEnvironment);
      this.gbTypeBluePrint.Controls.Add(this.rbNewEnvironment);
      this.gbTypeBluePrint.Location = new System.Drawing.Point(16, 12);
      this.gbTypeBluePrint.Name = "gbTypeBluePrint";
      this.gbTypeBluePrint.Size = new System.Drawing.Size(155, 75);
      this.gbTypeBluePrint.TabIndex = 16;
      this.gbTypeBluePrint.TabStop = false;
      this.gbTypeBluePrint.Text = "Type";
      // 
      // rbAliveEnvironment
      // 
      this.rbAliveEnvironment.AutoSize = true;
      this.rbAliveEnvironment.Location = new System.Drawing.Point(17, 51);
      this.rbAliveEnvironment.Name = "rbAliveEnvironment";
      this.rbAliveEnvironment.Size = new System.Drawing.Size(110, 17);
      this.rbAliveEnvironment.TabIndex = 1;
      this.rbAliveEnvironment.Text = "Alive Environment";
      this.rbAliveEnvironment.UseVisualStyleBackColor = true;
      this.rbAliveEnvironment.CheckedChanged += new System.EventHandler(this.rbAliveEnvironment_CheckedChanged);
      // 
      // rbNewEnvironment
      // 
      this.rbNewEnvironment.AutoSize = true;
      this.rbNewEnvironment.Checked = true;
      this.rbNewEnvironment.Location = new System.Drawing.Point(18, 21);
      this.rbNewEnvironment.Name = "rbNewEnvironment";
      this.rbNewEnvironment.Size = new System.Drawing.Size(109, 17);
      this.rbNewEnvironment.TabIndex = 0;
      this.rbNewEnvironment.TabStop = true;
      this.rbNewEnvironment.Text = "New Environment";
      this.rbNewEnvironment.UseVisualStyleBackColor = true;
      this.rbNewEnvironment.CheckedChanged += new System.EventHandler(this.rbNewEnvironment_CheckedChanged);
      // 
      // bpLoadFile
      // 
      this.bpLoadFile.CustomText = "Load package...";
      this.bpLoadFile.DisplayStyle = Cloudshare.Controls.ProgressBarDisplayText.CustomText;
      this.bpLoadFile.Location = new System.Drawing.Point(16, 105);
      this.bpLoadFile.Name = "bpLoadFile";
      this.bpLoadFile.Size = new System.Drawing.Size(573, 23);
      this.bpLoadFile.Step = 1;
      this.bpLoadFile.TabIndex = 18;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(718, 538);
      this.Controls.Add(this.bpLoadFile);
      this.Controls.Add(this.gbTypeBluePrint);
      this.Controls.Add(this.lbLogs);
      this.Controls.Add(this.BrowseButton);
      this.Controls.Add(this.FilenameTextBox);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.lblSnapshotName);
      this.Controls.Add(this.lblBluePrint);
      this.Controls.Add(this.SpapshotsComboBox);
      this.Controls.Add(this.BlueprintsComboBox);
      this.Controls.Add(this.InstallButton);
      this.Name = "MainForm";
      this.Text = "Cloudshare Installer";
      this.gbTypeBluePrint.ResumeLayout(false);
      this.gbTypeBluePrint.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button InstallButton;
    private System.Windows.Forms.OpenFileDialog OpenZipFileDialog;
    private System.Windows.Forms.ComboBox BlueprintsComboBox;
    private System.Windows.Forms.ComboBox SpapshotsComboBox;
    private System.Windows.Forms.Label lblBluePrint;
    private System.Windows.Forms.Label lblSnapshotName;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox FilenameTextBox;
    private System.Windows.Forms.Button BrowseButton;
    private System.Windows.Forms.ListBox lbLogs;
    private System.Windows.Forms.GroupBox gbTypeBluePrint;
    private System.Windows.Forms.RadioButton rbAliveEnvironment;
    private System.Windows.Forms.RadioButton rbNewEnvironment;
    private Controls.CustomProgressBar bpLoadFile;
  }
}

