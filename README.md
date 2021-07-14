# Vectorized-MultiLayerPerceptron

Vectorized Multi-Layer Perceptron in VB .NET from C# version https://github.com/HectorPulido/Vectorized-multilayer-neural-network

This is the classical XOR test, the [Iris flower](https://en.wikipedia.org/wiki/Iris_flower_data_set) test, and the [Sunspots](https://courses.cs.washington.edu/courses/cse599/01wi/admin/Assignments/nn.html) test.

# Versions

15/04/2021 V1.30
- clsMLPGeneric.ShowWeights added
- 3 XOR tests added

01/04/2021 V1.29
- New Sunspots test added

20/03/2021 V1.28
- trainingAlgorithm moved in clsMLPGeneric

20/03/2021 V1.27
- Sunspots tests and matrix size checks fixed

18/03/2021 V1.26
- Console demo: Menu added

30/01/2021 V1.25
- Sunspots dataset added (time series dataset)
- clsMLPGeneric: series array (for example time series)

09/01/2021 V1.24
- clsMLPGeneric.nbHiddenNeurons -> moved to specific classes

09/01/2021 V1.23
- HTangent -> Tanh

08/01/2021 V1.22
- Matrix.ToArraySingle -> ToArrayOfSingle

08/01/2021 V1.21
- Dataset directory

03/01/2021 V1.20
- modMLPTest.TestMLP2XORSigmoid fixed
- modActivation: Sigmoid and Tanh limits fixed
- clsRndExtension: NextDoubleGreaterThanZero added (RProp MLP)
- clsMLPGeneric.ShowWeights added, to compare configurations
- clsMLPGeneric.classificationObjective added (RProp MLP)
- clsMLPGeneric.useNguyenWidrowWeightsInitialization added (RProp MLP)
- clsMLPGeneric.minRandomValue added (RProp MLP)

12/12/2020 V1.19
- clsMLPGeneric.averageError: Single -> Double

06/12/2020 V1.18
- clsMLPGeneric.InitializeStruct function

21/11/2020 V1.17
- Iris flower prediction analog test added
- clsVecMatrix.ComputeErrorOneSample() : clsMLPGeneric's version used
- clsMLPGeneric.GetActivationFunctionType() added with enumActivationFunctionType
- clsMLPGeneric.RoundWeights() added
- clsMLPGeneric.ComputeErrorOneSample(targetArray!(,)) added
- clsMLPGeneric.ComputeAverageErrorOneSample!(targetArray!(,)) added

04/10/2020 V1.16
- Iris flower prediction test added
- Hyperbolic Tangent (Tanh) derivative fixed
- clsMLPGeneric.TestAllSamples: simplified
- clsMLPGeneric.PrintParameters: minimalSuccessTreshold displayed
- clsMLPGeneric.ShowThisIteration: also for last iteration

20/09/2020 V1.15
- Hyperbolic Tangent (Tanh) gain inversion: gain:=-2 -> gain:=2
- clsMLPGeneric.Initialize: weightAdjustment optional
- clsVectorizedMLPGeneric.neuronCount -> clsMLPGeneric, and displayed in PrintParameters()
- Compute success and fails after Train()
- Iris flower test added: https://en.wikipedia.org/wiki/Iris_flower_data_set
- Activation function: gain and center optional
- Learning mode added: VectorialBatch (learn all samples in order as a vector for a batch of iterations)
- PrintWeights added for one XOR tests
- PrintOutput: option force display added
- Refactored code in clsMLPGenericVec: TrainVectorOneIteration(), SetOuput1D()

21/08/2020 V1.14
- ComputeSuccess added
- Tests added: 2 XOR and 3 XOR
- Refactored code in clsMLPGeneric: PrintOutput(iteration%), ComputeError(), ComputeAverageErrorFromLastError(), ComputeAverageError() and TestOneSample(input!(), ByRef ouput!())

04/08/2020 V1.13
- ActivationFunctionForMatrix ->
  ActivationFunctionOptimized
- Sigmoid and Hyperbolic Tangent (Bipolar Sigmoid) activations: optimized also with gain<>1
- Hyperbolic Tangent (Bipolar Sigmoid) activation: input/2
- Matrix class using Math.Net

25/06/2020 V1.12
- Matrix.ToVectorArraySingle() -> ToArraySingle()
- clsMLPGeneric: output Matrix instead of ouput array
- Single Matrix class: 3 times faster

06/06/2020 V1.11
- Source code cleaned
- Weight adjustment
- ComputeAverageError: in generic class
- Tests added for semi-stochastic and stochastic learning mode
- TrainSemiStochastic: fixed

16/05/2020 V1.10
- Faster tests

10/05/2020 V1.09
- Faster tests

10/05/2020 V1.08
- Homogenization of function names
- clsMLPGeneric: PrintParameters: parameters added
- Standard tests

02/05/2020 V1.07
- PrintParameters: activation function name displayed
- Faster tests

17/04/2020 V1.06
- Activation function added: Double threshold
- Print output standardized
- Variable names simplification

12/04/2020 V1.05 OOP paradigm
- Weight initialization: rounded, to reproduce functionnal tests exactly
- Generic code: see this repositery: https://github.com/PatriceDargenton/One-Ring-to-rule-them-all

21/03/2020 V1.04 Functionnal tests (2)
- OneIteration: Optional PrintOutput As Boolean = False
- Matrix: Implicit conversion operator !(,) -> Matrix
- clsVectorizedMatrixMLP: ComputeError, ComputeAverageError
- clsVectorizedMatrixMLP: variable names in camelCase

08/03/2020 V1.03 Functionnal tests
- Functionnal tests
- Class clsVectorizedMatrixMLP

25/01/2020 V1.02 Activation functions
- addBiasColumn: bias column can be disabled optionally
- Matrix.ToString(): Print using a ready to code format
- Matrix.Map(): Apply an activation function to each element of the Matrix
- Activation functions

31/12/2019 V1.01 Initial commit: C# -> VB .NET