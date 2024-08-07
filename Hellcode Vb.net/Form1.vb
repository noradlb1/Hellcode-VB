Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' النص الأصلي
        Dim originalString As String = TextBox2.Text

        ' تحويل كل حرف إلى قيمته العددية ASCII
        Dim bytesList As New List(Of Integer)
        For Each c As Char In originalString
            bytesList.Add(Asc(c))
        Next

        ' إعادة تحويل القيم العددية إلى نص
        Dim decodedString As String = ""
        For Each byteValue As Integer In bytesList
            decodedString &= Chr(byteValue)
        Next

        ' عرض النتائج
        TextBox1.Text = "Original String: " & originalString & Environment.NewLine
        TextBox1.Text &= "Bytes List: " & String.Join(", ", bytesList) & Environment.NewLine
        TextBox1.Text &= "Decoded String: " & decodedString
    End Sub
End Class
