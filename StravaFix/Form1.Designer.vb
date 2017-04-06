<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblFileName = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.chkNotStopped = New System.Windows.Forms.CheckBox()
        Me.chkPeed = New System.Windows.Forms.CheckBox()
        Me.bgwConvert = New System.ComponentModel.BackgroundWorker()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(29, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(185, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Drag and Drop a Strava GPX file here"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(29, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "File Dropped:"
        '
        'lblFileName
        '
        Me.lblFileName.AutoSize = True
        Me.lblFileName.Location = New System.Drawing.Point(32, 79)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.Size = New System.Drawing.Size(0, 13)
        Me.lblFileName.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(39, 197)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Go"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'chkNotStopped
        '
        Me.chkNotStopped.AutoSize = True
        Me.chkNotStopped.Checked = True
        Me.chkNotStopped.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNotStopped.Location = New System.Drawing.Point(39, 165)
        Me.chkNotStopped.Name = "chkNotStopped"
        Me.chkNotStopped.Size = New System.Drawing.Size(161, 17)
        Me.chkNotStopped.TabIndex = 4
        Me.chkNotStopped.Text = "I forgot to stop Strava at end"
        Me.chkNotStopped.UseVisualStyleBackColor = True
        '
        'chkPeed
        '
        Me.chkPeed.AutoSize = True
        Me.chkPeed.Checked = True
        Me.chkPeed.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPeed.Location = New System.Drawing.Point(39, 142)
        Me.chkPeed.Name = "chkPeed"
        Me.chkPeed.Size = New System.Drawing.Size(112, 17)
        Me.chkPeed.TabIndex = 5
        Me.chkPeed.Text = "I peed during race"
        Me.chkPeed.UseVisualStyleBackColor = True
        '
        'bgwConvert
        '
        Me.bgwConvert.WorkerReportsProgress = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 227)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(253, 22)
        Me.ProgressBar1.TabIndex = 6
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.chkPeed)
        Me.Controls.Add(Me.chkNotStopped)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblFileName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblFileName As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents chkNotStopped As CheckBox
    Friend WithEvents chkPeed As CheckBox
    Friend WithEvents bgwConvert As System.ComponentModel.BackgroundWorker
    Friend WithEvents ProgressBar1 As ProgressBar
End Class
