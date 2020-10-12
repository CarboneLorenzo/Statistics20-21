Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.RichTextBox1.Text = "Pong"
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
End Class
