Public Class Form1

    'params'
    Dim max As Integer = 120
    Dim min As Integer = 0
    '----------------------------------------------'

    Dim R As New Random
    Dim avg As Double = 0
    Dim list As New List(Of Integer)
    Dim done As Boolean = False
    Dim n As Integer = 0

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        n += 1
        Dim random As Integer = R.Next(min, max)
        avg = avg + (random - avg) / n
        list(random) += 1
        Me.RichTextBox1.AppendText("Age: " & random & "  Average: " & avg & Environment.NewLine)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If done = False Then
            For i As Integer = 0 To (max - 1)
                list.Insert(i, 0)
            Next
            done = True
        End If

        Me.RichTextBox1.AppendText("------------------------------------------------" & Environment.NewLine)
        Me.Timer1.Start()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Timer1.Stop()
        Me.RichTextBox1.AppendText("------------------------------------------------" & Environment.NewLine)
        For i As Integer = 0 To (max - 1)
            Me.RichTextBox1.AppendText("Age: " & i & "  Count: " & list(i) & Environment.NewLine)
        Next

    End Sub
End Class
