Imports System.Runtime.CompilerServices
Imports System.IO
Imports Gavaghan.Geodesy
Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form1
    Private totalDistance As Double = 0
    Private filename As String
    Private _Step As Integer


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
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
                        If ((prevtime - currtime).TotalSeconds > 10) Then
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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnShowMovingAvg.Click
        'Dim filetext As String()
        If (filename.Length = 0) Then MsgBox("You must drag a Strava gpx file")
        Dim xml As String = File.ReadAllText(filename)
        Dim catalog1 As gpx = xml.ParseXML(Of gpx)
        Dim trk As New gpxTrk()
        trk = catalog1.trk

        Dim trksegList As New List(Of gpxTrkTrkpt)
        trksegList = trk.trkseg.ToList

        Dim dt As New DataTable
        dt.Columns.Add("Time", GetType(DateTime))
        dt.Columns.Add("Lat", GetType(Decimal))
        dt.Columns.Add("Long", GetType(Decimal))
        dt.Columns.Add("Elv", GetType(Decimal))
        dt.Columns.Add("Distance", GetType(Decimal))
        dt.Columns.Add("Speed", GetType(Decimal))

        For Each t In trksegList
            dt.Rows.Add(t.time, t.lat, t.lon, t.ele, 0)
        Next
        DataGridView1.DataSource = dt

        For i As Integer = 1 To dt.Rows.Count - 2
            Dim speed As Double = ComputeSpeed(dt.Rows(i), dt.Rows(i + 1))
            If speed <> 0 Then
                dt.Rows(i)("Speed") = speed
            End If
        Next
        CreateGraph(dt)

        'MsgBox(" fix complete! - Distance is " & totalDistance)
        lblDistance.Text = String.Format("Distance: {0:0.0} miles", totalDistance)
        Dim dt1 As DateTime = dt.Rows(0)("Time")
        Dim dt2 As DateTime = dt.Rows(dt.Rows.Count - 1)("Time")
        Dim ts As TimeSpan = dt2.Subtract(dt1)
        Dim hours As Double = ts.TotalHours
        Dim totalSpeed As Double = totalDistance / hours
        Dim minMile As Double = 60 / totalSpeed
        Dim minutes As Double = (totalSpeed - Math.Truncate(totalSpeed)) * 60


        lblSpeed.Text = String.Format("Speed: {0:0.0} mph", 60 / totalSpeed)
        lblSpeed.Text = String.Format("Speed: {0:0}:{1:00}", Math.Truncate(minMile), Math.Truncate(minutes))
    End Sub
    Private Function ComputeSpeed(ByRef row1 As DataRow, row2 As DataRow) As Double
        Dim geoCalc As New GeodeticCalculator()
        Dim reference As Ellipsoid = Ellipsoid.WGS84
        Dim pt1 As New GlobalPosition(New GlobalCoordinates(New Angle(row1("Lat")), New Angle(row1("Long"))), row1("Elv"))
        Dim pt2 As New GlobalPosition(New GlobalCoordinates(New Angle(row2("Lat")), New Angle(row2("Long"))), row2("Elv"))
        Dim geoMeasurement As New GeodeticMeasurement
        Dim p2pMiles As Double

        geoMeasurement = geoCalc.CalculateGeodeticMeasurement(reference, pt1, pt2)
        p2pMiles = geoMeasurement.PointToPointDistance / 1000.0 * 0.621371192
        Dim dt1 As DateTime = row1("Time")
        Dim dt2 As DateTime = row2("Time")
        Dim ts As TimeSpan = dt2.Subtract(dt1)
        Dim hours As Double = ts.TotalHours
        Dim speed As Double = p2pMiles / hours
        ComputeSpeed = speed
        totalDistance += p2pMiles
        row1("Distance") = totalDistance
        row1("Speed") = speed

    End Function

    Private Sub CreateGraph(dt As DataTable)
        Me.Chart1.Series.Clear()
        Me.Chart1.Series.Add("Series1")
        Chart1.Series("Series1").ChartType = SeriesChartType.Point
        Chart1.Series("Series1").MarkerSize = 2
        Chart1.ChartAreas(0).AxisX.Title = "Distance"
        Chart1.ChartAreas(0).AxisY.Title = "Speed"

        For Each row As DataRow In dt.Rows
            Dim Xval As Decimal
            Dim Yval As Decimal
            If Not IsDBNull(row("Distance")) Then Xval = row("Distance")
            If Not IsDBNull(row("Speed")) Then Yval = row("Speed")
            If Yval <> 0 And Xval <> 0 Then Chart1.Series(0).Points.AddXY(Xval, Yval)

        Next

        AddMovingAverage(20)
    End Sub

    Private Sub AddMovingAverage(_step As Integer)
        Chart1.DataManipulator.CopySeriesValues("Series1", "Series2")
        Chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, _step.ToString, "Series1:Y", "Series2:Y")
        Chart1.Series("Series2").ChartType = SeriesChartType.FastLine
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Chart1_MouseEnter(sender As Object, e As EventArgs) Handles Chart1.MouseEnter
        Me.Chart1.Focus()

    End Sub

    Private Sub Chart1_MouseLeave(sender As Object, e As EventArgs) Handles Chart1.MouseLeave
        Me.Chart1.Parent.Focus()

    End Sub

    Private Sub btnUp_Click(sender As Object, e As EventArgs) Handles btnUp.Click
        _Step += 1
        If _Step <= 0 Then Exit Sub
        Chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, _Step.ToString, "Series1:Y", "Series2:Y")
    End Sub

    Private Sub btnDown_Click(sender As Object, e As EventArgs) Handles btnDown.Click

        _Step -= 1
        If _Step <= 0 Then Exit Sub
        Chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, _Step.ToString, "Series1:Y", "Series2:Y")

    End Sub

    Private Sub Chart1_MouseMove(sender As Object, e As MouseEventArgs) Handles Chart1.MouseMove
        If Chart1.Series.Count = 0 Then Exit Sub
        If Chart1.Series(0).Points.Count = 0 Then Exit Sub

        Dim result As HitTestResult = Chart1.HitTest(e.X, e.Y)
        Dim PointIndex As Integer = result.PointIndex
        If result.PointIndex > 0 Then
            If result.Series IsNot Nothing Then

                If result.Series.Name = "Series1" Then
                    For Each pt As DataPoint In Chart1.Series(0).Points
                        pt.Label = Nothing
                    Next
                    Dim speed As String = String.Format("{0:0.0 mph}", Chart1.Series("Series1").Points(PointIndex).YValues(0))
                    Chart1.Series("Series1").Points(PointIndex).Label = speed
                End If
            End If
            'For Each pt As DataPoint In Chart1.Series(1).Points
            '    pt.Label = Nothing
            '    '    If e.X = pt.XValue Then
            '    '        pt.Label = String.Format("{0:0.0 mph", pt.YValues(1))
            '    '    End If
            'Next
            'Chart1.Series("Series 1").Points(PointIndex).MarkerSize = 12
            'Chart1.Series("Series 1").Points(PointIndex).Color = Color.Red
            'Dim speed As String = String.Format("{0:0.0 mph}", Chart1.Series("Series2").Points(PointIndex).YValues(0))
            'Chart1.Series("Series2").Points(PointIndex).Label = speed
        End If
    End Sub
End Class



Public Module MyExtensions
    <Extension()>
    Public Sub Add(Of T)(ByRef arr As T(), item As T)
        Array.Resize(arr, arr.Length + 1)
        arr(arr.Length - 1) = item
    End Sub
End Module

