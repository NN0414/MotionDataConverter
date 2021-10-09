<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fMotionDataConverter
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
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

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請勿使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.M = New System.Windows.Forms.TableLayoutPanel()
        Me.D = New System.Windows.Forms.DataGridView()
        Me.EVENT_DATETIME = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MSG = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.B_TRANS = New System.Windows.Forms.Button()
        Me.M.SuspendLayout()
        CType(Me.D, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'M
        '
        Me.M.ColumnCount = 2
        Me.M.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90.0!))
        Me.M.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
        Me.M.Controls.Add(Me.D, 0, 0)
        Me.M.Controls.Add(Me.B_TRANS, 1, 0)
        Me.M.Dock = System.Windows.Forms.DockStyle.Fill
        Me.M.Location = New System.Drawing.Point(0, 0)
        Me.M.Name = "M"
        Me.M.RowCount = 1
        Me.M.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.M.Size = New System.Drawing.Size(800, 450)
        Me.M.TabIndex = 0
        '
        'D
        '
        Me.D.AllowUserToAddRows = False
        Me.D.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.D.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.EVENT_DATETIME, Me.MSG})
        Me.D.Dock = System.Windows.Forms.DockStyle.Fill
        Me.D.Location = New System.Drawing.Point(3, 3)
        Me.D.Name = "D"
        Me.D.RowHeadersVisible = False
        Me.D.RowTemplate.Height = 24
        Me.D.Size = New System.Drawing.Size(714, 444)
        Me.D.TabIndex = 0
        '
        'EVENT_DATETIME
        '
        Me.EVENT_DATETIME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.EVENT_DATETIME.DataPropertyName = "EVENT_DATETIME"
        Me.EVENT_DATETIME.FillWeight = 14.0!
        Me.EVENT_DATETIME.HeaderText = "時間"
        Me.EVENT_DATETIME.Name = "EVENT_DATETIME"
        '
        'MSG
        '
        Me.MSG.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.MSG.DataPropertyName = "MSG"
        Me.MSG.FillWeight = 86.0!
        Me.MSG.HeaderText = "訊息"
        Me.MSG.Name = "MSG"
        '
        'B_TRANS
        '
        Me.B_TRANS.Location = New System.Drawing.Point(723, 3)
        Me.B_TRANS.Name = "B_TRANS"
        Me.B_TRANS.Size = New System.Drawing.Size(74, 30)
        Me.B_TRANS.TabIndex = 1
        Me.B_TRANS.Text = "轉換"
        Me.B_TRANS.UseVisualStyleBackColor = True
        '
        'fMotionDataConverter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.M)
        Me.Font = New System.Drawing.Font("標楷體", 13.8!)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "fMotionDataConverter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CubismFadeMotion 轉換"
        Me.M.ResumeLayout(False)
        CType(Me.D, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents M As TableLayoutPanel
    Friend WithEvents D As DataGridView
    Friend WithEvents B_TRANS As Button
    Friend WithEvents EVENT_DATETIME As DataGridViewTextBoxColumn
    Friend WithEvents MSG As DataGridViewTextBoxColumn
End Class
