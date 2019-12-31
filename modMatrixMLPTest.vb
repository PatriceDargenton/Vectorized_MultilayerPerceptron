
' Patrice Dargenton
' Vectorized-MultiLayerPerceptron
' From https://github.com/HectorPulido/Vectorized-multilayer-neural-network : C# -> VB .NET conversion

Option Infer On ' Lambda function

Imports System.Runtime.InteropServices

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

            Dim OutputValue As Matrix = New Double(,) {
                {0},
                {1},
                {1},
                {0}}

            Dim NeuronCount = New Integer() {2, 3, 3, 1}
            Dim LayerCount = NeuronCount.Length
            Dim ExampleCount% = OutputValue.x
            Dim LearningRate = 0.1
            Dim r = New Random(1)
            Dim W = New Matrix(LayerCount - 1 - 1) {}

            For i = 0 To W.Length - 1
                W(i) = Matrix.Random(NeuronCount(i) + 1, NeuronCount(i + 1), r) * 2 - 1
            Next

            Dim iNbIter = 10001

            For epoch = 0 To iNbIter - 1

                Dim Z As Matrix() = Nothing
                Dim A As Matrix() = Nothing
                ForwardPropagation(Z, A, W, ExampleCount, LayerCount, InputValue)

                Dim Zlast As Matrix = Z(LayerCount - 1).Slice(0, 1, Z(LayerCount - 1).x, Z(LayerCount - 1).y)
                Dim output As Matrix = A(A.Length - 1).Slice(0, 1, A(A.Length - 1).x, A(A.Length - 1).y)

                Dim error_ As Matrix() = Nothing
                Dim delta As Matrix() = Nothing
                BackPropagation(delta, error_, output, OutputValue, Zlast, W, Z, A, LayerCount)

                Dim LastError As Matrix = error_(LayerCount - 1)
                GradientDescend(W, A, delta, LearningRate)

                If epoch Mod 1000 = 0 Then

                    Dim sMsg$ =
                        "Loss: " & LastError.abs.average * ExampleCount & vbLf &
                        "-------" & epoch & "----------------" & vbLf &
                        InputValue.ToString() & vbLf & output.ToString()

                    Debug.WriteLine(sMsg)
                    Console.WriteLine(sMsg)

                End If
            Next

        End Sub

        Private Sub ForwardPropagation(
            <Out> ByRef Z As Matrix(), <Out> ByRef A As Matrix(),
            W As Matrix(), ExampleCount%, LayerCount%, InputValue As Matrix)

            Z = New Matrix(LayerCount - 1) {}
            A = New Matrix(LayerCount - 1) {}
            Z(0) = InputValue.AddColumn(Matrix.Ones(ExampleCount, 1))
            A(0) = Z(0)

            For i = 1 To LayerCount - 1
                Z(i) = (A(i - 1) * W(i - 1)).AddColumn(Matrix.Ones(ExampleCount, 1))
                A(i) = Relu(Z(i))
            Next

            A(A.Length - 1) = Z(Z.Length - 1)

        End Sub

        Private Sub BackPropagation(
            <Out> ByRef delta As Matrix(), <Out> ByRef error_ As Matrix(),
            output As Matrix, OutputValue As Matrix, Zlast As Matrix,
            W As Matrix(), Z As Matrix(), A As Matrix(), LayerCount%)

            error_ = New Matrix(LayerCount - 1) {}
            error_(LayerCount - 1) = output - OutputValue
            delta = New Matrix(LayerCount - 1) {}
            delta(LayerCount - 1) = error_(LayerCount - 1) * sigmoid(Zlast, True)

            For i = LayerCount - 2 To 0 Step -1
                error_(i) = delta(i + 1) * W(i).T
                delta(i) = error_(i) * sigmoid(Z(i), True)
                delta(i) = delta(i).Slice(0, 1, delta(i).x, delta(i).y)
            Next

        End Sub

        Private Sub GradientDescend(ByRef W As Matrix(), A As Matrix(),
            delta As Matrix(), LearningRate#)

            For i = 0 To W.Length - 1
                W(i) -= A(i).T * delta(i + 1) * LearningRate
            Next

        End Sub

        Private Function sigmoid(m As Matrix, Optional derivated As Boolean = False) As Matrix

            Dim output As Double(,) = m

            Matrix.MatrixLoop(
                (Sub(i, j)
                     If derivated Then
                         Dim aux = 1 / (1 + Math.Exp(-output(i, j)))
                         output(i, j) = aux * (1 - aux)
                     Else
                         output(i, j) = 1 / (1 + Math.Exp(-output(i, j)))
                     End If
                 End Sub), m.x, m.y)

            Return output

        End Function

        Private Function Relu(m As Matrix, Optional derivated As Boolean = False) As Matrix

            Dim output As Double(,) = m

            Matrix.MatrixLoop(
                (Sub(i, j)
                     If derivated Then
                         output(i, j) = If(output(i, j) > 0, 1, 0)
                     Else
                         output(i, j) = If(output(i, j) > 0, output(i, j), 0)
                     End If
                 End Sub), m.x, m.y)

            Return output

        End Function

    End Module

End Namespace
