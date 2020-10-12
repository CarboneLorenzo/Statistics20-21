Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.RichTextBox1.Text = "Pong" + Environment.NewLine
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.RichTextBox1.ResetText()
    End Sub

    Private Sub RichTextBox1_MouseEnter(sender As Object, e As EventArgs) Handles RichTextBox1.MouseEnter
        Me.RichTextBox1.BackColor = Color.Red
    End Sub

    Private Sub RichTextBox1_MouseLeave(sender As Object, e As EventArgs) Handles RichTextBox1.MouseLeave
        Me.RichTextBox1.BackColor = Color.White
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.AllowDrop = True
        Me.RichTextBox1.AllowDrop = True
    End Sub

    Private Sub RichTextBox1_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles RichTextBox1.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each path In files
            Me.RichTextBox1.AppendText(path + Environment.NewLine)
        Next
    End Sub

    Private Sub RichTextBox1_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles RichTextBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub
End Class
