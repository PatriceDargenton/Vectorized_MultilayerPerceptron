
' Patrice Dargenton
' Vectorized-MultiLayerPerceptron
' From https://github.com/HectorPulido/Vectorized-multilayer-neural-network : C# -> VB .NET conversion

Option Infer On ' Lambda function

Imports System.Runtime.InteropServices ' OutAttribute

Namespace VectorizedMultiLayerPerceptron

    Module modMatrixMLPTest

        Sub Main()
            Console.WriteLine("Vectorized-MultiLayerPerceptron with the classical XOR test.")
            MatrixMLPTest()
            Console.WriteLine("Press a key to quit.")
            Console.ReadKey()
        End Sub

        Public Sub MatrixMLPTest()

            Dim InputValue As Matrix = New Double(,) {
                {0, 0},
                {0, 1},
                {1, 0},
                {1, 1}}

            Dim TargetValue As Matrix = New Double(,) {
                {0},
                {1},
                {1},
                {0}}

            Dim activFct As New ActivationFunction.SigmoidFunction ' linear
            Dim nbIter = 100000 ' Sigmoid: works
            'Dim activFct As New ActivationFunction.HyperbolicTangentFunction ' linear
            'Dim nbIter = 10000 ' Hyperbolic tangent: works
            'Dim activFct As New ActivationFunction.ELUFunction  ' linear
            'Dim nbIter = 10000 ' ELU: Does not work yet
            'Dim activFct As New ActivationFunction.GaussianFunction ' linear
            'Dim nbIter = 1000 ' Gaussian: works fine
            'Dim activFct As New ActivationFunction.SinusFunction ' linear
            'Dim nbIter = 1000 ' Sinus: works fine
            'Dim activFct As New ActivationFunction.ArcTangentFunction ' linear
            'Dim nbIter = 1000 ' ArcTangent: works fine
            'Dim activFct As New ActivationFunction.ReluFunction ' Non linear
            'Dim nbIter = 100000 ' ReLU: Does not work yet, but this next one yes:
            'Dim activFct As New ActivationFunction.ReluSigmoidFunction ' linear
            'Dim nbIter = 10000 ' ReLUSigmoid: works fine

            Dim lambdaFct = Function(x#) activFct.Activation(x, gain:=1, center:=0)
            Dim lambdaFctD = Function(x#) activFct.Derivative(x, gain:=1, center:=0)
            Dim activFctIsNonLinear = activFct.IsNonLinear

            Dim NeuronCount = New Integer() {2, 3, 3, 1}
            Dim LayerCount = NeuronCount.Length
            Dim ExampleCount% = TargetValue.x
            Dim LearningRate = 0.1
            Dim r = New Random(1)
            Dim W = New Matrix(LayerCount - 1 - 1) {}

            For i = 0 To W.Length - 1
                W(i) = Matrix.Random(NeuronCount(i) + 1, NeuronCount(i + 1), r) * 2 - 1
                'Debug.WriteLine("W(" & i & ")=" & W(i).ToString)
            Next

            For epoch = 0 To nbIter - 1

                Dim error_ As Matrix() = Nothing
                Dim output As Matrix = Nothing
                OneIteration(W, ExampleCount, LayerCount, InputValue, TargetValue,
                    LearningRate, lambdaFct, lambdaFctD, activFctIsNonLinear,
                    error_, output, testOnly:=False, addBiasColumn:=True)

                If (epoch + 1) Mod 1000 = 0 Then

                    Dim LastError = error_(LayerCount - 1)
                    Dim averageErr = LastError.abs.average * ExampleCount
                    Dim sMsg$ =
                        "Loss: " & averageErr.ToString("0.000000") & vbLf &
                        "-------" & epoch + 1 & "----------------" & vbLf &
                        InputValue.ToString() & vbLf & output.ToString()

                    Debug.WriteLine(sMsg)
                    Console.WriteLine(sMsg)

                    'For i = 0 To W.Length - 1
                    '    Debug.WriteLine("W(" & i & ")=" & W(i).ToString)
                    'Next

                End If
            Next

        End Sub

        Public Sub OneIteration(
            W As Matrix(), ExampleCount%, LayerCount%,
            InputValue As Matrix, TargetValue As Matrix, LearningRate#,
            lambdaFct As Func(Of Double, Double),
            lambdaFctD As Func(Of Double, Double),
            activFctIsNonLinear As Boolean,
            ByRef error_ As Matrix(), ByRef output As Matrix,
            testOnly As Boolean, addBiasColumn As Boolean)

            Dim Z As Matrix() = Nothing
            Dim A As Matrix() = Nothing
            ForwardPropagation(Z, A, W, ExampleCount, LayerCount, InputValue,
                lambdaFct, activFctIsNonLinear, addBiasColumn)

            Dim maxLayer% = LayerCount - 1
            Dim maxIndex% = A.Length - 1
            Dim Zlast As Matrix = Z(maxLayer)
            ' Cut first column for last layer
            Dim zx = Z(maxLayer).x
            Dim zy = Z(maxLayer).y
            If addBiasColumn Then Zlast = Zlast.Slice(0, 1, zx, zy)

            output = A(maxIndex)
            ' Cut first column for last index of result matrix
            Dim ax = A(maxIndex).x
            Dim ay = A(maxIndex).y
            If addBiasColumn Then output = output.Slice(0, 1, ax, ay)

            error_ = New Matrix(LayerCount - 1) {}
            error_(LayerCount - 1) = output - TargetValue

            If testOnly Then Exit Sub

            Dim delta As Matrix() = Nothing
            BackPropagation(delta, error_, output, TargetValue, Zlast, W, Z, A, LayerCount,
                lambdaFctD, addBiasColumn)

            GradientDescend(W, A, delta, LearningRate)

        End Sub

        Private Sub ForwardPropagation(
            <Out> ByRef Z As Matrix(), <Out> ByRef A As Matrix(),
            W As Matrix(), ExampleCount%, LayerCount%, InputValue As Matrix,
            lambdaFct As Func(Of Double, Double), activFctIsNonLinear As Boolean,
            addBiasColumn As Boolean)

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

                A(i) = Matrix.Map(Z(i), lambdaFct)
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
            output As Matrix, TargetValue As Matrix, Zlast As Matrix,
            W As Matrix(), Z As Matrix(), A As Matrix(), LayerCount%,
            lambdaFctD As Func(Of Double, Double), addBiasColumn As Boolean)

            delta = New Matrix(LayerCount - 1) {}

            'delta(LayerCount - 1) = error_(LayerCount - 1) * sigmoid(Zlast, derivated:=True)
            delta(LayerCount - 1) = error_(LayerCount - 1) * Matrix.Map(Zlast, lambdaFctD)

            For i = LayerCount - 2 To 0 Step -1

                Dim d = delta(i + 1)
                Dim t = W(i).T
                error_(i) = d * t

                'delta(i) = error_(i) * sigmoid(Z(i), derivated:=True)
                delta(i) = error_(i) * Matrix.Map(Z(i), lambdaFctD)

                ' Cut first column
                If addBiasColumn Then delta(i) = delta(i).Slice(0, 1, delta(i).x, delta(i).y)
            Next

        End Sub

        Private Sub GradientDescend(ByRef W As Matrix(), A As Matrix(), delta As Matrix(),
            LearningRate#)

            For i = 0 To W.Length - 1
                W(i) -= A(i).T * delta(i + 1) * LearningRate
            Next

        End Sub

        'Private Function sigmoid(m As Matrix, Optional derivated As Boolean = False) As Matrix

        '    Dim output As Double(,) = m

        '    Matrix.MatrixLoop(
        '        (Sub(i, j)
        '             If derivated Then
        '                 Dim aux = 1 / (1 + Math.Exp(-output(i, j)))
        '                 output(i, j) = aux * (1 - aux)
        '             Else
        '                 output(i, j) = 1 / (1 + Math.Exp(-output(i, j)))
        '             End If
        '         End Sub), m.x, m.y)

        '    Return output

        'End Function

        'Private Function Relu(m As Matrix, Optional derivated As Boolean = False) As Matrix

        '    Dim output As Double(,) = m

        '    Matrix.MatrixLoop(
        '        (Sub(i, j)
        '             If derivated Then
        '                 output(i, j) = If(output(i, j) > 0, 1, 0) ' Never called!
        '             Else
        '                 output(i, j) = If(output(i, j) > 0, output(i, j), 0)
        '             End If
        '         End Sub), m.x, m.y)

        '    Return output

        'End Function

    End Module

End Namespace
