Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class Form1
    Public address As String
    Public privateKey As String

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sha256 As New SHA256Managed
        Dim crypt As Byte()
        Dim password As String = TextBox1.Text
        crypt = sha256.ComputeHash(Encoding.UTF8.GetBytes("KRISTWALLET" + password))

        Dim out As String = ""
        Dim i As Integer
        For i = 0 To crypt.Length - 1
            out += crypt(i).ToString("x2")
        Next
        out += "-000"

        privateKey = out

        Using client As New Net.WebClient
            Dim reqparm As New Specialized.NameValueCollection
            reqparm.Add("privatekey", out)
            Dim responsebytes = client.UploadValues("http://krist.ceriat.net/v2", "POST", reqparm)
            Dim responsebody = (New System.Text.UTF8Encoding).GetString(responsebytes)
            address = responsebody.Substring(22, 10)
        End Using

        otherpage.Show()
        Me.Close()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub
End Class
