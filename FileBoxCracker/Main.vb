Imports System.Security.Cryptography
Imports System.Text
Imports Microsoft.VisualBasic.CompilerServices
Public Class Main
    Dim DirectorioRaiz As String = "C:\Users\" + Environment.UserName + "\FileBoxData"
    Dim ArchivoDB As String = DirectorioRaiz & "\DataBase_FileBox.db"
    Public Shared tripleDESCryptoServiceProvider_0 As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
    Public Shared md5CryptoServiceProvider_0 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
    Dim Llave As String = "Pw3jakfBIVYydqFZhjyQLwaJTWGPn9"

    Dim UserName As String
    Dim Password As String
    Dim Email As String
    Dim IsRegistered As String
    Dim CryptoKey As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LoadDB()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        UserName = TextBox1.Text
        Password = TextBox2.Text
        Email = TextBox3.Text
        IsRegistered = TextBox4.Text
        CryptoKey = TextBox5.Text
        SaveDB()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        RichTextBox1.AppendText(vbCrLf & Desencriptar(RichTextBox2.Text))
    End Sub

    Public Sub LoadDB()
        Try
            Dim RAWContent As String = My.Computer.FileSystem.ReadAllText(ArchivoDB)
            Dim DecryptContent As String = Desencriptar(RAWContent)
            Dim lines As String() = New TextBox() With {
            .Text = DecryptContent
            }.Lines
            RichTextBox1.AppendText(vbCrLf & "<--- DB Raw Content --->")
            RichTextBox1.AppendText(vbCrLf & RAWContent)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Decrypted Content Loaded --->")
            RichTextBox1.AppendText(vbCrLf & DecryptContent)
            Me.UserName = lines(0).Split(New Char() {">"c})(1).Trim()
            Me.Password = lines(1).Split(New Char() {">"c})(1).Trim()
            Me.Email = lines(2).Split(New Char() {">"c})(1).Trim()
            Me.IsRegistered = lines(3).Split(New Char() {">"c})(1).Trim()
            Me.CryptoKey = lines(4).Split(New Char() {">"c})(1).Trim()

            TextBox1.Text = UserName
            TextBox2.Text = Password
            TextBox3.Text = Email
            TextBox4.Text = IsRegistered
            TextBox5.Text = CryptoKey
            Console.WriteLine("USER DB:" &
                              vbCrLf &
                              "    " & UserName & vbCrLf &
                              "    " & Password & vbCrLf &
                              "    " & Email & vbCrLf &
                              "    " & IsRegistered & vbCrLf &
                              "    " & CryptoKey)
            RichTextBox1.ScrollToCaret()
        Catch ex As Exception
            Console.WriteLine("[Debugger@GetData]Error: " + ex.Message)
        End Try
    End Sub

    Sub SaveDB()
        Try
            If My.Computer.FileSystem.FileExists(ArchivoDB) = True Then
                My.Computer.FileSystem.DeleteFile(ArchivoDB)
            End If
            Dim FormatoDB As String = "Username>" & Me.UserName &
                            vbCrLf & "Password>" & Me.Password &
                            vbCrLf & "Email>" & Me.Email &
                            vbCrLf & "IsRegistered>" & Me.IsRegistered &
                            vbCrLf & "CryptoKey>" & Me.CryptoKey
            Dim EncryptedContent As String = Encriptar(FormatoDB)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Raw Content --->")
            RichTextBox1.AppendText(vbCrLf & FormatoDB)
            RichTextBox1.AppendText(vbCrLf & "<--- DB Encrypted Content Saved --->")
            RichTextBox1.AppendText(vbCrLf & EncryptedContent)
            My.Computer.FileSystem.WriteAllText(Me.ArchivoDB, EncryptedContent, False)
            RichTextBox1.ScrollToCaret()
            LoadDB()
        Catch ex As Exception
        End Try
    End Sub

    Public Function Encriptar(string_1 As String) As String
        Dim result As String
        If Operators.CompareString(Strings.Trim(string_1), "", False) <> 0 Then
            tripleDESCryptoServiceProvider_0.Key = md5CryptoServiceProvider_0.ComputeHash(New UnicodeEncoding().GetBytes(Me.Llave))
            tripleDESCryptoServiceProvider_0.Mode = CipherMode.ECB
            Dim cryptoTransform As ICryptoTransform = tripleDESCryptoServiceProvider_0.CreateEncryptor()
            Dim bytes As Byte() = Encoding.ASCII.GetBytes(string_1)
            result = Convert.ToBase64String(cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length))
        Else
            result = ""
        End If
        Return result
    End Function

    Public Function Desencriptar(string_1 As String) As String
        Dim result As String
        If Operators.CompareString(Strings.Trim(string_1), "", False) <> 0 Then
            tripleDESCryptoServiceProvider_0.Key = md5CryptoServiceProvider_0.ComputeHash(New UnicodeEncoding().GetBytes(Me.Llave))
            tripleDESCryptoServiceProvider_0.Mode = CipherMode.ECB
            Dim cryptoTransform As ICryptoTransform = tripleDESCryptoServiceProvider_0.CreateDecryptor()
            Dim array As Byte() = Convert.FromBase64String(string_1)
            result = Encoding.ASCII.GetString(cryptoTransform.TransformFinalBlock(array, 0, array.Length))
        Else
            result = ""
        End If
        Return result
    End Function
End Class
