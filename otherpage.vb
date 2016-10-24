Public Class otherpage
    Dim address As String
    Dim privateKey As String

    Public Function midReturn(ByVal first As String, ByVal last As String, ByVal total As String) As String
        If last.Length < 1 Then
            midReturn = total.Substring(total.IndexOf(first))
        End If
        If first.Length < 1 Then
            midReturn = total.Substring(0, (total.IndexOf(last)))
        End If
        Try
            midReturn = ((total.Substring(total.IndexOf(first), (total.IndexOf(last) - total.IndexOf(first)))).Replace(first, "")).Replace(last, "")
        Catch ArgumentOutOfRangeException As Exception
        End Try
    End Function

    Private Sub otherpage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        address = Form1.address
        privateKey = Form1.privateKey
        address_text.Text = address

        Dim request As String = String.Format("http://krist.ceriat.net/addresses/{0}", address)
        Dim webClient As New System.Net.WebClient
        Dim result As String = webClient.DownloadString(request)

        If (result.Substring(6, 5) = "false") Then
            balance_placeholder.Text = "0"
        Else
            balance_placeholder.Text = midReturn("balance"":", ",""totalin", result)
        End If


        Dim request2 As String = "http://krist.ceriat.net/addresses/rich?limit=16"
        Dim result2 As String = webClient.DownloadString(request2)


        Dim strLine() As String = result2.Split(New String() {"address"}, StringSplitOptions.RemoveEmptyEntries)

        For Each line As String In strLine.Skip(2)
            ListBox1.Items.Add(line.Substring(3, 10))
            ListBox2.Items.Add(midReturn("balance"":", ",""totalin", line))
        Next




        Dim request3 As String = String.Format("http://krist.ceriat.net/addresses/{0}/names", address)
        Dim result3 As String = webClient.DownloadString(request3)
        'MessageBox.Show(result3)

        Dim strLine1() As String = result3.Split(New String() {"name"""}, StringSplitOptions.RemoveEmptyEntries)

        For Each line1 As String In strLine1.Skip(1)
            'MessageBox.Show(line1)
            ListBox3.Items.Add(line1.Substring(2, InStr(line1, "owner") - 6))
            Dim a As String = midReturn("a"":""", """}", line1)
            If (a <> "") Then
                ListBox4.Items.Add(a)
            Else
                ListBox4.Items.Add("null")
            End If
        Next

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using client As New Net.WebClient
            Dim reqparm As New Specialized.NameValueCollection
            reqparm.Add("privatekey", privateKey)
            reqparm.Add("to", TextBox1.Text)
            reqparm.Add("amount", TextBox2.Text)
            Dim responsebytes = client.UploadValues("http://krist.ceriat.net/transactions/", "POST", reqparm)
            Dim responsebody = (New System.Text.UTF8Encoding).GetString(responsebytes)
            If (responsebody.Substring(6, 4) = "true") Then
                MessageBox.Show("Successfully sent KST!")
            Else
                MessageBox.Show("Insufficient funds!")
            End If
        End Using
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        My.Computer.Clipboard.SetText(address)
    End Sub

    Private Sub TableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles TableLayoutPanel1.Paint

    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim request As String = String.Format("http://krist.ceriat.net/addresses/{0}", address)
        Dim webClient As New System.Net.WebClient
        Dim result As String = webClient.DownloadString(request)

        If (result.Substring(6, 5) = "false") Then
            balance_placeholder.Text = "0"
        Else
            balance_placeholder.Text = midReturn("balance"":", ",""totalin", result)
        End If
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        Dim request As String = String.Format("http://krist.ceriat.net/addresses/{0}", address)
        Dim webClient As New System.Net.WebClient
        Dim result As String = webClient.DownloadString(request)

        If (result.Substring(6, 5) = "false") Then
            balance_placeholder.Text = "0"
        Else
            balance_placeholder.Text = midReturn("balance"":", ",""totalin", result)
        End If
    End Sub

    Private Sub ListBox3_DoubleClick(sender As Object, e As EventArgs) Handles ListBox3.DoubleClick
        MessageBox.Show(ListBox3.SelectedIndex)
    End Sub

    Private Sub ListBox4_DoubleClick(sender As Object, e As EventArgs) Handles ListBox4.DoubleClick
        MessageBox.Show(ListBox4.SelectedIndex)
    End Sub

    Private Sub ListBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox3.SelectedIndexChanged
        TextBox3.Text = ListBox4.Items.Item(ListBox3.SelectedIndex).ToString()
        Label10.Text = ListBox3.Items.Item(ListBox3.SelectedIndex).ToString()
        ListBox4.SelectedIndex = ListBox3.SelectedIndex
    End Sub

    Private Sub ListBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox4.SelectedIndexChanged
        ListBox3.SelectedIndex = ListBox4.SelectedIndex
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Using client As New Net.WebClient
            Dim reqparm As New Specialized.NameValueCollection
            reqparm.Add("privatekey", privateKey)
            reqparm.Add("a", TextBox3.Text)
            Dim responsebytes = client.UploadValues("http://krist.ceriat.net/names/" + Label10.Text + "/update", "POST", reqparm)
            Dim responsebody = (New System.Text.UTF8Encoding).GetString(responsebytes)
            If (responsebody.Substring(6, 4) = "true") Then
                MessageBox.Show("Successfully updated A record!")
                Dim request3 As String = String.Format("http://krist.ceriat.net/addresses/{0}/names", address)
                Dim webClient As New System.Net.WebClient

                Dim result3 As String = webClient.DownloadString(request3)

                Dim strLine1() As String = result3.Split(New String() {"name"""}, StringSplitOptions.RemoveEmptyEntries)
                ListBox3.Items.Clear()
                ListBox4.Items.Clear()
                For Each line1 As String In strLine1.Skip(1)

                    ListBox3.Items.Add(line1.Substring(2, InStr(line1, "owner") - 6))
                    Dim a As String = midReturn("a"":""", """}", line1)
                    If (a <> "") Then
                        ListBox4.Items.Add(a)
                    Else
                        ListBox4.Items.Add("null")
                    End If
                Next
            Else
                MessageBox.Show("An error occured!")
            End If
        End Using
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Using client As New Net.WebClient
            Dim reqparm As New Specialized.NameValueCollection
            reqparm.Add("privatekey", privateKey)
            reqparm.Add("address", TextBox4.Text)
            Dim responsebytes = client.UploadValues("http://krist.ceriat.net/names/" + Label10.Text + "/transfer", "POST", reqparm)
            Dim responsebody = (New System.Text.UTF8Encoding).GetString(responsebytes)
            If (responsebody.Substring(6, 4) = "true") Then
                MessageBox.Show("Successfully transferred!")
                Dim request3 As String = String.Format("http://krist.ceriat.net/addresses/{0}/names", address)
                Dim webClient As New System.Net.WebClient

                Dim result3 As String = webClient.DownloadString(request3)

                Dim strLine1() As String = result3.Split(New String() {"name"""}, StringSplitOptions.RemoveEmptyEntries)
                ListBox3.Items.Clear()
                ListBox4.Items.Clear()
                For Each line1 As String In strLine1.Skip(1)

                    ListBox3.Items.Add(line1.Substring(2, InStr(line1, "owner") - 6))
                    Dim a As String = midReturn("a"":""", """}", line1)
                    If (a <> "") Then
                        ListBox4.Items.Add(a)
                    Else
                        ListBox4.Items.Add("null")
                    End If
                Next
                TextBox4.Text = ""
            Else
                MessageBox.Show("An error occured!")
            End If
        End Using
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Form1.Show()
        Me.Close()
    End Sub
End Class