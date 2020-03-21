
Option Infer On ' Lambda function

Imports System.Runtime.InteropServices ' OutAttribute <Out>

Namespace VectorizedMatrixMLP

    Public Class clsVectorizedMatrixMLP

        Public activFct As ActivationFunction.IActivationFunction
        Public lambdaFct As Func(Of Double, Double)
        Public lambdaFctD As Func(Of Double, Double)
        Public activFctIsNonLinear As Boolean

        Public learningRate#
        Public nbIterations%

        Public input As Matrix
        Public target As Matrix
        Public targetArray As Single(,)

        ''' <summary>
        ''' Output matrix (returned to compute average error, and discrete error)
        ''' </summary>
        Public outputMatrix As Matrix

        Public exampleCount%

        Public w As Matrix()

        Private r As Random
        Private error_ As Matrix()
        Private lastError As Matrix

        ''' <summary>
        ''' Average error of the output matrix
        ''' </summary>
        Private averageError!

        Private layerCount%
        Private neuronCount%()
        Private addBiasColumn As Boolean = True

        Public Sub InitStruct(aiNeuronCount%(), addBiasColumn As Boolean)

            Me.addBiasColumn = addBiasColumn
            Me.layerCount = aiNeuronCount.Length
            ReDim Me.neuronCount(0 To Me.layerCount - 1)
            For i As Integer = 0 To Me.layerCount - 1
                Me.neuronCount(i) = aiNeuronCount(i)
                If addBiasColumn AndAlso
                    i > 0 AndAlso i < Me.layerCount - 1 Then Me.neuronCount(i) += 1 ' Bias
                'Debug.WriteLine("NeuronCount(" & i & ")=" & Me.NeuronCount(i))
            Next
            Me.exampleCount = Me.target.x
            Me.w = New Matrix(Me.layerCount - 1 - 1) {}

        End Sub

        Public Sub Randomize()

            Me.r = New Random(1)

            For i = 0 To Me.w.Length - 1
                Dim iNbNeur% = Me.neuronCount(i)
                If Me.addBiasColumn Then iNbNeur += 1
                Dim iNbNeurCP1% = Me.neuronCount(i + 1)
                Me.w(i) = Matrix.Random(iNbNeur, iNbNeurCP1, r) * 2 - 1
            Next

        End Sub

        Public Sub PrintWeights()
            For i = 0 To Me.w.Length - 1
                Debug.WriteLine("W(" & i & ")=" & Me.w(i).ToString)
            Next
        End Sub

        Public Sub Train(Optional PrintOutput As Boolean = False)

            For epoch = 0 To Me.nbIterations - 1
                OneIteration(Me.input, Me.exampleCount, epoch,
                    testOnly:=False, TargetValue:=Me.target, PrintOutput:=PrintOutput)
            Next

        End Sub

        Public Sub OneIteration(
            InputValue As Matrix, ExampleCount%, Iteration%, testOnly As Boolean,
            Optional TargetValue As Matrix = Nothing,
            Optional PrintOutput As Boolean = False)

            Me.error_ = Nothing
            Me.outputMatrix = Nothing

            Dim Z As Matrix() = Nothing
            Dim A As Matrix() = Nothing
            ForwardPropagation(InputValue, Z, A, ExampleCount)

            Dim maxLayer% = layerCount - 1
            Dim maxIndex% = A.Length - 1
            Dim Zlast As Matrix = Z(maxLayer)
            ' Cut first column for last layer
            Dim zx = Z(maxLayer).x
            Dim zy = Z(maxLayer).y
            If addBiasColumn Then Zlast = Zlast.Slice(0, 1, zx, zy)

            Me.outputMatrix = A(maxIndex)
            ' Cut first column for last index of result matrix
            Dim ax = A(maxIndex).x
            Dim ay = A(maxIndex).y
            If addBiasColumn Then Me.outputMatrix = Me.outputMatrix.Slice(0, 1, ax, ay)

            Me.error_ = Nothing
            If Not IsNothing(TargetValue) Then
                Me.error_ = New Matrix(Me.layerCount - 1) {}
                Me.error_(Me.layerCount - 1) = Me.outputMatrix - TargetValue
            End If

            If testOnly Then Exit Sub

            Dim delta As Matrix() = Nothing
            BackPropagation(delta, Me.error_, Zlast, Z) ', A)

            GradientDescend(A, delta)

            If PrintOutput AndAlso (Iteration < 10 OrElse
                ((Iteration + 1) Mod 100 = 0 AndAlso Iteration < 1000) OrElse
                ((Iteration + 1) Mod 1000 = 0 AndAlso Iteration < 10000) OrElse
                (Iteration + 1) Mod 10000 = 0) Then

                Dim sMsg$ = vbLf &
                    "-------" & Iteration + 1 & "----------------" & vbLf &
                    "Input: " & InputValue.ToString() & vbLf &
                    "Output: " & Me.outputMatrix.ToString() & vbLf
                'For i = 0 To Me.LayerCount - 1
                '    sMsg &= "Error(" & i & ")=" & Me.error_(i).ToString() & vbLf
                '    sMsg &= "A(" & i & ")=" & A(i).ToString() & vbLf
                'Next

                ComputeAverageError()
                sMsg &= "Loss: " & Me.averageError.ToString("0.000000") & vbLf

                Debug.WriteLine(sMsg)
                Console.WriteLine(sMsg)

            End If

        End Sub

        Private Sub ForwardPropagation(InputValue As Matrix,
            <Out> ByRef Z As Matrix(), <Out> ByRef A As Matrix(), ExampleCount%)

            Z = New Matrix(layerCount - 1) {}
            A = New Matrix(layerCount - 1) {}

            Z(0) = InputValue
            ' Column added with 1 for all examples
            If addBiasColumn Then Z(0) = Z(0).AddColumn(Matrix.Ones(ExampleCount, 1))
            A(0) = Z(0)

            For i = 1 To layerCount - 1

                Dim AW = A(i - 1) * w(i - 1)

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
            Zlast As Matrix, Z As Matrix()) ', A As Matrix())

            delta = New Matrix(Me.layerCount - 1) {}

            'delta(LayerCount - 1) = error_(LayerCount - 1) * sigmoid(Zlast, derivated:=True)
            delta(Me.layerCount - 1) = error_(Me.layerCount - 1) * Matrix.Map(Zlast, Me.lambdaFctD)

            For i = Me.layerCount - 2 To 0 Step -1

                Dim d = delta(i + 1)
                Dim t = Me.w(i).T
                Me.error_(i) = d * t

                'delta(i) = error_(i) * sigmoid(Z(i), derivated:=True)
                delta(i) = Me.error_(i) * Matrix.Map(Z(i), Me.lambdaFctD)

                ' Cut first column
                If addBiasColumn Then delta(i) = delta(i).Slice(0, 1, delta(i).x, delta(i).y)
            Next

        End Sub

        Private Sub GradientDescend(A As Matrix(), delta As Matrix())

            For i = 0 To w.Length - 1
                Me.w(i) -= A(i).T * delta(i + 1) * Me.learningRate
            Next

        End Sub

        Public Sub ComputeError()
            ' Calculate the error: ERROR = TARGETS - OUTPUTS
            Dim m As Matrix = Me.targetArray
            Me.lastError = m - Me.outputMatrix
        End Sub

        Public Sub ComputeAverageErrorFromLastError()
            Me.lastError = Me.error_(Me.layerCount - 1)
            ' Compute first abs then average:
            Me.averageError = CSng(Me.lastError.abs.average * Me.exampleCount)
        End Sub

        Public Function ComputeAverageError!()

            Me.ComputeError()
            Me.ComputeAverageErrorFromLastError()
            Return Me.averageError

        End Function

    End Class

End Namespace