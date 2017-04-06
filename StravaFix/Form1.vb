Imports System.Runtime.CompilerServices
Imports System.IO
Public Class Form1
    Private filename As String
    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each path In files

            lblFileName.Text = System.IO.Path.GetFileName(path)
            filename = path
        Next
    End Sub

    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' load the file to text
        Cursor = Cursors.WaitCursor
        bgwConvert.RunWorkerAsync()

    End Sub

    Private Sub bgwConvert_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwConvert.DoWork
        Dim filetext As String()
        If (filename.Length = 0) Then MsgBox("You must drag a Strava gpx file")
        filetext = System.IO.File.ReadAllLines(filename)
        Dim linecount As Long = filetext.LongCount
        Dim linepos As Long = 0

        Dim prevtime As DateTime = Nothing
        Dim currtime As DateTime
        Dim newfiletext As String = ""
        Dim peed As Boolean = False
        Dim oldpos As Decimal = 0
        If chkPeed.Checked Then


            For Each line As String In filetext
                linepos += 1
                Dim percentdone As Decimal = CDec(linepos) / CDec(linecount) * 100
                If percentdone > 10 And percentdone Mod 10 < 1 And percentdone > oldpos + 1 Then
                    bgwConvert.ReportProgress(percentdone)
                    oldpos = percentdone
                End If
                Dim newline = line
                If newline.Contains("<time>") Then
                    If prevtime = Nothing Then
                        Dim timestart As Integer = newline.IndexOf("<time>") + 6
                        Dim timeend As Integer = newline.LastIndexOf("</time>")
                        prevtime = DateTime.Parse(newline.Substring(timestart, timeend - timestart))
                    Else
                        Dim timestart As Integer = newline.IndexOf("<time>") + 6
                        Dim timeend As Integer = newline.LastIndexOf("</time>")
                        currtime = DateTime.Parse(newline.Substring(timestart, timeend - timestart))
                        If ((currtime - prevtime).TotalSeconds > 5) Then
                            peed = True

                        End If
                        If peed Then
                            Dim newutc As DateTime
                            newutc = prevtime.AddSeconds(1)

                            newline = "   <time>" & newutc.ToUniversalTime.ToString("yyyy-MM-ddTHH:mm:ssZ") & "</time>"
                            prevtime = newutc
                        Else
                            prevtime = currtime
                        End If

                    End If

                End If
                newfiletext &= newline & vbCrLf
            Next
            Dim newfilename As String
            newfilename = Path.GetDirectoryName(filename) & "\" & Path.GetFileNameWithoutExtension(filename) & "_fixed.gpx"
            If File.Exists(newfilename) Then File.Delete(newfilename)
            File.WriteAllText(newfilename, newfiletext)
            MsgBox("Pee fix complete!")

        End If

        If chkNotStopped.Checked Then
            Dim prevhr As Integer = 0
            Dim currhr As Integer = 0
            ' use the newfiletext
            Dim newfilename As String
            Dim atFinish = False
            Dim delete = False
            Dim deletepos As Long = 0
            prevtime = Nothing
            currtime = Nothing
            Dim measureHR As Boolean = False

            newfilename = Path.GetDirectoryName(filename) & "\" & Path.GetFileNameWithoutExtension(filename) & "_fixed.gpx"

            Dim reversedFileText As String()

            If chkPeed.Checked Then
                reversedFileText = File.ReadAllLines(newfilename).Reverse.ToArray
            Else
                reversedFileText = File.ReadAllLines(filename).Reverse.ToArray
            End If

            linepos = 0
            For Each line As String In reversedFileText
                linepos += 1
                Dim percentdone As Decimal = CDec(linepos) / CDec(linecount) * 100
                If percentdone > 10 And percentdone Mod 10 < 1 And percentdone > oldpos + 1 Then
                    bgwConvert.ReportProgress(percentdone)
                    oldpos = percentdone
                End If
                Dim newline = line
                If measureHR And newline.Contains("<gpxtpx:hr>") Then
                    measureHR = False
                    If prevhr = 0 Then
                        Dim hrstart As Integer = newline.IndexOf("<gpxtpx:hr>") + 11
                        Dim hrend As Integer = newline.LastIndexOf("</gpxtpx:hr>")
                        prevhr = CInt(newline.Substring(hrstart, hrend - hrstart))
                    Else
                        Dim hrstart As Integer = newline.IndexOf("<gpxtpx:hr>") + 11
                        Dim hrend As Integer = newline.LastIndexOf("</gpxtpx:hr>")
                        currhr = CInt(newline.Substring(hrstart, hrend - hrstart))
                        ' looking for HR to stabilize 
                        ' first HR (prevhr) should be say 120
                        ' next one (currhr) should be 121

                        If (currhr > 140) Then
                            If prevhr - currhr >= 1 Then
                                atFinish = True
                            End If
                            ' we're going up, we've reached the end - ie finish point.

                        End If
                        prevhr = currhr

                    End If
                End If

                If newline.Contains("<time>") Then
                    If prevtime = Nothing Then
                        Dim timestart As Integer = newline.IndexOf("<time>") + 6
                        Dim timeend As Integer = newline.LastIndexOf("</time>")
                        prevtime = DateTime.Parse(newline.Substring(timestart, timeend - timestart))
                    Else
                        Dim timestart As Integer = newline.IndexOf("<time>") + 6
                        Dim timeend As Integer = newline.LastIndexOf("</time>")
                        currtime = DateTime.Parse(newline.Substring(timestart, timeend - timestart))
                        If ((prevtime - currtime).TotalSeconds > 30) Then
                            ' measure hr
                            measureHR = True
                            prevtime = currtime
                        End If


                    End If
                End If


                If newline.Contains("<trkpt ") And atFinish Then
                    ' delete from here
                    delete = True
                    deletepos = reversedFileText.LongCount - linepos
                    Exit For
                End If
            Next

            Dim writeArray() As String
            ReDim writeArray(deletepos)

            Array.Copy(reversedFileText.Reverse.ToArray, writeArray, deletepos)
            writeArray.Add("  </trkseg>")
            writeArray.Add(" </trk>")
            writeArray.Add("</gpx>")
            If File.Exists(newfilename) Then File.Delete(newfilename)
            File.WriteAllLines(newfilename, writeArray)

            '    newfiletext &= "  </trkseg>" & vbCrLf
            '    newfiletext &= " </trk>" & vbCrLf
            '    newfiletext &= "</gpx>" & vbCrLf


            'newfiletext = ""
            'Dim BufferSize As Integer = 1024
            'Dim pos As Long = 0
            'If deletepos > 0 Then
            '    Dim fileToRead As String
            '    If chkPeed.Checked Then
            '        fileToRead = newfilename
            '    Else
            '        fileToRead = filename
            '    End If



            '    Using sr As StreamReader = New StreamReader(fileToRead)

            '        Dim line As String = sr.ReadLine()
            '        Do While pos < deletepos And (Not line Is Nothing)
            '            pos += 1
            '            newfiletext &= line & vbCrLf
            '            line = sr.ReadLine()
            '        Loop
            '    End Using

            '    newfiletext &= "  </trkseg>" & vbCrLf
            '    newfiletext &= " </trk>" & vbCrLf
            '    newfiletext &= "</gpx>" & vbCrLf

            '    If File.Exists(newfilename) Then File.Delete(newfilename)
            '    File.WriteAllText(newfilename, newfiletext)
            'End If

            MsgBox("Stop fix complete!")
        End If

    End Sub

    Private Sub bgwConvert_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwConvert.RunWorkerCompleted
        Me.Cursor = Cursors.Arrow
        'MsgBox("Complete!")
    End Sub

    Private Sub bgwConvert_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgwConvert.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub
End Class

Public Module MyExtensions
    <Extension()>
    Public Sub Add(Of T)(ByRef arr As T(), item As T)
        Array.Resize(arr, arr.Length + 1)
        arr(arr.Length - 1) = item
    End Sub
End Module

