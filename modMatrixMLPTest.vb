
' Patrice Dargenton
' Vectorized-MultiLayerPerceptron
' From https://github.com/HectorPulido/Vectorized-multilayer-neural-network : C# -> VB .NET conversion

Option Infer On ' Lambda function

Imports System.Runtime.InteropServices ' OutAttribute

Namespace VectorizedMatrixMLP

    Module modMatrixVecMLPTest

        Sub Main()
            Console.WriteLine("Vectorized-MultiLayerPerceptron with the classical XOR test.")
            MatrixMLPTest()
            Console.WriteLine("Press a key to quit.")
            Console.ReadKey()
        End Sub

        Public Sub MatrixMLPTest()

            Dim mlp As New clsVectorizedMatrixMLP

            mlp.input = New Double(,) {
                {1, 0},
                {0, 0},
                {0, 1},
                {1, 1}}

            mlp.targetArray = New Single(,) {
                {1},
                {0},
                {1},
                {0}}
            mlp.target = mlp.targetArray

            Dim NeuronCount%()
            NeuronCount = New Integer() {2, 2, 1}
            mlp.InitStruct(NeuronCount, addBiasColumn:=True)

            mlp.activFct = New ActivationFunction.SigmoidFunction ' linear
            mlp.nbIterations = 10000 ' Sigmoid: works

            'mlp.activFct = New ActivationFunction.HyperbolicTangentFunction ' linear
            'mlp.nbIterations = 5000 ' Hyperbolic tangent: works

            'mlp.activFct = New ActivationFunction.GaussianFunction ' linear
            'mlp.nbIterations = 1000 ' Gaussian: works fine

            'mlp.activFct = New ActivationFunction.SinusFunction ' linear
            'mlp.nbIterations = 500 ' Sinus: works fine

            'mlp.activFct = New ActivationFunction.ArcTangentFunction ' linear
            'mlp.nbIterations = 1000 ' Arc tangent: works fine

            'mlp.activFct = New ActivationFunction.ELUFunction  ' linear
            'mlp.nbIterations = 1000 ' ELU: works fine (but only one XOR)

            'mlp.activFct = New ActivationFunction.ReLUFunction ' Non linear
            'mlp.nbIterations = 100000 ' ReLU: Does not work yet, but this next one yes:

            'mlp.activFct = New ActivationFunction.ReLUSigmoidFunction ' linear
            'mlp.nbIterations = 5000 ' ReLUSigmoid: works fine

            mlp.lambdaFct = Function(x#) mlp.activFct.Activation(x, gain:=1, center:=0)
            mlp.lambdaFctD = Function(x#) mlp.activFct.Derivative(x, gain:=1, center:=0)
            mlp.activFctIsNonLinear = mlp.activFct.IsNonLinear

            mlp.LearningRate = 0.1

            mlp.Randomize()

            mlp.Train(PrintOutput:=True)

        End Sub

    End Module

End Namespace
