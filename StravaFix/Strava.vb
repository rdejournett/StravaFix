Public Class Strava

End Class

' NOTE: Generated code may require at least .NET Framework 4.5 Or .NET Core/Standard 2.0.
'''<remarks/>
<System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.topografix.com/GPX/1/1"),
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://www.topografix.com/GPX/1/1", IsNullable:=False)>
Partial Public Class gpx

    Private metadataField As gpxMetadata

    Private trkField As gpxTrk

    Private creatorField As String

    Private versionField As Decimal

    '''<remarks/>
    Public Property metadata() As gpxMetadata
        Get
            Return Me.metadataField
        End Get
        Set
            Me.metadataField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property trk() As gpxTrk
        Get
            Return Me.trkField
        End Get
        Set
            Me.trkField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property creator() As String
        Get
            Return Me.creatorField
        End Get
        Set
            Me.creatorField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property version() As Decimal
        Get
            Return Me.versionField
        End Get
        Set
            Me.versionField = Value
        End Set
    End Property
End Class

'''<remarks/>
<System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.topografix.com/GPX/1/1")>
Partial Public Class gpxMetadata

    Private timeField As Date

    '''<remarks/>
    Public Property time() As Date
        Get
            Return Me.timeField
        End Get
        Set
            Me.timeField = Value
        End Set
    End Property
End Class

'''<remarks/>
<System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.topografix.com/GPX/1/1")>
Partial Public Class gpxTrk

    Private nameField As String

    Private typeField As Byte

    Private trksegField() As gpxTrkTrkpt

    '''<remarks/>
    Public Property name() As String
        Get
            Return Me.nameField
        End Get
        Set
            Me.nameField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property type() As Byte
        Get
            Return Me.typeField
        End Get
        Set
            Me.typeField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlArrayItemAttribute("trkpt", IsNullable:=False)>
    Public Property trkseg() As gpxTrkTrkpt()
        Get
            Return Me.trksegField
        End Get
        Set
            Me.trksegField = Value
        End Set
    End Property
End Class

'''<remarks/>
<System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.topografix.com/GPX/1/1")>
Partial Public Class gpxTrkTrkpt

    Private eleField As Decimal

    Private timeField As Date

    Private extensionsField As gpxTrkTrkptExtensions

    Private latField As Decimal

    Private lonField As Decimal

    '''<remarks/>
    Public Property ele() As Decimal
        Get
            Return Me.eleField
        End Get
        Set
            Me.eleField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property time() As Date
        Get
            Return Me.timeField
        End Get
        Set
            Me.timeField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property extensions() As gpxTrkTrkptExtensions
        Get
            Return Me.extensionsField
        End Get
        Set
            Me.extensionsField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property lat() As Decimal
        Get
            Return Me.latField
        End Get
        Set
            Me.latField = Value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()>
    Public Property lon() As Decimal
        Get
            Return Me.lonField
        End Get
        Set
            Me.lonField = Value
        End Set
    End Property
End Class

'''<remarks/>
<System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.topografix.com/GPX/1/1")>
Partial Public Class gpxTrkTrkptExtensions

    Private trackPointExtensionField As TrackPointExtension

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute([Namespace]:="http://www.garmin.com/xmlschemas/TrackPointExtension/v1")>
    Public Property TrackPointExtension() As TrackPointExtension
        Get
            Return Me.trackPointExtensionField
        End Get
        Set
            Me.trackPointExtensionField = Value
        End Set
    End Property
End Class

'''<remarks/>
<System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.garmin.com/xmlschemas/TrackPointExtension/v1"),
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://www.garmin.com/xmlschemas/TrackPointExtension/v1", IsNullable:=False)>
Partial Public Class TrackPointExtension

    Private hrField As Byte

    Private cadField As Byte

    '''<remarks/>
    Public Property hr() As Byte
        Get
            Return Me.hrField
        End Get
        Set
            Me.hrField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property cad() As Byte
        Get
            Return Me.cadField
        End Get
        Set
            Me.cadField = Value
        End Set
    End Property
End Class

