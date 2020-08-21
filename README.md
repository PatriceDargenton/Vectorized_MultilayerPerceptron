# Vectorized-MultiLayerPerceptron

Vectorized Multi-Layer Perceptron in VB .NET from C# version https://github.com/HectorPulido/Vectorized-multilayer-neural-network

This is the classical XOR test.

# Versions

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