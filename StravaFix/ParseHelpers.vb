Imports System
Imports System.IO
Imports System.Web.Script.Serialization ' Add reference: System.Web.Extensions
Imports System.Xml
Imports System.Xml.Serialization

Imports System.Runtime.CompilerServices

Friend Module ParseHelpers
    Private json As JavaScriptSerializer

    Private ReadOnly Property _JSON As JavaScriptSerializer
        Get
            Return If(json, CSharpImpl.__Assign(json, New JavaScriptSerializer()))
        End Get
    End Property

    <Extension()>
    Function ToStream(ByVal this As String) As Stream
        Dim stream = New MemoryStream()
        Dim writer = New StreamWriter(stream)
        writer.Write(this)
        writer.Flush()
        stream.Position = 0
        Return stream
    End Function

    <Extension()>
    Function ParseXML(Of T As Class)(ByVal this As String) As T
        Dim reader = XmlReader.Create(this.Trim().ToStream(), New XmlReaderSettings() With {
            .ConformanceLevel = ConformanceLevel.Document
        })
        Return TryCast(New XmlSerializer(GetType(T)).Deserialize(reader), T)
    End Function

    <Extension()>
    Function ParseJSON(Of T As Class)(ByVal this As String) As T
        Return json.Deserialize(Of T)(this.Trim())
    End Function

    Private Class CSharpImpl
        <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
        Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class
End Module
