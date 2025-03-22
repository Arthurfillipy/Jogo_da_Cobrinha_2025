Public Class Form1
    Private WithEvents Timer1 As New Timer()
    Dim direcaoAtual As String = "Direita"
    Dim cobra As New List(Of Point)
    Dim comida As Point
    Dim jogoRodando As Boolean = True
    Dim aguardarReinicio As Boolean = False
    Dim jogoPausado As Boolean = False  ' Variável para pausar o jogo
    Dim tamanhoBloco As Integer = 15
    Dim larguraGrade As Integer
    Dim alturaGrade As Integer
    Dim pontuacao As Integer = 0
    Dim corFundo As Color = Color.Black ' Cor inicial de fundo
    Private btnConfig As New Button()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Defina KeyPreview para True
        Me.KeyPreview = True
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        larguraGrade = Me.ClientSize.Width \ tamanhoBloco
        alturaGrade = Me.ClientSize.Height \ tamanhoBloco

        Timer1.Interval = 100
        AddHandler Timer1.Tick, AddressOf Timer1_Tick
        Timer1.Start()
        CriarCobraInicial()
        GerarComida()
        AplicarCorFundo()
        CriarBotaoConfiguracoes()
    End Sub

    ' Método que cria o botão de configurações
    Private Sub CriarBotaoConfiguracoes()
        btnConfig.Text = "⚙ Configurações"
        btnConfig.Size = New Size(150, 40)
        btnConfig.Location = New Point(Me.ClientSize.Width - 160, 10)
        btnConfig.BackColor = Color.Gray
        btnConfig.ForeColor = Color.White
        btnConfig.Font = New Font("Arial", 10, FontStyle.Bold)
        AddHandler btnConfig.Click, AddressOf AbrirConfiguracoes
        Controls.Add(btnConfig)
    End Sub

    ' Método para abrir o menu de configurações
    Private Sub AbrirConfiguracoes(sender As Object, e As EventArgs)
        ' Pausa o jogo ao abrir as configurações
        Timer1.Stop()
        jogoPausado = True

        Dim configForm As New Form With {
            .Size = New Size(400, 300),
            .StartPosition = FormStartPosition.CenterScreen,
            .FormBorderStyle = FormBorderStyle.FixedDialog,
            .Text = "Configurações"
        }

        ' Botões para escolher a cor do fundo
        Dim btnCor1 As New Button With {
            .Text = "Verde",
            .Size = New Size(120, 40),
            .BackColor = Color.Green,
            .ForeColor = Color.White,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .Location = New Point(140, 30)
        }
        AddHandler btnCor1.Click, Sub() AlterarCorFundo(Color.Green, configForm)

        Dim btnCor2 As New Button With {
            .Text = "Azul",
            .Size = New Size(120, 40),
            .BackColor = Color.Blue,
            .ForeColor = Color.White,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .Location = New Point(140, 80)
        }
        AddHandler btnCor2.Click, Sub() AlterarCorFundo(Color.Blue, configForm)

        Dim btnCor3 As New Button With {
            .Text = "Vermelho",
            .Size = New Size(120, 40),
            .BackColor = Color.Red,
            .ForeColor = Color.White,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .Location = New Point(140, 130)
        }
        AddHandler btnCor3.Click, Sub() AlterarCorFundo(Color.Red, configForm)

        ' Botões para adicionar as cores Branca e Preta
        Dim btnCor4 As New Button With {
            .Text = "Branco",
            .Size = New Size(120, 40),
            .BackColor = Color.White,
            .ForeColor = Color.Black,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .Location = New Point(140, 180)
        }
        AddHandler btnCor4.Click, Sub() AlterarCorFundo(Color.White, configForm)

        Dim btnCor5 As New Button With {
            .Text = "Preto",
            .Size = New Size(120, 40),
            .BackColor = Color.Black,
            .ForeColor = Color.White,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .Location = New Point(140, 230)
        }
        AddHandler btnCor5.Click, Sub() AlterarCorFundo(Color.Black, configForm)

        ' Botão de continuar para retomar o jogo
        Dim btnContinuar As New Button With {
            .Text = "Continuar",
            .Size = New Size(120, 40),
            .BackColor = Color.LightGray,
            .Location = New Point(140, 270)
        }
        AddHandler btnContinuar.Click, Sub()
                                           jogoPausado = False
                                           Timer1.Start()  ' Retoma o jogo
                                           configForm.Close()
                                       End Sub

        configForm.Controls.Add(btnCor1)
        configForm.Controls.Add(btnCor2)
        configForm.Controls.Add(btnCor3)
        configForm.Controls.Add(btnCor4)
        configForm.Controls.Add(btnCor5)
        configForm.Controls.Add(btnContinuar)
        configForm.ShowDialog()
    End Sub

    ' Método para aplicar a cor de fundo selecionada
    Private Sub AlterarCorFundo(cor As Color, configForm As Form)
        corFundo = cor
        AplicarCorFundo()
        configForm.Close()
    End Sub

    ' Método para aplicar a cor de fundo à janela principal
    Private Sub AplicarCorFundo()
        Me.BackColor = corFundo
        Me.Invalidate() ' Força a atualização da tela
    End Sub

    ' Criação da cobra inicial
    Private Sub CriarCobraInicial()
        cobra.Clear()
        cobra.Add(New Point(larguraGrade \ 2, alturaGrade \ 2))
        pontuacao = 0
    End Sub

    ' Método para gerar a comida
    Private Sub GerarComida()
        Dim rnd As New Random()
        comida = New Point(rnd.Next(0, larguraGrade), rnd.Next(0, alturaGrade))
    End Sub

    ' Método que é chamado a cada intervalo de tempo
    Private Sub Timer1_Tick(sender As Object, e As EventArgs)
        If Not jogoRodando Then Exit Sub
        MoverCobra()
        Me.Invalidate()
    End Sub

    ' Método para mover a cobra
    Private Sub MoverCobra()
        If jogoPausado Then Exit Sub ' Se o jogo estiver pausado, não mova a cobra

        Dim cabeca = cobra(0)
        Dim novaPosicao As Point = cabeca

        Select Case direcaoAtual
            Case "Esquerda"
                novaPosicao.X -= 1
            Case "Direita"
                novaPosicao.X += 1
            Case "Cima"
                novaPosicao.Y -= 1
            Case "Baixo"
                novaPosicao.Y += 1
        End Select

        If novaPosicao.X < 0 Or novaPosicao.X >= larguraGrade Or novaPosicao.Y < 0 Or novaPosicao.Y >= alturaGrade Or cobra.Contains(novaPosicao) Then
            GameOver()
            Exit Sub
        End If

        cobra.Insert(0, novaPosicao)

        If novaPosicao = comida Then
            pontuacao += 10
            GerarComida()
        Else
            cobra.RemoveAt(cobra.Count - 1)
        End If
    End Sub

    ' Exibe a tela de Game Over
    Private Sub GameOver()
        Timer1.Stop()
        jogoRodando = False
        aguardarReinicio = True
        MostrarTelaGameOver()
    End Sub

    ' Exibe a tela de Game Over
    Private Sub MostrarTelaGameOver()
        Dim gameOverForm As New Form With {
            .Size = New Size(300, 200),
            .StartPosition = FormStartPosition.CenterScreen,
            .FormBorderStyle = FormBorderStyle.FixedDialog,
            .Text = "Game Over",
            .BackColor = Color.DarkRed
        }

        Dim lblPontuacao As New Label With {
            .Text = "Pontuação: " & pontuacao,
            .ForeColor = Color.White,
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .AutoSize = True,
            .Location = New Point(100, 40)
        }

        Dim btnContinuar As New Button With {
            .Text = "Continuar",
            .Size = New Size(100, 40),
            .BackColor = Color.LightGray,
            .Location = New Point(50, 100)
        }
        AddHandler btnContinuar.Click, Sub()
                                           jogoRodando = True
                                           CriarCobraInicial()
                                           GerarComida()
                                           Timer1.Start()
                                           gameOverForm.Close()
                                       End Sub

        Dim btnFechar As New Button With {
            .Text = "Fechar",
            .Size = New Size(100, 40),
            .BackColor = Color.LightGray,
            .Location = New Point(160, 100)
        }
        AddHandler btnFechar.Click, Sub()
                                        Application.Exit()
                                    End Sub

        gameOverForm.Controls.Add(lblPontuacao)
        gameOverForm.Controls.Add(btnContinuar)
        gameOverForm.Controls.Add(btnFechar)
        gameOverForm.ShowDialog()
    End Sub

    ' Evento KeyDown para mudar direção com W, A, S, D
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If jogoPausado Then Exit Sub ' Impede o movimento quando o jogo estiver pausado

        ' Alterando as teclas de movimento para W, A, S, D
        If e.KeyCode = Keys.W And direcaoAtual <> "Baixo" Then
            direcaoAtual = "Cima"
        ElseIf e.KeyCode = Keys.S And direcaoAtual <> "Cima" Then
            direcaoAtual = "Baixo"
        ElseIf e.KeyCode = Keys.A And direcaoAtual <> "Direita" Then
            direcaoAtual = "Esquerda"
        ElseIf e.KeyCode = Keys.D And direcaoAtual <> "Esquerda" Then
            direcaoAtual = "Direita"
        End If
    End Sub

    ' Método de desenho (paint) para desenhar a cobra e a comida
    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        Dim g As Graphics = e.Graphics
        Dim corCobra As Brush = Brushes.Green
        Dim corComida As Brush = Brushes.Red

        For Each p As Point In cobra
            g.FillRectangle(corCobra, p.X * tamanhoBloco, p.Y * tamanhoBloco, tamanhoBloco, tamanhoBloco)
            g.DrawRectangle(Pens.Black, p.X * tamanhoBloco, p.Y * tamanhoBloco, tamanhoBloco, tamanhoBloco)
        Next
        g.FillRectangle(corComida, comida.X * tamanhoBloco, comida.Y * tamanhoBloco, tamanhoBloco, tamanhoBloco)
        g.DrawRectangle(Pens.White, comida.X * tamanhoBloco, comida.Y * tamanhoBloco, tamanhoBloco, tamanhoBloco)
    End Sub
End Class
