Imports System.IO

Public Class fMotionDataConverter
    Private Const DateFormat As String = "yyyyMMdd"
    Private Const TimeFormat As String = "HH:mm:ss"
    Private Const logFilePath As String = "C:\MotionDataConverter\Log"
    Private WithEvents Converter As MotionDataConverter
    
	Private Sub fMotionDataConverter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Converter = New MotionDataConverter
            Call WriteLog("LOG 存放路徑為" & logFilePath)
        Catch ex As Exception
            MessageBox.Show("fMotionDataConverter_Load 發生異常錯誤!" & vbCrLf & ex.ToString, Me.Text)
        End Try
    End Sub

    Private Sub B_TRANS_Click(sender As Object, e As EventArgs) Handles B_TRANS.Click
        Try
			Dim OpenFileDialog1 As New OpenFileDialog
            OpenFileDialog1.InitialDirectory = "c:\"
            OpenFileDialog1.Filter = "json files (*.json)|*.json"
            OpenFileDialog1.Multiselect = True
            OpenFileDialog1.RestoreDirectory = True
			
            If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
                M.Enabled = False
                Cursor = Cursors.WaitCursor

                Dim FilePath, BeforeData, AfterData, NewFileName As String
                FilePath = OpenFileDialog1.FileName
                FilePath = FilePath.Substring(0, FilePath.LastIndexOf("\"))

                For Each FileName As String In OpenFileDialog1.SafeFileNames
                    Call WriteLog("開始轉換: " & FileName)

                    Call GetConvertData(FilePath, FileName, BeforeData)

                    If ProcessTransferData(BeforeData, AfterData) = True Then
                        Call CreateNewJson(FilePath, FileName, AfterData, NewFileName)
                        Call WriteLog("轉換成 " & NewFileName)
                    End If
                Next

                Call WriteLog("轉換完成!")
            End If
        Catch ex As Exception
            Call WriteLog("[嚴重錯誤]" & ex.ToString)
            MessageBox.Show("B_TRANS_Click 發生異常錯誤!" & vbCrLf & ex.ToString, Me.Text)
        Finally
            Cursor = Cursors.Default
            M.Enabled = True
        End Try
    End Sub

    Private Sub GetConvertData(ByVal filePath As String, ByVal fileName As String, ByRef convertData As String)
        Try
            convertData = ""
            convertData = File.ReadAllText(filePath & "\" & fileName)
            'convertData = convertData.Replace("1.#INF", "Infinity")
        Catch ex As Exception
            Throw New Exception("GetConvertData" & ex.ToString)
        End Try
    End Sub

    Private Function ProcessTransferData(ByVal convertData As String, ByRef transferData As String) As Boolean
        ProcessTransferData = False

        Try
            transferData = ""
            transferData = Converter.Convert(convertData)
            ProcessTransferData = True
        Catch ex As Exception
            Call WriteLog("ProcessTransferData 失敗!" & vbCrLf & ex.ToString)
        End Try
    End Function

    Private Sub CreateNewJson(ByVal filePath As String, ByVal oldFileName As String, ByVal newFileContent As String, ByRef newFileName As String)
        Try
            Call GetNewFileName(oldFileName, newFileName)
            File.WriteAllText(filePath & "/" & newFileName, newFileContent)
        Catch ex As Exception
            Call WriteLog("CreateNewJson 失敗!" & vbCrLf & ex.ToString)
        End Try
    End Sub

    Private Sub GetNewFileName(ByVal oldFileName As String, ByRef newFileName As String)
        Try
            Dim chrPos As Integer
            chrPos = oldFileName.IndexOf("-")
            newFileName = oldFileName.Remove(chrPos, oldFileName.Length - chrPos)
            newFileName = newFileName & ".motion3.json"
        Catch ex As Exception
            newFileName = oldFileName
        End Try
    End Sub

    Private Sub WriteLog(ByVal msg As String) Handles Converter.WriteLog
        Try
            Static LogTable As DataTable

            If LogTable Is Nothing Then
                LogTable = New DataTable
                With LogTable
                    .Columns.Add("EVENT_DATETIME")
                    .Columns.Add("MSG")
                    .Columns("EVENT_DATETIME").DefaultValue = Format(Now, TimeFormat)
                    .Columns("MSG").DefaultValue = " "
                End With
            End If

            Dim EventDateTime As String = Format(Now, TimeFormat)
            Call WriteLogToTxt(EventDateTime, msg)
            Dim LogRow As DataRow
            LogRow = LogTable.NewRow
            LogRow("EVENT_DATETIME") = EventDateTime
            LogRow("MSG") = msg
            LogTable.Rows.Add(LogRow)

            D.DataSource = LogTable
            D.Refresh()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub WriteLogToTxt(ByVal eventTime As String, ByVal msg As String)
        Try
            If Directory.Exists(logFilePath) = False Then
                Directory.CreateDirectory(logFilePath)
            End If

            Dim FileName As String
            FileName = logFilePath & "\" & Format(Now, DateFormat) & ".log"

            Dim LogFile As FileStream
            Dim LogWriter As StreamWriter

            LogFile = New FileStream(FileName, FileMode.Append, FileAccess.Write)
            LogWriter = New StreamWriter(LogFile, System.Text.Encoding.UTF8)
            LogWriter.BaseStream.Seek(0, SeekOrigin.End)
            LogWriter.WriteLine("[" & eventTime & "] " & msg)
            LogWriter.Close()
        Catch ex As Exception
        End Try
    End Sub
End Class
