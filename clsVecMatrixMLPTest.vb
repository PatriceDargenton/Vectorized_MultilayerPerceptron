
Option Infer On

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace VectorizedMatrixMLP

    <TestClass()>
    Public Class clsVecMatrixMLPTest

        Private m_mlp As clsVectorizedMatrixMLP
        Private NeuronCount%()

        <TestInitialize()>
        Public Sub Init()

            m_mlp = New clsVectorizedMatrixMLP
            NeuronCount = New Integer() {2, 2, 1}
            m_mlp.learningRate = 0.1
            m_mlp.input = New Double(,) {
                {1, 0},
                {0, 0},
                {0, 1},
                {1, 1}}

            m_mlp.targetArray = New Single(,) {
                {1},
                {0},
                {1},
                {0}}
            m_mlp.target = m_mlp.targetArray

        End Sub

        <TestMethod()>
        Public Sub VectorizedMatrixMLPXORSigmoid()

            m_mlp.nbIterations = 30000 ' Sigmoid: works
            m_mlp.activFct = New ActivationFunction.SigmoidFunction
            m_mlp.lambdaFct = Function(x#) m_mlp.activFct.Activation(x, gain:=1, center:=0)
            m_mlp.lambdaFctD = Function(x#) m_mlp.activFct.Derivative(x, gain:=1, center:=0)
            m_mlp.activFctIsNonLinear = m_mlp.activFct.IsNonLinear

            m_mlp.InitStruct(NeuronCount, addBiasColumn:=True)

            m_mlp.w(0) = {
                 {-0.5, -0.78, -0.07},
                 {0.54, 0.32, -0.13},
                 {-0.29, 0.89, -0.8}}
            m_mlp.w(1) = {
                 {0.28},
                 {-0.94},
                 {-0.5},
                 {-0.36}}

            m_mlp.Train()

            Dim expectedOutput = New Double(,) {
                {0.97},
                {0.02},
                {0.97},
                {0.02}}

            Dim sOutput = m_mlp.outputMatrix.ToString()
            Dim expectedMatrix As Matrix = expectedOutput ' Double(,) -> Matrix
            Dim sExpectedOutput = expectedMatrix.ToString()
            Assert.AreEqual(sOutput, sExpectedOutput)

            Dim rExpectedLoss# = 0.1
            'Dim targetArray As Single() = m_mlp.target.ToArraySingle
            Dim rLoss! = m_mlp.ComputeAverageError()
            Dim rLossRounded# = Math.Round(rLoss, 2)
            Assert.AreEqual(rExpectedLoss, rLossRounded)

        End Sub

        <TestMethod()>
        Public Sub VectorizedMatrixMLPXORSigmoidWithoutBias()

            m_mlp.nbIterations = 100000 ' Sigmoid: works
            m_mlp.activFct = New ActivationFunction.SigmoidFunction
            m_mlp.lambdaFct = Function(x#) m_mlp.activFct.Activation(x, gain:=1, center:=0)
            m_mlp.lambdaFctD = Function(x#) m_mlp.activFct.Derivative(x, gain:=1, center:=0)
            m_mlp.activFctIsNonLinear = m_mlp.activFct.IsNonLinear

            m_mlp.InitStruct(NeuronCount, addBiasColumn:=False)

            m_mlp.w(0) = {
                {0.38, 0.55},
                {0.24, 0.58}}
            m_mlp.w(1) = {
                {0.2},
                {0.16}}

            m_mlp.Train()

            Dim expectedOutput = New Double(,) {
                {0.93},
                {0.03},
                {0.93},
                {0.09}}

            Dim sOutput = m_mlp.outputMatrix.ToString()
            Dim expectedMatrix As Matrix = expectedOutput ' Double(,) -> Matrix
            Dim sExpectedOutput = expectedMatrix.ToString()
            Assert.AreEqual(sOutput, sExpectedOutput)

            Dim rExpectedLoss# = 0.26
            'Dim targetArray As Single() = m_mlp.target.ToArraySingle
            Dim rLoss! = m_mlp.ComputeAverageError()
            Dim rLossRounded# = Math.Round(rLoss, 2)
            Assert.AreEqual(rExpectedLoss, rLossRounded)

        End Sub

        <TestMethod()>
        Public Sub VectorizedMatrixMLPXORHTangent()

            m_mlp.nbIterations = 5000 ' Hyperbolic tangent: works
            m_mlp.activFct = New ActivationFunction.HyperbolicTangentFunction
            m_mlp.lambdaFct = Function(x#) m_mlp.activFct.Activation(x, gain:=1, center:=0)
            m_mlp.lambdaFctD = Function(x#) m_mlp.activFct.Derivative(x, gain:=1, center:=0)
            m_mlp.activFctIsNonLinear = m_mlp.activFct.IsNonLinear

            m_mlp.InitStruct(NeuronCount, addBiasColumn:=True)

            m_mlp.w(0) = {
                 {-0.5, -0.78, -0.07},
                 {0.54, 0.32, -0.13},
                 {-0.29, 0.89, -0.8}}
            m_mlp.w(1) = {
                 {0.28},
                 {-0.94},
                 {-0.5},
                 {-0.36}}

            m_mlp.Train()

            Dim expectedOutput = New Double(,) {
                {0.99},
                {0.0},
                {0.99},
                {0.0}}

            Dim sOutput = m_mlp.outputMatrix.ToString()
            Dim expectedMatrix As Matrix = expectedOutput ' Double(,) -> Matrix
            Dim sExpectedOutput = expectedMatrix.ToString()
            Assert.AreEqual(sOutput, sExpectedOutput)

            Dim rExpectedLoss# = 0.02
            'Dim targetArray As Single() = m_mlp.target.ToArraySingle
            Dim rLoss! = m_mlp.ComputeAverageError()
            Dim rLossRounded# = Math.Round(rLoss, 2)
            Assert.AreEqual(rExpectedLoss, rLossRounded)

        End Sub

        <TestMethod()>
        Public Sub VectorizedMatrixMLPXORGaussian()

            m_mlp.nbIterations = 400 ' Gaussian: works
            m_mlp.activFct = New ActivationFunction.GaussianFunction
            m_mlp.lambdaFct = Function(x#) m_mlp.activFct.Activation(x, gain:=1, center:=0)
            m_mlp.lambdaFctD = Function(x#) m_mlp.activFct.Derivative(x, gain:=1, center:=0)
            m_mlp.activFctIsNonLinear = m_mlp.activFct.IsNonLinear

            m_mlp.InitStruct(NeuronCount, addBiasColumn:=True)

            m_mlp.w(0) = {
                {-0.5, -0.78, -0.07},
                {0.54, 0.32, -0.13},
                {-0.29, 0.89, -0.8}}
            m_mlp.w(1) = {
                {0.28},
                {-0.94},
                {-0.5},
                {-0.36}}

            m_mlp.Train()

            Dim expectedOutput = New Double(,) {
                {0.99},
                {0.03},
                {0.99},
                {0.03}}

            Dim sOutput = m_mlp.outputMatrix.ToString()
            Dim expectedMatrix As Matrix = expectedOutput ' Double(,) -> Matrix
            Dim sExpectedOutput = expectedMatrix.ToString()
            Assert.AreEqual(sOutput, sExpectedOutput)

            Dim rExpectedLoss# = 0.08
            'Dim targetArray As Single() = m_mlp.target.ToArraySingle
            Dim rLoss! = m_mlp.ComputeAverageError()
            Dim rLossRounded# = Math.Round(rLoss, 2)
            Assert.AreEqual(rExpectedLoss, rLossRounded)

        End Sub

        <TestMethod()>
        Public Sub VectorizedMatrixMLPXORSinus()

            m_mlp.nbIterations = 200 ' Sinus: works
            m_mlp.activFct = New ActivationFunction.SinusFunction
            m_mlp.lambdaFct = Function(x#) m_mlp.activFct.Activation(x, gain:=1, center:=0)
            m_mlp.lambdaFctD = Function(x#) m_mlp.activFct.Derivative(x, gain:=1, center:=0)
            m_mlp.activFctIsNonLinear = m_mlp.activFct.IsNonLinear

            m_mlp.InitStruct(NeuronCount, addBiasColumn:=False)

            m_mlp.w(0) = {
                {0.7, 0.8},
                {0.04, 0.59}}
            m_mlp.w(1) = {
                {0.03},
                {0.66}}

            m_mlp.Train()

            Dim expectedOutput = New Double(,) {
                {0.99},
                {0.0},
                {0.99},
                {0.0}}

            Dim sOutput = m_mlp.outputMatrix.ToString()
            Dim expectedMatrix As Matrix = expectedOutput ' Double(,) -> Matrix
            Dim sExpectedOutput = expectedMatrix.ToString()
            Assert.AreEqual(sOutput, sExpectedOutput)

            Dim rExpectedLoss# = 0.02
            'Dim targetArray As Single() = m_mlp.target.ToArraySingle
            Dim rLoss! = m_mlp.ComputeAverageError()
            Dim rLossRounded# = Math.Round(rLoss, 2)
            Assert.AreEqual(rExpectedLoss, rLossRounded)

        End Sub

        <TestMethod()>
        Public Sub VectorizedMatrixMLPXORArcTangent()

            m_mlp.nbIterations = 500 ' Arc Tangent: works
            m_mlp.activFct = New ActivationFunction.ArcTangentFunction
            m_mlp.lambdaFct = Function(x#) m_mlp.activFct.Activation(x, gain:=1, center:=0)
            m_mlp.lambdaFctD = Function(x#) m_mlp.activFct.Derivative(x, gain:=1, center:=0)
            m_mlp.activFctIsNonLinear = m_mlp.activFct.IsNonLinear

            m_mlp.InitStruct(NeuronCount, addBiasColumn:=True)

            m_mlp.w(0) = {
                {-0.5, -0.78, -0.07},
                {0.54, 0.32, -0.13},
                {-0.29, 0.89, -0.8}}
            m_mlp.w(1) = {
                {0.28},
                {-0.94},
                {-0.5},
                {-0.36}}

            m_mlp.Train()

            Dim expectedOutput = New Double(,) {
                {0.99},
                {0},
                {0.99},
                {0}}

            Dim sOutput = m_mlp.outputMatrix.ToString()
            Dim expectedMatrix As Matrix = expectedOutput ' Double(,) -> Matrix
            Dim sExpectedOutput = expectedMatrix.ToString()
            Assert.AreEqual(sOutput, sExpectedOutput)

            Dim rExpectedLoss# = 0.03
            'Dim targetArray As Single() = m_mlp.target.ToArraySingle
            Dim rLoss! = m_mlp.ComputeAverageError()
            Dim rLossRounded# = Math.Round(rLoss, 2)
            Assert.AreEqual(rExpectedLoss, rLossRounded)

        End Sub

        <TestMethod()>
        Public Sub VectorizedMatrixMLPXORELU()

            m_mlp.nbIterations = 400 ' ELU: works
            m_mlp.activFct = New ActivationFunction.ELUFunction
            m_mlp.lambdaFct = Function(x#) m_mlp.activFct.Activation(x, gain:=0.1, center:=0.4)
            m_mlp.lambdaFctD = Function(x#) m_mlp.activFct.Derivative(x, gain:=0.1, center:=0.4)
            m_mlp.activFctIsNonLinear = m_mlp.activFct.IsNonLinear
            m_mlp.learningRate = 0.07

            m_mlp.InitStruct(NeuronCount, addBiasColumn:=True)

            m_mlp.w(0) = {
                {0.37, 0.08, 0.74},
                {0.59, 0.54, 0.32},
                {0.2, 0.78, 0.01}}
            m_mlp.w(1) = {
                {0.5},
                {0.27},
                {0.4},
                {0.11}}

            m_mlp.Train()

            Dim expectedOutput = New Double(,) {
                {0.99},
                {0.02},
                {0.99},
                {0.01}}

            Dim sOutput = m_mlp.outputMatrix.ToString()
            Dim expectedMatrix As Matrix = expectedOutput ' Double(,) -> Matrix
            Dim sExpectedOutput = expectedMatrix.ToString()
            Assert.AreEqual(sOutput, sExpectedOutput)

            Dim rExpectedLoss# = 0.04
            'Dim targetArray As Single() = m_mlp.target.ToArraySingle
            Dim rLoss! = m_mlp.ComputeAverageError()
            Dim rLossRounded# = Math.Round(rLoss, 2)
            Assert.AreEqual(rExpectedLoss, rLossRounded)

        End Sub

        <TestMethod()>
        Public Sub VectorizedMatrixMLPXORReLUSigmoid()

            m_mlp.nbIterations = 5000 ' ReLUSigmoid: works fine
            m_mlp.activFct = New ActivationFunction.ReLUSigmoidFunction
            m_mlp.lambdaFct = Function(x#) m_mlp.activFct.Activation(x, gain:=1, center:=0)
            m_mlp.lambdaFctD = Function(x#) m_mlp.activFct.Derivative(x, gain:=1, center:=0)
            m_mlp.activFctIsNonLinear = m_mlp.activFct.IsNonLinear

            m_mlp.InitStruct(NeuronCount, addBiasColumn:=True)

            m_mlp.w(0) = {
                {-0.5, -0.78, -0.07},
                {0.54, 0.32, -0.13},
                {-0.29, 0.89, -0.8}}
            m_mlp.w(1) = {
                {0.28},
                {-0.94},
                {-0.5},
                {-0.36}}

            m_mlp.Train()

            Dim expectedOutput = New Double(,) {
                {1.0},
                {0.0},
                {1.0},
                {0.0}}

            Dim sOutput = m_mlp.outputMatrix.ToString()
            Dim expectedMatrix As Matrix = expectedOutput ' Double(,) -> Matrix
            Dim sExpectedOutput = expectedMatrix.ToString()
            Assert.AreEqual(sOutput, sExpectedOutput)

            Dim rExpectedLoss# = 0.01
            'Dim targetArray As Single() = m_mlp.target.ToArraySingle
            Dim rLoss! = m_mlp.ComputeAverageError()
            Dim rLossRounded# = Math.Round(rLoss, 2)
            Assert.AreEqual(rExpectedLoss, rLossRounded)

        End Sub

    End Class

End Namespace