
Option Infer On ' Lambda function

Imports System.Runtime.InteropServices ' OutAttribute <Out>

Namespace VectorizedMatrixMLP

    Public Class clsVectorizedMatrixMLP

        Public activFct As ActivationFunction.IActivationFunction
        Public lambdaFct As Func(Of Double, Double)
        Public lambdaFctD As Func(Of Double, Double)
        Public activFctIsNonLinear As Boolean

        Public LearningRate#
        Public nbIterations%

        Public InputValue As Matrix
        Public TargetValue As Matrix
        Public output As Matrix

        Public ExampleCount%

        Public W As Matrix()

        Private r As Random
        Private error_ As Matrix()

        Private LayerCount%
        Private NeuronCount%()
        Private addBiasColumn As Boolean = True

        Public Sub New()
        End Sub

        Public Sub InitStruct(aiNeuronCount%(), addBiasColumn As Boolean)

            Me.addBiasColumn = addBiasColumn
            Me.LayerCount = aiNeuronCount.Length
            ReDim Me.NeuronCount(0 To Me.LayerCount - 1)
            For i As Integer = 0 To Me.LayerCount - 1
                Me.NeuronCount(i) = aiNeuronCount(i)
                If addBiasColumn AndAlso
                    i > 0 AndAlso i < Me.LayerCount - 1 Then Me.NeuronCount(i) += 1 ' Bias
            Next
            Me.ExampleCount = Me.TargetValue.x
            Me.W = New Matrix(Me.LayerCount - 1 - 1) {}

        End Sub

        Public Sub Randomize()

            Me.r = New Random(1)

            For i = 0 To Me.W.Length - 1
                Dim iNbNeur% = Me.NeuronCount(i)
                If Me.addBiasColumn Then iNbNeur += 1
                Dim iNbNeurCP1% = Me.NeuronCount(i + 1)
                Me.W(i) = Matrix.Random(iNbNeur, iNbNeurCP1, r) * 2 - 1
            Next

        End Sub

        Public Sub Train()

            For epoch = 0 To Me.nbIterations - 1
                OneIteration(Me.InputValue, Me.ExampleCount, epoch,
                    testOnly:=False, TargetValue:=Me.TargetValue)
            Next

        End Sub

        Public Sub OneIteration(
            InputValue As Matrix, ExampleCount%, Iteration%,
            testOnly As Boolean, Optional TargetValue As Matrix = Nothing)

            Me.error_ = Nothing
            Me.output = Nothing

            Dim Z As Matrix() = Nothing
            Dim A As Matrix() = Nothing
            ForwardPropagation(InputValue, Z, A, ExampleCount)

            Dim maxLayer% = LayerCount - 1
            Dim maxIndex% = A.Length - 1
            Dim Zlast As Matrix = Z(maxLayer)
            ' Cut first column for last layer
            Dim zx = Z(maxLayer).x
            Dim zy = Z(maxLayer).y
            If addBiasColumn Then Zlast = Zlast.Slice(0, 1, zx, zy)

            Me.output = A(maxIndex)
            ' Cut first column for last index of result matrix
            Dim ax = A(maxIndex).x
            Dim ay = A(maxIndex).y
            If addBiasColumn Then Me.output = Me.output.Slice(0, 1, ax, ay)

            Me.error_ = Nothing
            If Not IsNothing(TargetValue) Then
                Me.error_ = New Matrix(Me.LayerCount - 1) {}
                Me.error_(Me.LayerCount - 1) = Me.output - TargetValue
            End If

            If testOnly Then Exit Sub

            Dim delta As Matrix() = Nothing
            BackPropagation(delta, Me.error_, Zlast, Z, A)

            GradientDescend(A, delta, Me.LearningRate)

            If (Iteration < 10 OrElse
                ((Iteration + 1) Mod 100 = 0 AndAlso Iteration < 1000) OrElse
                ((Iteration + 1) Mod 1000 = 0 AndAlso Iteration < 10000) OrElse
                (Iteration + 1) Mod 10000 = 0) Then

                Dim sMsg$ = vbLf &
                    "-------" & Iteration + 1 & "----------------" & vbLf &
                    "Input: " & InputValue.ToString() & vbLf &
                    "Output: " & Me.output.ToString() & vbLf
                'For i = 0 To Me.LayerCount - 1
                '    sMsg &= "Error(" & i & ")=" & Me.error_(i).ToString() & vbLf
                '    sMsg &= "A(" & i & ")=" & A(i).ToString() & vbLf
                'Next

                Dim averageErr! = Me.ComputeAverageError()
                sMsg &= "Loss: " & averageErr.ToString("0.000000") & vbLf

                Debug.WriteLine(sMsg)
                Console.WriteLine(sMsg)

            End If

        End Sub

        Private Sub ForwardPropagation(InputValue As Matrix,
            <Out> ByRef Z As Matrix(), <Out> ByRef A As Matrix(), ExampleCount%)

            Z = New Matrix(LayerCount - 1) {}
            A = New Matrix(LayerCount - 1) {}

            Z(0) = InputValue
            ' Column added with 1 for all examples
            If addBiasColumn Then Z(0) = Z(0).AddColumn(Matrix.Ones(ExampleCount, 1))
            A(0) = Z(0)

            For i = 1 To LayerCount - 1

                Dim AW = A(i - 1) * W(i - 1)

                Z(i) = AW
                ' Column added with 1 for all examples
                If addBiasColumn Then Z(i) = Z(i).AddColumn(Matrix.Ones(ExampleCount, 1))

                A(i) = Matrix.Map(Z(i), Me.lambdaFct)
                'A(i) = sigmoid(Z(i))
                'A(i) = Relu(Z(i))

            Next

            ' How use Relu
            ' Change all sigmoid function, for relu function
            ' Last A must have no Nonlinear function Matrix, Last A must be Equal To Last Z;
            '  because of that Last Delta has not derivated Matrix "Last Delta = Last error Error * 1";
            ' The learning rate must be smaller, like 0.001
            ' Optionaly you can use a Softmax layer to make a clasifier
            ' Use if Relu OR iregularized Values
            If activFctIsNonLinear Then A(A.Length - 1) = Z(Z.Length - 1)

        End Sub

        Private Sub BackPropagation(
            <Out> ByRef delta As Matrix(), error_ As Matrix(),
            Zlast As Matrix, Z As Matrix(), A As Matrix())

            delta = New Matrix(Me.LayerCount - 1) {}

            'delta(LayerCount - 1) = error_(LayerCount - 1) * sigmoid(Zlast, derivated:=True)
            delta(Me.LayerCount - 1) = error_(Me.LayerCount - 1) * Matrix.Map(Zlast, Me.lambdaFctD)

            For i = Me.LayerCount - 2 To 0 Step -1

                Dim d = delta(i + 1)
                Dim t = Me.W(i).T
                Me.error_(i) = d * t

                'delta(i) = error_(i) * sigmoid(Z(i), derivated:=True)
                delta(i) = Me.error_(i) * Matrix.Map(Z(i), Me.lambdaFctD)

                ' Cut first column
                If addBiasColumn Then delta(i) = delta(i).Slice(0, 1, delta(i).x, delta(i).y)
            Next

        End Sub

        Private Sub GradientDescend(A As Matrix(), delta As Matrix(), LearningRate#)

            For i = 0 To W.Length - 1
                Me.W(i) -= A(i).T * delta(i + 1) * Me.LearningRate
            Next

        End Sub

        Public Function ComputeAverageError!()
            Dim LastError = Me.error_(Me.LayerCount - 1)
            Dim averageErr = CSng(LastError.abs.average * Me.ExampleCount)
            Return averageErr
        End Function

        Public Function ComputeAverageError!(targets_array!())
            Dim LastError = Me.error_(Me.LayerCount - 1)
            Dim averageErr = CSng(LastError.abs.average * Me.ExampleCount)
            Return averageErr
        End Function

    End Class

End Namespace